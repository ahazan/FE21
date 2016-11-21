using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using SEICRY_FE_UYU_9.Objetos;
using System.Runtime.Serialization;

namespace SEICRY_FE_UYU_9.Certificados.Xml.Serializacion
{
    /// <summary>
    /// Realiza las operaciones de serializacion de los certificados y sobres.
    /// </summary>
    public static class ProcSerializacion
    {
        /// <summary>
        /// Crea un xml previo para un certificado
        /// </summary>
        /// <param name="cfe"></param>
        /// <returns></returns>
        public static string CrearXmlCFE(CFE cfe)
        {
            List<CFE> listaCfe = new List<CFE>();

            listaCfe.Add(cfe);

            try
            {
                //Crear objeto serializador
                XmlSerializer serializer = new XmlSerializer(typeof(List<CFE>));

                //Serializar lista de objetos  y obtener texto
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, listaCfe);

                    return writer.ToString();
                }
            }
            catch (SerializationException ex)
            {
                return ex.Message.ToString();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: "+ ex.ToString());
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Crea un certificado previo para un sobre
        /// </summary>
        /// <param name="sobre"></param>
        /// <returns></returns>
        public static string CrearXmlSobre(Sobre sobre)
        {
            List<Sobre> listaSobre = new List<Sobre>();

            listaSobre.Add(sobre);

            try
            {
                //Crear objeto serializador
                XmlSerializer serializer = new XmlSerializer(typeof(List<Sobre>));                
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("ISO-8859-1");               

                //Serializar lista de objetos  y obtener texto
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, listaSobre);

                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Crea un certificado previo para un reporte diario
        /// </summary>
        /// <param name="rptd"></param>
        /// <returns></returns>
        public static string CrearXmlRPTD(RPTD rptd)
        {
            string xml = "";

            try
            {
                //Encabezado del xml
                xml += "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";

                //Inicio del reporte
                xml += "<ns1:Reporte xmlns:ns1=\"http://cfe.dgi.gub.uy\">";

                //Inicio de la caratula
                xml += "<ns1:Caratula version=\"1.0\">";

                xml += String.Format("<ns1:RUCEmisor>{0}</ns1:RUCEmisor>", rptd.RucEmisor);
                xml += String.Format("<ns1:FechaResumen>{0}</ns1:FechaResumen>", rptd.FechaResumen);
                xml += String.Format("<ns1:SecEnvio>{0}</ns1:SecEnvio>", rptd.SecuenciaEnvio);
                xml += String.Format("<ns1:TmstFirmaEnv>{0}</ns1:TmstFirmaEnv>", rptd.FechaHoraFirma);
                xml += String.Format("<ns1:CantComprobantes>{0}</ns1:CantComprobantes>", rptd.CantComprobantes);
                
                //Fin de la caratula
                xml += "</ns1:Caratula>";

                //Resumen
                foreach (RPTDResumen resumen in rptd.RptdResumen)
                {
                    switch (resumen.TipoCFE)
                    {
                        case RPTDResumen.ESTipoCFECFC.ETicket:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Tck", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.NCETicket:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Tck_Nota_Credito", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.NDETicket:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Tck_Nota_Debito", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.EFactura:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Fac", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.NCEFactura:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Fac_Nota_Credito", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.NDEFactura:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Fac_Nota_Debito", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.ERemito:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Rmt", resumen);
                            break;
                        case RPTDResumen.ESTipoCFECFC.EResguardo:
                            xml += ObtenerResumenTipoDocumento("ns1:Rsmn_Rgd", resumen);
                            break;
                    }
                }

                //Fin del reporte
                xml += "</ns1:Reporte>";
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: "+ ex.ToString());
                return ex.Message.ToString();
            }

            return xml;
        }

        /// <summary>
        /// Retorna un objeto resumen de forma estructurada
        /// </summary>
        /// <param name="descTipo"></param>
        /// <param name="resumen"></param>
        /// <returns></returns>
        public static string ObtenerResumenTipoDocumento(string descTipo, RPTDResumen resumen)
        {
            string xmlResumen = "";
            
            //Tipo del resumen
            xmlResumen += String.Format("<{0}>", descTipo);

            //Tipo del documento
            xmlResumen += String.Format("<ns1:TipoComp>{0}</ns1:TipoComp>", resumen.TipoCFEInt);

            //Inicio de datos del resumen
            xmlResumen += "<ns1:RsmnData>";

            //Inicio de Montos
            xmlResumen += "<ns1:Montos>";

            foreach (RPTDResumenMontos montos in resumen.Montos)
            {
                xmlResumen += "<ns1:Mnts_FyT_Item>";

                xmlResumen += String.Format("<ns1:Fecha>{0}</ns1:Fecha>", montos.FechaComprobante);
                xmlResumen += String.Format("<ns1:CodSuc>{0}</ns1:CodSuc>", montos.CodigoCasaPrincipal);
                xmlResumen += String.Format("<ns1:TotMntNoGrv>{0}</ns1:TotMntNoGrv>", montos.TotalMontoNoGravado);
                xmlResumen += String.Format("<ns1:TotMntExpyAsim>{0}</ns1:TotMntExpyAsim>", montos.TotalMontoExportacionAsimilados);
                xmlResumen += String.Format("<ns1:TotMntImpPerc>{0}</ns1:TotMntImpPerc>", montos.TotalMontoImpuestoPercibido);
                xmlResumen += String.Format("<ns1:TotMntIVAenSusp>{0}</ns1:TotMntIVAenSusp>", montos.TotalMontoIVASuspenso);
                xmlResumen += String.Format("<ns1:TotMntIVATasaMin>{0}</ns1:TotMntIVATasaMin>", montos.TotalMontoIVATasaMinima);
                xmlResumen += String.Format("<ns1:TotMntIVATasaBas>{0}</ns1:TotMntIVATasaBas>", montos.TotalMontoIVATasaBasica);
                xmlResumen += String.Format("<ns1:TotMntIVAOtra>{0}</ns1:TotMntIVAOtra>", montos.TotalMontoIVAOtraTasa);
                xmlResumen += String.Format("<ns1:MntIVATasaMin>{0}</ns1:MntIVATasaMin>", montos.TasaMinimaIVA);
                xmlResumen += String.Format("<ns1:MntIVATasaBas>{0}</ns1:MntIVATasaBas>", montos.TasaBasicaIVA);
                xmlResumen += String.Format("<ns1:TotMntTotal>{0}</ns1:TotMntTotal>", montos.TotalMontoTotal);
                xmlResumen += String.Format("<ns1:TotMntRetenido>{0}</ns1:TotMntRetenido>", montos.TotalMontoRetenido);

                xmlResumen += "</ns1:Mnts_FyT_Item>";
            }

            //Fin de montos
            xmlResumen += "</ns1:Montos>";

            //Comprobantes utilizados
            xmlResumen += String.Format("<ns1:CantDocsUtil>{0}</ns1:CantDocsUtil>", resumen.CantCFEUtilizados);
            xmlResumen += String.Format("<ns1:CantDocsMay_topeUI>{0}</ns1:CantDocsMay_topeUI>", resumen.CantComprobantesMayorLimite);
            xmlResumen += String.Format("<ns1:CantDocsAnulados>{0}</ns1:CantDocsAnulados>", resumen.CantComprobantesAnulados);
            xmlResumen += String.Format("<ns1:CantDocsEmi>{0}</ns1:CantDocsEmi>", resumen.CantCFEEmitidos);

            //Inicio de rangos utlizados
            xmlResumen += "<ns1:RngDocsUtil>";

            foreach (RPTDResumenCFEUtil rangosUtilizados in resumen.CfeUtilizados)
            {
                xmlResumen += "<ns1:RDU_Item>";

                xmlResumen += String.Format("<ns1:Serie>{0}</ns1:Serie>", rangosUtilizados.Serie);
                xmlResumen += String.Format("<ns1:NroDesde>{0}</ns1:NroDesde>", rangosUtilizados.NumInicialUtilizado);
                xmlResumen += String.Format("<ns1:NroHasta>{0}</ns1:NroHasta>", rangosUtilizados.NumFinalUtilizado);

                xmlResumen += "</ns1:RDU_Item>";
            }

            //Fin de rangos utilizados
            xmlResumen += "</ns1:RngDocsUtil>";

            //Inicio de rangos anulados
            xmlResumen += "<ns1:RngDocsAnulados>";

            foreach (RPTDResumenCFEAnul rangosAnulados in resumen.CfeAnulados)
            {
                xmlResumen += "<ns1:RDA_Item>";

                xmlResumen += String.Format("<ns1:Serie>{0}</ns1:Serie>", rangosAnulados.Serie);
                xmlResumen += String.Format("<ns1:NroDesde>{0}</ns1:NroDesde>", rangosAnulados.NumInicialAnulado);
                xmlResumen += String.Format("<ns1:NroHasta>{0}</ns1:NroHasta>", rangosAnulados.NumFinalAnulado);

                xmlResumen += "</ns1:RDA_Item>";
            }

            //Fin de rangos anulados
            xmlResumen += "</ns1:RngDocsAnulados>";

            //Fin de datos del resument
            xmlResumen += "</ns1:RsmnData>";

            //Fin de tipo de resumen
            xmlResumen += String.Format("</{0}>", descTipo); 

            return xmlResumen;
        }
    }
}
