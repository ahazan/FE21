using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using System.Collections;
using SEICRY_FE_UYU_9.Objetos;
using System.Xml;
using SEICRY_FE_UYU_9.Certificados.Xml.Transformacion;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.ACKS
{
    class RespuestaConsultaCFE
    {
        //Valores globales de la firma digital
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";

        ManteUdoCertificadoRecibido manteUdoSobreRecibido = new ManteUdoCertificadoRecibido();
        ManteUdoEstadoSobreRecibido manteUdoEstadoSobreRecibido = new ManteUdoEstadoSobreRecibido();

        /// <summary>
        /// Retorna el ACK de estado de los certificados de un sobre recibido
        /// </summary>
        /// <param name="token"></param>
        /// <param name="idRespuesta"></param>
        /// <returns></returns>
        public void ObtenerEstadoCFE(string token, string idRespuesta, string destinatario, string nombreArchivoEntrada)
        {
            string xmlACK;
            string nombreArchivo;

            try
            {
                ArrayList listaCertificados = manteUdoSobreRecibido.ConsultarCFEProcesado(token, idRespuesta);

                if (listaCertificados.Count > 0)
                {
                    foreach (CertificadosRecProcesados certificadoRecProcesado in listaCertificados)
                    {
                        certificadoRecProcesado.MotivosRechazo = manteUdoEstadoSobreRecibido.ConsultarCFEProcesado(certificadoRecProcesado.DocEntry);
                    }

                    xmlACK = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    xmlACK += "<ACKCFE xmlns=\"http://cfe.dgi.gub.uy\" version=\"1.0\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

                    xmlACK += ObtenerTagCaratula(idRespuesta, listaCertificados, nombreArchivoEntrada);
                    xmlACK += ObtenerTagCertificados(listaCertificados);

                    xmlACK += "</ACKCFE>";

                    ObtenerFirmaDigital();

                    nombreArchivo = ObtenerNombre(idRespuesta);
                    ProcTransformacion.GuardarACKCertificadoPrevio(nombreArchivo, xmlACK);
                    ProcTransformacion.FirmarACKCertificado(RUTA_CERTIFICADO, nombreArchivo, CLAVE_CERTIFICADO);

                    ManteUdoCorreos manteUdo = new ManteUdoCorreos();
                    Correo correo = manteUdo.Consultar();

                    if (correo != null)
                    {
                        string[] adjuntos = new string[1];
                        //Se agregan las rutas de los archivos adjuntos
                        adjuntos[0] = RutasCarpetas.RutaCarpetaACKCFEReceptor + nombreArchivo + ".xml";

                        Mail mail = new Mail(destinatario, correo.Cuenta, Mensaje.cACKAsunto,
                                Mensaje.cACKMensaje, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);

                        //Envia correo con acuse de respuesta
                        if (mail.enviarCorreoGmail())
                        {
                            if (!adjuntos[0].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaConsultaCFE/Error: " + ex.ToString());
            }
        }

        /// <summary>
        /// Valida que el archivo contenga la informacion para la consulta del estado de un CFE
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public SobreTransito ValidarContenidoConsulta(string nombreArchivo)
        {
            SobreTransito sobreTransito = null;
            XmlDocument xmlDocumento = new XmlDocument();

            try
            {
                xmlDocumento.Load(RutasCarpetas.RutaCarpetaConsultaEstado + "" + nombreArchivo);
                sobreTransito = new SobreTransito();
                sobreTransito.Token = xmlDocumento.GetElementsByTagName("Token").Item(0).InnerText;
                sobreTransito.IdReceptor = xmlDocumento.GetElementsByTagName("IDReceptor").Item(0).InnerText;
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaConsultaCFE/Error: " + ex.ToString());
            }

            return sobreTransito;
        }

        /// <summary>
        /// Retorna un string con lo datos de la caratula de la respueta
        /// </summary>
        /// <param name="idRespuesta"></param>
        /// <param name="listaCertificados"></param>
        /// <returns></returns>
        private string ObtenerTagCaratula(string idRespuesta, ArrayList listaCertificados, string nombreArchivo)
        {
            string xmlCaratula = "";

            try
            {
                CertificadoRecibido certificadoRecibido = manteUdoSobreRecibido.ConsultarDatosCertificadoRecibido(idRespuesta);
                int cantidadCertificadosSobre = manteUdoSobreRecibido.ConsultarCantidadCertificadosSobre(idRespuesta);

                DateTime dt = DateTime.Now;

                xmlCaratula += "<Caratula>";
                xmlCaratula += "<RUCReceptor>" + certificadoRecibido.RucReceptor + "</RUCReceptor>";
                xmlCaratula += "<RUCEmisor>" + certificadoRecibido.RucEmisor + "</RUCEmisor>";
                xmlCaratula += "<IDRespuesta>" + certificadoRecibido.IdConsecutio + "</IDRespuesta>";
                xmlCaratula += "<NomArch>sob_219999830019_20141210_2.xml</NomArch>";
                xmlCaratula += "<FecHRecibido>" + dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") + "</FecHRecibido>";
                xmlCaratula += "<IDEmisor>" + certificadoRecibido.IdEmisor + "</IDEmisor>";
                xmlCaratula += "<IDReceptor>" + certificadoRecibido.IdConsecutio + "</IDReceptor>";
                xmlCaratula += "<CantenSobre>" + cantidadCertificadosSobre + "</CantenSobre>";
                xmlCaratula += "<CantResponden>" + listaCertificados.Count + "</CantResponden>";
                xmlCaratula += "<CantCFEAceptados>" + ObtenerCantidadCertificados(listaCertificados, EEstado.aceptado) + "</CantCFEAceptados>";
                xmlCaratula += "<CantCFERechazados>" + ObtenerCantidadCertificados(listaCertificados, EEstado.rechazado) + "</CantCFERechazados>";
                xmlCaratula += "<CantCFCAceptados>0</CantCFCAceptados>";
                xmlCaratula += "<CantCFCObservados>0</CantCFCObservados>";
                xmlCaratula += "<CantOtrosRechazados>0</CantOtrosRechazados>";
                xmlCaratula += "<Tmst>" + dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") + "</Tmst>";
                xmlCaratula += "</Caratula>";
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaConsultaCFE/Error: " + ex.ToString());
            }

            return xmlCaratula;
        }

        /// <summary>
        /// Retorna cada uno de los certificados revisados
        /// </summary>
        /// <param name="listaCertificados"></param>
        /// <returns></returns>
        private string ObtenerTagCertificados(ArrayList listaCertificados)
        {
            string xmlCertificados = "";
            int numeroOrdindal = 1;

            try
            {
                foreach (CertificadosRecProcesados certificadoRecProcesado in listaCertificados)
                {
                    xmlCertificados += "<ACKCFE_Det>";
                    xmlCertificados += "<Nro_ordinal>" + numeroOrdindal + "</Nro_ordinal>";
                    xmlCertificados += "<TipoCFE>" + certificadoRecProcesado.TipoCom + "</TipoCFE>";
                    xmlCertificados += "<Serie>" + certificadoRecProcesado.SerieCom + "</Serie>";
                    xmlCertificados += "<NroCFE>" + certificadoRecProcesado.NumCom + "</NroCFE>";
                    xmlCertificados += "<FechaCFE>" + certificadoRecProcesado.FechaEmision + "</FechaCFE>";
                    xmlCertificados += "<TmstCFE>" + certificadoRecProcesado.FechaFirma + "</TmstCFE>";
                    xmlCertificados += "<Estado>" + ObtenerEstadoCertificado(certificadoRecProcesado) + "</Estado>";

                    foreach (EstadoCertificadoRecibido motivoRechazo in certificadoRecProcesado.MotivosRechazo)
                    {
                        xmlCertificados += "<MotivosRechazoCF>";
                        xmlCertificados += "<Motivo>" + motivoRechazo.Motivo + "</Motivo>";
                        xmlCertificados += "<Glosa>" + motivoRechazo.Glosa + "</Glosa>";
                        // xmlCertificados += "<Detalle>" + motivoRechazo.Detalle + "</Detalle>";
                        xmlCertificados += "</MotivosRechazoCF>";
                    }

                    xmlCertificados += "</ACKCFE_Det>";

                    numeroOrdindal++;
                }
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaConsultaCFE/Error: " + ex.ToString());
            }

            return xmlCertificados;
        }

        /// <summary>
        /// Retorna la cantidad de certificados aceptados o rechazados 
        /// </summary>
        /// <param name="listaCertificados"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        private int ObtenerCantidadCertificados(ArrayList listaCertificados, EEstado estado)
        {
            int cantidad = 0;

            try
            {
                if (estado == EEstado.aceptado)
                {
                    foreach (CertificadosRecProcesados certificadoRecProcesado in listaCertificados)
                    {
                        if (certificadoRecProcesado.Aprobado)
                        {
                            cantidad++;
                        }
                    }
                }
                else if (estado == EEstado.rechazado)
                {
                    foreach (CertificadosRecProcesados certificadoRecProcesado in listaCertificados)
                    {
                        if (!certificadoRecProcesado.Aprobado)
                        {
                            cantidad++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaConsultaCFE/Error: " + ex.ToString());
            }

            return cantidad;
        }

        /// <summary>
        /// Retorna el codigo del estado del certificado
        /// </summary>
        /// <param name="certificadoRecProcesado"></param>
        /// <returns></returns>
        private string ObtenerEstadoCertificado(CertificadosRecProcesados certificadoRecProcesado)
        {
            if (certificadoRecProcesado.Aprobado)
            {
                return "AE";
            }
            else if (!certificadoRecProcesado.Aprobado)
            {
                return "BE";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Retorna el nombre del archivo de ACK de certificados
        /// </summary>
        /// <param name="idRespuesta"></param>
        /// <returns></returns>
        private string ObtenerNombre(string idRespuesta)
        {
            string nombreArchivo = "";

            try
            {
                CertificadoRecibido certificadoRecibido = manteUdoSobreRecibido.ConsultarDatosCertificadoRecibido(idRespuesta);
                DateTime fechaActual;
                
                //Obtiene la fecha actual
                fechaActual = DateTime.Now;
                //Formatea fecha de modo: YYYYMMDD
                string fechaFormateada = String.Format("{0:yyyyMMdd}", fechaActual);
                //Firmar el xml
                nombreArchivo = "ME_" + idRespuesta + "_Sob_" + certificadoRecibido.RucReceptor + "_" + fechaFormateada + "_" + certificadoRecibido.IdEmisor;
            }
            catch (Exception)
            {
               // SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaConsultaCFE/Error: " + ex.ToString());
            }

            return nombreArchivo;
        }

        /// <summary>
        /// Metodo para obtener informacion de la firma digital
        /// </summary>
        public void ObtenerFirmaDigital()
        {
            ManteUdoCertificadoDigital manteUdoFirma = new ManteUdoCertificadoDigital();

            Certificado certificado = manteUdoFirma.Consultar();

            RUTA_CERTIFICADO = certificado.RutaCertificado;
            CLAVE_CERTIFICADO = certificado.Clave;
        }

        private enum EEstado
        {
            aceptado = 1,
            rechazado = 2
        }
    }
}
