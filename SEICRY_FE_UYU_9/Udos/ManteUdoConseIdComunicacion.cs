using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoConseIdComunicacion
    {
        /// <summary>
        /// Almacenar un nuevo registro en la tabla TFECOIDCOM
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public bool Almacenar(string conseIdComunicacion)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECOIDCOM");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_ConIdCom", conseIdComunicacion);

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
                consulta = "SELECT U_ConIdCom FROM [@TFECOIDCOM] WHERE DocEntry = (SELECT MAX(DocEntry) FROM [@TFECOIDCOM])";

                //Se realiza la consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    //Obtiene el consecutivo anterior de la consulta
                    resultado = registro.Fields.Item("U_ConIdCom").Value + "";
                    Almacenar(resultado);
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

        /// <summary>
        /// Generar consecutivo
        /// </summary>
        /// <returns></returns>
        public string GenerarIdComunicacion()
        {
            string resultado = "";

            //Se obtiene consecutivo anterior en caso de que exista
            string consecutivo = obtenerConsecutivoAnterior();

            //Caso primer consecutivo
            if (consecutivo.Equals(""))
            {
                resultado = "0000000001";
                //Se inserta el resultado
                Almacenar(resultado);
            }
            else
            {
                try
                {
                    double consec = Convert.ToDouble(consecutivo);
                    //Se incrementa el numero de consecutivo
                    consec += 1;
                    resultado = agregarCeros(consec, 10);
                    //Se inserta el resultado
                    Almacenar(resultado);
                }
                catch (Exception)
                {
                }
            }

            return resultado;
        }

        /// <summary>
        /// Metodo para agregar ceros a la izquierda de un string
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cantCeros"></param>
        /// <returns></returns>
        private string agregarCeros(double num, int cantDigitos)
        {
            string resultado = "", temp = "";
            int j = 0, cant = 0;

            try
            {
                temp = Convert.ToString(num);
                cant = temp.Length;

                while (j < (cantDigitos - cant))
                {
                    temp = "0" + temp;
                    j++;
                }

                resultado = temp;
            }
            catch (Exception)
            {
            }

            return resultado;
        }

    }
}
