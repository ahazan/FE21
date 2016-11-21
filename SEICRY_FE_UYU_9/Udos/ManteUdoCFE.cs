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
    /// Contiene los metodos para la adminstracion del udo de CFE's. 
    /// </summary>
    class ManteUdoCFE 
    {
        #region FUNCIONES

        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFECFE.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idCFE"></param>
        /// <param name="idSobre"></param>
        /// <param name="tipo"></param>
        /// <param name="idReceptor"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public bool Almacenar(CFE cfe, string MayorUI)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECFE");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Asiganar valor a cada una de la propiedades del udo
                dataGeneral.SetProperty("U_DocSap", cfe.DocumentoSAP);
                dataGeneral.SetProperty("U_TipoDoc", cfe.TipoCFEInt.ToString());
                dataGeneral.SetProperty("U_NumCFE", cfe.NumeroComprobante.ToString());
                dataGeneral.SetProperty("U_Serie", cfe.SerieComprobante);
                dataGeneral.SetProperty("U_EstadoDgi", cfe.EstadoDGI.ToString());
                dataGeneral.SetProperty("U_EstadoRec", cfe.EstadoReceptor.ToString());
                dataGeneral.SetProperty("U_Sucursal", cfe.CodigoCasaPrincipalEmisor.ToString());
                dataGeneral.SetProperty("U_FechaFirma", cfe.FechaFirma.ToString());
                dataGeneral.SetProperty("U_MayorUI", MayorUI);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Add(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
               // SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ManteUdoCFE/Error: " + ex.ToString());
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
        /// Actualiza los datos del CFE
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipo"></param>
        /// <param name="serie"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool Actualizar(int tipo, string serie, int numero, CFE.ESEstadoCFE estado, CFE.ESTipoReceptor tipoReceptor)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECFE");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", ConsultarDocEntryCFE(tipo, serie, numero ));

                //Apuntar al udo que corresponde con los parametros
                dataGeneral = servicioGeneral.GetByParams(parametros);

                if (tipoReceptor == CFE.ESTipoReceptor.DGI)
                {
                    dataGeneral.SetProperty("U_EstadoDgi", estado.ToString());
                }
                else
                {
                    dataGeneral.SetProperty("U_EstadoRec", estado.ToString());
                }

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Update(dataGeneral);

                resultado = true;
            }
            catch (Exception ex)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("actualizarCFE " + ex.ToString());
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
            return resultado;
        }

        /// <summary>
        /// Consulta el DocEntry del CFE filtrando por tipo, serie y numero
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipo"></param>
        /// <param name="serie"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        private string ConsultarDocEntryCFE(int tipo, string serie, int numero)
        {
            Recordset recSet = null;
            string consulta = "";
            string resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry from [@TFECFE] where U_TipoDoc = '" + tipo + "' and U_Serie ='" + serie + "' and U_NumCFE = '" + numero + "'";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Consulta el DocEntry del CFE filtrado por idEmisor
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipo"></param>
        /// <param name="serie"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        private string ConsultarDocEntryCFE(string idEmisor)
        {
            Recordset recSet = null;
            string consulta = "";
            string resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry from [@TFESOB] where U_IdEmi = '" + idEmisor + "'" ;

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("DocEntry").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Valida la existencia de un tipo y numero de CFE
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipo"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool ConsultarTipoNumero(int tipo, int numero)
        {
            Recordset recSet = null;
            string consulta = "";
            bool resultado = false;

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry from [@TFECFE] where U_TipoDoc = '" + tipo + "' and U_NumCFE = '" + numero + "'";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Consulta el numero de documento SAP
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="serie"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public string ConsultarDocumentoSap(string tipoDocumento, string serie, string numeroDocumento)
        {
            Recordset recSet = null;
            string consulta = "", resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet =ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_DocSap FROM [@TFECERCON] WHERE U_TipoDoc = '" + tipoDocumento + "' AND U_NumCFE = '" + numeroDocumento + "' AND U_Serie = '" + serie +"'";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("U_DocSap").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Se libera el objeto de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        #endregion FUNCIONES

        #region ENUMERACIONES

        public enum ESTipoCFECFC
        {
            /// <summary>
            /// E-Ticket. CFE
            /// </summary>
            ETicket = 101,

            /// <summary>
            /// Nota de credito E-Ticket. CFE
            /// </summary>
            NCETicket = 102,

            /// <summary>
            /// Noda de debito E-Ticket. CFE
            /// </summary>
            NDETicket = 103,

            /// <summary>
            /// E-Factura. CFE
            /// </summary>
            EFactura = 111,

            /// <summary>
            /// Nota de credito E-Factura. CFE
            /// </summary>
            NCEFactura = 112,

            /// <summary>
            /// Nota de debito E-Factura. CFE
            /// </summary>
            NDEFactura = 113,

            /// <summary>
            /// E-Remito. CFE
            /// </summary>
            ERemito = 181,

            /// <summary>
            /// E-Resguardo. CFE
            /// </summary>
            EResguardo = 182,

            /// <summary>
            /// E-Tiket Contingencia. CFC
            /// </summary>
            ETiketContingencia = 201,

            /// <summary>
            /// /// Nota de credito E-Ticket Contingencia. CFC 
            /// </summary>
            NCETicketContingencia = 202,

            /// <summary>
            /// Noda de debito E-Ticket Contingencia. CFC
            /// </summary>
            NDETicketContingencia = 203,

            /// <summary>
            /// E-Factura Contingencia. CFC
            /// </summary>
            EFacturaContingencia = 211,

            /// <summary>
            /// Nota de credito E-Factura Contingencia. CFC
            /// </summary>
            NCEFacturaContingencia = 212,

            /// <summary>
            /// Nota de debito E-Factura Contingencia. CFC
            /// </summary>
            NDEFacturaContingencia = 213,

            /// <summary>
            /// E-Remito Contingencia. CFC
            /// </summary>
            ERemitoContingencia = 281,

            /// <summary>
            /// E-Resguardo Contingencia. CFC
            /// </summary>
            EResguardoContingencia = 282,

            /// <summary>
            /// E-Factura Exportacion. CFC
            /// </summary>
            EFacturaExportacion = 121,

            /// <summary>
            /// Nota Credito E-Factura Exportacion. CFC
            /// </summary>
            NCEFacturaExportacion = 122,

            /// <summary>
            /// Nota de debito E-Factura Exportacion. CFC
            /// </summary>
            NDFacturaExportacion = 123,

            /// <summary>
            /// E-Remito Exportacion
            /// </summary>
            ERemitoExportacion = 124,

            /// <summary>
            /// E-Factura Exportacion Contingencia
            /// </summary>
            EFacturaExportacionContingencia = 221,

            /// <summary>
            /// NCEFacturaExportacionContingencia
            /// </summary>
            NCEFacturaExportacionContingencia = 222,

            /// <summary>
            /// NDEFacturaExportacionContingencia
            /// </summary>
            NDEFacturaExportacionContingencia = 223,

            /// <summary>
            /// ERemitoExportancionContingencia
            /// </summary>
            ERemitoExportacionContingencia = 224
        }

    }
        #endregion ENUMERACIONES
}

