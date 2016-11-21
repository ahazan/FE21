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
    /// Contiene los metodos para obtener los valores de las facturas
    /// </summary>
    class FacturaB1 : DocumentoB1
    {
        public static double descuentoGeneral = 0;
        public static double descuentoGeneralExtranjero = 0;
        public static double porcentajeDescuento = 0;

        public CFE ObtenerDatosFactura(int numFactura, CAE.ESTipoCFECFC tipoCFE,string formaPago, string adenda)
        {            
            Documents factura = null;

            CFE cfe = new CFE();
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
                factura = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);

                factura.GetByKey(numFactura);

                //Actualiza la forma de pago
                factura.UserFields.Fields.Item("U_U_FrmPagOIN").Value = formaPago;
                factura.Update();

                #region EMISOR

                emisor = this.ObtenerDatosEmisor() as Emisor;
                cfe.RucEmisor = emisor.Ruc;
                cfe.NombreEmisor = emisor.Nombre;
                cfe.NombreComercialEmisor = emisor.NombreComercial;
                cfe.NumeroResolucion = emisor.NumeroResolucion;
                cfe.CodigoCasaPrincipalEmisor = this.ObtenerSucursal(factura.DocEntry.ToString(), TABLA_FACTURA);
                cfe.CajaEmisor = this.ObtenerCaja(factura.DocEntry.ToString(), TABLA_FACTURA);
                cfe.DomicilioFiscalEmisor = this.ObtenerDireccionEmisor();
                cfe.CiuidadEmisor = this.ObtenerCiudadEmisor();
                cfe.DocumentoSAP = factura.DocEntry.ToString();
                cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;
              

                #endregion EMISOR

                #region RECEPTOR

                cfe.TipoDocumentoReceptor = CFE.ObtenerTipoDocumentoReceptor(factura.UserFields.Fields.Item("U_TipDocFA").Value);
                cfe.CodigoPaisReceptor = this.ObtenerCodPaisReceptor(factura.CardCode, cfe.TipoDocumentoReceptor);
                cfe.NombreReceptor = factura.CardName;

                if (cfe.PaisReceptor.Equals("UY"))
                {
                    cfe.NumDocReceptorUruguayo = factura.UserFields.Fields.Item("U_ValDocFA").Value;
                }
                else
                {
                    cfe.NumDocReceptorExtrangero = factura.UserFields.Fields.Item("U_ValDocFA").Value;

                    ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                    string ciGenereico = manteUdoAdobe.ObtenerCiGenerico();

                    if (cfe.NumDocReceptorExtrangero.Equals(ciGenereico))
                    {
                        cfe.NumDocReceptorExtrangero = "00000000";
                    }
                }
                
                //cfe.CiuidadReceptor = factura.Address.Replace("\r", "");
                cfe.CiuidadReceptor = this.ObtenerCiudad(factura.DocEntry, "INV12");
                cfe.PaisReceptor = this.ObtenerNombPaisReceptor(factura.CardCode);
                cfe.CorreoReceptor = this.ObtenerCorreoReceptor(factura.CardCode);
                cfe.DireccionReceptor = this.ObtenerCiudad(factura.DocEntry, "INV12"); //cfe.CiuidadReceptor;// + " " + cfe.PaisReceptor;
                cfe.NumeroCompraReceptor = this.ObtenerNroPedidoReceptor(factura.DocEntry, TABLA_FACTURA);

                #endregion RECEPTOR

                #region ITEMS

                for (int i = 0; i < factura.Lines.Count; i++)
                {
                    factura.Lines.SetCurrentLine(i);

                    //Si el Tipo de lista de Materiales de Ventas no lo agrego al XML 
                   if (!factura.Lines.TreeType.ToString().Equals("iIngredient"))
                    {
                      
                   
                    //Nueva instancia del objeto item de cfe
                    item = cfe.NuevoItem();

                    item.NumeroLinea = i + 1;

                    string indFacturacionNumero = ""; // factura.Lines.UserFields.Fields.Item("U_IndFA").Value + "";

                    if (indFacturacionNumero.Equals("6"))
                    {
                        item.IndicadorFacturacion = 6;
                    }
                    else if (indFacturacionNumero.Equals("7"))
                    {
                        item.IndicadorFacturacion = 7;
                    }
                    #region FE_EXPORTACION
                    else if (this.ValidarClienteExtranjero(factura.CardCode))
                    {
                        item.IndicadorFacturacion = 10;
                    }
                    #endregion FE_EXPORTACION
                    else
                    {
                        string resultadoIndicadorFacturacion = this.ObtenerIndicadorFacturacion(factura.Lines.TaxCode.ToString());

                        if (resultadoIndicadorFacturacion.Equals(""))
                        {
                            item.IndicadorFacturacion = 0;
                        }
                        else
                        {
                            item.IndicadorFacturacion = Convert.ToInt16(resultadoIndicadorFacturacion);
                        }
                    }

                                                                           
                     
                    item.NombreItem = factura.Lines.ItemCode;
                    item.DescripcionItem = factura.Lines.ItemDescription;
                    item.CantidadItem = factura.Lines.Quantity;
                    item.PrecioUnitarioItem = factura.Lines.Price - (factura.Lines.Price * (factura.DiscountPercent / 100));


                    if (item.PrecioUnitarioItem.ToString().Equals("0"))
                    {
                        item.IndicadorFacturacion = 5;
                    }

                   
                    item.PrecioUnitarioItemPDF = factura.Lines.Price;
                    item.LineNum = factura.Lines.LineNum;
                    item.ArticulosXUnidad = factura.Lines.UnitsOfMeasurment;
                    item.UnidadMedida = this.ObtenerUnidadMedidaItem(factura.Lines.ItemCode, factura.DocEntry + "", factura.Lines.LineNum + "", "INV1");
                    item.UnidadMedidaPDF = item.UnidadMedida;
                    item.TipoImpuesto = factura.Lines.TaxCode;


                    item.PrecioUnitarioItemFC = (factura.Lines.RowTotalFC / factura.Lines.Quantity) - ((factura.Lines.RowTotalFC / factura.Lines.Quantity) * (factura.DiscountPercent / 100));
                    item.PrecioUnitarioItemPDF_FC = (factura.Lines.RowTotalFC / factura.Lines.Quantity);

                    cfe.AgregarItem(item);

                    } 


                }
                descuentoGeneral = factura.TotalDiscount;
                descuentoGeneralExtranjero = factura.TotalDiscountFC;
                porcentajeDescuento = factura.DiscountPercent;
                
                #endregion ITEMS

                #region TOTALES ENCABEZADO

                cfe.TipoModena = this.ObtenerCodigoISOModena(factura.DocCurrency);

                ManteUdoTipoCambio manteUdoTipoCambio = new ManteUdoTipoCambio();
                string temp = "", confTC = manteUdoTipoCambio.ObtenerConfiguracion(out temp);

                if (confTC.Equals("N"))
                {
                    double docRate =  this.ObtenerDocRate();

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
                    cfe.TipoCambio = factura.DocRate;
                }

                //Si el cliente es extrangero entonces el monto de exportacion y asimilados es la suma de las lineas si importar el codigo de impuesto que tenga
                //y el monto total a pagar va ser ese mismo monto. En el xslt en ambos casos se asigna el mismo valor
                if (this.ValidarClienteExtranjero(factura.CardCode))
                {
                    cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesExportacion(factura);
                }
                else
                {
                    cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("1"));/// cfe.TipoCambio;
                    cfe.TotalMontoNoGravadoExtranjero = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("1"));
                    cfe.TotalMontoImpuestoPercibido = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("11"));// / cfe.TipoCambio;
                    cfe.TotalMontoImpuestoPercibidoExtranjero = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("11"));
                    cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("12"));// / cfe.TipoCambio;
                    cfe.TotalMontoIVASuspensoExtranjero = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("12"));
                    cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("2"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaMinimaExtranjero = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("2"));
                    cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("3"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaBasicaExtranjero = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("3"));
                    cfe.TotalMontoNetoIVAOtraTasa = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("4"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVAOtraTasaExtranjero = this.ObtenerTotalesPorImpuesto(factura, ObtenerCodigoImpuesto("4"));
                    cfe.TasaMinimaIVA = this.ObtenerIvaTasaMinima();
                    cfe.TasaBasicaIVA = this.ObtenerIvaTasaBasica();



                    //Montos para moneda del sietma FC, esta eestroctura se utliza para Secocenter que factura con moneda local Dolares
                    // y moneda extraneja Pesos UYU.
                    //**************************************************************************************************************************
                    cfe.TotalMontoNoGravadoFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("1"));/// cfe.TipoCambio;
                    cfe.TotalMontoNoGravadoExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("1"));
                    cfe.TotalMontoImpuestoPercibidoFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("11"));// / cfe.TipoCambio;
                    cfe.TotalMontoImpuestoPercibidoExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("11"));
                    cfe.TotalMontoIVASuspensoFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("12"));// / cfe.TipoCambio;
                    cfe.TotalMontoIVASuspensoExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("12"));
                    cfe.TotalMontoNetoIVATasaMinimaFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("2"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaMinimaExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("2"));
                    cfe.TotalMontoNetoIVATasaBasicaFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("3"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaBasicaExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("3"));
                    cfe.TotalMontoNetoIVAOtraTasaFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("4"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVAOtraTasaExtranjeroFC = this.ObtenerTotalesFCPorImpuesto(factura, ObtenerCodigoImpuesto("4"));                
                    //**************************************************************************************************************************


                    cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(factura.DocEntry.ToString(), "IVAB", "INV1");
                }

                cfe.Lineas = factura.Lines.Count;

                #endregion TOTALES ENCABEZADO

                #region MEDIOS DE PAGO

                mediosPago = cfe.NuevoMediosPago();
                mediosPago.Glosa = this.ObtenerMedioPago(factura.DocEntry.ToString(), TABLA_FACTURA);
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


                #region IDENTIFICACION DEL COMPROBANTE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(factura.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ETicketContingencia));
                    }
                    else
                    {
                        if (this.ValidarClienteExtranjero(factura.CardCode))
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFacturaExportacionContingencia));
                        }
                        else
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFacturaContingencia));
                        }
                    }
                }//Si no es contingencia
                else
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(factura.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ETicket));
                        rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ETicket), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                    }
                    else
                    {
                        if (this.ValidarClienteExtranjero(factura.CardCode))
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFacturaExportacion));
                            rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFacturaExportacion), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                        }
                        else
                        {
                            cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFactura));
                            rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFactura), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                        }
                    }
                }

                //Estado de contingencia serie y numeros ingresados manualmente
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

                cfe.FechaComprobante = factura.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = factura.DocDueDate.ToString("yyyy-MM-dd");

                if (factura.DocType == BoDocumentTypes.dDocument_Items)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Articulo;
                }
                else if (factura.DocType == BoDocumentTypes.dDocument_Service)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Servicio;
                }

                //if (factura.DocumentStatus == BoStatus.bost_Close)
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

                bool convertido = int.TryParse(factura.UserFields.Fields.Item("U_ModVenFA").Value, out modalidad);

                if (convertido)
                {
                    cfe.ModalidadVentaInt = modalidad;
                }

                #region FE_EXPORTACION
                //cfe.ViaTransporteInt = factura.TransportationCode;
                convertido = false;
                convertido = int.TryParse(factura.UserFields.Fields.Item(Globales.Constantes.UDFViaTransporteFA).Value, out viaTransporte);
                if (convertido)
                {
                    cfe.ViaTransporteInt = viaTransporte;
                }
                #endregion FE_EXPORTACION

                cfe.ClausulaVenta = factura.UserFields.Fields.Item("U_ClaVenFA").Value;

                #endregion EXPORTACION

                #region ADENDA

                cfe.TextoLibreAdenda = adenda;

                #endregion ADENDA

                #region CAE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Validda que el cliente sea de contado
                    if (this.ValidarClienteContado(factura.CardCode))
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.Contingencia)) as CAE;
                    }
                    else
                    {
                        if (ValidarClienteExtranjero(factura.CardCode))
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.Contingencia)) as CAE;
                        }
                        else
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.Contingencia)) as CAE;
                        }
                    }
                }//Si no es contingencia
                else
                {
                    //Valida que el cliente sea de contado
                    if (this.ValidarClienteContado(factura.CardCode))
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ETicket)) as CAE;
                    }
                    else
                    {
                        if (ValidarClienteExtranjero(factura.CardCode))
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFacturaExportacion)) as CAE;
                        }
                        else
                        {
                            cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EFactura)) as CAE;
                        }
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
                    TimeSpan diferenciaDias =  DateTime.Now.Subtract(fechaVencimiento); 
                    int diasFecha = int.Parse(diferenciaDias.Days.ToString());


                    if (diasFecha > 0 && finCae.Dias != 0)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox
                            ("!!!Atención!!! El rango se venció, fecha de vencimiento: " + cae.FechaVencimiento);
                    }
                    else
                    {
                        diasFecha = diasFecha * -1;

                        if (diasFecha <= finCae.Dias && finCae.Dias != 0 )
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
                cfe.OrigenFE = factura.UserFields.Fields.Item("U_Origen").Value;
                #endregion Web Service

                #region Redondeo
                // Si hay redondeo global en la factura, se agrega como ítem con Ind de Facturación 6 si es > 0 o 7 si es < 0 - 16.11.2016
                //Redondeo(factura.RoundingDiffAmount, ref cfe);                
                #endregion Redondeo
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("FacturaB1/Error: " + ex.ToString());
            }
            finally
            {
                if (factura != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(factura);
                    GC.Collect();
                }
            }

            return cfe;
        }

        /// <summary>
        /// Actualiza los datos de CFE en el documento procesado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numFactura"></param>
        /// <param name="serie"></param>
        /// <param name="numCFE"></param>
        public void ActualizarDatosCFEFActura(int numFactura, string serie, string numCFE)
        {
            Documents factura = null;

            Recordset recSet = null;
            string consulta = "";

            try
            {

                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);



                consulta = " Update OINV set U_SerieFA = '" + serie + "' , U_NumeroFA = '" + numCFE + "' where DocEntry = '" + numFactura + "'";
                //Ejecutar consulta
                recSet.DoQuery(consulta);


                //factura = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);
                //factura.GetByKey(numFactura);
                //factura.UserFields.Fields.Item("U_SerieFA").Value = serie;
                //factura.UserFields.Fields.Item("U_NumeroFA").Value = numCFE;                

                //factura.Update();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("FacturaB1/Error: " + ex.ToString());
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
            CFE cfe = new CFE();
            Documents factura = null;

            try
            {
                factura = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);
                factura.GetByKey(numeroDocumento);

                cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(factura, "IVAVEXE");
                cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesPorImpuesto(factura, " IVAEXPO");
                cfe.TotalMontoImpuestoPercibido = 0;
                cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(factura, "IVASUSP");
                cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(factura, "IVAVTM");
                cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(factura, "IVAVTB");
                cfe.TotalMontoNetoIVAOtraTasa = 0;
                cfe.TasaMinimaIVA = this.ObtenerMontoTasa("IVAVTM");
                cfe.TasaBasicaIVA = this.ObtenerMontoTasa("IVAVTB");
                cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(factura.DocEntry.ToString(), "IVAVEXE", "INV1");
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("FacturaB1/Error: " + ex.ToString());
            }
            finally
            {
                if (factura != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(factura);
                    GC.Collect(); 
                }
            }            

            return cfe;
        }
    }
}

