using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoConseIdEmisor
    {
        /// <summary>
        /// Almacenar un nuevo registro en la tabla TFECOIDEMI
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public bool Almacenar(string conseIdEmisor)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECOIDEMI");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_ConIdEmi", conseIdEmisor);

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
        /// Devuelve el consecutivo anterior
        /// </summary>
        /// <returns></returns>
        public string obtenerConsecutivoAnterior()
        {
            string resultado = "", consulta = "";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                //Consulta devuelve el ultimo consecutivo anterior
                consulta = "SELECT U_ConIdEmi FROM [@TFECOIDEMI] WHERE DocEntry = (SELECT MAX(DocEntry) FROM [@TFECOIDEMI])";

                //Se realiza la consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    //Obtiene el consecutivo anterior de la consulta
                    resultado = registro.Fields.Item("U_ConIdEmi").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            finally
            {
                if (registro != null)
                {
                    //Se libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }
    }
}
