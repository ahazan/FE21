using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Collections;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para realizar las operaciones de adminstracion de retencion/percepcion
    /// </summary>
    class ManteUdoRazonReferencia
    {
        /// <summary>
        /// Almacena un nuevo registro en la tabla de razones de referencia
        /// </summary>
        /// <param name="listaRazones"></param>
        /// <returns></returns>
        public bool Almacenar(List<RazonReferencia> listaRazones)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFERZR");
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                foreach (RazonReferencia razon in listaRazones)
                {
                    dataGeneral.SetProperty("U_Codigo", razon.CodigoRazon);
                    dataGeneral.SetProperty("U_Razon", razon.RazonReferenciaNC);

                    servicioGeneral.Add(dataGeneral);
                }
                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        // <summary>
        // Elimina todos los registros de retencion/percepcion existentes
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Eliminar()
        {
            bool resultado = false;
            Recordset registro = null;
            string consulta = "DELETE [@TFERZR]";

            try
            {
                //Obtener servicio general de la compañia
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    System.GC.Collect();
                }
            }
            return resultado;
        }
    }
}
