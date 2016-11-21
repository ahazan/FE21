using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    //Contiene los metodos para la administracion del udo de reportes diarios
    class ManteUdoRPTD
    {
        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFERPTD.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="fechaReporte"></param>
        /// <param name="secuenciaEnvio"></param>
        /// <param name="fechaResumen"></param>
        /// <returns></returns>
        public bool Almacenar(MonitorRPTD rptd)
        {
            bool resultado = false;
            
            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralData dataDetalle = null;
            GeneralDataCollection detalle = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFERPTD");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores a las propiedades del udo
                dataGeneral.SetProperty("U_Version", rptd.Version);
                dataGeneral.SetProperty("U_RucRec", rptd.RucReceptor);
                dataGeneral.SetProperty("U_RucEmi", rptd.RucEmisor);
                dataGeneral.SetProperty("U_NomArc", rptd.NombreArchivo);
                dataGeneral.SetProperty("U_FecRPTD", rptd.FechaRptd);
                dataGeneral.SetProperty("U_IdEmi", rptd.IdEmisor);
                dataGeneral.SetProperty("U_IdRec", rptd.IdReceptor);
                dataGeneral.SetProperty("U_SecEnvio", rptd.SecuenciaEnvio);
                dataGeneral.SetProperty("U_FecResum", rptd.FechaResumen);
                dataGeneral.SetProperty("U_Estado", rptd.Estado);

                detalle = dataGeneral.Child("TFERPTDDET");

                foreach (MonitorRPTDDET detalleRptd in rptd.Detalle)
                {
                    dataDetalle = detalle.Add();
                    dataDetalle.SetProperty("U_CodRec", detalleRptd.CodigoRechazo);
                    dataDetalle.SetProperty("U_GloRec", detalleRptd.GlosaRechazo);
                    dataDetalle.SetProperty("U_DetRec", detalleRptd.DetalleRechazo);
                }

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
                    //Liberar la memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
                if (detalle != null)
                {
                    //Liberar la memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(detalle);
                    System.GC.Collect();
                }
                if(dataDetalle != null)
                {
                    //Liberar la memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataDetalle);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Actualiza el contador de secuencia de envios para el reporte de una fecha determinada. Esta fecha es la de resumen, es decir, la fecha reportada.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="fechaResumen"></param>
        /// <param name="secuenciaEnvio"></param>
        /// <returns></returns>
        public bool ActualizarSecuenciaEnvio(string fechaResumen)
        {
            bool resultado = false;

            Recordset recSet = null;
            string sentencia = "";

            try
            {
                //Obtener el objeto estandar RecordSet
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer la sentencia 
                sentencia = "UPDATE [@TFERPTD] SET U_SecEnvio = ( U_SecEnvio + 1 ) WHERE U_FecResum = CONVERT(DATE, '" + fechaResumen + "')";

                //Ejecutar la sentencia
                recSet.DoQuery(sentencia);

                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de la memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el listado de sucursales para los rangos activos
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public List<string> ObtenerSucursales()
        {
            IRecordset recSet = null;
            string consulta = "";
            List<string> listaSucursales = new List<string>();

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT DISTINCT(U_Sucursal) FROM [@TFECAE] WHERE U_ValDesde <= GETDATE() AND U_ValHasta >= GETDATE()";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                recSet.MoveFirst();                

                for (int i = 0; i < recSet.RecordCount; i++)
                {
                    listaSucursales.Add(recSet.Fields.Item("U_Sucursal").Value + "");
                    recSet.MoveNext();
                }   
            }
            catch (Exception)
            {
                listaSucursales = null;
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
            return listaSucursales; 
        }

        /// <summary>
        /// Obtiene el listado cfe utilizados por fecha
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public List<CFE> ObtenerCertificadosUtilizados()
        {
            Recordset recSet = null;
            string consulta = "";
            List<CFE> listaCertificados = new List<CFE>();
            CFE cfe;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT T1.U_DocSap, T1.U_TipoDoc, T1.U_Serie, T1.U_NumCFE, T1.U_Sucursal FROM [@TFECFE] AS T1  WHERE CONVERT(DATE, T1.CreateDate, 103) = CONVERT(DATE, GETDATE(), 103)  ORDER BY U_DocSap, U_Sucursal";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                recSet.MoveFirst();

                for (int i = 0; i < recSet.RecordCount; i++)
                {
                    cfe = new CFE();

                    cfe.DocumentoSAP = recSet.Fields.Item("U_DocSap").Value + "";
                    cfe.TipoCFEInt = int.Parse(recSet.Fields.Item("U_TipoDoc").Value + "");
                    cfe.SerieComprobante = recSet.Fields.Item("U_Serie").Value + "";
                    cfe.NumeroComprobante = int.Parse(recSet.Fields.Item("U_NumCFE").Value + "");
                    cfe.CodigoCasaPrincipalEmisor = int.Parse(recSet.Fields.Item("U_Sucursal").Value + "");

                    listaCertificados.Add(cfe);

                    recSet.MoveNext();
                }
            }
            catch (Exception)
            {
                listaCertificados = null;
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
            return listaCertificados;
        }
    }
}
