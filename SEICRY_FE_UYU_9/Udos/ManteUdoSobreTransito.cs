using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Collections;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoSobreTransito
    {
        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFEST
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="maestro"></param>
        /// <returns></returns>
        public bool Almacenar(SobreTransito sobreTransito)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEST");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_NomSob", sobreTransito.NombreSobre);
                dataGeneral.SetProperty("U_Token", sobreTransito.Token);
                dataGeneral.SetProperty("U_IdRec", sobreTransito.IdReceptor);
                dataGeneral.SetProperty("U_TipoRec", sobreTransito.TipoReceptor.ToString());
                dataGeneral.SetProperty("U_CorRec", sobreTransito.CorreoReceptor);
                dataGeneral.SetProperty("U_Tipo", sobreTransito.Tipo );
                dataGeneral.SetProperty("U_Numero", sobreTransito.Numero );
                dataGeneral.SetProperty("U_Serie", sobreTransito.Serie);

                //Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error Almacenar Sobre Transito " + ex.ToString());
            }
            finally
            {
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto servicioGeneral
                    GC.SuppressFinalize(servicioGeneral);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto dataGeneral
                    GC.SuppressFinalize(dataGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Consulta el listado de todos los sobres en transito que existan
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public List<SobreTransito> Consultar(SobreTransito.ETipoReceptor tipoRec)
        {
            List<SobreTransito> listaSobresTransito = new List<SobreTransito>();
            Recordset recSet = null;
            SobreTransito sobreTransito = null;
            string consulta = "";
            int i = 0;

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry, U_NomSob, U_Token, U_IdRec, U_CorRec, U_TipoRec FROM [@TFEST] where U_TipoRec = '" + tipoRec + "'";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                while(i < recSet.RecordCount)
                {
                    //Crear objeto SobreTransito y establecer valores
                    sobreTransito = new SobreTransito();
                    sobreTransito.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                    sobreTransito.NombreSobre = recSet.Fields.Item("U_NomSob").Value + "";
                    sobreTransito.Token = recSet.Fields.Item("U_Token").Value + "";
                    sobreTransito.IdReceptor = recSet.Fields.Item("U_IdRec").Value + "";
                    sobreTransito.CorreoReceptor = recSet.Fields.Item("U_CorRec").Value + "";
                    sobreTransito.TipoReceptor = (SobreTransito.ETipoReceptor)Enum.Parse(typeof(SobreTransito.ETipoReceptor), recSet.Fields.Item("U_TipoRec").Value + "");

                    listaSobresTransito.Add(sobreTransito);

                    recSet.MoveNext();
                    i++;
                }
            }
            catch (Exception)
            {
                listaSobresTransito = null;
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
            return listaSobresTransito;
        }

        public string ConsultarDocEntry(string idRespuesta)
        {
            Recordset recSet = null;
            string consulta = "";
            string respuesta = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry from [@TFEST] where U_IdRec = '" + idRespuesta + "'";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    respuesta = recSet.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
                respuesta = "";
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
            return respuesta;
        }

        /// <summary>
        /// Elimina el sobre en transito
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public bool Eliminar(string docEntry)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = (ProcConexion.Comp.GetCompanyService()).GetGeneralService("TTFEST");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                if (ValidarDocEntry(docEntry))
                {
                    //Agregar el nuevo registro a la base de datos mediante el servicio general
                    servicioGeneral.Delete(parametros);
                }

                resultado = true;
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error Eliminar " + ex.ToString());
            }
            finally
            {
                if (parametros != null)
                {
                    //Liberar memoria utlizada por el objeto parametros
                    GC.SuppressFinalize(parametros);
                    System.GC.Collect();
                }
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto servicioGeneral
                    GC.SuppressFinalize(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Valida si el docEnty existe
        /// </summary>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        private bool ValidarDocEntry(string docEntry)
        {
            bool salida = false;
            Recordset registro = null;
            string consulta = "SELECT DocEntry FROM [@TFEST] WHERE DocEntry = '" + docEntry + "'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    salida = true;
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
                    GC.SuppressFinalize(registro);
                    GC.Collect();
                }
            }
            return salida;
        }

        /// <summary>
        /// Consulta el listado de todos los sobres en transito que existan
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public List<SobreTransito> ConsultarNoInterfiere(SobreTransito.ETipoReceptor tipoRec)
        {
            List<SobreTransito> listaSobresTransito = new List<SobreTransito>();
            Recordset recSet = null;
            SobreTransito sobreTransito = null;
            string consulta = "";
            int i = 0;

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
               // consulta = "SELECT DocEntry, U_NomSob, U_Token, U_IdRec, U_CorRec, U_TipoRec FROM [@TFEST] where U_TipoRec = '" + tipoRec + "' ORDER BY DocEntry Desc";

                consulta = " SELECT T1.DocEntry, T1.U_NomSob, T1.U_Token, T1.U_IdRec, T1.U_CorRec, T1.U_TipoRec, T1.U_Tipo, T1.U_Serie, T1.U_Numero FROM [@TFEST] T1 "
                 + " inner join [@TFECFE] T2 on T2.U_NumCFE = T1.U_Numero and T2.U_TipoDoc = T1.U_Tipo and T2.U_Serie = T1.U_Serie "
                 + " where T2.U_EstadoDgi = 'PendienteDGI' and T2.U_TipoDoc <> 101  or (T2.U_TipoDoc = 101 and T2.U_MayorUI = 'Y' and T2.U_EstadoDgi = 'PendienteDGI') "
                 + " order by T1.docentry desc ";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                while (i < recSet.RecordCount)
                {
                    //Crear objeto SobreTransito y establecer valores
                    sobreTransito = new SobreTransito();
                    sobreTransito.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                    sobreTransito.NombreSobre = recSet.Fields.Item("U_NomSob").Value + "";
                    sobreTransito.Token = recSet.Fields.Item("U_Token").Value + "";
                    sobreTransito.IdReceptor = recSet.Fields.Item("U_IdRec").Value + "";
                    sobreTransito.CorreoReceptor = recSet.Fields.Item("U_CorRec").Value + "";
                    sobreTransito.TipoReceptor = (SobreTransito.ETipoReceptor)Enum.Parse(typeof(SobreTransito.ETipoReceptor), recSet.Fields.Item("U_TipoRec").Value + "");

                    listaSobresTransito.Add(sobreTransito);

                    recSet.MoveNext();
                    i++;
                }
            }
            catch (Exception)
            {
                listaSobresTransito = null;
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
            return listaSobresTransito;
        }
    }
}
