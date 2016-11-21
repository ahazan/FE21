using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using iTextSharp.text.pdf;
using x509 = System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf.crypto;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using Org.BouncyCastle.X509;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Certificados.Xml.Transformacion;

using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Xml.Serialization;
using System.Xml.Schema;



namespace SEICRY_FE_UYU_9.Firma_Digital
{
    class FirmaDigital
    {
        /// <summary>
        /// Metodo para firmar digitalmente un documento
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="SignerCertificate"></param>
        /// <returns></returns>
        private static byte[] firmarDocumento(byte[] Message, x509.X509Certificate2 SignerCertificate)
        {
            //Creamos el contenedor
            ContentInfo docFirmar = new ContentInfo(Message);

            //Instanciamos el objeto SignedCms con el contenedor
            SignedCms objFirmado = new SignedCms(docFirmar, false);

            //Creamos el "firmante"
            CmsSigner objFirmante = new CmsSigner(SignerCertificate);
            objFirmante.IncludeOption = x509.X509IncludeOption.EndCertOnly;

            //Firma el mesnaje CMS/PKCS. El segundo argumento
            //es necesario para preguntar  por el pin.
            objFirmado.ComputeSignature(objFirmante, false);

            //Encodeamos el mensaje CMS/PKCS #7
            return objFirmado.Encode();
        }

        /// <summary>
        /// Configura la informacion del certificado digital
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="rutaCertificado"></param>
        /// <param name="pass"></param>
        public bool infoCertificado(string origen, string destino, string rutaCertificado, string pass)
        {
            bool resultado = false;

            try
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("origen " + origen);
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("destino " + destino);
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("rutaCertificado " + rutaCertificado);
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("clave " + pass);

                //Se obtiene el certficado
                x509.X509Certificate2 certificado = new x509.X509Certificate2(rutaCertificado, pass);
                X509CertificateParser objCP = new X509CertificateParser();

                Org.BouncyCastle.X509.X509Certificate[] objChain = new
                    Org.BouncyCastle.X509.X509Certificate[] { objCP.ReadCertificate(certificado.RawData) };

                //Objeto de tipo documento pdf
                PdfReader objReader = new PdfReader(origen);
                //Crea el objeto para la firma digital
                PdfStamper objStamper = PdfStamper.CreateSignature(objReader,
                    new FileStream(destino, FileMode.Create), '\0');
                PdfSignatureAppearance objSA = objStamper.SignatureAppearance;  

                //Configuracion de informacion para la firma digital
                objSA.SignDate = DateTime.Now;
                objSA.SetCrypto(null, objChain, null, null);                        
                objSA.Reason = "Comprobante Generado";
                objSA.Location = "Uruguay";
                objSA.Acro6Layers = true;             
                objSA.Render = PdfSignatureAppearance.SignatureRender.NameAndDescription;

                PdfSignature objSignature = new PdfSignature(PdfName.ADOBE_PPKMS,
                    PdfName.ADBE_PKCS7_SHA1);
                objSignature.Date = new PdfDate(objSA.SignDate);
                objSignature.Name = PdfPKCS7.GetSubjectFields(objChain[0]).GetField("CN");

                if (objSA.Reason != null)
                    objSignature.Reason = objSA.Reason;

                if (objSA.Location != null)
                    objSignature.Location = objSA.Location;

                objSA.CryptoDictionary = objSignature;
                int intCSize = 4000;

                Hashtable objTable = new Hashtable();
                objTable[PdfName.CONTENTS] = intCSize * 2 + 2;
                objSA.PreClose(objTable);
                Stream objStream = objSA.RangeStream;

                HashAlgorithm objSHA1 = new SHA1CryptoServiceProvider();
                int intRead = 0;

                byte[] bytBuffer = new byte[8192];
                while ((intRead = objStream.Read(bytBuffer, 0, 8192)) > 0)
                    objSHA1.TransformBlock(bytBuffer, 0, intRead, bytBuffer, 0);
                objSHA1.TransformFinalBlock(bytBuffer, 0, 0);

                byte[] bytPK = firmarDocumento(objSHA1.Hash, certificado);
                byte[] bytOut = new byte[intCSize];

                PdfDictionary objDict = new PdfDictionary();
                Array.Copy(bytPK, 0, bytOut, 0, bytPK.Length);

                objDict.Put(PdfName.CONTENTS, new PdfString(bytOut).SetHexWriting(true));
                objStream.Close();
                objSA.Close(objDict);
                resultado = true;
            }
            catch(Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
            }

            return resultado;
        }

        /// <summary>
        /// Firma el ACK generado
        /// </summary>
        /// <param name="nombreAlmacenClaves"></param>
        /// <param name="numeroCertificado"></param>
        /// <returns></returns>
        public bool FirmarACK(string nombreACKsf, string nombreACKf)
        {
            string rutaCertificado = "", passCertificado = "" ;
            bool resultado = true;

            try
            {
                ManteUdoCertificadoDigital cerDigital = new ManteUdoCertificadoDigital();

                //Se obtiene informacion del certificado digital
                rutaCertificado = cerDigital.ObtenerRutaCertificado();
                passCertificado = cerDigital.ObtenerPassCertificado();

                //Crea un nuevo objeto CspParameters para especificar el contenedor del certificado
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = rutaCertificado;

                //Crea un nuevo RSA. 
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                //Crea un nuevo documento xml
                XmlDocument xmlDocumento = new XmlDocument();

                //Carga el xml no firmado.
                xmlDocumento.PreserveWhitespace = true;
                
                xmlDocumento.Load(nombreACKsf);
               

              //CENTENARIO BUENO
                //xmlDocumento.LoadXml("<?xml version=\"1.0\"?><ACKSobre xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" version=\"1.0\" xmlns=\"http://cfe.dgi.gub.uy\"><Caratula><RUCReceptor>210308350014</RUCReceptor><RUCEmisor>219999830019</RUCEmisor><IDRespuesta>100</IDRespuesta><NomArch>sob_219999830019_20141211_2.xml</NomArch><FecHRecibido>2014-12-11T09:57:16.4132781-02:00</FecHRecibido><IDEmisor>2</IDEmisor><IDReceptor>100</IDReceptor><CantidadCFE>8</CantidadCFE><Tmst>2014-12-11T09:58:26.4133782-02:00</Tmst></Caratula><Detalle><Estado>AS</Estado><ParamConsulta><Token>8iLdRqMvLC3m/5dxX3sjsFuQ9qs4vkVUVZl6QqDE0/5rgMl/rs8CSUoPSDeG4QhL5S3OP+6fCHJMDIQwIJlXlQ==</Token><Fechahora>2014-12-11T09:59:16.4132781-02:00</Fechahora></ParamConsulta></Detalle></ACKSobre>");

                //CENTENARIO MALO
               // xmlDocumento.LoadXml("<?xml version=\"1.0\"?><ACKSobre xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" version=\"1.0\" xmlns=\"http://cfe.dgi.gub.uy\"><Caratula><RUCReceptor>210308350014</RUCReceptor><RUCEmisor>219999830019</RUCEmisor><IDRespuesta>101</IDRespuesta><NomArch>sob_219999830019_20141211_1.xml</NomArch><FecHRecibido>2014-12-11T09:57:43.5371631-02:00</FecHRecibido><IDEmisor>1</IDEmisor><IDReceptor>101</IDReceptor><CantidadCFE>15</CantidadCFE><Tmst>2014-12-11T09:57:43.5371631-02:00</Tmst></Caratula><Detalle><Estado>BS</Estado><MotivosRechazo><Motivo>S05</Motivo><Glosa>No coinciden cantidad CFE de carátula y contenido</Glosa></MotivosRechazo></Detalle></ACKSobre>");

                //LUSA BUENO
                //xmlDocumento.LoadXml("<?xml version=\"1.0\"?><ACKSobre xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" version=\"1.0\" xmlns=\"http://cfe.dgi.gub.uy\"><Caratula><RUCReceptor>210160990017</RUCReceptor><RUCEmisor>219999830019</RUCEmisor><IDRespuesta>35</IDRespuesta><NomArch>sob_219999830019_20141218_2.xml</NomArch><FecHRecibido>2014-12-18T15:08:46.4642461-02:00</FecHRecibido><IDEmisor>2</IDEmisor><IDReceptor>35</IDReceptor><CantidadCFE>8</CantidadCFE><Tmst>2014-12-18T15:08:50.4642461-02:00</Tmst></Caratula><Detalle><Estado>AS</Estado><ParamConsulta><Token>hmbh5ghNyOIEQ95B9oJtE9Tjsys=</Token><Fechahora>2014-12-18T15:08:52.4642461-02:00</Fechahora></ParamConsulta></Detalle></ACKSobre>");

                //LUSA MALO
                 //xmlDocumento.LoadXml("<?xml version=\"1.0\"?><ACKSobre xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" version=\"1.0\" xmlns=\"http://cfe.dgi.gub.uy\"><Caratula><RUCReceptor>210160990017</RUCReceptor><RUCEmisor>219999830019</RUCEmisor><IDRespuesta>34</IDRespuesta><NomArch>sob_219999830019_20141218_1.xml</NomArch><FecHRecibido>2014-12-18T15:08:52.7576766-02:00</FecHRecibido><IDEmisor>1</IDEmisor><IDReceptor>34</IDReceptor><CantidadCFE>5</CantidadCFE><Tmst>2014-12-18T15:08:54.4642461-02:00</Tmst></Caratula><Detalle><Estado>BS</Estado><MotivosRechazo><Motivo>S05</Motivo><Glosa>No coinciden cantidad CFE de carátula y contenido</Glosa></MotivosRechazo></Detalle></ACKSobre>");

                byte[] pfxBlob = File.ReadAllBytes(rutaCertificado); 

                //Firma el documento
                ProcTransformacion.SignXml(xmlDocumento, pfxBlob, passCertificado);

                //Guarda el certificado firmado
                xmlDocumento.Save(nombreACKf);

                //BUENO
                //xmlDocumento.Save(@"C:\Users\greivinsalas\Desktop\M_35_Sob_219999830019_20141218_2.xml");

                //MALO
               // xmlDocumento.Save(@"C:\Users\greivinsalas\Desktop\M_34_Sob_219999830019_20141218_1.xml");
               

            }
            catch (Exception)
            {
                resultado = false;
            }

            return resultado;
        }


        public XmlDocument refirmarDocumentos(XmlDocument XmlDocument)
        {
            string nombre = String.Empty;

            ManteUdoCertificadoDigital cerDigital = new ManteUdoCertificadoDigital();

            //Se obtiene informacion del certificado digital
            string rutaCertificado = cerDigital.ObtenerRutaCertificado();
            string clave = cerDigital.ObtenerPassCertificado();
                     
            DateTime fecha = DateTime.Now;
            string fechaFirma = fecha.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

            string strRutReceptor, strFecha, strFechaFirma, strInfoCertificado, strFechaEmision, strCodSucursal;

            XmlDocument documento = new XmlDocument();

            strRutReceptor      = "DGICFE:RutReceptor";
            strFecha            = "DGICFE:Fecha";
            strInfoCertificado  = "DGICFE:X509Certificate";
            strFechaFirma       = "ns1:TmstFirma";
            strFechaEmision     = "ns1:FchEmis";
            strCodSucursal      = "ns1:CdgDGISucur";

             nombre = String.Empty;

                documento = new XmlDocument();
                documento.PreserveWhitespace = true;
                documento = XmlDocument;                                          

                FirmaDigital firma = new FirmaDigital();
                string infoCertificado = ProcTransformacion.ObtenerCadenaCertificado();

                XmlNode nodoCertificado = documento.GetElementsByTagName(strInfoCertificado).Item(0);
                nodoCertificado.InnerXml = infoCertificado;
                // End Caratula /

                // Begin CFE /
                XmlNode nodoFechaFirma = documento.GetElementsByTagName(strFechaFirma).Item(0);
                nodoFechaFirma.InnerXml = fechaFirma;       
                              
                // End CFE /

                #region CFE_FIRMA
                // Elimino el nodo Firma del template                
                XmlNode nodoFirma = documento.GetElementsByTagName("Signature").Item(0);
                documento.GetElementsByTagName("ns1:CFE").Item(0).RemoveChild(nodoFirma);

                //Crea nuevo documento con el CFE para firmar
                XmlDocument docFirma = new XmlDocument();
                docFirma.PreserveWhitespace = true;

                XmlNode cfe = documento.DocumentElement.GetElementsByTagName("ns1:CFE").Item(0);
                XmlNode nuevo = docFirma.ImportNode(cfe, true);
                docFirma.AppendChild(nuevo);

                try
                {
                    // Create a new CspParameters object to specify
                    // a key container.
                    CspParameters cspParams = new CspParameters();
                    cspParams.KeyContainerName = rutaCertificado;

                    // Create a new RSA signing key and save it in the container. 
                    RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                    // Obtengo cadena certificado /
                    byte[] pfxBlob = File.ReadAllBytes(rutaCertificado);

                    // Sign the XML document.
                    firma.SignXml(docFirma, pfxBlob, rsaKey, clave);                    
                

                // Elimino el CFE del template //
                documento.DocumentElement.RemoveChild(cfe);

                // Agrego el CFE firmado //
                XmlNode cfeFirmado = docFirma.GetElementsByTagName("ns1:CFE").Item(0);
                nuevo = documento.ImportNode(cfeFirmado, true);
                documento.DocumentElement.AppendChild(nuevo);
                #endregion CFE_FIRMA    
                    
                }
                catch (Exception e)
                {

                }
            

            return documento;
        }

        public XmlDocument SignXml(XmlDocument xmlDoc, byte[] pfx, RSA rsaKey, string claveCert)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (rsaKey == null)
                throw new ArgumentException("Key");

            X509Certificate2 cert = new X509Certificate2(pfx, claveCert);
            RSACryptoServiceProvider Key = cert.PrivateKey as RSACryptoServiceProvider;

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc) { SigningKey = Key };

            // Add the key to the SignedXml document.
            //signedXml.SigningKey = rsaKey;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            /* */
            KeyInfoX509Data kdata = new KeyInfoX509Data(cert);
            kdata.AddIssuerSerial(cert.Issuer, cert.SerialNumber);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(kdata);
            signedXml.KeyInfo = keyInfo;
            /* */

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            /* 18.11.2016 - Se agrega prefijo ds a la firma digital */
            // Add prefix "ds:" to signature
            XmlElement signature = signedXml.GetXml();
            SetPrefix("ds", signature);

            // Load modified signature back
            signedXml.LoadXml(signature);

            // this is workaround for overcoming a bug in the library
            signedXml.SignedInfo.References.Clear();

            // Recompute the signature
            signedXml.ComputeSignature();
            string recomputedSignature = Convert.ToBase64String(signedXml.SignatureValue);

            // Replace value of the signature with recomputed one
            ReplaceSignature(signature, recomputedSignature);

            // Append the signature to the XML document. 
            xmlDoc.DocumentElement.InsertAfter(xmlDoc.ImportNode(signature, true), xmlDoc.DocumentElement.FirstChild);
            /*  */

            /*
            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            */

            return xmlDoc;
        }

        private void SetPrefix(string prefix, XmlNode node)
        {
            node.Prefix = prefix;
            foreach (XmlNode n in node.ChildNodes)
            {
                SetPrefix(prefix, n);
            }
        }

        private void ReplaceSignature(XmlElement signature, string newValue)
        {
            if (signature == null) throw new ArgumentNullException();
            if (signature.OwnerDocument == null) throw new ArgumentException("No owner document");

            XmlNamespaceManager nsm = new XmlNamespaceManager(signature.OwnerDocument.NameTable);
            nsm.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);

            XmlNode signatureValue = signature.SelectSingleNode("ds:SignatureValue", nsm);
            if (signatureValue == null)
                throw new Exception("Signature does not contain 'ds:SignatureValue'");

            signatureValue.InnerXml = newValue;
        }

        public string ObtenerCodigoSeguridad(XmlDocument cfeFirmado)
        {
            string codSeguridad = "";

            try
            {
                codSeguridad = cfeFirmado.GetElementsByTagName("ds:DigestValue").Item(0).InnerText.Substring(0, 6);
            }
            catch (Exception ex)
            {

            }
            return codSeguridad;
        }
    }
}
