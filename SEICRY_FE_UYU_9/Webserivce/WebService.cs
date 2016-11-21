using System;
using System.Collections.Generic;
using System.Collections;
using SEICRY_FE_UYU_9.Interfaz;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.DocumentosB1;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.XML;
using SEICRY_FE_UYU_9.Certificados.Xml.Transformacion;
using SEICRY_FE_UYU_9.Certificados.Xml.Serializacion;
using SEICRY_FE_UYU_9.ZonasCFE;
using SEICRY_FE_UYU_9.Globales;
using System.Text.RegularExpressions;
using SEICRY_FE_UYU_9.AbrirDialogo;
using System.Threading;
using System.Xml;
using System.IO;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SEICRY_FE_UYU_9.XSD;
using SEICRY_FE_UYU_9.GenerarPDF;
using SAPbobsCOM;

using System.Diagnostics;
using Microsoft.Win32;

namespace SEICRY_FE_UYU_9.WS
{
    class HiloWS
    {
        const string PROCESADO = "Procesado";
        const string ENPROCESO = "En Proceso";
        const string PENDIENTE = "Pendiente";
        const string FPAGOOINV = "U_U_FrmPagOIN";
        const string FPAGOORIN = "U_U_FrmPagORI";
        const string ADENDA = "U_Adenda";
        const string MOTREFND = "U_MotRefND";
        const string MOTREFNC = "U_MotRefNC";

        public volatile bool detenerHilo = false;

        //Instancias de mantenimientos
        private ManteUdoCAE manteUdoCae = new ManteUdoCAE();
        private ManteUdoCFE manteUdoCfe = new ManteUdoCFE();
        private ManteDocumentos manteDocumentos = new ManteDocumentos();


        private static string rutaCertificado;
        private static string claveCertificado;
        private static string url_envio;
        private static string url_consultas;

        //Variables de objetos
        private CFE cfe;
        private CAE cae;

        public void envioContinuo()
        {
            Thread threadEnvio = null;

            //Se define el hilo
            threadEnvio = new Thread(this.enviarPendientesWS);
            //Se pone como background para que se muera cuando se cierra la aplicacion
            threadEnvio.IsBackground = true;
            //Inicia el hilo
            threadEnvio.Start();
        }

        public void enviarPendientesWS()
        {
            while (!detenerHilo)
            {
                try
                {
                    this.procesarDocumentos("FACTURA");
                    this.procesarDocumentos("ND");
                    this.procesarDocumentos("NC");

                    //this.procesarDocumentos("REMITO");
                    //this.procesarDocumentos("RC"); //Resguardo Compra
                }
                catch (Exception ex)
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
                }
                finally
                {

                }
                //Se detiene el hilo por 1 segundos
                Thread.Sleep(5000);
            }
        }

        private void procesarDocumentos(string tipo)
        {
            int numero = 0;

            int milisegundos = 2000;

            string consulta = String.Empty;
            string formaPago = String.Empty;
            string adenda = String.Empty;
            string razonRef = String.Empty;

            Recordset oRecordSet = null;

            /* */
            string sCondition = "";
            string sTop = "";
            string sOrder = "";

            string sFiltroFecha = " AND CONVERT(date,DocDate) = CONVERT(date,GETDATE()) ";

            try
            {

                if (tipo == "FACTURA")
                {
                    /* Busco primero todas las facturas de WS */
                    sTop = "";
                    sCondition = " AND U_Origen = 'WS' AND U_Estado = '" + PENDIENTE + "' ";
                    sOrder = " ORDER BY DocNum ";

                    consulta = "SELECT " + sTop + " DocEntry," + FPAGOOINV + " AS FormaPAgo," + ADENDA + " AS Adenda" +
                                 " FROM " + DocumentoB1.TABLA_FACTURA + " WHERE ObjType = '13' AND DocSubType = '--'" + sFiltroFecha + sCondition + sOrder;

                    oRecordSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                    oRecordSet.DoQuery(consulta);

                    /* Proceso lo que tengo en espera */
                    while (!oRecordSet.EoF)
                    {
                        numero = oRecordSet.Fields.Item("DocEntry").Value;

                        FacturaB1 doc = new FacturaB1();
                        doc.ActualizarEstadoDocumento(numero, ENPROCESO, String.Empty);

                        formaPago = oRecordSet.Fields.Item("FormaPago").Value;
                        adenda = oRecordSet.Fields.Item("Adenda").Value;

                        this.enviarFactura(numero, formaPago, adenda);

                        Thread.Sleep(milisegundos);

                        oRecordSet.MoveNext();
                    }
                }
                else if (tipo == "ND") //Nota de Débito
                {
                    /* Busco primero todas las facturas de WS */
                    sTop = "";
                    sCondition = " AND U_Origen = 'WS' AND U_Estado = '" + PENDIENTE + "' ";
                    sOrder = " ORDER BY DocNum ";

                    consulta = "SELECT " + sTop + " DocEntry, " + FPAGOOINV + " AS FormaPago, " + ADENDA + " AS Adenda," + MOTREFND + " AS RazonRef" +
                               " FROM " + DocumentoB1.TABLA_NOTA_DEBITO + " WHERE ObjType = '13' AND DocSubType = 'DN' " + sFiltroFecha + sCondition + sOrder;

                    oRecordSet = null;
                    oRecordSet = ((SAPbobsCOM.Recordset)(ProcConexion.Comp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));

                    oRecordSet.DoQuery(consulta);

                    //if (oRecordSet.RecordCount <= 0) // Busco si hay ND DTW (Delivery)
                    //{
                    //    sTop = " TOP 10 ";
                    //    sCondition = " AND U_Origen = 'DTW' AND U_Estado = '" + PENDIENTE + "' ";
                    //    sOrder = " ORDER BY DocNum ";

                    //    consulta = "SELECT " + sTop + " DocEntry, " + FPAGOOINV + " AS FormaPago, " + ADENDA + " AS Adenda," + MOTREFND + " AS RazonRef" +
                    //           " FROM " + DocumentoB1.TABLA_NOTA_DEBITO + " WHERE ObjType = '13' AND DocSubType = 'DN' " + sFiltroFecha + sCondition + sOrder;

                    //    oRecordSet.DoQuery(consulta);
                    //}

                    while (!oRecordSet.EoF)
                    {
                        numero = oRecordSet.Fields.Item("DocEntry").Value;

                        NotaDebitoB1 doc = new NotaDebitoB1();
                        doc.ActualizarEstadoDocumento(numero, ENPROCESO, String.Empty);

                        formaPago = oRecordSet.Fields.Item("FormaPago").Value;
                        adenda = oRecordSet.Fields.Item("Adenda").Value;
                        razonRef = oRecordSet.Fields.Item("RazonRef").Value;
                        this.enviarND(numero, formaPago, adenda, razonRef);
                        oRecordSet.MoveNext();
                    }
                }
                else if (tipo == "NC") //Nota de Crédito
                {
                    /* Busco primero todas las NC de WS */
                    sTop = "";
                    sCondition = " AND U_Origen = 'WS' AND U_Estado = '" + PENDIENTE + "' ";
                    sOrder = " ORDER BY DocNum ";

                    consulta = "SELECT " + sTop + " DocEntry," + FPAGOORIN + " AS FormaPago," + ADENDA + " AS Adenda," + MOTREFNC + " AS RazonRef" +
                               " FROM [" + DocumentoB1.TABLA_NOTA_CREDITO + "] WHERE ObjType = '14' " + sFiltroFecha + sCondition + sOrder;

                    oRecordSet = null;
                    oRecordSet = ((SAPbobsCOM.Recordset)(ProcConexion.Comp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));

                    oRecordSet.DoQuery(consulta);

                    //if (oRecordSet.RecordCount <= 0) // Busco si hay NC DTW (Delivery)
                    //{
                    //    sTop = " TOP 10 ";
                    //    sCondition = " AND U_Origen = 'DTW' AND U_Estado = '" + PENDIENTE + "' ";
                    //    sOrder = " ORDER BY DocNum ";

                    //    consulta = "SELECT " + sTop + " DocEntry," + FPAGOORIN + " AS FormaPago," + ADENDA + " AS Adenda," + MOTREFNC + " AS RazonRef" +
                    //           " FROM [" + DocumentoB1.TABLA_NOTA_CREDITO + "] WHERE ObjType = '14' " + sFiltroFecha + sCondition + sOrder;

                    //    oRecordSet.DoQuery(consulta);
                    //}

                    while (!oRecordSet.EoF)
                    {
                        numero = oRecordSet.Fields.Item("DocEntry").Value;

                        NotaCreditoB1 doc = new NotaCreditoB1();
                        doc.ActualizarEstadoDocumento(numero, ENPROCESO, String.Empty);

                        formaPago = oRecordSet.Fields.Item("FormaPago").Value;
                        adenda = oRecordSet.Fields.Item("Adenda").Value;
                        razonRef = oRecordSet.Fields.Item("RazonRef").Value;
                        this.enviarNC(numero, formaPago, adenda, razonRef);
                        oRecordSet.MoveNext();
                    }


                }



                if (oRecordSet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                    GC.Collect();
                    oRecordSet = null;
                }


            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
            }
        }

        private void enviarFactura(int numeroDocumento, string formaPago, string adenda)
        {
            AdminEventosUI eventos = new AdminEventosUI(true);

            FacturaB1 facturaB1 = new FacturaB1();
            DatosPDF datosPdf = new DatosPDF();

            datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumento, "INV1", datosPdf);
            datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumento, "OINV", datosPdf);
            datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumento, "OINV");
            datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumento, "OINV");
            datosPdf.Titular = DocumentoB1.Titular(numeroDocumento, "OINV");
            datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
            datosPdf = DocumentoB1.ActualizarEstado(datosPdf);//Saint
            datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);//Saint  
            datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

            //Obtener el objeto cfe a partir de los datos del documento creado
            cfe = facturaB1.ObtenerDatosFactura(numeroDocumento, Objetos.CAE.ESTipoCFECFC.EFactura, formaPago, adenda);

            if (cfe != null)
            {
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    cae = manteUdoCae.Consultar("999", cfe.SerieComprobante);
                }
                else
                {
                    cae = manteUdoCae.Consultar(cfe.TipoCFEInt.ToString(), cfe.SerieComprobante);
                }

                //Actualizar datos del CFE en el documento creado
                facturaB1.ActualizarDatosCFEFActura(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante, cfe.NumeroComprobante.ToString());

                DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(), cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "13", "F");

                datosPdf.DescuentoGeneral = FacturaB1.descuentoGeneral;
                datosPdf.DescuentoExtranjero = FacturaB1.descuentoGeneralExtranjero;
                datosPdf.PorcentajeDescuento = FacturaB1.porcentajeDescuento;

                eventos.EnviarDocumento(cfe, cae, datosPdf, "INV1", null, "OINV");

                //Cambio el estado a Procesado para no volver a enviarlo
                facturaB1.ActualizarEstadoDocumento(numeroDocumento, PROCESADO, cfe.CodigoSeguridad);

                cfe = null;
                cae = null;

                //Valida que el documento sea un resguardo
                if (manteDocumentos.ValidarDocumentoResguardo(numeroDocumento, "INV5"))
                {
                    ResguardoB1 resguardoB1 = new ResguardoB1();

                    //Obtener el objeto cfe a partir de los datos del documento creado
                    cfe = resguardoB1.ObtenerDatosResguardo(numeroDocumento, Objetos.CAE.ESTipoCFECFC.ERemito);


                    List<ResguardoPdf> resguardoPdf = DocumentoB1.ObtenerResguardoPdf(numeroDocumento, "OINV", "INV1", "INV5");

                    if (cfe != null)
                    {
                        if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                        {
                            //Obtener el objeto cae utilizado en el documento creado
                            cae = manteUdoCae.Consultar("999", cfe.SerieComprobante);
                        }
                        else
                        {
                            //Obtener el objeto cae utilizado en el documento creado
                            cae = manteUdoCae.Consultar(cfe.TipoCFEInt.ToString(), cfe.SerieComprobante);
                        }

                        //Actualizar datos del CFE en el documento creado
                        resguardoB1.ActualizarDatosCFEResguardo(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante, cfe.NumeroComprobante.ToString());

                        datosPdf.DescuentoGeneral = ResguardoB1.descuentoGeneral; datosPdf.DescuentoExtranjero = ResguardoB1.descuentoGeneralExtranjero;

                        eventos.EnviarDocumento(cfe, cae, datosPdf, "INV1", resguardoPdf, "OINV");

                        //Cambio el estado a Procesado para no volver a enviarlo
                        resguardoB1.ActualizarEstadoDocumento(numeroDocumento, PROCESADO, cfe.CodigoSeguridad);

                        cfe = null;
                        cae = null;
                    }
                }
            }
        }

        private void enviarND(int numeroDocumento, string formaPago, string adenda, string razonRef)
        {
            AdminEventosUI eventos = new AdminEventosUI(true);

            NotaDebitoB1 notaDebitoB1 = new NotaDebitoB1();
            DatosPDF datosPdf = new DatosPDF();

            datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumento, "INV1", datosPdf);
            datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumento, "OINV", datosPdf);
            datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumento, "OINV");
            datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumento, "OINV");
            datosPdf.Titular = DocumentoB1.Titular(numeroDocumento, "OINV");
            datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
            datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
            datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
            datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

            //Obtener el objeto cfe a partir de los datos del documento creado
            cfe = notaDebitoB1.ObtenerDatosNotaDebito(numeroDocumento, Objetos.CAE.ESTipoCFECFC.NDEFactura, formaPago, adenda, razonRef);

            if (cfe != null)
            {
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Obtener el objeto cae utilizado en el documento creado
                    cae = manteUdoCae.Consultar("999", cfe.SerieComprobante);
                }
                else
                {
                    //Obtener el objeto cae utilizado en el documento creado
                    cae = manteUdoCae.Consultar(cfe.TipoCFEInt.ToString(), cfe.SerieComprobante);
                }

                //Actualizar datos del CFE en el documento creado
                notaDebitoB1.ActualizarDatosCFENotaDebito(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.InfoReferencia);

                DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(), cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "13", "ND");

                datosPdf.DescuentoGeneral = NotaDebitoB1.descuentoGeneral;
                datosPdf.DescuentoExtranjero = NotaDebitoB1.descuentoGeneralExtranjero;
                datosPdf.PorcentajeDescuento = NotaDebitoB1.porcentajeDescuento;
                eventos.EnviarDocumento(cfe, cae, datosPdf, "INV1", null, "OINV");

                //Cambio el estado a Procesado para no volver a enviarlo
                notaDebitoB1.ActualizarEstadoDocumento(numeroDocumento, PROCESADO, cfe.CodigoSeguridad);

                cfe = null;
                cae = null;
            }
        }

        private void enviarNC(int numeroDocumento, string formaPago, string adenda, string razonRef)
        {
            AdminEventosUI eventos = new AdminEventosUI(true);

            NotaCreditoB1 notaCreditoB1 = new NotaCreditoB1();
            DatosPDF datosPdf = new DatosPDF();

            datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumento, "RIN1", datosPdf);
            datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumento, "ORIN", datosPdf);
            datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumento, "ORIN");
            datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumento, "ORIN");
            datosPdf.Titular = DocumentoB1.Titular(numeroDocumento, "ORIN");
            datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
            datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
            datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
            datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

            datosPdf.FormaPago = formaPago;
            
            //Obtener el objeto cfe a partir de los datos del documento creado
            cfe = notaCreditoB1.ObtenerDatosNotaCredito(numeroDocumento, Objetos.CAE.ESTipoCFECFC.NCEFactura, formaPago, adenda, razonRef);

            if (cfe != null)
            {
                if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                {
                    //Obtener el objeto cae utilizado en el documento creado
                    cae = manteUdoCae.Consultar("999", cfe.SerieComprobante);
                }
                else
                {
                    //Obtener el objeto cae utilizado en el documento creado
                    cae = manteUdoCae.Consultar(cfe.TipoCFEInt.ToString(), cfe.SerieComprobante);
                }

                //Actualizar datos del CFE en el documento creado
                notaCreditoB1.ActualizarDatosCFENotaCredito(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.InfoReferencia);

                DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(), cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "14", "");

                datosPdf.DescuentoGeneral = NotaCreditoB1.descuentoGeneral;
                datosPdf.DescuentoExtranjero = NotaCreditoB1.descuentoGeneralExtranjero;
                datosPdf.PorcentajeDescuento = NotaCreditoB1.porcentajeDescuento;
                eventos.EnviarDocumento(cfe, cae, datosPdf, "RIN1", null, "ORIN");

                //Cambio el estado a Procesado para no volver a enviarlo
                notaCreditoB1.ActualizarEstadoDocumento(numeroDocumento, PROCESADO, cfe.CodigoSeguridad);

                cfe = null;
                cae = null;
            }
        }

        #region Transacciones_Periodicas
        public void procesarColaImpresion()
        {
            Thread threadEnvio = null;

            //Se define el hilo
            threadEnvio = new Thread(this.ImprimirCola);
            //Se pone como background para que se muera cuando se cierra la aplicacion
            threadEnvio.IsBackground = true;
            //Inicia el hilo
            threadEnvio.Start();
        }

        private void ImprimirCola()
        {
            string nombreArchivo;
            bool blnImprimir;

            try
            {
                if (Directory.Exists(RutasCarpetas.RutaCarpetaImpresion))
                {
                    foreach (string sinImprimir in Directory.GetFiles(RutasCarpetas.RutaCarpetaImpresion, "*.pdf"))
                    {
                        Program.colaImpresion.Enqueue(sinImprimir);
                    }
                }

                while (!detenerHilo)
                {
                    if (Program.colaImpresion.Count > 0)
                    {
                        nombreArchivo = Program.colaImpresion.Peek();
                        if (File.Exists(nombreArchivo))
                        {
                            blnImprimir = ImprimirPdf(nombreArchivo);
                            Program.colaImpresion.Dequeue();
                            File.Delete(nombreArchivo);
                        }
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage("Error al procesar cola de impresión");
            }
            finally
            {

            }
        }

        public Boolean ImprimirPdf(object nombreArchivo)
        {
            bool salida = false;

            try
            {
                Process proc = new Process();

                var adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("AcroRd32.exe");
                string rutaAdobe = adobe.GetValue("").ToString();

                //Se oculta la ventana
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                #region IMRPRESION DEFAULT

                //Se setea adobe para que abra en modo de impresion
                proc.StartInfo.Verb = "print";
                //Se carga la ruta del ejecutable de adobe
                proc.StartInfo.FileName = rutaAdobe;

                //Se cargan los argumentos, se envia a imprimir a la impresora x default
                proc.StartInfo.Arguments = String.Format(@"/p /h {0}", nombreArchivo);

                #endregion IMPRESION DEFAULT

                proc.StartInfo.UseShellExecute = false;
                //Evita crear la venta de impresion
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                //Inicia la impresion
                proc.Start();
                int idSesion = proc.SessionId;

                if (proc.HasExited == false)
                {
                    //Espera 12 segundos para cerrar la ventana de adobe
                    proc.WaitForExit(20000);
                }

                proc.EnableRaisingEvents = true;
                //Cierra el proceso
                proc.Close();
                //
                EliminarProcesoAdobe("AcroRd32", idSesion);
                salida = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error: " + ex.ToString());
            }
            return salida;
        }

        private static void EliminarProcesoAdobe(string name, int idSesion)
        {
            bool primerProceso = false;

            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.StartsWith(name))
                    {
                        if (clsProcess.SessionId == idSesion)
                        {
                            if (!primerProceso)
                            {
                                clsProcess.Kill();
                                primerProceso = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error: " + ex.ToString());
            }
        }
        #endregion Transacciones_Periodicas
    }
}
