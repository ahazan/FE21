using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoEnvioCorreoElectronico
    {
        public static bool metodoMantenimiento = false;

        /// <summary>
        /// Metodo para insertar datos en tabla TECA
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="atributos"></param>
        /// <param name="nombreUdo"></param>
        /// <returns true="Datos insertados"
        ///          false="Datos no insertados"></returns>
        private bool almacenarCorreoElectronico(Correo correo)
        {
            bool resultado = false;
            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTECA");

                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_Correo", correo.Cuenta);
                dataGeneral.SetProperty("U_Clave", correo.Clave);
                dataGeneral.SetProperty("U_Opcion", correo.Opcion);

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

        /// <summary>
        /// Metodo para actualizar los datos de correo electronico
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        private bool actualizarCorreoElectronico(Correo correo, string entry)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTECA");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", entry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_Correo", correo.Cuenta);
                dataGeneral.SetProperty("U_Clave", correo.Clave);
                dataGeneral.SetProperty("U_Opcion", correo.Opcion);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Update(dataGeneral);

                resultado = true;
            }
            catch (Exception)
            {                
            }
            finally
            {
                if (parametros != null)
                {
                    //Liberar memoria utlizada por parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por dataGeneral
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

        /// <summary>
        /// Determina si hay que insertar o actualizar los datos
        /// </summary>
        /// <returns></returns>
        public bool datosCorreo(Correo correo)
        {
            Recordset rSet = null;
            string consulta = "";
            bool resultado = false;                        

            try
            {
                rSet = ProcConexion.Comp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                consulta = "SELECT DocEntry FROM [@TECA]";

                rSet.DoQuery(consulta);

                //Si no hay registro inserta
                if (rSet.RecordCount == 0)
                {
                    metodoMantenimiento = false;

                    if (almacenarCorreoElectronico(correo))
                    {
                        resultado = true;
                    }
                }
                //Si hay registros actualiza
                else if (rSet.RecordCount > 0)
                {
                    metodoMantenimiento = true;

                    if (actualizarCorreoElectronico(correo, rSet.Fields.Item("DocEntry").Value.ToString()))
                    {
                        resultado = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (rSet != null)
                {
                    //Se libera el objeto rSet de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rSet);
                    GC.Collect();                       
                }
            }
            return resultado;
        }
    }
}
