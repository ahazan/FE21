using System;
using System.IO ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.XML;
using SEICRY_FE_UYU_9.ZonasCFE;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SAPWSDGI_1;

namespace SEICRY_FE_UYU_9.ComunicacionDGI
{
    class JobEnvioSobre
    {
        private PDFs pdfs;

        //Valores globales de la firma digital
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";

        SAPbouiCOM.Application app = SAPbouiCOM.Framework.Application.SBO_Application;

        #region CONSUMO

        /// <summary>
        /// Inicia el proceso de 
        /// </summary>
        /// <param name="parametros"></param>
        public void Trabajar(object parametros)
        {
            string respuesta = Consumir(parametros);

            if (!respuesta.Equals(""))
            {
                ProcesarRespuesta(respuesta, parametros);
            }
            else
            {
                ManteUdoControlSobres manteUdoControlSobres = new ManteUdoControlSobres();
                ParametrosJobWsDGI parametrosDGI = parametros as ParametrosJobWsDGI;

                ControlSobres controlSobres = new ControlSobres();
                controlSobres.Estado = "Pendiente";
                controlSobres.Serie = parametrosDGI.Cfe.SerieComprobante;
                controlSobres.Numero = parametrosDGI.Cfe.NumeroComprobante.ToString();
                controlSobres.Tipo = parametrosDGI.Cfe.TipoCFEInt.ToString();
                controlSobres.DocumentoSap = parametrosDGI.Cfe.DocumentoSAP;
                controlSobres.UsuarioSap = ProcConexion.Comp.UserName;

                manteUdoControlSobres.Almacenar(controlSobres);
            }
        }

        /// <summary>
        /// Consume el web service
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public string Consumir(object parametros)
        {
            string respuesta = "";          

            WebServiceDGI webServiceDgi = new WebServiceDGI(parametros);
            ParametrosJobWsDGI parametrosJobWsDGI = parametros as ParametrosJobWsDGI;

            XmlDocument xmlDocumento = new XmlDocument();

            try
            {

                if (File.Exists((RutasCarpetas.RutaCarpetaSobresDgi + "\\" + parametrosJobWsDGI.SobreDgi.Nombre + ".xml")))
                      {
                          xmlDocumento.Load(RutasCarpetas.RutaCarpetaSobresDgi + "\\" + parametrosJobWsDGI.SobreDgi.Nombre + ".xml");
                      }
                else if (File.Exists((RutasCarpetas.RutaCarpetaContingenciaSobresDgi + "\\" + parametrosJobWsDGI.SobreDgi.Nombre + ".xml")))
                      {
                        xmlDocumento.Load(RutasCarpetas.RutaCarpetaContingenciaSobresDgi + "\\" + parametrosJobWsDGI.SobreDgi.Nombre + ".xml");
                      }

                 

               
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Envio a DGI " + xmlDocumento.InnerXml);
                respuesta = webServiceDgi.WSDGI.SendWSDGI(xmlDocumento.InnerXml, clsWSDGI.WsMethod.Envelope);
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Respuesta " + respuesta);
                //respuesta = respuesta.Replace("<IDEmisor>0</IDEmisor>", "<IDEmisor>" + xmlDocumento.GetElementsByTagName("DGICFE:Idemisor").Item(0).InnerText + "</IDEmisor>");
                //respuesta = respuesta.Replace("<IDEmisor>0</IDEmisor>", "<IDEmisor>" + xmlDocumento.GetElementsByTagName("Idemisor").Item(0).InnerText + "</IDEmisor>");
            }
            catch (Exception)
            {
                app.MessageBox("No se pudo enviar a DGI por problemas de conexión, recordar reenvio de documento.");
                respuesta = "";
            }

            finally
            {

                if (webServiceDgi != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(webServiceDgi);
                    GC.Collect();
                }


                if (parametrosJobWsDGI != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(parametrosJobWsDGI);
                    GC.Collect();
                }


                if (xmlDocumento != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(xmlDocumento);
                    GC.Collect();
                }
                
            }

            return respuesta;
        }

        #endregion CONSUMO

        #region RESPUESTA

        /// <summary>
        /// Procesa la respuesta del web service de DGI
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <param name="parametros"></param>
        public void ProcesarRespuesta(string xmlRespuesta, object parametros)
        {
            string estadoRespuesta = "", token = "", idReceptor = "";
            ParametrosJobWsDGI parametrosJob = parametros as ParametrosJobWsDGI;

            XmlDocument xmlDocumento = new XmlDocument();
            ManteUdoSobre manteSobre = new ManteUdoSobre();

            try
            {

                xmlDocumento.LoadXml(xmlRespuesta);


                idReceptor = xmlDocumento.GetElementsByTagName("IDReceptor").Item(0).InnerText;
                estadoRespuesta = xmlDocumento.GetElementsByTagName("Estado").Item(0).InnerText;

                if (estadoRespuesta.Equals("AS"))
                {
                    token = xmlDocumento.GetElementsByTagName("Token").Item(0).InnerText;
                    string hora = xmlDocumento.GetElementsByTagName("Fechahora").Item(0).InnerText;

                    SobreTransito sobreTransito = new SobreTransito();
                    sobreTransito.NombreSobre = parametrosJob.Sobre.Nombre;
                    sobreTransito.Token = token;
                    sobreTransito.IdReceptor = idReceptor;
                    sobreTransito.TipoReceptor = SobreTransito.ETipoReceptor.DGI;
                    sobreTransito.Tipo = parametrosJob.Cfe.TipoCFEInt;
                    sobreTransito.Serie = parametrosJob.Cfe.SerieComprobante ;
                    sobreTransito.Numero = parametrosJob.Cfe.NumeroComprobante;
                   
                    ManteUdoSobreTransito manteTransito = new ManteUdoSobreTransito();
                    manteTransito.Almacenar(sobreTransito);

                 
                    //True = AS, no se agrega detalle correspondiente a info de rechazo
                    manteSobre.cargarSobreXml(xmlDocumento, parametrosJob.Sobre.Nombre, true);

                    //Se crea el documento Pdf
                    //CrearPDF(parametrosJob.Cfe, parametrosJob.Cae);                    
                }
                else if (estadoRespuesta.Equals("BS"))
                {
                  
                    //False = BS, se agrega detalle correspondiente a info de rechazo
                    manteSobre.cargarSobreXml(xmlDocumento, parametrosJob.Sobre.Nombre, false);
                }
            }
            catch (Exception)
            {
                app.MessageBox("No se pudo enviar a DGI por problemas de conexión, recordar reenvio de documento.");
                ManteUdoControlSobres manteUdoControlSobres = new ManteUdoControlSobres();
                ParametrosJobWsDGI parametrosDGI = parametros as ParametrosJobWsDGI;

                ControlSobres controlSobres = new ControlSobres();
                controlSobres.Estado = "Pendiente";
                controlSobres.Serie = parametrosDGI.Cfe.SerieComprobante;
                controlSobres.Numero = parametrosDGI.Cfe.NumeroComprobante.ToString();
                controlSobres.Tipo = parametrosDGI.Cfe.TipoCFEInt.ToString();
                controlSobres.DocumentoSap = parametrosDGI.Cfe.DocumentoSAP;
                controlSobres.UsuarioSap = ProcConexion.Comp.UserName;

                manteUdoControlSobres.Almacenar(controlSobres);
                //app.MessageBox("ERROR RESPUESTA: " + ex.ToString());
            }

            finally
            {

                if (xmlDocumento != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(xmlDocumento);
                    GC.Collect();
                }

                if (xmlDocumento != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(xmlDocumento);
                    GC.Collect();
                }

                if (manteSobre != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(manteSobre);
                    GC.Collect();
                }
            }
        }

        #endregion RESPUESTA

        #region CREAR PDF
        
        /// <summary>
        /// Metodo que llama a la generacion de documentos de tipo pdf  
        /// Firmados digitalmente
        /// </summary>
        /// <param name="cfe"></param>
        /// <param name="cae"></param>
        /// <param name="kilosFactura"></param>
        /// <param name="tabla"></param>
        /// <param name="descuentoGeneral"></param>
        public void CrearPDF(CFE cfe, CAE cae, DatosPDF datosPdf, string tabla, List<ResguardoPdf> resguardoPdf, string tablaCabezal)
        {
            try
            {

             

                if (cae != null)
                {
                    RutasCarpetas rutas = new RutasCarpetas();
                    rutas.generarCarpetas();

                    //Llamada a metodo para comprobar si hay datos de firma digital
                    ObtenerFirmaDigital();

                    //Crear instancia de Pdfs
                    pdfs = new PDFs(cfe, cae, datosPdf.KilosFactura);

                    ////Se genera el archivo xml
                    //ArchivoXml archXml = new ArchivoXml();
                    //archXml.generarXml(cfe, cae);

                    //Crear pdf y validar creacion
                    #region Proceso_WebService
                    if (pdfs.CrearPDF(tabla, datosPdf, resguardoPdf, tablaCabezal))
                    //if (pdfs.CrearPDF(tabla, datosPdf, resguardoPdf, tablaCabezal, cfe.OrigenFE)) *** Se comenta porque se hace esto para todos los casos!
                    #endregion Proceso_WebService
                    {
                        if (EventosPagina.errorRutaLogo)
                        {
                            //Se informa al usuario de que la ruta especificada para el logo es incorrecta
                            app.MessageBox(Mensaje.errRutaLogoIncorrecta);
                            EventosPagina.errorRutaLogo = false;
                        }

                        if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.sucFactura, AdminEventosUI.tipoMensajes.exito);
                        }
                        else
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.sucFacturaCorreo, AdminEventosUI.tipoMensajes.exito);
                        }
                    }
                    else
                    {
                        if (!EventosPagina.errorRutaLogo && !PDFs.errorCertificado && !PDFs.errorFirma && !PDFs.errorCorreo && !PDFs.errorImprimir)
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errFalloGenerarPdf, AdminEventosUI.tipoMensajes.error);
                        }
                        if (EventosPagina.errorRutaLogo)
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errRutaLogoIncorrecta, AdminEventosUI.tipoMensajes.error);
                            EventosPagina.errorRutaLogo = false;
                        }

                        if (PDFs.errorCertificado)
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errDatosNoConfigurados, AdminEventosUI.tipoMensajes.error);
                            PDFs.errorCertificado = false;
                        }

                        if (PDFs.errorFirma)
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errFalloFirmaPdf, AdminEventosUI.tipoMensajes.error);
                            PDFs.errorFirma = false;
                        }

                        if (PDFs.errorCorreo)
                        {
                            if (Mail.errorCorreo.Equals(""))
                            {
                                AdminEventosUI.mostrarMensaje(Mensaje.errConfErroneaCorreo, AdminEventosUI.tipoMensajes.error);
                            }
                            else
                            {
                                AdminEventosUI.mostrarMensaje(Mensaje.err + Mail.errorCorreo, AdminEventosUI.tipoMensajes.error);
                            }
                            PDFs.errorCorreo = false;
                        }
                        if (PDFs.errorImprimir)
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errFalloImprimirPdf, AdminEventosUI.tipoMensajes.error);
                            PDFs.errorImprimir = false;
                        }
                        
                    }
                }
                else
                {
                    AdminEventosUI.mostrarMensaje(Mensaje.errConfErroneaCae, AdminEventosUI.tipoMensajes.error);
                }
            }
            catch(Exception ex)
            {
                app.MessageBox("JobEnvioSobre/Error: " + ex.ToString());
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
            else
            {
                app.MessageBox(Mensaje.warNoConfigFirmaDigital);
            }
        }

        #endregion CREAR PDF
    }
}
