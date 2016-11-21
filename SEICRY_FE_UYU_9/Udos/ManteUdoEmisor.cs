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
    /// Contiene los metodos para la administracion de los datos de los emisores de facturas electronicas
    /// </summary>
    class ManteUdoEmisor
    {
        /// <summary>
        /// Realiza la consulta de los datos del emisor de facturas electrónicas
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public Emisor Consultar()
        {
            Recordset recSet = null;
            Emisor emisor = null;
            string consulta = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_Ruc, U_Nombre, U_NombreC, U_NumRes FROM [@TFEEMI] WHERE DocEntry = 1";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    //Crear objeto emisor y establecer valores
                    emisor = new Emisor();
                    emisor.Ruc = long.Parse(recSet.Fields.Item("U_Ruc").Value + "");
                    emisor.Nombre = recSet.Fields.Item("U_Nombre").Value + "";
                    emisor.NombreComercial = recSet.Fields.Item("U_NombreC").Value + "";
                    emisor.NumeroResolucion = recSet.Fields.Item("U_NumRes").Value + "";
                }
            }
            catch (Exception)
            {
                emisor = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return emisor;
        }

        /// <summary>
        /// Almacenar un nuevo registro en la base datos
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public bool Almacenar(Emisor emisor)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral= null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEEMI");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_Ruc", emisor.Ruc.ToString());
                dataGeneral.SetProperty("U_Nombre", emisor.Nombre);
                dataGeneral.SetProperty("U_NombreC", emisor.NombreComercial);
                dataGeneral.SetProperty("U_NumRes", emisor.NumeroResolucion);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
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
                    //Liberar memoria utlizada por servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Actualiza los datos del emisor de factura electronica en la base de datos
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public bool Actualizar(Emisor emisor)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEEMI");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", 1);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                dataGeneral.SetProperty("U_Ruc", emisor.Ruc.ToString());
                dataGeneral.SetProperty("U_Nombre", emisor.Nombre);
                dataGeneral.SetProperty("U_NombreC", emisor.NombreComercial);
                dataGeneral.SetProperty("U_NumRes", emisor.NumeroResolucion);

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
    }
}
