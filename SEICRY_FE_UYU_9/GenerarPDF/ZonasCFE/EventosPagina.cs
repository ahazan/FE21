using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.ZonasCFE
{
    public class EventosPagina : PdfPageEventHelper
    {
        string logo = string.Empty, domicilioFiscal = string.Empty, docNum = string.Empty, monto = string.Empty;
        CFE infoComprobantes = null;
        CAE infoCAE = null;


        public static bool errorRutaLogo = false;

        DatosPDF datosPdf = null;
        public EventosPagina(CFE pInfoComprobante, CAE pInfoCAE, string pDomicilioFiscal, string pLogo, string pDocNum, DatosPDF pDatosPdf)
        {
            this.infoCAE = pInfoCAE;
            this.infoComprobantes = pInfoComprobante;
            this.domicilioFiscal = pDomicilioFiscal;
            this.logo = pLogo;
            this.docNum = pDocNum;
            this.datosPdf = pDatosPdf;
        }

        /// <summary>
        /// Plantilla para mostrar el total de paginas
        /// </summary>
        PdfTemplate totalPaginas;
        ZonasCFE.CuerpoComprobante cuerpoComprobante = new CuerpoComprobante();

        /// <summary>
        /// Evento de inicio de documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="factura"></param>
        /// No usar elementos de interfaz
        public override void OnOpenDocument(PdfWriter writer, Document comprobante)
        {
            base.OnOpenDocument(writer, comprobante);
            //Se crea template para posicionar el total de numero de paginas
            totalPaginas = writer.DirectContent.CreateTemplate(30, 16);
        }

        /// <summary>
        /// Evento de inicio de pagina
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="factura"></param>
        public override void OnStartPage(PdfWriter writer, Document factura)
        {
            base.OnStartPage(writer, factura);
        }

        /// <summary>
        /// Evento de final de pagina, permite agregar encabezados y pie de pagina
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="factura"></param>
        public override void OnEndPage(PdfWriter writer, Document comprobante)
        {
            base.OnEndPage(writer, comprobante);
            float topMarg = comprobante.TopMargin, rightMarg = comprobante.RightMargin,
                leftMarg = comprobante.LeftMargin, bottomMarg = comprobante.BottomMargin;

            #region Encabezado

            Rectangle pagina = comprobante.PageSize;

            PdfPTable p = cuerpoComprobante.Emisor(infoComprobantes,
                domicilioFiscal, comprobante, docNum, datosPdf);

            //Se crea la tabla
            PdfPTable encabezado = new PdfPTable(new float[] { 2.5f, 0.5f, 2f });

            //Se ajustan las propiedades de la tabla
            encabezado.TotalWidth = pagina.Width - rightMarg - leftMarg;
            encabezado.SpacingBefore = 20f;

            PdfPCell contenido = new PdfPCell(p);
            contenido.Border = 0;
            contenido.FixedHeight = topMarg;

            #region Logo

            /*PdfPCell cLogo = new PdfPCell();
            cLogo.HorizontalAlignment = Element.ALIGN_CENTER;
            cLogo.Border = 0;
            cLogo.FixedHeight = topMarg;

            if (logo.Equals(""))
            {
                encabezado.AddCell(cLogo);
            }
            else
            {
                try
                {
                    //Agrega la imagen a la celda
                    Image iLogo = Image.GetInstance(logo);
                    iLogo.ScaleAbsolute(150f, 80f);
                    cLogo.AddElement(iLogo);

                    //Se agrega las celda a la tabla
                    encabezado.AddCell(cLogo);
                }
                catch (Exception)
                {
                    errorRutaLogo = true;
                    contenido.Colspan = 3;
                }
            }

            PdfPCell sinDatos = new PdfPCell();
            sinDatos.Border = 0;
            encabezado.AddCell(sinDatos);*/

            PdfPCell cLogo = new PdfPCell();
            cLogo.HorizontalAlignment = Element.ALIGN_CENTER;
            cLogo.Border = 0;
            //cLogo.FixedHeight = topMarg;
            cLogo.PaddingLeft = 60;

            if (logo.Equals(""))
            {
                encabezado.AddCell(cLogo);
            }
            else
            {
                try
                {
                    //Agrega la imagen a la celda
                    Image iLogo = Image.GetInstance(logo);

                    //iLogo.ScaleAbsolute(185f, topMarg);                    
                    iLogo.ScaleAbsoluteWidth(101);
                    iLogo.ScaleAbsoluteHeight(82);

                    cLogo.AddElement(iLogo);

                    //Se agrega las celda a la tabla
                    encabezado.AddCell(cLogo);
                }
                catch (Exception)
                {
                    errorRutaLogo = true;
                    contenido.Colspan = 3;
                }
            }

            PdfPCell sinDatos = new PdfPCell();
            sinDatos.Border = 0;
            encabezado.AddCell(sinDatos);

            #endregion Logo

            //Se agrega la celda a la tabla               
            encabezado.AddCell(contenido);

            //Se escribe el header directamente en el documento
            encabezado.WriteSelectedRows(0, -1, 10, pagina.Height, writer.DirectContent);

            #endregion Encabezado

            #region Pie de página

            //Se crea la tabla
            PdfPTable contenedor = new PdfPTable(new float[] { 1.75f, 5.0f, 4.25f, 0.5f });
            contenedor.TotalWidth = pagina.Width - rightMarg - leftMarg;

            Paragraph contPie;

            //Instancia de la clase COdigo Qr
            CodigoQr.CodigoQr codigo = new CodigoQr.CodigoQr();

            //monto = monto.Replace(",",".");
            monto = infoComprobantes.MontoTotalPagar.ToString();
            monto = monto.Replace(",", ".");

            //Se genera el QR
            //bool generoCodigo = codigo.generadorQR(Mensaje.pdfDirCodigoQrQas,
            //    rucEmisor, tipoCFEInt, serie, nroCFE, monto, fecha, hash1);
            bool generoCodigo = codigo.generadorQR(Mensaje.pdfDirCodigoQrQas,
                infoComprobantes, monto);

            //Se obtiene el numero de pagina
            string numeroPagina = "Página " + comprobante.PageNumber.ToString() + " de ";

            //Se valida si se genero el codigo
            if (generoCodigo)
            {
                //Se agrega el codigo
                contPie = cuerpoComprobante.CodigoQr(infoComprobantes.CodigoSeguridad);
            }
            else
            {
                contPie = new Paragraph();
            }

            //Se agrega el numero de pagina al parrafo
            contPie.Add(new Chunk(new VerticalPositionMark()));
            //contPie.Add(numeroPagina);

            PdfPTable infoCAEQr;

            //infoCAEQr = cuerpoComprobante.finArchivo(serieCAE, numDesde, numHasta, numResolucion, numAutorizacion, fechaVenc);
            infoCAEQr = cuerpoComprobante.FinArchivo(infoCAE, infoComprobantes);

            //Se agrega el parrafo a la celda
            PdfPCell qr = new PdfPCell(contPie);

            qr.Border = PdfPCell.NO_BORDER;

            //Se agrega la informacion de CAE y Qr
            PdfPCell infoQr = new PdfPCell(infoCAEQr);
            infoQr.Border = PdfPCell.NO_BORDER;

            PdfPCell vacio = new PdfPCell();
            vacio.Border = PdfPCell.NO_BORDER;

            //Se agrega el numero de pagina a la celda
            PdfPCell numPag = new PdfPCell(Image.GetInstance(totalPaginas));

            //Se ajustan propiedades de la celda
            numPag.Border = PdfPCell.NO_BORDER;
            numPag.HorizontalAlignment = Element.ALIGN_LEFT;
            numPag.VerticalAlignment = Element.ALIGN_BOTTOM;

            //Se crea la celda con la informacion para el numero de pagina
            Paragraph numPagInfo = new Paragraph(numeroPagina);
            numPagInfo.Add(Chunk.NEWLINE);
            PdfPCell infoNumPag = new PdfPCell(numPagInfo);
            infoNumPag.Border = PdfPCell.NO_BORDER;
            infoNumPag.HorizontalAlignment = Element.ALIGN_RIGHT;

            BaseFont letraTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(letraTimes, 11, Font.BOLD, Color.BLACK);

            Paragraph textoFecha = new Paragraph("", times);
            textoFecha.Add(Chunk.NEWLINE);
            textoFecha.Add(Mensaje.pdfFechaVencimiento);
            textoFecha.Add(Chunk.NEWLINE);
            //textoFecha.Add("        " + fechaVenc);
            textoFecha.Add("        " + infoCAE.FechaVencimiento);
            PdfPCell fechaVencimiento = new PdfPCell(textoFecha);
            fechaVencimiento.Border = PdfPCell.NO_BORDER;

            contenedor.AddCell(qr);
            contenedor.AddCell(infoQr);
            contenedor.AddCell(fechaVencimiento);
            contenedor.AddCell(vacio);
            contenedor.AddCell(vacio);
            contenedor.AddCell(vacio);
            contenedor.AddCell(infoNumPag);
            contenedor.AddCell(numPag);
            //Se escribe el footer directamente en el documento
            //contenedor.WriteSelectedRows(0, -1, 10, 110, writer.DirectContent);

            #endregion Pie de página
        }

        /// <summary>
        /// Evento de cierre de documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            int total = writer.PageNumber - 1;
            //Agrega el total de paginas
            ColumnText.ShowTextAligned(totalPaginas, Element.ALIGN_LEFT, new
                Phrase(total.ToString()), 2, 2, 0);
        }
    }
}
