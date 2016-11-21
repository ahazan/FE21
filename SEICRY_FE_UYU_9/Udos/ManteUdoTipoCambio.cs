using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoTipoCambio
    {
        /// <summary>
        /// Obtiene la configuracion de ejecucion para el reporte diario
        /// </summary>
        /// <returns></returns>
        public string ObtenerConfiguracion(out string docEntry)
        {
            Recordset recSet = null;
            string consulta = "", salida = "";
            docEntry = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_TC, DocEntry FROM [@TFEADOBE]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = recSet.Fields.Item("U_TC").Value + "";
                    docEntry = recSet.Fields.Item("docEntry").Value + "";
                }
            }
            catch (Exception)
            {
                salida = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return salida;
        }

        /// <summary>
        /// Almacena la configuracion de ejecucio del reporte diario
        /// </summary>
        /// <param name="confRptd"></param>
        /// <returns></returns>
        public bool AlmacenarConfiguracion(string tipoCambio)
        {
            bool salida = false;

            string docEntry = ""; 
            ObtenerConfiguracion(out docEntry);

            if (docEntry.Equals(""))
            {
                salida = Almacenar(tipoCambio);           
            }
            else
            {
                salida = Actualizar(tipoCambio, docEntry);                     
            }

            return salida;
        }

        /// <summary>
        /// Almacena la configuracion de ejecucion para el reporte diario
        /// </summary>
        /// <param name="confRptd"></param>
        /// <returns></returns>
        private bool Almacenar(string tipoCambio)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEADOBE");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_TC", tipoCambio);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Add(dataGeneral);

                salida = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.ToString());
            }
            finally
            {
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por objeto dataGeneral
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

            return salida;
        }

        /// <summary>
        /// Actualiza la configuracion de ejecucion del reporte diario
        /// </summary>
        /// <param name="confRptd"></param>
        /// <returns></returns>
        private bool Actualizar(string tipoCambio, string docEntry)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEADOBE");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_TC", tipoCambio);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Update(dataGeneral);

                salida = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por objeto dataGeneral
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

            return salida;
        }
                
    }
}
