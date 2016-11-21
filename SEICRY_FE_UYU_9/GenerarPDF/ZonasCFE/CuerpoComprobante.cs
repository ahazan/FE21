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

        private double totalPDF = 0;


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
            , string domicilioFiscal, Document factura, string docNum, DatosPDF datosPdf)
        {
            PdfPTable info = new PdfPTable(new float[] { 2f, 2f });

            info = CeldaNueva("RUT Emisor: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            info = CeldaNueva(pInfoComprobante.RucEmisor.ToString(), info, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            info = CeldaNueva("Tipo CFE:", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            if (pInfoComprobante.TipoCFEInt == 102 || pInfoComprobante.TipoCFEInt == 112)
            {
                info = CeldaNueva(pInfoComprobante.TipoCFE.ToString(), info, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }
            else
            {
                info = CeldaNueva(pInfoComprobante.TipoCFE.ToString(), info, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }

            info = CeldaNueva("Serie/Número: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            info = CeldaNueva(pInfoComprobante.SerieComprobante + "-" + pInfoComprobante.NumeroComprobante,
                info, Alineacion.Derecha, 0, TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            if (pInfoComprobante.TipoCFEInt != 182 && pInfoComprobante.TipoCFEInt != 282)
            {
                info = CeldaNueva("Forma de Pago: ", info, Alineacion.Izquierda, 0,
                        TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                info = CeldaNueva(pInfoComprobante.FormaPago.ToString(), info, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }

            DateTime fechaFormato = new DateTime();
            fechaFormato = Convert.ToDateTime(pInfoComprobante.FechaComprobante);

            info = CeldaNueva("Fecha de Emisión: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro, ColorB.Default);
            info = CeldaNueva(fechaFormato.ToString("dd/MM/yyyy"), info, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro, ColorB.Default);

            info = CeldaNueva("Moneda: ", info, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro, ColorB.Default);
            info = CeldaNueva(pInfoComprobante.TipoModena, info, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default,ColorC.Blanco,ColorC.Negro, ColorB.Default);

            if (pInfoComprobante.TipoCFEInt != 182 && pInfoComprobante.TipoCFEInt != 282) //Resguardo y Resguardo Cont
            {
                info = CeldaNueva("Condiciones de pago: ", info, Alineacion.Izquierda, 0,
                        TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                info = CeldaNueva(datosPdf.FormaPago, info, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }
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
            PdfPTable receptorInfo = new PdfPTable(new float[] { 2f, 0.30f, 1.70f, 2.70f });
            //PdfPTable receptorInfo = new PdfPTable(new float[] { 2.5f, 0.50f, 1.50f, 2.50f });
            receptorInfo.WidthPercentage = 100f;


            ObtenerEmpresaDatos(EmpresaDato);

            factura.Add(nL);
            
                PdfPCell nombreCia = new PdfPCell();
                nombreCia.Border = PdfPCell.NO_BORDER;
                nombreCia.HorizontalAlignment = Element.ALIGN_LEFT;
                Font letraMc = FontFactory.GetFont("Cambria", 12.7f, Font.BOLD, Color.GRAY);
                Chunk titulo = new Chunk("Ascensores Schindler S.A", letraMc);
                nombreCia.AddElement(titulo);
                receptorInfo.AddCell(nombreCia);

                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                if (ticket)
                {
                    if (comprobante.NumDocReceptor.Equals("00000000"))
                    {
                        ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                        string temp = manteUdoAdobe.ObtenerCiGenerico();
                        if (!temp.Equals(""))
                        {
                            comprobante.NumDocReceptorExtrangero = temp;
                        }
                    }
                    receptorInfo = CeldaNueva("Consumidor Final:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    receptorInfo = CeldaNueva("X", receptorInfo, Alineacion.Izquierda, 0,
                        TipoBordes.LateralDerechaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                }
                else
                {

                    /*receptorInfo = CeldaNueva("RUT Comprador:", receptorInfo, Alineacion.Izquierda, 0,
                        TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);*/
                    string docReceptor = "";
                    switch(comprobante.TipoDocumentoReceptorInt)
                    {
                        case 2:
                            docReceptor = "RUT Receptor:";
                            break;
                        case 3:
                            docReceptor = "C.I Receptor:";
                            break;
                        case 4:
                            docReceptor = "Otros:";
                            break;
                        case 5:
                            docReceptor = "Pasaporte Receptor:";
                            break;
                        case 6:
                            docReceptor = "DNI Receptor:";
                            break;
                        default:
                            docReceptor = "Documento Receptor:";
                            break;
                    }                    
                    receptorInfo = CeldaNueva(docReceptor, receptorInfo, Alineacion.Izquierda, 0,
                        TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    receptorInfo = CeldaNueva(comprobante.NumDocReceptor, receptorInfo, Alineacion.Izquierda, 0,
                        TipoBordes.LateralDerechaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                }
                PdfPCell datosCia = new PdfPCell();
                datosCia.Border = PdfPCell.NO_BORDER;
                datosCia.HorizontalAlignment = Element.ALIGN_LEFT;
                datosCia.Rowspan = 7;

                Font letraDatos = FontFactory.GetFont("Cambria", 8.45f, Font.BOLD, Color.GRAY);//new Font(Font.TIMES_ROMAN, 9f, Font.NORMAL);
                Font letraRoja = FontFactory.GetFont("Cambria", 9f, Font.UNDERLINE, Color.RED);
                Chunk dir = new Chunk("Edificio \"Torre el Gaucho\"", letraDatos);
                Chunk dir2 = new Chunk("Constituyente 1467 - Piso 18 of.1807", letraDatos);
                Chunk tel = new Chunk("Telefax: (598)2409 0614 - C.P.11200", letraDatos);
                Chunk nl = new Chunk(Chunk.NEWLINE);
                Chunk email = new Chunk("E-mail: ", letraDatos);
                Chunk email2 = new Chunk("schindler@adinet.com.uy",letraRoja);
                Chunk pais = new Chunk("Montevideo - Uruguay", letraDatos);
                
                /* Agregado 24.10.2016*/
                Chunk web = new Chunk("www.schindler.com.uy", letraDatos);
                /* Agregado 24.10.2016*/

                Phrase datosMaestros = new Phrase();
                datosMaestros.Leading = 10f;
                datosMaestros.Add(dir);
                datosMaestros.Add(nl);
                datosMaestros.Add(dir2);
                datosMaestros.Add(nl);
                datosMaestros.Add(tel);
                datosMaestros.Add(nl);
                datosMaestros.Add(email);
                datosMaestros.Add(email2);                
                datosMaestros.Add(nl);
                datosMaestros.Add(pais);
                
                /* Agregado 24.10.2016*/
                datosMaestros.Add(nl);
                datosMaestros.Add(web);
                /* Agregado 24.10.2016*/

                datosCia.AddElement(datosMaestros);
                receptorInfo.AddCell(datosCia);

                //receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    //TipoBordes.Vacia, 13, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                
                //receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                 //   TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva("Cliente:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.SocioNegocio + " - " + comprobante.NombreReceptor, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                //receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    //TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva("Nombre Fantasía:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.NombreExtranjero, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                //receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    //TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva("Dirección:", receptorInfo, Alineacion.Izquierda, 0,
                TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva(comprobante.DireccionReceptor, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                //receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 2,
                    //TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva("Ciudad - Departamento:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.Ciudad +" - "+datosPdf.Estado, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                //receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                  //  TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva("Dirección de Entrega:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.DireccionEntrega, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                        TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva("Email:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaUltima, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.Email, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralDerechaUltima, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                /*receptorInfo = CeldaNueva(EmpresaDato.Web, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default);*/
            /*}
            else
            {
                receptorInfo = CeldaNueva(EmpresaDato.Nombre, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva(Mensaje.pdfRUCCOMPRADOR, receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaPrimera, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

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
                    TipoBordes.LateralDerechaPrimera, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

                receptorInfo = CeldaNueva(EmpresaDato.NombreComercial, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("Cliente: ", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.SocioNegocio + " - " + comprobante.NombreReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

                receptorInfo = CeldaNueva(EmpresaDato.Direccion, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("Nombre Fantasía: ", receptorInfo, Alineacion.Izquierda, 0,
                TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.NombreExtranjero, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

                receptorInfo = CeldaNueva(EmpresaDato.Phone, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("Dirección: ", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva(comprobante.DireccionReceptor, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

                receptorInfo = CeldaNueva(EmpresaDato.E_Mail, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("Teléfono:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierda, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.Telefono, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerecha, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

                receptorInfo = CeldaNueva(EmpresaDato.Web, receptorInfo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("", receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva("Condiciones de Pago:", receptorInfo, Alineacion.Izquierda, 0,
                    TipoBordes.LateralIzquierdaUltima, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                receptorInfo = CeldaNueva(datosPdf.FormaPago, receptorInfo, Alineacion.Derecha, 0,
                    TipoBordes.LateralDerechaUltima, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            }*/

            PdfPTable recuadro = new PdfPTable(new float[] { 1 });
            recuadro.WidthPercentage = 100f;

            PdfPCell celda = new PdfPCell(receptorInfo);
            celda.Border = PdfPCell.NO_BORDER;
            recuadro.AddCell(celda);
            //recuadro.SpacingAfter = 10;
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
        public Document DetalleResguardo(Document resguardo, List<ResguardoPdf> listaResguardos, string fechaComprobante, CFE infoComprobante, CAE infoCAE)
        {
            int cantidadMaximaArticulos = 10, resguardosRegistrados = 0;
            double sumaRetenciones = 0;

            PdfPTable tablaResguardo = new PdfPTable(new float[] { 0.8f, 1f, 2f, 2f, 0.3f, 1.4f });
            tablaResguardo.WidthPercentage = 100f;

            tablaResguardo = CeldaNueva("FACTURA", tablaResguardo, Alineacion.Centrado, 2, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("MONTOIMPONIBLE", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 2,
                TipoLetra.Bold, AlineacionVertical.Centrado, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("IMPUESTO", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 2,
                TipoLetra.Bold, AlineacionVertical.Centrado, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("RETENCION", tablaResguardo, Alineacion.Centrado, 2, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("NÚMERO", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("FECHA", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("%", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("IMPORTE", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            int cont = 0;
            string StrFecha = "";

            foreach (ResguardoPdf facturaResguardo in listaResguardos)
            {
                sumaRetenciones += double.Parse(facturaResguardo.ImporteRetencion);

                tablaResguardo = CeldaNueva(facturaResguardo.NumeroFactura, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.LateralIzquierda, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                if (cont == 0)
                {
                    DateTime fechaFormato = new DateTime();
                    fechaFormato = new DateTime();
                    fechaFormato = Convert.ToDateTime(facturaResguardo.FechaFactura);
                    StrFecha = fechaFormato.ToString("dd/MM/yyyy");

                }

                facturaResguardo.FechaFactura = StrFecha;

                tablaResguardo = CeldaNueva(facturaResguardo.FechaFactura, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaResguardo = CeldaNueva(facturaResguardo.MontoImponible, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaResguardo = CeldaNueva(facturaResguardo.Impuesto, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaResguardo = CeldaNueva(facturaResguardo.PorcentajeRetencion, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaResguardo = CeldaNueva(facturaResguardo.ImporteRetencion, tablaResguardo, Alineacion.Centrado, 0,
                    TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                cont++;
                resguardosRegistrados++;
            }

            while (resguardosRegistrados < cantidadMaximaArticulos)
            {
                if (resguardosRegistrados == (cantidadMaximaArticulos - 1))
                {
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.LateralIzquierdaUltima,
                        9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 4, TipoBordes.UltimaFila,
                        9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima,
                        9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                }
                else
                {
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0,
                        TipoBordes.LateralIzquierda, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 4,
                        TipoBordes.Vacia, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 0,
                        TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                }
                resguardosRegistrados++;
            }

            tablaResguardo.SpacingAfter = 10;


            tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 6, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva(" ", tablaResguardo, Alineacion.Centrado, 3, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva("TOTAL " + infoComprobante.TipoModena, tablaResguardo, Alineacion.Derecha, 0, TipoBordes.Vacia, 10,
                0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaResguardo = CeldaNueva(sumaRetenciones.ToString(), tablaResguardo, Alineacion.Centrado, 2,
                TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            resguardo.Add(tablaResguardo);

            /*   */
            PdfPTable totales = new PdfPTable(new float[] { 1.25f, 3.75f, 0.75f, 2f, 1.25f });
            totales.WidthPercentage = 100f;
            totales.SpacingBefore = 0;
            totales.SpacingAfter = 10;
            totales = AgregarQRResguardo(totales, sumaRetenciones, infoComprobante, infoCAE);

            PdfPTable adendaTotales = new PdfPTable(new float[] { 6f, 0.1f, 2f });
            adendaTotales.WidthPercentage = 100f;
            adendaTotales.SpacingBefore = 0;
            adendaTotales = AgregarAdendaResguardo(adendaTotales, infoComprobante.TextoLibreAdenda);

            resguardo.Add(totales);
            resguardo.Add(adendaTotales);
            /*    */

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
            string nombreTabla, DatosPDF datosPdf, CFE infoComprobante, CAE infoCAE)
        {
            int cantidadMaximaArticulos = 12, articulosRegistrado = 0, resultado = 0;

            totalPDF = 0;

            //if (infoComprobante.Items.Count > 37)
            //{
            //    cantidadMaximaArticulos = infoComprobante.Items.Count;
            //}

            //PdfPTable mercaderia = new PdfPTable(new float[] { 0.80f, 0.20f, 1f, 0.5f, 0.5f, 0.25f, 0.75f, 0.50f,
            //0.75f, 1f, 0.75f, 1f, 1f });

            PdfPTable mercaderia = new PdfPTable(new float[] { 1.25f, 4f, 0.75f, 1f, 0.75f, 1.25f });

            mercaderia.WidthPercentage = 100f;
            mercaderia.SpacingAfter = 0;

            if (tipoDetalle == 0)
            {
                mercaderia = CeldaNueva("Código", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                    TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaArribaVerde);
                mercaderia = CeldaNueva("Descripción", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10,
                    0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaArribaVerde);
                mercaderia = CeldaNueva(Mensaje.pdfCantidad, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaArribaVerde);
                mercaderia = CeldaNueva(Mensaje.pdfPrecioUnitario, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaArribaVerde);
                mercaderia = CeldaNueva("%Desc", mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa, 10, 0,
                    TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaArribaVerde);              
                mercaderia = CeldaNueva(Mensaje.pdfPrecioFinal, mercaderia, Alineacion.Centrado, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaArribaVerde);

                foreach (CFEItems lineaProducto in infoComprobante.Items)
                {

                    string codigoPack = ObtenerPackCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                    if (codigoPack != "I")
                    {
                        
                            
                        //if (articulosRegistrado == (infoComprobante.Items.Count - 1) && articulosRegistrado == 37)
                        //{
                        if ((articulosRegistrado == (infoComprobante.Items.Count - 1) && articulosRegistrado >= 12) || articulosRegistrado == 12)
                        {
                            string codigo = ObtenerCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                            mercaderia = CeldaNueva(codigo, mercaderia, Alineacion.Izquierda, 0, TipoBordes.LateralIzquierdaUltima, 10, 0,
                                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                            if (lineaProducto.DescripcionItem.Length < 40)
                            {
                                mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 0,
                                    TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                            }
                            else
                            {
                                mercaderia = CeldaNueva(lineaProducto.DescripcionItem.Substring(0, 40), mercaderia, Alineacion.Izquierda, 0,
                                    TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                            }

                            mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.CantidadItem.ToString()), mercaderia,
                            Alineacion.Derecha, 0, TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                            
                                string precioAntesDescuento = ObtenerPrecioAntesDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                mercaderia = CeldaNueva(SeparadorMiles(precioAntesDescuento), mercaderia, Alineacion.Derecha, 0,
                                    TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                                string porcentajeDescuento = ObtenerPorcentajeDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                mercaderia = CeldaNueva(SeparadorMiles(porcentajeDescuento), mercaderia, Alineacion.Derecha, 0,
                                    TipoBordes.UltimaFila, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                                                                mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF.ToString()), mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                                articulosRegistrado++;

                                totalPDF += lineaProducto.MontoItemPDF;
                        }
                        else
                        {
                            if (articulosRegistrado == 11)
                            {

                                string codigo = ObtenerCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                mercaderia = CeldaNueva(codigo, mercaderia, Alineacion.Izquierda, 0, TipoBordes.LateralIzquierdaUltima, 10, 0,
                                    TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

                                /*if (lineaProducto.DescripcionItem.Length < 60)
                                {*/

                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.CajaAbajo, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaAbajo);
                                /*}
                                else
                                {
                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem.Substring(0, 40), mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.UltimaFila, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                                }*/
                                mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.CantidadItem.ToString()), mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.CajaAbajo, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaAbajo);


                                                                    string precioAntesDescuento = ObtenerPrecioAntesDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                    mercaderia = CeldaNueva(SeparadorMiles(precioAntesDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.CajaAbajo, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaAbajo);

                                    string porcentajeDescuento = ObtenerPorcentajeDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                    mercaderia = CeldaNueva(SeparadorMiles(porcentajeDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.CajaAbajo, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.CajaAbajo);                                   

                                    mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF.ToString()), mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                                    articulosRegistrado++;

                                    totalPDF += lineaProducto.MontoItemPDF;
                            }
                            else
                            {
                                string codigo = ObtenerCodigo(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);
                                mercaderia = CeldaNueva(codigo, mercaderia, Alineacion.Izquierda, 0, TipoBordes.Laterales, 10, 0,
                                    TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerdeNegro);

                                /*if (lineaProducto.DescripcionItem.Length < 60)
                                {*/

                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                                /*}
                                else
                                {
                                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem.Substring(0, 40), mercaderia, Alineacion.Izquierda, 0,
                                        TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.LateralVerde);
                                }*/
                                mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.CantidadItem.ToString()), mercaderia,
                                    Alineacion.Derecha, 0, TipoBordes.LateralDerecha, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);


                                     string precioAntesDescuento = ObtenerPrecioAntesDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                    mercaderia = CeldaNueva(SeparadorMiles(precioAntesDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.LateralDerecha, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);

                                    string porcentajeDescuento = ObtenerPorcentajeDescuento(infoComprobante.DocumentoSAP, lineaProducto.LineNum.ToString(), nombreTabla);

                                    mercaderia = CeldaNueva(SeparadorMiles(porcentajeDescuento), mercaderia, Alineacion.Derecha, 0,
                                        TipoBordes.LateralDerecha, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                                    

                                    mercaderia = CeldaNueva(SeparadorMiles(lineaProducto.MontoItemPDF.ToString()), mercaderia,
                                        Alineacion.Derecha, 0, TipoBordes.LateralDerecha, 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                                    articulosRegistrado++;

                                    totalPDF += lineaProducto.MontoItemPDF;
                            }
                        }

                    }
                }
                while (articulosRegistrado < cantidadMaximaArticulos)
                {
                    if (articulosRegistrado == (cantidadMaximaArticulos - 1))
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.CajaAbajo,
                            9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerdeNegro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerechaUltima,
                            9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    }
                    else
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.Laterales, 9,
                            0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerdeNegro);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerecha, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerecha, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerecha, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerecha, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.LateralVerde);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 0, TipoBordes.LateralDerecha, 9,
                            0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                    }
                    articulosRegistrado++;
                }
            }
            else
            {
                #region SERVICIO

                mercaderia = CeldaNueva(Mensaje.pdfDetalleMercaderia, mercaderia, Alineacion.Izquierda, 6,
                    TipoBordes.Completa, 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                mercaderia = CeldaNueva(Mensaje.pdfPrecioFinal, mercaderia, Alineacion.Izquierda, 0, TipoBordes.Completa,
                    10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

                foreach (CFEItems lineaProducto in infoComprobante.Items)
                {
                    mercaderia = CeldaNueva(lineaProducto.DescripcionItem, mercaderia, Alineacion.Izquierda, 6,
                        TipoBordes.LateralIzquierda, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                    mercaderia = CeldaNueva(lineaProducto.PrecioUnitarioItem.ToString(), mercaderia, Alineacion.Derecha,
                        0, TipoBordes.LateralDerecha, 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                    articulosRegistrado++;
                }

                while (articulosRegistrado < cantidadMaximaArticulos)
                {
                    if (articulosRegistrado == (cantidadMaximaArticulos - 1))
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Izquierda, 7, TipoBordes.UltimaFila, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                    }
                    else
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Izquierda, 6, TipoBordes.LateralIzquierda, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Derecha, 0, TipoBordes.LateralDerechaUltima, 9, 0,
                            TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                    }
                } 
                #endregion
            }

            /*if (ValidarFinHoja(articulosRegistrado, cantidadMaximaArticulos, out resultado))
            {
                int diferencia = resultado - articulosRegistrado, p = 0;
                if (diferencia < 14)
                {
                    while (p < diferencia)
                    {
                        mercaderia = CeldaNueva(" ", mercaderia, Alineacion.Centrado, 7, TipoBordes.Vacia, 9,
                               0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                        p++;                        
                    }
                    PdfPCell relleno = new PdfPCell();
                    relleno.FixedHeight = 15;
                    relleno.Border = PdfCell.NO_BORDER;
                    relleno.Colspan = 7;

                    mercaderia.AddCell(relleno);
                }
            }*/

            //PdfPTable totales = new PdfPTable(new float[] { 0.8f, 1.7f, 1.5f, 1f, 1f, 2f, 1f });
            PdfPTable totales = new PdfPTable(new float[] { 1.25f,3.75f, 0.75f, 2f, 1.25f });
            totales.WidthPercentage = 100f;
            totales.SpacingBefore = 0;
            totales.SpacingAfter = 10;
            totales = AgregarTotales(totales, infoComprobante, kilosFactura, datosPdf, infoCAE);

            PdfPTable adendaTotales = new PdfPTable(new float[] { 6f, 0.1f, 2f });
            //PdfPTable adendaTotales = new PdfPTable(new float[] { 3.25f, 0.75f, 2f, 1f, 1f, 1f });
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

            
                tablaFinal = CeldaNueva("ADENDA", tablaFinal, Alineacion.Centrado, 0, TipoBordes.CajaArriba
                                , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 2, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            
            tablaFinal = CeldaNueva(comprobante.TextoLibreAdenda, tablaFinal, Alineacion.Izquierda, 0, TipoBordes.CajaAbajo,
                10, 10, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("RECIBIDO", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.CajaArriba
                                , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("Firma:_____________________", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("Aclaración:_________________", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("C.I.:_______________________", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.CajaAbajo
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);


            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
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
            DatosPDF datosPdf, CAE infoCAE)
        {            
            double mExento = 0, mBasica = 0, mMinima = 0, porcentajeDescuento = 0, ivaMinima = 0, ivaBasica = 0;

            string strMoneda;

            //if (comprobante.TipoCambio != 1)
            if ( comprobante.TipoCFE == CFE.ESTipoCFECFC.EFacturaExportacion || comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemitoExportacion 
                 || comprobante.TipoCFE == CFE.ESTipoCFECFC.ERemitoExportacionContingencia || comprobante.TipoCFE == CFE.ESTipoCFECFC.NCEFacturaExportacion
                 || comprobante.TipoCFE == CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia || comprobante.TipoCFE == CFE.ESTipoCFECFC.NDEFacturaExportacion
                 || comprobante.TipoCFE == CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia )
            {
                /*mExento = comprobante.TotalMontoNoGravadoExtranjero;
                mMinima = comprobante.TotalMontoNetoIVATasaMinimaExtranjero ;
                mBasica = comprobante.TotalMontoNetoIVATasaBasicaExtranjero ;*/

                mExento = comprobante.TotalMontoExportacionAsimilados;
            }
            else if (comprobante.TipoCambio != 1)
            {
                mExento = comprobante.TotalMontoNoGravadoExtranjero;
                mMinima = comprobante.TotalMontoNetoIVATasaMinimaExtranjero;
                mBasica = comprobante.TotalMontoNetoIVATasaBasicaExtranjero;
            }
            else
            {
                mExento = comprobante.TotalMontoNoGravado;
                mMinima = comprobante.TotalMontoNetoIVATasaMinima;
                mBasica = comprobante.TotalMontoNetoIVATasaBasica;
            }
            string subtotalGravado = (mMinima + mBasica).ToString();

            tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaArticulos = CeldaNueva(" ", tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia, 9, 0,
                TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia,
                9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia,
                9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            string rangoCAE = infoCAE.Serie + "-" + infoCAE.NumeroDesde + "-" + infoCAE.NumeroHasta;

            //Instancia de la clase COdigo Qr
            CodigoQr.CodigoQr codigo = new CodigoQr.CodigoQr();

            //monto = monto.Replace(",",".");
            string monto = comprobante.MontoTotalPagar.ToString();
            monto = monto.Replace(",", ".");

            //Se genera el QR
            bool generoCodigo = codigo.generadorQR(Mensaje.pdfDirCodigoQrQas,
                comprobante, monto);

            Paragraph contCod;
            //Se valida si se genero el codigo
            if (generoCodigo)
            {
                //Se agrega el codigo
                contCod = CodigoQr(comprobante.CodigoSeguridad);
            }
            else
            {
                contCod = new Paragraph();
            }
            
            //Se agrega el parrafo a la celda
            PdfPCell qr = new PdfPCell(contCod);
            qr.Rowspan = 7;
            qr.Border = PdfPCell.TOP_BORDER + PdfPCell.LEFT_BORDER + PdfPCell.BOTTOM_BORDER;
                                    
            tablaArticulos.AddCell(qr);
            tablaArticulos = CeldaNueva(Mensaje.pdfCodigoSeguridad, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerechaPrimera
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
                
            tablaArticulos = CeldaNueva("Subtotal Neto", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            #region FE_EXPORTACION
            /*tablaArticulos = CeldaNueva(SeparadorMiles((comprobante.TotalMontoNetoIVATasaBasica + comprobante.TotalMontoNetoIVATasaMinima + comprobante.TotalMontoNoGravado).ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.CajaArriba, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);*/
            if (comprobante.TipoCFE.ToString().Contains("Expor"))
            {
                /*tablaArticulos = CeldaNueva(SeparadorMiles((comprobante.TotalMontoExportacionAsimilados).ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.CajaArriba, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);*/

                tablaArticulos = CeldaNueva(SeparadorMiles(totalPDF.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.CajaArriba, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }
            else
            {
                /*tablaArticulos = CeldaNueva(SeparadorMiles((comprobante.TotalMontoNetoIVATasaBasica + comprobante.TotalMontoNetoIVATasaMinima + comprobante.TotalMontoNoGravado).ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.CajaArriba, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);*/

                tablaArticulos = CeldaNueva(SeparadorMiles(totalPDF.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.CajaArriba, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }
            #endregion FE_EXPORTACION

            tablaArticulos = CeldaNueva(Mensaje.pdfRes + comprobante.NumeroResolucion, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            string strDto = "Descuento %";
            if (datosPdf.PorcentajeDescuento > 0)
            {
                strDto = "Descuento " + Math.Round(datosPdf.PorcentajeDescuento, 2).ToString() + "%";
            }

            /*tablaArticulos = CeldaNueva("Descuento %", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaArticulos = CeldaNueva(SeparadorMiles(datosPdf.PorcentajeDescuento.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Completa, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);*/

            tablaArticulos = CeldaNueva(strDto, tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            if (comprobante.TipoModena.Equals("UYU"))
            {
                tablaArticulos = CeldaNueva(SeparadorMiles(Math.Round(datosPdf.DescuentoGeneral,2).ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Completa, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }
            else
            {
                tablaArticulos = CeldaNueva(SeparadorMiles(Math.Round(datosPdf.DescuentoExtranjero, 2).ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Completa, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }

            tablaArticulos = CeldaNueva(Mensaje.pdfLinkDGI, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaArticulos = CeldaNueva("Subtotal Exento", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva(SeparadorMiles(mExento.ToString()), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Completa, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfIVADia, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva("Subtotal Gravado", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva(SeparadorMiles(subtotalGravado), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Completa, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfNumCAE + infoCAE.NumeroAutorizacion, tablaArticulos, Alineacion.Izquierda , 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro,ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            if (comprobante.TipoCambio != 1)
            {
                ivaMinima = comprobante.TotalIVATasaMinimaExtranjero;
                ivaBasica = comprobante.TotalIVATasaBasicaExtranjero;
            }
            else
            {
                ivaMinima = comprobante.TotalIVATasaMinima;
                ivaBasica = comprobante.TotalIVATasaBasica;
            }
            string iva = (ivaMinima + ivaBasica).ToString();

            string tasaIVA;
            if (ivaMinima > 0)
            {
                tasaIVA = "IVA " + Math.Round(comprobante.TasaMinimaIVA,0).ToString() + "%";
            }
            else if (ivaBasica > 0)
            {
                tasaIVA = "IVA " + Math.Round(comprobante.TasaBasicaIVA, 0).ToString() + "%";
            }
            else
            {
                tasaIVA = "IVA";
            }

            tablaArticulos = CeldaNueva(tasaIVA, tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva(SeparadorMiles(iva), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Completa, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfRanAut + rangoCAE, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        ,10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            //if (comprobante.TipoCambio != 1)
            //{
            //    double montoTotalPago = double.Parse(datosPdf.MontoTotalPagar);
            //    double tipoCambioS = comprobante.TipoCambio;
            //    double montoTotalPagarF = montoTotalPago / tipoCambioS;

                
            //    tablaArticulos = CeldaNueva("Total a Pagar " + comprobante.TipoModena, tablaArticulos, Alineacion.Derecha, 0,
            //    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            //    tablaArticulos = CeldaNueva(SeparadorMiles(Math.Round(montoTotalPagarF, 2).ToString()), tablaArticulos, Alineacion.Derecha, 0,
            //        TipoBordes.CajaAbajo, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            //}
            //else
            //{
            //     tablaArticulos = CeldaNueva("Total a Pagar " + comprobante.TipoModena, tablaArticulos, Alineacion.Derecha, 0,
            //    TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            //    tablaArticulos = CeldaNueva(SeparadorMiles(datosPdf.MontoTotalPagar), tablaArticulos, Alineacion.Derecha, 0,
            //        TipoBordes.CajaAbajo, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            //}            

            tablaArticulos = CeldaNueva("Redondeo", tablaArticulos, Alineacion.Derecha, 0,
            TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva(SeparadorMiles(datosPdf.Redondeo), tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.CajaAbajo, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfFechaVencimiento + infoCAE.FechaVencimiento, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerechaUltima
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            if (comprobante.TipoCambio != 1)
            {
                /*double montoTotalPago = double.Parse(datosPdf.MontoTotalPagar);
                double tipoCambioS = comprobante.TipoCambio;
                double montoTotalPagarF = montoTotalPago / tipoCambioS;


                tablaArticulos = CeldaNueva("Total a Pagar " + comprobante.TipoModena, tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);                
                tablaArticulos = CeldaNueva(SeparadorMiles(Math.Round(montoTotalPagarF, 2).ToString()), tablaArticulos, Alineacion.Derecha, 0,
                    TipoBordes.CajaAbajo, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);*/

                tablaArticulos = CeldaNueva("Total a Pagar " + comprobante.TipoModena, tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaArticulos = CeldaNueva(SeparadorMiles(datosPdf.MontoTotalPagarPesos), tablaArticulos, Alineacion.Derecha, 0,
                    TipoBordes.CajaAbajo, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }
            else
            {
                tablaArticulos = CeldaNueva("Total a Pagar " + comprobante.TipoModena, tablaArticulos, Alineacion.Derecha, 0,
               TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
                tablaArticulos = CeldaNueva(SeparadorMiles(datosPdf.MontoTotalPagar), tablaArticulos, Alineacion.Derecha, 0,
                    TipoBordes.CajaAbajo, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            }            
            
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);                    

            return tablaArticulos;
        }


        private PdfPTable AgregarAdendaResguardo(PdfPTable tablaFinal, string textoAdenda)
        {

            tablaFinal = CeldaNueva("ADENDA", tablaFinal, Alineacion.Centrado, 0, TipoBordes.CajaArriba
                            , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 2, TipoBordes.Vacia
                            , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaFinal = CeldaNueva(textoAdenda, tablaFinal, Alineacion.Izquierda, 0, TipoBordes.CajaAbajo,
                10, 10, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("RECIBIDO", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.CajaArriba
                                , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("Firma:_____________________", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("Aclaración:_________________", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("C.I.:_______________________", tablaFinal, Alineacion.Izquierda, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Laterales
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.CajaAbajo
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            tablaFinal = CeldaNueva("", tablaFinal, Alineacion.Centrado, 0, TipoBordes.Vacia
                                , 10, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);
            return tablaFinal;
        }

        private PdfPTable AgregarQRResguardo(PdfPTable tablaArticulos, double sumaRetenciones, CFE comprobante, CAE infoCAE)
        {

            string rangoCAE = infoCAE.Serie + "-" + infoCAE.NumeroDesde + "-" + infoCAE.NumeroHasta;

            //Instancia de la clase COdigo Qr
            CodigoQr.CodigoQr codigo = new CodigoQr.CodigoQr();

            //monto = monto.Replace(",",".");
            string monto = sumaRetenciones.ToString();
            monto = monto.Replace(",", ".");

            //Se genera el QR
            bool generoCodigo = codigo.generadorQR(Mensaje.pdfDirCodigoQrQas,
                comprobante, monto);

            Paragraph contCod;
            //Se valida si se genero el codigo
            if (generoCodigo)
            {
                //Se agrega el codigo
                contCod = CodigoQr(comprobante.CodigoSeguridad);
            }
            else
            {
                contCod = new Paragraph();
            }

            //Se agrega el parrafo a la celda
            PdfPCell qr = new PdfPCell(contCod);
            qr.Rowspan = 7;
            qr.Border = PdfPCell.TOP_BORDER + PdfPCell.LEFT_BORDER + PdfPCell.BOTTOM_BORDER;

            tablaArticulos.AddCell(qr);
            tablaArticulos = CeldaNueva(Mensaje.pdfCodigoSeguridad, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerechaPrimera
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
            TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfRes + comprobante.NumeroResolucion, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
            TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfLinkDGI, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Gris, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfIVADia, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfNumCAE + infoCAE.NumeroAutorizacion, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfRanAut + rangoCAE, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerecha
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Centrado, 0, TipoBordes.Vacia
                        , 9, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
            TipoBordes.Vacia, 11, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);

            tablaArticulos = CeldaNueva(Mensaje.pdfFechaVencimiento + infoCAE.FechaVencimiento, tablaArticulos, Alineacion.Izquierda, 0, TipoBordes.LateralDerechaUltima
                        , 10, 0, TipoLetra.Bold, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);
            tablaArticulos = CeldaNueva("", tablaArticulos, Alineacion.Derecha, 0,
                TipoBordes.Vacia, 11, 0, TipoLetra.Normal, AlineacionVertical.Default, ColorC.Blanco, ColorC.Negro, ColorB.Default);                    

            return tablaArticulos;
        }

        /// <summary>
        /// Metodo para crear celdas/Alineacion: 0 = derecha, 1 = izquierda, 2 = centrado
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        private PdfPTable CeldaNueva(string contenido, PdfPTable tabla, Alineacion alinear, int colSpan, TipoBordes bordes,
    int tamLetra, int rowSpan, TipoLetra letra, AlineacionVertical alinearVertical, ColorC colorFondo, ColorC colorLetra, ColorB colorBorde)
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
                else if (colorLetra == ColorC.Negro)
                {
                    times = new Font(letraTimes, tamLetra, Font.NORMAL, Color.BLACK);
                }
                else if (colorLetra == ColorC.Gris)
                {
                    times = new Font(letraTimes, tamLetra, Font.NORMAL, Color.GRAY);
                }
                else if (colorLetra == ColorC.GrisClaro)
                {
                    times = new Font(letraTimes, tamLetra, Font.NORMAL, Color.LIGHT_GRAY);
                }
            }
            else if (letra == TipoLetra.Bold)
            {
                if (colorLetra == ColorC.Blanco)
                {
                    times = new Font(letraTimes, tamLetra, Font.BOLD, Color.WHITE);
                }
                else if (colorLetra == ColorC.Negro)
                {
                    times = new Font(letraTimes, tamLetra, Font.BOLD, Color.BLACK);
                }
                else if (colorLetra == ColorC.Gris)
                {
                    times = new Font(letraTimes, tamLetra, Font.BOLD, Color.GRAY);
                }
                else if (colorLetra == ColorC.GrisClaro)
                {
                    times = new Font(letraTimes, tamLetra, Font.BOLD, Color.LIGHT_GRAY);
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
            else if(bordes == TipoBordes.Laterales)
            {
                resultado.Border = PdfCell.LEFT_BORDER + PdfCell.RIGHT_BORDER;
            }

            #region COLORES


            if (colorBorde == ColorB.CajaArribaVerde)
            {
                resultado.UseVariableBorders = true;
                resultado.BorderColorLeft = new Color(176,204,176);
                resultado.BorderColorRight = new Color(176, 204, 176);
                resultado.BorderColorTop = new Color(176, 204, 176);                
            }
            else if(colorBorde == ColorB.LateralVerde)
            {
                resultado.UseVariableBorders = true;
                resultado.BorderColorRight = new Color(176, 204, 176);                
            }
            else if (colorBorde == ColorB.LateralNegroVerde)
            {
                resultado.UseVariableBorders = true;
                resultado.BorderColorRight = Color.BLACK;
                resultado.BorderColorLeft = new Color(176, 204, 176);
            }
            else if (colorBorde == ColorB.LateralVerdeNegro)
            {
                resultado.UseVariableBorders = true;
                resultado.BorderColorLeft = Color.BLACK;
                resultado.BorderColorRight = new Color(176, 204, 176);
            }
            else if (colorBorde == ColorB.CajaAbajo)
            {
                resultado.UseVariableBorders = true;
                resultado.BorderColorLeft = new Color(176, 204, 176);
                resultado.BorderColorRight = new Color(176, 204, 176);
                resultado.BorderColorBottom = Color.BLACK;
            }


            if (colorFondo == ColorC.Negro)
            {
                resultado.BackgroundColor = Color.BLACK;
            }
            else if (colorFondo == ColorC.Gris)
            {
                resultado.BackgroundColor = Color.GRAY;
            }
            else if (colorFondo == ColorC.GrisClaro)
            {
                resultado.BackgroundColor = Color.LIGHT_GRAY;
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
        #region COMENTADO
        ///// Obtiene el Objeto Empresa Datos
        ///// </summary>
        ///// <param name="E_Mail"></param>
        ///// <param name="Phone1"></param>
        ///// <param name="direccion"></param>
        /////  <param name="Web"></param>
        /////  <param name="Nombre"></param>
        /////   <param name="NombreC"></param>
        ///// <returns></returns>
        //private EmpresaDatos ObtenerEmpresaDatos(EmpresaDatos Empresa)
        //{
        //    string consulta = string.Empty;
        //    Recordset registro = null;

        //    try
        //    {
        //        registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
        //        consulta = "select E_Mail,  concat('Tel:' , Phone1) as Phone1, concat(Street, ' ' , StreetNo,' - ' , City) as direccion, IntrntAdrs , U_Nombre, U_NombreC FROM OADM a inner join ADM1 b on a.Code = b.Code inner join [@TFEEMI] c on c.DocEntry = a.Code";
        //        registro.DoQuery(consulta);

        //        if (registro.RecordCount > 0)
        //        {
        //            Empresa.E_Mail = registro.Fields.Item("E_Mail").Value + "";
        //            Empresa.Phone = registro.Fields.Item("Phone1").Value + "";
        //            Empresa.Direccion = registro.Fields.Item("direccion").Value + "";
        //            Empresa.Web = registro.Fields.Item("IntrntAdrs").Value + "";
        //            Empresa.Nombre = registro.Fields.Item("U_Nombre").Value + "";
        //            Empresa.NombreComercial = registro.Fields.Item("U_NombreC").Value + "";
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        if (registro != null)
        //        {
        //            //Libera de memoria al objeto registro
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
        //            GC.Collect();
        //        }
        //    }

        //    return Empresa;
        //} 
        #endregion
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



                consulta = "select E_Mail, IntrntAdrs , U_Nombre, U_NombreC FROM OADM a inner join ADM1 b on a.Code = b.Code inner join [@TFEEMI] c on c.DocEntry = a.Code";
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
            CDerecha = 13,
            Laterales = 14
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
            Negro = 2,
            GrisClaro = 3
        }

        public enum ColorB
        {
            Default = 0,
            LateralVerde = 1,
            CajaArribaVerde = 2,
            LateralNegroVerde = 3,
            LateralVerdeNegro = 4,
            CajaAbajo = 5
        }
    }
}
