﻿
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
    /// Contiene los metodos para obtener los valores de los remitos
    /// </summary>
    class RemitoB1 : DocumentoB1
    {
        public static double descuentoGeneral = 0;
        public static double descuentoGeneralExtranjero = 0;
        public static double porcentajeDescuento = 0;

        public CFE ObtenerDatosRemito(int numRemito, CAE.ESTipoCFECFC tipoCFE, string adenda, string razonRef)
        {
            Documents remito = null;
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

                remito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

                remito.GetByKey(numRemito);

                #region EMISOR

                emisor = this.ObtenerDatosEmisor() as Emisor;
                cfe.RucEmisor = emisor.Ruc;
                cfe.NombreEmisor = emisor.Nombre;
                cfe.NombreComercialEmisor = emisor.NombreComercial;
                cfe.NumeroResolucion = emisor.NumeroResolucion;
                cfe.CodigoCasaPrincipalEmisor = this.ObtenerSucursal(remito.DocEntry.ToString(), TABLA_REMITO);
                cfe.CajaEmisor = this.ObtenerCaja(remito.DocEntry.ToString(), TABLA_REMITO);
                cfe.DomicilioFiscalEmisor = this.ObtenerDireccionEmisor();
                cfe.CiuidadEmisor = this.ObtenerCiudadEmisor();
                cfe.DocumentoSAP = remito.DocEntry.ToString();
                cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;

                #endregion EMISOR

                #region RECEPTOR

                cfe.TipoDocumentoReceptor = CFE.ObtenerTipoDocumentoReceptor(remito.UserFields.Fields.Item("U_TipDocRM").Value);
                cfe.CodigoPaisReceptor = this.ObtenerCodPaisReceptor(remito.CardCode, cfe.TipoDocumentoReceptor);
                cfe.NombreReceptor = remito.CardName;

                if (cfe.PaisReceptor.Equals("UY"))
                {
                    cfe.NumDocReceptorUruguayo = remito.UserFields.Fields.Item("U_ValDocRM").Value;
                }
                else
                {
                    cfe.NumDocReceptorExtrangero = remito.UserFields.Fields.Item("U_ValDocRM").Value;
                }

                //cfe.CiuidadReceptor = remito.Address.Replace("\r", "");
                cfe.CiuidadReceptor = this.ObtenerCiudad(remito.DocEntry, "DLN12");
                cfe.PaisReceptor = this.ObtenerNombPaisReceptor(remito.CardCode);
                cfe.CorreoReceptor = this.ObtenerCorreoReceptor(remito.CardCode);
                //cfe.DireccionReceptor = cfe.CiuidadReceptor;// +" " + cfe.PaisReceptor;
                cfe.DireccionReceptor = this.ObtenerCiudad(remito.DocEntry, "DLN12");
                cfe.NumeroCompraReceptor = this.ObtenerNroPedidoReceptor(remito.DocEntry, TABLA_REMITO);

                #endregion RECEPTOR

                #region TOTALES ENCABEZADO

                cfe.TipoModena = this.ObtenerCodigoISOModena(remito.DocCurrency);

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
                    cfe.TipoCambio = remito.DocRate;
                }

                //Si el cliente es extrangero entonces el monto de exportacion y asimilados es la suma de las lineas si importar el codigo de impuesto que tenga
                //y el monto total a pagar va ser ese mismo monto. En el xslt en ambos casos se asigna el mismo valor
                if (this.ValidarClienteExtranjero(remito.CardCode))
                {
                    cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesExportacion(remito);
                }
                else
                {
                    cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("1"));/// / cfe.TipoCambio;
                    cfe.TotalMontoNoGravadoExtranjero = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("1"));
                    cfe.TotalMontoImpuestoPercibido = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("11"));// / cfe.TipoCambio;
                    cfe.TotalMontoImpuestoPercibidoExtranjero = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("11"));
                    cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("12"));// / cfe.TipoCambio;
                    cfe.TotalMontoIVASuspensoExtranjero = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("12"));
                    cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("2"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaMinimaExtranjero = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("2"));
                    cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("3"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVATasaBasicaExtranjero = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("3"));
                    cfe.TotalMontoNetoIVAOtraTasa = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("4"));// / cfe.TipoCambio;
                    cfe.TotalMontoNetoIVAOtraTasaExtranjero = this.ObtenerTotalesPorImpuesto(remito, ObtenerCodigoImpuesto("4"));
                    cfe.TasaMinimaIVA = this.ObtenerIvaTasaMinima();
                    cfe.TasaBasicaIVA = this.ObtenerIvaTasaBasica();

                    cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(remito.DocEntry.ToString(), "IVAB", "DLN1");
                }

                cfe.Lineas = remito.Lines.Count;

                #endregion TOTALES ENCABEZADO

                #region ITEMS

                for (int i = 0; i < remito.Lines.Count; i++)
                {
                    remito.Lines.SetCurrentLine(i);


                    //Si el Tipo de lista de Materiales de Ventas no lo agrego al XML 
                    if (!remito.Lines.TreeType.ToString().Equals("iIngredient"))
                    {


                        //Nueva instancia del objeto item de cfe
                        item = cfe.NuevoItem();

                        item.NumeroLinea = i + 1;
                        item.NombreItem = remito.Lines.ItemCode;

                        string indFacturacionNumero = remito.Lines.UserFields.Fields.Item("U_IndRM").Value + "";

                        if (indFacturacionNumero.Equals("6"))
                        {
                            item.IndicadorFacturacion = 6;
                        }
                        if (indFacturacionNumero.Equals("7"))
                        {
                            item.IndicadorFacturacion = 7;
                        }
                        #region FE_EXPORTACION
                        else if (this.ValidarClienteExtranjero(remito.CardCode))
                        {
                            item.IndicadorFacturacion = 10;
                        }
                        #endregion FE_EXPORTACION
                        else
                        {
                            string resultadoIndicadorFacturacion = this.ObtenerIndicadorFacturacion(remito.Lines.TaxCode.ToString());

                            if (resultadoIndicadorFacturacion.Equals(""))
                            {
                                item.IndicadorFacturacion = 0;
                            }
                            else
                            {
                                item.IndicadorFacturacion = Convert.ToInt16(resultadoIndicadorFacturacion);
                            }



                      
                        }
                        item.DescripcionItem = remito.Lines.ItemDescription;
                        item.CantidadItem = remito.Lines.Quantity;
                        item.PrecioUnitarioItem = remito.Lines.Price - (remito.Lines.Price * (remito.DiscountPercent / 100));

                        if (item.PrecioUnitarioItem.ToString().Equals("0"))
                        {
                            item.IndicadorFacturacion = 5;
                        }


                        item.PrecioUnitarioItemPDF = remito.Lines.Price;
                        item.LineNum = remito.Lines.LineNum;
                        item.ArticulosXUnidad = remito.Lines.UnitsOfMeasurment;
                        item.UnidadMedida = this.ObtenerUnidadMedidaItem(remito.Lines.ItemCode, remito.DocEntry + "", remito.Lines.LineNum + "", "DLN1");
                        item.UnidadMedidaPDF = item.UnidadMedida;
                        item.TipoImpuesto = remito.Lines.TaxCode;

                        cfe.AgregarItem(item);

                    }
                }

                #endregion ITEMS

                #region MEDIOS DE PAGO

                mediosPago = cfe.NuevoMediosPago();
                mediosPago.Glosa = this.ObtenerMedioPago(remito.DocEntry.ToString(), TABLA_REMITO);
                cfe.AgregarMediosPago(mediosPago);

                #endregion MEDIOS DE PAGO

                descuentoGeneral = remito.TotalDiscount;
                descuentoGeneralExtranjero = remito.TotalDiscountFC;
                porcentajeDescuento = remito.DiscountPercent;

                #region IDENTIFICACION DEL COMPROBANTE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida si es un cliente extranjero
                    if (this.ValidarClienteExtranjero(remito.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemitoExportacionContingencia));
                    }
                    else
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemitoContingencia));
                    }

                }//Si no es contingencia
                else
                {
                    //Valida si es un cliente extranjero
                    if (this.ValidarClienteExtranjero(remito.CardCode))
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemitoExportacion));
                        rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemitoExportacion), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                    }
                    else
                    {
                        cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemito));
                        rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemito), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
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

                cfe.FechaComprobante = remito.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = remito.DocDueDate.ToString("yyyy-MM-dd");

                if (remito.DocType == BoDocumentTypes.dDocument_Items)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Articulo;
                }
                else if (remito.DocType == BoDocumentTypes.dDocument_Service)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Servicio;
                }

                if (remito.DocumentStatus == BoStatus.bost_Close)
                {
                    cfe.FormaPago = CFE.ESFormaPago.Contado;
                }
                else
                {
                    cfe.FormaPago = CFE.ESFormaPago.Credito;
                }

                #endregion IDENTIFICACION DEL COMPROBANTE

                #region EXPORTACION

                int modalidad, viaTransporte, bienes;

                bool convertido = int.TryParse(remito.UserFields.Fields.Item("U_ModVenRM").Value, out modalidad);

                if (convertido)
                {
                    cfe.ModalidadVentaInt = modalidad;
                }

                #region FE_EXPORTACION
                //cfe.ViaTransporteInt = remito.TransportationCode;
                convertido = false;
                convertido = int.TryParse(remito.UserFields.Fields.Item(Globales.Constantes.UDFViaTransporteRM).Value, out viaTransporte);
                if (convertido)
                {
                    cfe.ViaTransporteInt = viaTransporte;
                }

                convertido = false;
                convertido = int.TryParse(remito.UserFields.Fields.Item(Globales.Constantes.UDFIndTipoDeBienes).Value, out bienes);
                if (convertido)
                {
                    cfe.TipoTrasladoBienesInt = bienes;
                }
                #endregion FE_EXPORTACION

                cfe.ClausulaVenta = remito.UserFields.Fields.Item("U_ClaVenRM").Value;


                #endregion EXPORTACION

                #region ADENDA

                cfe.TextoLibreAdenda = adenda;
                ////cfe.TextoLibreAdenda = remito.UserFields.Fields.Item("U_AdendaRM").Value;

                #endregion ADENDA

                #region DOCUMENTO DE REFENCIA

                //cfeReferencia = this.ObtenerDocumentoReferencia(CFE.ESTipoCFECFC.ERemito, remito.DocEntry.ToString()) as CFE;
                //CFEInfoReferencia cfeInfoReferencia = cfe.NuevoInfoReferencia();

                //if (cfeReferencia != null)
                //{
                //    cfeInfoReferencia.NumeroLinea = 1;
                //    cfeInfoReferencia.SerieComprobanteReferencia = cfeReferencia.SerieComprobante;
                //    cfeInfoReferencia.NumeroComprobanteReferencia = cfeReferencia.NumeroComprobante;
                //    cfe.AgregarInfoReferencia(cfeInfoReferencia);
                //}
                cfeReferencia = (CFEInfoReferencia)this.ObtenerDocumentoReferencia(TABLA_DETALLES_REMITO, remito.DocEntry.ToString());

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

                    if (razonRef.Equals(""))
                    {

                        cfeReferencia.RazonReferencia = "Descuento General";
                    }
                    else
                    {
                        cfeReferencia.RazonReferencia = razonRef;
                    }
                }

                if (cfeReferencia != null)
                {
                    cfeReferencia.NumeroLinea = 1;
                    cfe.AgregarInfoReferencia(cfeReferencia);
                }

                #endregion  DOCUMENTO DE REFENCIA

                #region CAE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Valida si es un cliente extranjero
                    if (this.ValidarClienteExtranjero(remito.CardCode))
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
                    //Valida si es un cliente extranjero
                    if (this.ValidarClienteExtranjero(remito.CardCode))
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemitoExportacion)) as CAE;
                    }
                    else
                    {
                        cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.ERemito)) as CAE;
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

                    if ((cae.NumeroHasta - rango.NumeroActual) <= finCae.Cantidad)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox
                            ("!!!Atención!!!  Rango utilizado: " + rango.NumeroActual + " de: " + cae.NumeroHasta +
                            ". Solicitar Nuevos CAEs a DGI.");
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
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RemitoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (remito != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(remito);
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
        public void ActualizarDatosCFERemito(int numNotaDebito, string serie, string numCFE)
        {
            Documents notaDebito = null;

            try
            {
                notaDebito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

                notaDebito.GetByKey(numNotaDebito);

                notaDebito.UserFields.Fields.Item("U_SerieRM").Value = serie;
                notaDebito.UserFields.Fields.Item("U_NumeroRM").Value = numCFE;

                notaDebito.Update();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RemitoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (notaDebito != null)
                {
                    //Libera de memoria el objeto notaDebito
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
            Documents remito = null;
            CFE cfe = new CFE();

            try
            {
                remito = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                remito.GetByKey(numeroDocumento);

                cfe.TotalMontoNoGravado = this.ObtenerTotalesPorImpuesto(remito, "IVAVEXE");
                cfe.TotalMontoExportacionAsimilados = this.ObtenerTotalesPorImpuesto(remito, " IVAEXPO");
                cfe.TotalMontoImpuestoPercibido = 0;
                cfe.TotalMontoIVASuspenso = this.ObtenerTotalesPorImpuesto(remito, "IVASUSP");
                cfe.TotalMontoNetoIVATasaMinima = this.ObtenerTotalesPorImpuesto(remito, "IVAVTM");
                cfe.TotalMontoNetoIVATasaBasica = this.ObtenerTotalesPorImpuesto(remito, "IVAVTB");
                cfe.TotalMontoNetoIVAOtraTasa = 0;
                cfe.TasaMinimaIVA = this.ObtenerMontoTasa("IVAVTM");
                cfe.TasaBasicaIVA = this.ObtenerMontoTasa("IVAVTB");
                cfe.TotalIVAOtraTasa = this.ObtenerTotalesIVAPorTasa(remito.DocEntry.ToString(), "IVAVEXE", "DNL1");
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("RemitoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (remito != null)
                {
                    //Libera de memoria el objeto remito
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(remito);
                    GC.Collect();
                }
            }

            return cfe;
        }
    }
}
