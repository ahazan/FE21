using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoLogo
    {
        /// <summary>
        /// Metodo para consultar datos de la tabla TLOGO
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="usuario"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public string Consultar(bool control)
        {
            Recordset recSet = null;
            string consulta = "", resultado = "", usuario = ProcConexion.Comp.UserName;

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry, U_RutLog FROM [@TLOGO]";

                //Ejecuta consulta
                recSet.DoQuery(consulta);

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    if (control)
                    {
                        resultado = recSet.Fields.Item("U_RutLog").Value.ToString();
                    }
                    else
                    {
                        resultado = recSet.Fields.Item("DocEntry").Value.ToString();
                    }
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
            return resultado;
        }


        public int ConsultarTimeImp(bool IsLoadScreen = false)
        {
            Recordset recSet = null;
            string consulta = "";
            int resultado = 10;

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_TimeImp FROM [@TLOGO]";

                //Ejecuta consulta
                recSet.DoQuery(consulta);

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {

                    resultado = recSet.Fields.Item("U_TimeImp").Value;

                    if (IsLoadScreen != true)
                    {
                        resultado = resultado * 1000;
                    }



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
            return resultado;
        }

        /// <summary>
        /// Metodo para almacenar datos de la ruta del logo por usuario
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="usuario"></param>
        /// <param name="rutaLogo"></param>
        /// <returns></returns> 
        public bool AlmacenarLOGO(string rutaLogo, int timeImp)
        {
            bool resultado = false;
            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                string docEntry = Consultar(false);

                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTLOGO");

                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                if (docEntry.Equals(""))
                {
                    dataGeneral.SetProperty("U_RutLog", rutaLogo);
                    dataGeneral.SetProperty("U_TimeImp", timeImp);
                    //Agregar el nuevo registro a la base de datos 
                    servicioGeneral.Add(dataGeneral);
                }
                else
                {
                    //Nota: el orden importa por esa razon dataGeneral.SetProperty se repita en el if y else
                    //Obtener lista de parametros
                    parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                    //Establecer parametros
                    parametros.SetProperty("DocEntry", docEntry);

                    //Apuntar al udo que corresponde con los parametros
                    dataGeneral = servicioGeneral.GetByParams(parametros);
                    dataGeneral.SetProperty("U_RutLog", rutaLogo);
                    dataGeneral.SetProperty("U_TimeImp", timeImp);

                    //Actualiza el registro en la base de datos 
                    servicioGeneral.Update(dataGeneral);
                }
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
                    //Liberar memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Almacena los datos de alerta de fin de CAE
        /// </summary>
        /// <param name="cant"></param>
        /// <param name="dias"></param>
        /// <returns></returns>
        public bool Almacenar(string cant, string dias)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                string docEntry = Consultar(false);

                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTLOGO");

                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                if (docEntry.Equals(""))
                {
                    dataGeneral.SetProperty("U_Cant", cant);
                    dataGeneral.SetProperty("U_Dia", dias);
                    //Agregar el nuevo registro a la base de datos 
                    servicioGeneral.Add(dataGeneral);
                }
                else
                {
                    //Nota: el orden importa por esa razon dataGeneral.SetProperty se repita en el if y else
                    //Obtener lista de parametros
                    parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                    //Establecer parametros
                    parametros.SetProperty("DocEntry", docEntry);

                    //Apuntar al udo que corresponde con los parametros
                    dataGeneral = servicioGeneral.GetByParams(parametros);
                    dataGeneral.SetProperty("U_Cant", cant);
                    dataGeneral.SetProperty("U_Dia", dias);

                    //Actualiza el registro en la base de datos 
                    servicioGeneral.Update(dataGeneral);
                }
                salida = true;
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
                    //Liberar memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }      

            return salida;
        }        
    }
}
