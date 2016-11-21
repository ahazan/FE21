using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.DocumentosB1;
using SEICRY_FE_UYU_9.Udos;

namespace SEICRY_FE_UYU_9.ZonasCFE
{
    class CuerpoComprobante
    {

        private EmpresaDatos EmpresaDato = new EmpresaDatos();


        /// <summary>
        /// Nueva linea
        /// </summary>
        Chunk nL = Chunk.NEWLINE;
        /// <summary>
        /// Linea horizontal
        /// </summary>
        Chunk horiz = new Chunk(new VerticalPositionMark());

        /// <summary>
        /// Genera la zona del emisor
        /// </summary>
        /// <param name="pInfoComprobante"></param>
        /// <param name="domicilioFiscal"></param>
        /// <param name="factura"></param>
        /// <param name="docNum"></param>
        /// <returns></returns>
        public PdfPTable Emisor(CFE pInfoComprobante
            , string domicilioFiscal, Document factura, string docNum)
        {
            PdfPTable info = new PdfPTable(new float[] { 2f, 2f });

            info = CeldaNueva("RUT Emisor: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            info = CeldaNueva(pInfoComprobante.RucEmisor.ToString(), info, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            info = CeldaNueva("Tipo CFE:", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            if (pInfoComprobante.TipoCFEInt == 102 || pInfoComprobante.TipoCFEInt == 112)
            {
                info = CeldaNueva(pInfoComprobante.TipoCFE.ToString(), info, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                info = CeldaNueva(pInfoComprobante.TipoCFE.ToString(), info, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            info = CeldaNueva("Serie/Número: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            info = CeldaNueva(pInfoComprobante.SerieComprobante + "-" + pInfoComprobante.NumeroComprobante,
                info, Alineacion.Derecha, 0, TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            info = CeldaNueva("Forma Pago: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            info = CeldaNueva(pInfoComprobante.FormaPago.ToString(), info, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            info = CeldaNueva("Nro. Interno: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            info = CeldaNueva(docNum, info, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            DateTime fechaFormato = new DateTime();
            fechaFormato = Convert.ToDateTime(pInfoComprobante.FechaComprobante);

            info = CeldaNueva("Fecha de Emisión: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            info = CeldaNueva(fechaFormato.ToString("dd/MM/yyyy"), info, Alineacion.Izquierda, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            return info;
        }

        /// <summary>
        /// Genera la zona del receptor fiscal
        /// </summary>
        /// <param name="comprobante"></param>
        /// <param name="factura"></param>
        /// <param name="docCreado"></param>
        /// <param name="ticket"></param>
        /// <param name="nombreExtranjero"></param>
        /// <param name="codigoSocio"></param>
        /// <returns></returns>
        public Document Receptor(CFE comprobante, Document factura, PdfWriter docCreado, bool ticket,
            DatosPDF datosPdf)
        {
            PdfPTable receptorInfo = new PdfPTable(new float[] { 2.5f, 0.50f, 1.25f, 2.75f });
            receptorInfo.WidthPercentage = 100f;


            ObtenerEmpresaDatos(EmpresaDato);

            factura.Add(nL);
            if (ticket)
            {
                //receptorInfo = CeldaNueva(EmpresaDato.Nombre, receptorInfo, Alineacion.Centrado, 0,
                //    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("RACELY S.A", receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(Mensaje.pdfCONSUMOFINAL, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(comprobante.NumDocReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerechaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                //receptorInfo = CeldaNueva(EmpresaDato.NombreComercial, receptorInfo, Alineacion.Centrado, 0,
                //    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Secocenter", receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Cliente:",receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(datosPdf.SocioNegocio + " - " +datosPdf.NombreExtranjero, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                //receptorInfo = CeldaNueva(EmpresaDato.Web, receptorInfo, Alineacion.Centrado, 0,
                //    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Gral Flores 3196 - Montevideo", receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("Nombre Fantasía: ", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva(comprobante.NombreReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                
                receptorInfo = CeldaNueva("Tel:22045858", receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                //receptorInfo = CeldaNueva(EmpresaDato.Direccion, receptorInfo, Alineacion.Centrado, 0,
                //    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Dirección:", receptorInfo, Alineacion.Izquierda, 0,
                TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(comprobante.DireccionReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                //receptorInfo = CeldaNueva(EmpresaDato.Phone, receptorInfo, Alineacion.Centrado, 0,
                //    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("info@seocenter.com", receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Teléfono:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(datosPdf.Telefono, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                //receptorInfo = CeldaNueva(EmpresaDato.E_Mail, receptorInfo, Alineacion.Centrado, 0,
                //    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("www.secocenter.com", receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Condiciones Pago:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaUltima, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(datosPdf.FormaPago, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerechaUltima, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);                
            }
            else
            {
                receptorInfo = CeldaNueva(EmpresaDato.Nombre, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                
                #region FE_EXPORTACION
                /*receptorInfo = CeldaNueva(Mensaje.pdfRUCCOMPRADOR, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);*/                
                receptorInfo = CeldaNueva(comprobante.TipoDocumentoReceptor.ToString(), receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                #endregion FE_EXPORTACION

                if (comprobante.NumDocReceptor.Equals("00000000"))
                {
                    ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                    string temp = manteUdoAdobe.ObtenerCiGenerico();
                    if (!temp.Equals(""))
                    {
                        comprobante.NumDocReceptorExtrangero = temp;
                    }
                }


                receptorInfo = CeldaNueva(comprobante.NumDocReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerechaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                receptorInfo = CeldaNueva(EmpresaDato.NombreComercial, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Cliente: ", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(datosPdf.SocioNegocio + " - " +datosPdf.NombreExtranjero, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                receptorInfo = CeldaNueva(EmpresaDato.Direccion, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Nombre Fantasía: ", receptorInfo, Alineacion.Izquierda, 0,
                TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(comprobante.NombreReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                receptorInfo = CeldaNueva(EmpresaDato.Phone, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Dirección: ", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(comprobante.DireccionReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                receptorInfo = CeldaNueva(EmpresaDato.E_Mail, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Teléfono:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(datosPdf.Telefono, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                receptorInfo = CeldaNueva(EmpresaDato.Web, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva("Condiciones de Pago:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaUltima, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                receptorInfo = CeldaNueva(datosPdf.FormaPago, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerechaUltima, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            PdfPTable recuadro = new PdfPTable(new float[] { 1 });
            recuadro.WidthPercentage = 100f;

            PdfPCell celda = new PdfPCell(receptorInfo);
            celda.Border = PdfPCell.NO_BORDER;
            recuadro.AddCell(celda);
            recuadro.SpacingAfter = 10;
            factura.Add(recuadro);

            return factura;
        }




        /// <summary>
        /// Genera el detalle del resguardo de compras
        /// </summary>
        /// <param name="resguardo"></param>
        /// <param name="listaResguardos"></param>
        /// <param name="fechaComprobante"></param>
        /// <returns></returns>
        public Document DetalleResguardo(Document resguardo, List<ResguardoPdf> listaResguardos, string fechaComprobante)
        {
            int cantidadMaximaArticulos = 10, resguardosRegistrados = 0;
            double sumaRetenciones = 0;

            PdfPTable tablaResguardo = new PdfPTable(new float[] { 1f, 1f, 2f, 2f, 0.3f, 1.2f });
            tablaResguardo.WidthPercentage = 100f;

            tablaResguardo = CeldaNueva("FACTURA", tablaResguardo, Alineacion.Centrado, 2, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            tablaResguardo = CeldaNueva("MONTOIMPONIBLE", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 2,
                TipoLetra.Bold, AlineacionVertical.Centrado, ColorC.Negro, ColorC.Blanco);
            tablaResguardo = CeldaNueva("IMPUESTO", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 2,
                TipoLetra.Bold, AlineacionVertical.Centrado, ColorC.Negro, ColorC.Blanco);
            tablaResguardo = CeldaNueva("RETENCION", tablaResguardo, Alineacion.Centrado, 2, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaResguardo = CeldaNueva("NÚMERO", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaResguardo = CeldaNueva("FECHA", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaResguardo = CeldaNueva("%", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaResguardo = CeldaNueva("IMPORTE", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);

            int cont = 0;
            string StrFecha = "";

            foreach (ResguardoPdf facturaResguardo in listaResguardos)
            {
                sumaRetenciones += double.Parse(facturaResguardo.ImporteRetencion);

                tablaResguardo = CeldaNueva(facturaResguardo.NumeroFactura, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.LateralIzquierda, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);



                if (cont == 0)
                {
                    DateTime fechaFormato = new DateTime();
                    fechaFormato = new DateTime();
                    fechaFormato = Convert.ToDateTime(facturaResguardo.FechaFactura);
                    StrFecha = fechaFormato.ToString("dd/MM/yyyy");

                }

                facturaResguardo.FechaFactura = StrFecha;

                tablaResguardo = CeldaNueva(facturaResguardo.FechaFactura, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                tablaResguardo = CeldaNueva(facturaResguardo.MontoImponible, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                tablaResguardo = CeldaNueva(facturaResguardo.Impuesto, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                tablaResguardo = CeldaNueva(facturaResguardo.PorcentajeRetencion, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                tablaResguardo = CeldaNueva(facturaResguardo.ImporteRetencion, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                cont++;
                resguardosRegistrados++;
            }

            while (resguardosRegistrados < cantidadMaximaArticulos)
            {
                if (resguardosRegistrados == (cantidadMaximaArticulos - 1))
                {
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.LateralIzquierdaUltima,
                        9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 4, TipoBordes.UltimaFila,
                        9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima,
                        9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0,
                        TipoBordes.LateralIzquierda, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 4,
                        TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0,
                        TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                resguardosRegistrados++;
            }

            tablaResguardo.SpacingAfter = 10;


            tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 6, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 3, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            tablaResguardo = CeldaNueva("TOTAL ", tablaResguardo, Alineacion.Derecha, 0, TipoBordes.Vacia, 10,
                0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            tablaResguardo = CeldaNueva(sumaRetenciones.ToString(), tablaResguardo, Alineacion.Centrado, 2,
                TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            resguardo.Add(tablaResguardo);

            return resguardo;
        }


        /// <summary>
        /// Crea el detalle de los articulos o servicios
        /// </summary>
        /// <param name="comprobante"></param>
        /// <param name="tipoDetalle"></param>
        /// <param name="kilosFactura"></param>
        /// <param name="nombreTabla"></param>
        /// <param name="titular"></param>
        /// <param name="nombreVendedor"></param>
        /// <param name="infoComprobante"></param>
        /// <returns></returns>
        public Document DetalleMercaderia(Document comprobante, int tipoDetalle, string kilosFactura,
            string nombreTabla, DatosPDF datosPdf, CFE infoComprobante, string nombreTablaCabezal)
        {
            int cantidadMaximaArticulos = 26, articulosRegistrado = 0, resultado = 0;

            //if (infoComprobante.Items.Count > 37)
            //{
            //    cantidadMaximaArticulos = infoComprobante.Items.Count;
            //}

            //PdfPTable mercaderia = new PdfPTable(new float[] { 0.80f, 0.20f, 1f, 0.5f, 0.5f, 0.25f, 0.75f, 0.50f,
            //0.75f, 1f, 0.75f, 1f, 1f });

            PdfPTable mercaderia = new PdfPTable(new float[] { 1f, 3.5f, 0.75f, 1f, 0.75f, 1f, 1f });

            mercaderia.WidthPercentage = 100f;
            mercaderia.SpacingAfter = 0;

            if (tipoDetalle == 0)
            {
                mercaderia = CeldaNueva("Código", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                    TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                mercaderia = CeldaNueva("Descripción", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10,
                    0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                mercaderia = CeldaNueva(Mensaje.pdfCantidad, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                mercaderia = CeldaNueva(Mensaje.pdfPrecioUnitario, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                mercaderia = CeldaNueva("% Desc", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                    TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                mercaderia = CeldaNueva(Mensaje.pdfPrecioAnDesc, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                mercaderia = CeldaNueva(Mensaje.pdfPrecioFinal, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);

                foreach (CFEItems lineaProducto in infoComprobante.Items)
                {

                    string codigoPack = ObtenerPackCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                    if (codigoPack != "I")
                    {
                        if (articulosRegistrado == 37)
                        {

                            PdfPCell relleno = new PdfPCell();
                            relleno.FixedHeight = 15;
                            relleno.Border = PdfCell.NO_BORDER;
                            relleno.Colspan = 7;

                            mercaderia.AddCell(relleno);

                            mercaderia = CeldaNueva("Código", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                            TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                            mercaderia = CeldaNueva("Descripción", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10,
                                0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                            mercaderia = CeldaNueva(Mensaje.pdfCantidad, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                                10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                            mercaderia = CeldaNueva(Mensaje.pdfPrecioUnitario, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                                10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                            mercaderia = CeldaNueva("% Desc", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                                TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                            mercaderia = CeldaNueva(Mensaje.pdfPrecioAnDesc, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                                10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                            mercaderia = CeldaNueva(Mensaje.pdfPrecioFinal, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                                10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
                        }


                        //if (articulosRegistrado == (infoComprobante.Items.Count - 1) && articulosRegistrado == 37)
                        //{
                        if ((articulosRegistrado == (infoComprobante.Items.Count - 1) && articulosRegistrado >= 26) || articulosRegistrado == 36)
                        {
                            string codigo = ObtenerCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                            mercaderia = CeldaNueva(codigo, mercaderia, Alineacion.Derecha, 0, TipoBordes.LateralIzquierdaUltima, 10, 0,
                                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                            if (lineaProducto.DescripcionItem.Length < 40)
                            {
                                mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 0,
                                    TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                            }
                            else
                            {
                                mercaderia = CeldaNueva(lineaProducto.DescripcionItem.Substring(0, 40), mercaderia, Alineacion.Izquierda, 0,
                                    TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                            }

                            mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.CantidadItem.ToString()), mercaderia,
                            Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                            if (infoComprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
                            {
                                mercaderia = CeldaNueva("", mercaderia, Alineacion.Derecha, 0,
                                     TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                mercaderia = CeldaNueva("", mercaderia, Alineacion.Derecha, 0,
                                     TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                mercaderia = CeldaNueva("", mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                mercaderia = CeldaNueva("", mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                articulosRegistrado++;
                            }
                            else
                            {
                                string precioAntesDescuento = "";

                                if (infoComprobante.TipoCambio != 1)
                                {
                                    precioAntesDescuento = ObtenerPrecioAntesDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                }
                                else
                                {
                                     precioAntesDescuento = ObtenerPrecioAntesDescuentoFC(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla,nombreTablaCabezal);
                                }
                                                                                               

                                mercaderia = CeldaNueva(SeparadorMiles(precioAntesDescuento), mercaderia, Alineacion.Derecha, 0,
                                    TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                string porcentajeDescuento = ObtenerPorcentajeDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                mercaderia = CeldaNueva(SeparadorMiles(porcentajeDescuento), mercaderia, Alineacion.Derecha, 0,
                                    TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);


                                if (infoComprobante.TipoCambio != 1)
                                {
                                            mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.PrecioUnitarioItemPDF.ToString()), mercaderia,
                                          Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                            mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF.ToString()), mercaderia,
                                         Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                            articulosRegistrado++;
                                }
                                else
                                {
                                            mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.PrecioUnitarioItemPDF_FC.ToString()), mercaderia,
                                          Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                            mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF_FC.ToString()), mercaderia,
                                         Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                            articulosRegistrado++;
                                }
                                                                                        
                                                             
                            }

                        }
                        else
                        {
                            if (articulosRegistrado == (infoComprobante.Items.Count - 1) && articulosRegistrado == 25)
                            {

                                string codigo = ObtenerCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                mercaderia = CeldaNueva(codigo, mercaderia, Alineacion.Derecha, 0, TipoBordes.LateralIzquierdaUltima, 10, 0,
                                    TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                if (lineaProducto.DescripcionItem.Length < 40)
                                {

                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                }
                                else
                                {
                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem.Substring(0, 40), mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                }
                                mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.CantidadItem.ToString()), mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);


                                if (infoComprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
                                {
                                    mercaderia = CeldaNueva("", mercaderia, Alineacion.Derecha, 0,
                                      TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    mercaderia = CeldaNueva("", mercaderia, Alineacion.Derecha, 0,
                                       TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    mercaderia = CeldaNueva("", mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    mercaderia = CeldaNueva("", mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                                    articulosRegistrado++;
                                }

                                else
                                {
                                  
                                    string precioAntesDescuento = "";

                                    if (infoComprobante.TipoCambio != 1)
                                    {
                                        precioAntesDescuento = ObtenerPrecioAntesDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                    }
                                    else
                                    {
                                        precioAntesDescuento = ObtenerPrecioAntesDescuentoFC(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla, nombreTablaCabezal);
                                    }

                                    mercaderia = CeldaNueva(SeparadorMiles(precioAntesDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    string porcentajeDescuento = ObtenerPorcentajeDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                    mercaderia = CeldaNueva(SeparadorMiles(porcentajeDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                 


                                    if (infoComprobante.TipoCambio != 1)
                                    {
                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.PrecioUnitarioItemPDF.ToString()), mercaderia,
                                      Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF.ToString()), mercaderia,
                                            Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                        articulosRegistrado++;
                                    }
                                    else
                                    {
                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.PrecioUnitarioItemPDF_FC.ToString()), mercaderia,
                                      Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF_FC.ToString()), mercaderia,
                                            Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                        articulosRegistrado++;
                                    }


                                }
                            }
                            else
                            {
                                string codigo = ObtenerCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                mercaderia = CeldaNueva(codigo, mercaderia, Alineacion.Derecha, 0, TipoBordes.LateralIzquierda, 10, 0,
                                    TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                if (lineaProducto.DescripcionItem.Length < 40)
                                {

                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                                }
                                else
                                {
                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem.Substring(0, 40), mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                                }
                                mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.CantidadItem.ToString()), mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);


                                if (infoComprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
                                {
                                    mercaderia = CeldaNueva("", mercaderia, Alineacion.Derecha, 0,
                                      TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    mercaderia = CeldaNueva("", mercaderia, Alineacion.Derecha, 0,
                                       TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    mercaderia = CeldaNueva("", mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    mercaderia = CeldaNueva("", mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                                    articulosRegistrado++;
                                    

                                }

                                else
                                {
                                    string precioAntesDescuento = "";

                                    if (infoComprobante.TipoCambio != 1)
                                    {
                                        precioAntesDescuento = ObtenerPrecioAntesDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                    }
                                    else
                                    {
                                        precioAntesDescuento = ObtenerPrecioAntesDescuentoFC(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla,nombreTablaCabezal);
                                    }



                                    mercaderia = CeldaNueva(SeparadorMiles(precioAntesDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                    string porcentajeDescuento = ObtenerPorcentajeDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                    mercaderia = CeldaNueva(SeparadorMiles(porcentajeDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                                   


                                    if (infoComprobante.TipoCambio != 1)
                                    {
                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.PrecioUnitarioItemPDF.ToString()), mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF.ToString()), mercaderia,
                                            Alineacion.Derecha, 0, TipoBordes.LateralDerecha, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                        articulosRegistrado++;
                                    }
                                    else
                                    {
                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.PrecioUnitarioItemPDF_FC.ToString()), mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.Vacia, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                                        mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF_FC.ToString()), mercaderia,
                                            Alineacion.Derecha, 0, TipoBordes.LateralDerecha, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                                        articulosRegistrado++;
                                    }
                                }
                            }
                        }

                    }
                }
                while (articulosRegistrado < cantidadMaximaArticulos)
                {
                    if (articulosRegistrado == (cantidadMaximaArticulos - 1))
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralIzquierdaUltima,
                            9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 5, TipoBordes.UltimaFila, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima,
                            9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    }
                    else
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralIzquierda, 9,
                            0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 5, TipoBordes.Vacia, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerecha, 9,
                            0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    }
                    articulosRegistrado++;
                }
            }
            else
            {
                #region SERVICIO

                mercaderia = CeldaNueva(Mensaje.pdfDetalleMercaderia, mercaderia, Alineacion.Izquierda, 6,
                    TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                mercaderia = CeldaNueva(Mensaje.pdfPrecioFinal, mercaderia, Alineacion.Izquierda, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                foreach (CFEItems lineaProducto in infoComprobante.Items)
                {
                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 6,
                        TipoBordes.LateralIzquierda, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    mercaderia = CeldaNueva(lineaProducto.PrecioUnitarioItem.ToString(), mercaderia, Alineacion.Derecha,
                        0, TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    articulosRegistrado++;
                }

                while (articulosRegistrado < cantidadMaximaArticulos)
                {
                    if (articulosRegistrado == (cantidadMaximaArticulos - 1))
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Izquierda, 7, TipoBordes.UltimaFila, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    }
                    else
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Izquierda, 6, TipoBordes.LateralIzquierda, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    }
                }
                #endregion
            }

            if (ValidarFinHoja(articulosRegistrado, cantidadMaximaArticulos, out resultado))
            {
                int diferencia = resultado - articulosRegistrado, p = 0;
                if (diferencia < 14)
                {
                    while (p < diferencia)
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 7, TipoBordes.Vacia, 9,
                               0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        p++;
                    }
                    PdfPCell relleno = new PdfPCell();
                    relleno.FixedHeight = 15;
                    relleno.Border = PdfCell.NO_BORDER;
                    relleno.Colspan = 7;

                    mercaderia.AddCell(relleno);
                }
            }

            PdfPTable totales = new PdfPTable(new float[] { 0.8f, 1.7f, 1.5f, 2f, 1f, 1f, 1f });
            totales.WidthPercentage = 100f;
            totales.SpacingBefore = 0;
            totales.SpacingAfter = 0;
            totales = AgregarTotales(totales, infoComprobante, kilosFactura, datosPdf);

            PdfPTable adendaTotales = new PdfPTable(new float[] { 3.25f, 0.75f, 2f, 1f, 1f, 1f });
            adendaTotales.WidthPercentage = 100f;
            adendaTotales.SpacingBefore = 0;
            adendaTotales = AgregarAdendaTotales(adendaTotales, infoComprobante, datosPdf);

            comprobante.Add(mercaderia);
            comprobante.Add(totales);
            comprobante.Add(adendaTotales);

            return comprobante;
        }

        /// <summary>
        /// Agrega la adenda y las ultimas celdas de totales
        /// </summary>
        /// <param name="tablaFinal"></param>
        /// <param name="comprobante"></param>
        /// <param name="datosPdf"></param>
        /// <returns></returns>
        private PdfPTable AgregarAdendaTotales(PdfPTable tablaFinal, CFE comprobante, DatosPDF datosPdf)
        {
            #region CUARTA

            if (!comprobante.TextoLibreAdenda.Equals(""))
            {
                tablaFinal = CeldaNueva(Mensaje.pdfADENDA, tablaFinal, Alineacion.Izquierda, 0, TipoBordes.CajaArriba
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 2, TipoBordes.Vacia
                                , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            tablaFinal = CeldaNueva("Importes Netos", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Completa,
                9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("", tablaFinal,
                    Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalMontoNoGravado.ToString()), tablaFinal,
                    Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("",
                       tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            else
            {
                if (comprobante.TipoCambio != 1)
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalMontoNetoIVATasaMinimaExtranjero.ToString()),
                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalMontoNetoIVATasaMinimaFC.ToString()), tablaFinal, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }


            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("",
                      tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            else
            {
                if (comprobante.TipoCambio != 1)
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalMontoNetoIVATasaBasicaExtranjero.ToString()),
                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalMontoNetoIVATasaBasicaFC.ToString()), tablaFinal, Alineacion.Derecha,
                        0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }

            #endregion CUARTA


            #region QUINTA

            if (!comprobante.TextoLibreAdenda.Equals(""))
            {
                tablaFinal = CeldaNueva(comprobante.TextoLibreAdenda, tablaFinal, Alineacion.Izquierda, 0, TipoBordes.CajaAbajo
                , 10, 3, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia, 9, 3,
                    TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia, 9, 0,
                    TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);



            tablaFinal = CeldaNueva("Impuestos", tablaFinal, Alineacion.Izquierda, 0,
                TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);


            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("",
                                       tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                if (comprobante.TipoCambio != 1)
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalIVATasaMinimaExtranjero.ToString()),
                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalIVATasaMinimaFC.ToString()), tablaFinal, Alineacion.Derecha
                        , 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }

            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("",
                                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            else
            {
                if (comprobante.TipoCambio != 1)
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalIVATasaBasicaExtranjero.ToString()),
                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalIVATasaBasicaFC.ToString()), tablaFinal,
                        Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }

            #endregion QUINTA

            #region SEXTA

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia, 9, 0,
                    TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            tablaFinal = CeldaNueva("Totales", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Completa, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);


            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("", tablaFinal,
                                    Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                tablaFinal = CeldaNueva(SeparadorMiles(comprobante.TotalMontoNoGravado.ToString()), tablaFinal,
                    Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }


            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("",
                      tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                if (comprobante.TipoCambio.Equals("1"))
                {
                    tablaFinal = CeldaNueva(SeparadorMiles((comprobante.TotalMontoNetoIVATasaMinimaExtranjero +
                    comprobante.TotalIVATasaMinimaExtranjero).ToString()),
                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles((comprobante.TotalMontoNetoIVATasaMinimaFC
                        + comprobante.TotalIVATasaMinimaFC).ToString()), tablaFinal, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }


            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("",
                       tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            else
            {
                if (comprobante.TipoCambio != 1)
                {
                    tablaFinal = CeldaNueva(SeparadorMiles((comprobante.TotalMontoNetoIVATasaBasicaExtranjero
                        + comprobante.TotalIVATasaBasicaExtranjero).ToString()),
                        tablaFinal, Alineacion.Derecha, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles((comprobante.TotalMontoNetoIVATasaBasicaFC +
                        comprobante.TotalIVATasaBasicaFC).ToString()), tablaFinal, Alineacion.Derecha,
                        0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }

            #endregion SEXTA

            #region SEPTIMA

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia, 9, 0,
                    TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

            tablaFinal = CeldaNueva("Total a Pagar", tablaFinal, Alineacion.Derecha, 2, TipoBordes.Completa
                , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0,
                              TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            else
            {
                tablaFinal = CeldaNueva(comprobante.TipoModena, tablaFinal, Alineacion.Centrado, 0,
                TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }


            if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
            {
                tablaFinal = CeldaNueva("", tablaFinal,
                    Alineacion.Derecha, 0, TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            else
            {
                if (comprobante.TipoCambio != 1)
                {
                    double montoTotalPago = double.Parse(datosPdf.MontoTotalPagar);
                    double tipoCambioS = comprobante.TipoCambio;
                    double montoTotalPagarF = montoTotalPago;// / tipoCambioS;

                    tablaFinal = CeldaNueva(SeparadorMiles(Math.Round(montoTotalPagarF, 2).ToString()), tablaFinal,
                        Alineacion.Derecha, 0, TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaFinal = CeldaNueva(SeparadorMiles(datosPdf.MontoTotalPagarPesos), tablaFinal, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
            }

            #endregion SEPTIMA

            return tablaFinal;
        }


        /// <summary>
        /// Agrega los totales al pdf
        /// </summary>
        /// <param name="tablaArticulos"></param>
        /// <param name="comprobante"></param>
        /// <param name="kilosFactura"></param>
        /// <param name="nombreVendedor"></param>
        /// <param name="titular"></param>
        /// <returns></returns>
        private PdfPTable AgregarTotales(PdfPTable tablaArticulos, CFE comprobante, string kilosFactura,
            DatosPDF datosPdf)
        {
            string tituloPrimeraCelda = "", contenidoPrimeraCelda = "", titSegundaCelda = "", contSegundaCelda = "",
                titTerceraCelda = "", contTerceraCelda = "";
            bool kilos = false, vendedor = false, entrega = false;
            double mExento = 0, mBasica = 0, mMinima = 0, mPorExento = 0, mPorBasica = 0, mPorMinima = 0, porcentajeDescuento = 0;

            #region PRIMERA CELDA

            if (!kilosFactura.Equals("0"))
            {
                tituloPrimeraCelda = "Total Kilos: ";
                contenidoPrimeraCelda = kilosFactura;
                kilos = true;
            }
            else if (!datosPdf.NombreVendedor.Equals(""))
            {
                tituloPrimeraCelda = "Vendedor: ";
                contenidoPrimeraCelda = datosPdf.NombreVendedor;
                vendedor = true;
            }
            else if (!datosPdf.Titular.Equals(""))
            {
                tituloPrimeraCelda = "Entrega: ";
                contenidoPrimeraCelda = datosPdf.Titular;
                entrega = true;
            }

            if (tituloPrimeraCelda.Equals(""))
            {
                tablaArticulos = CeldaNueva(tituloPrimeraCelda, tablaArticulos, Alineacion.Izquierda, 2, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }
            else
            {
                tablaArticulos = CeldaNueva(tituloPrimeraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.CIzquierda
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                tablaArticulos = CeldaNueva(contenidoPrimeraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.CDerecha
                    , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
            }

            tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Blanco);
            tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Completa, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaArticulos = CeldaNueva("Exento", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Completa, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaArticulos = CeldaNueva("T.Mínima 10%", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Completa,
                9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);
            tablaArticulos = CeldaNueva("T.Básica 22% ", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Completa,
                9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);

            #endregion

            #region SEGUNDA-TERCERA CELDA

            if (tituloPrimeraCelda.Equals(""))
            {
                tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 3, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Negro, ColorC.Blanco);
                tablaArticulos = CeldaNueva("Importes Brutos", tablaArticulos, Alineacion.Izquierda, 0,
                    TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Negro, ColorC.Blanco);

                porcentajeDescuento = datosPdf.PorcentajeDescuento / 100;

                if (comprobante.TipoCambio != 1)
                {
                    mExento = comprobante.TotalMontoNoGravadoExtranjero / (1 - porcentajeDescuento);
                    mMinima = comprobante.TotalMontoNetoIVATasaMinimaExtranjero / (1 - porcentajeDescuento);
                    mBasica = comprobante.TotalMontoNetoIVATasaBasicaExtranjero / (1 - porcentajeDescuento);
                }
                else
                {
                    mExento = comprobante.TotalMontoNoGravadoFC / (1 - porcentajeDescuento);
                    mMinima = comprobante.TotalMontoNetoIVATasaMinimaFC / (1 - porcentajeDescuento);
                    mBasica = comprobante.TotalMontoNetoIVATasaBasicaFC / (1 - porcentajeDescuento);
                }

                mPorExento = mExento * porcentajeDescuento;
                mPorMinima = mMinima * porcentajeDescuento;
                mPorBasica = mBasica * porcentajeDescuento;


                if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
                {
                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                       TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 3, TipoBordes.Vacia
                            , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("Descuento " + datosPdf.PorcentajeDescuento.ToString() + "%",
                        tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Negro,ColorC.Blanco);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                }

                else
                {

                    tablaArticulos = CeldaNueva(SeparadorMiles(mExento.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mMinima.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mBasica.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 3, TipoBordes.Vacia
                            , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva("Descuento " + datosPdf.PorcentajeDescuento.ToString() + "%",
                        tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mPorExento.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mPorMinima.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mPorBasica.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                }
            }
            else
            {
                if (kilos)
                {
                    if (!datosPdf.NombreVendedor.Equals(""))
                    {
                        titSegundaCelda = "Vendedor: ";
                        contSegundaCelda = datosPdf.NombreVendedor;
                        vendedor = true;
                    }
                }
                else if (vendedor)
                {
                    vendedor = false;

                    if (!datosPdf.Titular.Equals(""))
                    {
                        titSegundaCelda = "Entrega: ";
                        contSegundaCelda = datosPdf.Titular;
                        entrega = true;
                    }
                }
                else if (entrega)
                {
                    titSegundaCelda = "";
                    contSegundaCelda = "";
                    entrega = false;
                }

                if (entrega || vendedor)
                {
                    tablaArticulos = CeldaNueva(titSegundaCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralIzquierdaUltima
                            , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(contSegundaCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerechaUltima
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }
                else
                {
                    tablaArticulos = CeldaNueva(titSegundaCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia
                            , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(contSegundaCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }

                porcentajeDescuento = datosPdf.PorcentajeDescuento / 100;

                if (comprobante.TipoCambio != 1)
                {
                    mExento = comprobante.TotalMontoNoGravadoExtranjero / (1 - porcentajeDescuento);
                    mMinima = comprobante.TotalMontoNetoIVATasaMinimaExtranjero / (1 - porcentajeDescuento);
                    mBasica = comprobante.TotalMontoNetoIVATasaBasicaExtranjero / (1 - porcentajeDescuento);
                }
                else
                {
                    mExento = comprobante.TotalMontoNoGravadoFC / (1 - porcentajeDescuento);
                    mMinima = comprobante.TotalMontoNetoIVATasaMinimaFC / (1 - porcentajeDescuento);
                    mBasica = comprobante.TotalMontoNetoIVATasaBasicaFC / (1 - porcentajeDescuento);
                }

                mPorExento = mExento * porcentajeDescuento;
                mPorMinima = mMinima * porcentajeDescuento;
                mPorBasica = mBasica * porcentajeDescuento;


                if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
                {
                    tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia, 9, 0,
                       TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("Importes Brutos", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Completa
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }

                else
                {
                    tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia, 9, 0,
                        TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva("Importes Brutos", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Completa
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);

                    tablaArticulos = CeldaNueva(SeparadorMiles(mExento.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva(SeparadorMiles(mMinima.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                    tablaArticulos = CeldaNueva(SeparadorMiles(mBasica.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                }

                if (vendedor)
                {
                    if (!datosPdf.Titular.Equals(""))
                    {
                        titTerceraCelda = "Entrega: ";
                        contTerceraCelda = datosPdf.Titular;
                        tablaArticulos = CeldaNueva(titTerceraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralIzquierdaUltima
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        tablaArticulos = CeldaNueva(contTerceraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerechaUltima
                            , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    }
                    else
                    {
                        tablaArticulos = CeldaNueva(titTerceraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                        tablaArticulos = CeldaNueva(contTerceraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia
                            , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    }
                }
                else
                {
                    tablaArticulos = CeldaNueva(titTerceraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(contTerceraCelda, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }

                tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia, 9, 0,
                    TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                tablaArticulos = CeldaNueva(SeparadorMiles("Descuento " + datosPdf.PorcentajeDescuento.ToString() + "%"),
                     tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro);


                if (comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemito)
                {
                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                }

                else
                {
                    tablaArticulos = CeldaNueva(SeparadorMiles(mPorExento.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mPorMinima.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);
                    tablaArticulos = CeldaNueva(SeparadorMiles(mPorBasica.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                        TipoBordes.Completa, 9, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro);

                }
            }

            #endregion SEGUNDA-TERCERA CELDA

            return tablaArticulos;
        }

        /// <summary>
        /// Metodo para crear celdas/Alineacion: 0 = derecha, 1 = izquierda, 2 = centrado
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        private PdfPTable CeldaNueva(string contenido, PdfPTable tabla, Alineacion alinear, int colSpan, TipoBordes bordes,
            int tamLetra, int rowSpan, TipoLetra letra, AlineacionVertical alinearVertical, ColorC colorFondo, ColorC colorLetra)
        {
            PdfPCell resultado = null;

            BaseFont letraTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = null;

            #region TIPO LETRAS

            if (letra == TipoLetra.Normal)
            {
                if (colorLetra == ColorC.Blanco)
                {
                    times = new Font(letraTimes, tamLetra, Font.NORMAL, Color.WHITE);
                }
                else
                {
                    times = new Font(letraTimes, tamLetra, Font.NORMAL, Color.BLACK);
                }
            }
            else if (letra == TipoLetra.Bold)
            {
                if (colorLetra == ColorC.Blanco)
                {
                    times = new Font(letraTimes, tamLetra, Font.BOLD, Color.WHITE);
                }
                else
                {
                    times = new Font(letraTimes, tamLetra, Font.BOLD, Color.BLACK);
                }                
            }
            else if (letra == TipoLetra.Italic)
            {
                times = new Font(letraTimes, tamLetra, Font.ITALIC, Color.BLACK);
            }
            else if (letra == TipoLetra.BoldItalic)
            {
                times = new Font(letraTimes, tamLetra, Font.BOLDITALIC, Color.BLACK);
            }

            #endregion TIPO LETRAS

            resultado = new PdfPCell(new Phrase(0, contenido, times));

            #region COMBINAR FILAS Y CELDAS

            if (colSpan != 0)
            {
                //Se combinan las celdas segun la cantidad especificada
                resultado.Colspan = colSpan;
            }

            if (rowSpan != 0)
            {
                //Se combinan las filas segun la cantidad especificada
                resultado.Rowspan = rowSpan;
            }

            #endregion COMBINAR FILAS Y CELDAS

            #region BORDES

            if (bordes == TipoBordes.Vacia)
            {
                resultado.Border = PdfPCell.NO_BORDER;
            }
            else if (bordes == TipoBordes.Completa)
            {
                resultado.Border = PdfPCell.BOTTOM_BORDER + PdfPCell.LEFT_BORDER + PdfPCell.RIGHT_BORDER + PdfCell.TOP_BORDER;
            }
            else if (bordes == TipoBordes.UltimaFila)
            {
                resultado.Border = PdfPCell.BOTTOM_BORDER;
            }
            else if (bordes == TipoBordes.PrimeraFila)
            {
                resultado.Border = PdfPCell.TOP_BORDER;
            }
            else if (bordes == TipoBordes.LateralDerecha)
            {
                resultado.Border = PdfPCell.RIGHT_BORDER;
            }
            else if (bordes == TipoBordes.LateralIzquierda)
            {
                resultado.Border = PdfPCell.LEFT_BORDER;
            }
            else if (bordes == TipoBordes.LateralDerechaUltima)
            {
                resultado.Border = PdfCell.BOTTOM_BORDER + PdfPCell.RIGHT_BORDER;
            }
            else if (bordes == TipoBordes.LateralIzquierdaUltima)
            {
                resultado.Border = PdfCell.BOTTOM_BORDER + PdfPCell.LEFT_BORDER;
            }
            else if (bordes == TipoBordes.LateralDerechaPrimera)
            {
                resultado.Border = PdfCell.TOP_BORDER + PdfPCell.RIGHT_BORDER;
            }
            else if (bordes == TipoBordes.LateralIzquierdaPrimera)
            {
                resultado.Border = PdfCell.TOP_BORDER + PdfPCell.LEFT_BORDER;
            }
            else if (bordes == TipoBordes.CajaArriba)
            {
                resultado.Border = PdfCell.LEFT_BORDER + PdfCell.TOP_BORDER + PdfCell.RIGHT_BORDER;
            }
            else if (bordes == TipoBordes.CajaAbajo)
            {
                resultado.Border = PdfCell.LEFT_BORDER + PdfCell.BOTTOM_BORDER + PdfCell.RIGHT_BORDER;
            }
            else if (bordes == TipoBordes.CDerecha)
            {
                resultado.Border = PdfCell.RIGHT_BORDER + PdfCell.BOTTOM_BORDER + PdfCell.TOP_BORDER;
            }
            else if (bordes == TipoBordes.CIzquierda)
            {
                resultado.Border = PdfCell.LEFT_BORDER + PdfCell.BOTTOM_BORDER + PdfCell.TOP_BORDER;
            }

            #region COLORES

            if (colorFondo == ColorC.Negro)
            {
                resultado.BackgroundColor = Color.BLACK;                
            }

            #endregion COLORES

            #endregion BORDES

            #region ALINEACION

            //Condiciones para alinear el contenido de la celda
            if (alinear == Alineacion.Izquierda)
            {
                resultado.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else if (alinear == Alineacion.Derecha)
            {
                resultado.HorizontalAlignment = Element.ALIGN_RIGHT;
            }
            else if (alinear == Alineacion.Centrado)
            {
                resultado.HorizontalAlignment = Element.ALIGN_CENTER;
            }


            //Condiciones para alinear el contenido de la celda verticalmente
            if (alinearVertical == AlineacionVertical.Abajo)
            {
                resultado.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            else if (alinearVertical == AlineacionVertical.Arriba)
            {
                resultado.VerticalAlignment = Element.ALIGN_TOP;
            }
            else if (alinearVertical == AlineacionVertical.Centrado)
            {
                resultado.VerticalAlignment = Element.ALIGN_CENTER;
            }

            #endregion ALINEACION

            tabla.AddCell(resultado);

            return tabla;
        }

        /// <summary>
        /// Metodo que genera parrafo con codigoQR
        /// </summary>
        /// <param name="factura"></param>
        /// <param name="rutaArchivos"></param>
        /// <param name="numResolucion"></param>
        /// <param name="yyyy"></param>
        /// <param name="ruc"></param>
        /// <param name="tipoCFE"></param>
        /// <param name="serie"></param>
        /// <param name="nroCFE"></param>
        /// <param name="monto"></param>
        /// <param name="fecha"></param>
        /// <param name="hash1"></param>
        /// <param name="nroCAE"></param>
        /// <param name="serieCAE"></param>
        /// <param name="nroInicialCAE"></param>
        /// <param name="nroFinalCAE"></param>
        /// <param name="fechaVenceCAE"></param>
        /// <returns></returns>
        public Paragraph CodigoQr(string hash1)
        {
            string rutaQR = RutasCarpetas.RutaCarpetaComprobantes + Mensaje.nomImagenQr;

            iTextSharp.text.Image imagenCodigo = iTextSharp.text.Image.
                GetInstance(rutaQR);
            imagenCodigo.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
            imagenCodigo.ScaleAbsolute(70f, 70f);

            Paragraph izqInferior = new Paragraph(new Chunk(imagenCodigo
                , 0, 0, true));

            izqInferior.Add(nL);

            if (hash1.Length >= 6)
            {
                izqInferior.Add(Mensaje.pdfCodSeg + hash1.Substring(0, 6));
            }
            else
            {
                izqInferior.Add(Mensaje.pdfCodSeg + hash1);
            }

            System.IO.File.Delete(rutaQR);

            return izqInferior;
        }

        /// <summary>
        /// Genera la parte final del documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="factura"></param>
        /// <param name="ruc"></param>
        /// <param name="tipoCFE"></param>
        /// <param name="serie"></param>
        /// <param name="monto"></param>
        /// <param name="fecha"></param>
        /// <param name="hash1"></param>
        /// <param name="nroCFE"></param>
        /// <param name="serieCAE"></param>
        /// <param name="nroInicialCAE"></param>
        /// <param name="nroFinalCAE"></param>
        /// <param name="yyyy"></param>
        /// <param name="numResolucion"></param>
        /// <param name="nroCAE"></param>
        /// <param name="fechaVenceCAE"></param>
        /// <param name="ruta"></param>        
        //public PdfPTable finArchivo(string serieCAE, string nroInicialCAE, string nroFinalCAE,
        //string numResolucion, string nroCAE, string fechaVenceCAE)
        public PdfPTable FinArchivo(CAE pCae, CFE pComprobante)
        {
            string vencimiento = Mensaje.pdfFechaVencimiento + pCae.FechaVencimiento;
            //string vencimiento = Mensaje.pdfFechaVencimiento + fechaVenceCAE;

            PdfPTable piePagina = new PdfPTable(new float[] { 2f, 1f });

            //piePagina.TotalWidth = factura.PageSize.Width - factura.LeftMargin
            //- factura.RightMargin;

            //string rangoCAE = serieCAE + "-" + nroInicialCAE + "-" + nroFinalCAE;
            string rangoCAE = pCae.Serie + "-" + pCae.NumeroDesde + "-" + pCae.NumeroHasta;

            BaseFont letraTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(letraTimes, 9, Font.NORMAL, Color.BLACK);

            Paragraph izqInferior = new Paragraph("", times);

            //Agrega leyendas con informacion del CAE
            Chunk textoCodigoSeguridad = new Chunk(Mensaje.pdfCodigoSeguridad);
            izqInferior.Add(textoCodigoSeguridad);
            izqInferior.Add(Chunk.NEWLINE);
            izqInferior.Add(Chunk.NEWLINE);
            izqInferior.Add(Mensaje.pdfRes + pComprobante.NumeroResolucion);
            izqInferior.Add(Chunk.NEWLINE);
            izqInferior.Add(Mensaje.pdfLinkDGI);
            izqInferior.Add(Chunk.NEWLINE);
            izqInferior.Add(Mensaje.pdfIVADia);
            izqInferior.Add(Chunk.NEWLINE);
            izqInferior.Add(Mensaje.pdfNumCAE + pCae.NumeroAutorizacion);
            izqInferior.Add(Chunk.NEWLINE);
            izqInferior.Add(Mensaje.pdfRanAut + rangoCAE);

            //Agrega codigo de seguridad y leyendas al footer
            PdfPCell izqInf = new PdfPCell(izqInferior);

            izqInf.Border = PdfPCell.NO_BORDER;
            izqInf.Colspan = 3;
            piePagina.AddCell(izqInf);

            //Cuadro con fecha de vencimiento
            PdfPTable tFechaVencimiento = new PdfPTable(new float[] { 1 });

            tFechaVencimiento.TotalWidth = 50f;

            Paragraph contenidoVence = new Paragraph();

            contenidoVence.Add(vencimiento);

            PdfPCell celda = new PdfPCell(contenidoVence);

            celda.MinimumHeight = 22f;
            celda.Border = PdfPCell.NO_BORDER;
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            tFechaVencimiento.AddCell(celda);

            return piePagina;
        }

        /// <summary>
        /// Genera la adenda en una nueva pagina
        /// </summary>
        /// <param name="factura"></param>
        /// <returns></returns>
        public Document Adenda(Document factura, string mensaje)
        {
            factura.Add(Chunk.NEWLINE);
            //factura.NewPage();
            PdfPTable adenda = new PdfPTable(new float[] { 1 });
            adenda.KeepTogether = true;

            Paragraph titulo = new Paragraph(Mensaje.pdfADENDA);
            titulo.Alignment = Element.ALIGN_CENTER;

            Paragraph contenido = new Paragraph(mensaje);
            contenido.Alignment = Element.ALIGN_JUSTIFIED;

            PdfPCell celdaAdenda = new PdfPCell();
            celdaAdenda.AddElement(titulo);
            celdaAdenda.AddElement(contenido);

            adenda.AddCell(celdaAdenda);

            factura.Add(adenda);

            return factura;
        }

        /// <summary>
        /// Obtiene el precio antes de descuento por articulo
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="itemName"></param>
        /// <param name="itemQuantity"></param>
        /// <returns></returns>
        private string ObtenerPrecioAntesDescuento(string docEntry, string lineNum, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT PriceBefDi FROM [" + tabla + "] WHERE DocEntry = '" + docEntry + "' AND LineNum = '" + lineNum + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("PriceBefDi").Value + "";
                }
                else
                {
                    resultado = "0";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }



         /// <summary>
        /// Obtiene el precio antes de descuento por articulo
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="itemName"></param>
        /// <param name="itemQuantity"></param>
        /// <returns></returns>
        private string ObtenerPrecioAntesDescuentoFC(string docEntry, string lineNum, string tabla, string tablaCabezal)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;
                           

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT (T1.PriceBefDi * ( 1 / T2.DocRate)) as PriceBefDiFC  FROM [" + tabla + "] T1  inner join [" + tablaCabezal + "]  T2 on T1.DocEntry = T2.DocEntry WHERE T1.DocEntry = '" + docEntry + "' AND T1.LineNum = '" + lineNum + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("PriceBefDiFC").Value + "";
                }
                else
                {
                    resultado = "0";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }
        



        /// <summary>
        /// Obtiene el Objeto Empresa Datos
        /// </summary>
        /// <param name="E_Mail"></param>
        /// <param name="Phone1"></param>
        /// <param name="direccion"></param>
        ///  <param name="Web"></param>
        ///  <param name="Nombre"></param>
        ///   <param name="NombreC"></param>
        /// <returns></returns>
        /*private EmpresaDatos ObtenerEmpresaDatos(EmpresaDatos Empresa)
        {
            string consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "select E_Mail,  concat('Tel:' , Phone1) as Phone1, concat(Street, ' ' , StreetNo,' - ' , City) as direccion, IntrntAdrs , U_Nombre, U_NombreC FROM OADM a inner join ADM1 b on a.Code = b.Code inner join [@TFEEMI] c on c.DocEntry = a.Code";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    Empresa.E_Mail = registro.Fields.Item("E_Mail").Value + "";
                    Empresa.Phone = registro.Fields.Item("Phone1").Value + "";
                    Empresa.Direccion = registro.Fields.Item("direccion").Value + "";
                    Empresa.Web = registro.Fields.Item("IntrntAdrs").Value + "";
                    Empresa.Nombre = registro.Fields.Item("U_Nombre").Value + "";
                    Empresa.NombreComercial = registro.Fields.Item("U_NombreC").Value + "";
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return Empresa;
        }*/
        private EmpresaDatos ObtenerEmpresaDatos(EmpresaDatos Empresa)
        {
            string consulta = string.Empty, CodigoSucursal = "", direccion = "", Telefono = "";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select T2.Name  from OUSR T1 inner join OUBR T2 ON T1.Branch = T2.Code WHERE T1.U_NAME = '" + Conexion.ProcConexion.Comp.UserName + "'" + "or T1.USER_CODE = '" + Conexion.ProcConexion.Comp.UserName + "'";

                //Ejecutar consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    CodigoSucursal = registro.Fields.Item("Name").Value + " ";
                }


                //Establecer consulta
                consulta = "SELECT U_Calle, U_Telefono, U_Ciudad FROM [@TSUCDIRE] where U_codigo = '" + CodigoSucursal + "'";

                //Ejecutar consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    direccion += registro.Fields.Item("U_Calle").Value + " ";
                    direccion += registro.Fields.Item("U_Ciudad").Value + "";
                    Telefono = registro.Fields.Item("U_Telefono").Value + "";

                }



                consulta = "select E_Mail,  concat('Tel:' , Phone1) as Phone1, concat(Street, ' ' , StreetNo,' - ' , City) as direccion, IntrntAdrs , U_Nombre, U_NombreC FROM OADM a inner join ADM1 b on a.Code = b.Code inner join [@TFEEMI] c on c.DocEntry = a.Code";
                registro.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (registro.RecordCount > 0)
                {
                    Empresa.E_Mail = registro.Fields.Item("E_Mail").Value + "";
                    Empresa.Phone = Telefono;
                    Empresa.Direccion = direccion;
                    Empresa.Web = registro.Fields.Item("IntrntAdrs").Value + "";
                    Empresa.Nombre = registro.Fields.Item("U_Nombre").Value + "";
                    Empresa.NombreComercial = registro.Fields.Item("U_NombreC").Value + "";
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return Empresa;
        }



        /// <summary>
        /// Obtiene el precio antes de descuento por articulo
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="itemName"></param>
        /// <param name="itemQuantity"></param>
        /// <returns></returns>
        private string ObtenerCodigo(string docEntry, string lineNum, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT ItemCode FROM [" + tabla + "] WHERE DocEntry = '" + docEntry + "' AND LineNum = '" + lineNum + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("ItemCode").Value + "";
                }
                else
                {
                    resultado = "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }


        /// <summary>
        /// Obtiene si Item es componente de un Pack - si lo es no lo imprimo
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="itemName"></param>
        /// <param name="itemQuantity"></param>
        /// <returns></returns>
        private string ObtenerPackCodigo(string docEntry, string lineNum, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT TreeType FROM [" + tabla + "] WHERE DocEntry = '" + docEntry + "' AND LineNum = '" + lineNum + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("TreeType").Value + "";
                }
                else
                {
                    resultado = "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }





        /// <summary>
        /// Obtiene el porcentaje descuento por articulo
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="itemName"></param>
        /// <param name="itemQuantity"></param>
        /// <returns></returns>
        private string ObtenerPorcentajeDescuento(string docEntry, string lineNum, string tabla)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT DiscPrcnt FROM [" + tabla + "] WHERE DocEntry = '" + docEntry + "' AND LineNum = '" + lineNum + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("DiscPrcnt").Value + "";
                }
                else
                {
                    resultado = "0";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el numero de linea de un articulo
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="descripcionArticulos"></param>
        /// <param name="cantidadArticulo"></param>
        /// <returns></returns>
        private string ObtenerNumeroLinea(string docEntry, string descripcionArticulos, string cantidadArticulo)
        {
            string resultado = string.Empty, consulta = string.Empty;
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                consulta = "SELECT LineNum FROM INV1 WHERE Dscription = '" + descripcionArticulos + "' AND Quantity = '" +
                    cantidadArticulo + "' AND DocEntry = '" + docEntry + "'";
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("LineNum").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Metodo que agrega separador de miles
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        private string SeparadorMiles(string numero)
        {
            string resultado = string.Empty;

            try
            {
                double temporal = double.Parse(numero);
                resultado = temporal.ToString("N2");
                resultado.Replace(".", "$");
                resultado.Replace(",", ".");
                resultado.Replace("$", ",");
            }
            catch (Exception)
            {
                resultado = numero;
            }

            return resultado;
        }

        /// <summary>
        /// Tipos de bordes para la celda
        /// </summary>
        public enum TipoBordes
        {
            Completa = 0,
            Vacia = 1,
            UltimaFila = 2,
            LateralDerecha = 3,
            LateralIzquierda = 4,
            LateralDerechaUltima = 5,
            LateralIzquierdaUltima = 6,
            LateralDerechaPrimera = 7,
            LateralIzquierdaPrimera = 8,
            PrimeraFila = 9,
            CajaArriba = 10,
            CajaAbajo = 11,
            CIzquierda = 12,
            CDerecha = 13
        }

        /// <summary>
        /// Tipos de letras para la celda
        /// </summary>
        public enum TipoLetra
        {
            Normal = 0,
            Bold = 1,
            Italic = 2,
            BoldItalic = 3
        }

        /// <summary>
        /// Tipo de alineacion del texto de la celda
        /// </summary>
        public enum Alineacion
        {
            Derecha = 0,
            Izquierda = 1,
            Centrado = 2
        }

        /// <summary>
        /// Valida los espacios para crear al final de la hoja
        /// </summary>
        /// <param name="articulosRegistrado"></param>
        /// <param name="cantidadMaximaArticulos"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        private bool ValidarFinHoja(int articulosRegistrado, int cantidadMaximaArticulos, out int resultado)
        {
            bool salida = false;
            int i = 1;

            try
            {
                resultado = 0;
                while (i <= 40)
                {
                    if (articulosRegistrado > cantidadMaximaArticulos && articulosRegistrado <= (i * 37))
                    {
                        resultado = i * 37;
                        salida = true;
                        break;
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                resultado = 0;
            }

            return salida;
        }

        /// <summary>
        /// Tipo de alineacion del texto de la celda
        /// </summary>
        public enum AlineacionVertical
        {
            Default = 0,
            Arriba = 1,
            Abajo = 2,
            Centrado = 3
        }

        public enum ColorC
        {
            Blanco = 0,
            Gris = 1,
            Negro = 2
        }
    }
}
