using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SEICRY_FE_UYU_9.Objetos.ISO3166
{
    /// <summary>
    /// Esta clase realiza validaciones sobre el xml ISO3166 - Codigos de paises
    /// </summary>
    public static class ValidacionISO3166
    {
        /// <summary>
        /// Valida que el codigo de un pais en estandar Alfa2, Alfa3 o Numerico exista segun estandar ISO 3166.
        /// </summary>
        /// <param name="codigoPaisAlfa2"></param>
        /// <param name="codigoPasiAlfa3"></param>
        /// <param name="codigoPaisNumerico"></param>
        /// <returns></returns>
        public static bool ValidarCodigoPais(string codigoPaisAlfa2, string codigoPasiAlfa3 = "", int codigoPaisNumerico = 0)
        {
            bool salida = false;

            try
            {

                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.Load(@"Certificados\ISO3166\ISO3166.xml");

                XmlNodeList listaAlfa2 = xmlDocumento.GetElementsByTagName("alfa2");
                XmlNodeList listaAlfa3 = xmlDocumento.GetElementsByTagName("alfa3");
                XmlNodeList listaNumerico = xmlDocumento.GetElementsByTagName("numerico");

                foreach (XmlElement nodo in listaAlfa2)
                {
                    if (nodo.InnerText == codigoPaisAlfa2)
                    {
                        salida = true;
                        break;
                    }
                }

                if (codigoPasiAlfa3 != "")
                {
                    foreach (XmlElement nodo in listaAlfa3)
                    {
                        if (nodo.InnerText == codigoPasiAlfa3)
                        {
                            salida = true;
                            break;
                        }
                    }
                }

                if (codigoPaisNumerico != 0)
                {
                    foreach (XmlElement nodo in listaNumerico)
                    {
                        if (nodo.InnerText == codigoPaisNumerico.ToString())
                        {
                            salida = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                salida = false;
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ValidacionISO3166/Error: " + ex.ToString());
            }
            
            return salida;
        }
    }
}
