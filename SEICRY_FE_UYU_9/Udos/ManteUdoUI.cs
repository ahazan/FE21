using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoUI
    {
        /// <summary>
        /// Consulta el valor del UI
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public double ConsultarValorUI()
        {
            Recordset recSet = null;
            string consulta = "";
            double valorUI = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select rate from ORTT where Currency = 'UI' and '12' = RTRIM(LTRIM(CONVERT (char (2), DATEPART(month, RateDate)))) and RTRIM(LTRIM(CONVERT (char (4), DATEPART(YEAR, GETDATE()))))-1 = RTRIM(LTRIM(CONVERT (char (4), DATEPART(YEAR, RateDate))))"; 
                    //"select rate from ORTT where Currency = 'UI' and RTRIM(LTRIM(CONVERT (char (2), DATEPART(month, GETDATE())))) = RTRIM(LTRIM(CONVERT (char (2), DATEPART(month, RateDate)))) and RTRIM(LTRIM(CONVERT (char (4), DATEPART(YEAR, GETDATE())))) = RTRIM(LTRIM(CONVERT (char (4), DATEPART(YEAR, RateDate))))";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    valorUI = double.Parse(recSet.Fields.Item("rate").Value + "");
                }
            }
            catch (Exception)
            {
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

            return valorUI;
        }




        public Boolean  ConsultarWSTransaccionesPeriodicas()
        {
            Recordset recSet = null;
            string consulta = "";
            Boolean respuesta = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select top 1 U_WsTransPer from  [@TFECONFTP]";
                //"select rate from ORTT where Currency = 'UI' and RTRIM(LTRIM(CONVERT (char (2), DATEPART(month, GETDATE())))) = RTRIM(LTRIM(CONVERT (char (2), DATEPART(month, RateDate)))) and RTRIM(LTRIM(CONVERT (char (4), DATEPART(YEAR, GETDATE())))) = RTRIM(LTRIM(CONVERT (char (4), DATEPART(YEAR, RateDate))))";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    respuesta = recSet.Fields.Item("U_WsTransPer").Value == "N" ? false : true;
                }
            }
            catch (Exception)
            {
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

            return respuesta;
        }
    }
}
