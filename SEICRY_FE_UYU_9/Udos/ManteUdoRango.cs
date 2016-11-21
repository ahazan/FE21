using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para la administracion del udo de Rangos
    /// </summary>
    class ManteUdoRango
    {
        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFECFE.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="rango"></param>
        /// <returns></returns>
        public bool Almacenar(Rango rango) 
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFERANGO");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_TipoDoc", ((int)rango.TipoDocumento).ToString());
                dataGeneral.SetProperty("U_NumIni", rango.NumeroInicial.ToString());
                dataGeneral.SetProperty("U_NumFin", rango.NumeroFinal.ToString());
                dataGeneral.SetProperty("U_NumAct", rango.NumeroActual.ToString());
                dataGeneral.SetProperty("U_Serie", rango.Serie);
                dataGeneral.SetProperty("U_ValDesde", rango.ValidoDesde);
                dataGeneral.SetProperty("U_ValHasta", rango.ValidoHasta);
                dataGeneral.SetProperty("U_IdCAE", rango.IdCAE);
                dataGeneral.SetProperty("U_Activo", rango.Activo);

                //Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
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
                    //Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Realiza la consulta para validar si un rango determinado ha sido utilizado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idCAE"></param>
        /// <returns></returns>
        public bool RangoUtilizado(string idCAE)
        {
            Recordset recSet = null;
            bool resultado = false;            
            string consulta = "";

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry FROM [@TFERANGO] WHERE U_IdCAE = '" + idCAE + "' AND U_NumAct > U_NumIni";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto recSet de la memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Activa los rangos en la BD
        /// </summary>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public bool ActivarRango(string docEntry, string estado)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFERANGO");

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
            }
            catch (Exception)
            {
            }
            finally
            {
                if (parametros != null)
                {
                    //Liberar memoria utlizada por el objeto parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Obtiene el docEntry 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string ObtenerDocEntry(RangoCAE rangoCAE)
        {
            string salida = "", consulta = "";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT DocEntry FROM [@TFERANGO] WHERE U_TipoDoc = '"+ rangoCAE.TipoDocumento + 
                           "' AND U_NumIni = '" + rangoCAE.NumeroInicial + "' AND U_NumFin = '"+ rangoCAE.NumeroFinal +
                           "' AND U_NumAct = '" + rangoCAE.NumeroActual +"' AND U_Serie = '"+ rangoCAE.Serie + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    salida = registro.Fields.Item("DocEntry").Value + "";
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

            return salida;
        }
    }
}
