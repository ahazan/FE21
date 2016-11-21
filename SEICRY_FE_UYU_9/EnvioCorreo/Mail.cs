using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using System.Threading;
using ImapX;
using System.Text.RegularExpressions;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Metodos_FTP;
using System.Xml;
using Limilabs.Client.IMAP;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.ACKS;

namespace SEICRY_FE_UYU_9
{
    class Mail
    {
        private string destinatario = "";
        private string emisor = "";
        private string asunto = "";
        private string contenido = "";
        private string smtpHost = "";
        private string usuario = "";
        private string pass = "";
        private string[] rutaParam = null;
        private int puerto = 0;
        public volatile bool detenerHilo = false;
        public static string errorCorreo = "";

        RespuestaSobre respuestaSobre = new RespuestaSobre();

        /// <summary>
        /// Constructor para el envio de correos con gmail
        /// </summary>
        /// <param name="destinatario"></param>
        /// <param name="emisor"></param>
        /// <param name="asunto"></param>
        /// <param name="contenido"></param>
        /// <param name="smtpHost"></param>
        /// <param name="usuario"></param>
        /// <param name="pass"></param>
        /// <param name="rutaParam"></param>
        /// <param name="puerto"></param>
        public Mail(string destinatario, string emisor, string asunto
            , string contenido, string smtpHost, string usuario, string pass,
            string[] rutaParam, int puerto)
        {
            this.destinatario = destinatario;
            this.emisor = emisor;
            this.asunto = asunto;
            this.contenido = contenido;
            this.smtpHost = smtpHost;
            this.usuario = usuario;
            this.pass = pass;
            this.rutaParam = rutaParam;
            this.puerto = puerto;
        }

        /// <summary>
        /// Constructor para el envio de correos con outlook
        /// </summary>
        /// <param name="destinatario"></param>
        /// <param name="asunto"></param>
        /// <param name="contenido"></param>
        /// <param name="rutaParam"></param>
        public Mail(string destinatario, string asunto, string contenido, string[] rutaParam)
        {
            this.destinatario = destinatario;
            this.asunto = asunto;
            this.contenido = contenido;
            this.rutaParam = rutaParam;
        }

        public Mail()
        {

        }

        /// <summary>
        /// Metodo para enviar un correo por outlook
        /// </summary>
        /// <returns></returns>
        public bool enviarOutlook()
        {
            bool resultado = false;

            try
            {
                Microsoft.Office.Interop.Outlook.Application app = new
                    Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(
                    Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

                mailItem.Subject = asunto;
                mailItem.To = destinatario;
                mailItem.Body = contenido;

                foreach (string rutaArchivo in rutaParam)
                {
                    mailItem.Attachments.Add(rutaArchivo);
                }
                //Quita el warning de ambiguedad con el metodo send
                ((Microsoft.Office.Interop.Outlook._MailItem)mailItem).Send();
                resultado = true;
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Metodo que descarga los archivos adjuntos de los correos
        /// no leidos de la cuenta de outlook
        /// </summary>
        public void bandejaEntradaOutlook()
        {
            Microsoft.Office.Interop.Outlook.Application app = null;
            Microsoft.Office.Interop.Outlook._NameSpace ns = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder inboxFolder = null;
            Microsoft.Office.Interop.Outlook.MailItem sinLeer;

            int p = 0, t = 0;

            while (!detenerHilo)
            {
                try
                {
                    app = new Microsoft.Office.Interop.Outlook.Application();

                    ns = app.GetNamespace("MAPI");
                    ns.Logon(null, null, false, false);

                    inboxFolder = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.
                        OlDefaultFolders.olFolderInbox);

                    ///Se obtiene la bandeja de entrada de la cuenta de correo
                    Microsoft.Office.Interop.Outlook.Items inboxItems = inboxFolder.Items;

                    t = inboxFolder.UnReadItemCount;

                    ///Se obtiene la bandeja de entrada de correos no leidos
                    inboxItems = inboxItems.Restrict("[unread] = true");

                    while (p < t)
                    {
                        if (p == 0)
                        {
                            ///Se obtiene el primer elemento de la bandeja de entrada
                            sinLeer = inboxItems.GetFirst();
                        }
                        else
                        {
                            ///Se obtiene el elemento siguiente de la bandeja de entrada
                            sinLeer = inboxItems.GetNext();
                        }

                        ///Se obtiene los archivos adjuntos del correo
                        Microsoft.Office.Interop.Outlook.Attachments adjuntos = sinLeer.Attachments;

                        foreach (Microsoft.Office.Interop.Outlook.Attachment archivo in adjuntos)
                        {
                            if (ValidarXmlSobre(archivo.FileName))
                            {
                                ///Se marca el correo como no leido
                                sinLeer.UnRead = false;

                                //Se descargar el archivo adjunto del correo
                                archivo.SaveAsFile(RutasCarpetas.RutaCarpetaBandejaEntrada + archivo.FileName);
                                string desde = sinLeer.Sender.ToString();

                                //Se sube el archivo al servidor FTP
                                FTP ftp = new FTP();
                                ftp.CargarArchivos(archivo.FileName, RutasCarpetas.RutaCarpetaBandejaEntrada, 3);

                                //Se guarda en la tabla de sobres Recibidos

                                respuestaSobre.GenerarXML(RutasCarpetas.RutaCarpetaBandejaEntrada, archivo.FileName, desde);
                            }
                            //Se comprueba que sea un ACK
                            else if (ValidarXmlACKSobre(archivo.FileName))
                            {
                                archivo.SaveAsFile(RutasCarpetas.RutaCarpetaBandejaEntrada);
                                sinLeer.UnRead = false;

                                SobreTransito sobreTransito = ObtenerSobreTransito(archivo.FileName, sinLeer.Sender.ToString());
                                ManteUdoSobreTransito manteSobreTransito = new ManteUdoSobreTransito();
                                manteSobreTransito.Almacenar(sobreTransito);
                            }
                        }
                        p = p + 1;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    ns = null;
                    app = null;
                    inboxFolder = null;

                    Thread.Sleep(60000);
                }
               
            }
        }

        /// <summary>
        /// Metodo para enviar correo con gmail
        /// </summary>
        /// <returns></returns>
        public bool enviarCorreoGmail()
        {
            bool resultado = false;            

            System.Net.Mail.MailMessage correo = null;
            System.Net.Mail.SmtpClient smtp = null;

            try
            {
                if (destinatario.Equals(""))
                {
                    errorCorreo = Mensaje.errConfCuentaCorreo;
                }
                else
                {
                    correo = new System.Net.Mail.MailMessage();

                    //Se ajustan los campos del correo
                    correo.To.Add(destinatario);
                    correo.Subject = asunto;
                    correo.From = new System.Net.Mail.MailAddress(emisor);
                    correo.Body = contenido;

                    //Agregar archivo adjunto
                    Collection<string> MailAttachments = new Collection<string>();
                    if (rutaParam != null)
                    {
                        foreach (string adjunto in rutaParam)
                        {
                            MailAttachments.Add(adjunto);
                        }
                    }

                    foreach (string rutaArchivo in MailAttachments)
                    {
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(rutaArchivo);
                        correo.Attachments.Add(attachment);
                    }

                    //Configuracion de parametros servidor SMTP
                    smtp = new System.Net.Mail.SmtpClient(smtpHost);
                    smtp.EnableSsl = true;
                    smtp.Port = puerto;

                    //Configuracion de credenciales
                    smtp.UseDefaultCredentials = false;

                    NetworkCredential credenciales =
                    new NetworkCredential(usuario, pass);

                    smtp.Credentials = credenciales;

                    smtp.Send(correo);
                    resultado = true;
                }
            }
            catch (ArgumentNullException)
            {
                errorCorreo = Mensaje.errCorreoMensajeNulo;
            }
            catch (SmtpFailedRecipientsException)
            {
                errorCorreo = Mensaje.errCorreoNoEntrega;
            }
            catch (SmtpException ex)
            {
                errorCorreo = Mensaje.errCorreoFalloConexion +" /ex/ "+ ex.ToString();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: EnvioGmail/" + ex.ToString());
                errorCorreo = ex.ToString();
            }
            finally
            {
                //Se liberan los objetos de la memoria
                if (smtp != null)
                {
                    smtp.Dispose();
                }
                if (correo != null)
                {
                    correo.Dispose();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Metodo que obtiene descarga los archivos adjuntos de
        /// los correos no leido y los marca como leido de la 
        /// bandeja de entrada de Gmail
        /// </summary>
        /// <returns></returns>
        public void bandejaEntradaGmail()
        {
            while (!detenerHilo)
            {
                try
                {
                    //Se obtiene informacion de cuenta de correo
                    ManteUdoCorreos manteUdo = new ManteUdoCorreos();
                    Correo correo = manteUdo.Consultar();

                    string clave = correo.Clave, usuario = correo.Cuenta;
                    ImapClient cliente = new ImapClient();

                    //Se conecta con el servidor
                    if (cliente.Connect(Mensaje.cliGmail, true))
                    {
                        //Se loguea con el usuario y la clave
                        cliente.Login(usuario, clave);

                        //Se obtiene la carpeta de bandeja de entrada
                        ImapX.Collections.FolderCollection folder = cliente.Folders;
                        ImapX.Folder buzonEntrada = cliente.Folders.Inbox;

                        //Se obtiene los mensajes no leidos
                        ImapX.Message[] mensajes = buzonEntrada.Search("UNSEEN", ImapX.Enums.MessageFetchMode.Full);

                        //Se recorren la lista de mensajes obtenidos
                        foreach (ImapX.Message mensaje in mensajes)
                        {
                            //Se obtienen los archivos adjuntos
                            foreach (var archivo in mensaje.Attachments)
                            {
                                //Se comprueba que sea un archivo xml
                                if (ValidarXmlSobre(archivo.FileName))
                                {
                                    mensaje.Seen = true;
                                    //Se descargan los archivos adjuntos
                                    archivo.Download();
                                    archivo.Save(RutasCarpetas.RutaCarpetaBandejaEntrada);
                                    string desde = mensaje.From.ToString();

                                    //Se guarda en la tabla de sobres Recibidos
                                    respuestaSobre.GenerarXML(RutasCarpetas.RutaCarpetaBandejaEntrada, archivo.FileName, desde);
                                }
                                //Se comprueba que sea un ACK
                                else if (ValidarXmlACKSobre(archivo.FileName))
                                {
                                    archivo.Download();
                                    archivo.Save(RutasCarpetas.RutaCarpetaBandejaEntrada);
                                    mensaje.Seen = true;

                                    //Obtiene datos del sobre de un ACK xml
                                    SobreTransito sobreTransito = ObtenerSobreTransito(archivo.FileName, mensaje.From.ToString());

                                    //Guarda los datos en la tabla TFEST(Sobre en transito)
                                    ManteUdoSobreTransito manteSobreTransito = new ManteUdoSobreTransito();
                                    manteSobreTransito.Almacenar(sobreTransito);
                                }
                                //else if (ValidarXmlACKCFE(archivo.FileName))
                                //{
                                //    archivo.Save(RutasCarpetas.RutaCarpetaAcuseRecibidoCertificado);
                                //    mensaje.Seen = true;

                                //    RespuestaCertificados respuestaCFE = new RespuestaCertificados();
                                //    respuestaCFE.ProcesarRespuesta(archivo.FileName, CFE.ESTipoReceptor.Receptor);

                                //}
                                //else
                                //{
                                //    archivo.Save(RutasCarpetas.RutaCarpetaConsultaEstado);
                                //    mensaje.Seen = true;

                                //    RespuestaConsultaCFE respuestaConsultaCfe = new RespuestaConsultaCFE();
                                //    SobreTransito sobreTransito = respuestaConsultaCfe.ValidarContenidoConsulta(archivo.FileName);

                                //    if (sobreTransito != null)
                                //    {
                                //        respuestaConsultaCfe.ObtenerEstadoCFE(sobreTransito.Token, sobreTransito.IdReceptor, mensaje.From.ToString(), archivo.FileName);
                                //    }
                                //}                            
                            }
                        }

                        //Se cierra sesion
                        cliente.Logout();
                        //Se desconecta del servidor
                        cliente.Disconnect();
                        //Se libera el objeto
                        cliente.Dispose();
                    }
                }
                catch (Exception)
                {
                    //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Mail/BandejaEntradaGmail/Error: " + ex.ToString());
                }

                finally

                { 
                  Thread.Sleep(30000);
                }
              
            }
        }

        /// <summary>
        /// Metodo para validar que el nombre de un sobre cumple el formato:
        ///             SOB_RucEmisor_AAAAMMDD_IdRespuesta.xml
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public bool ValidarXmlSobre(string nombreArchivo)
        {
            bool salida = false;

            try
            {
                if (Regex.IsMatch(nombreArchivo, Mensaje.expRegSobreNombre) || Regex.IsMatch(nombreArchivo, Mensaje.expRegSobreNombreMinu))
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }

            return salida;
        }

        /// <summary>
        /// Metodo para validar que el nombre de un ACK cumple el formato:
        ///             M_IdEmisor_SOB_RucEmisor_AAAAMMDD_IdRespuesta.xml
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public bool ValidarXmlACKSobre(string nombreArchivo)
        {
            bool salida = false;

            try
            {
                if (Regex.IsMatch(nombreArchivo, Mensaje.expRegACKSobreNombre) || Regex.IsMatch(nombreArchivo, Mensaje.expRegACKSobreNombreMay))
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }

            return salida;
        }

        /// <summary>
        /// Metodo para validar que el nombre de un ACK cumple el formato:
        ///             ME_IdEmisor_Sob_RucEmisor_AAAAMMDD_IdRespuesta.xml
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public bool ValidarXmlACKCFE(string nombreArchivo)
        {
            bool salida = false;

            try
            {
                if (Regex.IsMatch(nombreArchivo, Mensaje.expRegACKCFENombre) || Regex.IsMatch(nombreArchivo, Mensaje.expRegACKCFENombreMay))
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }

            return salida;
        }

        /// <summary>
        /// Metodo para obtener datos de un ACK
        /// </summary>
        /// <param name="nombreACK"></param>
        /// <returns></returns>
        private SobreTransito ObtenerSobreTransito(string nombreACK, string correoReceptor)
        {
            SobreTransito resultado = new SobreTransito();
            string idReceptor = "", token = "";

            try
            {

                XmlDocument datosAck = new XmlDocument();

                datosAck.Load(RutasCarpetas.RutaCarpetaBandejaEntrada + nombreACK);

                idReceptor = ObtenerTag(datosAck, true);
                token = ObtenerTag(datosAck, false);

                resultado.IdReceptor = datosAck.GetElementsByTagName(idReceptor).Item(0).InnerText;
                resultado.NombreSobre = nombreACK;
                resultado.TipoReceptor = SobreTransito.ETipoReceptor.Receptor;
                resultado.Token = datosAck.GetElementsByTagName(token).Item(0).InnerText;
                resultado.CorreoReceptor = correoReceptor;

               
              
            }
            catch (Exception)
            {
               // AdminEventosUI.mostrarMensaje("Mail/ObtenerSobreTransito/Error: " + ex.ToString(), AdminEventosUI.tipoMensajes.error);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el tag que trae el documento
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string ObtenerTag(XmlDocument ack, bool campo)
        {
            string resultado = "";
            List<string> tags = new List<string>();

            try
            {
                if (campo)
                {
                    tags.Add("IDRECEPTOR");
                    tags.Add("idreceptor");
                    tags.Add("IDReceptor");
                    tags.Add("IdReceptor");
                    tags.Add("idReceptor");
                }
                else
                {
                    tags.Add("TOKEN");
                    tags.Add("Token");
                    tags.Add("token");                    
                }

                foreach (string tag in tags)
                {
                    if (ValidarTag(tag, ack))
                    {
                        resultado = tag;
                    }
                }
            }
            catch (Exception)
            {
            }
            
            return resultado;
        }

        /// <summary>
        /// Valida si un tag es parte del xml
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="ack"></param>
        /// <returns></returns>
        private bool ValidarTag(string tag, XmlDocument ack)
        {
            bool validado = false;

            try
            {
                string temp = ack.GetElementsByTagName(tag).Item(0).InnerText;
                validado = true;
            }
            catch (Exception)
            {                
            }

            return validado;
        }
    }
}