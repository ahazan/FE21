using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoUsuarios
    {
        /// <summary>
        /// Actualiza el valor de "autorizado" para un usuario determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idUsuario"></param>
        /// <param name="autorizacion"></param>
        /// <returns></returns>
        public bool Actualizar(int idUsuario, string autorizacion)
        {
            //Obtiene objeto de negocio para usuario
            Users usuario = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oUsers);
            int resultadoOperacion = -1;

            try
            {
                //Obtiene el usuario por identificador
                usuario.GetByKey(idUsuario);

                //Asigna el valor de autorizacion
                usuario.UserFields.Fields.Item("U_ADNE").Value = autorizacion;

                //Actualiza el usuario
                resultadoOperacion = usuario.Update();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (usuario != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(usuario);
                    GC.Collect();
                }
            }

            //Retorna true o false dependiendo del resultado de la operacion
            return resultadoOperacion == 0 ? true : false;
        }

        /// <summary>
        /// Consulta si un usuario determinado tiene autorizacion para crear documentos no electronicos
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool ConsultarAutorizacion()
        {
            bool salida = false;
            Recordset recSet = null;
            string consulta = "";

            if (ProcConexion.Comp.UserName.Equals("manager"))
            {
                salida = true;
            }
            else
            {
                try
                {
                    //Obtener objeto de recordset 
                    recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                    //Establecer consulta
                    consulta = "SELECT U_ADNE FROM OUSR WHERE USER_CODE = '" + ProcConexion.Comp.UserName + "' AND U_ADNE IS NOT NULL";

                    //Ejecuta consulta
                    recSet.DoQuery(consulta);

                    //Validar que existan valores
                    if (recSet.RecordCount > 0)
                    {
                        salida = recSet.Fields.Item("U_ADNE").Value.toString() == "Y" ? true : false;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (recSet != null)
                    {
                        //Se libera recSet de memoria
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                        System.GC.Collect();
                    }
                }
            }
            return salida;
        }
    }
}
