using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml.Xsl;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Diagnostics;
using System.Xml;
using System.Security.Cryptography.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.Metodos_FTP;

namespace SEICRY_FE_UYU_9.Certificados.Xml.Transformacion
{
    /// <summary>
    /// Realiza los procesos de firma y validacion de CFE's
    /// </summary>
    public static class ProcTransformacion
    {
        public static string RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaCertificadosTemporales;
        public static string RUTA_SOBRES = RutasCarpetas.RutaCarpetaSobres;
        public static string RUTA_REPORTES = RutasCarpetas.RutaCarpetaReporteDiario;

        #region GUARDAR PREVIO

        /// <summary>
        /// Guarda la version previa de un certificado 
        /// </summary>
        /// <param name="numeroCertificado"></param>
        /// <param name="xmlCertificdo"></param>
        /// <returns></returns>
        public static bool GuardarCertificadoPrevio(int tipo, string serrie, string numeroCertificado, string xmlCertificdo)
        {
            try
            {
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaContingenciaComprobantes;
                }
                else
                {
                    RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaCertificadosTemporales;
                }

                //Almacena el archivo xml previo para el certificado final.
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlCertificdo);
                xmlDocumento.Save(RUTA_CERTIFICADOS + "\\" + tipo + "" + serrie + "" + numeroCertificado + "_prev.xml");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Guarda la version previa de un sobre
        /// </summary>
        /// <param name="nombreSobrePrevio"></param>
        /// <param name="xmlSobre"></param>
        /// <returns></returns>
        public static bool GuardarSobrePrevio(string nombreSobrePrevio, string xmlSobre, bool sobreDgi)
        {
            try
            {
                if (sobreDgi)
                {
                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaContingenciaSobresDgi;
                    }
                    else
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaSobresDgi;
                    }
                }
                else
                {
                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaContingenciaSobres;
                    }
                    else
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaSobres;
                    }
                }


                //Almacena el archivo xml previo para el certificado final.
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlSobre);
                xmlDocumento.Save(RUTA_SOBRES + "\\" + nombreSobrePrevio + ".xml");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Guarda la version previa de un reporte diario
        /// </summary>
        /// <param name="nombreRptdPrevio"></param>
        /// <param name="xmlRptd"></param>
        /// <returns></returns>
        public static bool GuardarRptdPrevio(string nombreRptdPrevio, string xmlRptd)
        {
            try
            {
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    RUTA_REPORTES = RutasCarpetas.RutaCarpetaContingenciaReporteDiario;
                }
                else
                {
                    RUTA_REPORTES = RutasCarpetas.RutaCarpetaReporteDiario;
                }

                //Almacena el archivo xml previo para el certificado final.
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlRptd);
                xmlDocumento.Save(RUTA_REPORTES + "\\" + nombreRptdPrevio + "_nf.xml");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Guarda la version previa de un ACK de Certificado 
        /// </summary>
        /// <param name="numeroCertificado"></param>
        /// <param name="xmlCertificdo"></param>
        /// <returns></returns>
        public static bool GuardarACKCertificadoPrevio(string nombreACK, string xmlCertificdo)
        {
            try
            {
                //Almacena el archivo xml previo para el certificado final.
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlCertificdo);
                xmlDocumento.Save(RutasCarpetas.RutaCarpetaACKCFEReceptor + "\\" + nombreACK + "_prev.xml");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region TRANSFORMAR XSLT

        /// <summary>
        /// Realiza la transformacion del certificado previo al certificado no firmado apliacando el formato oficial de la DGI
        /// </summary>
        /// <param name="numeroCertificado"></param>
        /// <returns></returns>
        public static bool TransformarCertificado(int tipo, string serie, string numeroCertificado, CFE.ESTipoCFECFC tipoCFE, CFE.ESTipoDocumentoReceptor tipoDocReceptor, bool referenciaGlobal = false)
        {
            try
            {
                XslCompiledTransform proc = new XslCompiledTransform();

                //Carga el estilo del certificado dependiendo del tipo del certificado procesado
                switch (tipoCFE)
                {
                    case CFE.ESTipoCFECFC.ETicket:

                        if (tipoDocReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocReceptor == CFE.ESTipoDocumentoReceptor.CI)
                        {
                            proc.Load(@"Certificados\Xml\xslt\ETicket.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\ETicketExtranjero.xslt");
                        }

                        break;

                    case CFE.ESTipoCFECFC.ETicketContingencia:

                        if (tipoDocReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocReceptor == CFE.ESTipoDocumentoReceptor.CI)
                        {
                            proc.Load(@"Certificados\Xml\xslt\ETicket.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\ETicketExtranjero.xslt");
                        }

                        break;

                    case CFE.ESTipoCFECFC.NCETicket:

                        if (tipoDocReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocReceptor == CFE.ESTipoDocumentoReceptor.CI)
                        {
                            if (referenciaGlobal)
                            {
                                proc.Load(@"Certificados\Xml\xslt\NCETicketRefGlobal.xslt");
                            }
                            else
                            {
                                proc.Load(@"Certificados\Xml\xslt\NCETicket.xslt");
                            }
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCETicketExtranjero.xslt");
                        }

                        break;

                    case CFE.ESTipoCFECFC.NCETicketContingencia:

                        if (tipoDocReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocReceptor == CFE.ESTipoDocumentoReceptor.CI)
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCETicket.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCETicketExtranjero.xslt");
                        }

                        break;

                    case CFE.ESTipoCFECFC.NDETicket:

                        if (tipoDocReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocReceptor == CFE.ESTipoDocumentoReceptor.CI)
                        {
                            if (referenciaGlobal)
                            {
                                proc.Load(@"Certificados\Xml\xslt\NDETicketRefGlobal.xslt");
                            }
                            else
                            {
                                proc.Load(@"Certificados\Xml\xslt\NDETicket.xslt");
                            }
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDETicketExtranjero.xslt");
                        }

                        break;

                    case CFE.ESTipoCFECFC.NDETicketContingencia:

                        if (tipoDocReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocReceptor == CFE.ESTipoDocumentoReceptor.CI)
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDETicket.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDETicketExtranjero.xslt");
                        }

                        break;

                    case CFE.ESTipoCFECFC.EFactura:

                        proc.Load(@"Certificados\Xml\xslt\EFactura.xslt");
                        
                        break;

                    case CFE.ESTipoCFECFC.EFacturaContingencia:

                        proc.Load(@"Certificados\Xml\xslt\EFactura.xslt");
                       
                        break;

                    case CFE.ESTipoCFECFC.NCEFactura:

                        if (referenciaGlobal)
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCEFacturaRefGlobal.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCEFactura.xslt");
                        }
                        
                        break;

                    case CFE.ESTipoCFECFC.NCEFacturaContingencia:

                        proc.Load(@"Certificados\Xml\xslt\NCEFactura.xslt");
                        
                        break;

                    case CFE.ESTipoCFECFC.NDEFactura:

                        if (referenciaGlobal)
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDEFacturaRefGlobal.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDEFactura.xslt");
                        }
                        
                        break;

                    case CFE.ESTipoCFECFC.NDEFacturaContingencia:

                        proc.Load(@"Certificados\Xml\xslt\NDEFactura.xslt");

                        break;

                    case CFE.ESTipoCFECFC.ERemito:

                        proc.Load(@"Certificados\Xml\xslt\ERemito.xslt");

                        break;

                    case CFE.ESTipoCFECFC.ERemitoContingencia:

                        proc.Load(@"Certificados\Xml\xslt\ERemito.xslt");

                        break;

                    case CFE.ESTipoCFECFC.EResguardo:

                        proc.Load(@"Certificados\Xml\xslt\EResguardo.xslt");

                        break;

                    case CFE.ESTipoCFECFC.EResguardoContingencia:

                        proc.Load(@"Certificados\Xml\xslt\EResguardo.xslt");

                        break;

                    case CFE.ESTipoCFECFC.EFacturaExportacion:

                        proc.Load(@"Certificados\Xml\xslt\EFacturaExportacion.xslt");

                        break;

                    case CFE.ESTipoCFECFC.EFacturaExportacionContingencia:

                        proc.Load(@"Certificados\Xml\xslt\EFacturaExportacion.xslt");

                        break;

                    case CFE.ESTipoCFECFC.NCEFacturaExportacion:

                        #region FE_EXPORTACION
                        //proc.Load(@"Certificados\Xml\xslt\NCEFacturaExportacion.xslt");
                        if (referenciaGlobal)
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCEFacturaExportacionRefGlobal.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NCEFacturaExportacion.xslt");
                        }
                        #endregion FE_EXPORTACION

                        break;

                    case CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia:

                        proc.Load(@"Certificados\Xml\xslt\NCEFacturaExportacion.xslt");

                        break;

                    case CFE.ESTipoCFECFC.NDEFacturaExportacion:

                        #region FE_EXPORTACION
                        //proc.Load(@"Certificados\Xml\xslt\NDEFacturaExportacion.xslt");
                        if (referenciaGlobal)
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDEFacturaExportacionRefGlobal.xslt");
                        }
                        else
                        {
                            proc.Load(@"Certificados\Xml\xslt\NDEFacturaExportacion.xslt");
                        }
                        #endregion FE_EXPORTACION

                        break;

                    case CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia:

                        proc.Load(@"Certificados\Xml\xslt\NDEFacturaExportacion.xslt");

                        break;

                    case CFE.ESTipoCFECFC.ERemitoExportacion:

                        proc.Load(@"Certificados\Xml\xslt\ERemitoExportacion.xslt");

                        break;

                    case CFE.ESTipoCFECFC.ERemitoExportacionContingencia:

                        proc.Load(@"Certificados\Xml\xslt\ERemitoExportacion.xslt");

                        break;

                    default:
                        break;
                }

                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaContingenciaComprobantes;
                }
                else
                {
                    RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaCertificadosTemporales;
                }
                proc.Transform(RUTA_CERTIFICADOS + "\\" + tipo + "" + serie + "" + numeroCertificado + "_prev.xml", RUTA_CERTIFICADOS + "\\" + tipo + "" + serie + "" + numeroCertificado + "_nf.xml");

                //Eliminar certificado previo
                File.Delete(RUTA_CERTIFICADOS + "\\" + tipo + "" + serie + "" + numeroCertificado + "_prev.xml");
            }
            catch (Exception)
            {               
                return false;
            }

            return true;
        }

        /// <summary>
        /// Realiza la transformacion del sobre previo al sobre no firmado apliacando el formato oficial de la DGI
        /// </summary>
        /// <param name="nombreSobrePrevio"></param>
        /// <param name="nombreSobre"></param>
        /// <returns></returns>
        public static string TransformarSobre(string nombreSobrePrevio, string nombreSobre, List<CFE> listaCertificados, string adenda, bool sobreDgi)
        {
            try
            {
                XslCompiledTransform proc = new XslCompiledTransform();

                if (sobreDgi)
                {
                    proc.Load(@"Certificados\Xml\xslt\Sobre.xslt");
                }
                else
                {
                    if(adenda.Equals(""))
                    {
                        proc.Load(@"Certificados\Xml\xslt\SobreEntreEmpresas.xslt");
                    }
                    else
                    {
                        proc.Load(@"Certificados\Xml\xslt\SobreEntreEmpresasAdenda.xslt");
                    }
                }

                if (sobreDgi)
                {
                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaContingenciaSobresDgi;
                    }
                    else
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaSobresDgi;
                    }
                }
                else
                {
                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaContingenciaSobres;
                    }
                    else
                    {
                        RUTA_SOBRES = RutasCarpetas.RutaCarpetaSobres;
                    }
                }

                proc.Transform(RUTA_SOBRES + "\\" + nombreSobrePrevio + ".xml", RUTA_SOBRES + "\\" + nombreSobre + "_nf.xml");

                //Eliminar sobre previo
                File.Delete(RUTA_SOBRES + "\\" + nombreSobrePrevio + ".xml");

                //Obtener sobre no firmado
                XmlDocument xmlDocumentoNF = new XmlDocument();
                xmlDocumentoNF.Load(RUTA_SOBRES + "\\" + nombreSobre + "_nf.xml");
                XmlDocument xmlDocumentoNFAdenda = new XmlDocument();

                if (!adenda.Equals(""))
                {
                    xmlDocumentoNFAdenda.Load(RUTA_SOBRES + "\\" + nombreSobre + "_nf.xml");
                }

                //Obtener cada uno de los certificados
                XmlDocument xmlDocumentoCertificado = new XmlDocument();
                XmlDocument xmlDocumentoCertificadoAdenda = new XmlDocument();

                foreach (CFE cfe in listaCertificados)
                {
                    xmlDocumentoCertificado.Load(RUTA_CERTIFICADOS + "\\" + cfe.TipoCFEInt + cfe.SerieComprobante + cfe.NumeroComprobante + ".xml");

                    xmlDocumentoNF.DocumentElement.AppendChild(xmlDocumentoNF.ImportNode(xmlDocumentoCertificado.GetElementsByTagName("ns1:CFE").Item(0), true));
                    //xmlDocumentoNF.DocumentElement.AppendChild(xmlDocumentoNF.ImportNode(xmlDocumentoCertificado.GetElementsByTagName("CFE").Item(0), true));

                    if (!adenda.Equals(""))
                    {
                        if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                        {
                            xmlDocumentoCertificadoAdenda.Load(RutasCarpetas.RutaCarpetaContingenciaComprobantesAdenda + "\\" + cfe.TipoCFEInt + cfe.SerieComprobante + cfe.NumeroComprobante + ".xml");
                        }
                        else
                        {
                            xmlDocumentoCertificadoAdenda.Load(RutasCarpetas.RutaCarpetaComprobantesAdenda + "\\" + cfe.TipoCFEInt + cfe.SerieComprobante + cfe.NumeroComprobante + ".xml");
                        }
                        //xmlDocumentoNFAdenda.DocumentElement.AppendChild(xmlDocumentoNFAdenda.ImportNode(xmlDocumentoCertificadoAdenda.GetElementsByTagName("ns1:CFE").Item(0), true));
                        xmlDocumentoNFAdenda.GetElementsByTagName("DGICFE:CFE_Adenda").Item(0).AppendChild(xmlDocumentoNFAdenda.ImportNode(xmlDocumentoCertificadoAdenda.GetElementsByTagName("ns1:CFE").Item(0), true));

                        XmlElement cfeAdenda = xmlDocumentoNFAdenda.CreateElement("ns0", "Adenda", @"http://cfe.dgi.gub.uy");
                        XmlAttribute version = xmlDocumentoNFAdenda.CreateAttribute("version");
                        version.Value = "1.0";                        
                        cfeAdenda.Attributes.Append(version);
                        cfeAdenda.InnerText = adenda;

                        xmlDocumentoNFAdenda.GetElementsByTagName("DGICFE:CFE_Adenda").Item(0).AppendChild(cfeAdenda);
                    }
                }

                xmlDocumentoNF.Save(RUTA_SOBRES + "\\" + nombreSobre + ".xml");

                if (!adenda.Equals(""))
                {
                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        //guarda el sobre con adenda si existe
                        xmlDocumentoNFAdenda.Save(RutasCarpetas.RutaCarpetaContingenciaSobresAdenda + "\\" + nombreSobre + ".xml");
                    }
                    else
                    {
                        //guarda el sobre con adenda si existe
                        xmlDocumentoNFAdenda.Save(RutasCarpetas.RutaCarpetaSobresComprobantesAdenda + "\\" + nombreSobre + ".xml");
                    }                    
                }

                //Eliminar sobre no firmado
                File.Delete(RUTA_SOBRES + "\\" + nombreSobre + "_nf.xml");

                //if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                //{
                //    FTP ftp = new FTP();
                //    if (sobreDgi)
                //    {
                //        ftp.CargarArchivos(nombreSobre + ".xml", RUTA_SOBRES + nombreSobre + ".xml", 10);
                //    }
                //    else
                //    {
                //        ftp.CargarArchivos(nombreSobre + ".xml", RUTA_SOBRES + nombreSobre + ".xml", 5);
                //    }
                //}
            }
            catch (Exception)
            {
                return "";
            }

            return nombreSobre + ".xml";
        }

        #endregion TRANSFORMAR XSLT

        #region FIRMA CERTIFICADO

        /// <summary>
        /// Retorna el string del certificado indicado.
        /// </summary>
        /// <param name="ubicacionCertificado"></param>
        /// <returns></returns>
        public static string ObtenerCadenaCertificado()
        {
            string resultado = "";

            try
            {
                ManteUdoCertificadoDigital mante = new ManteUdoCertificadoDigital();

                Certificado certificado = mante.Consultar();

                X509Certificate2 objCert = new X509Certificate2(certificado.RutaCertificado, certificado.Clave);
                resultado = Convert.ToBase64String(objCert.GetRawCertData());
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }

            return resultado;
        }

        /// <summary>
        ///  Funcion para leer archivo certificado
        /// </summary>
        /// <param name="strArchivo"></param>
        /// <returns></returns>
        public static byte[] LeerArchivo(string strArchivo)
        {
            FileStream f = new FileStream(strArchivo, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        /// <summary>
        /// Realiza la firma del certificado utilizando un certificado digital 
        /// </summary>
        /// <param name="nombreAlmacenClaves"></param>
        /// <param name="numeroCertificado"></param>
        /// <returns></returns>
        public static bool FirmarCertificado(string nombreAlmacenClaves, int tipo, string serie, string numeroCertificado, string clave, bool adenda, ref string codSeguridad)
        {
            Firma_Digital.FirmaDigital firma = new Firma_Digital.FirmaDigital();
            
            try
            {
                //Crea un nuevo objeto CspParameters para especificar el contenedor de claves
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = nombreAlmacenClaves;

                //Crea un nuevo RSA. 
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                //Crea un nuevo documento xml
                XmlDocument xmlDocumento = new XmlDocument();

                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    if (adenda)
                    {
                        RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaContingenciaComprobantesAdenda;
                    }
                    else
                    {
                        RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaContingenciaComprobantes;
                    }                                        
                }
                else
                {
                    if (adenda)
                    {
                        RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaComprobantesAdenda;
                    }
                    else
                    {
                        RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaCertificadosTemporales;
                    }
                }

                //Carga el xml no firmado.
                xmlDocumento.PreserveWhitespace = true;
                xmlDocumento.Load(RUTA_CERTIFICADOS + "\\" + tipo + serie + numeroCertificado + "_nf.xml");

                byte[] pfxBlob = File.ReadAllBytes(nombreAlmacenClaves);

                //Firma el documento
                //SignXml(xmlDocumento, pfxBlob, clave);
                firma.SignXml(xmlDocumento, pfxBlob, rsaKey, clave);

                codSeguridad = "";
                codSeguridad = firma.ObtenerCodigoSeguridad(xmlDocumento);

                //Eliminar certificado no firmado
                File.Delete(RUTA_CERTIFICADOS + "\\" + tipo + serie + numeroCertificado + "_nf.xml");

                //Guarda el certificado firmado
                xmlDocumento.Save(RUTA_CERTIFICADOS + "\\" + tipo + serie + numeroCertificado + ".xml");

                ValidarFirmaElectronica(xmlDocumento, nombreAlmacenClaves, clave);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  Realiza la firma del ack de certificados
        /// </summary>
        /// <param name="nombreAlmacenClaves"></param>
        /// <param name="nombreACK"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public static bool FirmarACKCertificado(string nombreAlmacenClaves, string nombreACK, string clave)
        {
            try
            {
                //Crea un nuevo objeto CspParameters para especificar el contenedor de claves
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = nombreAlmacenClaves;

                //Crea un nuevo RSA. 
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                //Crea un nuevo documento xml
                XmlDocument xmlDocumento = new XmlDocument();

                //Carga el xml no firmado.
                xmlDocumento.PreserveWhitespace = true;


                xmlDocumento.Load(RutasCarpetas.RutaCarpetaACKCFEReceptor + "\\" + nombreACK + "_prev.xml");

                //xmlDocumento.LoadXml("<?xml version=\"1.0\"?><ACKCFE xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" version=\"1.0\" xmlns=\"http://cfe.dgi.gub.uy\"><Caratula><RUCReceptor>210160990017</RUCReceptor><RUCEmisor>219999830019</RUCEmisor><IDRespuesta>35</IDRespuesta><NomArch>sob_219999830019_20141218_2.xml</NomArch><FecHRecibido>2014-12-18T15:08:46.4742616-02:00</FecHRecibido><IDEmisor>2</IDEmisor><IDReceptor>35</IDReceptor><CantenSobre>8</CantenSobre><CantResponden>8</CantResponden><CantCFEAceptados>4</CantCFEAceptados><CantCFERechazados>4</CantCFERechazados><CantCFCAceptados>0</CantCFCAceptados><CantCFCObservados>0</CantCFCObservados><CantOtrosRechazados>0</CantOtrosRechazados><Tmst>2014-12-18T15:08:50.4742616-02:00</Tmst></Caratula><ACKCFE_Det><Nro_ordinal>1</Nro_ordinal><TipoCFE>111</TipoCFE><Serie>A</Serie><NroCFE>1</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>AE</Estado></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>2</Nro_ordinal><TipoCFE>111</TipoCFE><Serie>A</Serie><NroCFE>2</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>BE</Estado><MotivosRechazoCF><Motivo>E05</Motivo><Glosa>No cumple validaciones de Formato comprobantes</Glosa></MotivosRechazoCF></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>3</Nro_ordinal><TipoCFE>181</TipoCFE><Serie>A</Serie><NroCFE>1</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>AE</Estado></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>4</Nro_ordinal><TipoCFE>181</TipoCFE><Serie>A</Serie><NroCFE>2</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>BE</Estado><MotivosRechazoCF><Motivo>E05</Motivo><Glosa>No cumple validaciones de Formato comprobantes</Glosa></MotivosRechazoCF></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>5</Nro_ordinal><TipoCFE>182</TipoCFE><Serie>A</Serie><NroCFE>1</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>AE</Estado></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>6</Nro_ordinal><TipoCFE>182</TipoCFE><Serie>A</Serie><NroCFE>2</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>BE</Estado><MotivosRechazoCF><Motivo>E05</Motivo><Glosa>No cumple validaciones de Formato comprobantes</Glosa></MotivosRechazoCF></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>7</Nro_ordinal><TipoCFE>141</TipoCFE><Serie>A</Serie><NroCFE>1</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>AE</Estado></ACKCFE_Det><ACKCFE_Det><Nro_ordinal>8</Nro_ordinal><TipoCFE>141</TipoCFE><Serie>A</Serie><NroCFE>2</NroCFE><FechaCFE>2014-12-18</FechaCFE><TmstCFE>2014-12-18T15:08:52-02:00</TmstCFE><Estado>BE</Estado><MotivosRechazoCF><Motivo>E05</Motivo><Glosa>No cumple validaciones de Formato comprobantes</Glosa></MotivosRechazoCF></ACKCFE_Det></ACKCFE>");

                byte[] pfxBlob = File.ReadAllBytes(nombreAlmacenClaves);

                //Firma el documento
                SignXml(xmlDocumento, pfxBlob, clave);

                //Eliminar certificado no firmado
                File.Delete(RutasCarpetas.RutaCarpetaACKCFEReceptor + "\\" + nombreACK + "_prev.xml");

                //Guarda el certificado firmado
                xmlDocumento.Save(RutasCarpetas.RutaCarpetaACKCFEReceptor + "\\" + nombreACK + ".xml");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Realiza la firma del reporte diario utilizando un certificado digital 
        /// </summary>
        /// <param name="nombreAlmacenClaves"></param>
        /// <param name="nombreReporte"></param>
        /// <returns></returns>
        public static bool FirmarReporte(string nombreAlmacenClaves, string nombreReporte, string clave)
        {
            try
            {
                //Crea un nuevo objeto CspParameters para especificar el contenedor de claves
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = nombreAlmacenClaves;

                //Crea un nuevo RSA. 
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                //Crea un nuevo documento xml
                XmlDocument xmlDocumento = new XmlDocument();

                //Carga el xml no firmado.
                xmlDocumento.PreserveWhitespace = true;
                xmlDocumento.Load(RUTA_REPORTES + "\\" + nombreReporte + "_nf.xml");

                byte[] pfxBlob = File.ReadAllBytes(nombreAlmacenClaves);

                //Firma el documento
                SignXml(xmlDocumento, pfxBlob, clave);

                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    RUTA_REPORTES = RutasCarpetas.RutaCarpetaContingenciaReporteDiario;
                }
                else
                {
                    RUTA_REPORTES = RutasCarpetas.RutaCarpetaReporteDiario;
                }

                //Eliminar sobre no firmado
                File.Delete(RUTA_REPORTES + "\\" + nombreReporte + "_nf.xml");

                //Guarda el certificado firmado
                xmlDocumento.Save(RUTA_REPORTES + "\\" + nombreReporte + ".xml");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Realiza la firma de un documento xml utilizando un RSA 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="Key"></param>       
        public static void SignXml(XmlDocument originalXmlDocument, byte[] pfx, String pfxPassword)
        {
            X509Certificate2 cert = new X509Certificate2(pfx, pfxPassword);
            RSACryptoServiceProvider Key = cert.PrivateKey as RSACryptoServiceProvider;
            SignedXml signedXml = new SignedXml(originalXmlDocument) { SigningKey = Key };
            Reference reference = new Reference() { Uri = String.Empty };
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            //XmlDsigC14NTransform env2 = new XmlDsigC14NTransform();
            reference.AddTransform(env);
            //reference.AddTransform(env2);
            KeyInfoX509Data kdata = new KeyInfoX509Data(cert);
            kdata.AddIssuerSerial(cert.Issuer, cert.SerialNumber);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(kdata);
            signedXml.KeyInfo = keyInfo;
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            //Agrega el elemento xml al documento xml (Certificado)
            //originalXmlDocument.DocumentElement.AppendChild(originalXmlDocument.ImportNode(signedXml.GetXml(), true));

            originalXmlDocument.GetElementsByTagName("ns1:CFE").Item(0).AppendChild(originalXmlDocument.ImportNode(signedXml.GetXml(), true));

            // return signedXml.GetXml();
        }

        #endregion FIRMA CERTIFICADO

        #region CODIGO DE SEGURDAD

        public static string ObtenerCodigoSegurdad(string numeroCertificado)
        {
            string codigo = "";
            int TipoRuta = 1;
            string resultado = numeroCertificado;

            try
            {
                    XmlDocument xmlDocumento = new XmlDocument();
                     FTP ftp = new FTP();

               
                //Se asigna la ruta dependiendo del estado de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaContingenciaComprobantes ;
                    TipoRuta = 6;  
                }
                else
                {
                    RUTA_CERTIFICADOS = RutasCarpetas.RutaCarpetaCertificadosTemporales;
                                           
                }




                if (ftp.descargarArchivos(resultado, RUTA_CERTIFICADOS, TipoRuta))
                    {
                        //Por si el archivo ya fue movido por el FTP se descrga de nuevo.
                    }
              


                     
                  
             xmlDocumento.Load(RUTA_CERTIFICADOS + "\\" + numeroCertificado + ".xml");                               
             codigo = xmlDocumento.GetElementsByTagName("DigestValue").Item(0).InnerText;

            }
            catch (Exception)
            {
            }


            return codigo;
        }

        #endregion CODIGO DE SEGURIDAD

        #region VALIDAR FIRMA ELECTRONICA

        /// <summary>
        /// Valida la firma electronica en el un CFE
        /// </summary>
        /// <param name="originalXmlDocument"></param>
        /// <param name="nombreAlmacenClaves"></param>
        /// <param name="pfxPassword"></param>
        /// <returns></returns>
        public static bool ValidarFirmaElectronica(XmlDocument originalXmlDocument, string nombreAlmacenClaves, String pfxPassword)
        {
            SignedXml signedXml = null;
            X509Certificate2 cert = null;

            try
            {
                byte[] pfxBlob = File.ReadAllBytes(nombreAlmacenClaves);

                cert = new X509Certificate2(pfxBlob, pfxPassword);

                // Create a new SignedXml object and pass it
                // the XML document class.
                signedXml = new SignedXml(originalXmlDocument);

                // Find the "Signature" node and create a new
                // XmlNodeList object.
                XmlNodeList nodeList = originalXmlDocument.GetElementsByTagName("Signature");

                if (nodeList.Count <= 0)
                {
                    return false;
                }

                if (nodeList.Count >= 2)
                {
                    return false;
                }

                // Load the first <signature> node.  
                signedXml.LoadXml((XmlElement)nodeList[0]);
            }
            catch (Exception)
            {
            }

            // Check the signature and return the result.
            return signedXml.CheckSignature(cert, true);
        }

        #endregion VALIDAR FIRMA ELECTRONICA

        #region ADENDA

        /// <summary>
        /// Genera comprobante con Adenda
        /// </summary>
        /// <param name="numeroCertificado"></param>
        public static void GenerarCFEAdenda(int tipoComprobante, string serieComprobante, string numeroCertificado, string adenda)
        {
            try
            {
                string rutaComprobantesAdenda = string.Empty;

                //Se asigna la ruta dependiendo del estado de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    rutaComprobantesAdenda = RutasCarpetas.RutaCarpetaContingenciaComprobantesAdenda;
                }
                else
                {
                    rutaComprobantesAdenda = RutasCarpetas.RutaCarpetaComprobantesAdenda;
                }

                //Se define la ruta del comprobante
                string rutaArchivoAdenda = rutaComprobantesAdenda + "\\" + tipoComprobante + serieComprobante + numeroCertificado + "_nf.xml";

                //Copia el archivo a la carpeta de comprobantes con adenda
                System.IO.File.Copy(RUTA_CERTIFICADOS + "\\" + tipoComprobante + serieComprobante + numeroCertificado + "_nf.xml", rutaArchivoAdenda, true);

                /*XmlDocument sinAdenda = new XmlDocument();
                //Se carga el xml de la carpeta comprobantes con adenda
                sinAdenda.Load(rutaArchivoAdenda);*/

                /* */
                /*XmlDocument xmlSalidaConAdenda = new XmlDocument();
                xmlSalidaConAdenda.PreserveWhitespace = true;

                XmlNode cfeAdenda = xmlSalidaConAdenda.CreateElement("CFE_Adenda");
                xmlSalidaConAdenda.AppendChild(cfeAdenda);

                XmlNode nodoCFE = sinAdenda.DocumentElement;
                xmlSalidaConAdenda.DocumentElement.AppendChild(xmlSalidaConAdenda.ImportNode(nodoCFE, true));

                XmlNode nodoAdenda = xmlSalidaConAdenda.CreateElement("Adenda");
                nodoAdenda.InnerText = adenda;

                xmlSalidaConAdenda.DocumentElement.AppendChild(nodoAdenda);

                xmlSalidaConAdenda.Save(rutaArchivoAdenda);*/                
                /* */
                
                /*
                //Se crea el tag Adenda
                XmlElement nodoAdenda = sinAdenda.CreateElement("Adenda");
                //Se le agrega la informacion al tag Adenda
                nodoAdenda.InnerText = adenda;

                //Se obtiene el nodo raiz
                XmlNode raiz = sinAdenda.DocumentElement;
                //Se agrega el nuevo nodo a la raiz
                raiz.AppendChild(nodoAdenda);

                //Se guarda el nodo en el archivo
                sinAdenda.Save(rutaArchivoAdenda);
                 * */

            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// Obtiene la adenda en caso de que exista
        /// </summary>
        /// <param name="cfe"></param>
        /// <returns></returns>
        public static string ObtenerAdenda(CFE cfe)
        {
            string resultado = "";

            if (cfe != null)
            {
                if (!cfe.TextoLibreAdenda.Equals(""))
                {
                    resultado = cfe.TextoLibreAdenda;
                }
            }

            return resultado;
        }

        #endregion ADENDA
    }
}
