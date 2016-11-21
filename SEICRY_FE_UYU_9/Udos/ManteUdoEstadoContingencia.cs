using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM.Framework;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoEstadoContingencia
    {
        #region  FUNCIONES

        /// <summary>
        /// Almacena un nuevo registro en la tabla de estado de contingencia
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public bool Almacenar(string estado)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEESTCON");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_Activo", estado);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Add(dataGeneral);

                FrmEstadoContingencia.estadoContingencia = estado;

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
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Actualiza un nuevo registro en la tabla de estado de contingencia
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public bool Actualizar(string estado)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;
            string docEntry = ObtenerDocEntry();

            try
            {
                if (docEntry.Equals(""))
                {
                    resultado = false;
                }
                else
                {
                    //Obtener servicio general de la compañia
                    servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEESTCON");

                    //Apuntar a la cabecera del udo
                    dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                    //Obtener lista de parametros
                    parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                    //Establecer parametros
                    parametros.SetProperty("DocEntry", docEntry);

                    //Apuntar al udo que corresponde con los parametros
                    dataGeneral = servicioGeneral.GetByParams(parametros);

                    //Establecer los valores para las propiedades
                    dataGeneral.SetProperty("U_Activo", estado);

                    //Agregar el nuevo registro a la base de datos mediante el serivicio general
                    servicioGeneral.Update(dataGeneral);

                    resultado = true;

                    FrmEstadoContingencia.estadoContingencia = estado;
                }
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
        /// Realiza la consulta del esatdo de contingencia
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string Consultar()
        {
            Recordset recSet = null;
            string consulta = "", estado = "";

            try
            {
                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT U_Activo FROM [@TFEESTCON]";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                if (recSet.RecordCount > 0)
                {
                    estado = recSet.Fields.Item("U_Activo").Value + "";

                    if (estado.Equals("N"))
                    {
                        estado = ContingenciaUser(); 
                    }
                }

                else
                {
                    estado = ContingenciaUser(); 
                }
                    
            }


            catch (Exception)
            {
                estado = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utlizada por servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return estado;
        }

        private string ContingenciaUser()
        {
            Recordset recSet = null;
            string consulta = "", estado = "";

            try
            {
                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "select T1.U_SNConti  from OUSR T1 WHERE T1.U_NAME = '" + Conexion.ProcConexion.Comp.UserName + "'  or T1.USER_CODE = '" + Conexion.ProcConexion.Comp.UserName + "' ";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                if (recSet.RecordCount > 0)
                {
                    estado = recSet.Fields.Item("U_SNConti").Value + "";
                }

                FrmEstadoContingencia.estadoContingencia = estado;

            }



            catch (Exception)
            {
                estado = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utlizada por servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return estado;
        }

        /// <summary>
        /// Actualiza el estado de contingencia
        /// </summary>
        public void ActualizarEstadoContingencia()
        {
            FrmEstadoContingencia.estadoContingencia = Consultar();
        }

        /// <summary>
        /// Obtiene el docEntry que almacena el estado de contingencia
        /// </summary>
        /// <returns></returns>
        private string ObtenerDocEntry()
        {
            string resultado = "", consulta = "";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT MAX(DocEntry) AS 'DocEntry' FROM [@TFEESTCON]";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            finally
            {
                if (registro != null)
                {
                    //Se libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        #endregion  FUNCIONES
    }
}
