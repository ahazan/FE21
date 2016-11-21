using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.CodigoQr
{
    class CodigoQr
    {
        /// <summary>
        /// Genera codigo QR
        /// </summary>
        /// <param name="link"></param>
        /// <param name="ruc"></param>
        /// <param name="tipoCFE"></param>
        /// <param name="serie"></param>
        /// <param name="nroCFE"></param>
        /// <param name="monto"></param>
        /// <param name="fecha"></param>
        /// <param name="hash1"></param>
        /// <returns></returns>
        //public bool generadorQR(string link, string ruc, string tipoCFE,
        //    string serie, string nroCFE, string monto, string fecha, string hash1)
            public bool generadorQR(string link, CFE pComprobante, string monto)
        {
            bool resultado = false;
            string rutaQ = RutasCarpetas.RutaCarpetaComprobantes + Mensaje.nomImagenQr;
            try
            {
                DateTime fechaFormateada = DateTime.Parse(pComprobante.FechaComprobante);

                    //hash1 = Uri.EscapeDataString(hash1);
                    //string informacion = link + "?" + ruc + "," + tipoCFE + "," + serie +
                    //    "," + nroCFE + "," + monto + "," + fecha + "," + hash1;
                    pComprobante.CodigoSeguridad = Uri.EscapeDataString(pComprobante.CodigoSeguridad);
                    string informacion = link + "?" + pComprobante.RucEmisor + "," + pComprobante.TipoCFEInt + "," + pComprobante.SerieComprobante +
                        "," + pComprobante.NumeroComprobante + "," + monto + "," + fechaFormateada.ToString("dd/MM/yyyy") + "," + pComprobante.CodigoSeguridad;
                    var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                    var qrCode = qrEncoder.Encode(informacion);

                    var renderer = new GraphicsRenderer(new FixedModuleSize
                        (10, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                    using (var stream = new FileStream(rutaQ, FileMode.Create))
                        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                    resultado = true;
            }
            catch (Exception )
            {                
            }

            return resultado;
        }
       
    }
}
