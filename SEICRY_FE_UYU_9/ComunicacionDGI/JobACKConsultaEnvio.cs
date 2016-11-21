using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;
using System.Xml;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.ComunicacionDGI
{
    class JobACKConsultaEnvio
    {
        public volatile bool detenerHilo = false;        

        /// <summary>
        /// Inicia el hilo para el envio y generaicon 
        /// </summary>
        public void IniciarProceso()
        {
            Thread envGenAckCorreo = null;

            //Se define el hilo
            envGenAckCorreo = new Thread(enviaAckCorreo);
            envGenAckCorreo.IsBackground = true;
            //Inicia el hilo
            envGenAckCorreo.Start();            
        }

        /// <summary>
        /// Metodo que sube los archivos al ftp
        /// </summary>
        public void enviaAckCorreo()
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
                        GC.SuppressFinalize(actual);
                        GC.Collect();
                    }

                    ManteUdoSobreTransito manteSobreTransito = new ManteUdoSobreTransito();
                    List<SobreTransito> listaACKPendientes = manteSobreTransito.Consultar(SobreTransito.ETipoReceptor.Receptor);
                    
                    foreach(SobreTransito sobreTransito in listaACKPendientes)
                    {
                        //XmlDocument ACKReceptor = new XmlDocument();
                        XmlTextWriter writer = new XmlTextWriter(RutasCarpetas.RutaCarpetaACKSobreReceptor + sobreTransito.NombreSobre + ".xml", Encoding.UTF8);
                        writer.Formatting = Formatting.Indented;

                        //Empieza el documento ACKSOBRE
                        writer.WriteStartDocument();
                            writer.WriteStartElement("ACKReceptor");
                                writer.WriteElementString("Token", sobreTransito.Token);
                                writer.WriteElementString("IDReceptor",sobreTransito.IdReceptor);
                            writer.WriteEndElement();
                        writer.WriteEndDocument();
                        //Envia el contenido al documento
                        writer.Flush();

                        //Cierra el documento
                        writer.Close();

                        tipoCorreo(sobreTransito);
                    }

                }
                catch (Exception)
                {
                }
                finally
                {
                    //Se incrementa el temporizador
                    temporizador++;
                }
                //Se detiene el hilo por 12 horas = ((SxMin * SXHora) * #horas) * 1000 = ((60 * 60) * 12) * 1000 = 43200000
                Thread.Sleep(43200000);
            }
        }

        /// <summary>
        /// Determina que tipo de cuentas de correo utilizar
        /// </summary>
        /// <returns></returns>
        public bool tipoCorreo(SobreTransito sobreTransito)
        {
            bool resultado = false;

            try
            {
                //Se obtiene credenciales para el envio del correo
                ManteUdoCorreos manteUdo = new ManteUdoCorreos();
                Correo correo = manteUdo.Consultar();

                if (correo != null)
                {
                    string[] adjuntos = new string[1];

                    //Se agregan las rutas de los archivos adjuntos
                    adjuntos[0] = RutasCarpetas.RutaCarpetaACKSobreReceptor + sobreTransito.NombreSobre + ".xml";
                    string mensaje = "Datos para Consulta de Estado de CFEs" + Environment.NewLine
                                            + "<ACKReceptor>" + Environment.NewLine
                                            + "    <Token> " + sobreTransito.Token + "</Token>" + Environment.NewLine
                                            + "    <IdReceptor> " + sobreTransito.IdReceptor + "</IdReceptor>" + Environment.NewLine
                                            + "</ACKReceptor>" + Environment.NewLine 
                                            + "Saludos";

                    //0 == Gmail
                    if (correo.Opcion.Equals("0"))
                    {
                        ///Envia correo con una cuenta de gmail
                        Mail mail = new Mail(sobreTransito.CorreoReceptor, correo.Cuenta, Mensaje.ACKAsunto,
                             mensaje, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);
                        if (mail.enviarCorreoGmail())
                        {
                            if (!adjuntos[0].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[0]);
                            }
                            resultado = true;
                        }
                    }
                    //1 == Outlook
                    else if (correo.Opcion.Equals("0"))
                    {

                        ///Envia correo con una cuenta de outlook
                        Mail mail = new Mail(sobreTransito.CorreoReceptor, Mensaje.ACKAsunto , mensaje, adjuntos);
                        if (mail.enviarOutlook())
                        {
                            if (!adjuntos[1].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[0]);
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
    }
}
