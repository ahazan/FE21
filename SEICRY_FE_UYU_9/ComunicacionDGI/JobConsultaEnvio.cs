using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using System.Collections;
using SEICRY_FE_UYU_9.Conexion;
using System.Xml;
using SEICRY_FE_UYU_9.ACKS;
using System.Threading;
using SAPWSDGI_1;
using SAPbouiCOM;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;


namespace SEICRY_FE_UYU_9.ComunicacionDGI
{

   

    class JobConsultaEnvio
    {
        ManteUdoComprobantes manteUdoComprobantes = new ManteUdoComprobantes();
        ManteUdoSobreTransito manteUdoSobreTransito = new ManteUdoSobreTransito();
        ManteUdoCFE manteUdoCfe = new ManteUdoCFE();
        RespuestaCertificados respuestaCertificado = new RespuestaCertificados();

        //Variable Info Sistema
        private static SAPbouiCOM.Application app = SAPbouiCOM.Framework.Application.SBO_Application;


        /// <summary>
        /// Inicia el proceso de 
        /// </summary>
        /// <param name="parametros"></param>
        public void Trabajar(object parametros)
        {
            Consumir(parametros);
                     
                        
        }


        /// <summary>
        /// Inicia el proceso de 
        /// </summary>
        /// <param name="parametros"></param>
        public void TrabajarCFE_Trancados(object parametros)
        {
            SobresTrancados(parametros);


        }

        /// <summary>
        /// Consume el web service
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public void Consumir(object parametros)
        {
            //while (true)
            //{
                try
                {
                    SAPbouiCOM.Form formularioActivo = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
                    GC.SuppressFinalize(formularioActivo);
                    GC.Collect();

                    List<SobreTransito> listaSobresTransito = manteUdoSobreTransito.ConsultarNoInterfiere(SobreTransito.ETipoReceptor.DGI);

                    foreach (SobreTransito sobreTransito in listaSobresTransito)
                    {
                        ConsultarDGI(parametros, sobreTransito);
                    }

                    Thread.Sleep(30000);	

                }
                catch (Exception)
                {
                    //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("consumir consulta " + ex.ToString());
                }
            //}
                finally
                {
                    Consumir(parametros);
                }
               
        }



        /// <summary>
        /// Consume el web service
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public void SobresTrancados(object parametros)
        {
            //while (true)
            //{
            try
            {
                SAPbouiCOM.Form formularioActivo = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
                GC.SuppressFinalize(formularioActivo);
                GC.Collect();
                              
               
                if (ConsultoPendientes())
                {
                   // app.StatusBar.SetText("Hay CFE para Re-Enviar a DGI, Favor verifique.", BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                    app.MessageBox("Hay CFE para Re-Enviar a DGI, Favor verifique.", 1,"Ok");
                }



                Thread.Sleep(9000000);

            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("consumir consulta " + ex.ToString());
            }
            //}
            finally
            {
                SobresTrancados(parametros);
            }

        }


        private Boolean ConsultoPendientes()
        {
            string consulta = "";
            Recordset recSet = null;
            Boolean resultado = false;

           
            JobEnvioSobreMasivo Usuario = new JobEnvioSobreMasivo();

            //Obtener objeto estandar de record set
            recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);


            //Establecer consulta
            if (Usuario.SuperUsuario())
            {
                consulta = "SELECT CASE WHEN (U_Tipo = '111' OR U_Tipo = '101' OR U_Tipo = '103' OR U_Tipo = '113') THEN " +
                            "(SELECT DocNum FROM OINV WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '112' OR U_Tipo = '102') THEN " +
                            "(SELECT DocNum FROM ORIN WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '181') THEN (SELECT DocNum FROM " +
                            "ODLN WHERE DocEntry = U_DocSap) ELSE U_DocSap END AS 'Número de Documento SAP', U_Tipo AS 'Tipo Documento', " +
                            "U_Serie AS 'Serie', U_Numero AS 'Número CFE', CreateDate AS 'Fecha Creación' FROM [@TFECONSOB]" +
                            "WHERE U_Estado = 'Pendiente' ";
            }
            else
            {
                consulta = "SELECT CASE WHEN (U_Tipo = '111' OR U_Tipo = '101' OR U_Tipo = '103' OR U_Tipo = '113') THEN " +
                            "(SELECT DocNum FROM OINV WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '112' OR U_Tipo = '102') THEN " +
                            "(SELECT DocNum FROM ORIN WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '181') THEN (SELECT DocNum FROM " +
                            "ODLN WHERE DocEntry = U_DocSap) ELSE U_DocSap END AS 'Número de Documento SAP', U_Tipo AS 'Tipo Documento', " +
                            "U_Serie AS 'Serie', U_Numero AS 'Número CFE', CreateDate AS 'Fecha Creación' FROM [@TFECONSOB]" +
                            "WHERE U_Estado = 'Pendiente' AND U_Usuario = '" + ProcConexion.Comp.UserName + "' AND CreateDate BETWEEN '" +
                            DateTime.Now.ToString("yyyy-MM-dd") +
                            "' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            }



            try
            {
                //Ejecutar consulta
                recSet.DoQuery(consulta);


                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = true;
                }


            }
            catch (Exception)
            {

            }

            return resultado;
        }

        private void ConsultarDGI(Object parametros, SobreTransito sobreTransito)
        {
            string xmlConsulta = "";
            string xmlRespuesta = "";

            WebServiceDGI webServiceDgi = new WebServiceDGI(parametros);

            try
            {
                //Armar el xml con los datos de para la consulta                        
                xmlConsulta = "<ConsultaCFE xmlns=\"http://dgi.gub.uy\"> <IdReceptor>" + sobreTransito.IdReceptor + "</IdReceptor><Token>" + sobreTransito.Token + "</Token> </ConsultaCFE>";
                //Invocar el web service                        
                xmlRespuesta = webServiceDgi.WSDGI.SendWSDGI(xmlConsulta, clsWSDGI.WsMethod.Query);

                if (ValidarRespuesta(xmlRespuesta) == false)
                {
                    //Procesar la respuesta
                    respuestaCertificado.ProcesarRespuesta(xmlRespuesta, CFE.ESTipoReceptor.DGI, sobreTransito.DocEntry);
                }
                else
                {
                    // Elimino sobre con error.
                    //manteUdoSobreTransito.Eliminar(sobreTransito.DocEntry);
                }

                   
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("consultarDGI " + ex.ToString());
            }
        }

        /// <summary>
        /// Valida si la respuesta es valida para procesarla
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <returns></returns>
        private bool ValidarRespuesta(string xmlRespuesta)
        {
            bool salida = false;

            try
            {
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlRespuesta);

                string temp = xmlDocumento.GetElementsByTagName("Respuesta").Item(0).InnerText;
                salida = true;
            }
            catch (Exception)
            {
            }

            return salida;
        }
    }
}



