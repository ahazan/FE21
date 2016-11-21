using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SAPbouiCOM.Framework;
using System.Text.RegularExpressions;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodo para la administracion de los CAE's obtenidos de la DGI
    /// </summary>
    class ManteUdoCAE
    {
        #region  FUNCIONES

        /// <summary>
        /// Almacena un nuevo registro en la tabla de CAE's
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="cae"></param>
        /// <returns></returns>
        public bool Almacenar(CAE cae, out string idCAE)
        {
            bool resultado = false;
            idCAE = "";

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;


            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECAE");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_TipoDoc", ((int)cae.TipoCFE).ToString());
                dataGeneral.SetProperty("U_NombDoc", cae.NombreDocumento);
                dataGeneral.SetProperty("U_Sucursal", cae.Sucursal);
                dataGeneral.SetProperty("U_Caja", QuitarGuion(cae.Caja));
                dataGeneral.SetProperty("U_Serie", cae.Serie.ToUpper());
                dataGeneral.SetProperty("U_NumIni", cae.NumeroDesde.ToString());
                dataGeneral.SetProperty("U_NumFin", cae.NumeroHasta.ToString());
                dataGeneral.SetProperty("U_TipAut", cae.TipoAutorizacion);
                dataGeneral.SetProperty("U_NumAut", cae.NumeroAutorizacion);
                dataGeneral.SetProperty("U_ValDesde", cae.FechaEmision);
                dataGeneral.SetProperty("U_ValHasta", cae.FechaVencimiento);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Add(dataGeneral);

                //Consulta el DocEntry generado para el cae recien ingresado
                idCAE = ConsultarNumeroRangoCreado(cae);

                resultado = true;
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
            return resultado;
        }

        /// <summary>
        /// Realiza la consulta de un rango CAE según su número
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        public CAE Consultar(string numeroRango)
        {
            Recordset recSet = null;
            CAE cae = null;
            string consulta = "";

            try
            {

                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT U_TipoDoc, U_NombDoc, U_Serie, U_NumIni, U_NumFin, U_TipAut, U_NumAut, CONVERT(VARCHAR(10), U_ValDesde, 103) AS U_ValDesde, CONVERT(VARCHAR(10), U_ValHasta, 103) AS U_ValHasta, U_Sucursal, U_Caja FROM [@TFECAE] WHERE DocEntry = '" + numeroRango + "'";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                if (recSet.RecordCount > 0)
                {
                    //Crear nuevo objeto CAE
                    cae = new CAE();

                    //Establezco los valores del cae con los valores obtenidos en la consulta
                    cae.TipoCFE = CAE.ObtenerTipoCFECFC(recSet.Fields.Item("U_TipoDoc").Value + "");
                    cae.NombreDocumento = CAE.ObtenerNombreCFECFC(recSet.Fields.Item("U_TipoDoc").Value + "");
                    cae.Sucursal = ""; // recSet.Fields.Item("U_Sucursal").Value + "";
                    cae.Caja = recSet.Fields.Item("U_Caja").Value + "";
                    cae.Serie = recSet.Fields.Item("U_Serie").Value + "";
                    cae.NumeroDesde = int.Parse(recSet.Fields.Item("U_NumIni").Value + "");
                    cae.NumeroHasta = int.Parse(recSet.Fields.Item("U_NumFin").Value + "");
                    cae.TipoAutorizacion = recSet.Fields.Item("U_TipAut").Value + "";
                    cae.NumeroAutorizacion = recSet.Fields.Item("U_NumAut").Value + "";
                    cae.FechaEmision = recSet.Fields.Item("U_ValDesde").Value + "";
                    cae.FechaVencimiento = recSet.Fields.Item("U_ValHasta").Value + "";
                }
            }
            catch (Exception)            
            {
                cae = null;
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
            return cae;           
        }

        /// <summary>
        /// Realiza la consulta de un rango CAE según el tipo de documento y la serie
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        public CAE Consultar(string tipoDocumento, string serie)
        {
            Recordset recSet = null;
            string consulta = "";
            CAE cae = null;

            try
            {
                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT T1.U_TipoDoc, U_NombDoc, T1.U_Serie, T1.U_NumIni, T1.U_NumFin, U_TipAut, U_NumAut, " +
                           "CONVERT(VARCHAR(10), T1.U_ValDesde, 103) AS U_ValDesde, CONVERT(VARCHAR(10), T1.U_ValHasta, 103) " +
                           " AS U_ValHasta, U_Sucursal, U_Caja FROM [@TFECAE] AS T1 INNER JOIN [@TFERANGO] AS T2 ON T1.DocEntry " +
                           "= T2.U_IdCAE AND T2.U_Activo = 'Y' WHERE T1.U_TipoDoc = '" + tipoDocumento + "' AND T1.U_Serie = '" + serie +"'";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                if (recSet.RecordCount > 0)
                {
                    //Crear nuevo objeto CAE
                    cae = new CAE();

                    //Establezco los valores del cae con los valores obtenidos en la consulta
                    cae.TipoCFE = CAE.ObtenerTipoCFECFC(recSet.Fields.Item("U_TipoDoc").Value + "");
                    cae.NombreDocumento = CAE.ObtenerNombreCFECFC(recSet.Fields.Item("U_TipoDoc").Value + "");
                    cae.Sucursal = ""; // recSet.Fields.Item("U_Sucursal").Value + "";
                    cae.Caja = recSet.Fields.Item("U_Caja").Value + "";
                    cae.Serie = recSet.Fields.Item("U_Serie").Value + "";
                    cae.NumeroDesde = int.Parse(recSet.Fields.Item("U_NumIni").Value + "");
                    cae.NumeroHasta = int.Parse(recSet.Fields.Item("U_NumFin").Value + "");
                    cae.TipoAutorizacion = recSet.Fields.Item("U_TipAut").Value + "";
                    cae.NumeroAutorizacion = recSet.Fields.Item("U_NumAut").Value + "";
                    cae.FechaEmision = recSet.Fields.Item("U_ValDesde").Value + "";
                    cae.FechaVencimiento = recSet.Fields.Item("U_ValHasta").Value + "";
                }
            }
            catch (Exception)
            {
                cae = null;
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
            return cae;
        }

        /// <summary>
        /// Actualiza los datos de un rango determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="cae"></param>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        public bool Actualizar(CAE cae, string numeroRango)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECAE");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", numeroRango);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_TipoDoc", ((int)cae.TipoCFE).ToString());
                dataGeneral.SetProperty("U_NombDoc", cae.NombreDocumento);
                dataGeneral.SetProperty("U_Sucursal", cae.Sucursal);
                dataGeneral.SetProperty("U_Caja", QuitarGuion(cae.Caja));
                dataGeneral.SetProperty("U_Serie", cae.Serie.ToUpper());
                dataGeneral.SetProperty("U_NumIni", cae.NumeroDesde.ToString());
                dataGeneral.SetProperty("U_NumFin", cae.NumeroHasta.ToString());
                dataGeneral.SetProperty("U_TipAut", cae.TipoAutorizacion);
                dataGeneral.SetProperty("U_NumAut", cae.NumeroAutorizacion);
                dataGeneral.SetProperty("U_ValDesde", cae.FechaEmision);
                dataGeneral.SetProperty("U_ValHasta", cae.FechaVencimiento);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Update(dataGeneral);

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
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
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
        /// Elimina todo lo que no sea numero de una cadena
        /// </summary>
        /// <param name="conGuion"></param>
        /// <returns></returns>
        private string QuitarGuion(string conGuion)
        {
            string resultado = "";

            try
            {
                resultado = Regex.Replace(conGuion, "([^0-9])", "");
            }
            catch(Exception)
            {
                resultado = conGuion;
            }

            return resultado;
        }

        /// <summary>
        /// Elimina un rango determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        public bool Eliminar(string numeroRango)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECAE");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", numeroRango);

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
        /// Consultar lista de rangos que se encuentran activos a la fecha actual
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string[] ConsultarRangosActivos()
        {
            string[] rangosActivos = null;
            Recordset recSet = null;
            string consulta = "";

            try
            {

                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry FROM [@TFECAE] WHERE U_ValDesde <= GETDATE() AND U_ValHasta >= GETDATE()";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Ubicar record set en la primera posicion
                recSet.MoveFirst();

                //Crear arreglo strin para agregar los valores de DocEntry consultados
                rangosActivos = new string[recSet.RecordCount];

                //Obtener valores consultados
                for (int i = 0; i < recSet.RecordCount; i++)
                {
                    //Agregar valor al arreglo
                    rangosActivos[i] = recSet.Fields.Item("DocEntry").Value + "";

                    //Avanzar record set
                    recSet.MoveNext();
                }
            }
            catch (Exception)
            {
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
            return rangosActivos;
        }

        /// <summary>
        /// Consulta el DocEntry del un rango en especifico
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        private string ConsultarNumeroRangoCreado(CAE cae)
        {
            Recordset recSet = null;
            string consulta = "";
            string DocEntryObtenido = "";

            try
            {

                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT DocEntry FROM [@TFECAE] WHERE U_NumAut = '" + cae.NumeroAutorizacion +
                    "' AND U_TipoDoc = '" + ((int)cae.TipoCFE) + "' AND U_Serie = '" + cae.Serie + "'";
                //consulta = "SELECT DocEntry FROM [@TFECAE] WHERE U_NumAut = '" + cae.NumeroAutorizacion + "' AND";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                if (recSet.RecordCount > 0)
                {
                    DocEntryObtenido = recSet.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
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

            return DocEntryObtenido;
        }

        /// <summary>
        /// Consulta las sucursales configuradas en B1
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public Sucursal[] ConsultarListaSucursales()
        {
            Sucursal[] sucursales = null;
            Sucursal sucursal = null;

            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT Name, Remarks FROM [OUBR]";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Inicializa el 
                sucursales = new Sucursal[recSet.RecordCount];

                //Validar que se hayan obtenido resultado
                if (recSet.RecordCount > 0)
                {
                    //Ubicar el cursor en la primera posicion
                    recSet.MoveFirst();

                    //Recorrer la lista de resultados
                    for (int i = 0; i < recSet.RecordCount; i++)
                    {
                        sucursal = new Sucursal();
                        sucursal.Nombre = recSet.Fields.Item("Name").Value + "";
                        sucursal.Descripcion = recSet.Fields.Item("Remarks").Value + "";
                        sucursales[i] = sucursal;

                        //Mover el cursor
                        recSet.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                sucursales = null;
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

            return sucursales;
        }

        /// <summary>
        /// Valida que exista un CAE activo para un tipo de documento determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoCAE"></param>
        /// <returns></returns>
        public bool TipoCAEValido(CAE.ESTipoCFECFC tipoCAE)
        {
            bool resultado = false;
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estadar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT T1.DocEntry FROM [@TFECAE] AS T1 INNER JOIN [@TFERANGO] AS T2 ON T1.DocEntry = T2.U_IdCAE WHERE T1.U_TipoDoc = '" + tipoCAE + "' AND T1.U_ValDesde <= GETDATE() AND T1.U_ValHasta >= GETDATE() AND T2.U_Activo = 'Y'";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
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
                    //Liberar memoria utlizada por servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return resultado;
        }


        public ConfCAEFin ObtenerConfiguracionCaeFin()
        {
            ConfCAEFin salida = null;
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select U_Cant, U_Dia from [@TLOGO]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = new ConfCAEFin();
                    salida.NumCaeFin = recSet.Fields.Item("U_Cant").Value + "";
                    salida.FechaCaeFin = recSet.Fields.Item("U_Dia").Value + "";                  
                }
            }
            catch (Exception)
            {
                salida = null;
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

            return salida;
        }


        #endregion FUNCIONES

    }
}
