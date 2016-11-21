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
    /// Contiene los metodos para obtener los valores de los resguardos
    /// </summary>
    class ResguardoB1 : DocumentoB1
    {
        public static double descuentoGeneral = 0;
        public static double descuentoGeneralExtranjero = 0;

        public CFE ObtenerDatosResguardo(int numResguardo, CAE.ESTipoCFECFC tipoCFE)
        {
            Documents resguardo = null;
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

                resguardo = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);

                resguardo.GetByKey(numResguardo);

                #region EMISOR

                emisor = this.ObtenerDatosEmisor() as Emisor;
                cfe.RucEmisor = emisor.Ruc;
                cfe.NombreEmisor = emisor.Nombre;
                cfe.NombreComercialEmisor = emisor.NombreComercial;
                cfe.NumeroResolucion = emisor.NumeroResolucion;
                cfe.CodigoCasaPrincipalEmisor = this.ObtenerSucursal(resguardo.DocEntry.ToString(), TABLA_RESGUARDO);
                cfe.CajaEmisor = this.ObtenerCaja(resguardo.DocEntry.ToString(), TABLA_RESGUARDO);
                cfe.DomicilioFiscalEmisor = this.ObtenerDireccionEmisor();
                cfe.CiuidadEmisor = this.ObtenerCiudadEmisor();
                cfe.DocumentoSAP = resguardo.DocEntry.ToString();
                cfe.EstadoDGI = CFE.ESEstadoCFE.PendienteDGI;
                cfe.EstadoReceptor = CFE.ESEstadoCFE.PendienteReceptor;

                #endregion EMISOR

                #region RECEPTOR

                cfe.TipoDocumentoReceptor = CFE.ObtenerTipoDocumentoReceptor(resguardo.UserFields.Fields.Item("U_TipDocFA").Value);
                cfe.CodigoPaisReceptor = this.ObtenerCodPaisReceptor(resguardo.CardCode, cfe.TipoDocumentoReceptor);
                cfe.NombreReceptor = resguardo.CardName;

                if (cfe.PaisReceptor.Equals("UY"))
                {
                    cfe.NumDocReceptorUruguayo = resguardo.UserFields.Fields.Item("U_ValDocFA").Value;
                }
                else
                {
                    cfe.NumDocReceptorExtrangero = resguardo.UserFields.Fields.Item("U_ValDocFA").Value;
                }

                //cfe.CiuidadReceptor = resguardo.Address.Replace("\r", "");
                cfe.CiuidadReceptor = this.ObtenerCiudad(resguardo.DocEntry, "INV12");
                cfe.PaisReceptor = this.ObtenerNombPaisReceptor(resguardo.CardCode);
                cfe.CorreoReceptor = this.ObtenerCorreoReceptor(resguardo.CardCode);
                //cfe.DireccionReceptor = cfe.CiuidadReceptor;// +" " + cfe.PaisReceptor;
                cfe.DireccionReceptor = this.ObtenerCiudad(resguardo.DocEntry, "INV12");
                cfe.NumeroCompraReceptor = this.ObtenerNroPedidoReceptor(resguardo.DocEntry, TABLA_RESGUARDO);

                #endregion RECEPTOR

                #region TOTALES ENCABEZADO

                cfe.TipoModena = this.ObtenerCodigoISOModena(resguardo.DocCurrency);

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
                    cfe.TipoCambio = resguardo.DocRate;
                }

                cfe.Lineas = resguardo.Lines.Count;

                //CFERetencPercep retPer = ObtenerRetencionPercepcion(resguardo.DocEntry.ToString(), "INV5") as CFERetencPercep;

                //cfe.AgregarRetencPercep(retPer);


                List<CFERetencPercep> listretPer = ObtenerRetencionPercepcion(resguardo.DocEntry.ToString(), "INV5", resguardo.DocCurrency.ToString()) as List<CFERetencPercep>;

                // SE CARGA EN VLORES de reneciones
                for (int i = 0; i < listretPer.Count; i++)
                {                             
                    cfe.AgregarRetencPercep(listretPer[i]);
                }                                       
                             

                #endregion TOTALES ENCABEZADO

                #region ITEMS

                List<CFEItemsRetencPercep> listaItemRecPerc = this.ObtenerItemRetencionPercepcion(resguardo.DocEntry.ToString(), "INV5", resguardo.DocCurrency.ToString()) as List<CFEItemsRetencPercep>;

                for (int i = 0; i < listaItemRecPerc.Count; i++)
                {
                    resguardo.Lines.SetCurrentLine(i);

                    //Nueva instancia del objeto item de cfe
                    item = cfe.NuevoItem();

                    item.AgregarItemRetencPercep(listaItemRecPerc[i]);

                    item.NumeroLinea = i + 1;

                    item.IndicadorFacturacion = 4;

                    cfe.AgregarItem(item);
                }

                cfe.Lineas = listaItemRecPerc.Count;

                #endregion ITEMS

                #region MEDIOS DE PAGO

                mediosPago = cfe.NuevoMediosPago();
                mediosPago.Glosa = this.ObtenerMedioPago(resguardo.DocEntry.ToString(), TABLA_RESGUARDO);
                cfe.AgregarMediosPago(mediosPago);

                #endregion MEDIOS DE PAGO
                cfe.FechaComprobante = resguardo.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = resguardo.DocDueDate.ToString("yyyy-MM-dd");

                descuentoGeneral = resguardo.TotalDiscount;
                descuentoGeneralExtranjero = resguardo.TotalDiscountFC;

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
                    SAPbouiCOM.Item txtNumCFE = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtNumCFE");
                    SAPbouiCOM.Item txtSeCFE = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtSeCFE");

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

                cfe.FechaComprobante = resguardo.DocDate.ToString("yyyy-MM-dd");
                cfe.FechaVencimiento = resguardo.DocDueDate.ToString("yyyy-MM-dd");

                if (resguardo.DocType == BoDocumentTypes.dDocument_Items)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Articulo;
                }
                else if (resguardo.DocType == BoDocumentTypes.dDocument_Service)
                {
                    cfe.TipoDocumentoSAP = CFE.ESTIpoDocumentoSAP.Servicio;
                }

                if (resguardo.DocumentStatus == BoStatus.bost_Close)
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

                List<CFEInfoReferencia> listaRef = this.ObtenerReferenciaResguardo(resguardo.DocEntry.ToString(), TABLA_RESGUARDO) as List<CFEInfoReferencia>;

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
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ResguardoB1/Error: " + ex.ToString());
            }
            finally
            {
                if(resguardo != null)
                {
                    //Libera de memoria el objeto resguardo
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resguardo);
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
        public void ActualizarDatosCFEResguardo(int numResguardo, string serie, string numCFE)
        {
            Documents resguardo = null;

            try
            {
                resguardo = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.oInvoices);
                resguardo.GetByKey(numResguardo);
                resguardo.UserFields.Fields.Item("U_SerieRG").Value = serie;
                resguardo.UserFields.Fields.Item("U_NumeroRG").Value = numCFE;
                resguardo.Update();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ResguardoB1/Error: " + ex.ToString());
            }
            finally
            {
                if (resguardo != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resguardo);
                    GC.Collect();
                }
            }
        }

    }
}
