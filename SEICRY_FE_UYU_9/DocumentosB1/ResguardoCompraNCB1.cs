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
using SAPbouiCOM;

namespace SEICRY_FE_UYU_9.DocumentosB1
{
    /// <summary>
    /// Contiene los metodos para obtener los valores de los resguardos
    /// </summary>
    class ResguardoCompraNCB1 : DocumentoB1
    {
        public static double descuentoGeneral = 0;
        public static double descuentoGeneralExtranjero = 0;

        public CFE ObtenerDatosResguardo(int numResguardo, CAE.ESTipoCFECFC tipoCFE)
        {
            Documents resguardoCompras = null;
            CFE cfe = new CFE();
            CFEItems item;
            Emisor emisor;
            Rango rango = null;
            CFEMediosPago mediosPago;
            CAE cae;
            descuentoGeneral = 0;
            descuentoGeneralExtranjero = 0;

            try
            {
                resguardoCompras = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);

                resguardoCompras.GetByKey(numResguardo);

                #region EMISOR

                emisor = this.ObtenerDatosEmisor() as Emisor;
                cfe.RucEmisor = emisor.Ruc;
                cfe.NombreEmisor = emisor.Nombre;
                cfe.NombreComercialEmisor = emisor.NombreComercial;
                cfe.NumeroResolucion = emisor.NumeroResolucion;
                cfe.CodigoCasaPrincipalEmisor = this.ObtenerSucursal(resguardoCompras.DocEntry.ToString(), TABLA_RESGUARDO_PROVEEDOR);
                cfe.CajaEmisor = this.ObtenerCaja(resguardoCompras.DocEntry.ToString(), TABLA_RESGUARDO_PROVEEDOR);
                cfe.DomicilioFiscalEmisor = this.ObtenerDireccionEmisor();
                cfe.CiuidadEmisor = this.ObtenerCiudadEmisor();
                cfe.DocumentoSAP = resguardoCompras.DocEntry.ToString();
                cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;

                #endregion EMISOR

                #region RECEPTOR

                cfe.TipoDocumentoReceptor = CFE.ObtenerTipoDocumentoReceptor(resguardoCompras.UserFields.Fields.Item("U_TipDocNC").Value);
                cfe.CodigoPaisReceptor = this.ObtenerCodPaisReceptor(resguardoCompras.CardCode, cfe.TipoDocumentoReceptor);
                cfe.NombreReceptor = resguardoCompras.CardName;

                if (cfe.PaisReceptor.Equals("UY"))
                {
                    cfe.NumDocReceptorUruguayo = resguardoCompras.UserFields.Fields.Item("U_ValDocNC").Value;
                }
                else
                {
                    cfe.NumDocReceptorExtrangero = resguardoCompras.UserFields.Fields.Item("U_ValDocNC").Value;
                }

                //cfe.CiuidadReceptor = resguardoCompras.Address.Replace("\r", "");
                cfe.CiuidadReceptor = this.ObtenerCiudad(resguardoCompras.DocEntry, "RPC12");
                cfe.PaisReceptor = this.ObtenerNombPaisReceptor(resguardoCompras.CardCode);
                cfe.CorreoReceptor = this.ObtenerCorreoReceptor(resguardoCompras.CardCode);
                //cfe.DireccionReceptor = cfe.CiuidadReceptor;// +" " + cfe.PaisReceptor;
                cfe.DireccionReceptor = this.ObtenerCiudad(resguardoCompras.DocEntry, "RPC12");
                cfe.NumeroCompraReceptor = this.ObtenerNroPedidoReceptor(resguardoCompras.DocEntry, TABLA_RESGUARDO_PROVEEDOR);

                #endregion RECEPTOR

                #region TOTALES ENCABEZADO

                cfe.TipoModena = this.ObtenerCodigoISOModena(resguardoCompras.DocCurrency);

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
                    cfe.TipoCambio = resguardoCompras.DocRate;
                }

                cfe.Lineas = resguardoCompras.Lines.Count;

                List<CFERetencPercep> listretPer = ObtenerRetencionPercepcion(resguardoCompras.DocEntry.ToString(), "RPC5", resguardoCompras.DocCurrency.ToString()) as List<CFERetencPercep>;

                // SE CARGA EN VLORES NEGATIVOS de retencion.

             for (int i = 0; i < listretPer.Count; i++)
                {
                    //Nueva instancia del objeto item de cfe
                    CFERetencPercep Perepcion = new CFERetencPercep();

                    Perepcion =  listretPer[i];

                    Perepcion.ValorRetencPercep = Perepcion.ValorRetencPercep * -1;

                    cfe.AgregarRetencPercep(Perepcion);
                }                                       
                             

                #endregion TOTALES ENCABEZADO

                #region ITEMS

                List<CFEItemsRetencPercep> listaItemRecPerc = this.ObtenerItemRetencionPercepcion(resguardoCompras.DocEntry.ToString(), "RPC5", resguardoCompras.DocCurrency.ToString()) as List<CFEItemsRetencPercep>;

                for (int i = 0; i < listaItemRecPerc.Count; i++)
                {
                    CFEItemsRetencPercep ItemRetencion = new CFEItemsRetencPercep();

                    ItemRetencion = listaItemRecPerc[i];

                   //ItemRetencion.ValorRetencPercep = ItemRetencion.ValorRetencPercep * -1;
                  //ItemRetencion.MontoSujetRetencPercep = ItemRetencion.MontoSujetRetencPercep * -1;
                    

                    //Nueva instancia del objeto item de cfe
                    item = cfe.NuevoItem();

                    item.AgregarItemRetencPercep(ItemRetencion);

                    item.IndicadorFacturacion = 9;

                    item.NumeroLinea = i + 1;

                    cfe.AgregarItem(item);
                }

                descuentoGeneral = resguardoCompras.TotalDiscount;
                descuentoGeneralExtranjero = resguardoCompras.TotalDiscountFC;


                cfe.Lineas = listaItemRecPerc.Count;
                
                #endregion ITEMS

                #region MEDIOS DE PAGO

                mediosPago = cfe.NuevoMediosPago();
                mediosPago.Glosa = this.ObtenerMedioPago(resguardoCompras.DocEntry.ToString(), TABLA_RESGUARDO_PROVEEDOR);
                cfe.AgregarMediosPago(mediosPago);

                #endregion MEDIOS DE PAGO

                descuentoGeneral = resguardoCompras.TotalDiscount;
                descuentoGeneralExtranjero = resguardoCompras.TotalDiscountFC;

                #region IDENTIFICACION DEL COMPROBANTE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EResguardoContingencia));
                }
                //Si no es contingencia
                else
                {
                    cfe.TipoCFE = CFE.ObtenerTipoCFECFC(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EResguardo));
                    rango = this.ObtenerDatosRangoCFE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EResguardo), cfe.CodigoCasaPrincipalEmisor.ToString(), cfe.CajaEmisor) as Rango;
                }

                //Estado de contigencia serie y numero se ingresan manualmente
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    Item txtNumCFE = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtNumCFE");
                    Item txtSeCFE = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtSeCFE");

                    cfe.SerieComprobante = ((SAPbouiCOM.EditText)txtSeCFE.Specific).Value + "";
                    cfe.NumeroComprobante = int.Parse((((SAPbouiCOM.EditText)txtNumCFE.Specific).Value + "")) + 1;
                }
                else
                {
                    if (rango != null)
                    {
                        cfe.SerieComprobante = rango.Serie;
                        cfe.NumeroComprobante = rango.NumeroActual;
                    }
                }

                cfe.FechaComprobante = resguardoCompras.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = resguardoCompras.DocDueDate.ToString("yyyy-MM-dd");

                if (resguardoCompras.DocType == BoDocumentTypes.dDocument_Items)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Articulo;
                }
                else if (resguardoCompras.DocType == BoDocumentTypes.dDocument_Service)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Servicio;
                }

                if (resguardoCompras.DocumentStatus == BoStatus.bost_Close)
                {
                    cfe.FormaPago = CFE.ESFormaPago.Contado;
                }
                else
                {
                    cfe.FormaPago = CFE.ESFormaPago.Credito;
                }

                #endregion IDENTIFICACION DEL COMPROBANTE

                #region ADENDA

                //ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();
                //Adenda adenda = manteUdoAdenda.ObtenerAdenda(ProcConexion.Comp, Adenda.ESTipoObjetoAsignado.TipoCFE111, resguardo.DocNum.ToString());

                //if (tipoCFE == CAE.ESTipoCFECFC.EResguardo)
                //{
                //    resguardo.UserFields.Fields.Item("U_AdendaRG").Value = adenda.CadenaAdenda;// resguardo.UserFields.Fields.Item("U_AdendaFA").Value;
                //    resguardo.Update();
                //}

                //cfe.TextoLibreAdenda = adenda.CadenaAdenda;
                //cfe.TextoLibreAdenda = resguardo.UserFields.Fields.Item("U_AdendaFA").Value;

                #endregion ADENDA

                #region DOCUMENTOS DE REFENCIA

                List<CFEInfoReferencia> listaRef = this.ObtenerReferenciaResguardo(resguardoCompras.DocEntry.ToString(), TABLA_RESGUARDO_PROVEEDOR) as List<CFEInfoReferencia>;

                for (int i = 0; i < listaRef.Count; i++)
                {
                    cfe.AgregarInfoReferencia(listaRef[i]);
                }

                #endregion DOCUMENTOS DE REFENCIA

                #region CAE

                //Valida que el documento sea de contingencia
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.Contingencia)) as CAE;

                }//Si no es contingencia
                else
                {
                    cae = this.ObtenerDatosCAE(CAE.ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC.EResguardo)) as CAE;

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
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ResguardoCompra/Error: " + ex.ToString());
            }
            finally
            {
                if (resguardoCompras != null)
                {
                    //Libera de memoria el objeto resguardoCompras
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resguardoCompras);
                    GC.Collect();
                }
            }

            return cfe;
        }

        /// <summary>
        /// Actualiza los datos de CFE en el documento procesado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numResguardo"></param>
        /// <param name="serie"></param>
        /// <param name="numCFE"></param>
        public void ActualizarDatosCFEResguardo(int numResguardo, string serie, string numCFE )
        {
            Documents resguardo = null;

            try
            {               
                    resguardo = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes );

                    resguardo.GetByKey(numResguardo);

                resguardo.UserFields.Fields.Item("U_SerieRG").Value = serie;
                resguardo.UserFields.Fields.Item("U_NumeroRG").Value = numCFE;

                resguardo.Update();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ResguardoCompra/Error: " + ex.ToString());
            }
            finally
            {
                if (resguardo != null)
                {
                    //Libera de memoria al objeto resguardo
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resguardo);
                    GC.Collect();
                }
            }
        }

    }
}
