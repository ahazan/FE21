using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using System.Collections;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoEstadoSobreRecibido
    {
        /// <summary>
        /// Almacena un nuevo motivo de rechazo
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="estadoCertificadoRecibido"></param>
        /// <returns></returns>
        public bool Almacenar(EstadoCertificadoRecibido estadoCertificadoRecibido)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEESTCFER");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_Motivo", estadoCertificadoRecibido.Motivo);
                dataGeneral.SetProperty("U_Glosa", estadoCertificadoRecibido.Glosa);
                dataGeneral.SetProperty("U_Detalle", estadoCertificadoRecibido.Detalle);
                dataGeneral.SetProperty("U_ConsRec", estadoCertificadoRecibido.IdConsecutivo);
               
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
        /// Actualiza los datos de un motivo de rechazo determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="estadoCertificadoRecibido"></param>
        /// <returns></returns>
        public bool Actualizar(EstadoCertificadoRecibido estadoCertificadoRecibido)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEESTCFER");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", estadoCertificadoRecibido.DocEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_Motivo", estadoCertificadoRecibido.Motivo);
                dataGeneral.SetProperty("U_Glosa", estadoCertificadoRecibido.Glosa);
                dataGeneral.SetProperty("U_Detalle", estadoCertificadoRecibido.Detalle);
                dataGeneral.SetProperty("U_ConsRec", estadoCertificadoRecibido.IdConsecutivo);

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
        /// Consulta los datos de un motivo de rechazo por el DocEntry
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public EstadoCertificadoRecibido Consultar(string docEntry)
        {
            EstadoCertificadoRecibido estadoCerRecibido = null;
            Recordset recSet = null;
            string consulta;

            try
            {
                //Obtener objeto estandar de record set 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry, U_Motivo, U_Glosa, U_Detalle, U_ConsRec from [@TFEESTCFER] where DocEntry = '" + docEntry + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Ubicar el record set en la ultima posicion
                recSet.MoveFirst();

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Crea una instancia del objeto estado certificado recibido
                    estadoCerRecibido = new EstadoCertificadoRecibido();

                    //Establece las propiedaes al objeto configuracion ftp
                    estadoCerRecibido.Motivo = recSet.Fields.Item("U_Motivo").Value + "";
                    estadoCerRecibido.Glosa = recSet.Fields.Item("U_Glosa").Value + "";
                    estadoCerRecibido.Detalle = recSet.Fields.Item("U_Detalle").Value + "";
                    estadoCerRecibido.IdConsecutivo = recSet.Fields.Item("U_ConsRec").Value + "";
                    estadoCerRecibido.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
                estadoCerRecibido = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return estadoCerRecibido;
        }

        /// <summary>
        /// Elimina un motivo de rechazo segun un DocEntry determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public bool Eliminar( string docEntry)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEESTCFER");

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
        /// Consulta los motivos de rechazo para un certificado recibido determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="token"></param>
        /// <param name="idRespuesta"></param>
        /// <returns></returns>
        public ArrayList ConsultarCFEProcesado(string idCertificado)
        {
            string consulta = "";
            ArrayList listaMotivosRechazo = new ArrayList();
            EstadoCertificadoRecibido motivo = new EstadoCertificadoRecibido();
            Recordset recSet = null;

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "select U_ConsRec, U_Motivo, U_Glosa, U_Detalle from [@TFEESTCFER] where U_ConsRec = " + idCertificado;
                recSet.DoQuery(consulta);

                for (int i = 0; i < recSet.RecordCount; i++)
                {
                    motivo = new EstadoCertificadoRecibido();

                    motivo.IdConsecutivo = recSet.Fields.Item("U_ConsRec").Value + "";
                    motivo.Motivo = recSet.Fields.Item("U_Motivo").Value + "";
                    motivo.Glosa = recSet.Fields.Item("U_Glosa").Value + "";
                    motivo.Detalle = recSet.Fields.Item("U_Detalle").Value + "";
                    listaMotivosRechazo.Add(motivo);
                    recSet.MoveNext();
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ConsultaCEFPrpcesado/Error " + ex.ToString());
                listaMotivosRechazo = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera de memoria el objeto obtenerSobreRecibido
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return listaMotivosRechazo;
        }

        
    }
}
