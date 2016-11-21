using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoCorreos
    {
        /// <summary>
        /// Metodo para cosultar la tabla TECA de envio de correos automaticos
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public Correo Consultar()
        {
            Recordset recSet = null;
            Correo correo = null;
            string consulta = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_Correo, U_Clave, U_Opcion FROM [@TECA]";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    //Crar objeto corre y establecer valores
                    correo = new Correo();
                     
                    correo.Cuenta = recSet.Fields.Item(0).Value + "";
                    correo.Clave = recSet.Fields.Item(1).Value + "";
                    correo.Opcion = recSet.Fields.Item(2).Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto recSet de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }
            return correo;
        }
    }
}
