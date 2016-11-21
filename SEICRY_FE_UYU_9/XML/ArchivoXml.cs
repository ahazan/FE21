using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.XML;
using System.IO;
using System.Xml.Linq;
using SEICRY_FE_UYU_9.Interfaz;

namespace SEICRY_FE_UYU_9.XML
{
    class ArchivoXml
    {        
        public ArchivoXml()
        {
        }

        public static string rutaXml = "";

        /// <summary>
        /// Metodo que genera un archivo Xml
        /// </summary>
        /// <returns></returns>
        public bool generarXml(CFE infoCFE, CAE infoCAE)
        {
            bool resultado = false;

            try
            {
                XDocument documentoXml = new XDocument(
                                            new XDeclaration("1.0", "UTF-8", string.Empty),
                                            new XElement("Comprobantes",
                                                new XElement("Comprobante",
                                                    new XElement("ruc", infoCFE.RucEmisor.ToString()),
                                                    new XElement("compania", infoCFE.NombreEmisor.ToString()),
                                                    new XElement("serie", infoCFE.SerieComprobante.ToString()),
                                                    new XElement("tipoCFE", infoCFE.TipoCFE.ToString()),
                                                    new XElement("numero", infoCFE.NumeroComprobante.ToString()),
                                                    new XElement("numeroCAE", infoCAE.NumeroAutorizacion.ToString()),
                                                    new XElement("vencimientoCAE", infoCAE.FechaVencimiento.ToString()),
                                                    new XElement("fechaEmision", infoCFE.FechaComprobante.ToString()),
                                                    new XElement("fechaFirma", infoCFE.FechaHoraFirma.ToString()),
                                                    new XElement("rangoDesde", infoCAE.NumeroDesde.ToString()),
                                                    new XElement("rangoHasta", infoCAE.NumeroHasta.ToString()),
                                                    new XElement("moneda", infoCFE.TipoModena.ToString()),
                                                    new XElement("tasaBasica", infoCFE.TasaBasicaIVA.ToString()),
                                                    new XElement("tasaMinima", infoCFE.TasaMinimaIVA.ToString()),
                                                    new XElement("montoTotal", infoCFE.TotalMontoTotal.ToString()),
                                                    new XElement("montoNoFacturable", infoCFE.MontoNoFacturable.ToString()),
                                                    new XElement("montoTotalPagar", infoCFE.MontoTotalPagar.ToString()),
                                                    new XElement("totalMontoNoGravado", infoCFE.TotalMontoNoGravado.ToString()),
                                                    new XElement("codigoSeguridad", infoCFE.CodigoSeguridad),
                                                    new XElement("IVATasaBAsica", infoCFE.TotalIVATasaBasica.ToString()),
                                                    new XElement("netoIVATasaBasica", infoCFE.TotalMontoNetoIVATasaBasica.ToString())
                                                            )
                                                         )
                                                     );

                if (!FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    rutaXml = RutasCarpetas.RutaCarpetaComprobantes + infoCFE.TipoCFEInt + infoCFE.SerieComprobante
                        + infoCFE.NumeroComprobante + ".xml";
                }
                else
                {
                    rutaXml = RutasCarpetas.RutaCarpetaContingenciaComprobantes + infoCFE.TipoCFEInt + infoCFE.SerieComprobante
                        + infoCFE.NumeroComprobante + ".xml";
                }
                documentoXml.Save(rutaXml);

                resultado = true;
            }
            catch (Exception)
            {
            }
            
            return resultado;
        }
    }
}
