using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Security.Cryptography.X509Certificates;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoCertificadoDigital
    {
        public static bool errorCertificado = false;

        /// <summary>
        /// Almacena los datos de la tabla TFECERT
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="ruta"></param>
        /// <param name="pass"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool AlmacenarTFECERT(string ruta, string pass)
        {
            bool salida = true;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            if (comprobarCertificado(ruta, pass))
            {
                try
                {
                    string[] borrar = consultaBorrar();
                    if (borrar != null)
                    {
                        Eliminar(borrar);
                    }

                    //Obtener servicio general de la compañia
                    servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECERT");

                    //Apuntar a la cabecera del udo
                    dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                    //Establecer los valores para las propiedades
                    dataGeneral.SetProperty("U_RutaCer", ruta);
                    dataGeneral.SetProperty("U_ClaveCer", pass);
                    dataGeneral.SetProperty("U_UsuarioCer", ProcConexion.Comp.UserName);

                    //Agregar el nuevo registro a la base de datos mediante el serivicio general
                    servicioGeneral.Add(dataGeneral);
                }
                catch (Exception)
                {
                    salida = false;
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
            }
            else
            {
                errorCertificado = true;
                salida = false;
            }

            return salida;
        }

        /// <summary>
        /// Consulta los datos de la firma digital configurados para un usuario determinado.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Certificado Consultar()
        {
            Certificado certificado = null;
            Recordset recSet= null;
            string consulta;

            try
            {
                //Obtener objeto estandar de record set 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_RutaCer, U_ClaveCer FROM [@TFECERT]";//  WHERE U_UsuarioCer = '" + comp.UserName + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Ubicar el record set en la ultima posicion
                recSet.MoveLast();

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Crea una instancia del objeto configuracion ftp
                    certificado = new Certificado();

                    //Establece las propiedaes al objeto configuracion ftp
                    certificado.RutaCertificado = recSet.Fields.Item("U_RutaCer").Value + "";
                    certificado.Clave = recSet.Fields.Item("U_ClaveCer").Value + "";
                }
            }
            catch (Exception)
            {
                certificado = null;
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
            return certificado;
        }

        /// <summary>
        /// Elimina un rango determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        private bool Eliminar(string[] listaDocEntry)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECERT");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                foreach (string docEntry in listaDocEntry)
                {
                    //Establecer parametros
                    parametros.SetProperty("DocEntry", docEntry);
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
                    //Liberar memoria utlizada por objeto parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
                if(servicioGeneral != null)
                {
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Metodo para obtener los docEntry de los registros a borrar
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        private string[] consultaBorrar()
        {
            Recordset recSet = null;
            string[] resultado = null;
            string consulta = "";            
            int i = 0;

            try
            {
                //Obtener objeto estandar de record set 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT docEntry FROM [@TFECERT]";// WHERE U_UsuarioCer = '" + comp.UserName + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    recSet.MoveFirst();
                    resultado = new string[recSet.RecordCount];

                    while (i < recSet.RecordCount)
                    {
                        resultado[i] = recSet.Fields.Item("docEntry").Value.ToString();
                        recSet.MoveNext();
                        i++;
                    }                    
                }
            }
            catch (Exception)
            {
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
            return resultado;
        }

        /// <summary>
        /// Metodo para obtener la ruta del certificado
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ObtenerRutaCertificado()
        {
            Recordset recSet = null;
            string resultado = "", consulta = "", usuario = ProcConexion.Comp.UserName;

            try
            {
                //Obtener objeto estandar de record set 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_RutaCer FROM [@TFECERT]"; //WHERE U_UsuarioCer = '" + usuario + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("U_RutaCer").Value;
                }
            }
            catch (Exception)
            {
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
            return resultado;
        }

        /// <summary>
        /// Metodo para obtener la clave del certificado digital
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ObtenerPassCertificado()
        {
            Recordset recSet = null;
            string resultado = "", consulta = "",  usuario = ProcConexion.Comp.UserName;

            try
            {
                //Obtener objeto estandar de record set 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_ClaveCer FROM [@TFECERT]";// WHERE U_UsuarioCer = '" + usuario + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("U_ClaveCer").Value;
                }
            }
            catch(Exception)
            {            
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
            return resultado;
        }

        /// <summary>
        /// Comprueba que exista la ruta del certificado
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private bool comprobarCertificado(string ruta, string pass)
        {
            bool respuesta = false;

            try
            {
                X509Certificate2 objCert = new X509Certificate2(ruta, pass);
                respuesta = true;
            }
            catch(Exception)
            {            
            }

            return respuesta;
        }
    }
}
