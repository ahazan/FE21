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
    /// Contiene todos lo metodos para la administracion de los datos de conexion con el servidor FTP
    /// </summary>
    class ManteUdoFTP
    {
        //Variable para controlar los mensajes de error del FTP
        public static bool errorManteFTP = false;

        /// <summary>
        /// Almacenar un nuevo registro en la base datos
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="emisor"></param>
        /// <returns></returns>
        public bool Almacenar(ConfigFTP configFtp)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECONFTP");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                dataGeneral.SetProperty("U_Serv", configFtp.Servidor);
                dataGeneral.SetProperty("U_RepComp", configFtp.RepoComp);
                dataGeneral.SetProperty("U_RepSob", configFtp.RepoSob);
                dataGeneral.SetProperty("U_RepBan", configFtp.RepoBandejaEntrada);
                dataGeneral.SetProperty("U_RepConCom", configFtp.RepoContingenciaComprobantes);
                dataGeneral.SetProperty("U_RepConReDi", configFtp.RepoContingenciaReportesDiarios);
                dataGeneral.SetProperty("U_RepConSob", configFtp.RepoContingenciaSobres);
                dataGeneral.SetProperty("U_Usuario", configFtp.Usuario);
                dataGeneral.SetProperty("U_Clave", configFtp.Clave);
                dataGeneral.SetProperty("U_RepResp", configFtp.RepoResp);
                dataGeneral.SetProperty("U_RepRepDi", configFtp.RepoRepDi);
                dataGeneral.SetProperty("U_RutWsE", configFtp.RepoWebServiceEnvio);
                dataGeneral.SetProperty("U_RutWsC", configFtp.RepoWebServiceConsulta);
                dataGeneral.SetProperty("U_RepCfe", configFtp.RepoCFEs);
                dataGeneral.SetProperty("U_RepCerAnu", configFtp.RepoCertificadosAnulados);
                dataGeneral.SetProperty("U_ConSobDgi", configFtp.RepoContingenciaSobreDgi);
                dataGeneral.SetProperty("U_FilePcDel", configFtp.FileDelete);
               // dataGeneral.SetProperty("U_RUT", configFtp.RutDgi);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Add(dataGeneral);

                //Liberar memoria utlizada por dataGeneral
                System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                System.GC.Collect();

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
        /// Actualiza los datos de la configuracion de conexion al servidor FTP
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="configFtp"></param>
        /// <returns></returns>
        public bool Actualizar(ConfigFTP configFtp)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECONFTP");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", "1");

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                dataGeneral.SetProperty("U_Serv", configFtp.Servidor);
                dataGeneral.SetProperty("U_RepComp", configFtp.RepoComp);
                dataGeneral.SetProperty("U_RepSob", configFtp.RepoSob);
                dataGeneral.SetProperty("U_RepBan", configFtp.RepoBandejaEntrada);
                dataGeneral.SetProperty("U_RepResp", configFtp.RepoResp);
                dataGeneral.SetProperty("U_RepRepDi", configFtp.RepoRepDi);
                dataGeneral.SetProperty("U_RepConSob", configFtp.RepoContingenciaSobres);
                dataGeneral.SetProperty("U_RepConCom", configFtp.RepoContingenciaComprobantes);
                dataGeneral.SetProperty("U_RepConReDi", configFtp.RepoContingenciaReportesDiarios);
                dataGeneral.SetProperty("U_Usuario", configFtp.Usuario);
                dataGeneral.SetProperty("U_Clave", configFtp.Clave);
                dataGeneral.SetProperty("U_RutWsE", configFtp.RepoWebServiceEnvio);
                dataGeneral.SetProperty("U_RutWsC", configFtp.RepoWebServiceConsulta);
                dataGeneral.SetProperty("U_RepCfe" ,configFtp.RepoCFEs);
                dataGeneral.SetProperty("U_RepCerAnu", configFtp.RepoCertificadosAnulados);
                dataGeneral.SetProperty("U_ConSobDgi", configFtp.RepoContingenciaSobreDgi);
                dataGeneral.SetProperty("U_ConSobDgi", configFtp.RepoContingenciaSobreDgi);
                dataGeneral.SetProperty("U_FilePcDel", configFtp.FileDelete);
                ///dataGeneral.SetProperty("U_RUT", configFtp.RutDgi);

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
                    //Liberar memoria utlizada por parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por dataGeneral
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
        /// Realiza la consulta de la configuracion para la conexion FTP almacenada 
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public ConfigFTP Consultar()
        {
            ConfigFTP configFtp = null;

            //Obtener objeto estandar de record set 
            Recordset recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                //Establecer consulta
                string consulta = "SELECT U_Serv, U_RepComp, U_RepSob, U_RepBan, U_ConSobDgi, U_RepConSob, U_RepConCom, U_RepConReDi, U_Usuario, U_Clave, U_RepResp, U_RepRepDi, U_RutWsE, U_RutWsC, U_RepCfe, U_RepCerAnu, U_FilePcDel FROM [@TFECONFTP]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Crea una instancia del objeto configuracion ftp
                    configFtp = new ConfigFTP();

                    //Establece las propiedaes al objeto configuracion ftp
                    configFtp.Servidor = recSet.Fields.Item("U_Serv").Value + "";
                    configFtp.RepoComp = recSet.Fields.Item("U_RepComp").Value + "";
                    configFtp.RepoSob = recSet.Fields.Item("U_RepSob").Value + "";
                    configFtp.RepoBandejaEntrada = recSet.Fields.Item("U_RepBan").Value + "";
                    configFtp.RepoContingenciaSobres = recSet.Fields.Item("U_RepConSob").Value + "";
                    configFtp.RepoContingenciaComprobantes = recSet.Fields.Item("U_RepConCom").Value + "";
                    configFtp.RepoContingenciaReportesDiarios = recSet.Fields.Item("U_RepConReDi").Value + "";
                    configFtp.Usuario = recSet.Fields.Item("U_Usuario").Value + "";
                    configFtp.Clave = recSet.Fields.Item("U_Clave").Value + "";
                    configFtp.RepoResp = recSet.Fields.Item("U_RepResp").Value + "";
                    configFtp.RepoRepDi = recSet.Fields.Item("U_RepRepDi").Value + "";
                    configFtp.RepoWebServiceEnvio = recSet.Fields.Item("U_RutWsE").Value + "";
                    configFtp.RepoWebServiceConsulta = recSet.Fields.Item("U_RutWsC").Value + "";
                    configFtp.RepoCFEs = recSet.Fields.Item("U_RepCfe").Value + "";
                    configFtp.RepoCertificadosAnulados = recSet.Fields.Item("U_RepCerAnu").Value + "";
                    configFtp.RepoContingenciaSobreDgi = recSet.Fields.Item("U_ConSobDgi").Value + "";
                    configFtp.FileDelete = recSet.Fields.Item("U_FilePcDel").Value + "";


                    
                }
            }
            catch (Exception)
            {
                //Se activa bandera de error
                errorManteFTP = true;
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
            return configFtp;
        
        }

        /// <summary>
        /// Consulta las direcciones de los web services de la DGI
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public ConfigFTP ConsultarURLWebService()
        {
            ConfigFTP configFtp = null;

            //Obtener objeto estandar de record set 
            Recordset recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                //Establecer consulta
                string consulta = "SELECT U_RutWsE, U_RutWsC FROM [@TFECONFTP]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Crea una instancia del objeto configuracion ftp
                    configFtp = new ConfigFTP();

                    //Establece las propiedaes al objeto configuracion ftp
                    configFtp.RepoWebServiceEnvio = recSet.Fields.Item("U_RutWsE").Value + "";
                    configFtp.RepoWebServiceConsulta = recSet.Fields.Item("U_RutWsC").Value + "";
                }
            }
            catch (Exception)
            {
                //Se activa bandera de error
                errorManteFTP = true;
                configFtp = null;
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
            return configFtp;
        }

        /// <summary>
        /// Valida que ya exista una configuracion almacenada. Si existe se debe actulizar si no, se almancena un nuevo registro. 
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool ExisteConfiguracion()
        {
            Recordset recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);            
            bool resultado = false;

            try
            {                
                //Establecer consulta
                string consulta = "SELECT DocEntry FROM [@TFECONFTP]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    resultado = true;
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
    }
}
