using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para administrar los datos de los tipos de documentos que se deben conservar
    /// </summary>
    class ManteUdoDocCon
    {
        /// <summary>
        /// Almacena un nuevo registro en la tabla de tipos de documentos a conservar
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tiposDocumentosConservar"></param>
        /// <returns></returns>
        public bool Almacenar(CAE cae)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFETDCON");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_TipoDoc", ((int)cae.TipoCFE).ToString());
                dataGeneral.SetProperty("U_NombDoc", cae.NombreDocumento);
                dataGeneral.SetProperty("U_IndCon", cae.IndicadorConservar);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
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
        /// Actualiza los datos de un tipo de documento a conservar determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="cae"></param>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        public bool Actualizar(CAE cae, string numeroRegistro)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFETDCON");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", numeroRegistro);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_TipoDoc", ((int)cae.TipoCFE).ToString());
                dataGeneral.SetProperty("U_NombDoc", cae.NombreDocumento);
                dataGeneral.SetProperty("U_IndCon", cae.IndicadorConservar);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
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
