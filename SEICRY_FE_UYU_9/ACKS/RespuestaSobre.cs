using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.XSD;
using SEICRY_FE_UYU_9.Udos;
using System.Xml;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SEICRY_FE_UYU_9.Firma_Digital;

namespace SEICRY_FE_UYU_9.ACKS
{
    class RespuestaSobre
    {
        /// <summary>
        /// Metodo para cargar sobres con facturas desde un archivo xml a la tabla TFESOBFAC y TFESOBFACDET 
        /// </summary>
        /// <param name="rutaRespuesta"></param>
        public void GenerarXML(string rutaSobre, string nombreSobre, string correoEmisor)
        {
            string contenido = "", tag = "", tag2 = "DGICFE:", tag3 = "cfe:";
            bool dgiActivado = false, tag3Activado = false;
            ValidarSobre validaSobre = new ValidarSobre();
            ValidarCFE validaCfe = new ValidarCFE();
            ManteUdoConsecutivo manteUdoConsecutivo = new ManteUdoConsecutivo();
            ManteUdoCertificadoRecibido manteUdoSobreRecibido = new ManteUdoCertificadoRecibido();

            try
            {
                XmlDocument documento = new XmlDocument();
                List<CertificadoRecibido> listaCertificadosRecibidos = new List<CertificadoRecibido>();
                CertificadoRecibido certificadoRecibido;

                documento.Load(rutaSobre + nombreSobre);

                //Obtiene el contenido del xml en string
                contenido = documento.InnerXml;

                //Se obtiene el nodo princicpal
                XmlNodeList sobre = documento.GetElementsByTagName("DGICFE:EnvioCFE");

                if (sobre.Count == 0)
                {
                    sobre = documento.GetElementsByTagName("EnvioCFE_entreEmpresas");                    
                }

                if (sobre.Count == 0)
                {
                    sobre = documento.GetElementsByTagName("DGICFE:EnvioCFE_entreEmpresas");
                    dgiActivado = true;
                }

                if (sobre.Count == 0)
                {
                    sobre = documento.GetElementsByTagName("cfe:EnvioCFE_entreEmpresas");
                    dgiActivado = true;
                    tag3Activado = true;
                }

                //Se valida el tipo de tag
                if (ValidarNSTag(documento))
                {
                    tag = "ns0:";
                }
                
                XmlNodeList caratula = documento.GetElementsByTagName("Caratula");

                if (caratula.Count == 0)
                {
                    caratula = documento.GetElementsByTagName(tag2 + "Caratula");
                }

                if (caratula.Count == 0)
                {
                    caratula = documento.GetElementsByTagName(tag3 + "Caratula");
                }

                foreach (XmlElement nodoCaratula in caratula)
                {
                    certificadoRecibido = new CertificadoRecibido();

                    if (!dgiActivado)
                    {
                        tag2 = "";
                    }

                    
                    if (tag3Activado)
                    {
                        tag2 = tag3;
                        tag = "";
                    }

                    //Se obtienen los datos de la caratula
                    certificadoRecibido.RucReceptor = nodoCaratula.GetElementsByTagName(tag2 + "RutReceptor")[0].InnerText;
                    certificadoRecibido.RucEmisor = nodoCaratula.GetElementsByTagName(tag2 + "RUCEmisor")[0].InnerText;
                    certificadoRecibido.IdEmisor = nodoCaratula.GetElementsByTagName(tag2 + "Idemisor")[0].InnerText;
                    certificadoRecibido.FechaSobre = nodoCaratula.GetElementsByTagName(tag2 + "Fecha")[0].InnerText;
                    certificadoRecibido.CantCFE = nodoCaratula.GetElementsByTagName(tag2 + "CantCFE")[0].InnerText;

                    XmlNodeList listaCfes = documento.GetElementsByTagName(tag + "CFE");
                    foreach (XmlElement nodoCfe in listaCfes)
                    {                        
                        certificadoRecibido.FechaFirma = nodoCfe.GetElementsByTagName(tag + "TmstFirma")[0].InnerText;
                        certificadoRecibido.TipoCFE = nodoCfe.GetElementsByTagName(tag + "TipoCFE")[0].InnerText;
                        certificadoRecibido.SerieComprobante = nodoCfe.GetElementsByTagName(tag + "Serie")[0].InnerText;
                        certificadoRecibido.NumeroComprobante = nodoCfe.GetElementsByTagName(tag + "Nro")[0].InnerText;
                        certificadoRecibido.FechaEmision = nodoCfe.GetElementsByTagName(tag + "FchEmis")[0].InnerText;
                        certificadoRecibido.RazonSocial = nodoCfe.GetElementsByTagName(tag + "RznSoc")[0].InnerText;

                        //Datos del CAE
                        certificadoRecibido.DNroCAE = nodoCfe.GetElementsByTagName(tag + "DNro")[0].InnerText;
                        certificadoRecibido.HNroCAE = nodoCfe.GetElementsByTagName(tag + "HNro")[0].InnerText;
                        if (!certificadoRecibido.TipoCFE.Equals("182"))
                        {
                            if (!certificadoRecibido.TipoCFE.Equals("282"))
                            {
                                certificadoRecibido.FVenCAE = nodoCfe.GetElementsByTagName(tag + "FecVenc")[0].InnerText;
                            }
                        }

                        //Se inicializa una nueva lista para el detalle de la factura del sobre
                        certificadoRecibido.DetalleCertificadoRecibido = new List<DetCertificadoRecibido>();

                        string tipoMoneda = "";
                        foreach (XmlElement totales in nodoCfe.GetElementsByTagName(tag + "Totales"))
                        {
                            tipoMoneda = totales.GetElementsByTagName(tag + "TpoMoneda")[0].InnerText;
                        }

                        foreach (XmlElement detalle in nodoCfe.GetElementsByTagName(tag + "Detalle"))
                        {
                            //Se crea un elemento de detalle de factura
                            DetCertificadoRecibido detalleCertificadoRecibido;

                            foreach (XmlElement item in detalle.GetElementsByTagName(tag + "Item"))
                            {
                                detalleCertificadoRecibido = new DetCertificadoRecibido();
                                detalleCertificadoRecibido.NumeroComprobante = certificadoRecibido.NumeroComprobante;
                                detalleCertificadoRecibido.SerieComprobante = certificadoRecibido.SerieComprobante;
                                detalleCertificadoRecibido.TipoCFE = certificadoRecibido.TipoCFE;
                                detalleCertificadoRecibido.PrecioUnitario = item.GetElementsByTagName(tag + "PrecioUnitario")[0].InnerText;
                                detalleCertificadoRecibido.MontoItem = item.GetElementsByTagName(tag +"MontoItem")[0].InnerText;
                                detalleCertificadoRecibido.NombreItem = item.GetElementsByTagName(tag +"NomItem")[0].InnerText;
                                string cantidad = item.GetElementsByTagName(tag + "Cantidad")[0].InnerText;
                                cantidad = cantidad.Replace(".",",");
                                detalleCertificadoRecibido.Cantidad = double.Parse(cantidad);
                                detalleCertificadoRecibido.TipoMoneda = tipoMoneda;
                                
                                //Se agregan los detalles al sobre
                                certificadoRecibido.DetalleCertificadoRecibido.Add(detalleCertificadoRecibido);                                
                            }
                        }
                    }
                    listaCertificadosRecibidos.Add(certificadoRecibido);
                }

                ValidarSobre valida = new ValidarSobre();
                List<ErrorValidarSobre> listaErrores = new List<ErrorValidarSobre>();
                bool estadoSobre = true;

                if (!valida.validarRucSobreContraComprobante(documento, listaErrores, tag))
                {
                    estadoSobre = false;
                }

                if (!valida.validarCantidadCfe(documento, listaErrores, tag))
                {
                    estadoSobre = false;
                }
                
                //Valida el sobre con el XSD
                if (!valida.ValidarXsdSobre(contenido))
                {
                    estadoSobre = false;
                    foreach (string rechazo in ValidarSobre.erroresEncontrados)
                    {
                        ErrorValidarSobre error = new ErrorValidarSobre();
                        error.CodigoRechazo = Mensaje.errSobValXsdCod;
                        error.DetalleRechazo = Mensaje.errSobValXsdDet;
                        error.GlosaRechazo = rechazo;
                        listaErrores.Add(error);
                    }
                }               

                if (estadoSobre)
                {                    
                    //Sobre validado                                       
                    
                    foreach (CertificadoRecibido certRecibido in listaCertificadosRecibidos)
                    {
                        certRecibido.IdConsecutio = "";

                        //Se insertan los datos en la tabla de la BD
                        manteUdoSobreRecibido.AlmacenarMaestro(certRecibido, correoEmisor, nombreSobre);
                    }
                }
                else
                {             
                    //Sobre rechazado
                    if (listaCertificadosRecibidos.Count > 0)
                    {
                        GenerarACK("BS", listaCertificadosRecibidos[0], correoEmisor, listaErrores, nombreSobre);
                    }
                }
            }

            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaSobre/GenerarXml/Error:" + ex.ToString());
            }
        }

        /// <summary>
        /// Valida si se usa el tag ns0 o ns1
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        private bool ValidarNSTag(XmlDocument documento)
        {
            bool resultado = false;
            string temp = "";

            try
            {
                temp = documento.GetElementsByTagName("ns0:DNro")[0].InnerText;
                resultado = true;
            }
            catch (Exception)
            {
            }

            return resultado;
        }


        /// <summary>
        /// Metodo que genera el Acuse de Recibo
        /// </summary>
        public string GenerarACK(string estado, CertificadoRecibido sobreRecibido, string correoEmisor, List<ErrorValidarSobre> listaErrores, string nombreSobre)
        {
            ValidarSobre validaSobre = new ValidarSobre();

            //Se crea objeto de tipo Consecutivo
            Consecutivo consecutivo = new Consecutivo();
            XmlTextWriter writer = null;
            string nombreACK = "", nombreACKF = "";
            DateTime fechaActual;

            try
            {
                //Genera el idReceptor para el ACK
                consecutivo.IdReceptor = validaSobre.generarIdReceptor();
                //Genera el TokenBinario para el ACK
                consecutivo.Token = validaSobre.generarTokenBinario(consecutivo.IdReceptor);

                ManteUdoConsecutivo manteConsecutivo = new ManteUdoConsecutivo();
                //Inserta en la tabla TFECONS el nuevo registro de consecutivo
                manteConsecutivo.Almacenar(consecutivo);

                //Obtiene la fecha actual
                fechaActual = DateTime.Now;
                //Formatea fecha de modo: YYYYMMDD
                string fechaFormateada = String.Format("{0:yyyyMMdd}", fechaActual);

                //Nombre del ACK segun formato de DGI
                nombreACK = "M_" + consecutivo.IdReceptor + "_Sob_" + sobreRecibido.RucEmisor + "_" + fechaFormateada + "_" + sobreRecibido.IdEmisor + "sf" + ".xml";
                //Nombre del ACK firmado digitalmente
                nombreACKF = "M_" + consecutivo.IdReceptor + "_Sob_" + sobreRecibido.RucEmisor + "_" + fechaFormateada + "_" + sobreRecibido.IdEmisor + ".xml";

                writer = new XmlTextWriter(RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACK, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;

                //Empieza el documento ACKSOBRE
                writer.WriteStartDocument();

                //Agrega el primer elemento y el namespace
                writer.WriteStartElement("ACKSobre");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteAttributeString("version", "1.0");
                writer.WriteAttributeString("xmlns", "http://cfe.dgi.gub.uy");

                //Empieza el Tag de Caratula 
                writer.WriteStartElement("Caratula");
                writer.WriteElementString("RUCReceptor", sobreRecibido.RucReceptor);
                writer.WriteElementString("RUCEmisor", sobreRecibido.RucEmisor);
                writer.WriteElementString("IDRespuesta", consecutivo.IdReceptor);
                writer.WriteElementString("NomArch", nombreSobre.ToLower());
                writer.WriteElementString("FecHRecibido", sobreRecibido.FechaSobre);
                writer.WriteElementString("IDEmisor", sobreRecibido.IdEmisor);
                writer.WriteElementString("IDReceptor", consecutivo.IdReceptor);
                writer.WriteElementString("CantidadCFE", sobreRecibido.CantCFE);
                writer.WriteElementString("Tmst", sobreRecibido.FechaEmision);

                //Termina el Tag de Caratula
                writer.WriteEndElement();

                //Empieza el Tag de Detalle
                writer.WriteStartElement("Detalle");
                writer.WriteElementString("Estado", estado);
                //Caso sobre rechazado
                if (estado.Equals("BS"))
                {
                    //Empieza el Tag de MotivosRechazo
                    writer.WriteStartElement("MotivosRechazo");
                    foreach (ErrorValidarSobre error in listaErrores)
                    {
                        writer.WriteElementString("Motivo", error.CodigoRechazo);
                        writer.WriteElementString("Glosa", error.GlosaRechazo);
                    }
                    //Termina el Tag de MotivosRechazo
                    writer.WriteEndElement();
                }
                //Caso sobre Aceptado
                else if (estado.Equals("AS"))
                {
                    //Empieza el Tag de ParamConsulta
                    writer.WriteStartElement("ParamConsulta");
                    writer.WriteElementString("Token", consecutivo.Token);

                    fechaFormateada = String.Format("{0:yyyy-MM-dd H:mm:ss}", fechaActual);

                    writer.WriteElementString("Fechahora", fechaFormateada);

                    //Termina el Tag de ParamConsulta
                    writer.WriteEndElement();
                }
                //Termina el Tag de Detalle
                writer.WriteEndElement();

                //Termina el Tag de ACKSobre
                writer.WriteEndElement();

                //Termina el documento SOBREACK
                writer.WriteEndDocument();

                //Envia el contenido al documento
                writer.Flush();

                //Cierra el documento
                writer.Close();

                FirmaDigital firma = new FirmaDigital();

                //Procede a intentar firmar el xml del ACK
                if (firma.FirmarACK(RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACK,
                    RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACKF))
                {
                    if (!nombreACK.Equals(""))
                    {
                        //Borra el ACK generado
                        System.IO.File.Delete(RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACK);

                        //Instancia para obtener datos de cuenta de correo electronico
                        ManteUdoCorreos mante = new ManteUdoCorreos();
                        Correo correo = mante.Consultar();

                        string[] adjuntos = new string[1];
                        adjuntos[0] = RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACKF;

                        if (correo != null)
                        {
                            ///Envia correo con una cuenta de gmail
                            Mail mail = new Mail(correoEmisor, correo.Cuenta, Mensaje.cACKAsunto,
                                 Mensaje.cACKMensaje, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);
                            mail.enviarCorreoGmail();
                        }
                    }
                }
                else
                {
                    //Borra el ACK firmado
                    System.IO.File.Delete(RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACK);
                }
            }
            catch (Exception)
            {
                if (!nombreACK.Equals(""))
                {
                    //Borra el ACK generado
                    System.IO.File.Delete(RutasCarpetas.RutaCarpetaAcuseRecibidoSobre + nombreACK);
                }

                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RespuestaSobre/Error: " + ex.ToString());
            }
            finally
            {
                //Cierre de documento
                writer.Close();
            }

            return consecutivo.IdReceptor;
        }
    }
}
