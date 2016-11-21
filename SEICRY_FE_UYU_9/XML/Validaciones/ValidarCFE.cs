using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Certificados.Xml.Transformacion;
using SEICRY_FE_UYU_9.Objetos;
using System.Collections;

namespace SEICRY_FE_UYU_9.XSD
{
    class ValidarCFE
    {
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";

        //Instancias para validaciones
        private ManteUdoCFE manteUdoCfe = new ManteUdoCFE();

        //XmlTextReader para cada XSD a cargar
        static XmlTextReader trTipoCFE = null;
        static XmlTextReader trTipoDGI = null;
        static XmlTextReader trCFEDGI = null;

        //Esquema para cargar los XSD
        static XmlSchemaCollection xsc = null;
        //static XmlSchemaSet xsc2 = null;

        //Contienen los errores de formato encontrados en el momento de realizar la validacion
        public static List<string> erroresEncontrados = new List<string>();
        //Numero de errores encontrados en el momento de realizar la validacion
        static int numErrores = 0;

        public void ValidarContenidoCFE(string xmlCFE)
        {
            XmlDocument xmlDocumento = new XmlDocument();
            xmlDocumento.LoadXml(xmlCFE);
        }

        /// <summary>
        /// Metodo para realizar las validaciones de los comprobantes fiscales
        /// </summary>
        /// <returns></returns>
        public bool ValidarComprobanteFiscal(XmlDocument documento, int tipo, int numeroCFE, string serieCFE,
             int inicioCAE, int finCAE, string fechaFirma, string fechaCAE, int docEntry)
        {
            bool resultado = true, error = false;
            //Lista que contiene los errores
            List<ErrorValidarSobre> listaErrores = new List<ErrorValidarSobre>();

            List<string> listaCfe = ObtenerCFEs(documento);

            foreach (string cfe in listaCfe)
            {
                if (ValidarCFESeleccionado(tipo.ToString(), serieCFE, numeroCFE.ToString(), cfe))
                {
                    if (!ValidarXsdCFE(cfe))
                    {
                        //Se activa bandera de error
                        error = true;

                        //Agrega los errores encontrados en la validacion de los 
                        foreach (string errorXsd in erroresEncontrados)
                        {
                            //Se crea objeto de error
                            ErrorValidarSobre errorValidar = new ErrorValidarSobre();
                            errorValidar.CodigoRechazo = Mensaje.errCompValXsdCod;
                            errorValidar.DetalleRechazo = Mensaje.errCompValXsdDet;
                            errorValidar.GlosaRechazo = errorXsd;

                            //Se agrega el error a la lista de errores
                            listaErrores.Add(errorValidar);
                        }
                    }
                }
            }

            if (!ValidarNumeroTipoCFE(tipo, numeroCFE))
            {
                //Se activa bandera de error
                error = true;

                ErrorValidarSobre errorValidar = new ErrorValidarSobre();
                errorValidar.CodigoRechazo = Mensaje.errCompValNumCFECod;
                errorValidar.DetalleRechazo = Mensaje.errCompValNumCFEDetGlo;
                errorValidar.GlosaRechazo = Mensaje.errCompValNumCFEDetGlo;

                //Se agrega el error a la lista de errores
                listaErrores.Add(errorValidar);
            }
            if (!ValidarNumeroCFEContraCAE(numeroCFE, inicioCAE, finCAE))
            {
                //Se activa bandera de error
                error = true;

                //Se crea objeto de error
                ErrorValidarSobre errorValidar = new ErrorValidarSobre();
                errorValidar.CodigoRechazo = Mensaje.errCompValNumCAECFECod;
                errorValidar.DetalleRechazo = Mensaje.errCompValNumCAECFEDetGlo;
                errorValidar.GlosaRechazo = Mensaje.errCompValNumCAECFEDetGlo;

                //Se agrega el error a la lista de errores
                listaErrores.Add(errorValidar);
            }
            //if (ValidarFimaElectronica(documento.InnerXml))
            //{
            //    //Se activa bandera de error
            //    error = true;

            //    //Se crea objeto de error
            //    ErrorValidarSobre errorValidar = new ErrorValidarSobre();
            //    errorValidar.CodigoRechazo = Mensaje.errCompValFirmaCod;
            //    errorValidar.DetalleRechazo = Mensaje.errCompValFirmaDetGlo;
            //    errorValidar.GlosaRechazo = Mensaje.errCompValFirmaDetGlo;

            //    //Se agrega el error a la lista de errores
            //    listaErrores.Add(errorValidar);
            //}
            if (ValidarFechaFirmaContraCAE(fechaFirma, fechaCAE))
            {
                //Se activa bandera de error
                error = true;

                //Se crea objeto de error
                ErrorValidarSobre errorValidar = new ErrorValidarSobre();

                errorValidar.CodigoRechazo = Mensaje.errCompValFechaCAECod;
                errorValidar.DetalleRechazo = Mensaje.errCompValFechaCAEDetGlo;
                errorValidar.GlosaRechazo = Mensaje.errCompValFechaCAEDetGlo;

                //Se agrega el error a la lista de errores
                listaErrores.Add(errorValidar);
            }

            if (error == true)
            {
                resultado = false;
                AlmacenarErrores(listaErrores, docEntry);
            }

            return resultado;
        }

        #region VALIDAR XSD

        /// <summary>
        /// Metodo para cargar los XSD(Solo se cargan una vez debido a que el proceso tarda aprox 2 minutos)
        /// </summary>
        private void CargarXSD()
        {
            if (trTipoDGI == null)
            {
                trTipoDGI = new XmlTextReader(RutasXSD.RutaXsdTipoDgi);
            }
            if (trTipoCFE == null)
            {
                trTipoCFE = new XmlTextReader(RutasXSD.RutaXsdTipoCfe);
            }
            if (trCFEDGI == null)
            {
                trCFEDGI = new XmlTextReader(RutasXSD.RutaXsdCFEDGI);
            }

            if (xsc == null)
            {
                xsc = new XmlSchemaCollection();
                //xsc2 = new XmlSchemaSet();
                //xsc2.ValidationEventHandler +=new ValidationEventHandler(ValidationHandler);

                try
                {
                    //Se cargan los XSD al esquema                    
                    xsc.Add(null, trTipoDGI);
                    xsc.Add(null, trTipoCFE);
                    xsc.Add(null, trCFEDGI);
                    //xsc2.Add(null, trTipoDGI);
                    //xsc2.Add(null, trTipoCFE);
                    //xsc2.Add(null, trCFEDGI);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Metodo que valida un xml contra los XSD
        /// </summary>
        /// <param name="contenidoSobre"></param>
        /// <returns></returns>
        public bool ValidarXsdCFE(string contenido)
        {
            bool resultado = false;
            XmlValidatingReader validar = null;
            erroresEncontrados = new List<string>();

            try
            {
                //Carga el xsc
                CargarXSD();

                //if (xsc2.Count > 0)
                //{
                //    XmlReaderSettings settings = new XmlReaderSettings();
                //    settings.ValidationType = ValidationType.Schema;
                //    settings.Schemas.Add(xsc2);
                //    settings.Schemas.Compile();
                //    settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
                //    XmlTextReader validar = new XmlTextReader(contenido);

                //    using (XmlReader reader = XmlReader.Create(validar, settings))
                //    {
                //        while (reader.Read())
                //        {
                //        }

                //        reader.Close();
                //    }
                //}

                validar = new XmlValidatingReader(contenido, XmlNodeType.Document, null);

                //Se agrega el esquema
                validar.Schemas.Add(xsc);
                //Se define el tipo de validacion
                validar.ValidationType = ValidationType.Schema;

                //Se agrega el manejador de eventos
                validar.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                //Se empieza a recorrer el xml para la validacion
                while (validar.Read()) ;

                //Se cierra el lector
                validar.Close();

                //Validacion aprobada
                if (numErrores == 0)
                {
                    resultado = true;
                    erroresEncontrados = new List<string>();
                }
                else
                {
                    //Validacion no aprobada(se resetea el numErrores debido a que es estatico)
                    numErrores = 0;
                }
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Manejador de validacion, controla  los mensajes de error y la cantidad de errores encontrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ValidationHandler(object sender, ValidationEventArgs e)
        {
            erroresEncontrados.Add(e.Message);
            numErrores++;
        }

        #endregion VALIDAR XSD

        #region VALIDAR NUMERO Y TIPO DE CFE

        /// <summary>
        /// Valida que el tipo y numero de CFE no existan
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool ValidarNumeroTipoCFE(int tipo, int numero)
        {
            return !manteUdoCfe.ConsultarTipoNumero(tipo, numero);
        }

        #endregion VALIDAR NUMERO Y TIPO DE CFE

        #region VALIDAR NUMERO CFE CONTRA CAE

        /// <summary>
        /// Valida que el numero del CFE sea valido segun los datos del CAE
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="inicioCAE"></param>
        /// <param name="finCAE"></param>
        /// <returns></returns>
        public bool ValidarNumeroCFEContraCAE(int numero, int inicioCAE, int finCAE)
        {
            return (inicioCAE <= numero && finCAE >= numero) ? true : false;
        }

        #endregion VALIDAR NUMERO CFE CONTRA CAE

        #region VALIDAR FIRMA ELECTRONICA

        /// <summary>
        /// Valida la firma electronica en un CFE
        /// </summary>
        /// <param name="cfe"></param>
        /// <returns></returns>
        public bool ValidarFimaElectronica(string cfe)
        {
            XmlDocument xmlDocumento = new XmlDocument();
            xmlDocumento.LoadXml(cfe);

            ObtenerFirmaDigital();

            return ProcTransformacion.ValidarFirmaElectronica(xmlDocumento, RUTA_CERTIFICADO, CLAVE_CERTIFICADO);
        }

        /// <summary>
        /// Metodo para obtener informacion de la firma digital
        /// </summary>
        public bool ObtenerFirmaDigital()
        {
            ManteUdoCertificadoDigital manteUdoFirma = new ManteUdoCertificadoDigital();

            Certificado certificado = manteUdoFirma.Consultar();

            if (certificado != null)
            {
                RUTA_CERTIFICADO = certificado.RutaCertificado;
                CLAVE_CERTIFICADO = certificado.Clave;

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion VALIDAR FIRMA ELECTRONICA

        #region VALIDAR FECHA FIRMA CONTRA FECHA CAE

        /// <summary>
        /// Valida que la fecha de la firma sea menor o igual a la fecha de vencimiento del CAE
        /// </summary>
        /// <param name="fechaFirma"></param>
        /// <param name="fechaCAE"></param>
        /// <returns></returns>
        public bool ValidarFechaFirmaContraCAE(string fechaFirma, string fechaCAE)
        {
            DateTime dateFirma = DateTime.Now, dateCAE = DateTime.Now;

            try
            {
                dateFirma = Convert.ToDateTime(fechaFirma); //2014-09-16T09:38:56-06:00

                dateCAE = Convert.ToDateTime(fechaCAE);//2016-06-04
            }
            catch (Exception)
            {
                return false;
            }

            return dateFirma <= dateCAE ? true : false;
        }

        #endregion VALIDAR FECHA FIRMA CONTRA FECHA CAE

        #region ALMACENAR ERRORES

        /// <summary>
        /// Envia almacenar los errores en la tabla TFEESTCFER
        /// </summary>
        /// <param name="listaErrores"></param>
        /// <param name="DocEntry"></param>
        private void AlmacenarErrores(List<ErrorValidarSobre> listaErrores, int DocEntry)
        {
            //Se recorren los errores encontrados
            foreach (ErrorValidarSobre errorEncontrado in listaErrores)
            {
                //Se crea el objeto para estado de certificado recibido
                EstadoCertificadoRecibido estadoCertificadoRecibido = new EstadoCertificadoRecibido();

                estadoCertificadoRecibido.IdConsecutivo = DocEntry.ToString();
                estadoCertificadoRecibido.Motivo = errorEncontrado.CodigoRechazo;
                estadoCertificadoRecibido.Detalle = errorEncontrado.DetalleRechazo;
                estadoCertificadoRecibido.Glosa = errorEncontrado.GlosaRechazo;

                ManteUdoEstadoSobreRecibido manteEstadoSobreRecibido = new ManteUdoEstadoSobreRecibido();
                manteEstadoSobreRecibido.Almacenar(estadoCertificadoRecibido);
            }
        }

        #endregion ALMACENAR ERRORES

        /// <summary>
        /// Metodo para obtener los CFEs
        /// </summary>
        /// <param name="sinFormato"></param>
        /// <returns></returns>
        private List<string> ObtenerCFEs(XmlDocument sinFormato)
        {
            List<string> listaCfes = new List<string>();

            string tempComprobante = "", tag = "";

            try
            {
                //Obtiene los comprobantes del sobre
                XmlNodeList cantCFes = sinFormato.GetElementsByTagName("ns0:CFE");

                foreach (XmlElement comprobante in cantCFes)
                {
                    //Determina el tipo de comprobante fiscal
                    tag = obtenerTipoComprobante(comprobante);
                    //Agrega tags necesarios para formatear archivo xml
                    tempComprobante = "<ns0:CFE version=\"1.0\" xmlns:ns0=\"http://cfe.dgi.gub.uy\">" + comprobante.InnerXml.Replace("<ns0:" + tag + " xmlns:ns0=\"http://cfe.dgi.gub.uy\">", "<ns0:" + tag + ">") + "</ns0:CFE>";
                    listaCfes.Add(tempComprobante);
                }
            }
            catch (Exception)
            {
            }

            return listaCfes;
        }

        /// <summary>
        /// Determina si un tag pertenece a un elemento Xml
        /// </summary>
        /// <param name="comprobante"></param>
        /// <returns></returns>
        private string obtenerTipoComprobante(XmlElement comprobante)
        {
            string tag = "";

            if (contieneComprobantes(comprobante, "ns1:eFact"))
            {
                tag = "eFact";
            }
            else if (contieneComprobantes(comprobante, "ns1:eFact_Exp"))
            {
                tag = "eFact_Exp";
            }
            else if (contieneComprobantes(comprobante, "ns1:eTck"))
            {
                tag = "eTck";
            }
            else if (contieneComprobantes(comprobante, "ns1:eRem"))
            {
                tag = "eRem";
            }
            else if (contieneComprobantes(comprobante, "ns1:eRem_Exp"))
            {
                tag = "eRem_Exp";
            }
            else if (contieneComprobantes(comprobante, "ns1:eResg"))
            {
                tag = "eResg";
            }

            return tag;
        }

        /// <summary>
        /// Comprueba si un elemento xml tiene el tag del comprobante
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public bool contieneComprobantes(XmlElement documento, string tag)
        {
            bool salida = true;

            try
            {
                //Verifica si un xml contiene un tag especifico
                XmlNodeList existeComprobantes = documento.GetElementsByTagName(tag);
                if (existeComprobantes.Count == 0)
                {
                    salida = false;
                }
            }
            catch
            {
                salida = false;
            }

            return salida;
        }

        private bool ValidarCFESeleccionado(string tipo, string serie, string numero, string cfe)
        {
            XmlDocument xmlDocumento = new XmlDocument();
            xmlDocumento.LoadXml(cfe);

            if (xmlDocumento.GetElementsByTagName("ns0:TipoCFE")[0].InnerText.Equals(tipo) && xmlDocumento.GetElementsByTagName("ns0:Serie")[0].InnerText.Equals(serie) && xmlDocumento.GetElementsByTagName("ns0:Nro")[0].InnerText.Equals(numero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
