using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Xml;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para la adminstracion del udo de Impuestos. 
    /// </summary>
    class ManteUdoImpuestos
    {
        #region FUNCIONES

        /// <summary>
        ///  Ingresa un nuevo registro a la tabla @TFEIMPDGIB1.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idSobre"></param>
        /// <param name="nombreSobre"></param>
        /// <param name="fechaSobre"></param>
        /// <returns></returns>
        public bool Almacenar(Impuesto impuesto)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEIMPDGIB1");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Asiganar valor a cada una de las caracteristicas del udo
                dataGeneral.SetProperty("U_TipImpDgi", impuesto.TipoImpuestoDgi);
                dataGeneral.SetProperty("U_Desc", impuesto.Descripcion);
                dataGeneral.SetProperty("U_CodImpB1",impuesto.CodigoImpuestoB1);

                //Agregar el nuevo registro a la base de datos mediante el servicio general de la compañia
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
                    //Liberar memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }   

        /// <summary>
        /// Elimina los datos de la tabla TFEIMPDGIB1
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        public bool Eliminar(string docEntry)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEIMPDGIB1");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Eliminar el rango
                servicioGeneral.Delete(parametros);

                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
                if (parametros != null)
                {
                    //Liberar memoria utlizada por objeto parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
            }
            return resultado;
        }


        /// <summary>
        /// Obtiene todos los DocEntries de la tabla [@TFEIMPDGIB1]
        /// </summary>
        /// <returns></returns>
        public List<Impuesto> ObtenerRegistros()
        {
            List<Impuesto> resultado = new List<Impuesto>();
            Recordset registro = null;
            string consulta = "SELECT DocEntry, U_TipImpDgi, U_Desc, U_CodImpB1 FROM [@TFEIMPDGIB1]";
            int j = 0;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    //Se recorre el resultado de la consulta
                    while (j < registro.RecordCount)
                    {
                        Impuesto temp = new Impuesto();

                        temp.DocEntry = registro.Fields.Item("DocEntry").Value + "";
                        temp.TipoImpuestoDgi = registro.Fields.Item("U_TipImpDgi").Value + "";
                        temp.Descripcion = registro.Fields.Item("U_Desc").Value + "";
                        temp.CodigoImpuestoB1 = registro.Fields.Item("U_CodImpB1").Value + "";
                        //Se agrega el docEntry a la lista
                        resultado.Add(temp);
                        registro.MoveNext();
                        j++;
                    }
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

        #endregion FUNCIONES
    }
}
