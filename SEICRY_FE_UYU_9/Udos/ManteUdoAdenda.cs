using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoAdenda
    {
        /// <summary>
        /// Retorna la adenda para un socio de negocio 
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idSocioNegocio"></param>
        /// <returns></returns>
        public Adenda ObtenerAdenda(Adenda.ESTipoObjetoAsignado tipoObjetoAsignado, string objetoAsignado)
        {
            Adenda adenda = new Adenda();

            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry, U_TipoObjAsig, U_ObjAsig, U_Adenda1 + ' ' + U_Adenda2 + ' ' + U_Adenda3 + ' ' + U_Adenda4 + ' ' + U_Adenda5 + ' ' + U_Adenda6 + ' ' + U_Adenda7 + ' ' + U_Adenda8 + ' ' + U_Adenda9+ ' ' + U_Adenda10 as Adenda  from [@TFEADENDA] where U_TipoObjAsig = '" + tipoObjetoAsignado.ToString() + "' and U_ObjAsig = '" + objetoAsignado + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    adenda.DocEntry = recSet.Fields.Item("DocEntry").Value + "";
                    adenda.TipoObjetoAsignado = (Adenda.ESTipoObjetoAsignado) Enum.Parse(typeof(Adenda.ESTipoObjetoAsignado),( recSet.Fields.Item("U_TipoObjAsig").Value + ""));
                    adenda.ObjetoAsignado = recSet.Fields.Item("U_ObjAsig").Value + "";
                    adenda.CadenaAdenda = recSet.Fields.Item("Adenda").Value + "";
                }
            }
            catch (Exception)
            {
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

            return adenda;
        }

        /// <summary>
        /// Almacena o actualiza la adenda
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="adenda"></param>
        /// <returns></returns>
        public bool AlmacenarAdenda(Adenda adenda)
        {
            bool salida = false;

            adenda.ArregloAdenda = SepararAdenda(adenda.CadenaAdenda);

            if (adenda.DocEntry.Equals(""))
            {
                salida = Almacenar(adenda);
            }
            else
            {
                salida = Actualizar(adenda);
            }

            return salida;
        }

        /// <summary>
        /// Almacena la adenda
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoObjetoAsignado"></param>
        /// <param name="objetoAsignado"></param>
        /// <param name="partesAdenda"></param>
        /// <returns></returns>
        private bool Almacenar(Adenda adenda)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEADENDA");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_TipoObjAsig", adenda.TipoObjetoAsignado.ToString());
                dataGeneral.SetProperty("U_ObjAsig", adenda.ObjetoAsignado);
                dataGeneral.SetProperty("U_Adenda1", adenda.ArregloAdenda[0]);
                dataGeneral.SetProperty("U_Adenda2", adenda.ArregloAdenda[1]);
                dataGeneral.SetProperty("U_Adenda3", adenda.ArregloAdenda[2]);
                dataGeneral.SetProperty("U_Adenda4", adenda.ArregloAdenda[3]);
                dataGeneral.SetProperty("U_Adenda5", adenda.ArregloAdenda[4]);
                dataGeneral.SetProperty("U_Adenda6", adenda.ArregloAdenda[5]);
                dataGeneral.SetProperty("U_Adenda7", adenda.ArregloAdenda[6]);
                dataGeneral.SetProperty("U_Adenda8", adenda.ArregloAdenda[7]);
                dataGeneral.SetProperty("U_Adenda9", adenda.ArregloAdenda[8]);
                dataGeneral.SetProperty("U_Adenda10", adenda.ArregloAdenda[9]);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Add(dataGeneral);

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

        /// <summary>
        /// Almacena la adenda
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoObjetoAsignado"></param>
        /// <param name="objetoAsignado"></param>
        /// <param name="partesAdenda"></param>
        /// <returns></returns>
        private bool Actualizar(Adenda adena)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFEADENDA");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", adena.DocEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_TipoObjAsig", adena.TipoObjetoAsignado.ToString());
                dataGeneral.SetProperty("U_ObjAsig", adena.ObjetoAsignado);
                dataGeneral.SetProperty("U_Adenda1", adena.ArregloAdenda[0]);
                dataGeneral.SetProperty("U_Adenda2", adena.ArregloAdenda[1]);
                dataGeneral.SetProperty("U_Adenda3", adena.ArregloAdenda[2]);
                dataGeneral.SetProperty("U_Adenda4", adena.ArregloAdenda[3]);
                dataGeneral.SetProperty("U_Adenda5", adena.ArregloAdenda[4]);
                dataGeneral.SetProperty("U_Adenda6", adena.ArregloAdenda[5]);
                dataGeneral.SetProperty("U_Adenda7", adena.ArregloAdenda[6]);
                dataGeneral.SetProperty("U_Adenda8", adena.ArregloAdenda[7]);
                dataGeneral.SetProperty("U_Adenda9", adena.ArregloAdenda[8]);
                dataGeneral.SetProperty("U_Adenda10", adena.ArregloAdenda[9]);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
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

        /// <summary>
        /// Separa el texto de la adenda en 10 parte de 254 caracteres maximo
        /// </summary>
        /// <param name="adenda"></param>
        /// <returns></returns>
        private string[] SepararAdenda(string adenda)
        {
            string[] partesAdenda = new string[10];
            int cantCaractProc = 0;
            int cantCaractRest = adenda.Length;
            int cuentaVuelta = 0;

            while (cantCaractRest > 0 && cuentaVuelta < 10)
            {
                if (cantCaractRest >= 254)
                {
                    partesAdenda[cuentaVuelta] = adenda.Substring(cantCaractProc, 254);
                }
                else
                {
                    partesAdenda[cuentaVuelta] = adenda.Substring(cantCaractProc, cantCaractRest);
                }

                cantCaractProc += 254;
                cuentaVuelta++;
                cantCaractRest = cantCaractRest - 254;
            }

            for (int i = 0; i < partesAdenda.Length; i++)
            {
                if (partesAdenda[i] == null)
                {
                    partesAdenda[i] = "";
                }
            }

            return partesAdenda;
        }        
    }
}
