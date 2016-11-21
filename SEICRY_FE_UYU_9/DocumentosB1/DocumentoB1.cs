using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;
using SAPbobsCOM;
using System.Xml;

namespace SEICRY_FE_UYU_9.DocumentosB1
{
    public abstract class DocumentoB1
    {
        //Variables estaticas
        public const string TABLA_FACTURA = "OINV";
        public const string TABLA_FACTURA_COMPRA = "OPCH";
        public const string TABLA_NOTA_DEBITO = "OINV";
        public const string TABLA_NOTA_CREDITO = "ORIN";
        public const string TABLA_REMITO = "ODLN";
        public const string TABLA_RESGUARDO = "OINV";
        public const string TABLA_RESGUARDO_COMPRA = "OPCH";
        public const string TABLA_DETALLES_NOTA_CREDITO = "RIN1";
        public const string TABLA_DETALLES_NOTA_DEBITO = "INV1";
        public const string TABLA_DETALLES_REMITO = "DLN1";
        public const string TABLA_RESGUARDO_PROVEEDOR = "ORPC";

        //Variables visibles en toda la clase
        ManteDocumentos manteDocumentos = new ManteDocumentos();

        /// <summary>
        /// Retona el DocEntry del documento creado
        /// </summary>
        /// <param name="xmlResultado"></param>
        /// <returns></returns>
        public static int ObtenerNumeroDocumentoCreado(string xmlResultado)
        {
            int numFacturaCreado = 0;

            //Crea el documento xml
            XmlDocument xmlDocumento = new XmlDocument();

            //Carga el xml que contiene el resultado
            xmlDocumento.LoadXml(xmlResultado);

            //Obtiene el valor del DocEntry
            numFacturaCreado = int.Parse(xmlDocumento.GetElementsByTagName("DocEntry").Item(0).InnerText);

            return numFacturaCreado;
        }

        /// <summary>
        /// Valida que un documento determinado sea factura o nota de Debito. Retorna true si es factura
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public static bool ValidarDocumentoFactura(int numeroDocumento, string tabla)
        {
            ManteDocumentos manteDocumentos = new ManteDocumentos();
            return manteDocumentos.ValidarDocumentoFactura(numeroDocumento, tabla);
        }

        /// <summary>
        /// Retona un objeto Emisor con los datos almacenados en Detalles de la Sociedad
        /// </summary>
        /// <returns></returns>
        protected virtual Object ObtenerDatosEmisor()
        {
            //Obtiene y retorna un Objeto emisor
            return manteDocumentos.ConsultarDatosEmisor();
        }

        /// <summary>
        /// Retorna el indicador de facturacion
        /// </summary>
        /// <param name="tipoImpuesto"></param>
        /// <returns></returns>
        protected virtual string ObtenerIndicadorFacturacion(string tipoImpuesto)
        {
            return manteDocumentos.ObtenerIndicadorFacturacion(tipoImpuesto);
        }

        /// <summary>
        /// Retorna el codigo de la sucursal donde se confecciono el documento
        /// </summary>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        protected virtual string ObtenerSucursal(string codigoDocumento, string tablaDocumentos)
        {
            return manteDocumentos.ConsultarSucursalEmisor(tablaDocumentos, codigoDocumento);
        }

        /// <summary>
        /// Retorna el codigo de la caja donde se confecciono el documento
        /// </summary>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        protected virtual string ObtenerCaja(string codigoDocumento, string tablaDocumentos)
        {
            return manteDocumentos.ConsultarCajaEmisor(tablaDocumentos, codigoDocumento);
        }

        /// <summary>
        /// Retorna la direccion del emisor
        /// </summary>
        /// <returns></returns>
        protected virtual string ObtenerDireccionEmisor()
        {
            return manteDocumentos.ConsultarDireccionEmisor();
        }

        /// <summary>
        /// Retorna la ciuda del emisor
        /// </summary>
        /// <returns></returns>
        protected virtual string ObtenerCiudadEmisor()
        {
            return manteDocumentos.ConsultarCiudadEmisor();
        }

        /// <summary>
        /// Retorna el pais del socio de negocios
        /// </summary>
        /// <returns></returns>
        protected virtual string ObtenerCodPaisReceptor(string codigoSocioNegocios, CFE.ESTipoDocumentoReceptor tipoDocumentoReceptor)
        {
            return manteDocumentos.ConsultarCodPaisSocioNegocio(codigoSocioNegocios, tipoDocumentoReceptor);
        }

        /// <summary>
        /// Retorna el nombre del pais del socio de negocios
        /// </summary>
        /// <returns></returns>
        protected virtual string ObtenerNombPaisReceptor(string codigoSocioNegocios)
        {
            return manteDocumentos.ConsultarNombPaisSocioNegocio(codigoSocioNegocios);
        }

        /// <summary>
        /// Retorna la direccion de correo electronico del receptor
        /// </summary>
        /// <param name="codigoSocioNegocios"></param>
        /// <returns></returns>
        protected virtual string ObtenerCorreoReceptor(string codigoSocioNegocios)
        {
            return manteDocumentos.ConsultarCorreoReceptor(codigoSocioNegocios);
        }

        /// <summary>
        /// Obtiene el codigo ISO para una moneda determinada
        /// </summary>
        /// <param name="codigoModena"></param>
        /// <returns></returns>
        protected virtual string ObtenerCodigoISOModena(string codigoModena)
        {
            return manteDocumentos.ConsultarISOMoneda(codigoModena);
        }

        /// <summary>
        /// Retorna la sumatoria de precios de cada linea con un tipo de impuesto determinado
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="tipoImpuesto"></param>
        /// <returns></returns>
        protected virtual double ObtenerTotalesPorImpuesto(Documents documento, string tipoImpuesto)
        {
            double total = 0;
            Document_Lines lineas = null;

            try
            {
                lineas = documento.Lines;

                for (int i = 0; i < lineas.Count; i++)
                {
                    lineas.SetCurrentLine(i);

                    if (lineas.TaxCode.Equals(tipoImpuesto))
                    {
                        if (manteDocumentos.ConsultarSujetoImpuesto(lineas.ItemCode))
                        {
                            total += (lineas.Price * lineas.Quantity) - ((lineas.Price * lineas.Quantity) * (documento.DiscountPercent * 0.01));                           

                        }
                        else
                        {
                            total += 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("DocumentoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (lineas != null)
                {
                    //Libera de memoria el objete lineas
                    GC.SuppressFinalize(lineas);
                    GC.Collect();
                }
            }

            return total;
        }



        protected virtual double ObtenerTotalesFCPorImpuesto(Documents documento, string tipoImpuesto)
        {
            double total = 0;
            Document_Lines lineas = null;

            try
            {
                lineas = documento.Lines;

                for (int i = 0; i < lineas.Count; i++)
                {
                    lineas.SetCurrentLine(i);

                    if (lineas.TaxCode.Equals(tipoImpuesto))
                    {
                        if (manteDocumentos.ConsultarSujetoImpuesto(lineas.ItemCode))
                        {
                            //total += (lineas.RowTotalFC * lineas.Quantity) - ((lineas.Price * lineas.Quantity) * (documento.DiscountPercent * 0.01));


                            total += (lineas.RowTotalFC) - ((lineas.Price * lineas.Quantity) * (documento.DiscountPercent * 0.01));

                             
                        }
                        else
                        {
                            total += 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("DocumentoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (lineas != null)
                {
                    //Libera de memoria el objete lineas
                    GC.SuppressFinalize(lineas);
                    GC.Collect();
                }
            }

            return total;
        }

        #region FE_EXPORTACION
        /// <summary>
        /// Retorna la sumatoria de precios de cada linea sin importar el tipo de impuesto. Esto para exportacion
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="tipoImpuesto"></param>
        /// <returns></returns>
        protected virtual double ObtenerTotalesExportacion(Documents documento)
        {
            double total = 0;
            Document_Lines lineas = null;

            try
            {
                lineas = documento.Lines;

                for (int i = 0; i < lineas.Count; i++)
                {
                    lineas.SetCurrentLine(i);

                    total += (lineas.Price * lineas.Quantity) - ((lineas.Price * lineas.Quantity) * (documento.DiscountPercent * 0.01));
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("DocumentoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (lineas != null)
                {
                    //Libera de memoria el objete lineas
                    GC.SuppressFinalize(lineas);
                    GC.Collect();
                }
            }
            return total;
        }
        //protected virtual double ObtenerTotalesExportacion(Documents documento)
        //{
        //    double total = 0;
        //    Document_Lines lineas = null;

        //    try
        //    {
        //        lineas = documento.Lines;

        //        for (int i = 0; i < lineas.Count; i++)
        //        {
        //            lineas.SetCurrentLine(i);

        //            total += lineas.Price;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("DocumentoB1/Error: " + ex.ToString());
        //    }
        //    finally
        //    {
        //        if (lineas != null)
        //        {
        //            //Libera de memoria el objete lineas
        //            GC.SuppressFinalize(lineas);
        //            GC.Collect();
        //        }
        //    }

        //    return total;
        //}
        #endregion FE_EXPORTACION

        /// <summary>
        /// Retorna el monto asignado a una tasa determinada
        /// </summary>
        /// <param name="tipoTasa"></param>
        /// <returns></returns>
        protected virtual double ObtenerMontoTasa(string tipoTasa)
        {
            return manteDocumentos.ConsultarMontoTasa(tipoTasa);
        }

        /// <summary>
        /// Retorna la sumatoria total de las lineas con un tipo de impuesto definido
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="tipoImpuesto"></param>
        /// <returns></returns>
        protected virtual double ObtenerTotalesIVAPorTasa(string codigoDocumento, string tipoImpuesto, string tabla)
        {
            return manteDocumentos.ConsultaTotalTasa(codigoDocumento, tipoImpuesto, tabla);
        }

        /// <summary>
        /// Retorna el numero de CFE y la serie para el documento procesado
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerDatosRangoCFE(string tipoDocumento, string sucursal, string caja)
        {
            return manteDocumentos.ConsultarDatosRangoCFE(tipoDocumento, sucursal, caja);
        }

        /// <summary>
        /// Retorna la unidad de medida de un articulo determinado
        /// </summary>
        /// <param name="codigoItem"></param>
        /// <returns></returns>
        protected virtual string ObtenerUnidadMedidaItem(string codigoItem, string docEntry, string lineNum,string tabla)
        {
            return manteDocumentos.ConsultaUnidadMedidaItem(codigoItem, docEntry, lineNum, tabla);
        }

        /// <summary>
        /// Rertorna el medio de pago utiliazado en un documento determinado
        /// </summary>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        protected virtual string ObtenerMedioPago(string codigoDocumento, string tablaDocumentos)
        {
            return manteDocumentos.ConsultarMedioPago(tablaDocumentos, codigoDocumento);
        }

        /// <summary>
        /// Retorna los datos del CFE (serie y numero de CFE) al que se le aplicó una nota de correcion
        /// </summary>
        /// <param name="codigoDocumento"></param>
        /// <param name="tablaDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerDocumentoReferencia(string tablaDetalleDocumentos, string codigoDocumento)
        {
            return manteDocumentos.ConsultarDocumentoReferencia(tablaDetalleDocumentos, codigoDocumento);
        }

        /// <summary>
        /// Retorna los datos del CFE (serie y numero de CFE) al que se le aplicó una nota de correcion
        /// </summary>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerDocumentoReferencia(CFE.ESTipoCFECFC tipoDocumento, string codigoDocumento)
        {
            return manteDocumentos.ConsultarDocumentoReferencia(tipoDocumento, codigoDocumento);
        }

        /// <summary>
        /// Valida que un cliente tenga el tipo de pago de contado establecido
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        protected virtual bool ValidarClienteContado(string codigoCliente)
        {
            return manteDocumentos.ValidarClienteContado(codigoCliente);
        }

        /// <summary>
        /// Retorna un objeto CAE con los datos para el CFE
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerDatosCAE(string tipoDocumento)
        {
            return manteDocumentos.ConsultarDatosCAE(tipoDocumento);
        }

        /// <summary>
        /// Valida que el cliente se extranjero
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        protected virtual bool ValidarClienteExtranjero(string codigoCliente)
        {
            return manteDocumentos.ValidarClienteExtranjero(codigoCliente);
        }

        /// <summary>
        /// Consulta el indicador de facturacion de cada articulo 
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        protected virtual int ValidarIndicadorFacturacionArticulo(string codigoArticulo)
        {
            return manteDocumentos.ConsultarIndicadorFacturacionArticulo(codigoArticulo);
        }

        /// <summary>
        ///  Consulta el valor del iva tasa basica
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        protected virtual int ObtenerIvaTasaBasica()
        {
            return manteDocumentos.ConsultarIvaTasaBasica();
        }

        /// <summary>
        ///  Consulta el valor del iva tasa minima
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        protected virtual int ObtenerIvaTasaMinima()
        {
            return manteDocumentos.ConsultarIvaTasaMinima();
        }

        /// <summary>
        /// Consulta el codigo de impuesto para cada tipo 
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        protected virtual string ObtenerCodigoImpuesto(string tipoImpuesto)
        {
            return manteDocumentos.ConsultarCodigoImpuesto(tipoImpuesto);
        }

        /// <summary>
        /// Retorn un objeto CFERetencPercep codigo y valor y el monto de la retencion
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerRetencionPercepcion(string numeroDocumento, string tabla, string moneda)
        {
            return manteDocumentos.ConsultarRetencionPerecepcion(numeroDocumento, tabla, moneda);
        }

        /// <summary>
        /// Obtiene la lista de retenciones y percepciones del documento
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerItemRetencionPercepcion(string numeroDocumento, string tabla, string moneda)
        {
            return manteDocumentos.ConsultarItemRetencionPercepcion(numeroDocumento, tabla, moneda);
        }

        /// <summary>
        /// Retorna la informacion de referncia de un resguardo
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        protected virtual Object ObtenerReferenciaResguardo(string numeroDocumento, string tabla)
        {
            return manteDocumentos.ConsultarReferenciaResguardo(numeroDocumento, tabla);
        }

        protected virtual double ObtenerDocRate()
        {
            return manteDocumentos.ObtenerDocRate();
        }

        /// <summary>
        /// Obtiene el total de kilos por factura
        /// </summary>
        /// <param name="numDocEntry"></param>
        /// <returns></returns>
        public static DatosPDF ObtenerkilosFactura(int numDocEntry, string tabla, DatosPDF datosPdf)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT SUM(Weight1) AS 'Suma', MAX(BaseRef) AS 'BaseRef' FROM [" + tabla + "] WHERE DocEntry = '" + numDocEntry + "'";

                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    datosPdf.KilosFactura = registro.Fields.Item("Suma").Value + "";
                    datosPdf.DocumentoBase = registro.Fields.Item("BaseRef").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    GC.SuppressFinalize(registro);
                    GC.Collect();
                }
            }

            return datosPdf;
        }

        /// <summary>
        /// Actualiza el numero de pedido
        /// </summary>
        /// <param name="datosPdf"></param>
        /// <returns></returns>
        public static DatosPDF ActualizarNumPedido(DatosPDF datosPdf)
        {
            string consulta = "SELECT Ref2 FROM ORDR WHERE DocEntry = '" + datosPdf.DocumentoBase + "'", temp = "";
            Recordset registro = null;

            try
            {
                if (!datosPdf.DocumentoBase.Equals(""))
                {
                    registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                    registro.DoQuery(consulta);
                    if (registro.RecordCount > 0)
                    {
                        temp = registro.Fields.Item("Ref2").Value + "";
                        if (!temp.Equals(""))
                        {
                            datosPdf.DocumentoBase = temp;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return datosPdf;
        }


        /// <summary>
        /// Obtiene el docNum del documento
        /// </summary>
        /// <param name="numDocEntry"></param>
        /// <returns></returns>
        public static DatosPDF ObtenerDatosPDF(int numDocEntry, string tabla, DatosPDF datosPdf)
        {
            string consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                /*consulta = "SELECT DocNum, VatSum, DocTotal,DocTotalFC,  REPLACE(REPLACE(T1.Address2, CHAR(13), ''), CHAR(10), '') AS Address2, T2.CardCode, T2.E_Mail, T2.Notes, T2.Phone1, " +
                "T2.Phone2, T2.ShipType, T2.CardFName, T1.RoundDif , T1.NumAtCard, OCTG.PymntGroup FROM [" + tabla + "] AS T1 " +
                           "INNER JOIN OCRD AS T2 ON T2.CardCode = T1.CardCode " +
                           "INNER JOIN OCTG ON OCTG.GroupNum = T2.GroupNum " +
                           "WHERE T1.DocEntry = '" + numDocEntry + "'";*/

                consulta = "SELECT DocNum, VatSum, DocTotal,DocTotalFC,  REPLACE(REPLACE(T1.Address2, CHAR(13), ''), CHAR(10), '') AS Address2, T2.CardCode, T2.E_Mail, T2.Notes, T2.Phone1, " +
                "T2.Phone2, T2.ShipType, T2.CardFName, ( CASE WHEN T1.DocCur = 'UYU' THEN T1.RoundDif ELSE T1.RoundDifFC END ) AS RoundDif, T1.NumAtCard, OCTG.PymntGroup FROM [" + tabla + "] AS T1 " +
                           "INNER JOIN OCRD AS T2 ON T2.CardCode = T1.CardCode " +
                           "INNER JOIN OCTG ON OCTG.GroupNum = T2.GroupNum " +
                           "WHERE T1.DocEntry = '" + numDocEntry + "'";

                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    datosPdf.DocNum = registro.Fields.Item("DocNum").Value + "";
                    datosPdf.ImpuestoCalculado = registro.Fields.Item("VatSum").Value + "";
                    datosPdf.MontoTotalPagar = registro.Fields.Item("DocTotal").Value + "";
                    datosPdf.MontoTotalPagarPesos = registro.Fields.Item("DocTotalFC").Value + "";
                    datosPdf.SocioNegocio = registro.Fields.Item("CardCode").Value + "";
                    datosPdf.Telefono = registro.Fields.Item("Phone1").Value + "";
                    datosPdf.Telefono2 = registro.Fields.Item("Phone2").Value + "";
                    datosPdf.NumeroOrden = registro.Fields.Item("NumAtCard").Value + "";
                    datosPdf.FormaPago = registro.Fields.Item("PymntGroup").Value + "";
                    datosPdf.Comentarios = registro.Fields.Item("Notes").Value + "";
                    datosPdf.CodigoDireccion = registro.Fields.Item("ShipType").Value + "";
                    datosPdf.Redondeo = registro.Fields.Item("RoundDif").Value + "";

                    datosPdf.NombreExtranjero = registro.Fields.Item("CardFName").Value + "";

                    //datosPdf.DireccionEntrega = registro.Fields.Item("Address2").Value + "";
                    datosPdf.DireccionEntrega = ObtenerDireccionEntrega(numDocEntry, tabla);

                    datosPdf.Email            = registro.Fields.Item("E_Mail").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return datosPdf;
        }

        /// <summary>
        /// Obtener nombre de vendedor
        /// </summary>
        /// <param name="numDocEntry"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public static string ObtenerNombreVendedor(int numDocEntry, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT OSLP.SlpName FROM [" + tabla + "]  INNER JOIN OSLP ON OSLP.SlpCode = [" + tabla + "].SlpCode WHERE DocEntry = '" + numDocEntry + "'";

                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("SlpName").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el titular
        /// </summary>
        /// <param name="numDocEntry"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public static string Titular(int numDocEntry, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT lastName FROM OHEM INNER JOIN "+ tabla +" AS T1 ON T1.OwnerCode = OHEM.empID AND t1.DocEntry = '" + numDocEntry +"'"; 

                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("lastName").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el nombre extranjero
        /// </summary>
        /// <param name="numDocEntry"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public static string ObtenerNombreExtranjero(int numDocEntry, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT OCRD.CardFName FROM OCRD INNER JOIN [" + tabla + "] ON OCRD.CardCode = " + tabla + ".CardCode  WHERE " + tabla + ".DocEntry = '" + numDocEntry + "'";

                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("CardFName").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene datos de direccion
        /// </summary>
        /// <param name="datosPdf"></param>
        /// <returns></returns>
        public static DatosPDF ObtenerDatosDireccion(DatosPDF datosPdf)
        {
            string consulta = "SELECT CRD1.State, CRD1.City FROM OCRD INNER JOIN CRD1 ON OCRD.CardCode = CRD1.CardCode WHERE OCRD.CardCode = '" + datosPdf.SocioNegocio +"' AND AdresType = 'B'";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    datosPdf.Estado = registro.Fields.Item("State").Value + "";
                    datosPdf.Ciudad = registro.Fields.Item("City").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return datosPdf;
        }

        /// <summary>
        /// Obtiene datos de direccion
        /// </summary>
        /// <param name="datosPdf"></param>
        /// <returns></returns>
        public static DatosPDF ActualizarEstado(DatosPDF datosPdf)
        {
            string consulta = "SELECT Name FROM OCST WHERE Code = '" + datosPdf.Estado + "' AND Country = 'UY'";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    datosPdf.Estado = registro.Fields.Item("Name").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return datosPdf;
        }

        /// <summary>
        /// Obtiene datos de direccion
        /// </summary>
        /// <param name="datosPdf"></param>
        /// <returns></returns>
        public static DatosPDF ActualizarCodigo(DatosPDF datosPdf)
        {
            string consulta = "SELECT WebSite FROM OSHP WHERE TrnspCode = '" + datosPdf.CodigoDireccion + "'";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    datosPdf.CodigoDireccion = registro.Fields.Item("WebSite").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return datosPdf;
        }

        /// <summary>
        /// Obtiene el nombre del empleado
        /// </summary>
        /// <param name="numEmpl"></param>
        /// <returns></returns>
        public static string ObtenerNombreEmpleado(string numEmpl)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT lastName FROM OHEM WHERE empID = '" + numEmpl + "'";

                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("lastName").Value + "";
                }
            }
            catch (Exception)
            {
                resultado = "0";
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene los datos de la retencion 
        /// </summary>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        public static List<ResguardoPdf> ObtenerResguardoPdf(int docEntry, string tabla, string tabla1, string tabla5, Boolean NCProveedor = false)
        {
            List<ResguardoPdf> listaResguardo = new List<ResguardoPdf>();
           
            ResguardoPdf Resguardo= null;
            Recordset registro = null;
            string consulta = "";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                
                /*consulta = "SELECT SUM(T1.LineTotal) AS 'MontoImponible', OWHT.PrctBsAmnt AS 'PorRet', T5.WTAmnt AS 'Importe', " +
                           "OWHT.WTName AS 'Impuesto', CASE WHEN TMain.NumAtCard IS NULL THEN  '' ELSE TMain.NumAtCard END AS 'NumFac' FROM "+ tabla5 + " AS T5 " +
	                       "INNER JOIN OWHT ON T5.WTCode = OWHT.WTCode INNER JOIN "+tabla+" AS TMain ON T5.AbsEntry = TMain.DocEntry " +
	                       "INNER JOIN "+ tabla1 +" AS T1 ON T1.DocEntry = TMain.DocEntry WHERE AbsEntry = '" + docEntry + "' " +
	                       "GROUP BY OWHT.PrctBsAmnt, T5.WTAmnt, OWHT.WTName, TMain.NumAtCard";*/

                consulta = "SELECT CASE WHEN T1.Currency = 'UYU' THEN SUM(T1.LineTotal) ELSE SUM(T1.TotalFrgn) END AS 'MontoImponible', OWHT.PrctBsAmnt AS 'PorRet', " +
                            "CASE WHEN T1.Currency = 'UYU' THEN T5.WTAmnt ELSE T5.WTAmntFC END AS 'Importe', " +
                           "OWHT.WTName AS 'Impuesto', CASE WHEN TMain.NumAtCard IS NULL THEN  '' ELSE TMain.NumAtCard END AS 'NumFac' FROM " + tabla5 + " AS T5 " +
                           "INNER JOIN OWHT ON T5.WTCode = OWHT.WTCode INNER JOIN " + tabla + " AS TMain ON T5.AbsEntry = TMain.DocEntry " +
                           "INNER JOIN " + tabla1 + " AS T1 ON T1.DocEntry = TMain.DocEntry WHERE AbsEntry = '" + docEntry + "' " +
                           "GROUP BY OWHT.PrctBsAmnt, T5.WTAmnt, OWHT.WTName, TMain.NumAtCard, T1.Currency, T5.WTAmntFC";

                registro.DoQuery(consulta);

                registro.MoveFirst();

                if (registro.RecordCount > 0)
                {
                                  

                     for (int i = 0; i < registro.RecordCount; i++)
                {

                    Resguardo = new ResguardoPdf(); 


                    Resguardo.NumeroFactura = Convert.ToString(registro.Fields.Item("NumFac").Value);
                    Resguardo.Impuesto = Convert.ToString(registro.Fields.Item("Impuesto").Value);


                    Resguardo.MontoImponible = Convert.ToString(registro.Fields.Item("MontoImponible").Value);
                    Resguardo.ImporteRetencion = Convert.ToString(registro.Fields.Item("Importe").Value);


                    // Si es una NC de Proveedor los Montos debe ser Negativos
                    if (NCProveedor == true)
                    {
                        Resguardo.MontoImponible = Convert.ToString(Convert.ToDouble(Resguardo.MontoImponible) * -1);
                        Resguardo.ImporteRetencion = Convert.ToString(Convert.ToDouble(Resguardo.ImporteRetencion) * -1); 
                    }


                    Resguardo.PorcentajeRetencion = Convert.ToString(registro.Fields.Item("PorRet").Value);

                    listaResguardo.Add(Resguardo);

                    registro.MoveNext();
                }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return listaResguardo;
        }

        /// <summary>
        /// Actualiza el asiento con la serie y cae correspondiente
        /// </summary>
        /// <param name="numeroAsiento"></param>
        /// <param name="serieCAE"></param>
        private static void ActualizarAsiento(int numeroAsiento, string serieCAE)
        {
            JournalEntries asientoComprobantes = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oJournalEntries);
            asientoComprobantes.GetByKey(numeroAsiento);
            asientoComprobantes.Reference3 = serieCAE;
            ManteDocumentos manteDoc = new ManteDocumentos();            
            asientoComprobantes.Update();
            manteDoc.ActualizarLineaRef3(numeroAsiento, serieCAE);
        }

        /// <summary>
        /// Obtiene la forma de pago
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="docNum"></param>
        /// <returns></returns>
        public static string ObtenerFormaPago(string tabla, string docNum)
        {
            Recordset registro = null;
            string consulta = "", campoPago = "", resultado = "";

            try
            {
                if (tabla.Equals("OINV"))
                {
                    campoPago = "U_U_FrmPagOIN";
                }
                else if (tabla.Equals("ORIN"))
                {
                    campoPago = "U_U_FrmPagORI";
                }

                if (!campoPago.Equals(""))
                {
                    registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                    consulta = "SELECT " + campoPago + " FROM "+ tabla +" WHERE docNum = '" + docNum + "'";

                    registro.DoQuery(consulta);
                    {
                        if (registro.RecordCount > 0)
                        {
                            resultado = registro.Fields.Item(campoPago).Value + "";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza datos de CAE en el asiento correspondiente
        /// </summary>               
        /// <param name="docNum"></param>
        /// <param name="serie"></param>
        /// <param name="numeroComprobate"></param>
        /// <param name="tipoObjeto"></param>
        public static void ActualizarCAEAsiento(int docNum, string tipoCfe, string serie, string numeroComprobate,
            string tipoObjeto, string factONotDeb)
        {
            Recordset registro = null;
            string consulta = "";                       

            try
            {
                if (factONotDeb.Equals("F"))
                {
                    consulta = "SELECT number FROM OJDT WHERE baseRef = '" + docNum + "' AND TransType = '"  + tipoObjeto + "'"; // AND DocSeries = 4";
                }
                else if (factONotDeb.Equals("ND"))
                {
                    consulta = "SELECT number FROM OJDT WHERE baseRef = '" + docNum + "' AND TransType = '" +tipoObjeto + "'"; // AND DocSeries = 5";
                }
                else
                {
                    consulta = "SELECT number FROM OJDT WHERE baseRef = '" + docNum + "' AND TransType = '" +tipoObjeto + "'";
                }

                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                registro.DoQuery(consulta);
                {
                    if (registro.RecordCount > 0)
                    {
                        string number = registro.Fields.Item("number").Value + "";
                        ActualizarAsiento(int.Parse(number), tipoCfe + serie + numeroComprobate);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Obtiene la forma de pago de la factura asociada a la nota de credito
        /// </summary>
        /// <param name="DocEntry"></param>
        /// <returns></returns>
        public static string ObtenerFormaPagoFactura(int DocEntry)
        {
            string resultado = "", docEntryFactura = "", consulta = "";
            Recordset registro = null;

            try
            {
                docEntryFactura = ObtenerFacturaReferencia(DocEntry);

                if (!docEntryFactura.Equals(""))
                {
                    registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                    consulta = "SELECT U_U_FrmPagOIN FROM OINV WHERE DocNum = '" + docEntryFactura + "'";
                    registro.DoQuery(consulta);

                    if (registro.RecordCount > 0)
                    {
                        resultado = registro.Fields.Item("U_U_FrmPagOIN").Value + "";
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene la factura de referencia
        /// </summary>
        /// <param name="docEntry"></param>
        /// <returns></returns>
        private static string ObtenerFacturaReferencia(int docEntry)
        {
            string resultado = "", consulta = "SELECT BaseRef FROM ORIN INNER JOIN RIN1 ON RIN1.DocEntry = ORIN.DocEntry " +
             "WHERE ORIN.DocEntry = '" + docEntry + "' GROUP BY BaseRef";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("BaseRef").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene la ciudad receptora del documento
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        protected virtual string ObtenerCiudad(int docEntry, string tabla)
        {
            string resultado = "", consulta = "SELECT StreetB FROM " + tabla + " WHERE DocEntry = '" + docEntry + "'";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);
                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("StreetB").Value + "";
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        protected virtual string ObtenerNroPedidoReceptor(int docEntry, string tabla)
        {
            string resultado = "";
                        
            string consulta = "SELECT ImportEnt FROM " + tabla + " WHERE DocEntry = " + docEntry;
            
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);
                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("ImportEnt").Value + "";
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene la ciudad receptora del documento
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public static string ObtenerDireccionEntrega(int docEntry, string tabla)
        {
            //string resultado = "", consulta = "SELECT Address2 FROM " + tabla + " WHERE DocEntry = '" + docEntry + "'";
            string resultado = "", consulta;

            consulta = "select CONCAT(T0.Street,'  ',T2.Name) AS DirEntrega from CRD1 T0 inner join " + tabla + " T1 on t0.CardCode = t1.CardCode " + 
                "inner join OCST T2 on t0.Country = T2.Country and T0.State = T2.Code " +
                "where T0.AdresType = 'S' and T1.DocEntry = " + docEntry;

            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);
                if (registro.RecordCount > 0)
                {
                    //resultado = registro.Fields.Item("Address2").Value + "";
                    resultado = registro.Fields.Item("DirEntrega").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el email el contacto del cliente
        /// </summary>
        /// <param name="cardCode"></param>        
        /// <returns></returns>
        public static string ObtenerMailCliente(string cardCode)
        {
            //string resultado = "", consulta = "SELECT DISTINCT(E_MailL) 'Email'  FROM OCPR WHERE Cardcode = '" + cardCode + "'";
            string resultado = "", consulta = "SELECT DISTINCT(E_Mail) 'Email' FROM OCRD WHERE Cardcode = '" + cardCode + "'";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);
                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("Email").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }


        #region Proceso_WebService
        public virtual void ActualizarEstadoDocumento(int nroDocumento, string estado, string codSeguridad)
        {
            Recordset oRecordSet = null;
            string seguridad = "";

            try
            {
                if (codSeguridad.Length > 0)
                {
                    seguridad = codSeguridad.Substring(0, 6);
                }
                else
                {
                    seguridad = String.Empty;
                }

                oRecordSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                string consulta = " update oinv set U_Estado = '" + estado + "' where DocEntry = " + nroDocumento + "";

                oRecordSet.DoQuery(consulta);
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("FacturaB1/Error: " + ex.ToString());
            }
            finally
            {
                if (oRecordSet != null)
                {
                    //Libera de memoria el objeto factura
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                    GC.Collect();
                }
            }
        }
        #endregion Proceso_WebService

        #region Transacciones_Periodicas
        public virtual void ActualizarOrigenBorrador(int nroDocumento, string objType, string estado, string formaPago)
        {
            Recordset oRecordSet = null;
            string tabla = ""; string cpoFormaPago = "";

            if (objType.Equals("112"))
            {
                tabla = "ODRF";
                cpoFormaPago = "U_U_FrmPagOIN";
            }

            try
            {
                oRecordSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                string consulta = "update " + tabla + " set U_Origen = 'WS', U_Estado = '" + estado + "', " + cpoFormaPago + " = '" + formaPago + "' where DocEntry = " + nroDocumento + "";

                oRecordSet.DoQuery(consulta);
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("FacturaB1/Error: " + ex.ToString());
            }
            finally
            {
                if (oRecordSet != null)
                {
                    //Libera de memoria el objeto factura
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                    GC.Collect();
                }
            }
        }
        #endregion Transacciones_Periodicas

        #region Redondeo
        /*public virtual void Redondeo(double p_Redondeo, ref CFE p_Cfe)
        {
            int numeroLinea, indFacturacion;                        
            
            if (p_Redondeo != 0)
            {
                numeroLinea = p_Cfe.Items.Count + 1;
                if (p_Redondeo > 0)
                {
                    indFacturacion = 6;                    
                }
                else
                {
                    indFacturacion = 7;
                }
                AgregarRedondeoCFE(ref p_Cfe, indFacturacion, p_Redondeo, numeroLinea);
            }
        }

        private void AgregarRedondeoCFE(ref CFE p_Cfe, int indFacturacion, double redondeo, int nroLinea)
        {
            CFEItems item;
            item = p_Cfe.NuevoItem();

            item.NumeroLinea = nroLinea;

            item.IndicadorFacturacion = indFacturacion;

            item.NombreItem = "I99999";
            item.DescripcionItem = "Ajuste por redondeo";
            item.CantidadItem = 1;
            item.PrecioUnitarioItem = redondeo;

            item.LineNum = nroLinea;
            item.UnidadMedida = "N/A";
            item.TipoImpuesto = "VEXE";

            p_Cfe.Redondeo = item;

            /*p_Cfe.Lineas++;
            p_Cfe.TotalMontoNoGravado += redondeo;
            p_Cfe.MontoTotalPagar += redondeo;
            p_Cfe.TotalMontoTotal += redondeo;*/
        /*}*/

        #endregion Redondeo
    }
}
