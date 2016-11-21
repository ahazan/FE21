using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Collections;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para realizar las operaciones de adminstracion de retencion/percepcion
    /// </summary>
    class ManteUdoRetencionPercepcion
    {
        /// <summary>
        /// Almacena un nuevo registro en la tabla de retencion/percepecion
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="listaRetencionPercepcion"></param>
        /// <returns></returns>
        public bool Almacenar(ArrayList listaRetencionPercepcion)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFERP");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Recorre la lista de retencion/percepcion
                foreach (RetencionPercepcion retPer in listaRetencionPercepcion)
                {
                    //Establecer los valores para las propiedades
                    dataGeneral.SetProperty("U_SuPas", retPer.SujetoPasivo);
                    dataGeneral.SetProperty("U_ConRet", retPer.ContribuyenteRetenido);
                    dataGeneral.SetProperty("U_Agente", retPer.AgenteResponsable);
                    dataGeneral.SetProperty("U_FormBeta", retPer.FormularioLineaBeta);
                    dataGeneral.SetProperty("U_CodRet", retPer.CodigoRetencion);

                    //Agregar el nuevo registro a la base de datos mediante el serivicio general
                    servicioGeneral.Add(dataGeneral);
                }
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
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Elimina todos los registros de retencion/percepcion existentes
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="listaRetencionPercepcion"></param>
        /// <returns></returns>
        public bool Eliminar(ArrayList listaRetencionPercepcion)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFERP");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                foreach (RetencionPercepcion retPer in listaRetencionPercepcion)
                {
                    //Establecer parametros
                    parametros.SetProperty("DocEntry", retPer.IdRetencionPercepcion);

                    //Eliminar el rango
                    servicioGeneral.Delete(parametros);
                }
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
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }
    }
}
