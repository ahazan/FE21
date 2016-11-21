using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using SEICRY_FE_UYU_9.Globales;
using System.Security.Cryptography;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.XSD
{     
    class ValidarSobre
    {
        SAPbouiCOM.Application app = SAPbouiCOM.Framework.Application.SBO_Application;

        //XmlTextReader para cada XSD a cargar
        static XmlTextReader trEnvioSobre = null, trEempresas = null, trTipoCFE = null, trTipoDGI = null;
        //Esquema para cargar los XSD
        static XmlSchemaCollection xsc = null;
        //static XmlSchemaSet xsc2 = null;

        //Contienen los errores de formato encontrados en el momento de realizar la validacion
        public static List<string> erroresEncontrados = new List<string>();
        //Numero de errores encontrados en el momento de realizar la validacion
        static int numErrores = 0;

        /// <summary>
        /// Metodo para cargar los XSD(Solo se cargan una vez debido a que el proceso tarda aprox 2 minutos)
        /// </summary>
        private void cargarXSD()
        {


            #region ENVIO ENTRE EMPRESAS

            if (trEnvioSobre == null)
            {
                trEnvioSobre = new XmlTextReader(RutasXSD.RutaXsdEnvioCFEentreEmpresa);
            }

            if (trEempresas == null)
            {
                trEempresas = new XmlTextReader(RutasXSD.RutaXsdCFEEmpresasType);
            }

            #endregion ENVIO ENTRE EMPRESAS


            #region ENVIO CON DGI

            //if (trEnvioSobre == null)
            //{                
            //    trEnvioSobre = new XmlTextReader(RutasXSD.RutaXsdEnvioSobre);
            //} 

            #endregion ENVIO CON DGI

            #region XSD HOJAS(SIEMPRE SE USAN)

            if (trTipoDGI == null)
            {
                trTipoDGI = new XmlTextReader(RutasXSD.RutaXsdTipoDgi);
            }
            if (trTipoCFE == null)
            {
                trTipoCFE = new XmlTextReader(RutasXSD.RutaXsdTipoCfe);
            } 

            #endregion XSD HOJAS(SIEMPRE SE USAN)
            
            if(xsc == null)
            {
                xsc = new XmlSchemaCollection();               

                try
                {
                    //Se cargan los XSD al esquema
                    xsc.Add(null, trTipoDGI);
                    xsc.Add(null, trTipoCFE);
                    xsc.Add(null, trEempresas);//no se usa con envio a DGI
                    xsc.Add(null, trEnvioSobre);
                }
                catch(Exception ex)
                {
                    app.MessageBox("ValidarSobre/CArgarXSD/Error: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Metodo que valida un xml contra los XSD
        /// </summary>
        /// <param name="contenidoSobre"></param>
        /// <returns></returns>
        public bool ValidarXsdSobre(string contenidoSobre)
        {           
            bool resultado = false;
            XmlValidatingReader validar = null;           
            XmlDocument xmlDocumento = new XmlDocument();
            
            try
            {
                xmlDocumento.LoadXml(contenidoSobre);
                
                XmlNodeList listaNodosAdenda = xmlDocumento.GetElementsByTagName("Adenda");

                if (listaNodosAdenda.Count == 0)
                {
                    string tag = ObtenerTagAdenda(xmlDocumento);
                    listaNodosAdenda = xmlDocumento.GetElementsByTagName(tag + "Adenda");
                }

                foreach (XmlElement nodo in listaNodosAdenda)
                {
                    nodo.RemoveAll();
                }

                contenidoSobre = xmlDocumento.InnerXml.Replace("<Adenda></Adenda>", "");

                erroresEncontrados = new List<string>();

                //Carga el XSD
                cargarXSD();

                //Se inicializa el objeto con el string del xml
                validar = new XmlValidatingReader(contenidoSobre, XmlNodeType.Document, null);

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
            catch(Exception ex)
            {
                app.MessageBox("ValidarSobre/ValidarXSdSobre/Error: " + ex.ToString());
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

        /// <summary>
        /// Metodo para generar un token binario(Tipo no base2)
        /// </summary>
        /// <param name="idReceptor"></param>
        /// <returns></returns>
        public string generarTokenBinario(string consecutivo)
        {
            string resultado = "";

            //Se inicializa el proveedor de servicio
            SHA512CryptoServiceProvider proServ = new SHA512CryptoServiceProvider();

            try
            {
                //Se obtienen los bytes del consecutivo
                byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(consecutivo);
                //Se aplica el hash
                byte[] tokBytes = proServ.ComputeHash(tokenBytes);
                //Se limpia el proveedor de servicios
                proServ.Clear();

                resultado = Convert.ToBase64String(tokBytes);
            }
            catch(Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Generar consecutivo
        /// </summary>
        /// <returns></returns>
        public string generarIdReceptor()
        {
            string resultado = "";

            ManteUdoConsecutivo manteConsecutivo = new ManteUdoConsecutivo();
            //Se obtiene 
            string consecutivo = manteConsecutivo.ObtenerConsecutivoAnterior();

            if (consecutivo.Equals(""))
            {
                resultado = "00000001";
            }
            else
            {
                try 
                {
                    double consec = Convert.ToDouble(consecutivo);
                    consec += 1;
                    resultado = agregarCeros(consec, 8);
                }
                catch(Exception)
                {
                }
            }

            return resultado;
        }

        /// <summary>
        /// Metodo para agregar ceros a la izquierda de un string
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cantCeros"></param>
        /// <returns></returns>
        private string agregarCeros(double num, int cantCeros)
        {
            string resultado = "", temp = "";
            int j = 0, cant = 0;

            try
            {
                temp = Convert.ToString(num);
                cant = temp.Length;

                while (j < (8 - cant))
                {
                   temp = "0" + temp;
                    j++;
                }

                resultado = temp;
            }
            catch (Exception ex)
            {
                app.MessageBox("ValidarSobre/AgregarCeros/Error: " + ex.ToString());
            }

            return resultado;
        }

        /// <summary>
        /// Valida el ruc del sobre contra los ruc de los comprobantes
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public bool validarRucSobreContraComprobante(XmlDocument documento, List<ErrorValidarSobre> listaErrores, string tag)
        {
            string rucSobre = "";
            bool salida = true;

            try
            {
                //Se obtiene el rucEmisor del sobre
                //rucSobre = documento.GetElementsByTagName("DGICFE:RUCEmisor").Item(0).InnerText;
                XmlNodeList rucSobreXNL = documento.GetElementsByTagName("RUCEmisor");

                if (rucSobreXNL.Count > 0)
                {
                    rucSobre = documento.GetElementsByTagName("RUCEmisor").Item(0).InnerText;
                }
                else
                {
                    rucSobreXNL = documento.GetElementsByTagName("DGICFE:RUCEmisor");

                    if (rucSobreXNL.Count > 0)
                    {
                        rucSobre = documento.GetElementsByTagName("DGICFE:RUCEmisor").Item(0).InnerText;
                    }

                    rucSobreXNL = documento.GetElementsByTagName("cfe:RUCEmisor");

                    if (rucSobreXNL.Count > 0)
                    {
                        rucSobre = documento.GetElementsByTagName("cfe:RUCEmisor").Item(0).InnerText;
                    }
                }

                tag = "";

                XmlNodeList listaRucEmisores = documento.GetElementsByTagName(tag + "RUCEmisor");

                if (listaRucEmisores.Count == 0)
                {
                    listaRucEmisores = documento.GetElementsByTagName("ns0:RUCEmisor");
                }

                foreach (XmlElement rucEmisor in listaRucEmisores)
                {
                    if (!rucSobre.Equals(rucEmisor.InnerText))
                    {
                        ErrorValidarSobre error = new ErrorValidarSobre();
                        error.CodigoRechazo = Mensaje.errSobValRucCod;
                        error.DetalleRechazo = Mensaje.errSobValRucDetGlo;
                        error.GlosaRechazo = Mensaje.errSobValRucDetGlo;
                        listaErrores.Add(error);
                        salida = false;
                    }
                }
            }
            catch (Exception ex)
            {
                app.MessageBox("ValidarSobre/ValRucContrasobre/Error: "+ ex.ToString());
                salida = false;
            }

            return salida;
        }

        /// <summary>
        /// Valida que la cantidad Cfes especificados en la linea DGICFE:CantCFE 
        /// corresponda con la cantidad de Cfes dentro del sobre
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public bool validarCantidadCfe(XmlDocument documento, List<ErrorValidarSobre> listaErrores, string tag)
        {
            bool salida = true;
            int cantCfe = 0, cfesEncontrados = 0;
            XmlNodeList listaFacturas = null;
            XmlNodeList listaTickets = null;
            XmlNodeList listaRemito = null;
            XmlNodeList listaResguardo = null;

            try
            {
                //Se obtiene el rucEmisor del sobre
                XmlNodeList cantCfeNL = documento.GetElementsByTagName("CantCFE");

                if (cantCfeNL.Count > 0)
                {
                    cantCfe = Convert.ToInt16(documento.GetElementsByTagName("CantCFE").Item(0).InnerText);
                    tag = "";
                }
                else
                {
                    cantCfeNL = documento.GetElementsByTagName("DGICFE:CantCFE");

                    if (cantCfeNL.Count > 0)
                    {
                        cantCfe = Convert.ToInt16(documento.GetElementsByTagName("DGICFE:CantCFE").Item(0).InnerText);
                        tag = "DGICFE:";
                    }

                    cantCfeNL = documento.GetElementsByTagName("cfe:CantCFE");

                    if (cantCfeNL.Count > 0)
                    {
                        cantCfe = Convert.ToInt16(documento.GetElementsByTagName("cfe:CantCFE").Item(0).InnerText);
                        tag = "cfe:";
                    }                    
                }

                tag = "";
                if (contieneComprobantes(documento, tag + "eFact"))
                {
                    listaFacturas = documento.GetElementsByTagName(tag + "eFact");
                    foreach (XmlElement factura in listaFacturas)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "ns0:";
                if (contieneComprobantes(documento, tag + "eFact"))
                {
                    listaFacturas = documento.GetElementsByTagName(tag + "eFact");
                    foreach (XmlElement factura in listaFacturas)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "DGICFE:";
                if (contieneComprobantes(documento, tag + "eFact"))
                {
                    listaFacturas = documento.GetElementsByTagName(tag + "eFact");
                    foreach (XmlElement factura in listaFacturas)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "";
                if (contieneComprobantes(documento, tag + "eTck"))
                {
                    listaTickets = documento.GetElementsByTagName(tag +"eTck");
                    foreach (XmlElement ticket in listaTickets)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "ns0:";
                if (contieneComprobantes(documento, tag + "eTck"))
                {
                    listaTickets = documento.GetElementsByTagName(tag + "eTck");
                    foreach (XmlElement ticket in listaTickets)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "DGICFE:";
                if (contieneComprobantes(documento, tag + "eTck"))
                {
                    listaTickets = documento.GetElementsByTagName(tag + "eTck");
                    foreach (XmlElement ticket in listaTickets)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "";
                if (contieneComprobantes(documento, tag + "eRem"))
                {
                    listaRemito = documento.GetElementsByTagName(tag + "eRem");
                    foreach (XmlElement remito in listaRemito)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "ns0:";
                if (contieneComprobantes(documento, tag + "eRem"))
                {
                    listaRemito = documento.GetElementsByTagName(tag + "eRem");
                    foreach (XmlElement remito in listaRemito)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "DGICFE:";
                if (contieneComprobantes(documento, tag + "eRem"))
                {
                    listaRemito = documento.GetElementsByTagName(tag + "eRem");
                    foreach (XmlElement remito in listaRemito)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "";
                if (contieneComprobantes(documento, tag + "eResg"))
                {
                    listaResguardo = documento.GetElementsByTagName(tag + "eResg");
                    foreach (XmlElement resguardo in listaResguardo)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "ns0:";
                if (contieneComprobantes(documento, tag + "eResg"))
                {
                    listaResguardo = documento.GetElementsByTagName(tag + "eResg");
                    foreach (XmlElement resguardo in listaResguardo)
                    {
                        cfesEncontrados++;
                    }
                }

                tag = "DGICFE:";
                if (contieneComprobantes(documento, tag + "eResg"))
                {
                    listaResguardo = documento.GetElementsByTagName(tag + "eResg");
                    foreach (XmlElement resguardo in listaResguardo)
                    {
                        cfesEncontrados++;
                    }
                }

                if (cfesEncontrados != cantCfe)
                {
                    salida = false;
                    ErrorValidarSobre error = new ErrorValidarSobre();
                    error.CodigoRechazo = Mensaje.errSobValCantCfeCod;
                    error.DetalleRechazo = Mensaje.errSobValCantCfeDetGlo;
                    error.GlosaRechazo = Mensaje.errSobValCantCfeDetGlo;
                    listaErrores.Add(error);
                }
            }
            catch (Exception ex)
            {
                app.MessageBox("ValidarSobre/ValidarCantCfe/Error: " + ex.ToString());
                salida = false;
            }

            return salida;
        }

        /// <summary>
        /// Comprueba si un documento xml tiene el tag del comprobante
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public bool contieneComprobantes(XmlDocument documento, string tag)
        {
            bool salida = true;

            try
            {
                XmlNodeList existeComprobantes = documento.GetElementsByTagName(tag);
                if (existeComprobantes.Count == 0)
                {
                    salida = false;
                }
            }
            catch(Exception ex)
            {
                app.MessageBox("ValidarSobre/contieneComprobantes/Error: " + ex.ToString());
                salida = false;
            }

            return salida;
        }

        /// <summary>
        /// Obtiene el tag ns# para la adenda
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        private string ObtenerTagAdenda(XmlDocument documento)
        {
            string resultado = "";
            List<string> listaTags = new List<string> {"ns0:","ns1:", "ns2:", "ns3:", "ns4:", "ns5:", "ns6:", "ns7:", "cfe:", "CFE:" };

            try
            {
                foreach (string tag in listaTags)
                {
                  XmlNodeList nodoLista =  documento.GetElementsByTagName(tag + "Adenda");
                  if (nodoLista.Count > 0)
                  {
                      resultado = tag;
                      break;
                  }
                }
            }
            catch (Exception ex)
            {
                app.MessageBox("ValidarSobre/ObtenerTagAdenda/Error: " + ex.ToString());
            }

            return resultado;
        }
    }
}
