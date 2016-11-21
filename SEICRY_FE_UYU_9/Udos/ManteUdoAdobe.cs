using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoAdobe
    {
        /// <summary>
        /// Metodo para consultar datos de la tabla TFEADOBE
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="usuario"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public string Consultar(bool control)
        {
            Recordset recSet = null;
            string consulta = "", resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry, U_Ruta FROM [@TFEADOBE]";

                //Ejecuta consulta
                recSet.DoQuery(consulta);

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    if (control)
                    {
                        resultado = recSet.Fields.Item("U_Ruta").Value.ToString();
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

        /// <summary>
        /// Obtiene el C.I generico registrado en la base de datos
        /// </summary>
        public string ObtenerCiGenerico()
        {
            Recordset registro = null;
            string consulta = "SELECT U_DatosCi FROM [@TFEADOBE]", resultado = "";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("U_DatosCi").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                //Libera de memoria al objeto registro
                if (registro != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }


        /// <summary>
        /// Metodo para almacenar datos de la ruta de adobe por usuario
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="usuario"></param>
        /// <param name="rutaLogo"></param>
        /// <returns></returns> 
        public bool  AlmacenarAdobeReader(string rutaAdobeReader)
        {
            bool resultado = false;
            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;
            string usuario = ProcConexion.Comp.UserName;

            try
            {
                string docEntry = Consultar(false);

                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEADOBE");

                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                if (docEntry.Equals(""))
                {
                    dataGeneral.SetProperty("U_Usuario", usuario);
                    dataGeneral.SetProperty("U_Ruta", rutaAdobeReader);
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

                    dataGeneral.SetProperty("U_Usuario", usuario);
                    dataGeneral.SetProperty("U_Ruta", rutaAdobeReader);

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
        /// Almacena el C.I generico
        /// </summary>
        /// <param name="cIgenerico"></param>
        /// <returns></returns>
        public bool AlmacenarCI(string cIgenerico)
        {
            bool resultado = false;
            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                string docEntry = ConsultarCI();

                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEADOBE");

                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                if (docEntry.Equals(""))
                {
                    dataGeneral.SetProperty("U_DatosCi", cIgenerico);
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

                    dataGeneral.SetProperty("U_DatosCi", cIgenerico);

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
        /// Devuelve la ruta de Adobe Reader
        /// </summary>
        /// <returns></returns>
        public string ObtenerRuta()
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT U_RUTA FROM [@TFEADOBE]";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("U_Ruta").Value + "";
                }
            }
            catch(Exception)
            {
            }
            finally
            {
                if(registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Consulta si existe un C.I registrado en la base de datos
        /// </summary>
        /// <returns></returns>
        private string ConsultarCI()
        {
            Recordset recSet = null;
            string consulta = "", resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry FROM [@TFEADOBE]";

                //Ejecuta consulta
                recSet.DoQuery(consulta);

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("DocEntry").Value.ToString();                 
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
    }
}
