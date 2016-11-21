using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SEICRY_FE_UYU_9.Certificados.Xml.Transformacion;
using SEICRY_FE_UYU_9.Certificados.Xml.Serializacion;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SEICRY_FE_UYU_9.XML;
using System.IO;
using System.Xml;
using SEICRY_FE_UYU_9.GenerarPDF;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.MejoraTiempo
{
    class AccionesFueraHilo
    {
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";
        public static string URL_ENVIO = "";
        public static string URL_CONSULTAS = "";

        private Sobre sobre;
        private Sobre sobreDgi;
        private ManteUdoCFE manteUdoCfe = new ManteUdoCFE();
        ComunicacionDgi comunicacionDGI = new ComunicacionDgi();
        private List<CFE> listaCertificadosCreados = new List<CFE>();


        public void EjecucionTareas(object pCfe)
        {
            ParameterizedThreadStart inicioParametrizado = new ParameterizedThreadStart(EjecutarTareas);

            try
            {
               
                Thread threadCrearXml = new Thread(inicioParametrizado);
                threadCrearXml.IsBackground = true;
                threadCrearXml.Start(pCfe);
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("CertificadoXML/Error: " + ex.ToString());
            }

            finally
            {

                if (inicioParametrizado != null)
                {

                    GC.SuppressFinalize(inicioParametrizado);
                    //Libera de memoria el objeto factura
                     GC.Collect();
                }
                              
            }
        }

        /// <summary>
        /// Ejecuta el hilo para envio de correo
        /// </summary>
        /// <param name="estadoAdenda"></param>
        public void EjecutarCorreo(object estadoAdenda)
        {
            try
            {
                ParameterizedThreadStart inicioCorreo = new ParameterizedThreadStart(EjecutaCorreo);
                Thread correo = new Thread(inicioCorreo);
                correo.IsBackground = true;
                correo.Start(estadoAdenda);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ejecuta el metodo de envio de correo
        /// </summary>
        /// <param name="parametros"></param>
        private void EjecutaCorreo(object parametros)
        {
            DatosCorreo datosCorreo = parametros as DatosCorreo;

            if (!TipoCorreo(datosCorreo))
            {
                if (Mail.errorCorreo.Equals(""))
                {
                    AdminEventosUI.mostrarMensaje(Mensaje.errConfErroneaCorreo, AdminEventosUI.tipoMensajes.error);
                }
                else
                {
                    AdminEventosUI.mostrarMensaje(Mensaje.err + Mail.errorCorreo, AdminEventosUI.tipoMensajes.error);
                }
            }
        }

        /// <summary>
        /// Ejecuta una serie de tareas
        /// </summary>
        /// <param name="parametros"></param>
        private void EjecutarTareas(object parametros)
        {
            CFE cfe = parametros as CFE;
            CAE cae = AdminEventosUI.caePrueba;
            ObtenerFirmaDigital();
            ObtenerUrlWebService();

            cfe.CodigoSeguridad = CrearCertificado(cfe);
            //cfe.CodigoSeguridad = ObtenerCodigoSeguridad(cfe.TipoCFEInt + "" + cfe.SerieComprobante + "" + cfe.NumeroComprobante);

            #region Proceso_WebService            
            ActualizarCodSeguridad(cfe);
            #endregion Proceso_WebService

            sobreDgi = CrearSobre(cfe, true);
            //Se genera el archivo xml
            ArchivoXml archXml = new ArchivoXml();
            archXml.generarXml(cfe, cae);
            AlmacenarCFECreado(cfe);
            sobre = CrearSobre(cfe, false);

            double topeUI = Convert.ToDouble(ValorUI.valorUI * 10000, System.Globalization.CultureInfo.InvariantCulture);

          //  if (!FrmEstadoContingencia.estadoContingencia.Equals("Y"))
          //  {
                if ((cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ETicketContingencia))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCETicketContingencia))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDETicketContingencia)))
                {
                    if (cfe.TipoModena.Equals("UYU") || cfe.TipoModena.Equals("$"))
                    {                        
                      
                        if (ValorUI.valorUI * 10000 < (cfe.TotalMontoTotal - cfe.TotalIVATasaMinima - cfe.TotalIVATasaBasica))                      

                        {
                            EnviarSobre(sobre, sobreDgi, cfe, cae);
                        }
                    }
                    else
                    {
                        if (ValorUI.valorUI * 10000 < (cfe.TotalMontoTotal - cfe.TotalIVATasaMinima - cfe.TotalIVATasaBasica) * cfe.TipoCambio)                       
                        {
                            EnviarSobre(sobre, sobreDgi, cfe, cae);
                        }
                    }
                }
                else
                {
                    //Enviar sobre a DGI
                    EnviarSobre(sobre, sobreDgi, cfe, cae);
                }
           // }

        }

        /// <summary>
        /// Crea los archivos de xml para los certificados
        /// </summary>
        /// <param name="cfe"></param>
        private string CrearCertificado(CFE cfe)
        {
            string codSeguridad = "";
            
            try
            {
                int i = 0;
                bool estadoAdenda = false;

                if (cfe.Items != null)
                {
                    while (i < cfe.Items.Count)
                    {
                        if (cfe.Items[i].UnidadMedida.Length > 4)
                        {
                            cfe.Items[i].UnidadMedida = "N/A";
                        }
                        i++;
                    }
                }

                //Limpia la lista de certificados creados
                listaCertificadosCreados.Clear();

                //Agregar certificado a lista de certificados creados
                listaCertificadosCreados.Add(cfe);

                String xmlCertificado = ProcSerializacion.CrearXmlCFE(cfe);
                string adenda = ProcTransformacion.ObtenerAdenda(cfe);
                ProcTransformacion.GuardarCertificadoPrevio(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), xmlCertificado);

                if (cfe.InfoReferencia.Count > 0)
                {
                    //Valida que la referencia sea global o especifica
                    if (cfe.InfoReferencia[0].IndicadorReferenciaGlobal == CFEInfoReferencia.ESIndicadorReferenciaGlobal.ReferenciaGlobal)
                    {
                        ProcTransformacion.TransformarCertificado(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.TipoCFE, cfe.TipoDocumentoReceptor, true);
                    }
                    else
                    {
                        ProcTransformacion.TransformarCertificado(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.TipoCFE, cfe.TipoDocumentoReceptor);
                    }
                }
                else
                {
                    ProcTransformacion.TransformarCertificado(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.TipoCFE, cfe.TipoDocumentoReceptor);
                }

                if (!adenda.Equals(""))
                {
                    ProcTransformacion.GenerarCFEAdenda(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), adenda);
                    estadoAdenda = true;
                }

                ProcTransformacion.FirmarCertificado(RUTA_CERTIFICADO, cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), CLAVE_CERTIFICADO, false, ref codSeguridad);

                if (estadoAdenda)
                {
                    ProcTransformacion.FirmarCertificado(RUTA_CERTIFICADO, cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), CLAVE_CERTIFICADO, estadoAdenda, ref codSeguridad);
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
            }
            return codSeguridad;
        }

        /// <summary>
        /// Metodo para obtener el codigo de seguridad
        /// </summary>
        /// <param name="numeroCertificado"></param>
        /// <returns></returns>
        private string ObtenerCodigoSeguridad(string numeroCertificado)
        {
            return ProcTransformacion.ObtenerCodigoSegurdad(numeroCertificado);
        }

        /// <summary>
        /// Metodo para alamcenar los comprobantes creados
        /// </summary>
        /// <param name="cfe"></param>
        private void AlmacenarCFECreado(CFE cfe)
        {
            string MayorUI = "N";


            if ((cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ETicketContingencia))
                     || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCETicketContingencia))
                     || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDETicketContingencia)))
            {
                if (cfe.TipoModena.Equals("UYU") || cfe.TipoModena.Equals("$"))
                {
                    if (ValorUI.valorUI * 10000 < (cfe.TotalMontoNoGravado + cfe.TotalMontoNetoIVATasaMinima + cfe.TotalMontoNetoIVATasaBasica))
                    {
                        MayorUI = "Y"; 
                    }
                }
                else
                {
                    if (ValorUI.valorUI * 10000 < (cfe.TotalMontoNoGravado + cfe.TotalMontoNetoIVATasaMinima + cfe.TotalMontoNetoIVATasaBasica) * cfe.TipoCambio)
                    {
                        MayorUI = "Y";
                    }
                }
            }           

                //Almacena el cfe creado
            manteUdoCfe.Almacenar(cfe, MayorUI);
            
        }

        /// <summary>
        /// Metodo para crear sobre para el comprobante
        /// </summary>
        /// <param name="cfe"></param>
        private Sobre CrearSobre(CFE cfe, bool sobreDgi)
        {
            Sobre sobre = new Sobre(cfe);
            string infoCertificado = "";

            try
            {
                infoCertificado = ProcTransformacion.ObtenerCadenaCertificado();

                if (infoCertificado.Equals(""))
                {
                    sobre = null;
                }
                else
                {
                    if (sobreDgi)
                    {
                        ManteUdoDocumento manteUdoDocumento = new ManteUdoDocumento();
                        string rutConfigurado = manteUdoDocumento.ObtenerRut();
                        //Proceso para DGI
                        if (rutConfigurado != null)
                        {
                            sobre.RucReceptor = rutConfigurado;//214844360018;//219999830019
                        }
                        else
                        {
                            sobre.RucReceptor = "214844360018";//219999830019
                        }
                        sobre.X509Certificate = infoCertificado;
                        sobre.ObtenerCertificadosCreados(listaCertificadosCreados);

                        string xmlSobreDGI = ProcSerializacion.CrearXmlSobre(sobre);

                        ProcTransformacion.GuardarSobrePrevio(sobre.NombrePrev, xmlSobreDGI, true);
                        ProcTransformacion.TransformarSobre(sobre.NombrePrev, sobre.Nombre, sobre.ListaCertificados, "", true);
                    }
                    else
                    {
                        //Proceso para Tercero

                        sobre.X509Certificate = infoCertificado;
                        sobre.ObtenerCertificadosCreados(listaCertificadosCreados);

                        string xmlSobreCliente = ProcSerializacion.CrearXmlSobre(sobre);

                        ProcTransformacion.GuardarSobrePrevio(sobre.NombrePrev, xmlSobreCliente, false);

                        if (!cfe.TextoLibreAdenda.Equals(""))
                        {
                            ProcTransformacion.TransformarSobre(sobre.NombrePrev, sobre.Nombre, sobre.ListaCertificados, cfe.TextoLibreAdenda, false);
                        }
                        else
                        {
                            ProcTransformacion.TransformarSobre(sobre.NombrePrev, sobre.Nombre, sobre.ListaCertificados, "", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
            }

            return sobre;
        }

        /// <summary>
        /// Metodo para obtener informacion de la firma digital
        /// </summary>
        public void ObtenerFirmaDigital()
        {
            ManteUdoCertificadoDigital manteUdoFirma = new ManteUdoCertificadoDigital();

            Certificado certificado = manteUdoFirma.Consultar();

            if (certificado != null)
            {
                RUTA_CERTIFICADO = certificado.RutaCertificado;
                CLAVE_CERTIFICADO = certificado.Clave;
            }
            else
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(Mensaje.warNoConfigFirmaDigital);
            }
        }

        /// <summary>
        /// Realiza el envio de los sobre por medio del web service de DGI.
        /// </summary>
        /// <param name="sobre"></param>
        public void EnviarSobre(Sobre sobre, Sobre sobreDgi, CFE cfe, CAE cae)
        {

            ParametrosJobWsDGI parametrosJobWsDGI = new ParametrosJobWsDGI(RUTA_CERTIFICADO, CLAVE_CERTIFICADO, URL_ENVIO, URL_CONSULTAS, cfe, cae);

            try
            {            
                parametrosJobWsDGI.Sobre = sobre;
                parametrosJobWsDGI.SobreDgi = sobreDgi;

                comunicacionDGI.ConsumirWsEnviarSobre(parametrosJobWsDGI);
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
            }


            finally
            {

                //if (parametrosJobWsDGI != null)
                //{
                //    //Libera de memoria el objeto factura
                //    GC.SuppressFinalize(parametrosJobWsDGI);
                //    GC.Collect();
                //}             

            }
        }

        /// <summary>
        /// Obtiene las direcciones web de los web services de la DGI
        /// </summary>
        public void ObtenerUrlWebService()
        {
            try
            {
                ManteUdoFTP manteUdoFtp = new ManteUdoFTP();

                ConfigFTP configFtp = manteUdoFtp.ConsultarURLWebService();

                if (configFtp != null)
                {
                    URL_ENVIO = configFtp.RepoWebServiceEnvio;
                    URL_CONSULTAS = configFtp.RepoWebServiceConsulta;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ejecuta el hilo de impresion
        /// </summary>
        /// <param name="nombreArchivo"></param>
        public void Imprimir(object nombreArchivo)
        {
            try
            {
                ParameterizedThreadStart inicioImprimir = new ParameterizedThreadStart(AccionImprimir);
                Thread correo = new Thread(inicioImprimir);
                correo.IsBackground = true;
                correo.Start(nombreArchivo);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Envia un documento a imprimir
        /// </summary>
        /// <param name="nombreArchivo"></param>
        private void AccionImprimir(object nombreArchivo)
        {
            Imprimir imprimir = new Imprimir();

            List<string> log;

            if (!imprimir.ImprimirPdf(nombreArchivo, out log))
            {
                //errorImprimir = true;
            }
            System.IO.File.AppendAllLines(RutasCarpetas.RutaCarpetaLogImpresion + "LogImpresion.txt", log);
        }

        /// <summary>
        /// Determina que tipo de cuentas de correo utilizar
        /// </summary>
        /// <returns></returns>
        public bool TipoCorreo(DatosCorreo datosCorreo)
        {
            bool resultado = false;

            string contenido = datosCorreo.NombreEmisor + " - " + Mensaje.pdfContenido;
            string asunto = datosCorreo.NombreEmisor + " " + datosCorreo.TipoCFE + " " + datosCorreo.NombreCompuesto;
            string emisor = datosCorreo.NombreEmisor + "<{0}>";

            try
            {
                //Se obtiene credenciales para el envio del correo
                ManteUdoCorreos manteUdo = new ManteUdoCorreos();
                Correo correo = manteUdo.Consultar();

                if (correo != null)
                {

                    emisor = String.Format(emisor, correo.Cuenta);
                    
                    string[] adjuntos = new string[2];//2

                    //Se agregan las rutas de los archivos adjuntos
                    adjuntos[0] = RutasCarpetas.RutaCarpetaComprobantes + datosCorreo.NombreCompuesto + Mensaje.pdfExt;
                    if (datosCorreo.EstadoAdenda)
                    {
                        adjuntos[1] = CambiarNombre(RutasCarpetas.RutaCarpetaSobresComprobantesAdenda + datosCorreo.NombreCompuesto + ".xml", datosCorreo.NombreCompuesto + ".xml");
                    }
                    else
                    {
                        adjuntos[1] = CambiarNombre(RutasCarpetas.RutaCarpetaSobres + datosCorreo.NombreCompuesto + ".xml", datosCorreo.NombreCompuesto + ".xml");
                    }

                    //0 == Gmail
                    if (correo.Opcion.Equals("0"))
                    {
                        if (!adjuntos[1].Equals(""))
                        {

                            ///Envia correo con una cuenta de gmail
                            /*Mail mail = new Mail(datosCorreo.CorreoReceptor, correo.Cuenta, datosCorreo.NombreCompuesto,
                                 Mensaje.pdfContenido, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);*/
                            Mail mail = new Mail(datosCorreo.CorreoReceptor, emisor, asunto, contenido, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);
                            if (mail.enviarCorreoGmail())
                            {
                                if (!adjuntos[1].Equals(""))
                                {
                                    //Borra el archivo de sobre copiado para enviar en el correo
                                    System.IO.File.Delete(adjuntos[1]);
                                }
                                resultado = true;
                            }

                        }
                    }
                    //1 == Outlook
                    else if (correo.Opcion.Equals("1"))
                    {

                        if (!adjuntos[1].Equals(""))
                        {

                            ///Envia correo con una cuenta de outlook
                            //Mail mail = new Mail(datosCorreo.CorreoReceptor, Mensaje.pdfAsunto, Mensaje.pdfContenido, adjuntos);
                            Mail mail = new Mail(datosCorreo.CorreoReceptor, asunto, contenido, adjuntos);
                            if (mail.enviarOutlook())
                            {
                                if (!adjuntos[1].Equals(""))
                                {
                                    //Borra el archivo de sobre copiado para enviar en el correo
                                    System.IO.File.Delete(adjuntos[1]);
                                }
                                resultado = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Cambia el nombre del sobre para enviarlo por correo
        /// </summary>
        /// <param name="nombreSobre"></param>
        /// <returns></returns>
        private string CambiarNombre(string nombreSobre, string NombreArchivo)
        {
            string resultado = "";
            DateTime fechaActual;

            try
            {

                if (!File.Exists(nombreSobre))
                {
                    FTP ftp = new FTP();

                    if (ftp.descargarArchivos(NombreArchivo, RutasCarpetas.RutaCarpetaSobres, 0))
                    {
                      
                    }

                }            
              

                    //Obtiene la fecha actual
                    fechaActual = DateTime.Now;
                    //Formatea fecha de modo: YYYYMMDD
                    string fechaFormateada = String.Format("{0:yyyyMMdd}", fechaActual);
                    RucIdEmisor rucIdEmisor = ObtenerDatosXml(nombreSobre);

                    //Se crea el nombre del sobre segun formato de DGI
                    //resultado = "SOB_" + rucIdEmisor.RucEmisor + "_" + fechaFormateada + "_" + rucIdEmisor.IdEmisor + ".xml";
                    resultado = "Sob_" + rucIdEmisor.RucEmisor + "_" + fechaFormateada + "_" + rucIdEmisor.IdEmisor + ".xml";

                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        resultado = RutasCarpetas.RutaCarpetaContingenciaSobresTemporales + resultado;
                    }
                    else
                    {
                        resultado = RutasCarpetas.RutaCarpetaSobresTemporales + resultado;
                    }
                    System.IO.File.Copy(nombreSobre, resultado, false);

                
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: CambiarNombre/ " + ex.ToString());
                resultado = "";
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene Ruc y Id del emisor del sobre(Xml)
        /// </summary>
        /// <param name="rutaXml"></param>
        /// <returns></returns>
        private RucIdEmisor ObtenerDatosXml(string rutaXml)
        {
            RucIdEmisor respuesta = new RucIdEmisor();
            XmlDocument documento = new XmlDocument();

            try
            {
                documento.Load(rutaXml);

                //Se obtiene el RucEmisor
                respuesta.RucEmisor = documento.GetElementsByTagName("DGICFE:RUCEmisor").Item(0).InnerText;

                //Se obtiene el IdEmisor
                respuesta.IdEmisor = documento.GetElementsByTagName("DGICFE:Idemisor").Item(0).InnerText;
            }
            catch (Exception)
            {
            }

            return respuesta;
        }

        #region Proceso_WebService
        private void ActualizarCodSeguridad(CFE cfe)
        {
            string tabla = "";
            string consulta = "";

            if (cfe.CodigoSeguridad.Length > 0)
            {
                if ((cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ETicketContingencia))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.EFactura)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.EFacturaContingencia))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDEFactura))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDEFacturaContingencia)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDETicketContingencia))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.EFacturaExportacion)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.EFacturaExportacionContingencia))
                    || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDEFacturaExportacion)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia)))
                {
                    tabla = "OINV";
                }
                else if ((cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCEFactura)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCEFacturaContingencia))
                            || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCETicket)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCETicketContingencia))
                            || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCEFacturaExportacion)) || (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia)))
                {
                    tabla = "ORIN";
                }
                else if (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ERemito) || cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ERemitoContingencia) || cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ERemitoExportacion)
                    || cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.ERemitoExportacionContingencia))
                {
                    tabla = "ODLN";
                }
                else if (cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.EResguardo) || cfe.TipoCFE.Equals(CFE.ESTipoCFECFC.EResguardoContingencia))
                {
                    tabla = DocumentosB1.DocumentoB1.TABLA_RESGUARDO_COMPRA;
                }

                consulta = "update " + tabla + " set U_CodSeguridad = '" + cfe.CodigoSeguridad.Substring(0, 6) + "' where DocEntry = " + cfe.DocumentoSAP;

                Recordset oRecordSet = null;

                try
                {
                    oRecordSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                    oRecordSet.DoQuery(consulta);
                }
                catch (Exception ex)
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("CodSeguridad/Error: " + ex.ToString());
                }
                finally
                {
                    if (oRecordSet != null)
                    {
                        //Libera de memoria el objeto factura
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                        GC.Collect();
                    }
                }
            }
        }
        #endregion Proceso_WebService
    }
}
