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
    class ManteUdoCertificadoAnulado
    {
        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFECEANU y su hija
        /// @TFECEANUDET.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="maestro"></param>
        /// <returns></returns>
        public bool AlmacenarMaestro(Anulado certRechazado)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralData dataDetalle = null;
            GeneralDataCollection detalle = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECEANU");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_SerieCert", certRechazado.SerieCertificado);
                dataGeneral.SetProperty("U_NumDoc", certRechazado.NumeroDocumento);
                dataGeneral.SetProperty("U_Version", certRechazado.Version);
                dataGeneral.SetProperty("U_RucEmisor", certRechazado.RucEmisor);
                dataGeneral.SetProperty("U_RucRecep", certRechazado.RucReceptor);
                dataGeneral.SetProperty("U_CantComp", certRechazado.CantidadComprobantes);
                dataGeneral.SetProperty("U_FeHoFir", certRechazado.FechaHoraFirma);

                detalle = dataGeneral.Child("TFECEANUDET");

                foreach (DetAnulado razonRechazo in certRechazado.DetalleRechazo)
                {
                    dataDetalle = detalle.Add();
                    dataDetalle.SetProperty("U_TipoCFE", razonRechazo.TipoCFE);
                    dataDetalle.SetProperty("U_SerieComp", razonRechazo.SerieComprobante);
                    dataDetalle.SetProperty("U_NumComp", razonRechazo.NumeroComprobante);
                    dataDetalle.SetProperty("U_FecComp", razonRechazo.FechaComprobante);
                    dataDetalle.SetProperty("U_CodAnu", razonRechazo.CodigoAnulacion);
                    dataDetalle.SetProperty("U_GlosaDoc", razonRechazo.GlosaRechazo);
                }

                ////Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
                servicioGeneral.Add(dataGeneral);                

                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (servicioGeneral != null)
                {
                    ////Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    ////Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (dataDetalle != null)
                {
                    ////Liberar memoria utlizada por el objeto dataDetalle
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataDetalle);
                    System.GC.Collect();
                }
                if (detalle != null)
                {
                    ////Liberar memoria utlizada por el objeto detalle
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(detalle);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Actualiza los registros a la tabla @TFECEANU 
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="certificadosAnulados"></param>
        /// <returns></returns>
        public bool ActualizarMaestro(ArrayList certificadosAnulados)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECEANU");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                foreach (Anulado anulado in certificadosAnulados)
                {
                    //Establecer parametros
                    parametros.SetProperty("DocEntry", anulado.DocEntry);

                    //Apuntar al udo que corresponde con los parametros
                    dataGeneral = servicioGeneral.GetByParams(parametros);

                    //Establecer los valores para cada una de las propiedades del udo
                    dataGeneral.SetProperty("U_Corregido", anulado.CorregidoCon);

                    ////Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
                    servicioGeneral.Update(dataGeneral);
                }
                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (servicioGeneral != null)
                {
                    ////Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    ////Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (parametros != null)
                {
                    ////Liberar memoria utlizada por el objeto parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
            }
            return resultado;
        }
    }
}
