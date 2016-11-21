using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Collections;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Registros
    {
        /// <summary>
        /// Metodo para realizar una consulta por medio de un recordSet
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="consulta"></param>
        /// <param name="parametros"></param>
        /// <param name="salida"></param>
        /// <param name="salidaMultiple"></param>
        /// <returns></returns>
        public Object realizarConsulta(string consulta)
        {
            Recordset registro = null;
            Object resultado = null;            

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                registro.MoveFirst();

                resultado = new Object();

                if (registro.RecordCount > 0)
                {
                    foreach (var campo in registro.Fields)
                    {
                        resultado = campo;
                    }
                }                 
            }
            catch(Exception)
            {            
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Metodo para determinar si una consulta devuelve resultados
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public bool Consulta(string consulta)
        {
            bool resultado = false;
            Recordset registro = null;

            try
            {
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
                    GC.Collect();
                }
            }

            return resultado;
        }
    }
}
