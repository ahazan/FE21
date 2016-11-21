using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Xml;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para la adminstracion del udo de Sobres. 
    /// </summary>
    class ManteUdoSobre
    {
        #region FUNCIONES

        /// <summary>
        ///  Ingresa un nuevo registro a la tabla @TFESOBRE.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idSobre"></param>
        /// <param name="nombreSobre"></param>
        /// <param name="fechaSobre"></param>
        /// <returns></returns>
        public bool Almacenar(string idSobre, string nombreSobre, string fechaSobre)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFESOBRE");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Asiganar valor a cada una de las caracteristicas del udo
                dataGeneral.SetProperty("U_IDSobre", idSobre);
                dataGeneral.SetProperty("U_NomSobre", nombreSobre);
                dataGeneral.SetProperty("U_FecSobre", fechaSobre);

                //Agregar el nuevo registro a la base de datos mediante el servicio general de la compañia
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
                    //Liberar memoria utilizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFESOB y su hija
        /// @TFESOBDET.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="maestro"></param>
        /// <returns></returns>
        public bool AlmacenarMaestro(SobreReporte sobre)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralData dataDetalle = null;
            GeneralDataCollection detalle = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFESOB");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_VerSobre", sobre.Version);
                dataGeneral.SetProperty("U_RucRec", sobre.RucReceptor.ToString());
                dataGeneral.SetProperty("U_RucEmi", sobre.RucEmisor.ToString());
                dataGeneral.SetProperty("U_IdResp", sobre.IdRespuesta.ToString());
                dataGeneral.SetProperty("U_NomArc", sobre.NombreArchivo);
                dataGeneral.SetProperty("U_FeHoEnRe", sobre.FechaHoraRecepcion);
                dataGeneral.SetProperty("U_IdEmi", sobre.IdEmisor.ToString());
                dataGeneral.SetProperty("U_IdRec", sobre.IdReceptor.ToString());
                dataGeneral.SetProperty("U_CantComp", sobre.CantidadComprobantes);
                dataGeneral.SetProperty("U_FeHoFiEl", sobre.FechaHoraFirma);

                if (sobre.DetalleSobre != null)
                { 
                    detalle = dataGeneral.Child("TFESOBDET");

                    foreach (DetSobre detalleSobre in sobre.DetalleSobre)
                    {
                        dataDetalle = detalle.Add();
                        dataDetalle.SetProperty("U_EstRecEnv", detalleSobre.EstadoRecepcion);
                        dataDetalle.SetProperty("U_CodMotRec", detalleSobre.CodigoMotivoRechazo);
                        dataDetalle.SetProperty("U_GloMotRec", detalleSobre.GlosaMotivoRechazo);
                        dataDetalle.SetProperty("U_DetRec", detalleSobre.DetalleRechazo);
                    }
                }

                //Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
                AdminEventosUI.mostrarMensaje("Error: Al guardar información del Sobre" + ex.ToString(), AdminEventosUI.tipoMensajes.error);
            }
            finally
            {
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por el objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (dataDetalle != null)
                {
                    //Liberar memoria utlizada por el objeto dataDetalle
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataDetalle);
                    System.GC.Collect();
                }
                if (detalle != null)
                {
                    //Liberar memoria utlizada por el objeto detalle
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(detalle);
                    System.GC.Collect();
                }
                if (servicioGeneral != null)
                {
                    ////Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Metodo para cargar un sobre desde un archivo xml
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="nombreSobre"></param>
        /// <param name="estado">true = AS, false = BS</param>
        public void cargarSobreXml(XmlDocument documento, string nombreSobre, bool estado)
        {
            try
            {
                XmlNodeList sobre = documento.GetElementsByTagName("ACKSobre");
                XmlNodeList listaNodos = ((XmlElement)sobre[0]).GetElementsByTagName("Caratula");
                XmlNodeList listaDetalles = ((XmlElement)sobre[0]).GetElementsByTagName("Detalle");

                SobreReporte sobreReporte = new SobreReporte();
                              
                foreach (XmlElement nodo in listaNodos)
                {
                    sobreReporte.Version = "0";
                   

                    sobreReporte.RucReceptor =  long.Parse(nodo.GetElementsByTagName("RUCReceptor")[0].InnerText);


                    if (nodo.GetElementsByTagName("RUCEmisor")[0].InnerText != "")
                        {
                             sobreReporte.RucEmisor = long.Parse(nodo.GetElementsByTagName("RUCEmisor")[0].InnerText);
                        }
                    else
                    {
                        sobreReporte.RucEmisor = long.Parse("000000000000");
                    }
                                   
                    sobreReporte.IdRespuesta = long.Parse(nodo.GetElementsByTagName("IDRespuesta")[0].InnerText);
                    sobreReporte.NombreArchivo = nombreSobre + ".xml";
                    sobreReporte.FechaHoraRecepcion = nodo.GetElementsByTagName("FecHRecibido")[0].InnerText;
                    sobreReporte.IdEmisor = long.Parse(nodo.GetElementsByTagName("IDEmisor")[0].InnerText);
                    sobreReporte.IdReceptor = long.Parse(nodo.GetElementsByTagName("IDReceptor")[0].InnerText);
                    sobreReporte.CantidadComprobantes = int.Parse(nodo.GetElementsByTagName("CantidadCFE")[0].InnerText);
                    sobreReporte.FechaHoraFirma = nodo.GetElementsByTagName("Tmst")[0].InnerText;

                    if (!estado)
                    {
                        sobreReporte.DetalleSobre = new List<DetSobre>();
                        DetSobre detalleSobre = new DetSobre();

                        foreach (XmlElement detalle in listaDetalles)
                        {
                            if (detalle.ChildNodes != null)
                            {
                                if (detalle.ChildNodes.Count > 1)
                                {
                                    detalleSobre.EstadoRecepcion = detalle.GetElementsByTagName("Estado")[0].InnerText;
                                    XmlNodeList listaGlosa = detalle.GetElementsByTagName("MotivosRechazo");

                                    foreach (XmlElement glosa in listaGlosa)
                                    {
                                        detalleSobre.CodigoMotivoRechazo = glosa.GetElementsByTagName("Motivo")[0].InnerText;
                                        detalleSobre.GlosaMotivoRechazo = glosa.GetElementsByTagName("Glosa")[0].InnerText;
                                        detalleSobre.DetalleRechazo = glosa.GetElementsByTagName("Detalle")[0].InnerText;
                                    }
                                }
                            }
                        }
                        sobreReporte.DetalleSobre.Add(detalleSobre);
                    }

                    sobreReporte.IdSobre = nombreSobre + ".xml";

                    AlmacenarMaestro(sobreReporte);
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("cargarSobreml/ " + ex.ToString());
            }
        }

        /// <summary>
        /// Obtiene el nombre de un sobre en base al id de receptor
        /// </summary>
        /// <param name="idReceptor"></param>
        /// <returns></returns>
        public string obtenerNombre(string idReceptor)
        {
            string resultado = "";
            Recordset nombre = null;

            try
            {
                nombre = Conexion.ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                Conexion.ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                nombre.DoQuery("SELECT U_IdSobre FROM [@TFESOB] WHERE U_IdResp = '" + idReceptor + "'");

                if (nombre.RecordCount > 0)
                {
                    resultado = nombre.Fields.Item("U_IdSobre").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (nombre != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(nombre);
                    GC.Collect();
                }
            }

            return resultado;
        }

        #endregion FUNCIONES
    }
}
