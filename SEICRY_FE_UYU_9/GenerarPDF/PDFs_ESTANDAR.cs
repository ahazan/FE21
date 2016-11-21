using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Firma_Digital;
using SEICRY_FE_UYU_9.Globales;
using System.Xml;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SEICRY_FE_UYU_9.GenerarPDF;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.MejoraTiempo;

namespace SEICRY_FE_UYU_9
{
    class PDFs
    {
        public static string nombreCompuesto;
        public static bool errorCertificado = false;
        public static bool errorCorreo = false;
        public static bool errorImprimir = false;
        public static bool errorFirma = false;
        public static string archivoSinFirmar = "";
        public static string archivoFirmado = "";
        public static string erroCorreo = "";
        private string rutaLogo = "";

        /// <summary>
        /// Variables contenedoras de informacion fiscal
        /// </summary>
        CFE infoComprobante = new CFE();
        CAE infoCAE = new CAE();

        private string kilosComprobante = string.Empty;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="_infoComprobante"></param>
        /// <param name="_infoCAE"></param>
        public PDFs(CFE _infoComprobante, CAE _infoCAE, string _kilosComprobante)
        {
            this.infoComprobante = _infoComprobante;
            this.infoCAE = _infoCAE;
            this.kilosComprobante = _kilosComprobante;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public PDFs()
        {
        }

        /// <summary>
        /// Metodo para crear un pdf para los documentos fiscales
        /// </summary>
        /// <returns></returns>
        #region Proceso_WebService
        //public bool CrearPDF(string tabla, DatosPDF datosPDF, List<ResguardoPdf> resguardoPdf, string TablaCabezal)
        #endregion Proceso_WebService
        public bool CrearPDF(string tabla, DatosPDF datosPDF, List<ResguardoPdf> resguardoPdf, string TablaCabezal, string origenFE = null)
        {
            bool resultado = false;
            int opcFactura = 0;

            //Nombre del archivo sin extension y ruta
            nombreCompuesto = infoComprobante.TipoCFEInt + infoComprobante.SerieComprobante +
                    infoComprobante.NumeroComprobante;


            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
            {
                //Nombre de Archivos con ruta para proceso de firma electronica
                archivoSinFirmar = RutasCarpetas.RutaCarpetaContingenciaComprobantes + nombreCompuesto + "sf.pdf";
                archivoFirmado = RutasCarpetas.RutaCarpetaContingenciaComprobantes + nombreCompuesto + ".pdf";
            }
            else
            {
                //Nombre de Archivos con ruta para proceso de firma electronica
                archivoSinFirmar = RutasCarpetas.RutaCarpetaComprobantes + nombreCompuesto + "sf.pdf";
                archivoFirmado = RutasCarpetas.RutaCarpetaComprobantes + nombreCompuesto + ".pdf";
            }

            ManteUdoLogo mante = new ManteUdoLogo();
            rutaLogo = mante.Consultar(true);
            bool estadoAdenda = false;

            try
            {
                //Muestra cursor de espera
                Cursor.Current = Cursors.WaitCursor;
               Document comprobante = new Document(PageSize.A4, 10, 10, 100, 160);       //centenario         
               // Document comprobante = new Document(PageSize.A4, 10, 10, 230, 110);                //saint

                //Se crea el archivo que va contener el Pdf
                StreamWriter fac = File.CreateText(archivoSinFirmar);
                fac.Close();

                FileStream docCreado = new FileStream(archivoSinFirmar, FileMode.Open
                    , FileAccess.Write);

                //Se crea una instancia para escribir el contenido del pdf en el archivo fisico
                PdfWriter writer = PdfWriter.GetInstance(comprobante, docCreado);

                #region Encabezado y Pie de Pagina

                string domicilioFiscalEmisor = ObtenerDomicilioFiscalEmisor();

                //Se envia la informacion contenida en el encabezado de las paginas
                writer.PageEvent = new ZonasCFE.EventosPagina(infoComprobante, infoCAE, domicilioFiscalEmisor,
                    rutaLogo, datosPDF.DocNum); //, datosPDF);//saint

                #endregion Encabezado y Pie de Pagina

                //Se abre el documento pdf a crear
                comprobante.Open();

                ZonasCFE.CuerpoComprobante cuerpoComprobante = new ZonasCFE.CuerpoComprobante();

                bool ticket = ComprobarTicket(infoComprobante.TipoCFEInt);

                #region Receptor

                //Genera la zona de informacion del receptor de la factura
                comprobante = cuerpoComprobante.Receptor(infoComprobante, comprobante, writer, ticket,
                  datosPDF);

                #endregion Receptor

                #region Detalle Producto o Servicio

                if (infoComprobante.TipoCFEInt != 182 && infoComprobante.TipoCFEInt != 282)
                {
                    //Verificar el si la factura es para productos o servicios
                    if (infoComprobante.TipoDocumentoSAP.ToString().Equals("Servicio"))
                    {
                        opcFactura = 1;
                    }

                    //Comprueba si hay que agregar la adenda
                    if (Adenda())
                    {
                        estadoAdenda = true;
                    }

                    ///Genera el detalle de productos para la factura;                    
                    comprobante = cuerpoComprobante.DetalleMercaderia(comprobante, opcFactura, kilosComprobante,
                     tabla, datosPDF, infoComprobante, TablaCabezal);
                }
                else
                {
                    List<ResguardoPdf> listaResguardo = new List<ResguardoPdf>();


                    foreach (ResguardoPdf facturaResguardo in resguardoPdf)
                    {

                        facturaResguardo.FechaFactura = infoComprobante.FechaComprobante;

                        if (datosPDF.DescuentoGeneral > 0)
                        {
                            facturaResguardo.MontoImponible = (double.Parse(facturaResguardo.MontoImponible) - datosPDF.DescuentoGeneral).ToString();
                        }
                        else if (datosPDF.DescuentoExtranjero > 0)
                        {
                            facturaResguardo.MontoImponible = (double.Parse(facturaResguardo.MontoImponible) - datosPDF.DescuentoExtranjero).ToString();
                        }

                        listaResguardo.Add(facturaResguardo);

                    }

                    comprobante = cuerpoComprobante.DetalleResguardo(comprobante, listaResguardo, infoComprobante.FechaComprobante);
                }


                #endregion Detalle Producto o Servicio

                //Se cierran los documentos utilizados
                comprobante.Close();
                docCreado.Close();

                //Firma digitalmente al documento creado
                FirmaDigital firma = new FirmaDigital();

                ManteUdoCertificadoDigital cerDigital = new ManteUdoCertificadoDigital();

                //Se obtiene informacion del certificado digital
                string rutaCertificado = cerDigital.ObtenerRutaCertificado();
                string passCertificado = cerDigital.ObtenerPassCertificado();

                if (rutaCertificado.Equals("") || passCertificado.Equals(""))
                {
                    errorCertificado = true;
                }
                else
                {
                    if (firma.infoCertificado(archivoSinFirmar, archivoFirmado, rutaCertificado, passCertificado))
                    {
                        //Valida que exista la ruta a borrar
                        if (!archivoSinFirmar.Equals(""))
                        {
                            //Borra el archivo pdf sin la firma electronica
                            System.IO.File.Delete(PDFs.archivoSinFirmar);
                        }

                        if (!FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                        {
                            AccionesFueraHilo acciones = new AccionesFueraHilo();

                            #region Proceso_WebService
                            //acciones.Imprimir(archivoFirmado);
                            if (origenFE == null)
                            {
                                acciones.Imprimir(archivoFirmado);
                            }
                            else
                            {
                                string copiaArchivo = archivoFirmado.Replace(RutasCarpetas.RutaCarpetaComprobantes, RutasCarpetas.RutaCarpetaImpresion);
                                System.IO.File.Copy(archivoFirmado, copiaArchivo, false);
                                Program.colaImpresion.Enqueue(copiaArchivo);
                            }
                            #endregion Proceso_WebService

                            DatosCorreo datosCorreo = new DatosCorreo();
                            datosCorreo.CorreoReceptor = infoComprobante.CorreoReceptor;
                            datosCorreo.EstadoAdenda = estadoAdenda;
                            datosCorreo.NombreCompuesto = nombreCompuesto;
                            acciones.EjecutarCorreo(datosCorreo);
                            resultado = true;
                        }
                        else
                        {
                            resultado = true;
                        }
                    }
                    else
                    {
                        errorFirma = true;
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: CrearPDF/ " + ex.ToString());
            }
            finally
            {
                //Se quita el cursor de espera
                Cursor.Current = Cursors.AppStarting;
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el domicilio fiscal del emisor
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ObtenerDomicilioFiscalEmisor()
        {
            Recordset recSet = null;
            string consulta = "", direccion = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT Street, StreetNo, City FROM ADM1";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    direccion += recSet.Fields.Item("Street").Value + " ";
                    direccion += recSet.Fields.Item("StreetNo").Value + " ";
                    direccion += recSet.Fields.Item("City").Value + "";
                }
            }
            catch (Exception)
            {
                direccion = "Sin registrar";
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
            if (direccion.Equals(""))
            {
                direccion = "Sin registrar";
                return direccion;
            }
            else
            {
                return direccion.Replace("\r", " ");
            }
        }

        /// <summary>
        /// Determina que tipo de cuentas de correo utilizar
        /// </summary>
        /// <returns></returns>
        public bool TipoCorreo(bool estadoAdenda)
        {
            bool resultado = false;

            try
            {
                //Se obtiene credenciales para el envio del correo
                ManteUdoCorreos manteUdo = new ManteUdoCorreos();
                Correo correo = manteUdo.Consultar();

                if (correo != null)
                {
                    string[] adjuntos = new string[2];//2

                    //Se agregan las rutas de los archivos adjuntos
                    adjuntos[0] = RutasCarpetas.RutaCarpetaComprobantes + nombreCompuesto + Mensaje.pdfExt;
                    if (estadoAdenda)
                    {
                        adjuntos[1] = CambiarNombre(RutasCarpetas.RutaCarpetaSobresComprobantesAdenda + nombreCompuesto + ".xml");
                    }
                    else
                    {
                        adjuntos[1] = CambiarNombre(RutasCarpetas.RutaCarpetaSobres + nombreCompuesto + ".xml");
                    }

                    //0 == Gmail
                    if (correo.Opcion.Equals("0"))
                    {
                        ///Envia correo con una cuenta de gmail
                        Mail mail = new Mail(infoComprobante.CorreoReceptor, correo.Cuenta, nombreCompuesto,
                             Mensaje.pdfContenido, Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, adjuntos, 587);
                        if (mail.enviarCorreoGmail())
                        {
                            if (!adjuntos[1].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[1]);
                            }
                            resultado = true;
                        }
                        else
                        {
                            erroCorreo = Mail.errorCorreo;
                        }
                    }
                    //1 == Outlook
                    else if (correo.Opcion.Equals("1"))
                    {
                        ///Envia correo con una cuenta de outlook
                        Mail mail = new Mail(infoComprobante.CorreoReceptor, Mensaje.pdfAsunto, Mensaje.pdfContenido, adjuntos);
                        if (mail.enviarOutlook())
                        {
                            if (!adjuntos[1].Equals(""))
                            {
                                //Borra el archivo de sobre copiado para enviar en el correo
                                System.IO.File.Delete(adjuntos[1]);
                            }
                            resultado = true;
                        }
                        else
                        {
                            erroCorreo = Mail.errorCorreo;
                        }
                    }
                }
                else
                {
                    erroCorreo = Mensaje.errCorreoDatosConfigurados;
                }
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Metodo que comprueba si hay que agregar la adenda
        /// </summary>
        /// <returns></returns>
        private bool Adenda()
        {
            bool resultado = false;

            if (infoComprobante.TextoLibreAdenda != null)
            {
                ///Comprueba si la adenda fue agregada a la factura
                if (!infoComprobante.TextoLibreAdenda.Equals(""))
                {
                    resultado = true;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Determina si un Cfe es de tipo ticket
        /// </summary>
        /// <param name="tipoCfe"></param>
        /// <returns></returns>
        private bool ComprobarTicket(int tipoCfe)
        {
            bool salida = false;

            if (tipoCfe == 101 || tipoCfe == 102 || tipoCfe == 103 || tipoCfe == 131 || tipoCfe == 132 || tipoCfe == 133 ||
                tipoCfe == 201 || tipoCfe == 202 || tipoCfe == 203 || tipoCfe == 231 || tipoCfe == 132 || tipoCfe == 233)
            {
                salida = true;
            }

            return salida;
        }

        /// <summary>
        /// Cambia el nombre del sobre para enviarlo por correo
        /// </summary>
        /// <param name="nombreSobre"></param>
        /// <returns></returns>
        public string CambiarNombre(string nombreSobre)
        {
            string resultado = "";
            DateTime fechaActual;

            try
            {
                //Verifica si el archivo existe
                if (File.Exists(nombreSobre))
                {
                    //Obtiene la fecha actual
                    fechaActual = DateTime.Now;
                    //Formatea fecha de modo: YYYYMMDD
                    string fechaFormateada = String.Format("{0:yyyyMMdd}", fechaActual);
                    RucIdEmisor rucIdEmisor = ObtenerDatosXml(nombreSobre);

                    //Se crea el nombre del sobre segun formato de DGI
                    resultado = "SOB_" + rucIdEmisor.RucEmisor + "_" + fechaFormateada + "_" + rucIdEmisor.IdEmisor + ".xml";

                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                    {
                        resultado = RutasCarpetas.RutaCarpetaContingenciaSobresTemporales + resultado;
                    }
                    else
                    {
                        resultado = RutasCarpetas.RutaCarpetaSobresTemporales + resultado;
                    }
                    System.IO.File.Copy(nombreSobre, resultado, false);
                }
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene Ruc y Id del emisor del sobre(Xml)
        /// </summary>
        /// <param name="rutaXml"></param>
        /// <returns></returns>
        private RucIdEmisor ObtenerDatosXml(string rutaXml)
        {
            RucIdEmisor respuesta = new RucIdEmisor();
            XmlDocument documento = new XmlDocument();

            try
            {
                documento.Load(rutaXml);

                //Se obtiene el RucEmisor
                respuesta.RucEmisor = documento.GetElementsByTagName("DGICFE:RUCEmisor").Item(0).InnerText;

                //Se obtiene el IdEmisor
                respuesta.IdEmisor = documento.GetElementsByTagName("DGICFE:Idemisor").Item(0).InnerText;
            }
            catch (Exception)
            {
            }

            return respuesta;
        }
    }
}