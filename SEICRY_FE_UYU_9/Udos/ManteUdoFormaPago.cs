using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoFormaPago
    {
        /// <summary>
        /// Retorna el docEntry o la forma de pago
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoSalida"></param>
        /// <returns></returns>
        public string ObtenerDocEntryFormaPago(bool tipoSalida)
        {
            string resultado = string.Empty;

            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                if (tipoSalida)
                {
                    //Establecer consulta
                    consulta = "SELECT DocEntry FROM [@TFEFRMPG]";
                }
                else
                {
                    consulta = "SELECT U_FrmPag FROM [@TFEFRMPG]";
                }

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    if (tipoSalida)
                    {
                        resultado = recSet.Fields.Item("DocEntry").Value + "";
                    }
                    else
                    {
                        resultado = recSet.Fields.Item("U_FrmPag").Value + "";
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
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Almacena la adenda
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoObjetoAsignado"></param>
        /// <param name="objetoAsignado"></param>
        /// <param name="partesAdenda"></param>
        /// <returns></returns>
        public bool Almacenar(string formaPago)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEFRMPG");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_FrmPag", formaPago);               

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Add(dataGeneral);

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

        /// <summary>
        /// Almacena la adenda
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoObjetoAsignado"></param>
        /// <param name="objetoAsignado"></param>
        /// <param name="partesAdenda"></param>
        /// <returns></returns>
        public bool Actualizar(string formaPago, string docEntry)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEFRMPG");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_FrmPag", formaPago);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Update(dataGeneral);

                salida = true;
            }
            catch (Exception ex)
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
