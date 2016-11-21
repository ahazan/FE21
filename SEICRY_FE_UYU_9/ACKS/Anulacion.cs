using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.ACKS
{
    class Anulacion
    {
        public void EnviarAnulacion(XmlDocument xmlRespuesta)
        {
            string xmlAnulacion = "";

            xmlAnulacion += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xmlAnulacion += "<CFEs_Anulados_x_RechazoDGI version=\"1.0\" xmlns:ns1=\"http://cfe.dgi.gub.uy\">";
            xmlAnulacion += ObtenerCaratula(xmlRespuesta);
            xmlAnulacion += ObtenerCertificadosAnulados(xmlRespuesta);
            xmlAnulacion += "</CFEs_Anulados_x_RechazoDGI>";
        }

        /// <summary>
        /// Retorna los datos de caratula
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <returns></returns>
        private string ObtenerCaratula(XmlDocument xmlRespuesta)
        {
            DateTime dt = DateTime.Now;
            string xmlCaratula = "";
            ManteUdoConseIdComunicacion manteUdoConseIdComunicacion = new ManteUdoConseIdComunicacion();

            xmlCaratula += "<ns1:Caratula>";
            xmlCaratula += "<ns1:RUCEmisor>" + xmlRespuesta.GetElementsByTagName("RUCEmisor").Item(0).InnerText + "</ns1:RUCEmisor>";
            xmlCaratula += "<ns1:RUCReceptor>" + xmlRespuesta.GetElementsByTagName("RUCReceptor").Item(0).InnerText + "</ns1:RUCReceptor>";
            xmlCaratula += "<ns1:IDComunicacion>" + manteUdoConseIdComunicacion.GenerarIdComunicacion() + "</ns1:IDComunicacion>";
            xmlCaratula += "<ns1:CantCFEAnulados>" + ObtenerCantidadAnulados(xmlRespuesta) + "</ns1:CantCFEAnulados>";
            xmlCaratula += "<ns1:Tmst>" + dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") + "</ns1:Tmst>";
            xmlCaratula += "</ns1:Caratula>";

            return xmlCaratula;
        }

        /// <summary>
        /// Retorna los datos de los certificados anulados
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <returns></returns>
        private string ObtenerCertificadosAnulados(XmlDocument xmlRespuesta)
        {
            string xmlCertificados = "";

            XmlNodeList listaNodosCFE = xmlRespuesta.GetElementsByTagName("ACKCFE_Det");

            foreach (XmlElement nodo in listaNodosCFE)
            {
                xmlCertificados += "<ns1:CFEAnulado>";
                xmlCertificados += "<ns1:TipoCFE>" + nodo.GetElementsByTagName("TipoCFE").Item(0).InnerText + "</ns1:TipoCFE>";
                xmlCertificados += "<ns1:Serie>" + nodo.GetElementsByTagName("Serie").Item(0).InnerText + "</ns1:Serie>";
                xmlCertificados += "<ns1:NroCFE>" + nodo.GetElementsByTagName("NroCFE").Item(0).InnerText + "</ns1:NroCFE>";
                xmlCertificados += "<ns1:FechaCFE>" + nodo.GetElementsByTagName("FechaCFE").Item(0).InnerText + "</ns1:FechaCFE>";
                xmlCertificados += "<ns1:Motivo_Cod>RD</ns1:Motivo_Cod>";
                xmlCertificados += "<ns1:Motivo_Glosa>Comprobante anulado por rechazo de DGI</ns1:Motivo_Glosa>";
                xmlCertificados += "</ns1:CFEAnulado>";
            }

            return xmlCertificados;
        }

        /// <summary>
        /// Retorna la cantidad de ceritifcados anulados
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <returns></returns>
        private int ObtenerCantidadAnulados(XmlDocument xmlRespuesta)
        {
            XmlNodeList listaNodosCFE = xmlRespuesta.GetElementsByTagName("ACKCFE_Det");
            int i = 0;
            int cantidadAnulados = 0;

            foreach (XmlElement nodo in listaNodosCFE)
            {
                if (nodo.GetElementsByTagName("Estado").Item(i).InnerText.Equals("BE"))
                {
                    cantidadAnulados++;
                }

                i++;
            }

            return cantidadAnulados;
        }
    }
}
