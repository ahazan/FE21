using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.ACKS
{
    class RespuestaCertificados
    {
        ManteUdoComprobantes manteUdoComprobantes = new ManteUdoComprobantes();
        ManteUdoSobreTransito manteUdoSobreTransito = new ManteUdoSobreTransito();
        ManteUdoCFE manteUdoCfe = new ManteUdoCFE();
        Anulacion anulacion = new Anulacion();

        /// <summary>
        /// Procesa la respuesta recibida para certificados
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <param name="tipoReceptor"></param>
        /// <param name="docEntry"></param>
        public void ProcesarRespuesta(string xmlRespuesta, CFE.ESTipoReceptor tipoReceptor, string docEntry)
        {
            try
            {
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlRespuesta);

                List<string> listaEstado = new List<string>();

                listaEstado.Add("ESTADO");
                listaEstado.Add("estado");
                listaEstado.Add("Estado");

                string estado = ObtenerTag(xmlDocumento, listaEstado);

                //Validar si el certificado fue aceptado
                if (xmlDocumento.GetElementsByTagName(estado).Item(0).InnerText.Equals("AE"))//Comprobante aceptado
                {
                    CertificadoAprobado(xmlDocumento, tipoReceptor, docEntry);
                }
                else if (xmlDocumento.GetElementsByTagName(estado).Item(0).InnerText.Equals("BE"))//Comprobante rechazado
                {
                    CertificadoRechazado(xmlDocumento, tipoReceptor, docEntry);

                    if (tipoReceptor == CFE.ESTipoReceptor.DGI)
                    {
                        anulacion.EnviarAnulacion(xmlDocumento);
                    }
                }
                else if (xmlDocumento.GetElementsByTagName(estado).Item(0).InnerText.Equals("CE"))//Comprobante observado
                {

                }
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error ProcesarRespuesta/ " + ex.ToString());
            }
        }

        /// <summary>
        /// Procesa la respuesta
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="tipoReceptor"></param>
        public void ProcesarRespuesta(string nombreArchivo, CFE.ESTipoReceptor tipoReceptor)
        {
            XmlDocument xmlDocumento = new XmlDocument();
            xmlDocumento.Load(RutasCarpetas.RutaCarpetaAcuseRecibidoCertificado + nombreArchivo);

            List<string> listaIDRespuesta = new List<string>();

            listaIDRespuesta.Add("IDRespuesta");
            listaIDRespuesta.Add("IdRespuesta");
            listaIDRespuesta.Add("IDRESPUESTA");
            listaIDRespuesta.Add("idrespuesta");
            listaIDRespuesta.Add("Idrespuesta");
            listaIDRespuesta.Add("idRespuesta");

            string idRespuesta = ObtenerTag(xmlDocumento, listaIDRespuesta);

            string docEntry = manteUdoSobreTransito.ConsultarDocEntry(xmlDocumento.GetElementsByTagName("").Item(0).InnerText);

            List<string> listaEstado = new List<string>();

            listaEstado.Add("ESTADO");
            listaEstado.Add("estado");
            listaEstado.Add("Estado");

            string estado = ObtenerTag(xmlDocumento, listaEstado);

            //Validar si el certificado fue aceptado
            if (xmlDocumento.GetElementsByTagName(estado).Item(0).InnerText.Equals("AE"))//Comprobante aceptado
            {
                CertificadoAprobado(xmlDocumento, tipoReceptor, docEntry);
            }
            else if (xmlDocumento.GetElementsByTagName(estado).Item(0).InnerText.Equals("BE"))//Comprobante rechazado
            {
                CertificadoRechazado(xmlDocumento, tipoReceptor, docEntry);

            }
            else if (xmlDocumento.GetElementsByTagName(estado).Item(0).InnerText.Equals("CE"))//Comprobante observado
            {

            }
        }

        /// <summary>
        /// Procesa la respuesta cuando el certificado esta rechazado
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <param name="docEntry"></param>
        private void CertificadoRechazado(XmlDocument xmlRespuesta, CFE.ESTipoReceptor tipoReceptor, string docEntry)
        {
            Comprobantes comprobante = new Comprobantes();
            DetComprobante detComprobantes;
            DetComprobanteGlosa detGlosa;

            XmlNodeList listaNodosCFE;
            XmlNodeList listaNodosMotivosRechazo;

            try
            {

                //Cabecera
                comprobante.Version = "1.0";
                comprobante.RucReceptor = long.Parse(xmlRespuesta.GetElementsByTagName("RUCReceptor").Item(0).InnerText);
                comprobante.RucEmisor = long.Parse(xmlRespuesta.GetElementsByTagName("RUCEmisor").Item(0).InnerText);
                comprobante.IdRespuesta = int.Parse(xmlRespuesta.GetElementsByTagName("IDRespuesta").Item(0).InnerText);
                comprobante.NombreArchivo = xmlRespuesta.GetElementsByTagName("NomArch").Item(0).InnerText;
                comprobante.FechaHoraRecepcion = xmlRespuesta.GetElementsByTagName("FecHRecibido").Item(0).InnerText;
                comprobante.IdEmisor = int.Parse(xmlRespuesta.GetElementsByTagName("IDEmisor").Item(0).InnerText);
                comprobante.IdReceptor = int.Parse(xmlRespuesta.GetElementsByTagName("IDReceptor").Item(0).InnerText);
                comprobante.CantidadComprobantesSobre = int.Parse(xmlRespuesta.GetElementsByTagName("CantenSobre").Item(0).InnerText);
                comprobante.CantidadComprobantesResponden = int.Parse(xmlRespuesta.GetElementsByTagName("CantResponden").Item(0).InnerText);
                comprobante.CantidadCFEAceptados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFEAceptados").Item(0).InnerText);
                comprobante.CantidadCFERechazados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFERechazados").Item(0).InnerText);
                comprobante.CantidadCFCAceptados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFCAceptados").Item(0).InnerText);
                comprobante.CantidadCFCObservados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFCObservados").Item(0).InnerText);
                comprobante.CantidadOtrosRechazados = int.Parse(xmlRespuesta.GetElementsByTagName("CantOtrosRechazados").Item(0).InnerText);
                comprobante.FechaHoraFirma = xmlRespuesta.GetElementsByTagName("Tmst").Item(0).InnerText;

                //Detalle
                listaNodosCFE = xmlRespuesta.GetElementsByTagName("ACKCFE_Det");

                foreach (XmlElement nodoComprobante in listaNodosCFE)
                {
                    detComprobantes = new DetComprobante();
                    detComprobantes.NumeroOrdinal = int.Parse(nodoComprobante.GetElementsByTagName("Nro_ordinal").Item(0).InnerText);
                    detComprobantes.TipoCFE = int.Parse(nodoComprobante.GetElementsByTagName("TipoCFE").Item(0).InnerText);
                    detComprobantes.SerieComprobante = nodoComprobante.GetElementsByTagName("Serie").Item(0).InnerText;
                    detComprobantes.NumeroComprobante = int.Parse(nodoComprobante.GetElementsByTagName("NroCFE").Item(0).InnerText);
                    detComprobantes.FechaComprobante = nodoComprobante.GetElementsByTagName("FechaCFE").Item(0).InnerText;
                    detComprobantes.FechaHoraFirma = nodoComprobante.GetElementsByTagName("TmstCFE").Item(0).InnerText;
                    detComprobantes.EstadoRecepcion = nodoComprobante.GetElementsByTagName("Estado").Item(0).InnerText;
                    detComprobantes.TipoReceptor = tipoReceptor;

                    comprobante.DetalleComprobante.Add(detComprobantes);

                    //Actualiza el estado del CFE en el listado principal

                    if (tipoReceptor == CFE.ESTipoReceptor.DGI)
                    {
                        manteUdoCfe.Actualizar(detComprobantes.TipoCFE, detComprobantes.SerieComprobante, detComprobantes.NumeroComprobante, CFE.ESEstadoCFE.RechazadoDGI, tipoReceptor);
                    }
                    else if (tipoReceptor == CFE.ESTipoReceptor.Receptor)
                    {
                        manteUdoCfe.Actualizar(detComprobantes.TipoCFE, detComprobantes.SerieComprobante, detComprobantes.NumeroComprobante, CFE.ESEstadoCFE.RechazadoReceptor, tipoReceptor);
                    }


                    //Motivos de rechazo
                    listaNodosMotivosRechazo = nodoComprobante.GetElementsByTagName("MotivosRechazoCF");

                    foreach (XmlElement nodoRechazo in listaNodosMotivosRechazo)
                    {
                        detGlosa = new DetComprobanteGlosa();
                        detGlosa.CodigoMotivoRechazo = nodoRechazo.GetElementsByTagName("Motivo").Item(0).InnerText;
                        detGlosa.GlosaMotivo = nodoRechazo.GetElementsByTagName("Glosa").Item(0).InnerText;
                        detGlosa.DetalleRechazo = nodoRechazo.GetElementsByTagName("Detalle").Item(0).InnerText;

                        comprobante.DetalleGlosa.Add(detGlosa);
                    }
                }

                //Almacenar en monitor de sobres
                bool respuestaAlmacenar = manteUdoComprobantes.AlmacenarMaestro(comprobante);

                if (respuestaAlmacenar)
                {
                    //Eliminar de sobres en transito
                   // manteUdoSobreTransito.Eliminar(docEntry);
                }
            }
            catch(Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("certiREcha " + ex.ToString());
            }
        }

        /// <summary>
        /// Procesa la respuesta cuando el certificado esta aprobado
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <param name="docEntry"></param>
        private void CertificadoAprobado(XmlDocument xmlRespuesta, CFE.ESTipoReceptor tipoReceptor, string docEntry)
        {
            Comprobantes comprobante = new Comprobantes();
            DetComprobante detComprobantes;

            XmlNodeList listaNodosCFE;

            try
            {
                //Cabecera
                comprobante.Version = "1.0";
                comprobante.RucReceptor = long.Parse(xmlRespuesta.GetElementsByTagName("RUCReceptor").Item(0).InnerText);
                comprobante.RucEmisor = long.Parse(xmlRespuesta.GetElementsByTagName("RUCEmisor").Item(0).InnerText);
                comprobante.IdRespuesta = int.Parse(xmlRespuesta.GetElementsByTagName("IDRespuesta").Item(0).InnerText);
                comprobante.NombreArchivo = xmlRespuesta.GetElementsByTagName("NomArch").Item(0).InnerText;
                comprobante.FechaHoraRecepcion = xmlRespuesta.GetElementsByTagName("FecHRecibido").Item(0).InnerText;
                comprobante.IdEmisor = int.Parse(xmlRespuesta.GetElementsByTagName("IDEmisor").Item(0).InnerText);
                comprobante.IdReceptor = int.Parse(xmlRespuesta.GetElementsByTagName("IDReceptor").Item(0).InnerText);
                comprobante.CantidadComprobantesSobre = int.Parse(xmlRespuesta.GetElementsByTagName("CantenSobre").Item(0).InnerText);
                comprobante.CantidadComprobantesResponden = int.Parse(xmlRespuesta.GetElementsByTagName("CantResponden").Item(0).InnerText);
                comprobante.CantidadCFEAceptados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFEAceptados").Item(0).InnerText);
                comprobante.CantidadCFERechazados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFERechazados").Item(0).InnerText);
                comprobante.CantidadCFCAceptados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFCAceptados").Item(0).InnerText);
                comprobante.CantidadCFCObservados = int.Parse(xmlRespuesta.GetElementsByTagName("CantCFCObservados").Item(0).InnerText);
                comprobante.CantidadOtrosRechazados = int.Parse(xmlRespuesta.GetElementsByTagName("CantOtrosRechazados").Item(0).InnerText);
                comprobante.FechaHoraFirma = xmlRespuesta.GetElementsByTagName("Tmst").Item(0).InnerText;

                //Detalle
                listaNodosCFE = xmlRespuesta.GetElementsByTagName("ACKCFE_Det");

                foreach (XmlElement nodoComprobante in listaNodosCFE)
                {
                    detComprobantes = new DetComprobante();
                    detComprobantes.NumeroOrdinal = int.Parse(nodoComprobante.GetElementsByTagName("Nro_ordinal").Item(0).InnerText);
                    detComprobantes.TipoCFE = int.Parse(nodoComprobante.GetElementsByTagName("TipoCFE").Item(0).InnerText);
                    detComprobantes.SerieComprobante = nodoComprobante.GetElementsByTagName("Serie").Item(0).InnerText;
                    detComprobantes.NumeroComprobante = int.Parse(nodoComprobante.GetElementsByTagName("NroCFE").Item(0).InnerText);
                    detComprobantes.FechaComprobante = nodoComprobante.GetElementsByTagName("FechaCFE").Item(0).InnerText;
                    detComprobantes.FechaHoraFirma = nodoComprobante.GetElementsByTagName("TmstCFE").Item(0).InnerText;
                    detComprobantes.EstadoRecepcion = nodoComprobante.GetElementsByTagName("Estado").Item(0).InnerText;

                    comprobante.DetalleComprobante.Add(detComprobantes);

                    if (tipoReceptor == CFE.ESTipoReceptor.DGI)
                    {
                        //Actualiza el estado del CFE en el listado principal
                        manteUdoCfe.Actualizar(detComprobantes.TipoCFE, detComprobantes.SerieComprobante, detComprobantes.NumeroComprobante, CFE.ESEstadoCFE.AprobadoDGI, CFE.ESTipoReceptor.DGI);
                    }
                    else if (tipoReceptor == CFE.ESTipoReceptor.Receptor)
                    {
                        //Actualiza el estado del CFE en el listado principal
                        manteUdoCfe.Actualizar(detComprobantes.TipoCFE, detComprobantes.SerieComprobante, detComprobantes.NumeroComprobante, CFE.ESEstadoCFE.AprobadoReceptor, CFE.ESTipoReceptor.Receptor);
                    }
                }

                //Almacenar en monitor de sobres
                bool respuestaAlmacenar = manteUdoComprobantes.AlmacenarMaestro(comprobante);

                if (respuestaAlmacenar)
                {
                    //Eliminar de sobres en transito
                   // manteUdoSobreTransito.Eliminar(docEntry);
                }
            }
            catch(Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("CertiApro " + ex.ToString());
            }
        }

        /// <summary>
        /// Obtiene el tag que trae el documento
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string ObtenerTag(XmlDocument ack, List<string> tags)
        {
            string resultado = "";

            try
            {                                
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
