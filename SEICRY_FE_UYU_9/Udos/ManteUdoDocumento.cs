using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoDocumento
    {
        /// <summary>
        /// Obtiene una lista de los CAEs activos
        /// </summary>
        /// <returns></returns>
        public List<ValidacionCAE> ObtenerListaCAEs()
        {
            List<ValidacionCAE> CAEs = new List<ValidacionCAE>();
            Recordset registro = null;
            string consulta = "SELECT U_TipoDoc, U_NumFin, U_NumAct, U_ValDesde, U_ValHasta FROM [@TFERANGO] WHERE U_Activo = 'Y'";
            int i = 0;

            try
            {
                //Obtener objeto estadar de record set
                registro = Conexion.ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Ejecutar consulta 
                registro.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                if (registro.RecordCount > 0)
                {
                    //Se obtiene la informacion de la base de datos
                    while (i < registro.RecordCount)
                    {
                        ValidacionCAE validacionCAE = new ValidacionCAE();

                        validacionCAE.TipoDocumento = registro.Fields.Item("U_TipoDoc").Value + "";
                        validacionCAE.NumeroFinal = int.Parse(registro.Fields.Item("U_NumFin").Value + "");
                        validacionCAE.NumeroActual = int.Parse(registro.Fields.Item("U_NumAct").Value + "");
                        validacionCAE.ValidoDesde = registro.Fields.Item("U_ValDesde").Value + "";
                        validacionCAE.ValidoHasta = registro.Fields.Item("U_ValHasta").Value + "";

                        CAEs.Add(validacionCAE);

                        registro.MoveNext();
                        i++;
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

            return CAEs;
        }

        /// <summary>
        /// Obtiene el RUT a utilizar en los comprobantes fiscales
        /// </summary>
        /// <returns></returns>
        public string ObtenerRut()
        {
            string salida = "", consulta = "SELECT U_RUT FROM [@TFEADOBE]";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);
                if (registro.RecordCount > 0)
                {
                    salida = registro.Fields.Item("U_RUT").Value + "";
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
            return salida;
        }
    }
}
