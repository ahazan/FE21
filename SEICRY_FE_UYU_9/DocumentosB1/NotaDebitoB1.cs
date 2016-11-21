using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Udos;

namespace SEICRY_FE_UYU_9.DocumentosB1
{
    /// <summary>
    /// Contiene los metodos para obtener los valores de las notas de debito
    /// </summary>
    class NotaDebitoB1 : DocumentoB1
    {
        public static double descuentoGeneral = 0;
        public static double descuentoGeneralExtranjero = 0;
        public static double porcentajeDescuento = 0;

        public CFE ObtenerDatosNotaDebito(int numNotaDebito, CAE.ESTipoCFECFC tipoCFE, string formaPago, string adenda, string razonRef)
        {
            Documents notaDebito = null;
            CFE cfe = new CFE();
            CFEInfoReferencia cfeReferencia;
            CFEItems item;
            Emisor emisor;
            Rango rango = null;
            CFEMediosPago mediosPago;
            CAE cae;
            descuentoGeneral = 0;
            descuentoGeneralExtranjero = 0;
            porcentajeDescuento = 0;

            try
            {

                notaDebito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);
                notaDebito.GetByKey(numNotaDebito);

                //Actualiza la forma de pagos
                notaDebito.UserFields.Fields.Item("U_U_FrmPagOIN").Value = formaPago;
                notaDebito.Update();

                #region EMISOR

                emisor = this.ObtenerDatosEmisor() as Emisor;
                cfe.RucEmisor = emisor.Ruc;
                cfe.NombreEmisor = emisor.Nombre;
                cfe.NombreComercialEmisor = emisor.NombreComercial;
                cfe.NumeroResolucion = emisor.NumeroResolucion;
                cfe.CodigoCasaPrincipalEmisor = this.ObtenerSucursal(notaDebito.DocEntry.ToString(), TABLA_NOTA_DEBITO);
                cfe.CajaEmisor = this.ObtenerCaja(notaDebito.DocEntry.ToString(), TABLA_NOTA_DEBITO);
                cfe.DomicilioFiscalEmisor = this.ObtenerDireccionEmisor();
                cfe.CiuidadEmisor = this.ObtenerCiudadEmisor();
                cfe.DocumentoSAP = notaDebito.DocEntry.ToString();
                cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;

                #endregion EMISOR

                #region RECEPTOR

                cfe.TipoDocumentoReceptor = CFE.ObtenerTipoDocumentoReceptor(notaDebito.UserFields.Fields.Item("U_TipDocND").Value);
                cfe.CodigoPaisReceptor = this.ObtenerCodPaisReceptor(notaDebito.CardCode, cfe.TipoDocumentoReceptor);
                cfe.NombreReceptor = notaDebito.CardName;

                if (cfe.PaisReceptor.Equals("UY"))
                {
                    cfe.NumDocReceptorUruguayo = notaDebito.UserFields.Fields.Item("U_ValDocND").Value;
                }
                else
                {
                    cfe.NumDocReceptorExtrangero = notaDebito.UserFields.Fields.Item("U_ValDocND").Value;

                    ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                    string ciGenereico = manteUdoAdobe.ObtenerCiGenerico();

                    if (cfe.NumDocReceptorExtrangero.Equals(ciGenereico))
                    {
                        cfe.NumDocReceptorExtrangero = "00000000";
                    }
                }

                //cfe.CiuidadReceptor = notaDebito.Address.Replace("\r", "");
                cfe.CiuidadReceptor = this.ObtenerCiudad(notaDebito.DocEntry, "INV12");
                cfe.PaisReceptor = this.ObtenerNombPaisReceptor(notaDebito.CardCode);
                cfe.CorreoReceptor = this.ObtenerCorreoReceptor(notaDebito.CardCode);
                //cfe.DireccionReceptor = cfe.CiuidadReceptor;// +" " + cfe.PaisReceptor;
                cfe.DireccionReceptor = this.ObtenerCiudad(notaDebito.DocEntry, "INV12");
                cfe.NumeroCompraReceptor = this.ObtenerNroPedidoReceptor(notaDebito.DocEntry, TABLA_NOTA_DEBITO);

                #endregion RECEPTOR

                #region TOTALES ENCABEZADO

                cfe.TipoModena = this.ObtenerCodigoISOModena(notaDebito.DocCurrency);

                ManteUdoTipoCambio manteUdoTipoCambio = new ManteUdoTipoCambio();
                string temp = "", confTC = manteUdoTipoCambio.ObtenerConfiguracion(out temp);

                if (confTC.Equals("N"))
                {
                     double docRate = this.ObtenerDocRate();
                    
                    if (cfe.TipoModena == "UYU" || cfe.TipoModena == "UY")
                    {
                                               
                            cfe.TipoCambio = 1;
                     }
                        else
                        {
                             if (docRate > 0)
                                {
                                    cfe.TipoCambio = 1 / docRate;
                                    cfe.TipoCambio = Math.Round(cfe.TipoCambio, 2);
                                }  
                        }                        
                    }
                                    
                else
                {
                    cfe.TipoCambio = notaDebito.DocRate;
                }

                //Si el cliente es extrangero entonces el monto de exportacion y asimilados es la suma de las lineas si importar el codigo de impuesto que tenga
                //y el monto total a pagar va ser ese mismo monto. En el xslt en ambos casos se asigna el mismo valor
                if (this.ValidarClienteExtranjero(notaDebito.CardCode))
                {
                    cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesExportacion(notaDebito);
                }
                else
                {
                    cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("1"));// / cfe.TipoCambio;
                    cfe.TotalMontoNoGravadoExtranjero = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("1"));
                    cfe.TotalMontoImpuestoPercibido = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("11"));// / cfe.TipoCambio;
                    cfe.TotalMontoImpuestoPercibidoExtranjero = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("11"));
                    cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("12"));// / cfe.TipoCambio;
                    cfe.TotalMontoIVASuspensoExtranjero = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("12"));
                    cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("2"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaMinimaExtranjero = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("2"));
                    cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("3"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaBasicaExtranjero = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("3"));
                    cfe.TotalMontoNetoIVAOtraTasa = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("4"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVAOtraTasaExtranjero = this.ObtenerTotalesPorImpuesto(notaDebito, ObtenerCodigoImpuesto("4"));
                    cfe.TasaMinimaIVA = this.ObtenerIvaTasaMinima();
                    cfe.TasaBasicaIVA = this.ObtenerIvaTasaBasica();

                    cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(notaDebito.DocEntry.ToString(), "IVAB", "INV1");
                }

                cfe.Lineas = notaDebito.Lines.Count;

                #endregion TOTALES ENCABEZADO

                #region ITEMS

                for (int i = 0; i < notaDebito.Lines.Count; i++)
                {
                    notaDebito.Lines.SetCurrentLine(i);


                        //Si el Tipo de lista de Materiales de Ventas no lo agrego al XML 
                    if (!notaDebito.Lines.TreeType.ToString().Equals("iIngredient"))
                    {


                        //Nueva instancia del objeto item de cfe
                        item = cfe.NuevoItem();

                        item.NumeroLinea = i + 1;

                        string indFacturacionNumero = "";// notaDebito.Lines.UserFields.Fields.Item("U_IndND").Value + "";

                        if (indFacturacionNumero.Equals("6"))
                        {
                            item.IndicadorFacturacion = 6;
                        }
                        else if (indFacturacionNumero.Equals("7"))
                        {
                            item.IndicadorFacturacion = 7;
                        }
                        #region FE_EXPORTACION
                        else if (this.ValidarClienteExtranjero(notaDebito.CardCode))
                        {
                            item.IndicadorFacturacion = 10;
                        }
                        #endregion FE_EXPORTACION
                        else
                        {
                            string resultadoIndicadorFacturacion = this.ObtenerIndicadorFacturacion(notaDebito.Lines.TaxCode.ToString());

                            if (resultadoIndicadorFacturacion.Equals(""))
                            {
                                item.IndicadorFacturacion = 0;
                            }
                            else
                            {
                                item.IndicadorFacturacion = Convert.ToInt16(resultadoIndicadorFacturacion);
                            }


                          
                        }
                        item.NombreItem = notaDebito.Lines.ItemCode;
                        item.DescripcionItem = notaDebito.Lines.ItemDescription;
                        item.CantidadItem = notaDebito.Lines.Quantity;
                        item.PrecioUnitarioItem = notaDebito.Lines.Price - (notaDebito.Lines.Price * (notaDebito.DiscountPercent / 100));

                        if (item.PrecioUnitarioItem.ToString().Equals("0"))
                        {
                            item.IndicadorFacturacion = 5;
                        }


                        item.PrecioUnitarioItemPDF = notaDebito.Lines.Price;
                        item.LineNum = notaDebito.Lines.LineNum;
                        item.ArticulosXUnidad = notaDebito.Lines.UnitsOfMeasurment;
                        item.UnidadMedida = this.ObtenerUnidadMedidaItem(notaDebito.Lines.ItemCode, notaDebito.DocEntry + "", notaDebito.Lines.LineNum + "", "INV1");
                        item.UnidadMedidaPDF = item.UnidadMedida;
                        item.TipoImpuesto = notaDebito.Lines.TaxCode;

                        cfe.AgregarItem(item);

                    }
                }

                #endregion ITEMS

                #region MEDIOS DE PAGO

                mediosPago = cfe.NuevoMediosPago();
                mediosPago.Glosa = this.ObtenerMedioPago(notaDebito.DocEntry.ToString(), TABLA_NOTA_DEBITO);
                if (formaPago.Equals("Contado"))
                {
                    cfe.FormaPago = CFE.ESFormaPago.Contado;
                }
                else
                {
                    cfe.FormaPago = CFE.ESFormaPago.Credito;
                }
                cfe.AgregarMediosPago(mediosPago);

                #endregion MEDIOS DE PAGO

                descuentoGeneral = notaDebito.TotalDiscount;
                descuentoGeneralExtranjero = notaDebito.TotalDiscountFC;
                porcentajeDescuento = notaDebito.DiscountPercent;

                #region IDENTIFICACION DEL COMPROBANTE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaDebito.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDETicketContingencia));
                    }
                    else
                    {
                        //Valida si es un ciente extranjero
                        if (this.ValidarClienteExtranjero(notaDebito.CardCode))
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFacturaExportacionContingencia));
                        }
                        else
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFacturaContingencia));
                        }
                    }
                }//Si no es contingencia
                else
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaDebito.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDETicket));
                        rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDETicket), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                    }
                    else
                    {
                        //Valida si es un cliente extranjero
                        if (this.ValidarClienteExtranjero(notaDebito.CardCode))
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFacturaExportacion));
                            rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFacturaExportacion), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                        }
                        else
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFactura));
                            rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFactura), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                        }
                    }
                }

                //Estado de contigencia serie y numero se ingresan manualmente
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    SAPbouiCOM.Item txtNumCFE = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtNumCFE");
                    SAPbouiCOM.Item txtSeCFE = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtSeCFE");

                    cfe.SerieComprobante = ((SAPbouiCOM.EditText)txtSeCFE.Specific).Value + "";
                    cfe.NumeroComprobante = int.Parse(((SAPbouiCOM.EditText)txtNumCFE.Specific).Value + "");
                }
                else
                {
                    if (rango != null)
                    {
                        cfe.SerieComprobante = rango.Serie;
                        cfe.NumeroComprobante = rango.NumeroActual;
                    }
                }

                cfe.FechaComprobante = notaDebito.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = notaDebito.DocDueDate.ToString("yyyy-MM-dd");

                if (notaDebito.DocType == BoDocumentTypes.dDocument_Items)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Articulo;
                }
                else if (notaDebito.DocType == BoDocumentTypes.dDocument_Service)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Servicio;
                }

                //if (notaDebito.DocumentStatus == BoStatus.bost_Close)
                //{
                //    cfe.FormaPago = CFE.ESFormaPago.Contado;
                //}
                //else
                //{
                //    cfe.FormaPago = CFE.ESFormaPago.Credito;
                //}

                #endregion IDENTIFICACION DEL COMPROBANTE

                #region EXPORTACION

                int modalidad, viaTransporte;

                bool convertido = int.TryParse(notaDebito.UserFields.Fields.Item("U_ModVenND").Value, out modalidad);

                if (convertido)
                {
                    cfe.ModalidadVentaInt = modalidad;
                }

                #region FE_EXPORTACION
                //cfe.ViaTransporteInt = notaDebito.TransportationCode;
                convertido = false;
                convertido = int.TryParse(notaDebito.UserFields.Fields.Item(Globales.Constantes.UDFViaTransporteND).Value, out viaTransporte);
                if (convertido)
                {
                    cfe.ViaTransporteInt = viaTransporte;
                }
                #endregion FE_EXPORTACION

                cfe.ClausulaVenta = notaDebito.UserFields.Fields.Item("U_ClaVenND").Value;

                #endregion EXPORTACION

                #region ADENDA

                //ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();
                //Adenda adenda = manteUdoAdenda.ObtenerAdenda(ProcConexion.Comp, Adenda.ESTipoObjetoAsignado.TipoCFE113, notaDebito.DocNum.ToString());

                //cfe.TextoLibreAdenda = adenda.CadenaAdenda;
                cfe.TextoLibreAdenda = adenda;// notaDebito.UserFields.Fields.Item("U_AdendaND").Value;

                #endregion ADENDA

                #region DOCUMENTO DE REFERENCIA                

                //cfeReferencia = (CFEInfoReferencia)this.ObtenerDocumentoReferencia(TABLA_DETALLES_NOTA_DEBITO, notaDebito.DocEntry.ToString());

                //if (cfeReferencia != null)
                //{
                //    cfeReferencia.NumeroLinea = 1;
                //}
                //else
                //{
                //    cfeReferencia = new CFEInfoReferencia();
                //    cfeReferencia.IndicadorReferenciaGlobal = CFEInfoReferencia.ESIndicadorReferenciaGlobal.ReferenciaGlobal;
                //    cfeReferencia.NumeroLinea = 1;
                //    cfeReferencia.NumeroComprobanteReferencia = "";
                //    cfeReferencia.SerieComprobanteReferencia = "";

                //    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                //    {
                //        cfeReferencia.TipoCFEReferencia = 111;
                //        cfeReferencia.SerieComprobanteReferencia = "A";//Serie del documento de referencia
                //        cfeReferencia.NumeroComprobanteReferencia = "1";//Numero de CFE del documento de referencia
                //    }

                //    if (razonRef.Equals(""))
                //    {

                //        cfeReferencia.RazonReferencia = "Descuento General";
                //    }
                //    else
                //    {
                //        cfeReferencia.RazonReferencia = razonRef;
                //    }
                //}

                cfeReferencia = new CFEInfoReferencia();
                cfeReferencia.NumeroLinea = 1;
                cfeReferencia.SerieComprobanteReferencia = notaDebito.UserFields.Fields.Item("U_SerieRefND").Value;

                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    cfeReferencia.TipoCFEReferencia = 111;
                    cfeReferencia.SerieComprobanteReferencia = "A"; //Serie del documento de referencia
                    cfeReferencia.NumeroComprobanteReferencia = "1"; //Numero de CFE del documento de referencia
                }
                else
                {
                    if (cfeReferencia.SerieComprobanteReferencia == "" || cfeReferencia.SerieComprobanteReferencia == "1")
                    {
                        cfeReferencia.IndicadorReferenciaGlobal = CFEInfoReferencia.ESIndicadorReferenciaGlobal.ReferenciaGlobal;
                        cfeReferencia.SerieComprobanteReferencia = "";
                        cfeReferencia.NumeroComprobanteReferencia = "";
                        cfeReferencia.RazonReferencia = "Descuento General";
                    }
                    else
                    {
                        cfeReferencia.TipoCFEReferencia = 111;
                        cfeReferencia.NumeroComprobanteReferencia = notaDebito.UserFields.Fields.Item("U_NumRefND").Value;
                        cfeReferencia.RazonReferencia = razonRef;
                    }
                }

                if (cfeReferencia != null)
                {
                    cfe.AgregarInfoReferencia(cfeReferencia);
                }

                #endregion  DOCUMENTO DE REFERENCIA

                #region CAE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaDebito.CardCode))
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.Contingencia)) as CAE;
                    }
                    else
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.Contingencia)) as CAE;
                    }
                }//Si no es contingencia
                else
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaDebito.CardCode))
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDETicket)) as CAE;
                    }
                    else
                    {
                        #region FE_EXPORTACION
                        //cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFactura)) as CAE;                        
                        if (ValidarClienteExtranjero(notaDebito.CardCode))
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFacturaExportacion)) as CAE;
                        }
                        else
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NDEFactura)) as CAE;
                        }
                        #endregion FE_EXPORTACION
                    }
                }

                if (cae.NumeroAutorizacion != null)
                {
                    cfe.NumeroCAE = long.Parse(cae.NumeroAutorizacion);
                    cfe.NumeroInicialCAE = cae.NumeroDesde;
                    cfe.NumeroFinalCAE = cae.NumeroHasta;
                    cfe.FechaVencimientoCAE = cae.FechaVencimiento;

                    ManteDocumentos manteDocumentos = new ManteDocumentos();
                    FinCae finCae = manteDocumentos.ObtenerAlertaCAE();

                      if (finCae.Cantidad > 0 && rango != null)
                    {
                    if ((cae.NumeroHasta - rango.NumeroActual) <= finCae.Cantidad)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox
                            ("!!!Atención!!!  Rango utilizado: " + rango.NumeroActual + " de: " + cae.NumeroHasta +
                            ". Solicitar Nuevos CAEs a DGI.");
                    }
                    }

                    DateTime fechaVencimiento = DateTime.Parse(cae.FechaVencimiento);
                    TimeSpan diferenciaDias = DateTime.Now.Subtract(fechaVencimiento);
                    int diasFecha = int.Parse(diferenciaDias.Days.ToString());

                    if (diasFecha > 0 && finCae.Dias != 0)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox
                            ("!!!Atención!!! El rango se venció, fecha de vencimiento: " + cae.FechaVencimiento);
                    }
                    else
                    {
                        diasFecha = diasFecha * -1;

                        if (diasFecha <= finCae.Dias && finCae.Dias != 0)
                        {
                            SAPbouiCOM.Framework.Application.SBO_Application.MessageBox
                                ("!!!Atención!!! El rango se va vencer, fecha de vencimiento: " + cae.FechaVencimiento);
                        }
                    }
                }
                else
                {
                    cfe = null;
                }

                #endregion CAE

                #region Web Service
                cfe.OrigenFE = notaDebito.UserFields.Fields.Item("U_Origen").Value;
                #endregion Web Service
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("NotaDebitoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (notaDebito != null)
                {
                    //Libera de memoria el obtejo notaDebito
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(notaDebito);
                    GC.Collect();
                }
            }

            return cfe;
        }

        /// <summary>
        /// Actualiza los datos de CFE en el documento procesado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numNotaDebito"></param>
        /// <param name="serie"></param>
        /// <param name="numCFE"></param>
        public void ActualizarDatosCFENotaDebito(int numNotaDebito, string serie, string numCFE, List<CFEInfoReferencia> referencias)
        {
            Documents notaDebito = null;

            try
            {
                notaDebito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);
                notaDebito.GetByKey(numNotaDebito);
                notaDebito.UserFields.Fields.Item("U_SerieND").Value = serie;
                notaDebito.UserFields.Fields.Item("U_NumeroND").Value = numCFE;

                if (referencias != null)
                {
                    foreach (CFEInfoReferencia referenciaND in referencias)
                    {
                        notaDebito.UserFields.Fields.Item("U_SerieRefND").Value = referenciaND.SerieComprobanteReferencia;
                        notaDebito.UserFields.Fields.Item("U_NumRefND").Value = referenciaND.NumeroComprobanteReferencia;
                    }
                }
                notaDebito.Update();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("NotaDebitoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (notaDebito != null)
                {
                    //Libera de memoria el obtejo notaDebito
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(notaDebito);
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Retorna un CFE con los totales para un documento determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public CFE ObtenerTotalesCertificado(int numeroDocumento)
        {
            CFE cfe = new CFE();

            Documents notaDebito = null;

            try
            {
                notaDebito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);
                notaDebito.GetByKey(numeroDocumento);

                cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(notaDebito, "IVAVEXE");
                cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesPorImpuesto(notaDebito, " IVAEXPO");
                cfe.TotalMontoImpuestoPercibido = 0;
                cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(notaDebito, "IVASUSP");
                cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(notaDebito, "IVAVTM");
                cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(notaDebito, "IVAVTB");
                cfe.TotalMontoNetoIVAOtraTasa = 0;
                cfe.TasaMinimaIVA = this.ObtenerMontoTasa("IVAVTM");
                cfe.TasaBasicaIVA = this.ObtenerMontoTasa("IVAVTB");
                cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(notaDebito.DocEntry.ToString(), "IVAVEXE", "INV1");
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("NotaDebitoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (notaDebito != null)
                {
                    //Libera de memoria el obtejo notaDebito
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(notaDebito);
                    GC.Collect();
                }
            }



            return cfe;
        }
    }
}
