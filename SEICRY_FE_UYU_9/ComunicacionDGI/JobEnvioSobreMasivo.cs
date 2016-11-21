using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.XML;
using SEICRY_FE_UYU_9.ZonasCFE;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SAPWSDGI_1;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;
using System.Security.Cryptography;
using SEICRY_FE_UYU_9.Firma_Digital;



namespace SEICRY_FE_UYU_9.ComunicacionDGI
{
    class JobEnvioSobreMasivo
    {
        //Valores globales de la firma digital
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";
       
        SAPbouiCOM.Application app = SAPbouiCOM.Framework.Application.SBO_Application;

        #region CONSUMO

        /// <summary>
        /// Inicia el proceso de 
        /// </summary>
        /// <param name="parametros"></param>
        public void Trabajar(object parametros)
        {
            ControlSobres controlSobre = new ControlSobres();

            try
            {
                

                string respuesta = Consumir(parametros, out controlSobre, SuperUsuario());

                if (!respuesta.Equals(""))
                {
                    ProcesarRespuesta(respuesta, parametros, controlSobre);
                }
            }
            catch (Exception)
            {
            }

            finally
            {

                if (controlSobre != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(controlSobre);
                    GC.Collect();
                }


            }

        }



        public Boolean SuperUsuario()
        {
            string consulta = string.Empty, SuperUser = string.Empty;
            Recordset registro = null;
            Boolean Respuesta = false;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select T1.U_SuperUser  from OUSR T1 inner join OUBR T2 ON T1.Branch = T2.Code WHERE T1.U_NAME = '" + Conexion.ProcConexion.Comp.UserName + "'" + "or T1.USER_CODE = '" + Conexion.ProcConexion.Comp.UserName + "'";

                //Ejecutar consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    SuperUser = registro.Fields.Item("U_SuperUser").Value + "";
                }


                if (SuperUser.Equals("Y"))
                {
                    Respuesta = true;
                }


            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return Respuesta;
        }

       



        /// <summary>
        /// Consume el web service
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public string Consumir(object parametros, out ControlSobres controlSobres, Boolean SuperUser)
        {
            string respuesta = "";          

            WebServiceDGIMasivo webServiceDgi = new WebServiceDGIMasivo(parametros);
            ParametrosJobWsDGIMasivo parametrosJobWsDGIMasivo = parametros as ParametrosJobWsDGIMasivo;

            XmlDocument xmlDocumento = new XmlDocument();
            ManteUdoControlSobres manteUdoControlSobres = new ManteUdoControlSobres();
            controlSobres = new ControlSobres();

            try
            {


                if (SuperUser)
                {
                    FTP ftp = new FTP();
                                       

                    if (ftp.descargarArchivos(parametrosJobWsDGIMasivo.NombreSobre + ".xml", RutasCarpetas.RutaCarpetaSobresDgi, 0))
                    {

                    }
                    else if (ftp.descargarArchivos(parametrosJobWsDGIMasivo.NombreSobre + ".xml", RutasCarpetas.RutaCarpetaContingenciaSobresDgi, 5))
                    {

                    }

                }

                if (File.Exists((RutasCarpetas.RutaCarpetaSobresDgi + "\\" + parametrosJobWsDGIMasivo.NombreSobre + ".xml")))
                {
                    xmlDocumento.Load(RutasCarpetas.RutaCarpetaSobresDgi + "\\" + parametrosJobWsDGIMasivo.NombreSobre + ".xml");
                }
                else if (File.Exists((RutasCarpetas.RutaCarpetaContingenciaSobresDgi + "\\" + parametrosJobWsDGIMasivo.NombreSobre + ".xml")))
                {
                    xmlDocumento.Load(RutasCarpetas.RutaCarpetaContingenciaSobresDgi + "\\" + parametrosJobWsDGIMasivo.NombreSobre + ".xml");
                }

                

                controlSobres.Tipo = parametrosJobWsDGIMasivo.Tipo + "";
                controlSobres.Serie = parametrosJobWsDGIMasivo.Serie;
                controlSobres.Numero = parametrosJobWsDGIMasivo.Numero + "";
                controlSobres.Estado = "Enviado";
                controlSobres.UsuarioSap = ProcConexion.Comp.UserName;
                controlSobres = manteUdoControlSobres.ObtenerDocEntry(controlSobres);
                manteUdoControlSobres.Actualizar(controlSobres);
                manteUdoControlSobres.ActualizarFirmaElectronica(controlSobres);
                           

               // RE firmo el documento con la fecha del Envio
                    FirmaDigital firma = new FirmaDigital();
                 xmlDocumento = firma.refirmarDocumentos(xmlDocumento);


                // Envio el documento re firmado.
                respuesta = webServiceDgi.WSDGI.SendWSDGI(xmlDocumento.InnerXml, clsWSDGI.WsMethod.Envelope);

                if (!respuesta.Equals(""))
                {
                    respuesta = respuesta.Replace("<IDEmisor>0</IDEmisor>", "<IDEmisor>" + xmlDocumento.GetElementsByTagName("DGICFE:Idemisor").Item(0).InnerText + "</IDEmisor>");
                    //manteUdoControlSobres.Eliminar(controlSobres);
                }
            }
            catch (Exception)
            {

                if (controlSobres.Tipo != "" && controlSobres.Numero != "0" && controlSobres.Serie != "")
                {
                    controlSobres.Estado = "Pendiente";
                }

                else
                {
                    controlSobres.Estado = "NoAplica";
                }

                manteUdoControlSobres.Actualizar(controlSobres);               
                respuesta = "";
            }

            finally
            {

                if (webServiceDgi != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(webServiceDgi);
                    GC.Collect();
                }


                if (parametrosJobWsDGIMasivo != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(parametrosJobWsDGIMasivo);
                    GC.Collect();
                }

                if (xmlDocumento != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(xmlDocumento);
                    GC.Collect();
                }

                if (manteUdoControlSobres != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(manteUdoControlSobres);
                    GC.Collect();
                }


                if (controlSobres != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(controlSobres);
                    GC.Collect();
                }

            }

            return respuesta;
        }        

     


        #endregion CONSUMO

        #region RESPUESTA

        /// <summary>
        /// Procesa la respuesta del web service de DGI
        /// </summary>
        /// <param name="xmlRespuesta"></param>
        /// <param name="parametros"></param>
        public void ProcesarRespuesta(string xmlRespuesta, object parametros, ControlSobres controlSobres)
        {
            string estadoRespuesta = "", token = "", idReceptor = "";
            ParametrosJobWsDGIMasivo parametrosJob = parametros as ParametrosJobWsDGIMasivo;
            ManteUdoControlSobres manteUdoControlSobres = new ManteUdoControlSobres();
            ManteUdoCFE manteUdoCfe = new ManteUdoCFE();

            try
            {
                XmlDocument xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlRespuesta);

                idReceptor = xmlDocumento.GetElementsByTagName("IDReceptor").Item(0).InnerText;
                estadoRespuesta = xmlDocumento.GetElementsByTagName("Estado").Item(0).InnerText;

                if (estadoRespuesta.Equals("AS"))
                {
                    manteUdoControlSobres.Eliminar(controlSobres);
                    token = xmlDocumento.GetElementsByTagName("Token").Item(0).InnerText;
                    string hora = xmlDocumento.GetElementsByTagName("Fechahora").Item(0).InnerText;

                    SobreTransito sobreTransito = new SobreTransito();
                    sobreTransito.NombreSobre = parametrosJob.NombreSobre;
                    sobreTransito.Token = token;
                    sobreTransito.IdReceptor = idReceptor;
                    sobreTransito.TipoReceptor = SobreTransito.ETipoReceptor.DGI;

                    sobreTransito.Serie = parametrosJob.Serie ;
                    sobreTransito.Numero = parametrosJob.Numero;
                    sobreTransito.Tipo = parametrosJob.Tipo ;


                    ManteUdoSobreTransito manteTransito = new ManteUdoSobreTransito();
                    manteTransito.Almacenar(sobreTransito);

                    ManteUdoSobre manteSobre = new ManteUdoSobre();
                    //True = AS, no se agrega detalle correspondiente a info de rechazo
                    manteSobre.cargarSobreXml(xmlDocumento, parametrosJob.NombreSobre, true);
                }
                else if (estadoRespuesta.Equals("BS"))
                {
                    manteUdoControlSobres.Eliminar(controlSobres);
                    ManteUdoSobre manteSobre = new ManteUdoSobre();
                    //False = BS, se agrega detalle correspondiente a info de rechazo
                    manteSobre.cargarSobreXml(xmlDocumento, parametrosJob.NombreSobre, false);
                }
            }
            catch (Exception)
            {
                controlSobres.Estado = "Pendiente";
                manteUdoControlSobres.Actualizar(controlSobres);
                app.MessageBox("No se pudo enviar a DGI");
            }

            finally
            {

                if (parametrosJob != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(parametrosJob);
                    GC.Collect();
                }

                if (manteUdoControlSobres != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(manteUdoControlSobres);
                    GC.Collect();
                }


                if (manteUdoCfe != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(manteUdoCfe);
                    GC.Collect();
                }

            }
        }

        #endregion RESPUESTA
    }
}
