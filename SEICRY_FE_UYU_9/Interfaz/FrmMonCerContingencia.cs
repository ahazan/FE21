using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Conexion;
using SAPbobsCOM;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SEICRY_FE_UYU_9.Globales;
using System.Threading;
using SEICRY_FE_UYU_9.Metodos_FTP;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmMonCerContingencia : FrmBase
    {
        //Valores globales de la firma digital
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";

        //Valores globales para las direcciones de los web services
        public static string URL_ENVIO = "";
        public static string URL_CONSULTAS = "";

        public static bool dgiContingencia = false;

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Ajusta el Formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            Formulario.Freeze(true);

            BloquearCeldas();
            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Item.Enabled = false;
            }
            else
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Item.Enabled = true;
            }

            Formulario.Freeze(false);
        }

        /// <summary>
        /// Estable DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Bloquea las celdas del grid
        /// </summary>
        /// <param name="comp"></param>
        private void BloquearCeldas()
        {
            int j = 0;
            Grid grdCerCon = (Grid)Formulario.Items.Item("grdCerCon").Specific;

            foreach (GridColumn columna in grdCerCon.Columns)
            {
                grdCerCon.Columns.Item(j).Editable = false;
                j++;
            }
        }

        /// <summary>
        /// Obtener lista de sobres en la carpeta de contingencia
        /// </summary>
        /// <param name="idGrid"></param>
        /// <returns></returns>
        public bool EnviarSobreDGI(string idGrid)
        {
            ManteUdoMonCerContingencia manteContingencia = new ManteUdoMonCerContingencia();
            List<Sobre> listaSobres = new List<Sobre>();
            FTP ftp = new FTP();
            bool resultado = false;

            dgiContingencia = true;

            try
            {
                //Obtiene la firma digital
                ObtenerFirmaDigital();
                //Obtiene la url del web service de consulta
                ObtenerUrlWebService();

                //Obtiene el grid del formulario
                Grid grdCerCon = (Grid)Formulario.Items.Item(idGrid).Specific;
                int fila = 0, filaSeleccionada = 0;
                string nombreTemp = "", tipoDocumento = "", serie = "", numeroDocumento = "", docEntry = "";

                //Recorre las filas para determinar la fila seleccionada
                while (fila < grdCerCon.Rows.Count)
                {
                    //Se obtiene la fila seleccionada del grid
                    if (grdCerCon.Rows.IsSelected(fila))
                    {
                        //Asigna la fila seleccionada
                        filaSeleccionada = fila;
                        fila = grdCerCon.Rows.Count + 1;
                    }
                    fila++;
                }
                //Obtiene el nombre compuesto de las columnas del grid
                tipoDocumento = grdCerCon.DataTable.Columns.Item("Tipo de Documento").Cells.Item(filaSeleccionada).Value.ToString();
                serie = grdCerCon.DataTable.Columns.Item("Serie").Cells.Item(filaSeleccionada).Value.ToString();
                numeroDocumento = grdCerCon.DataTable.Columns.Item("Número de Documento").Cells.Item(filaSeleccionada).Value.ToString();
                nombreTemp = tipoDocumento + serie + numeroDocumento;               

                Sobre sobre = new Sobre();
                //Obtiene los datos del sobre xml qhe baja del Ftp
                sobre = obtenerDatosSobre(sobre, nombreTemp);
                listaSobres.Add(sobre);               

                //Metodo encargado de buscar el correo receptor y enviar por correo sobre.xml y comprobante.pdf
                EnviarCorreo(tipoDocumento, serie, numeroDocumento);

                //Envia el sobre a DGI
                EnviarSobre(sobre);
                //Obtiene el DocEntry
                docEntry = manteContingencia.ObtenerDocEntry(tipoDocumento, serie, numeroDocumento);
                manteContingencia.CopiarCFE(docEntry);
                //Se elimina de la tabla TFECERCON
                manteContingencia.Eliminar(docEntry);

                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                dgiContingencia = false;
            }

            return resultado;
        }

        /// <summary>
        /// Realiza el envio de los sobre por medio del web service de DGI.
        /// </summary>
        /// <param name="sobre"></param>
        public void EnviarSobre(Sobre sobre)
        {
            ParametrosJobWsDGI parametrosJobWsDGI = new ParametrosJobWsDGI(RUTA_CERTIFICADO, CLAVE_CERTIFICADO, URL_ENVIO, URL_CONSULTAS, null, null);
            parametrosJobWsDGI.Sobre = sobre;
            parametrosJobWsDGI.SobreDgi = sobre;

            //Procesos web service
            ComunicacionDgi comunicacionDGI = new ComunicacionDgi();
            comunicacionDGI.ConsumirWsEnviarSobre(parametrosJobWsDGI);
        }

        /// <summary>
        /// Obtiene las direcciones web de los web services de la DGI
        /// </summary>
        public void ObtenerUrlWebService()
        {
            ManteUdoFTP manteUdoFtp = new ManteUdoFTP();

            ConfigFTP configFtp = manteUdoFtp.ConsultarURLWebService();

            if (configFtp != null)
            {
                URL_ENVIO = configFtp.RepoWebServiceEnvio;
                URL_CONSULTAS = configFtp.RepoWebServiceConsulta;
            }
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
        }

        /// <summary>
        /// Obtiene los datos del sobre de un xml
        /// </summary>
        /// <param name="sobre"></param>
        /// <returns></returns>
        private Sobre obtenerDatosSobre(Sobre sobre, string nombreSobre)
        {
            string nombreExt = nombreSobre + ".xml";

            try
            {
                FTP ftp = new FTP();
                //Se descarga el archivo del servidor ftp
                //ftp.descargarArchivos(nombreExt, RutasCarpetas.RutaCarpetaSobresTemporales, 5);
                //ftp.descargarArchivos(nombreExt, RutasCarpetas.RutaCarpetaSobresDgi, 10);
                //ftp.descargarArchivos(nombreExt, RutasCarpetas.RutaCarpetaComprobantes, 6);
                //Se borra del ftp
                //ftp.EliminarFTP(5, nombreExt);
                //ftp.EliminarFTP(10, nombreExt);
                //ftp.EliminarFTP(6, nombreExt);

                XmlDocument documento = new XmlDocument();
                documento.Load(RutasCarpetas.RutaCarpetaContingenciaSobresDgi + nombreExt);
                sobre.Nombre = nombreSobre;
            }
            catch (Exception)
            {
                //Se muestra mensaje de error
                AdminEventosUI.mostrarMensaje(Mensaje.errNoDescargaSobres + nombreExt, AdminEventosUI.tipoMensajes.error);
            }

            return sobre;
        }

        /// <summary>
        /// Envia comprobante y sobre al correo receptor
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="serie"></param>
        /// <param name="numeroDocumento"></param>
        public void EnviarCorreo(string tipoDocumento, string serie, string numeroDocumento)
        {
            ManteUdoMonCerContingencia manteContingencia = new ManteUdoMonCerContingencia();
            string correoReceptor = manteContingencia.ObtenerCorreoReceptor(tipoDocumento, serie, numeroDocumento);

            if (!correoReceptor.Equals(""))
            {
                tipoCorreo(tipoDocumento + serie + numeroDocumento, correoReceptor);
            }
        }

        /// <summary>
        /// Determina que tipo de cuentas de correo utilizar
        /// </summary>
        /// <returns></returns>
        public bool tipoCorreo(string nombreCompuesto, string correoReceptor)
        {
            bool resultado = false;

            try
            {
                //Se obtiene credenciales para el envio del correo
                ManteUdoCorreos manteUdo = new ManteUdoCorreos();
                Correo correo = manteUdo.Consultar();

                if (correo != null)
                {
                    string[] adjuntos = new string[2];

                    PDFs pdf = new PDFs();

                    //FTP ftp = new FTP();

                    //ftp.descargarArchivos(nombreCompuesto + Mensaje.pdfExt, RutasCarpetas.RutaCarpetaAdjuntos, 6);
                    //ftp.EliminarFTP(6, nombreCompuesto + Mensaje.pdfExt);
                    
                    //Se agregan las rutas de los archivos adjuntos
                    adjuntos[0] = RutasCarpetas.RutaCarpetaContingenciaComprobantes + nombreCompuesto + Mensaje.pdfExt;
                    if (ValidarAdenda(nombreCompuesto + ".xml"))
                    {
                        adjuntos[1] = RutasCarpetas.RutaCarpetaContingenciaSobresAdenda + nombreCompuesto + ".xml";
                        System.IO.File.Copy(RutasCarpetas.RutaCarpetaContingenciaSobresAdenda + nombreCompuesto + ".xml", RutasCarpetas.RutaCarpetaSobresComprobantesAdenda + nombreCompuesto + ".xml", true);
                        System.IO.File.Copy(RutasCarpetas.RutaCarpetaContingenciaSobres + nombreCompuesto + ".xml", RutasCarpetas.RutaCarpetaSobres + nombreCompuesto + ".xml", true);
                    }
                    else
                    {
                        adjuntos[1] = RutasCarpetas.RutaCarpetaContingenciaSobres + nombreCompuesto + ".xml";
                        System.IO.File.Copy(RutasCarpetas.RutaCarpetaContingenciaSobres + nombreCompuesto + ".xml", RutasCarpetas.RutaCarpetaSobres + nombreCompuesto + ".xml", true);
                    }

                    System.IO.File.Copy(RutasCarpetas.RutaCarpetaContingenciaSobresDgi + nombreCompuesto + ".xml", RutasCarpetas.RutaCarpetaSobresDgi + nombreCompuesto + ".xml", true);
                    System.IO.File.Delete(RutasCarpetas.RutaCarpetaContingenciaSobresDgi + nombreCompuesto + ".xml");
                    //adjuntos[1] = pdf.cambiarNombre(RutasCarpetas.RutaCarpetaSobresTemporales + nombreCompuesto + ".xml");

                    //System.IO.File.Copy(RutasCarpetas.RutaCarpetaContingenciaComprobantes + nombreCompuesto + Mensaje.pdfExt, RutasCarpetas.RutaCarpetaComprobantes + nombreCompuesto + Mensaje.pdfExt, true);                    

                    //0 == Gmail
                    if (correo.Opcion.Equals("0"))
                    {
                        ///Envia correo con una cuenta de gmail
                        Mail mail = new Mail(correoReceptor, correo.Cuenta, nombreCompuesto,
                             Mensaje.pdfContenido, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);
                        if (mail.enviarCorreoGmail())
                        {
                            if (!adjuntos[0].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[0]);
                            }
                            if (!adjuntos[1].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[1]);
                            }
                            resultado = true;
                        }
                    }
                    //1 == Outlook
                    else if (correo.Opcion.Equals("1"))
                    {
                        ///Envia correo con una cuenta de outlook
                        Mail mail = new Mail(correoReceptor, Mensaje.pdfAsunto, Mensaje.pdfContenido, adjuntos);
                        if (mail.enviarOutlook())
                        {
                            if (!adjuntos[0].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[1]);
                            }
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
            catch (Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Comprueba si el sobre tiene adenda
        /// </summary>
        /// <param name="sobreComprobar"></param>
        /// <returns></returns>
        private bool ValidarAdenda(string sobreComprobar)
        {
            bool resultado = false;

            try
            {
                XmlDocument sobreAdenda = new XmlDocument();
                sobreAdenda.Load(RutasCarpetas.RutaCarpetaContingenciaSobresAdenda + sobreComprobar);
                resultado = true;
            }
            catch(Exception)
            {            
            }

            return resultado;
        }
    }
}
