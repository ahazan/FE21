using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.DocumentosB1
{
    /// <summary>
    /// Contiene los metodos para obtener los valores de las notas de credito
    /// </summary>
    class NotaCreditoB1 : DocumentoB1
    {
        public static double descuentoGeneral = 0;
        public static double descuentoGeneralExtranjero = 0;
        public static double porcentajeDescuento = 0;

        public CFE ObtenerDatosNotaCredito(int numNotaCredito, CAE.ESTipoCFECFC tipoCFE, string formaPago, string adenda, string razonRef)
        {
            Documents notaCredito = null;
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
                notaCredito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oCreditNotes);
                notaCredito.GetByKey(numNotaCredito);

                //Actualiza la forma de pago
                notaCredito.UserFields.Fields.Item("U_U_FrmPagORI").Value = formaPago;
                notaCredito.Update();

                #region EMISOR

                emisor = this.ObtenerDatosEmisor() as Emisor;
                cfe.RucEmisor = emisor.Ruc;
                cfe.NombreEmisor = emisor.Nombre;
                cfe.NombreComercialEmisor = emisor.NombreComercial;
                cfe.NumeroResolucion = emisor.NumeroResolucion;
                cfe.CodigoCasaPrincipalEmisor = this.ObtenerSucursal(notaCredito.DocEntry.ToString(), TABLA_NOTA_CREDITO);
                cfe.CajaEmisor = this.ObtenerCaja(notaCredito.DocEntry.ToString(), TABLA_NOTA_CREDITO);
                cfe.DomicilioFiscalEmisor = this.ObtenerDireccionEmisor();
                cfe.CiuidadEmisor = this.ObtenerCiudadEmisor();
                cfe.DocumentoSAP = notaCredito.DocEntry.ToString();
                cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;

                #endregion EMISOR

                #region RECEPTOR

                cfe.TipoDocumentoReceptor = CFE.ObtenerTipoDocumentoReceptor(notaCredito.UserFields.Fields.Item("U_TipDocNC").Value);
                cfe.CodigoPaisReceptor = this.ObtenerCodPaisReceptor(notaCredito.CardCode, cfe.TipoDocumentoReceptor);
                cfe.NombreReceptor = notaCredito.CardName;

                if (cfe.PaisReceptor.Equals("UY"))
                {
                    cfe.NumDocReceptorUruguayo = notaCredito.UserFields.Fields.Item("U_ValDocNC").Value;
                }
                else
                {
                    cfe.NumDocReceptorExtrangero = notaCredito.UserFields.Fields.Item("U_ValDocNC").Value;

                    ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                    string ciGenereico = manteUdoAdobe.ObtenerCiGenerico();

                    if (cfe.NumDocReceptorExtrangero.Equals(ciGenereico))
                    {
                        cfe.NumDocReceptorExtrangero = "00000000";
                    }
                }                

                //cfe.CiuidadReceptor = notaCredito.Address.Replace("\r", "");
                cfe.CiuidadReceptor = this.ObtenerCiudad(notaCredito.DocEntry, "RIN12");
                cfe.PaisReceptor = this.ObtenerNombPaisReceptor(notaCredito.CardCode);
                cfe.CorreoReceptor = this.ObtenerCorreoReceptor(notaCredito.CardCode);
                cfe.DireccionReceptor = this.ObtenerCiudad(notaCredito.DocEntry, "RIN12");
                //cfe.DireccionReceptor = cfe.CiuidadReceptor;// +" " + cfe.PaisReceptor;
                cfe.NumeroCompraReceptor = this.ObtenerNroPedidoReceptor(notaCredito.DocEntry, TABLA_NOTA_CREDITO);

                #endregion RECEPTOR

                #region ITEMS

                for (int i = 0; i < notaCredito.Lines.Count; i++)
                {
                    notaCredito.Lines.SetCurrentLine(i);

                    //Si el Tipo de lista de Materiales de Ventas no lo agrego al XML 
                    if (!notaCredito.Lines.TreeType.ToString().Equals("iIngredient"))
                    {

                        //Nueva instancia del objeto item de cfe
                        item = cfe.NuevoItem();

                        item.NumeroLinea = i + 1;

                        string indFacturacionNumero = "";//notaCredito.Lines.UserFields.Fields.Item("U_IndNC").Value + "";

                        if (indFacturacionNumero.Equals("6"))
                        {
                            item.IndicadorFacturacion = 6;
                            notaCredito.Lines.TaxCode = "0";
                            int a = notaCredito.Update();
                        }
                        else if (indFacturacionNumero.Equals("7"))
                        {
                            item.IndicadorFacturacion = 7;
                            item.IndicadorFacturacion = 7;
                            notaCredito.Lines.TaxCode = "0";
                            notaCredito.Update();
                        }
                        #region FE_EXPORTACION
                        else if (this.ValidarClienteExtranjero(notaCredito.CardCode))
                        {
                            item.IndicadorFacturacion = 10;
                        }
                        #endregion FE_EXPORTACION
                        else
                        {
                            string resultadoIndicadorFacturacion = this.ObtenerIndicadorFacturacion(notaCredito.Lines.TaxCode.ToString());

                            if (resultadoIndicadorFacturacion.Equals(""))
                            {
                                item.IndicadorFacturacion = 0;
                            }
                            else
                            {
                                item.IndicadorFacturacion = Convert.ToInt16(resultadoIndicadorFacturacion);
                            }
                        }


                       
                        item.NombreItem = notaCredito.Lines.ItemCode;
                        item.DescripcionItem = notaCredito.Lines.ItemDescription;
                        item.CantidadItem = notaCredito.Lines.Quantity;
                        item.PrecioUnitarioItem = notaCredito.Lines.Price - (notaCredito.Lines.Price * (notaCredito.DiscountPercent / 100));


                        if (item.PrecioUnitarioItem.ToString().Equals("0"))
                        {
                            item.IndicadorFacturacion = 5;
                        }



                        item.PrecioUnitarioItemPDF = notaCredito.Lines.Price;
                        item.LineNum = notaCredito.Lines.LineNum;
                        item.ArticulosXUnidad = notaCredito.Lines.UnitsOfMeasurment;
                        item.UnidadMedida = this.ObtenerUnidadMedidaItem(notaCredito.Lines.ItemCode, notaCredito.DocEntry + "", notaCredito.Lines.LineNum + "", "RIN1");
                        item.UnidadMedidaPDF = item.UnidadMedida;
                        item.TipoImpuesto = notaCredito.Lines.TaxCode;

                        item.PrecioUnitarioItemFC = (notaCredito.Lines.RowTotalFC / notaCredito.Lines.Quantity) - ((notaCredito.Lines.RowTotalFC / notaCredito.Lines.Quantity) * (notaCredito.DiscountPercent / 100));
                        item.PrecioUnitarioItemPDF_FC = (notaCredito.Lines.RowTotalFC / notaCredito.Lines.Quantity);

                        cfe.AgregarItem(item);

                    }
                }

                #endregion ITEMS

                #region TOTALES ENCABEZADO

                cfe.TipoModena = this.ObtenerCodigoISOModena(notaCredito.DocCurrency);

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
                    cfe.TipoCambio = notaCredito.DocRate;
                }

                //Si el cliente es extrangero entonces el monto de exportacion y asimilados es la suma de las lineas si importar el codigo de impuesto que tenga
                //y el monto total a pagar va ser ese mismo monto. En el xslt en ambos casos se asigna el mismo valor
                if (this.ValidarClienteExtranjero(notaCredito.CardCode))
                {
                    cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesExportacion(notaCredito);
                }
                else
                {
                    cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("1"));// / cfe.TipoCambio;
                    cfe.TotalMontoNoGravadoExtranjero = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("1"));
                    cfe.TotalMontoImpuestoPercibido = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("11"));// / cfe.TipoCambio;
                    cfe.TotalMontoImpuestoPercibidoExtranjero = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("11"));
                    cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("12"));// / cfe.TipoCambio;
                    cfe.TotalMontoIVASuspensoExtranjero = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("12"));
                    cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("2"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaMinimaExtranjero = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("2"));
                    cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("3"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaBasicaExtranjero = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("3"));
                    cfe.TotalMontoNetoIVAOtraTasa = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("4"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVAOtraTasaExtranjero = this.ObtenerTotalesPorImpuesto(notaCredito, ObtenerCodigoImpuesto("4"));
                    cfe.TasaMinimaIVA = this.ObtenerIvaTasaMinima();
                    cfe.TasaBasicaIVA = this.ObtenerIvaTasaBasica();


                    //Montos para moneda del sietma FC, esta eestroctura se utliza para Secocenter que factura con moneda local Dolares
                    // y moneda extraneja Pesos UYU.
                    //**************************************************************************************************************************
                    cfe.TotalMontoNoGravadoFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("1"));/// cfe.TipoCambio;
                    cfe.TotalMontoNoGravadoExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("1"));
                    cfe.TotalMontoImpuestoPercibidoFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("11"));// / cfe.TipoCambio;
                    cfe.TotalMontoImpuestoPercibidoExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("11"));
                    cfe.TotalMontoIVASuspensoFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("12"));// / cfe.TipoCambio;
                    cfe.TotalMontoIVASuspensoExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("12"));
                    cfe.TotalMontoNetoIVATasaMinimaFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("2"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaMinimaExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("2"));
                    cfe.TotalMontoNetoIVATasaBasicaFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("3"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaBasicaExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("3"));
                    cfe.TotalMontoNetoIVAOtraTasaFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("4"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVAOtraTasaExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(notaCredito, ObtenerCodigoImpuesto("4"));
                    //**************************************************************************************************************************

                    cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(notaCredito.DocEntry.ToString(), "IVAB", "RIN1");
                }


                cfe.Lineas = notaCredito.Lines.Count;

                #endregion TOTALES ENCABEZADO

                #region MEDIOS DE PAGO

                mediosPago = cfe.NuevoMediosPago();
                mediosPago.Glosa = this.ObtenerMedioPago(notaCredito.DocEntry.ToString(), TABLA_NOTA_CREDITO);
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
                descuentoGeneral = notaCredito.TotalDiscount;
                descuentoGeneralExtranjero = notaCredito.TotalDiscountFC;
                porcentajeDescuento = notaCredito.DiscountPercent;

                #region IDENTIFICACION DEL COMPROBANTE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaCredito.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCETicketContingencia));
                    }
                    else
                    {
                        //Valida si es un cliente extranjero
                        if (this.ValidarClienteExtranjero(notaCredito.CardCode))
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFacturaExportacionContingencia));
                        }
                        else
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFacturaContingencia));
                        }
                    }
                }//Si no es contingencia
                else
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaCredito.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCETicket));
                        rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCETicket), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                    }
                    else
                    {
                        //Valida si es un cliente extranjero
                        if (this.ValidarClienteExtranjero(notaCredito.CardCode))
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFacturaExportacion));
                            rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFacturaExportacion), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                        }
                        else
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFactura));
                            rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFactura), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                        }
                    }
                }

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

                cfe.FechaComprobante = notaCredito.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = notaCredito.DocDueDate.ToString("yyyy-MM-dd");

                if (notaCredito.DocType == BoDocumentTypes.dDocument_Items)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Articulo;
                }
                else if (notaCredito.DocType == BoDocumentTypes.dDocument_Service)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Servicio;
                }

                //if (notaCredito.DocumentStatus == BoStatus.bost_Close)
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

                bool convertido = int.TryParse(notaCredito.UserFields.Fields.Item("U_ModVenNC").Value, out modalidad);

                if (convertido)
                {
                    cfe.ModalidadVentaInt = modalidad;
                }

                #region FE_EXPORTACION
                //cfe.ViaTransporteInt = notaCredito.TransportationCode;
                convertido = false;
                convertido = int.TryParse(notaCredito.UserFields.Fields.Item(Globales.Constantes.UDFViaTransporteNC).Value, out viaTransporte);
                if (convertido)
                {
                    cfe.ViaTransporteInt = viaTransporte;
                }
                #endregion FE_EXPORTACION

                cfe.ClausulaVenta = notaCredito.UserFields.Fields.Item("U_ClaVenNC").Value;


                #endregion EXPORTACION

                #region ADENDA
                //ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();
                //Adenda adenda = manteUdoAdenda.ObtenerAdenda(ProcConexion.Comp, Adenda.ESTipoObjetoAsignado.TipoCFE112, notaCredito.DocNum.ToString());

                //cfe.TextoLibreAdenda = adenda.CadenaAdenda;
                cfe.TextoLibreAdenda = adenda;// notaCredito.UserFields.Fields.Item("U_AdendaND").Value;

                #endregion ADENDA

                #region DOCUMENTOS DE REFENCIA

                /*cfeReferencia = (CFEInfoReferencia)this.ObtenerDocumentoReferencia(TABLA_DETALLES_NOTA_CREDITO, notaCredito.DocEntry.ToString());

                if (cfeReferencia != null)
                {
                    cfeReferencia.NumeroLinea = 1;
                }
                else
                {
                    cfeReferencia = new CFEInfoReferencia();
                    cfeReferencia.IndicadorReferenciaGlobal = CFEInfoReferencia.ESIndicadorReferenciaGlobal.ReferenciaGlobal;
                    cfeReferencia.NumeroLinea = 1;
                    cfeReferencia.NumeroComprobanteReferencia = "";
                    cfeReferencia.SerieComprobanteReferencia = "";

                      if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                      {
                            cfeReferencia.TipoCFEReferencia = 111;
                            cfeReferencia.SerieComprobanteReferencia ="A";//Serie del documento de referencia
                            cfeReferencia.NumeroComprobanteReferencia = "1";//Numero de CFE del documento de referencia
                      }


                    if (razonRef.Equals(""))
                    {

                        cfeReferencia.RazonReferencia = "Descuento General";
                    }
                    else
                    {
                        cfeReferencia.RazonReferencia = razonRef;
                    }
                }*/

                cfeReferencia = new CFEInfoReferencia();
                cfeReferencia.NumeroLinea = 1;
                cfeReferencia.SerieComprobanteReferencia = notaCredito.UserFields.Fields.Item("U_SerieRefNC").Value;

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
                        cfeReferencia.NumeroComprobanteReferencia = notaCredito.UserFields.Fields.Item("U_NumRefNC").Value;
                        cfeReferencia.RazonReferencia = razonRef;
                    }
                }

                if (cfeReferencia != null)
                {
                    cfe.AgregarInfoReferencia(cfeReferencia);
                }

                #endregion  DOCUMENTOS DE REFERENCIA

                #region CAE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(notaCredito.CardCode))
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
                    if (this.ValidarClienteContado(notaCredito.CardCode))
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCETicket)) as CAE;
                    }
                    else
                    {
                        #region FE_EXPORTACION
                        //cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFactura)) as CAE;
                        if (ValidarClienteExtranjero(notaCredito.CardCode))
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFacturaExportacion)) as CAE;
                        }
                        else
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.NCEFactura)) as CAE;
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
                cfe.OrigenFE = notaCredito.UserFields.Fields.Item("U_Origen").Value;
                #endregion Web Service
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("NotaCreditoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (notaCredito != null)
                {
                    //Libera de memoria el obtejo notaCredito
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(notaCredito);
                    GC.Collect();
                }
            }

            return cfe;
        }

        /// <summary>
        /// Actualiza los datos de CFE en el documento procesado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numNotaCredito"></param>
        /// <param name="serie"></param>
        /// <param name="numCFE"></param>
        public void ActualizarDatosCFENotaCredito(int numNotaCredito, string serie, string numCFE, List<CFEInfoReferencia> referencias)
        {
            Documents notaCredito = null;
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //notaCredito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oCreditNotes);

                //notaCredito.GetByKey(numNotaCredito);

                //notaCredito.UserFields.Fields.Item("U_SerieNC").Value = serie;
                //notaCredito.UserFields.Fields.Item("U_NumeroNC").Value = numCFE;


                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);


                if (referencias != null)
                {
                    foreach (CFEInfoReferencia referenciaNC in referencias)
                    {
                        if (!referenciaNC.SerieComprobanteReferencia.Equals("")
                            && !referenciaNC.NumeroComprobanteReferencia.Equals(""))
                        {
                            //notaCredito.UserFields.Fields.Item("U_SerieRefNC").Value = referenciaNC.SerieComprobanteReferencia;
                            //notaCredito.UserFields.Fields.Item("U_NumRefNC").Value = referenciaNC.NumeroComprobanteReferencia;

                            consulta = " Update ORIN set U_SerieNC = '" + serie + "' , U_NumeroNC = '" + numCFE + "' , U_SerieRefNC = '" + referenciaNC.SerieComprobanteReferencia + "', U_NumRefNC = '" + referenciaNC.NumeroComprobanteReferencia + "'  where DocEntry = '" + numNotaCredito + "'";

                        }

                        else

                        {


                            consulta = " Update ORIN set U_SerieNC = '" + serie + "' , U_NumeroNC = '" + numCFE + "' where DocEntry = '" + numNotaCredito + "'";

                        }
                    }
                }

                else
{


                consulta = " Update ORIN set U_SerieNC = '" + serie + "' , U_NumeroNC = '" + numCFE + "' where DocEntry = '" + numNotaCredito + "'";
             
}

                //Ejecutar consulta
                recSet.DoQuery(consulta);

               // notaCredito.Update();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("NotaCreditoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
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
            Documents notaCredito = null;
            CFE cfe = new CFE();

            try
            {
                notaCredito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oCreditNotes);

                notaCredito.GetByKey(numeroDocumento);

                cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(notaCredito, "IVAVEXE");
                cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesPorImpuesto(notaCredito, " IVAEXPO");
                cfe.TotalMontoImpuestoPercibido = 0;
                cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(notaCredito, "IVASUSP");
                cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(notaCredito, "IVAVTM");
                cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(notaCredito, "IVAVTB");
                cfe.TotalMontoNetoIVAOtraTasa = 0;
                cfe.TasaMinimaIVA = this.ObtenerMontoTasa("IVAVTM");
                cfe.TasaBasicaIVA = this.ObtenerMontoTasa("IVAVTB");
                cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(notaCredito.DocEntry.ToString(), "IVAVEXE", "RIN1");
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("NotaCreditoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (notaCredito != null)
                {
                    //Libera de memoria el obtejo notaCredito
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(notaCredito);
                    GC.Collect();
                }
            }

            return cfe;
        }
    }
}
