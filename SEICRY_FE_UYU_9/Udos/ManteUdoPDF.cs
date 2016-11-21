using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoPDF
    {
        /// <summary>
        /// Metodo para almacenar datos de la ruta de adobe por usuario
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="usuario"></param>
        /// <param name="rutaLogo"></param>
        /// <returns></returns>
 
        public bool AlmacenarPDf(string nombrePdf)
        {
            bool resultado = false;
            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;            

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEPDF");

                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_ArcPdf", nombrePdf);
                //Agregar el nuevo registro a la base de datos 
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (servicioGeneral != null)
                {
                    //Liberar memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }        
    }
}
