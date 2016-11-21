using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Collections;
using System.Xml;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoComprobantes
    {
        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFECOMP y sus tablas de detalle
        /// @TFECOMPDET y @TFECOMPDET2.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="maestro"></param>
        /// <returns></returns>
        public bool AlmacenarMaestro(Comprobantes comprobante)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralData dataDetalle = null;
            GeneralDataCollection detalle = null;
            GeneralDataCollection detalle2 = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECOMP");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_VerComp", comprobante.Version);
                dataGeneral.SetProperty("U_RucRec", comprobante.RucReceptor.ToString());
                dataGeneral.SetProperty("U_RucEmi", comprobante.RucEmisor.ToString());
                dataGeneral.SetProperty("U_IdResp", comprobante.IdRespuesta.ToString());
                dataGeneral.SetProperty("U_NomArc", comprobante.NombreArchivo);
                dataGeneral.SetProperty("U_FeHoEnRe", comprobante.FechaHoraRecepcion);
                dataGeneral.SetProperty("U_IdEmi", comprobante.IdEmisor.ToString());
                dataGeneral.SetProperty("U_IdRec", comprobante.IdReceptor.ToString());
                dataGeneral.SetProperty("U_CanComSob", comprobante.CantidadComprobantesSobre.ToString());
                dataGeneral.SetProperty("U_CanComRes", comprobante.CantidadComprobantesResponden.ToString());
                dataGeneral.SetProperty("U_CanCFEAce", comprobante.CantidadCFEAceptados.ToString());
                dataGeneral.SetProperty("U_CanCFERec", comprobante.CantidadCFERechazados.ToString());
                dataGeneral.SetProperty("U_CanCFCAce", comprobante.CantidadCFCAceptados.ToString());
                dataGeneral.SetProperty("U_CanCFCObs", comprobante.CantidadCFCObservados.ToString());
                dataGeneral.SetProperty("U_CanOTRRec", comprobante.CantidadOtrosRechazados.ToString());
                dataGeneral.SetProperty("U_FeHoFiEl", comprobante.FechaHoraFirma);

                detalle2 = dataGeneral.Child("TFECOMPDET2");

                //Agregar datos a la tabla de detalle
                foreach (DetComprobanteGlosa detalleGlosa in comprobante.DetalleGlosa)
                {
                    dataDetalle = detalle2.Add();
                    dataDetalle.SetProperty("U_CodMotRec", detalleGlosa.CodigoMotivoRechazo);
                    dataDetalle.SetProperty("U_GloMotRec", detalleGlosa.GlosaMotivo);
                    dataDetalle.SetProperty("U_DetRec", detalleGlosa.DetalleRechazo);
                }

                detalle = dataGeneral.Child("TFECOMPDET");

                //Agregar datos a la tabla de detalle
                foreach (DetComprobante detalleComprobante in comprobante.DetalleComprobante)
                {
                    dataDetalle = detalle.Add();
                    dataDetalle.SetProperty("U_NroOrd", detalleComprobante.NumeroOrdinal.ToString());
                    dataDetalle.SetProperty("U_TipoCFE", detalleComprobante.TipoCFE.ToString());
                    dataDetalle.SetProperty("U_SerComp", detalleComprobante.SerieComprobante);
                    dataDetalle.SetProperty("U_NumComp", detalleComprobante.NumeroComprobante.ToString());
                    dataDetalle.SetProperty("U_FecComp", detalleComprobante.FechaComprobante);
                    dataDetalle.SetProperty("U_FecFirEle", detalleComprobante.FechaHoraFirma);
                    dataDetalle.SetProperty("U_EstRec", detalleComprobante.EstadoRecepcion);
                    dataDetalle.SetProperty("U_TipoRec", detalleComprobante.TipoReceptor.ToString());
                }

                //Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
               // SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error almacenar maestro " + ex.ToString());
            }
            finally
            {
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (dataDetalle != null)
                {
                    //Liberar memoria utlizada por el objeto dataDetalle
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataDetalle);
                    System.GC.Collect();
                }
                if (detalle != null)
                {
                    //Liberar memoria utlizada por el objeto detalle
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(detalle);
                    System.GC.Collect();
                }
                if (detalle2 != null)
                {
                    //Liberar memoria utlizada por el objeto detalle2
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(detalle2);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Metodo para obtener la informacion de un comprobante rechazado
        /// </summary>
        /// <param name="serieCertificado"></param>
        /// <param name="numComprobante"></param>
        /// <returns></returns>
        public string obtenerCertificadoRechazado(string serieCertificado, string numComprobante, string tipoCFE, string tipoReceptor)
        {
            string resultado = "", consulta = "";
            Recordset obtenerCertificado = null;

            try
            {
                //Obtener objeto estadar de record set
                obtenerCertificado = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT dc.U_NumComp AS 'Número Comprobante', dc.U_SerComp AS 'Serie Comprobante'," +
                           " dg.U_CodMotRec AS 'Código Motivo Rechazo', dg.U_GloMotRec AS 'Glosa Motivo Rechazo', " +
                           " dg.U_DetRec AS 'Detalle Rechazo' FROM [@TFECOMPDET2] AS dg INNER JOIN [@TFECOMPDET] AS dc " +
                           " ON dg.DocEntry = dc.DocEntry AND dc.U_NumComp = '" + numComprobante + "' and dc.U_SerComp = " +
                           " '" + serieCertificado + "' AND dc.U_TipoCFE = '" + tipoCFE + "' and dc.U_TipoRec='" + tipoReceptor + "'";

                //Ejecutar consulta 
                obtenerCertificado.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                if (obtenerCertificado.RecordCount > 0)
                {
                    resultado = consulta;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (obtenerCertificado != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obtenerCertificado);
                    GC.Collect();
                }
            }
            return resultado;
        }
    }
}