using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Udos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SEICRY_FE_UYU_9.Conexion;


namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoControlSobres 
    {
        #region FUNCIONES

        /// <summary>
        /// Almacena los sobres
        /// </summary>
        /// <param name="controlSobres"></param>
        /// <returns></returns>
        public bool Almacenar(ControlSobres controlSobres)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECONSOB");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Asiganar valor a cada una de la propiedades del udo
                dataGeneral.SetProperty("U_Tipo", controlSobres.Tipo);
                dataGeneral.SetProperty("U_Serie", controlSobres.Serie);
                dataGeneral.SetProperty("U_Numero", controlSobres.Numero);
                dataGeneral.SetProperty("U_Estado", controlSobres.Estado);
                dataGeneral.SetProperty("U_Usuario", controlSobres.UsuarioSap);
                dataGeneral.SetProperty("U_DocSap", controlSobres.DocumentoSap);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: Almacenar Envio DGI/ " + ex.ToString());
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
        /// Actualiza el estado de los sobres
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool Actualizar(ControlSobres control)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECONSOB");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", control.DocEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);
                dataGeneral.SetProperty("U_Estado", control.Estado);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Update(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.ToString());
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
        /// Elimina los sobres enviados a DGI
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool Eliminar(ControlSobres control)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECONSOB");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", control.DocEntry);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Delete(parametros);

                resultado = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.ToString());
            }
            finally
            {
                if (parametros != null)
                {
                    //Liberar memoria utlizada por el objeto parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
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
        /// Obtiene le docEntry de un sobre
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public ControlSobres ObtenerDocEntry(ControlSobres control)
        {
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                JobEnvioSobreMasivo Usuario = new JobEnvioSobreMasivo();

                
                //Establecer consulta
                if (Usuario.SuperUsuario())
                {
                    consulta = "SELECT DocEntry FROM [@TFECONSOB] WHERE U_Tipo = '" + control.Tipo + "' AND U_Serie ='" +
                                    control.Serie + "' AND U_Numero = '" + control.Numero + "'";
                }
                else
                {
                    consulta = "SELECT DocEntry FROM [@TFECONSOB] WHERE U_Tipo = '" + control.Tipo + "' AND U_Serie ='" +
                                     control.Serie + "' AND U_Numero = '" + control.Numero + "' AND U_Usuario = '" + control.UsuarioSap + "'";
                }

                               
             

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    control.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
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
            return control;
        }
        

        public void ActualizarFirmaElectronica(ControlSobres control)
        {
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);             

                //Establecer consulta            
                 consulta = "Update [@TFECFE] set U_FechaFirma = GETDATE()  where U_TipoDoc = '" + control.Tipo + "' AND U_Serie ='" +
                                    control.Serie + "' AND U_NumCFE = '" + control.Numero + "'";             

                //Ejectura consulta
                recSet.DoQuery(consulta);              
               
            }
            catch (Exception)
            {
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
        }
        #endregion FUNCIONES
    }              
}

