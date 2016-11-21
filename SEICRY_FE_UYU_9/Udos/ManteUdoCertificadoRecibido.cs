using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Collections;
using System.Xml;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SEICRY_FE_UYU_9.Firma_Digital;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    class ManteUdoCertificadoRecibido
    {
        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFESOBREC y su hija
        /// @TFESOBRECDET.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="maestro"></param>
        /// <returns></returns>
        public bool AlmacenarMaestro(CertificadoRecibido certificado, string correo, string nombreSobre)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralData dataDetalle = null;
            GeneralDataCollection detalle = null;

            try
            {
                //Obtener el servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFESOBREC");

                //Apuntar a la cabecera del udo
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Establecer los valores para cada una de las propiedades del udo
                dataGeneral.SetProperty("U_RucEmi", certificado.RucEmisor);
                dataGeneral.SetProperty("U_RucRec", certificado.RucReceptor);
                dataGeneral.SetProperty("U_FeSob", certificado.FechaSobre);
                dataGeneral.SetProperty("U_FeFir", certificado.FechaFirma);
                dataGeneral.SetProperty("U_TipoCFE", certificado.TipoCFE);
                dataGeneral.SetProperty("U_NumCom", certificado.NumeroComprobante);
                dataGeneral.SetProperty("U_SerCom", certificado.SerieComprobante);
                dataGeneral.SetProperty("U_DNroCAE", certificado.DNroCAE);
                dataGeneral.SetProperty("U_HNroCAE", certificado.HNroCAE);
                dataGeneral.SetProperty("U_FVenCAE", certificado.FVenCAE);
                dataGeneral.SetProperty("U_IdCons", certificado.IdConsecutio);
                dataGeneral.SetProperty("U_IdEmisor", certificado.IdEmisor);
                dataGeneral.SetProperty("U_NomSob", nombreSobre);
                dataGeneral.SetProperty("U_CorEmi", correo);
                dataGeneral.SetProperty("U_CanCfe", certificado.CantCFE);
                dataGeneral.SetProperty("U_RazSoc", certificado.RazonSocial);

                detalle = dataGeneral.Child("TFESOBRECDET");

                foreach (DetCertificadoRecibido certificadoDetalle in certificado.DetalleCertificadoRecibido)
                {
                    dataDetalle = detalle.Add();
                    dataDetalle.SetProperty("U_NumCom", certificadoDetalle.NumeroComprobante);
                    dataDetalle.SetProperty("U_SerCom", certificadoDetalle.SerieComprobante);
                    dataDetalle.SetProperty("U_TipCFE", certificadoDetalle.TipoCFE);
                    dataDetalle.SetProperty("U_NomItem", certificadoDetalle.NombreItem);
                    string temp = certificadoDetalle.Cantidad.ToString();
                    temp = temp.Replace(".", ",");
                    dataDetalle.SetProperty("U_Cant", temp);//
                    dataDetalle.SetProperty("U_PreUni", certificadoDetalle.PrecioUnitario);
                    dataDetalle.SetProperty("U_MonIte", certificadoDetalle.MontoItem);
                    dataDetalle.SetProperty("U_TpoMon",certificadoDetalle.TipoMoneda);
                }

                ////Agregar el nuevo registro a la base de dato utilizando el servicio general de la compañia
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ManteUdoCertificadoRecibido/Error: " + ex.ToString());
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
                    //Liberar memoria utlizada por el objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza es valor de aprobado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="docEntry"></param>
        /// <param name="aprobado"></param>
        /// <returns></returns>
        public bool ActualizarEstado(string docEntry, string aprobado)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFESOBREC");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_Aprobado", aprobado);

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Update(dataGeneral);

                resultado = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (servicioGeneral != null)
                {
                    //Liberar memoria utlizada por objeto servicioGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(servicioGeneral);
                    System.GC.Collect();
                }
                if (dataGeneral != null)
                {
                    //Liberar memoria utlizada por objeto dataGeneral
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataGeneral);
                    System.GC.Collect();
                }
                if (parametros != null)
                {
                    //Liberar memoria utlizada por objeto parametros
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(parametros);
                    System.GC.Collect();
                }
            }
            return resultado;
        }        

        /// <summary>
        /// Consulta el estado de cada CFE asociado uno id de respuesta determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="token"></param>
        /// <param name="idRespuesta"></param>
        /// <returns></returns>
        public ArrayList ConsultarCFEProcesado(string token, string idRespuesta)
        {
            string consulta = "";
            ArrayList listaComporobantes = new ArrayList();
            CertificadosRecProcesados certificado = new CertificadosRecProcesados();
            Recordset recSet = null;

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "select T1.DocEntry, U_Aprobado, U_TipoCFE, U_SerCom, U_NumCom, U_FecEmi, U_FeFir from [@TFESOBREC] as T1 inner join [@TFECONS] as T2 on T1.U_IdCons = T2.DocEntry where T2.U_IdRec = " + idRespuesta + " and U_Token = '" + token + "' and U_Aprobado in ('Y', 'N')";
                recSet.DoQuery(consulta);
                int i = 0;

                while (i < recSet.RecordCount)
                {
                    certificado = new CertificadosRecProcesados();
                    certificado.DocEntry = recSet.Fields.Item("DocEntry").Value + "";                    
                    certificado.TipoCom = recSet.Fields.Item("U_TipoCFE").Value + "";
                    certificado.SerieCom = recSet.Fields.Item("U_SerCom").Value + "";
                    certificado.NumCom = recSet.Fields.Item("U_NumCom").Value + "";
                    certificado.FechaEmision = recSet.Fields.Item("U_FecEmi").Value + "";
                    certificado.FechaFirma = recSet.Fields.Item("U_FeFir").Value + "";

                    if (recSet.Fields.Item("U_Aprobado").Value == "Y")
                    {
                        certificado.Aprobado = true;
                    }
                    else
                    {
                        certificado.Aprobado = false;
                    }

                    listaComporobantes.Add(certificado);
                    recSet.MoveNext();
                    i++;
                }                
            }
            catch (Exception)
            {
                listaComporobantes = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera de memoria el objeto recSet
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return listaComporobantes;
        }

        /// <summary>
        /// Retorna los datos de caratula de un sobre recibido
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="DocEntry"></param>
        /// <returns></returns>
        public CertificadoRecibido ConsultarDatosCertificadoRecibido(string idRespuesta)
        {
            Recordset recSet = null;
            CertificadoRecibido certificadoRecibido = null; 

            string consulta = "";

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                consulta = "select U_RucRec, U_RucEmi, U_IdCons, U_IdEmisor from [@TFESOBREC] where U_IdCons = " + idRespuesta;
                recSet.DoQuery(consulta);                

                if(recSet.RecordCount > 0)
                {
                    certificadoRecibido = new CertificadoRecibido();
                    certificadoRecibido.RucReceptor = recSet.Fields.Item("U_RucRec").Value + "";
                    certificadoRecibido.RucEmisor = recSet.Fields.Item("U_RucEmi").Value + "";
                    certificadoRecibido.IdConsecutio = recSet.Fields.Item("U_IdCons").Value + "";
                    certificadoRecibido.IdEmisor = recSet.Fields.Item("U_IdEmisor").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera de memoria el objeto recSet
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return certificadoRecibido;
        }

        /// <summary>
        /// Retorna la cantidad de ceritificados recibidos en un sobre
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idRespuesta"></param>
        /// <returns></returns>
        public int ConsultarCantidadCertificadosSobre(string idRespuesta)
        {
            string consulta = "";
            int cantidad = 0;

            //Obtener objeto estadar de record set
            Recordset recSet = null;

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "select count(DocEntry) as 'Cantidad' from [@TFESOBREC] where U_IdCons = " + idRespuesta;

                //Ejecutar consulta 
                recSet.DoQuery(consulta);
                //Validar que se hayan obtenido resultado
                recSet.MoveFirst();

                if (recSet.RecordCount > 0)
                {
                    cantidad = int.Parse(recSet.Fields.Item("Cantidad").Value + "");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera de memoria el objeto recSet
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return cantidad;
        }

        /// <summary>
        /// Obtiene los datos de un certificado recibido
        /// </summary>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public List<CertificadoRecibido> ObtenerCertificadoRecibido(string docEntry)
        {
            List<CertificadoRecibido> listaCertificadoRecibido = new List<CertificadoRecibido>();
            CertificadoRecibido certificadoRecibido = null;
            string consulta = "";

            //Obtener objeto estadar de record set
            Recordset recSet = null;

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                consulta = "SELECT U_RucRec, U_RucEmi, U_IdCons, U_IdEmisor, U_NomSob, U_CorEmi, U_FeSob, U_FeFir, U_CanCfe FROM [@TFESOBREC] "+
                " WHERE DocEntry = '" + docEntry + "'";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                recSet.MoveFirst();

                if (recSet.RecordCount > 0)
                {
                    certificadoRecibido = new CertificadoRecibido();

                    certificadoRecibido.RucReceptor = recSet.Fields.Item("U_RucRec").Value + "";
                    certificadoRecibido.RucEmisor = recSet.Fields.Item("U_RucEmi").Value + "";
                    certificadoRecibido.IdConsecutio = recSet.Fields.Item("U_IdCons").Value + "";
                    certificadoRecibido.IdEmisor = recSet.Fields.Item("U_IdEmisor").Value + "";
                    certificadoRecibido.NombreSobre = recSet.Fields.Item("U_NomSob").Value + "";
                    certificadoRecibido.CorreoEmisor = recSet.Fields.Item("U_CorEmi").Value + "";
                    certificadoRecibido.FechaSobre = recSet.Fields.Item("U_FeSob").Value + "";
                    certificadoRecibido.FechaEmision = recSet.Fields.Item("U_FeFir").Value + "";
                    certificadoRecibido.CantCFE = recSet.Fields.Item("U_CanCfe").Value + "";
                }

                listaCertificadoRecibido.Add(certificadoRecibido);
                //Se libera de memoria el objeto recSet
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera de memoria el objeto recSet
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return listaCertificadoRecibido;
        }

        /// <summary>
        /// Obtiene la lista de errore de un sobre recibido
        /// </summary>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public List<ErrorValidarSobre> ObtenerErroresSobre(string docEntry)
        {
            List<ErrorValidarSobre> listaErrores = new List<ErrorValidarSobre>();
            ErrorValidarSobre error = null;
            string consulta = "";

            //Obtener objeto estadar de record set
            Recordset recSet = null;

            try
            {
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                consulta = "SELECT U_Motivo, U_Glosa, U_Detalle FROM [@TFEESTCFER] WHERE U_ConsRec = '" + docEntry + "'";

                //Ejecutar consulta 
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido resultado
                recSet.MoveFirst();

                if (recSet.RecordCount > 0)
                {
                    int i = 0;

                    while (i < recSet.RecordCount)
                    {
                        error = new ErrorValidarSobre();

                        error.CodigoRechazo = recSet.Fields.Item("U_Motivo").Value + "";
                        error.DetalleRechazo = recSet.Fields.Item("U_Detalle").Value + "";
                        error.GlosaRechazo = recSet.Fields.Item("U_Glosa").Value + "";
                        listaErrores.Add(error);
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
                    //Se libera de memoria el objeto recSet
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    GC.Collect();
                }
            }

            return listaErrores;
        }

        /// <summary>
        /// Actualiza el idConsecutivo del sobre recibido
        /// </summary>
        /// <param name="idconsecutivo"></param>
        /// <returns></returns>
        public bool ActualizarIdConsecutivo(string idconsecutivo, string docEntry)
        {
            bool salida = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = (ProcConexion.Comp.GetCompanyService()).GetGeneralService("TTFETDCON");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                //Establecer los valores para las propiedades
                dataGeneral.SetProperty("U_IdCons", idconsecutivo);                

                //Agregar el nuevo registro a la base de datos mediante el serivicio general
                servicioGeneral.Update(dataGeneral);

                salida = true;
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
            return salida;
        }
    }
}
