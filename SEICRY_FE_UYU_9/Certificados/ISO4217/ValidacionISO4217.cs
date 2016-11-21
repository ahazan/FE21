using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SEICRY_FE_UYU_9.Objetos.ISO4217
{
    /// <summary>
    /// Esta clase realiza validaciones sobre el xml ISO4217 - Codigos de monedas
    /// </summary>
    public static class ValidacionISO4217
    {
        /// <summary>
        /// Valida que el codigo de un moneda exista segun estandar ISO 4217.
        /// </summary>
        /// <param name="tipoModena"></param>
        /// <returns></returns>
        public static bool ValidarCodigoMoneda(string tipoModena)
        {
            bool salida = false;

            try
            {
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.Load(@"Certificados\ISO4217\ISO4217.xml");

                XmlNodeList listaCcy = xmlDocumento.GetElementsByTagName("Ccy");

                foreach (XmlElement nodo in listaCcy)
                {
                    if (nodo.InnerText == tipoModena)
                    {
                        salida = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                salida = false;
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ValidacionISO4217/Error: " + ex.ToString());
            }

            return salida;
        }
    }
}
