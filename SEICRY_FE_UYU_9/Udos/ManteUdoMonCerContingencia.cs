using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Udos
{
    /// <summary>
    /// Contiene los metodos para la adminstracion del udo de Monitor de Certificados en Contingencia
    /// </summary>
    class ManteUdoMonCerContingencia
    {
        #region FUNCIONES

        /// <summary>
        /// Ingresa un nuevo registro a la tabla @TFECERCON
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="idCFE"></param>
        /// <param name="idSobre"></param>
        /// <param name="tipo"></param>
        /// <param name="idReceptor"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public bool Almacenar(CFE cfe)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralData dataGeneral = null;

            try
            {
                //Obtener el servicio de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECERCON");

                //Apuntar a la cabecera del UDO
                dataGeneral = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                //Asiganar valor a cada una de la propiedades del udo
                dataGeneral.SetProperty("U_DocSap", cfe.DocumentoSAP);
                dataGeneral.SetProperty("U_TipoDoc", cfe.TipoCFEInt.ToString());
                dataGeneral.SetProperty("U_NumCFE", cfe.NumeroComprobante.ToString());
                dataGeneral.SetProperty("U_Serie", cfe.SerieComprobante);
                dataGeneral.SetProperty("U_EmaRec", cfe.CorreoReceptor);

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
        /// Copiar el Cfe de contingencia a la tabla de CFEs
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public bool CopiarCFE(string docEntry)
        {
            bool resultado = false;
            ManteUdoCFE manteUdoCfe = new ManteUdoCFE();
            Recordset registro = null;
            string consulta = "SELECT U_DocSap, U_TipoDoc, U_Serie, U_NumCFE FROM [@TFECERCON] WHERE DocEntry = '"+ docEntry +"'";
            CFE cfe = new CFE();

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    cfe.DocumentoSAP = registro.Fields.Item("U_DocSap").Value + "";
                    cfe.TipoCFEInt = Convert.ToInt16(registro.Fields.Item("U_TipoDoc").Value + "");
                    cfe.SerieComprobante = registro.Fields.Item("U_Serie").Value + "";
                    cfe.NumeroComprobante = int.Parse(registro.Fields.Item("U_NumCFE").Value + "");
                    cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;
                    cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                    //if (manteUdoCfe.Almacenar(cfe))
                    //{
                    //    resultado = true;
                    //}                    
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto 
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Elimina el sobre en transito
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public bool Eliminar(string docEntry)
        {
            bool resultado = false;

            GeneralService servicioGeneral = null;
            GeneralDataParams parametros = null;

            try
            {
                //Obtener servicio general de la compañia
                servicioGeneral = ProcConexion.Comp.GetCompanyService().GetGeneralService("TTFECERCON");

                //Obtener lista de parametros
                parametros = servicioGeneral.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                //Establecer parametros
                parametros.SetProperty("DocEntry", docEntry);

                //Agregar el nuevo registro a la base de datos mediante el servicio general
                servicioGeneral.Delete(parametros);

                resultado = true;
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
        /// Consulta el DocEntry filtrando por tipo, serie y numero
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipo"></param>
        /// <param name="serie"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public string ObtenerDocEntry(string tipo, string serie, string numero)
        {
            Recordset recSet = null;
            string consulta = "", resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select DocEntry from [@TFECERCON] where U_TipoDoc = '" + tipo + "' and U_Serie ='" + serie + "' and U_NumCFE = '" + numero + "'";

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
        /// Obtener correo receptor basado en tipoDocumento, serie y numeroDocumento
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipo"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public string ObtenerCorreoReceptor(string tipoDocumento, string serie, string numeroDocumento)
        {
            Recordset recSet = null;
            string consulta = "", resultado = "";

            try
            {
                //Obtener objeto de recordset 
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select U_EmaRec from [@TFECERCON] where U_TipoDoc = '" + tipoDocumento + "' and U_Serie ='" + serie + "' and U_NumCFE = '" + numeroDocumento + "'";

                //Ejectura consulta
                recSet.DoQuery(consulta);

                //Posicionar cursor al inicio
                recSet.MoveFirst();

                //Validar que existan valores
                if (recSet.RecordCount > 0)
                {
                    resultado = recSet.Fields.Item("U_EmaRec").Value + "";
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

