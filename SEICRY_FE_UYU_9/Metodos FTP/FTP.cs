using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using System.Threading;
using SAPbouiCOM;
using System.Security;
using SEICRY_FE_UYU_9.Globales;
using System.Text.RegularExpressions;
using System.Xml;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Interfaz;
using SAPbobsCOM;

namespace SEICRY_FE_UYU_9.Metodos_FTP
{
    class FTP
    {
        public static string rutaLocal = "";
        public volatile bool detenerHilo = false;        

        /// <summary>
        /// Metodo para cargar archivos en un servidor FTP
        /// tipoRuta: 
        ///            0 = Sobres /
        ///            1 = Comprobantes /
        ///            2 = Bandeja Entrada /
        ///            3 = Respuestas /
        ///            4 = Reporte Diario /
        ///            5 = ContingenciaSobres /
        ///            6 = ContingenciaComprobantes /
        ///            7 = ContingenciaReportesDiarios /
        ///            8 = CFEs /
        ///            9 = CertificadosAnulados/
        ///            10 = ContingenciaSobresDgi
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public bool CargarArchivos(string nombreArchivo, string rutaArchivo, int tipoRuta)
        {
            string servidorFTP = "", usuarioFTP = "", claveFTP = "", rutaFTP = "";
            bool resultado = false;

            ManteUdoFTP udoFTP = new ManteUdoFTP();

            ConfigFTP config = udoFTP.Consultar();

            if (config != null)
            {
                servidorFTP = config.Servidor;
                usuarioFTP = config.Usuario;
                claveFTP = config.Clave;

                rutaFTP = ObtenerTipoRuta(tipoRuta, config);
                Boolean BorroFile;

                //Valida que la ruta no este vacia
                if (!rutaFTP.Equals(""))
                { 
                    try
                    {
                        rutaFTP = Mensaje.dirFtp + servidorFTP + rutaFTP + nombreArchivo;

                        //Se conecta al servidor ftp
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(rutaFTP);

                        //Se define el metodo a ejecutar = cargaArchivos
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        //Se inicia sesion en el servidor
                        request.Credentials = new NetworkCredential(usuarioFTP, claveFTP);

                        rutaLocal = rutaArchivo;
                        //Se obtiene el contenido del archivo
                        byte[] contenido = File.ReadAllBytes(rutaLocal);

                        request.ContentLength = contenido.Length;

                        //Se escribe el contenido en un stream y es enviado al servidor ftp
                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(contenido, 0, contenido.Length);
                        requestStream.Close();

                        //Se obtiene el formulario activo para mantener la conexion con DIServer
                        Form activo = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
                        //Se libera el objeto de memoria
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(activo);
                        GC.Collect();

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Console.WriteLine(response.StatusDescription);                    
                        response.Close();
                        

                        BorroFile = BorroFiles();

                        if (BorroFile != true)
                        {
                            //Se borra el archivo subido
                            File.Delete(rutaLocal);
                        }
                    
                        resultado = true;
                    }
                    catch(Exception)
                    {
                    }
                }
            }
            return resultado;
        }

        private Boolean BorroFiles()
        {

            Recordset recSet = null;
            string consulta = "", resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select U_FilePcDel from  [@TFECONFTP]";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("U_FilePcDel").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
                
            }


            if (resultado.Equals("Y"))
                {
                    return true;
                }
            else
                {
                    return false;
                }
                                                                  
        }


        /// <summary>
        /// Metodo que sube los archivos al ftp
        /// </summary>
        public void subirBorrarArchivosFTP()
        {
            int temporizador = 0;

            while (!detenerHilo)
            {                
                try
                {
                    //Mantiene activo la UI
                    if (temporizador == 30000)
                    {
                        //Se obtiene formulario activo para mantener conexion con la DIServer
                        Form actual = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
                        //Libera el objeto de memoria
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(actual);
                        GC.Collect();
                    }

                    //Se obtienen los directorios
                    DirectoryInfo directorioComprobantes = new DirectoryInfo(RutasCarpetas.RutaCarpetaComprobantes);
                    DirectoryInfo directorioSobres = new DirectoryInfo(RutasCarpetas.RutaCarpetaSobres);
                    DirectoryInfo directorioBandeja = new DirectoryInfo(RutasCarpetas.RutaCarpetaBandejaEntrada);
                    DirectoryInfo directorioCFEs = new DirectoryInfo(RutasCarpetas.RutaCarpetaCertificadosTemporales);

                    //Se obtienen las listas de archivos de los directorios
                    FileInfo[] comprobantesXML = directorioComprobantes.GetFiles(Mensaje.filXml);
                    FileInfo[] comprobantesPDF = directorioComprobantes.GetFiles(Mensaje.filPdf);
                    FileInfo[] sobresXML = directorioSobres.GetFiles(Mensaje.filXml);
                    FileInfo[] bandejaXML = directorioBandeja.GetFiles(Mensaje.filXml);
                    FileInfo[] cfesXML = directorioCFEs.GetFiles(Mensaje.filXml);

                    FTP ftp = new FTP();

                        foreach (FileInfo comprobante in comprobantesPDF)
                        {
                            if (!Regex.IsMatch(comprobante.Name, Globales.Mensaje.expRegPdfNoFirmado))
                            {
                                //Se suben los archivos al servidor ftp                    
                                ftp.CargarArchivos(comprobante.Name, comprobante.FullName, 1);
                            }
                        }

                        foreach (FileInfo comprobante in comprobantesXML)
                        {
                            if (!Regex.IsMatch(comprobante.Name, Globales.Mensaje.expRegXmlNoFirmado))
                            {
                                //Se suben los archivos al servidor ftp
                                ftp.CargarArchivos(comprobante.Name, comprobante.FullName, 1);
                            }
                        }

                        foreach (FileInfo sobre in sobresXML)
                        {
                            if (!Regex.IsMatch(sobre.Name, Globales.Mensaje.expRegSobreNoFirmado))
                            {
                                //Se suben los archivos al servidor ftp
                                ftp.CargarArchivos(sobre.Name, sobre.FullName, 0);
                            }
                        }
                        foreach (FileInfo adjunto in bandejaXML)
                        {
                            //Se suben los archivos al servidor ftp
                            ftp.CargarArchivos(adjunto.Name, adjunto.FullName, 2);
                        }
                        foreach (FileInfo cfe in cfesXML)
                        {
                            //Se suben los archivos al servidor ftp
                            ftp.CargarArchivos(cfe.Name, cfe.FullName, 8);
                        }
                    }
                catch(Exception)
                {            
                }
                finally{
                    //Se incrementa el temporizador
                    temporizador++;

                    Thread.Sleep(600000);
                }
                //Se detiene el hilo por 10 minutos (600000000 /  1000)
               
            }
        }

        /// <summary>
        /// Metodo para descargar archivos del servidor ftp
        /// tipoRuta: 
        ///            0 = Sobres /
        ///            1 = Comprobantes /
        ///            2 = Bandeja Entrada /
        ///            3 = Respuestas /
        ///            4 = Reporte Diario /
        ///            5 = ContingenciaSobres /
        ///            6 = ContingenciaComprobantes /
        ///            7 = ContingenciaReportesDiarios /
        ///            8 = CFEs /
        ///            9 = CertificadosAnulados /
        ///            10 = Contingencia Sobres Dgi
        /// </summary>
        /// <param name="rutaFTP"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="pRutaLocal"></param>
        /// <returns></returns>
        public bool descargarArchivos(string nombreArchivo, string pRutaLocal, int tipoRuta)
        {
            string servidorFTP = "", usuarioFTP = "", claveFTP = "", rutaFTP = "";
            bool resultado = false;

            ManteUdoFTP udoFTP = new ManteUdoFTP();

            ConfigFTP config = udoFTP.Consultar();

            if (config != null)
            {
                servidorFTP = config.Servidor;
                usuarioFTP = config.Usuario;
                claveFTP = config.Clave;

                //Se valida el tipo de extension(Se manejan rutas distintas para xml y pdf)
                rutaFTP = ObtenerTipoRuta(tipoRuta, config);

                //Valida que la ruta no este vacia
                if (!rutaFTP.Equals(""))
                {
                    try
                    {
                        rutaFTP = Mensaje.dirFtp + servidorFTP + rutaFTP + nombreArchivo;
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(rutaFTP);

                        //Se define el metodo a ejecutar = cargaArchivos
                        request.Method = WebRequestMethods.Ftp.DownloadFile;
                        ////Se inicia sesion en el servidor
                        request.Credentials = new NetworkCredential(usuarioFTP, claveFTP);
                        request.UseBinary = true;
                        ////request.UsePassive = false;

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                        ////Se descarga el archivo
                        Stream responseStream = response.GetResponseStream();
                        FileStream fileStream = new FileStream(pRutaLocal + nombreArchivo, FileMode.Create);


                        byte[] buffer = new byte[2048];
                        int count = responseStream.Read(buffer, 0, buffer.Length);

                        while (count > 0)
                        {
                            fileStream.Write(buffer, 0, count);
                            count = responseStream.Read(buffer, 0, buffer.Length);
                        }

                        fileStream.Close();
                        response.Close();
                                   
                        resultado = true;
                    }
                    catch (Exception)
                    {
                        //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("E/DEsc/FTP: " + ex.ToString());
                    }
                }
            }

            return resultado;
        }        

        /// <summary>
        /// Elimina archivos del ftp
        /// tipoRuta: 
        ///            0 = Sobres /
        ///            1 = Comprobantes /
        ///            2 = Bandeja Entrada /
        ///            3 = Respuestas /
        ///            4 = Reporte Diario /
        ///            5 = ContingenciaSobres /
        ///            6 = ContingenciaComprobantes /
        ///            7 = ContingenciaReportesDiarios /
        ///            8 = CFEs /
        ///            9 = CertificadosAnulados /
        ///            10 = Contingencia Sobres Dgi
        /// </summary>
        /// <param name="tipoRuta"></param>
        public void EliminarFTP(int tipoRuta, string nombreArchivo)
        {
            string servidorFTP = "", usuarioFTP = "", claveFTP = "", rutaFTP = "";
            ManteUdoFTP udoFTP = new ManteUdoFTP();

            try
            {
                ConfigFTP config = udoFTP.Consultar();

                if (config != null)
                {
                    servidorFTP = config.Servidor;
                    usuarioFTP = config.Usuario;
                    claveFTP = config.Clave;

                    //Se valida el tipo de extension(Se manejan rutas distintas para xml y pdf)
                    rutaFTP = ObtenerTipoRuta(tipoRuta, config);

                    rutaFTP = Mensaje.dirFtp + servidorFTP + rutaFTP;

                    // Get the object used to communicate with the server.
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(rutaFTP + nombreArchivo);
                    request.Method = WebRequestMethods.Ftp.DeleteFile;

                    // This example assumes the FTP site uses anonymous logon.
                    request.Credentials = new NetworkCredential(usuarioFTP, claveFTP);

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                }
            }
            catch (Exception)
            {
            }
        }


        //#region COMENTADO

        ///// <summary>
        ///// Metodo para contenido de un directorio FTP
        /////            0 = Sobres /
        /////            1 = Comprobantes /
        /////            2 = Bandeja Entrada /
        /////            3 = Respuestas /
        /////            4 = Reporte Diario /
        /////            5 = ContingenciaSobres /
        /////            6 = ContingenciaComprobantes /
        /////            7 = ContingenciaReportesDiarios /
        /////            8 = CFEs /
        /////            9 = CertificadosAnulados
        ///// </summary>
        ///// <param name="tipoRuta"></param>
        //public void obtenerDirectorioDescargar(int tipoRuta)
        //{
        //    string servidorFTP = "", usuarioFTP = "", claveFTP = "", rutaFTP = "";

        //    List<string> comprobantesPdf = new List<string>();
        //    List<string> comprobantesPdfXml = new List<string>();
        //    List<string> comprobantesXml = new List<string>();
        //    List<string> reportes = new List<string>();
        //    List<string> sobres = new List<string>();

        //    ManteUdoFTP udoFTP = new ManteUdoFTP();

        //    try
        //    {
        //        ConfigFTP config = udoFTP.Consultar(Conexion.ProcConexion.Comp);

        //        if (config != null)
        //        {
        //            //Obtiene datos de la configuracion registrada en la base de datos
        //            servidorFTP = config.Servidor;
        //            usuarioFTP = config.Usuario;
        //            claveFTP = config.Clave;

        //            //Se valida el tipo de extension(Se manejan rutas distintas para xml y pdf)
        //            rutaFTP = ObtenerTipoRuta(tipoRuta, config);

        //            //Se crea la ruta correspondiente al servidor Ftp
        //            rutaFTP = Mensaje.dirFtp + servidorFTP + rutaFTP;

        //            //Obtiene el objeto para comunicarse con el servidor Ftp
        //            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(rutaFTP);
        //            //Se ajusta el metodo para obtener los directorios
        //            request.Method = WebRequestMethods.Ftp.ListDirectory;

        //            // This example assumes the FTP site uses anonymous logon.
        //            request.Credentials = new NetworkCredential(usuarioFTP, claveFTP);

        //            //Se obtiene la respuesta del servidor Ftp
        //            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //            Stream responseStream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(responseStream);

        //            //Mientras no sea el fin del stream
        //            while(!reader.EndOfStream)
        //            {
        //                string temp = reader.ReadLine();

        //                //Valida el formato del nombre del archivo y el tipo de ruta
        //                if (Regex.IsMatch(temp, Mensaje.expRegCfePdf) && tipoRuta == 6)
        //                {
        //                    //Archivos generados al crear el comprobante
        //                    comprobantesPdf.Add(temp);
        //                }
        //                //Valida el formato del nombre del archivo y el tipo de ruta
        //                if (Regex.IsMatch(temp, Mensaje.expRegCfePdfXml) && tipoRuta == 6)
        //                {
        //                    //Es copia de informacion de comprobantePdf
        //                    comprobantesPdfXml.Add(temp);
        //                }
        //                //Valida el formato del nombre del archivo y el tipo de ruta
        //                else if (Regex.IsMatch(temp, Mensaje.expRegCfeXml) && tipoRuta == 6)
        //                {
        //                    //Datos de los Cfes exclusivamente
        //                    comprobantesXml.Add(temp);
        //                }
        //                //Valida el formato del nombre del archivo y el tipo de ruta
        //                else if(Regex.IsMatch(temp, Mensaje.expRegSobre) && tipoRuta == 5)
        //                {
        //                    //Sobres con Cfes incluidos
        //                    sobres.Add(temp);
        //                }
        //                //Valida el formato del nombre del archivo y el tipo de ruta
        //                else if (Regex.IsMatch(temp, Mensaje.expRegReportesDiarios) && tipoRuta == 7)
        //                {
        //                    //Reporte Diarios generados
        //                    reportes.Add(temp);
        //                }
        //            }                    

        //            //Se cierra el streamReader
        //            reader.Close();
        //            //Se cierra la respuesta
        //            response.Close();
                    
        //            foreach (string comprobantePdf in comprobantesPdf)
        //            {
        //                //Descarga el archivo de la zona Contingencia en el servidor Ftp
        //                descargarArchivos(comprobantePdf, RutasCarpetas.RutaCarpetaComprobantes, 6);
        //                //Elimina el archivo de la zona Contingencia en el servidor Ftp
        //                EliminarFTP(6, comprobantePdf);
        //                //Carga el archivo en la zona de Comprobantes en el servidor Ftp
        //                CargarArchivos(comprobantePdf, RutasCarpetas.RutaCarpetaComprobantes + comprobantePdf, 1);
        //            }
        //            foreach (string comprobantePdfXml in comprobantesPdfXml)
        //            {
        //                //Descarga el archivo de la zona Contingencia en el servidor Ftp
        //                descargarArchivos(comprobantePdfXml, RutasCarpetas.RutaCarpetaComprobantes, 6);
        //                //Elimina el archivo de la zona Contingencia en el servidor Ftp
        //                EliminarFTP(6, comprobantePdfXml);
        //                //Carga el archivo en la zona de Comprobantes en el servidor Ftp
        //                CargarArchivos(comprobantePdfXml, RutasCarpetas.RutaCarpetaComprobantes + comprobantePdfXml, 1);
        //            }
        //            foreach (string comprobanteXml in comprobantesXml)
        //            {
        //                //Descarga el archivo de la zona Contingencia en el servidor Ftp
        //                descargarArchivos(comprobanteXml, RutasCarpetas.RutaCarpetaCertificadosTemporales, 6);
        //                //Elimina el archivo de la zona Contingencia en el servidor Ftp
        //                EliminarFTP(6, comprobanteXml);
        //                //Carga el archivo en la zona de Comprobantes en el servidor Ftp
        //                cargarComprobanteCFEBD(comprobanteXml, RutasCarpetas.RutaCarpetaCertificadosTemporales);
                        
        //            }
        //            foreach (string sobre in sobres)
        //            {
        //                //Descarga el archivo de la zona Contingencia en el servidor Ftp
        //                descargarArchivos(sobre, RutasCarpetas.RutaCarpetaSobres, 5);
        //                //Elimina el archivo de la zona Contingencia en el servidor Ftp
        //                EliminarFTP(5, sobre);
        //                //Carga el archivo en la zona de Comprobantes en el servidor Ftp
        //                CargarArchivos(sobre, RutasCarpetas.RutaCarpetaSobres + sobre, 0);
        //            }
        //            foreach (string reporte in reportes)
        //            {
        //                //Descarga el archivo de la zona Contingencia en el servidor Ftp
        //                descargarArchivos(reporte, RutasCarpetas.RutaCarpetaReporteDiario, 7);
        //                //Elimina el archivo de la zona Contingencia en el servidor Ftp
        //                EliminarFTP(7, reporte);
        //                //Carga el archivo en la zona de Comprobantes en el servidor Ftp
        //                CargarArchivos(reporte, RutasCarpetas.RutaCarpetaReporteDiario + reporte, 4);
        //            }
        //        }
        //    }
        //    catch(Exception)
        //    {
        //    }
        //}

        //#endregion COMENTADO

        /// <summary>
        /// Devuelve el tipo de ruta
        /// </summary>
        /// <param name="tipoRuta"></param>
        /// <returns></returns>
        private string ObtenerTipoRuta(int tipoRuta, ConfigFTP config)
        {
            string resultado = "";

            if (tipoRuta == 0)
            {
                resultado = config.RepoSob;
            }
            else if (tipoRuta == 1)
            {
                resultado = config.RepoComp;
            }
            else if (tipoRuta == 2)
            {
                resultado = config.RepoBandejaEntrada;
            }
            else if (tipoRuta == 3)
            {
                resultado = config.RepoResp;
            }
            else if (tipoRuta == 4)
            {
                resultado = config.RepoRepDi;
            }
            else if (tipoRuta == 5)
            {
                resultado = config.RepoContingenciaSobres;
            }
            else if (tipoRuta == 6)
            {
                resultado = config.RepoContingenciaComprobantes;
            }
            else if (tipoRuta == 7)
            {
                resultado = config.RepoContingenciaReportesDiarios;
            }
            else if (tipoRuta == 8)
            {
                resultado = config.RepoCFEs;
            }
            else if (tipoRuta == 9)
            {
                resultado = config.RepoCertificadosAnulados;
            }
            else if (tipoRuta == 10)
            {
                resultado = config.RepoContingenciaSobreDgi;
            }

           

            return resultado;
        }

        /// <summary>
        ///// Carga a la base de datos un comprobante desde un archivo xml
        ///// </summary>
        ///// <param name="nombreComprobante"></param>
        ///// <param name="rutaComprobante"></param>
        //private void cargarComprobanteCFEBD(string nombreComprobante, string rutaComprobante)
        //{
        //    XmlDocument comprobante = new XmlDocument();

        //    try
        //    {
        //        comprobante.Load(rutaComprobante + nombreComprobante);

        //        ManteUdoCFE manteCFE = new ManteUdoCFE();
        //        CFE cfe = new CFE();

        //        string tipoDoc = comprobante.GetElementsByTagName("ns1:TipoCFE").Item(0).InnerText;
                
        //        cfe.TipoCFE = obtenerTipoCFE(Convert.ToInt16(tipoDoc));
        //        cfe.SerieComprobante = comprobante.GetElementsByTagName("ns1:Serie").Item(0).InnerText;
        //        cfe.NumeroComprobante = int.Parse(comprobante.GetElementsByTagName("ns1:Nro").Item(0).InnerText);
        //        cfe.FechaHoraFirma = comprobante.GetElementsByTagName("ns1:FchEmis").Item(0).InnerText;
                
        //        cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
        //        cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;              
        //        cfe.DocumentoSAP = manteCFE.ConsultarDocumentoSap(tipoDoc, cfe.SerieComprobante, cfe.NumeroComprobante.ToString());

        //        manteCFE.Almacenar(cfe);
        //    }
        //    catch(Exception)
        //    {            
        //    }
        //}

        /// <summary>
        /// Obtiene el enum para tipo de CFE
        /// </summary>
        /// <param name="tipoCFE"></param>
        /// <returns></returns>
        private CFE.ESTipoCFECFC obtenerTipoCFE(int tipoCFE)
        {

            if (tipoCFE == 111)
            {
                return CFE.ESTipoCFECFC.EFactura;
            }
            else if (tipoCFE == 211)
            {
                return CFE.ESTipoCFECFC.EFacturaContingencia;
            }
            else if (tipoCFE == 181)
            {
                return CFE.ESTipoCFECFC.ERemito;
            }
            else if (tipoCFE == 281)
            {
                return CFE.ESTipoCFECFC.ERemitoContingencia;
            }
            else if (tipoCFE == 182)
            {
                return CFE.ESTipoCFECFC.EResguardo;
            }
            else if (tipoCFE == 282)
            {
                return CFE.ESTipoCFECFC.EResguardoContingencia;
            }
            else if (tipoCFE == 101)
            {
                return CFE.ESTipoCFECFC.ETicket;
            }
            else if (tipoCFE == 201)
            {
                return CFE.ESTipoCFECFC.ETicketContingencia;
            }
            else if (tipoCFE == 112)
            {
                return CFE.ESTipoCFECFC.NCEFactura;
            }
            else if (tipoCFE == 212)
            {
                return CFE.ESTipoCFECFC.NCEFacturaContingencia;
            }
            else if (tipoCFE == 102)
            {
                return CFE.ESTipoCFECFC.NCETicket;
            }
            else if (tipoCFE == 202)
            {
                return CFE.ESTipoCFECFC.NCETicketContingencia;
            }
            else if (tipoCFE == 113)
            {
                return CFE.ESTipoCFECFC.NDEFactura;
            }
            else if (tipoCFE == 213)
            {
                return CFE.ESTipoCFECFC.NDEFacturaContingencia;
            }
            else if (tipoCFE == 103)
            {
                return CFE.ESTipoCFECFC.NDETicket;
            }
            else if (tipoCFE == 203)
            {
                return CFE.ESTipoCFECFC.NDETicketContingencia;
            }
            else if (tipoCFE == 121)
            {
                return CFE.ESTipoCFECFC.EFacturaExportacion;
            }
            else if (tipoCFE == 122)
            {
                return CFE.ESTipoCFECFC.NCEFacturaExportacion;
            }
            else if (tipoCFE == 123)
            {
                return CFE.ESTipoCFECFC.NDEFacturaExportacion;
            }
            else if (tipoCFE == 124)
            {
                return CFE.ESTipoCFECFC.ERemitoExportacion;
            }
            else if (tipoCFE == 221)
            {
                return CFE.ESTipoCFECFC.EFacturaExportacionContingencia;
            }
            else if (tipoCFE == 222)
            {
                return CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia;
            }
            else if (tipoCFE == 223)
            {
                return CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia;
            }
            else if (tipoCFE == 224)
            {
                return CFE.ESTipoCFECFC.ERemitoExportacionContingencia;
            }

            //Cumple con el retorno para el metodo
            return CFE.ESTipoCFECFC.EFactura;
        }

    }
}
