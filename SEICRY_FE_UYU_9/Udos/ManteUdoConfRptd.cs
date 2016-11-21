using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoConfRptd
    {
        /// <summary>
        /// Obtiene la configuracion de ejecucion para el reporte diario
        /// </summary>
        /// <returns></returns>
        public ConfRptd ObtenerConfiguracion()
        {
            ConfRptd salida = null;
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_DiaE, U_Modo, DocEntry, U_SecEnv, U_CAE_General,  [U_AutoGenerar] ,[U_HoraEjecucion],[U_DiaEFinal] FROM [@TFEDRPTD]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = new ConfRptd();
                    salida.DiaEjecucion = recSet.Fields.Item("U_DiaE").Value + "";
                    salida.ModoEjecucion = recSet.Fields.Item("U_Modo").Value + "";
                    salida.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                    salida.SecuenciaEnvio = recSet.Fields.Item("U_SecEnv").Value + "";
                    salida.CAEGenerico = recSet.Fields.Item("U_CAE_General").Value + "";
                    salida.HoraEjec = recSet.Fields.Item("U_HoraEjecucion").Value + "";
                    salida.DiaFin = recSet.Fields.Item("U_DiaEFinal").Value + "";
                    salida.AutoGenerar = recSet.Fields.Item("U_AutoGenerar").Value + "";
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

        public ConfRptd ObtenerConfiguracionDocEntry(ConfRptd salida)
        {
           
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT  DocEntry FROM [@TFEDRPTD]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {                    
                    salida.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                    
                }
            }
            catch (Exception)
            {
                salida.DocEntry = "";
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


        /// <summary>
        /// Almacena la configuracion de ejecucio del reporte diario
        /// </summary>
        /// <param name="confRptd"></param>
        /// <returns></returns>
        public bool AlmacenarConfiguracion(ConfRptd confRptd)
        {
            bool salida = false;




            confRptd = ObtenerConfiguracionDocEntry(confRptd);

            if (confRptd.DocEntry  == "")
            {
                salida = Almacenar(confRptd);
            }
            else
            {
                salida = Actualizar(confRptd, confRptd.DocEntry);
            }

            return salida;
        }

        /// <summary>
        /// Almacena la configuracion de ejecucion para el reporte diario
        /// </summary>
        /// <param name="confRptd"></param>
        /// <returns></returns>
        private bool Almacenar(ConfRptd confRptd)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTCRPTD");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_DiaE", confRptd.DiaEjecucion);
                dataGeneral.SetProperty("U_Modo", confRptd.ModoEjecucion);
                dataGeneral.SetProperty("U_SecEnv", confRptd.SecuenciaEnvio);
                dataGeneral.SetProperty("U_CAE_General", confRptd.CAEGenerico);
                dataGeneral.SetProperty("U_AutoGenerar", confRptd.AutoGenerar);
                dataGeneral.SetProperty("U_HoraEjecucion", confRptd.HoraEjec );
                dataGeneral.SetProperty("U_DiaEFinal", confRptd.DiaFin );

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Add(dataGeneral);

                salida = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.ToString());
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
        /// Actualiza la configuracion de ejecucion del reporte diario
        /// </summary>
        /// <param name="confRptd"></param>
        /// <returns></returns>
        private bool Actualizar(ConfRptd confRptd, string docEntry)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTCRPTD");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_DiaE", confRptd.DiaEjecucion);
                dataGeneral.SetProperty("U_Modo", confRptd.ModoEjecucion);
                dataGeneral.SetProperty("U_SecEnv", confRptd.SecuenciaEnvio);
                dataGeneral.SetProperty("U_CAE_General", confRptd.CAEGenerico);
                dataGeneral.SetProperty("U_AutoGenerar", confRptd.AutoGenerar);
                dataGeneral.SetProperty("U_HoraEjecucion", confRptd.HoraEjec);
                dataGeneral.SetProperty("U_DiaEFinal", confRptd.DiaFin);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Update(dataGeneral);

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
                
    }
}
