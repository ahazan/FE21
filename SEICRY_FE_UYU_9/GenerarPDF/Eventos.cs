using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace SEICRY_FE_UYU_9
{
    class Eventos : IPdfPCellEvent, IPdfPTableEvent
    {
        /// <summary>
        /// Metodo para manejar los eventos de la tabla
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fEncabezado"></param>
        /// <param name="fInicio"></param>
        /// <param name="canvas"></param>
        public void TableLayout(PdfPTable tabla, float[][] width, float[] height,
            int fEncabezado, int fInicio, PdfContentByte[] canvas)
        {
            float[] widths = width[0];
            float x1 = widths[0];
            float x2 = widths[widths.Length - 1];
            float y1 = height[0];
            float y2 = height[height.Length - 1];
            PdfContentByte cb = canvas[PdfPTable.LINECANVAS];
            cb.Rectangle(x1, y1, x2 - x1, y2 - y1);
            cb.Stroke();
            cb.ResetRGBColorStroke();
        }

        /// <summary>
        /// Metodo para manejar los eventos de las celdas
        /// </summary>
        /// <param name="celda"></param>
        /// <param name="posicion"></param>
        /// <param name="canvass"></param>
        public void CellLayout(PdfPCell celda, iTextSharp.text.Rectangle posicion
            , PdfContentByte[] canvass)
        {
            float x1 = posicion.GetLeft(0) + 2;
            float x2 = posicion.GetRight(0) - 2;
            float y1 = posicion.GetTop(0) - 2;
            float y2 = posicion.GetBottom(0) + 2;
            PdfContentByte canvas = canvass[PdfPTable.LINECANVAS];
            canvas.Rectangle(x1, y1, x2 - x1, y2 - y1);
            canvas.Stroke();
            canvas.ResetRGBColorStroke();
        }
    }
}
