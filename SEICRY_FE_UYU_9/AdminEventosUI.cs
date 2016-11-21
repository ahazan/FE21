
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
using SEICRY_FE_UYU_9.MejoraTiempo;

namespace SEICRY_FE_UYU_9
{
    class AdminEventosUI
    {
        //Valores globales de la firma digital
        public static string RUTA_CERTIFICADO = "";
        public static string CLAVE_CERTIFICADO = "";

        //Valores globales para las direcciones de los web services
        public static string URL_ENVIO = "";
        public static string URL_CONSULTAS = "";

        //Variable globar de control de tipos de usuarios
        public static bool modoUsuario = false;
        public static bool comboBubbleEvent = false;

        #region Nombre Formularios

        //Formularios
        private FrmDetallesSociedad frmDetallesSociedad;
        private FrmDocumento frmDocumento = new FrmDocumento();
        private FrmAdminCAE frmAdminCAE = new FrmAdminCAE();
        private FrmNuevoCAE frmNuevoCAE = new FrmNuevoCAE();
        private FrmActualizarCAE frmActualizarCAE = new FrmActualizarCAE();
        private FrmVisualizarRangos frmVisualizarRangos = new FrmVisualizarRangos();
        private FrmFTP frmFtp = new FrmFTP();
        private FrmDocCon frmDocCon = new FrmDocCon();
        private FrmRetPer frmRetPer = new FrmRetPer();
        private FrmMonitor frmMonitor = new FrmMonitor();
        private FrmMonitorReporte frmMonitorReporte = new FrmMonitorReporte();
        private FrmCerDig frmCerDig = new FrmCerDig();
        private FrmVisualizarCertificado frmVisualizarCertificado = new FrmVisualizarCertificado();
        private FrmMonitorAnulado frmMonitorAnulado = new FrmMonitorAnulado();
        private FrmMonitorSobre frmMonitorSobre = new FrmMonitorSobre();
        private FrmCertificadoRecibidos frmCertificadoRecibido = new FrmCertificadoRecibidos();
        private FrmCertificadoRecibidoDet frmCertificadoRecibidoDet;
        private FrmAprobacion frmAprobacion = new FrmAprobacion();
        private FrmMotivoRechazo frmMotivoRechazo = new FrmMotivoRechazo();
        private FrmVisualizarSobreFactura frmVisualizarSobreFactura = new FrmVisualizarSobreFactura();
        private FrmEstadoContingencia frmEstadoContingencia = new FrmEstadoContingencia();
        private FrmEnvioCorreoElectronico frmEnvioCorreoElectronico = new FrmEnvioCorreoElectronico();
        private FrmLogo frmLogo = new FrmLogo();
        private FrmVisualizar frmVisualizar;
        private FrmAutoDocNoElectronico frmAutoDocNoElectronico = new FrmAutoDocNoElectronico();
        private FrmMonCerContingencia frmMonCerContingencia = new FrmMonCerContingencia();
        private FrmImpuestosDgiB1 frmImpuestosdgiB1 = new FrmImpuestosDgiB1();
        private FrmArticulos frmArticulos = new FrmArticulos();
        private FrmAdobe frmAdobe = new FrmAdobe();
        private FrmSociosNegocios frmSociosNegocio = new FrmSociosNegocios();
        private FrmUsuarios frmUsuarios = new FrmUsuarios();
        private FrmFormaPago frmFormaPago = new FrmFormaPago();
        private FrmMonImpresion frmMonImpresion = new FrmMonImpresion();
        private FrmConfFinCae frmConfFinCae = new FrmConfFinCae();
        private FrmEnvioDGICfes frmEnvioDGICfes = new FrmEnvioDGICfes();
        private FrmRazonReferenciaNC frmRazonReferenciaNC = new FrmRazonReferenciaNC();
        private FrmConfRptd frmConfRptd = new FrmConfRptd();
        private FrmConfTipoCambio frmConfTipoCambio = new FrmConfTipoCambio();
        private FrmSucuDire frmSucuDire = new FrmSucuDire();

        public static bool frmVisualizarActivo = false;
        private static bool modoBusquedaNavegacion = false;
        public static bool cierreFormulario = false;
        private static string formaPagoNC = "";
        public static CAE caePrueba = null;
        private static string RutClienteCbo = "";
        private static string RutClienteTxt = "";

        private static bool Salir = false;

        #endregion Nombre Formularios

        //Lista de instancias de documentos
        private ArrayList listaInstanciasDocumentos = new ArrayList();
        private List<CFE> listaCertificadosCreados = new List<CFE>();

        //Variable control de errores para configuracion de correos electronicos
        string errorVerifica = "";
        public static string docNumPDf = "";

        //Variable para controlar la creacion de documento electronicos
        bool crearDocumentoElectronico = false;

        //Filtros de los eventos
        private EventFilters listaFiltrosEventos;
        private EventFilter filtroEvento;

        //Instancias de mantenimientos
        private ManteUdoCAE manteUdoCae = new ManteUdoCAE();
        private ManteUdoCFE manteUdoCfe = new ManteUdoCFE();
        private ManteDocumentos manteDocumentos = new ManteDocumentos();

        //Variables de objetos
        private CFE cfe;
        //private Sobre sobre;
        //private Sobre sobreDgi;
        private CAE cae;

        //Procesos web service
        ComunicacionDgi comunicacionDGI = new ComunicacionDgi();

        //Lista de certificados creados
        private static string codigoSNAnterior = "";


        //Variable Info Sistema
        private static SAPbouiCOM.Application app = SAPbouiCOM.Framework.Application.SBO_Application;


        private Boolean _NewDoc = true;


        /*  */
        private string clausulaDeVta;
        private string modalidadVta;
        private string viaTransporte;
        /*  */
        

        public AdminEventosUI()
        {
            ListarEventos();
        }

        #region Proceso_WebService
        public AdminEventosUI(bool ws = true)
        {

        }
        #endregion Proceso_WebService

        /// <summary>
        /// Lista los eventos y establece los filtros necesarios para el add-on
        /// </summary>
        private void ListarEventos()
        {
            #region Eventos

            //Listar eventos de la aplicacion
            app.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            app.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
            app.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormDataEvent);
            app.AppEvent += new _IApplicationEvents_AppEventEventHandler(app_AppEvent);

                               

            //Filtrar eventos para la aplicacion
            listaFiltrosEventos = new EventFilters();

            #region Filtros FORM_LOAD

            //Filtros para eventos Form load
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_LOAD);
            filtroEvento.AddEx("136"); //Detalles de sociedad
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo Compra
            filtroEvento.AddEx("181");//Resguardo Compra NC
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("60091");//Facturas de Reserva
            filtroEvento.AddEx("134");
            filtroEvento.AddEx("20700");//Usuarios
            filtroEvento.AddEx("150");//Articulos
            filtroEvento.AddEx("frmEnvCom");//Envio Masivo Comprobantes a DGI
            filtroEvento.AddEx("frmConCae");//Configuracion Alerta Fin CAE

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros FORM_LOAD

            #region Filtros FORM_UNLOAD

            //Filtros para eventos Form Unload
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_UNLOAD);
            filtroEvento.AddEx("frmCAE"); //Administracion de CAE
            filtroEvento.AddEx("frmNCAE"); //Nuevo CAE
            filtroEvento.AddEx("frmACAE"); //Actualizar CAE
            filtroEvento.AddEx("frmCUDO"); //Crear UDO
            filtroEvento.AddEx("frmVRan"); //Visualizar rangos
            filtroEvento.AddEx("frmFTP"); //Configuracion del FTP 
            filtroEvento.AddEx("frmDocCon"); //Documentos a conservar
            filtroEvento.AddEx("frmRetPer"); //Retencion/Percepcion
            filtroEvento.AddEx("frmSucuDire"); //Retencion/Percepcion
            filtroEvento.AddEx("frmMon"); //Monitor
            filtroEvento.AddEx("frmMonRp"); //Monitor Reporte
            filtroEvento.AddEx("frmCerDig");//Certificados Digitales
            filtroEvento.AddEx("frmVisCer");//Certificados Rechazados
            filtroEvento.AddEx("frmMonAnu");//Monitor Anulado
            filtroEvento.AddEx("frmMonSob");//Monitor Sobres
            filtroEvento.AddEx("frmSobFac");//Sobre Factura
            filtroEvento.AddEx("frmVisSob");//Visualizar sobre Factura
            filtroEvento.AddEx("frmEstCont");//Estado de Contingencia
            filtroEvento.AddEx("frmECA");//Envio correo electronico
            filtroEvento.AddEx("frmLogo");//Logo para pdf
            filtroEvento.AddEx("frmVisualizar");//Visualiza XML
            filtroEvento.AddEx("frmAutoNoElec");//Autoriza Documentos no Electronicos
            filtroEvento.AddEx("frmCerRec");//Certificados Recibidos
            filtroEvento.AddEx("frmSRDet");//Detalles de Certificados Recibidos
            filtroEvento.AddEx("frmACR");//Aprobacion Certificados Recibidos
            filtroEvento.AddEx("frmCerCon");//Monitor Certificados Contingencia
            filtroEvento.AddEx("frmMotRech"); //Motivos de rechazo de certificados recibidos
            filtroEvento.AddEx("frmImpDgi"); //Indicador de impuestos
            filtroEvento.AddEx("frmAdenda");//Adenda
            filtroEvento.AddEx("frmFormaPago");//Forma de Pago
            filtroEvento.AddEx("frmAdoRea");//Adobe Reader
            filtroEvento.AddEx("frmEnvCom");//Envio Masivo Comprobantes a DGI  
            filtroEvento.AddEx("frmConCae");//Configuracion Alerta Fin CAE
            filtroEvento.AddEx("frmRazRefNC");//Razon Referencia Nota Credito
            filtroEvento.AddEx("frmConfRptd");//Configuracion Reporte Diario
            filtroEvento.AddEx("frmConfTipC");//Configuracion Tipo Cambio
            filtroEvento.AddEx("0");//Configuracion Tipo Cambio


            #endregion Filtros FORM_UNLOAD

            #region Filtros ITEM_PRESSED

            //Filtros para item pressed
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_ITEM_PRESSED);
            filtroEvento.AddEx("136"); //Detalles de sociedad  
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("60091");//Facturas de Reserva
            filtroEvento.AddEx("141");//Resguardo de compra
            filtroEvento.AddEx("181");//Resguardo de compra NC
            filtroEvento.AddEx("frmCAE"); //Administracion de CAE
            filtroEvento.AddEx("frmNCAE"); //Nuevo CAE
            filtroEvento.AddEx("frmACAE"); //Actualizar CAE
            filtroEvento.AddEx("frmCUDO"); //Crear UDO
            filtroEvento.AddEx("frmVRan"); //Visualizar rangos
            filtroEvento.AddEx("frmFTP"); //Configuracion del FTP 
            filtroEvento.AddEx("frmDocCon"); //Documentos a conservar
            filtroEvento.AddEx("frmRetPer"); //Retencion/Percepcion
            filtroEvento.AddEx("frmSucuDire"); //Sucursal direccion
            filtroEvento.AddEx("frmMon"); //Monitor
            filtroEvento.AddEx("frmMonRp"); //Monitor Reporte
            filtroEvento.AddEx("frmCerDig");//Certificados Digitales
            filtroEvento.AddEx("frmVisCer");//Certificados Rechazados
            filtroEvento.AddEx("frmMonAnu");//Monitor Anulados
            filtroEvento.AddEx("frmMonSob");//Monitor Sobres
            filtroEvento.AddEx("frmCerRec");//Certificados Recibidos
            filtroEvento.AddEx("frmSRDet");//Detalles de Certificados Recibidos
            filtroEvento.AddEx("frmSobFac");//Sobre Factura
            filtroEvento.AddEx("frmVisSob");//Visualizar Sobre Factura
            filtroEvento.AddEx("frmEstCont");//Estado de Contingencia
            filtroEvento.AddEx("frmECA");//Envio Correo Electronico
            filtroEvento.AddEx("frmLogo");//Logo para el pdf
            filtroEvento.AddEx("frmVisualizar");//Visualiza Arhivos XML
            filtroEvento.AddEx("frmAutoNoElec");//Autoriza documentos no electronicos
            filtroEvento.AddEx("frmACR");//Aprobacion Certificados Recibidos
            filtroEvento.AddEx("frmCerCon");//Monitor Certificados Contingencia
            filtroEvento.AddEx("frmMotRech"); //Motivos de rechazo de certificados recibidos
            filtroEvento.AddEx("frmImpDgi");//Indicador de Impuestos
            filtroEvento.AddEx("134");//Socios de Negocio
            filtroEvento.AddEx("20700");//Usuarios
            filtroEvento.AddEx("frmAdenda");//Adenda
            filtroEvento.AddEx("frmFormaPago");//Forma de pago
            filtroEvento.AddEx("frmAdoRea");//Adobe Reader
            filtroEvento.AddEx("frmEnvCom");//Envio Masivo Comprobantes a DGI
            filtroEvento.AddEx("frmConCae");//Configuracion Alerta Fin CAE
            filtroEvento.AddEx("-9876");//CopiaFactura->NotaCredito
            filtroEvento.AddEx("frmRazRefNC");//Razon Referencia Nota Credito
            filtroEvento.AddEx("frmConfRptd");//Configuracion Reporte Diario
            filtroEvento.AddEx("frmConfTipC");//Configuracion Tipo Cambio
            filtroEvento.AddEx("0");//Configuracion Tipo Cambio

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros ITEM_PRESSED

            #region Filtros CLICK

            //Filtros para click
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_CLICK);
            filtroEvento.AddEx("frmACR");//Aprobacion Certificados Recibidos

            #endregion Filtros ITEM_CLICK

            #region Filtros MENU_CLICK

            //Filtros para menu
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_MENU_CLICK);
            filtroEvento.AddEx("mFE"); //Menu Factura Eletronica
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo Compra
            filtroEvento.AddEx("181");//Resguardo Compra NC
            filtroEvento.AddEx("60091");//Facturas de Reserva

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros MENU_CLICK

            #region Filtros COMBO_SELECT

            //Filtros para seleccion de combo box
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_COMBO_SELECT);
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo de Compra
            filtroEvento.AddEx("181");//Resguardo de Compra NC
            filtroEvento.AddEx("60091");//Facturas de Reserva
            filtroEvento.AddEx("frmMotRech"); //Motivos de rechazo de certificados recibidos
            filtroEvento.AddEx("frmAdenda");//Adenda
            filtroEvento.AddEx("frmFormaPago");//Forma de pago

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros COMBO_SELECT

            #region Filtros FORM_ACTIVATE

            //Filtros para form activate
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_ACTIVATE);
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo Compra
            filtroEvento.AddEx("181");//Resguardo Compra NC
            filtroEvento.AddEx("60091");//Facturas de Reserva

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros FORM_ACTIVATE

            #region Filtros FORM_DEACTIVATE
            //Filtros para form deactivate
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_DEACTIVATE);
            filtroEvento.AddEx("133");

            #endregion Filtros FORM_DEACTIVE

            #region Filtros FORM_DATA_LOAD

            //Filtros para form data load
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_DATA_LOAD);
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo Compra
            filtroEvento.AddEx("181");//Resguardo Compra NC
            filtroEvento.AddEx("60091");//Facturas de Reserva

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros FORM_DATA_LOAD

            #region Filtros FORM_DATA_UPDATE

            //Filtros para form data update
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_DATA_UPDATE);
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo Compra
            filtroEvento.AddEx("181");//Resguardo Compra
            filtroEvento.AddEx("60091");//Factura de Reserva

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros FORM_DATA_UPDATE

            #region Filtros FORM_DATA_ADD

            //Filtros para form data add
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_FORM_DATA_ADD);
            filtroEvento.AddEx("133"); //Facturas
            filtroEvento.AddEx("179"); //Notas de credito
            filtroEvento.AddEx("65303"); //Notas de debito
            filtroEvento.AddEx("140"); //Remito
            filtroEvento.AddEx("141");//Resguardo Compra
            filtroEvento.AddEx("181");//Resguardo Compra
            filtroEvento.AddEx("60091");//Factura de Reserva

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion Filtros FORM_DATA_ADD

            #region Filtros KEY_DOWN

            //Filtros para key down
            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_KEY_DOWN);
            filtroEvento.AddEx("frmRetPer"); //Retencion/Percepcion
            filtroEvento.AddEx("frmSucuDire"); //Sucursal direccion
            filtroEvento.AddEx("frmAdenda");//Adenda
            filtroEvento.AddEx("frmRazRefNC");//Razon referencia NC

            #endregion Filtros KEY_DOWN

            #region FILTROS FOCUS

            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_GOT_FOCUS);
            filtroEvento.AddEx("frmMonAnu"); //Monitor de certificados anulados
            filtroEvento.AddEx("frmMotRech"); //Motivos de rechazo de certificados recibidos

            filtroEvento = listaFiltrosEventos.Add(BoEventTypes.et_LOST_FOCUS);
            filtroEvento.AddEx("133");//Factura Sistema
            filtroEvento.AddEx("140");//Remito
            filtroEvento.AddEx("179");//Nota Credito Sistema
            filtroEvento.AddEx("65303");//Notas de Debito
            filtroEvento.AddEx("141");//Resguardo Compras
            filtroEvento.AddEx("181");//Resguardo Compras
            filtroEvento.AddEx("60091");//Factura de Reservas

            filtroEvento.AddEx("540010035"); //Transacciones Periódicas
            filtroEvento.AddEx("540010036"); //Modelos de Transacciones Periódicas

            #endregion FILTROS FOCUS

            //Establecer lista de eventos
            app.SetFilter(listaFiltrosEventos);

            #endregion Eventos
        }

        void app_AppEvent(BoAppEventTypes EventType)
        {

                Program.Cerrar();         
        }

        //*****************Eventos de Datos******************//
        void SBO_Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.ActionSuccess)
                {
                    #region DATA_ADD

                    //Al crear un nuevo documento de cualquier tipo
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                    {
                        if (crearDocumentoElectronico)
                        {
                            if (BusinessObjectInfo.FormTypeEx.Equals("133") || BusinessObjectInfo.FormTypeEx.Equals("65303")
                                || BusinessObjectInfo.FormTypeEx.Equals("179") || BusinessObjectInfo.FormTypeEx.Equals("140")
                                || BusinessObjectInfo.FormTypeEx.Equals("141") || BusinessObjectInfo.FormTypeEx.Equals("60091") || BusinessObjectInfo.FormTypeEx.Equals("181"))
                            {
                                //Obtener el numero del documento creado
                                int numeroDocumentoCreado = DocumentoB1.ObtenerNumeroDocumentoCreado(BusinessObjectInfo.ObjectKey);

                                #region FACTURA, NOTA DE DEBITO Y RESGUARDO

                                //Valida que el tipo de documento creado sea Factura o Nota de Debito
                                if (BusinessObjectInfo.Type.Equals("13"))
                                {
                                    //Creo ProgressBar y seteo vaalor MAx
                                    // SAPbouiCOM.ProgressBar ProgBar = app.StatusBar.CreateProgressBar("Generando Documento Electronico...", 100000, false);

                                    //ProgressBar(ProgBar,"Generando Documento Electronico...",10000);

                                    //Valida que el documento sea una factura
                                    if (DocumentoB1.ValidarDocumentoFactura(numeroDocumentoCreado, "OINV"))
                                    {
                                        FacturaB1 facturaB1 = new FacturaB1();
                                        string formaPago = string.Empty;

                                        DatosPDF datosPdf = new DatosPDF();

                                        datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumentoCreado, "INV1", datosPdf);
                                        datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumentoCreado, "OINV", datosPdf);
                                        datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumentoCreado, "OINV");
                                        //datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumentoCreado, "OINV");
                                        datosPdf.Titular = DocumentoB1.Titular(numeroDocumentoCreado, "OINV");
                                        datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
                                        datosPdf = DocumentoB1.ActualizarEstado(datosPdf);//Saint
                                        datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);//Saint  
                                        datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

                                        datosPdf.Email = DocumentoB1.ObtenerMailCliente(datosPdf.SocioNegocio);


                                        //Se agrega la forma de pago
                                        if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected != null)
                                        {
                                            formaPago = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected.Value + "";
                                        }

                                        //Se almacena la adenda
                                        string adenda = ((EditText)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("txtAdn").Specific).Value;

                                        //Obtener el objeto cfe a partir de los datos del documento creado
                                        cfe = facturaB1.ObtenerDatosFactura(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.EFactura, formaPago, adenda);


                                        //   ProgressBar(ProgBar,"Generando Documento Electronico...", 50000);

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
                                            facturaB1.ActualizarDatosCFEFActura(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante,
                                                cfe.NumeroComprobante.ToString());

                                            DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(),
                                                cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "13", "F");

                                            datosPdf.DescuentoGeneral = FacturaB1.descuentoGeneral;
                                            datosPdf.DescuentoExtranjero = FacturaB1.descuentoGeneralExtranjero;
                                            datosPdf.PorcentajeDescuento = FacturaB1.porcentajeDescuento;


                                            // ProgressBar(ProgBar,"Enviando Documento Electronico...", 80000);

                                            this.EnviarDocumento(cfe, cae, datosPdf, "INV1", null, "OINV");


                                            ActulizoFormaPago();

                                            cfe = null;
                                            cae = null;

                                            //Valida que el documento sea un resguardo
                                            if (manteDocumentos.ValidarDocumentoResguardo(numeroDocumentoCreado, "INV5"))
                                            {
                                                ResguardoB1 resguardoB1 = new ResguardoB1();

                                                //Obtener el objeto cfe a partir de los datos del documento creado
                                                cfe = resguardoB1.ObtenerDatosResguardo(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.ERemito);


                                                List<ResguardoPdf> resguardoPdf = DocumentoB1.ObtenerResguardoPdf(numeroDocumentoCreado, "OINV", "INV1", "INV5");

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
                                                    resguardoB1.ActualizarDatosCFEResguardo(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante,
                                                        cfe.NumeroComprobante.ToString());

                                                    datosPdf.DescuentoGeneral = ResguardoB1.descuentoGeneral;
                                                    datosPdf.DescuentoExtranjero = ResguardoB1.descuentoGeneralExtranjero;

                                                    this.EnviarDocumento(cfe, cae, datosPdf, "INV1", resguardoPdf, "OINV");

                                                    cfe = null;
                                                    cae = null;
                                                }
                                            }




                                        }//Si es nota de debito
                                    }
                                    else
                                    {
                                        NotaDebitoB1 notaDebitoB1 = new NotaDebitoB1();

                                        string formaPago = string.Empty;

                                        DatosPDF datosPdf = new DatosPDF();

                                        datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumentoCreado, "INV1", datosPdf);
                                        datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumentoCreado, "OINV", datosPdf);
                                        datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumentoCreado, "OINV");
                                        //datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumentoCreado, "OINV");
                                        datosPdf.Titular = DocumentoB1.Titular(numeroDocumentoCreado, "OINV");
                                        datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
                                        datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
                                        datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
                                        datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

                                        datosPdf.Email = DocumentoB1.ObtenerMailCliente(datosPdf.SocioNegocio);

                                        //  ProgressBar(ProgBar, "Generando Documento Electronico...", 10000);

                                        if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected != null)
                                        {
                                            formaPago = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected.Value + "";
                                        }

                                        //Se almacena la adenda
                                        string adenda = ((EditText)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("txtAdn").Specific).Value;
                                        string razonRef = "";

                                        if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxRazRef").Specific).Selected != null)
                                        {
                                            razonRef = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxRazRef").Specific).Selected.Description + "";
                                        }


                                        //Obtener el objeto cfe a partir de los datos del documento creado
                                        cfe = notaDebitoB1.ObtenerDatosNotaDebito(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.NDEFactura, formaPago, adenda, razonRef);


                                        // ProgressBar(ProgBar, "Generando Documento Electronico...", 50000);

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
                                            notaDebitoB1.ActualizarDatosCFENotaDebito(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante,
                                                cfe.NumeroComprobante.ToString(), cfe.InfoReferencia);

                                            DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(),
                                                cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "13", "ND");

                                            datosPdf.DescuentoGeneral = NotaDebitoB1.descuentoGeneral;
                                            datosPdf.DescuentoExtranjero = NotaDebitoB1.descuentoGeneralExtranjero;
                                            datosPdf.PorcentajeDescuento = NotaDebitoB1.porcentajeDescuento;

                                            //  ProgressBar(ProgBar, "Generando Documento Electronico...", 80000);

                                            this.EnviarDocumento(cfe, cae, datosPdf, "INV1", null, "OINV");


                                            ActulizoFormaPago();

                                            cfe = null;
                                            cae = null;
                                        }
                                    }
                                }

                                #endregion FACTURA Y NOTA DE DEBITO

                                #region NOTA DE CREDITO

                                //Valida que el tipo de documento creado sea Nota de Credito
                                else if (BusinessObjectInfo.Type.Equals("14"))
                                {
                                    NotaCreditoB1 notaCreditoB1 = new NotaCreditoB1();
                                    string formaPago = string.Empty;

                                    DatosPDF datosPdf = new DatosPDF();

                                    datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumentoCreado, "RIN1", datosPdf);
                                    datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumentoCreado, "ORIN", datosPdf);
                                    datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumentoCreado, "ORIN");
                                    //datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumentoCreado, "ORIN");
                                    datosPdf.Titular = DocumentoB1.Titular(numeroDocumentoCreado, "ORIN");
                                    datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

                                    datosPdf.Email = DocumentoB1.ObtenerMailCliente(datosPdf.SocioNegocio);

                                    //formaPago = DocumentoB1.ObtenerFormaPagoFactura(numeroDocumentoCreado);
                                    formaPago = DocumentoB1.ObtenerFormaPago(DocumentoB1.TABLA_NOTA_CREDITO, numeroDocumentoCreado.ToString());

                                    //Creo ProgressBar y seteo vaalor MAx
                                    // SAPbouiCOM.ProgressBar ProgBar = app.StatusBar.CreateProgressBar("Generando Documento Electronico...", 100000, false);

                                    // ProgressBar(ProgBar, "Generando Documento Electronico...", 10000);

                                    if (formaPago.Equals(""))
                                    {
                                        if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected != null)
                                        {
                                            formaPago = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected.Value + "";
                                        }
                                    }
                                    //datosPdf.FormaPago = formaPago;

                                    //Se almacena la adenda
                                    string adenda = ((EditText)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("txtAdn").Specific).Value;
                                    string razonRef = "";

                                    if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxRazRef").Specific).Selected != null)
                                    {
                                        razonRef = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxRazRef").Specific).Selected.Description + "";
                                    }

                                    //Obtener el objeto cfe a partir de los datos del documento creado
                                    cfe = notaCreditoB1.ObtenerDatosNotaCredito(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.NCEFactura, formaPago, adenda, razonRef);

                                    //ProgressBar(ProgBar, "Generando Documento Electronico...", 50000);


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
                                        notaCreditoB1.ActualizarDatosCFENotaCredito(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante,
                                            cfe.NumeroComprobante.ToString(), cfe.InfoReferencia);

                                        DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(),
                                            cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "14", "");

                                        datosPdf.DescuentoGeneral = NotaCreditoB1.descuentoGeneral;
                                        datosPdf.DescuentoExtranjero = NotaCreditoB1.descuentoGeneralExtranjero;
                                        datosPdf.PorcentajeDescuento = NotaCreditoB1.porcentajeDescuento;

                                        // ProgressBar(ProgBar, "Generando Documento Electronico...", 80000);

                                        this.EnviarDocumento(cfe, cae, datosPdf, "RIN1", null, "ORIN");


                                        ActulizoFormaPago();

                                        cfe = null;
                                        cae = null;
                                    }
                                }

                                #endregion NOTA DE CREDITO

                                #region REMITO

                                //Valida que el tipo de documento creado sea Remito
                                else if (BusinessObjectInfo.Type.Equals("15"))
                                {
                                    RemitoB1 remitoB1 = new RemitoB1();

                                    //Creo ProgressBar y seteo vaalor MAx
                                    //SAPbouiCOM.ProgressBar ProgBar = app.StatusBar.CreateProgressBar("Generando Documento Electronico...", 100000, false);

                                    //  ProgressBar(ProgBar, "Generando Documento Electronico...", 10000);

                                    //Se almacena la adenda
                                    string adenda = ((EditText)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("txtAdn").Specific).Value;

                                    string razonRef = "";

                                    //if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxRazRef").Specific).Selected != null)
                                    //{
                                    //    razonRef = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxRazRef").Specific).Selected.Description + "";
                                    //}

                                    //Obtener el objeto cfe a partir de los datos del documento creado
                                    cfe = remitoB1.ObtenerDatosRemito(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.ERemito, adenda, razonRef);

                                    // ProgressBar(ProgBar, "Generando Documento Electronico...", 50000);

                                    DatosPDF datosPdf = new DatosPDF();

                                    datosPdf = DocumentoB1.ObtenerkilosFactura(numeroDocumentoCreado, "DLN1", datosPdf);
                                    datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumentoCreado, "ODLN", datosPdf);
                                    datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumentoCreado, "ODLN");
                                    //datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumentoCreado, "ODLN");
                                    datosPdf.Titular = DocumentoB1.Titular(numeroDocumentoCreado, "ODLN");
                                    datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarNumPedido(datosPdf);

                                    datosPdf.Email = DocumentoB1.ObtenerMailCliente(datosPdf.SocioNegocio);

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
                                        remitoB1.ActualizarDatosCFERemito(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante, cfe.NumeroComprobante.ToString());
                                        DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(),
                                            cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "15", "");

                                        datosPdf.DescuentoGeneral = RemitoB1.descuentoGeneral;
                                        datosPdf.DescuentoExtranjero = RemitoB1.descuentoGeneralExtranjero;
                                        datosPdf.PorcentajeDescuento = RemitoB1.porcentajeDescuento;

                                        // ProgressBar(ProgBar, "Generando Documento Electronico...", 80000);

                                        this.EnviarDocumento(cfe, cae, datosPdf, "DLN1", null, "ODLN");

                                        ActulizoFormaPago();

                                        cfe = null;
                                        cae = null;
                                    }
                                }

                                #endregion REMITO

                                #region RESGUARDO COMPRA

                                else if (BusinessObjectInfo.Type.Equals("18"))
                                {
                                    DatosPDF datosPdf = new DatosPDF();

                                    datosPdf.KilosFactura = "";
                                    datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumentoCreado, "OPCH", datosPdf);
                                    datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumentoCreado, "OPCH");
                                    //datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumentoCreado, "OPCH");
                                    datosPdf.Titular = DocumentoB1.Titular(numeroDocumentoCreado, "OPCH");
                                    datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
                                    datosPdf.Email = DocumentoB1.ObtenerMailCliente(datosPdf.SocioNegocio);

                                    //SAPbouiCOM.ProgressBar ProgBar = app.StatusBar.CreateProgressBar("Generando Documento Electronico...", 100000, false);

                                    // ProgressBar(ProgBar, "Generando Documento Electronico...", 10000);

                                    //Se almacena la adenda
                                    string adenda = ((EditText)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("txtAdn").Specific).Value;

                                    //Valida que el documento sea una factura
                                    if (DocumentoB1.ValidarDocumentoFactura(numeroDocumentoCreado, "OPCH"))
                                    {
                                        cfe = null;
                                        cae = null;

                                        //Valida que el documento sea un resguardo
                                        if (manteDocumentos.ValidarDocumentoResguardo(numeroDocumentoCreado, "PCH5"))
                                        {
                                            ResguardoCompraB1 resguardoCompraB1 = new ResguardoCompraB1();
                                            List<ResguardoPdf> resguardoPdf = DocumentoB1.ObtenerResguardoPdf(numeroDocumentoCreado, "OPCH", "PCH1", "PCH5");

                                            //Obtener el objeto cfe a partir de los datos del documento creado
                                            cfe = resguardoCompraB1.ObtenerDatosResguardo(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.EResguardo, adenda);

                                            //ProgressBar(ProgBar, "Generando Documento Electronico...", 50000);

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
                                                resguardoCompraB1.ActualizarDatosCFEResguardo(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante,
                                                    cfe.NumeroComprobante.ToString());

                                                DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(),
                                                    cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "18", "");
                                                if (ResguardoCompraB1.descuentoGeneral > 0)
                                                {
                                                    datosPdf.DescuentoGeneral = ResguardoCompraB1.descuentoGeneral;
                                                }
                                                else if (ResguardoCompraB1.descuentoGeneralExtranjero > 0)
                                                {
                                                    datosPdf.DescuentoExtranjero = ResguardoCompraB1.descuentoGeneralExtranjero;
                                                }

                                                //ProgressBar(ProgBar, "Generando Documento Electronico...", 80000);

                                                this.EnviarDocumento(cfe, cae, datosPdf, "PCH1", resguardoPdf, "OPCH");

                                                ActulizoFormaPago();

                                                cfe = null;
                                                cae = null;
                                            }
                                        }

                                    }
                                }
                                #endregion RESGUARDO COMPRA

                                #region RESGUARDO COMPRA NC PROVEEDORES

                                else if (BusinessObjectInfo.Type.Equals("19"))
                                {
                                    DatosPDF datosPdf = new DatosPDF();

                                    datosPdf.KilosFactura = "";
                                    datosPdf = DocumentoB1.ObtenerDatosPDF(numeroDocumentoCreado, "ORPC", datosPdf);
                                    datosPdf.NombreVendedor = DocumentoB1.ObtenerNombreVendedor(numeroDocumentoCreado, "ORPC");
                                    //datosPdf.NombreExtranjero = DocumentoB1.ObtenerNombreExtranjero(numeroDocumentoCreado, "ORPC");
                                    datosPdf.Titular = DocumentoB1.Titular(numeroDocumentoCreado, "ORPC");
                                    datosPdf = DocumentoB1.ObtenerDatosDireccion(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarEstado(datosPdf);
                                    datosPdf = DocumentoB1.ActualizarCodigo(datosPdf);
                                    datosPdf.Email = DocumentoB1.ObtenerMailCliente(datosPdf.SocioNegocio);

                                    //SAPbouiCOM.ProgressBar ProgBar = app.StatusBar.CreateProgressBar("Generando Documento Electronico...", 100000, false);

                                    //ProgressBar(ProgBar, "Generando Documento Electronico...", 10000);

                                    //Valida que el documento sea una factura
                                    if (DocumentoB1.ValidarDocumentoFactura(numeroDocumentoCreado, "ORPC"))
                                    {
                                        cfe = null;
                                        cae = null;

                                        //Valida que el documento sea un resguardo
                                        if (manteDocumentos.ValidarDocumentoResguardo(numeroDocumentoCreado, "RPC5"))
                                        {
                                            ResguardoCompraNCB1 resguardoCompraNCB1 = new ResguardoCompraNCB1();

                                            List<ResguardoPdf> resguardoPdf = DocumentoB1.ObtenerResguardoPdf(numeroDocumentoCreado, "ORPC", "RPC1", "RPC5", true);

                                            //Obtener el objeto cfe a partir de los datos del documento creado
                                            cfe = resguardoCompraNCB1.ObtenerDatosResguardo(numeroDocumentoCreado, Objetos.CAE.ESTipoCFECFC.EResguardo);

                                            //ProgressBar(ProgBar, "Generando Documento Electronico...", 50000);

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
                                                resguardoCompraNCB1.ActualizarDatosCFEResguardo(int.Parse(cfe.DocumentoSAP), cfe.SerieComprobante,
                                                    cfe.NumeroComprobante.ToString());

                                                DocumentoB1.ActualizarCAEAsiento(int.Parse(datosPdf.DocNum), cfe.TipoCFEInt.ToString(),
                                                    cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), "19", "");
                                                if (ResguardoCompraB1.descuentoGeneral > 0)
                                                {
                                                    datosPdf.DescuentoGeneral = ResguardoCompraB1.descuentoGeneral;
                                                }
                                                else if (ResguardoCompraB1.descuentoGeneralExtranjero > 0)
                                                {
                                                    datosPdf.DescuentoExtranjero = ResguardoCompraB1.descuentoGeneralExtranjero;
                                                }


                                                //ProgressBar(ProgBar, "Generando Documento Electronico...", 80000);

                                                this.EnviarDocumento(cfe, cae, datosPdf, "RPC1", resguardoPdf, "ORPC");


                                                ActulizoFormaPago();


                                                cfe = null;
                                                cae = null;
                                            }
                                        }

                                    }
                                }
                                #endregion RESGUARDO COMPRA

                                #region Transacciones_Periodicas
                                if (BusinessObjectInfo.Type.Equals("112")) //Facturas
                                {
                                    string formaPago = "";
                                    if (((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected != null)
                                    {
                                        formaPago = ((ComboBox)app.Forms.Item(BusinessObjectInfo.FormUID).Items.Item("cbxForPag").Specific).Selected.Value + "";
                                    }

                                    FacturaB1 factura = new FacturaB1();
                                    factura.ActualizarOrigenBorrador(numeroDocumentoCreado, BusinessObjectInfo.Type, "Pendiente", formaPago);
                                }
                                #endregion Transacciones_Periodicas

                            }
                        }
                    }
                }

                    #endregion DATA_ADD

                #region DATA_LOAD

                //Al cargar los datos de un documento
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    if (!BusinessObjectInfo.BeforeAction)
                    {
                        if (BusinessObjectInfo.FormTypeEx.Equals("133") || BusinessObjectInfo.FormTypeEx.Equals("65303") ||
                            BusinessObjectInfo.FormTypeEx.Equals("179") || BusinessObjectInfo.FormTypeEx.Equals("140") ||
                            BusinessObjectInfo.FormTypeEx.Equals("60091"))
                        {
                            string var = BusinessObjectInfo.FormUID;

                            //Recorrer la lista de instancias de documentos para ubicar la que corresponde
                            //al documento que se esta ejecutando
                            foreach (FrmDocumento frmDocumento in listaInstanciasDocumentos)
                            {
                                if (frmDocumento.IdFormulario == BusinessObjectInfo.FormUID && frmDocumento.TypeEx == BusinessObjectInfo.FormTypeEx)
                                {
                                    if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                                    {
                                        frmDocumento.DesbloquearCamposCFE();
                                    }
                                    else
                                    {
                                        frmDocumento.BloquearCamposCFE();
                                    }

                                    frmDocumento.CambiarEstadoCamposReferencia(false, BusinessObjectInfo.FormTypeEx);
                                    //frmDocumento.CargarAdenda(BusinessObjectInfo.FormTypeEx);
                                    Form formularioActivo = app.Forms.ActiveForm;


                                    if (CampoNoNull(formularioActivo))
                                    {
                                        docNumPDf = ((EditText)formularioActivo.Items.Item("8").Specific).Value + "";
                                        ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;
                                        formularioActivo.Items.Item("cbElc").Enabled = false;

                                       
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion DATA_LOAD

                
            }


        }



        //*****************Eventos de Menus******************//
        void SBO_Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {

                #region ADMIN CAE

                if (pVal.MenuUID.Equals("mACAE") || pVal.MenuUID.Equals("mACAEU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmAdminCAE.MostarFormulario("frmCAE", RutasFormularios.FRM_ADMIN_CAE);
                    }
                }

                #endregion ADMIN CAE

                #region VISUALIZAR RANGOS

                if (pVal.MenuUID.Equals("mVRAN") || pVal.MenuUID.Equals("mVRANU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmVisualizarRangos.MostarFormulario("frmVRan", RutasFormularios.FRM_VISUALIZAR_RANGOS);
                    }
                }

                #endregion VISUALIZAR RANGOS

                #region CONFIGURACION FTP

                if (pVal.MenuUID.Equals("mCFTP"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmFtp.MostarFormulario("frmFTP", RutasFormularios.FRM_FTP);
                    }
                }

                #endregion CONFIGURACION FTP

                #region TIPOS DE DOCUMENTOS A CONSERVAR

                //Al precionar el menu de tipos de documentos a conservar
                if (pVal.MenuUID.Equals("mTDC"))
                {
                    if (pVal.BeforeAction)
                    {
                        frmDocCon.MostarFormulario("frmDocCon", RutasFormularios.FRM_DOC_CON);
                    }
                }

                #endregion TIPOS DE DOCUMENTOS A CONSERVAR

                #region RETENCION/PERCEPCION

                if (pVal.MenuUID.Equals("mRP"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmRetPer.MostarFormulario("frmRetPer", RutasFormularios.FRM_RETENCION_PERCEPCION);
                    }
                }

                #endregion RETENCION/PERCEPCION


                #region SUCURSALES DIRECCIONES

                if (pVal.MenuUID.Equals("mSD"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmSucuDire.MostarFormulario("frmSucuDire", RutasFormularios.FRM_SUCURSALES_DIRECCIONES);
                    }
                }

                #endregion SUCURSALES DIRECCIONES

                #region DOCUMENTOS

                //Al presionar el menu "Nuevo"
                if (pVal.MenuUID.Equals("1282"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Captura el error en caso de que se el formulario no se de un documento valido
                        try
                        {
                            foreach (FrmDocumento frmDocumento in listaInstanciasDocumentos)
                            {
                                //Bloquea los campos de CFE
                                if (FrmEstadoContingencia.estadoContingencia.Equals("N"))
                                {
                                    frmDocumento.BloquearCamposCFE();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (pVal.MenuUID.Equals("2053"))
                {
                    //ValidarTipoCAEExistente(CAE.ESTipoCFECFC.EFactura);
                }

                #endregion DOCUMENTOS

                #region MONITOR DE CERTIFICADOS

                if (pVal.MenuUID.Equals("mMONC") || pVal.MenuUID.Equals("mMONCU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmMonitor.MostarFormulario("frmMon", RutasFormularios.FRM_MONITOR);
                    }
                }

                #endregion MONITOR DE CERTIFICADOS

                #region MONITOR DE REPORTE

                if (pVal.MenuUID.Equals("mMONR") || pVal.MenuUID.Equals("mMONRU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmMonitorReporte.MostarFormulario("frmMonRp", RutasFormularios.FRM_MONITOR_REPORTE);
                    }
                }

                #endregion MONITOR DE REPORTE

                #region MONITOR DE CERTIFICADOS ANULADOSDGI

                if (pVal.MenuUID.Equals("mMONA") || pVal.MenuUID.Equals("mMONAU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmMonitorAnulado.MostarFormulario("frmMonAnu", RutasFormularios.FRM_MONITOR_ANULADO);
                    }
                }

                #endregion MONITOR DE CERTIFICADOS ANULADOSDGI

                #region CONFIGURACION CERTIFICADOS DIGITALES

                if (pVal.MenuUID.Equals("mCD") || pVal.MenuUID.Equals("mCDU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmCerDig.MostarFormulario("frmCerDig", RutasFormularios.FRM_CERTIFICADOS_DIGITALES);
                    }
                }

                #endregion CONFIGURACION CERTIFICADOS DIGITALES

                #region MONITOR SOBRES

                if (pVal.MenuUID.Equals("mMONS") || pVal.MenuUID.Equals("mMONSU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmMonitorSobre.MostarFormulario("frmMonSob", RutasFormularios.FRM_MONITOR_SOBRE);
                    }
                }

                #endregion MONITOR SOBRES

                #region MONITOR CERTIFICADOS CONTINGENCIA

                if (pVal.MenuUID.Equals("mCCTG") || pVal.MenuUID.Equals("mCCTGU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmMonCerContingencia.MostarFormulario("frmCerCon", RutasFormularios.FRM_MONITOR_CERTIFICADOS_CONTNGENCIA);
                    }
                }

                #endregion MONITOR CERTIFICADOS CONTINGENCIA

                #region CERTIFICADOS RECIBIDOS

                if (pVal.MenuUID.Equals("mSOF") || pVal.MenuUID.Equals("mSOFU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmCertificadoRecibido.MostarFormulario("frmCerRec", RutasFormularios.FRM_CERTIFICADO_RECIBIDO);
                    }
                }

                #endregion CERTIFICADOS RECIBIDOS

                #region CONTINGENCIA

                //Estado de la contingencia
                if (pVal.MenuUID.Equals("mECTG") || pVal.MenuUID.Equals("mECTGU"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmEstadoContingencia.MostarFormulario("frmEstCont", RutasFormularios.FRM_ESTADO_CONTINGENCIA);
                    }
                }

                #endregion CONTINGENCIA

                #region ENVIO CORREO ELECTRONICO

                if (pVal.MenuUID.Equals("mECA"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmEnvioCorreoElectronico.MostarFormulario("frmECA", RutasFormularios.FRM_ENVIO_CORREO_ELECTRONICO);
                    }
                }

                #endregion ENVIO CORREO ELECTRONICO

                #region LOGO

                if (pVal.MenuUID.Equals("mLOGO"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmLogo.MostarFormulario("frmLogo", RutasFormularios.FRM_LOGO);
                    }
                }

                #endregion LOGO

                #region ADOBE

                //if (pVal.MenuUID.Equals("mADRE"))
                //{
                //    if (!pVal.BeforeAction)
                //    {
                //        frmAdobe.MostarFormulario("frmAdoRea", RutasFormularios.FRM_ADOBE);
                //    }
                //}

                #endregion LOGO

                #region AUTORIZACION DOCUMENTOS NO ELECTRONICOS

                if (pVal.MenuUID.Equals("mADNE"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmAutoDocNoElectronico.MostarFormulario("frmAutoNoElec", RutasFormularios.FRM_ESTADO_DOCS_NO_ELECTRONICOS);
                    }
                }

                #endregion AUTORIZACION DOCUMENTOS NO ELECTRONICOS

                #region INDICADORES IMPUESTO

                if (pVal.MenuUID.Equals("mII"))
                {
                    if (pVal.BeforeAction)
                    {
                        frmImpuestosdgiB1.MostarFormulario("frmImpDgi", RutasFormularios.FRM_IMPUESTOS_DGI_B1);
                    }
                }

                #endregion INDICADORES IMPUESTO

                #region FORMA PAGO

                if (pVal.MenuUID.Equals("mFPG"))
                {
                    if (pVal.BeforeAction)
                    {
                        frmFormaPago.MostarFormulario("frmFormaPago", RutasFormularios.FRM_FORMA_PAGO);
                    }
                }

                #endregion FORMA PAGO

                #region CONFIGURACION ALERTA FIN CAE

                if (pVal.MenuUID.Equals("mCFCA"))
                {
                    if (pVal.BeforeAction)
                    {
                        frmConfFinCae.MostarFormulario("frmConCae", RutasFormularios.FRM_CONFIGURACION_FIN_CAE);
                    }
                }

                #endregion CONFIGURACION ALERTA FIN CAE

                #region ENVIO DGI COMPROBANTES FISCALES PENDIENTES

                if (pVal.MenuUID.Equals("mEDCP"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmEnvioDGICfes.MostarFormulario("frmEnvCom", RutasFormularios.FRM_ENVIO_DGI_CFES);
                    }
                }

                #endregion ENVIO DGI COMPROBANTES FISCALES PENDIENTES

                #region RAZON REFERENCIA NOTAS DE CREDITO

                if (pVal.MenuUID.Equals("mRRNC"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmRazonReferenciaNC.MostarFormulario("frmRazRefNC", RutasFormularios.FRM_RAZON_REFERENCIA_NC);
                    }
                }

                #endregion RAZON REFERENCIA NOTAS DE CREDITO

                #region CONFIGURACION REPORTE DIARIO

                if (pVal.MenuUID.Equals("mERD"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmConfRptd.MostarFormulario("frmConfRptd", RutasFormularios.FRM_CONF_REPORTE_DIARIO);
                    }
                }

                #endregion CONFIGURACION REPORTE DIARIO

                #region CONFIGURACION TIPO CAMBIO

                if (pVal.MenuUID.Equals("mCTC"))
                {
                    if (!pVal.BeforeAction)
                    {
                        frmConfTipoCambio.MostarFormulario("frmConfTipC", RutasFormularios.FRM_CONF_TIPO_CAMBIO);
                    }
                }

                #endregion CONFIGURACION TIPO CAMBIO

                #region Botones Navegacion

                //Evento del botones de navegacion
                //Evento del botones de navegacion
                if (pVal.MenuUID.Equals("1288") || pVal.MenuUID.Equals("1291") || pVal.MenuUID.Equals("1289") || pVal.MenuUID.Equals("1290"))
                {
                    Form formularioActivo = app.Forms.ActiveForm;

                    if (!pVal.BeforeAction)
                    {
                        if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("179") ||
                            formularioActivo.TypeEx.Equals("140") || formularioActivo.TypeEx.Equals("65303")
                            || formularioActivo.TypeEx.Equals("141") || formularioActivo.TypeEx.Equals("60091") || formularioActivo.TypeEx.Equals("181"))
                        {
                            formularioActivo.Freeze(true);
                            modoBusquedaNavegacion = true;
                            cierreFormulario = true;

                            string formaPago = "";
                            docNumPDf = ((EditText)formularioActivo.Items.Item("8").Specific).Value + "";

                            if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("65303"))
                            {
                                formaPago = DocumentoB1.ObtenerFormaPago("OINV", docNumPDf);
                            }
                            else if (formularioActivo.TypeEx.Equals("179"))
                            {
                                formaPago = DocumentoB1.ObtenerFormaPago("ORIN", docNumPDf);
                            }

                            if (formularioActivo.TypeEx.Equals("179") || formularioActivo.TypeEx.Equals("65300"))
                            {
                                formularioActivo.Items.Item("txtSeRef").Enabled = false;
                                formularioActivo.Items.Item("txtNumRef").Enabled = false;

                                formularioActivo.Items.Item("cbxRazRef").Enabled = false;
                                formularioActivo.Items.Item("stRazRef").Enabled = false;

                                formularioActivo.Items.Item("cbxTipDoc").Enabled = false;
                                formularioActivo.Items.Item("txtTipDoc").Enabled = false;
                            }

                            if (formaPago.Equals("Contado"))
                            {
                                ((ComboBox)formularioActivo.Items.Item("cbxForPag").Specific).
                                    Select("Contado", BoSearchKey.psk_ByValue);
                            }
                            else if (formaPago.Equals("Crédito"))
                            {
                                ((ComboBox)formularioActivo.Items.Item("cbxForPag").Specific).
                                    Select("Crédito", BoSearchKey.psk_ByValue);
                            }

                            if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("179") ||
                             formularioActivo.TypeEx.Equals("65303")|| formularioActivo.TypeEx.Equals("141") || formularioActivo.TypeEx.Equals("60091") || formularioActivo.TypeEx.Equals("181"))
                            {
                                formularioActivo.Items.Item("txtNumCFE").Enabled = false;
                                formularioActivo.Items.Item("txtSeCFE").Enabled = false;

                                formularioActivo.Items.Item("cbxTipDoc").Enabled = false;
                                formularioActivo.Items.Item("txtTipDoc").Enabled = false;

                            }


                            ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;


                            formularioActivo.Items.Item("cbElc").Enabled = false;

                            if (AdminEventosUI.modoUsuario)
                            {
                                formularioActivo.Items.Item("btnImp").Enabled = true;
                                //formularioActivo.Items.Item("cbElc").Enabled = true;
                            }
                            else
                            {
                                formularioActivo.Items.Item("btnImp").Enabled = true;
                                // formularioActivo.Items.Item("cbElc").Enabled = false;
                            }
                            formularioActivo.Freeze(false);

                            //if (formularioActivo.Mode != BoFormMode.fm_OK_MODE)
                            //{
                            //  formularioActivo.Mode = BoFormMode.fm_EDIT_MODE ;
                            //}

                        }
                    }
                    else
                    {
                        if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("179") ||
                            formularioActivo.TypeEx.Equals("140") || formularioActivo.TypeEx.Equals("65303")
                            || formularioActivo.TypeEx.Equals("141") || formularioActivo.TypeEx.Equals("60091") || formularioActivo.TypeEx.Equals("181"))
                        {
                            modoBusquedaNavegacion = true;
                            formularioActivo.Freeze(true);
                            cierreFormulario = true;


                            string formaPago = "";
                            docNumPDf = ((EditText)formularioActivo.Items.Item("8").Specific).Value + "";

                            if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("65303"))
                            {
                                formaPago = DocumentoB1.ObtenerFormaPago("OINV", docNumPDf);
                            }
                            else if (formularioActivo.TypeEx.Equals("179"))
                            {
                                formaPago = DocumentoB1.ObtenerFormaPago("ORIN", docNumPDf);
                            }

                            if (formularioActivo.TypeEx.Equals("179") || formularioActivo.TypeEx.Equals("65300"))
                            {
                                formularioActivo.Items.Item("txtSeRef").Enabled = false;
                                formularioActivo.Items.Item("txtNumRef").Enabled = false;

                                formularioActivo.Items.Item("cbxRazRef").Enabled = false;
                                formularioActivo.Items.Item("stRazRef").Enabled = false;

                                formularioActivo.Items.Item("cbxTipDoc").Enabled = false;
                                formularioActivo.Items.Item("txtTipDoc").Enabled = false;

                            }



                            if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("179") ||
                              formularioActivo.TypeEx.Equals("65303") || formularioActivo.TypeEx.Equals("141") || formularioActivo.TypeEx.Equals("60091") || formularioActivo.TypeEx.Equals("181"))
                            {
                                ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;
                                formularioActivo.Items.Item("cbElc").Enabled = false;

                                formularioActivo.Items.Item("txtNumCFE").Enabled = false;
                                formularioActivo.Items.Item("txtSeCFE").Enabled = false;

                                //formularioActivo.Items.Item("cbxTipDoc").Enabled = false;
                                //formularioActivo.Items.Item("txtTipDoc").Enabled = false;



                            }


                            if (formaPago.Equals("Contado"))
                            {
                                ((ComboBox)formularioActivo.Items.Item("cbxForPag").Specific).
                                    Select("Contado", BoSearchKey.psk_ByValue);
                            }
                            else if (formaPago.Equals("Crédito"))
                            {
                                ((ComboBox)formularioActivo.Items.Item("cbxForPag").Specific).
                                    Select("Crédito", BoSearchKey.psk_ByValue);
                            }


                            ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;
                            formularioActivo.Items.Item("cbElc").Enabled = false;

                            if (AdminEventosUI.modoUsuario)
                            {
                                formularioActivo.Items.Item("btnImp").Enabled = true;
                                // formularioActivo.Items.Item("cbElc").Enabled = true;
                            }
                            else
                            {
                                formularioActivo.Items.Item("btnImp").Enabled = true;
                                // formularioActivo.Items.Item("cbElc").Enabled = false;
                            }

                            formularioActivo.Freeze(false);

                            //if (formularioActivo.Mode != BoFormMode.fm_OK_MODE)
                            //{
                            //    formularioActivo.Mode = BoFormMode.fm_EDIT_MODE;
                            //}
                        }
                    }
                }
                #endregion Botones Navegacion

                #region Boton Nuevo

                //Evento del boton nuevo
                else if (pVal.MenuUID.Equals("1282"))
                {
                    if (!pVal.BeforeAction)
                    {
                        Form formularioActivo = app.Forms.ActiveForm;

                        if (formularioActivo.TypeEx.Equals("133"))
                        {
                            cierreFormulario = false;
                            modoBusquedaNavegacion = false;
                            ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;
                            formularioActivo.Items.Item("cbElc").Enabled = false;
                            formularioActivo.Items.Item("txtNumCFE").Enabled = true;
                            formularioActivo.Items.Item("txtSeCFE").Enabled = true;
                        }

                        if (formularioActivo.TypeEx.Equals("179") || formularioActivo.TypeEx.Equals("65300"))
                        {
                            formularioActivo.Items.Item("txtSeRef").Enabled = true;
                            formularioActivo.Items.Item("txtNumRef").Enabled = true;
                            ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;
                            formularioActivo.Items.Item("cbElc").Enabled = false;

                            formularioActivo.Items.Item("cbxRazRef").Enabled = true;
                            formularioActivo.Items.Item("stRazRef").Enabled = true;
                        }


                        if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("179") ||
                            formularioActivo.TypeEx.Equals("140") || formularioActivo.TypeEx.Equals("65303") ||
                            formularioActivo.TypeEx.Equals("141") || formularioActivo.TypeEx.Equals("60091") || formularioActivo.TypeEx.Equals("181"))
                        {
                            docNumPDf = "";
                            cierreFormulario = false;

                            formularioActivo.Freeze(true);
                            ((EditText)formularioActivo.Items.Item("txtTipDoc").Specific).Value = "";
                            formularioActivo.Freeze(false);

                            modoBusquedaNavegacion = false;
                            if (AdminEventosUI.modoUsuario)
                            {
                                //formularioActivo.Items.Item("cbElc").Enabled = true;
                            }
                            else
                            {
                                formularioActivo.Items.Item("cbElc").Enabled = false;
                            }

                            formularioActivo.Items.Item("btnImp").Enabled = false;



                            ManteUdoFormaPago manteUdoFormaPago = new ManteUdoFormaPago();
                            string formaPago = manteUdoFormaPago.ObtenerDocEntryFormaPago(false);


                            ActulizoFormaPago();

                        }


                        _NewDoc = true;
                    }
                }

                #endregion Boton Nuevo

                #region Boton Busqueda

                else if (pVal.MenuUID.Equals("1281"))
                {
                    if (!pVal.BeforeAction)
                    {
                        Form formularioActivo = app.Forms.ActiveForm;

                        if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("179") ||
                            formularioActivo.TypeEx.Equals("140") || formularioActivo.TypeEx.Equals("65303")
                            || formularioActivo.TypeEx.Equals("141") || formularioActivo.TypeEx.Equals("60091") || formularioActivo.TypeEx.Equals("181"))
                        {
                            docNumPDf = "";

                            formularioActivo.Freeze(true);
                            //string formaPago = ""; ///docNum = ((EditText)formularioActivo.Items.Item(8).Specific).Value + "";
                            formularioActivo.Freeze(true);
                            ((EditText)formularioActivo.Items.Item("txtTipDoc").Specific).Value = "";
                            formularioActivo.Freeze(false);


                            formularioActivo.Items.Item("btnImp").Enabled = false;

                            ((EditText)formularioActivo.Items.Item("8").Specific).Value = "";

                            formularioActivo.Freeze(false);

                            if (formularioActivo.TypeEx.Equals("133") || formularioActivo.TypeEx.Equals("65303"))
                            {
                                //formaPago = DocumentoB1.ObtenerFormaPago("OINV", docNum);
                                formularioActivo.Items.Item("txtNumCFE").Enabled = false;
                                formularioActivo.Items.Item("txtSeCFE").Enabled = false;

                                formularioActivo.Items.Item("cbxTipDoc").Enabled = false;
                                formularioActivo.Items.Item("txtTipDoc").Enabled = false;
                           
                            }

                            if (formularioActivo.TypeEx.Equals("179") || formularioActivo.TypeEx.Equals("65300"))
                            {
                                formularioActivo.Items.Item("cbxRazRef").Enabled = false;
                                formularioActivo.Items.Item("stRazRef").Enabled = false;

                                formularioActivo.Items.Item("cbxTipDoc").Enabled = false;
                                formularioActivo.Items.Item("txtTipDoc").Enabled = false;
                              
                            }

                            modoBusquedaNavegacion = true ;

                            if (AdminEventosUI.modoUsuario)
                            {
                                // formularioActivo.Items.Item("cbElc").Enabled = true;
                            }
                            else
                            {
                                formularioActivo.Items.Item("cbElc").Enabled = false;
                            }

                        }
                    }
                }

                #endregion Boton Busqueda
            }
            catch (Exception)
            {
            }
        }

        //*****************Eventos de Items******************//
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                /* Captura evento Copiar a desde Factura a NC clientes */
                if (pVal.FormTypeEx.Equals("133") && pVal.EventType == BoEventTypes.et_COMBO_SELECT && pVal.ItemUID.Equals("10000329") && pVal.BeforeAction)
                {
                    if (pVal.PopUpIndicator == 0)
                    {
                        clausulaDeVta = "FOB";
                        modalidadVta = "1";
                        viaTransporte = "2";
                    }
                }
                /*    */



                // Este codigo hace que cuando el formulario tenga el modo Activate obliga a refrescar el Combo
                // de socio de negocio, haciendo que se dispare de nuevo los evventos que hacen que se complete el RUT
                // de nuevo y quede siempre todo bien cargado.
                if (pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("181") || pVal.FormTypeEx.Equals("133")
                    || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("179"))
                {



                    if (pVal.ItemUID.Equals("4") && _NewDoc == true)
                    {


                        if (!pVal.BeforeAction)
                        {
                            Form formularioActivo = app.Forms.ActiveForm;
                            EditText codigoSN = (EditText)app.Forms.Item(pVal.FormUID).Items.Item("4").Specific;


                            seteoFormaPago(formularioActivo.TypeEx);


                            if (!codigoSN.Value.ToString().Equals(""))
                            {
                                string temp = codigoSN.Value + "";
                                codigoSN.Value = temp;
                            }


                            _NewDoc = false;
                        }
                    }
                }


                #region DETALLES DE LA SOCIEDAD

                //Al abrir formulario de detalles de la compania
                if (pVal.FormTypeEx.Equals("136") && pVal.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    if (!pVal.BeforeAction)
                    {
                        frmDetallesSociedad = new Interfaz.FrmDetallesSociedad();
                        frmDetallesSociedad.AgregarTabFE(FormUID);
                        frmDetallesSociedad.Consutar();
                    }
                }

                //Al presionar el botón actualizar del formulario de detalles de la compania
                if (pVal.FormTypeEx.Equals("136") && pVal.ItemUID.Equals("1"))
                {
                    if (pVal.BeforeAction)
                    {
                        //Valida que los valores hayan sido ingresados y tengan el formato correcto
                        if (!frmDetallesSociedad.Validar())
                        {
                            //Desactiva los eventos estándar
                            BubbleEvent = false;
                        }
                        else
                        {
                            //Si ya se han almacenado datos, estos se actualizar
                            if (frmDetallesSociedad.ExistenDatos())
                            {
                                //Actualiza los datos de la sociedad
                                frmDetallesSociedad.Actualizar();
                            }
                            //Si no existen datos almacenado se ingresa un nuevo registro
                            else
                            {
                                //Se ingresa un nuevo registro
                                frmDetallesSociedad.Almacenar();
                            }

                            //Activa los eventos estandar
                            BubbleEvent = true;
                        }
                    }
                }

                //Tab de factura electronica
                else if (pVal.ItemUID.Equals("tabFE"))
                {
                    if (pVal.BeforeAction)
                    {
                        frmDetallesSociedad.SeleccionarTabFe();
                    }
                }

                #endregion DETALLES DE LA SOCIEDAD

                #region DOCUMENTOS

                //Al cargar el formulario se crea el tab de adenda
                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("65303") || pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("140")
                    || pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("181"))
                {

                    if (pVal.EventType != BoEventTypes.et_FORM_ACTIVATE && pVal.EventType != BoEventTypes.et_LOST_FOCUS)
                    {
                        string a = "";

                    }

                    if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                    {
                        if (!pVal.BeforeAction)
                        {
                            modoBusquedaNavegacion = false;

                            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                            {
                                app.MessageBox("Alerta!!!! Documento en Contingencia, debe ingresar la serie y su respectivo número manualmente");
                            }

                            frmDocumento = new FrmDocumento();

                            //Crea los componentes del formulario del documeto
                            frmDocumento.CrearComponentes(pVal.FormUID, pVal.FormTypeEx);

                            //Agregar la instancia al arreglo
                            listaInstanciasDocumentos.Add(frmDocumento);

                            //Variable para almacenar el mensaje de la validacion
                            string mensaje = "";

                            Form formularioActivo = app.Forms.ActiveForm;
                            EditText codigoSN = (EditText)app.Forms.Item(pVal.FormUID).Items.Item("4").Specific;



                            if (!codigoSN.Value.ToString().Equals(""))
                            {
                                string temp = codigoSN.Value + "";
                                codigoSN.Value = temp;
                            }


                            if (!RutClienteCbo.ToString().Equals(""))
                            {
                                if (RutClienteCbo.ToString().Equals("RUT       "))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(0, BoSearchKey.psk_Index);
                                }


                                else if (RutClienteCbo.ToString().Equals("C.I.      "))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(1, BoSearchKey.psk_Index);
                                }

                                #region FE_EXPORTACION
                                else if (RutClienteCbo.ToString().Contains("Otros"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(2, BoSearchKey.psk_Index);
                                }
                                else if (RutClienteCbo.ToString().Contains("DNI"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(3, BoSearchKey.psk_Index);
                                }
                                else if (RutClienteCbo.ToString().Contains("Pasaporte"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(4, BoSearchKey.psk_Index);
                                }
                                #endregion FE_EXPORTACION

                            }


                            if (!formaPagoNC.ToString().Equals(""))
                            {
                                if (formaPagoNC.ToString().Equals("Contado"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxForPag").Specific).Select(1, BoSearchKey.psk_Index);
                                }
                                else if (formaPagoNC.ToString().Equals("Crédito"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxForPag").Specific).Select(0, BoSearchKey.psk_Index);
                                }


                                if (!FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                                {
                                    ////Caso Entrega
                                    //if (pVal.FormTypeEx.Equals("140"))
                                    //{
                                    //    //Se valida que existan CAES validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(0);
                                    //}
                                    ////Caso factura 


                                    //else if (pVal.FormTypeEx.Equals("133"))
                                    //{

                                    //    //Se valida que existan CAES validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(1);
                                    //}

                                    //else if (pVal.FormTypeEx.Equals("60091"))
                                    //{

                                    //    //Se valida que existan CAEs validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(1);
                                    //}
                                    ////Caso Nota Debito


                                    //else if (pVal.FormTypeEx.Equals("65303"))
                                    //{

                                    //    //Se valida que existan CAES validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(2);
                                    //}
                                    ////Caso Nota Credito


                                    //else if (pVal.FormTypeEx.Equals("179"))
                                    //{

                                    //    //Se valida que existan CAES validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(3);
                                    //}

                                    //else if (pVal.FormTypeEx.Equals("141"))
                                    //{

                                    //    //Se validan que existan CAES validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(1);
                                    //}


                                    //else if (pVal.FormTypeEx.Equals("181"))
                                    //{

                                    //    //Se validan que existan CAES validos para el documento
                                    //    mensaje = frmDocumento.ValidarCAEs(1);
                                    //}


                                    ////Se informa de los CAES no configurado o validos
                                    //if (!mensaje.Equals(""))
                                    //{

                                    //    //app.MessageBox(mensaje);
                                    //}
                                }
                                else
                                {
                                    if (!frmDocumento.ValidarCAEContingencia())
                                    {
                                        app.MessageBox(Mensaje.errValCAEsContingencia);
                                    }
                                }
                            }

                            if (viaTransporte != "")
                            {
                                /*  */
                                if (clausulaDeVta != "")
                                {
                                    ((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtClaVen").Specific).Value = clausulaDeVta;
                                    clausulaDeVta = "";
                                }
                                if (modalidadVta != "")
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxModVen").Specific).Select(modalidadVta, BoSearchKey.psk_Index);
                                    modalidadVta = "";
                                }
                                if (viaTransporte != "")
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxViaTran").Specific).Select(viaTransporte, BoSearchKey.psk_Index);
                                    viaTransporte = "";
                                }                                
                                /*  */
                            }
                        }
                    }
                }

                //Al dar clic en tab de adenda
                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("65303") ||
                    pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("140") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("181"))
                {
                    if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                    {
                        if (pVal.ItemUID.Equals("tabAdn"))
                        {
                            if (pVal.BeforeAction)
                            {
                                foreach (FrmDocumento frmDocumento in listaInstanciasDocumentos)
                                {
                                    if (frmDocumento.IdFormulario == pVal.FormUID && frmDocumento.TypeEx == pVal.FormTypeEx)
                                    {
                                        frmDocumento.SeleccionarTabAdenda();
                                    }
                                }
                            }
                        }
                    }
                }

                //Al cambiar el combo box de tipos de documentos receptor
                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("65303") ||
                    pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("140") ||
                    pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("181"))
                {
                    if (pVal.EventType == BoEventTypes.et_COMBO_SELECT || pVal.EventType == BoEventTypes.et_FORM_ACTIVATE)
                    {
                        if (pVal.ItemUID.Equals("cbxTipDoc"))
                        {
                            if (!pVal.BeforeAction)
                            {
                                string codigo = ((EditText)app.Forms.ActiveForm.Items.Item("4").Specific).Value + "";

                                if (codigo == "")
                                {
                                    //Obtiene el codigo del socio de negocio


                                    frmDocumento.CambiarEstadoValorDocumentoReceptor(codigo, RutClienteTxt);
                                }


                                //else
                                //{
                                //    comboBubbleEvent = false;
                                //}
                            }
                        }
                    }
                }

                //Al cambiar el combo box de tipos de documentos receptor



                //Al seleccionar check box de documento de contiengecia
                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("65303") ||
                    pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("140") ||
                    pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("181"))
                {
                    if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                    {
                        if (pVal.ItemUID.Equals("cbCont"))
                        {
                            if (!pVal.BeforeAction)
                            {
                                frmDocumento.CambiarEstadoDocumentoElectronico();
                            }
                        }
                        else if (pVal.ItemUID.Equals("btnImp"))
                        {
                            if (!pVal.BeforeAction)
                            {
                                if (docNumPDf.Equals(""))
                                {
                                    Form formularioActivo = app.Forms.ActiveForm;

                                    docNumPDf = ((EditText)formularioActivo.Items.Item("8").Specific).Value + "";
                                }
                                string tabla = "";
                                List<string> tipos = new List<string>();

                                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("60091"))
                                {
                                    tabla = "OINV";
                                    tipos.Add("111");
                                    tipos.Add("101");
                                }
                                else if (pVal.FormTypeEx.Equals("65303"))
                                {
                                    tipos.Add("113");
                                    tipos.Add("103");
                                }
                                else if (pVal.FormTypeEx.Equals("179"))
                                {
                                    tabla = "ORIN";
                                    tipos.Add("112");
                                    tipos.Add("102");
                                }
                                else if (pVal.FormTypeEx.Equals("140"))
                                {
                                    tabla = "ODLN";
                                    tipos.Add("181");
                                }


                                string docEntry = frmDocumento.ObtenerDocEntry(docNumPDf, tabla);
                                string archivoFirmado = frmDocumento.ObtenerPDFReimpresion(docEntry, tipos);



                                //  SAPbouiCOM.ProgressBar ProgBar = app.StatusBar.CreateProgressBar("Generando Documento Electronico...", 100000, false);




                                if (!archivoFirmado.Equals(""))
                                {
                                    Imprimir imprimir = new Imprimir();
                                    List<string> log = new List<string>();

                                    //  ProgressBar(ProgBar, "Imprimiendo Documento Electronico...", 90000);

                                    if (!imprimir.ImprimirPdf(RutasCarpetas.RutaCarpetaComprobantes + archivoFirmado, out log))
                                    {
                                        mostrarMensaje("Error al imprimir", tipoMensajes.error);
                                    }
                                    else
                                    {
                                        //   ProgressBar(ProgBar, "Imprimiendo  Documento Electronico...", 100000);                                         
                                    }


                                    System.IO.File.AppendAllLines(RutasCarpetas.RutaCarpetaLogImpresion + "LogImpresion.txt", log);
                                }
                                else
                                {
                                    mostrarMensaje("No se pudo obtener archivo para firmar", tipoMensajes.error);
                                }
                            }
                        }


                    }
                }

                //Al presionar crear en alguno de los documentos
                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("65303")
                    || pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("140")
                    || pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("181"))
                {
                    if (pVal.ItemUID.Equals("1") && pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                    {
                        if (pVal.BeforeAction)
                        {
                            //Recorrer la lista de instancias de documentos para ubicar la que corresponde al documento que se esta ejecutando
                            foreach (FrmDocumento frmDocumento in listaInstanciasDocumentos)
                            {
                                if (frmDocumento.IdFormulario == pVal.FormUID && frmDocumento.TypeEx == pVal.FormTypeEx)
                                {
                                    //Obtiene valor que indica si se debe crear el documento electronico
                                    crearDocumentoElectronico = frmDocumento.ValidarDocumentoElectronico();

                                    if (crearDocumentoElectronico)
                                    {
                                        Form formularioActivo = app.Forms.ActiveForm;
                                        Item socioNegocio = app.Forms.Item(pVal.FormUID).Items.Item("4");
                                        EditText codigoSN = (EditText)socioNegocio.Specific;
                                        Item txtItemItem = app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc");
                                        EditText etItemItem = (EditText)txtItemItem.Specific;
                                        string cedulaJuridica = etItemItem.Value + "";
                                        string tipoSN = string.Empty;
                                        Item Total = app.Forms.Item(pVal.FormUID).Items.Item("29");
                                        EditText etTotal = (EditText)Total.Specific;

                                        Boolean CF = false;
                                        formularioActivo.Items.Item("cbElc").Enabled = false;
                                        ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;


                                        Item TipoCambio = app.Forms.Item(pVal.FormUID).Items.Item("64");
                                        EditText etTipoCambio = (EditText)TipoCambio.Specific;

                                        Item Moneda = app.Forms.Item(pVal.FormUID).Items.Item("63");
                                        IComboBox etMoneda = (IComboBox)Moneda.Specific;


                                        /*WsWoow AH 08.06.2016*/
                                        Item tipoDoc = app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc");
                                        string strTipoDoc = ((ComboBox)tipoDoc.Specific).Value.Trim();
                                        /*WsWoow 08.06.2016*/



                                        ///PARTE NUEVA PROBANDO
                                        if (manteDocumentos.ValidarClienteContado(codigoSN.Value.ToString()))
                                        {
                                            tipoSN = "CI";
                                            CF = true;
                                        }
                                        else
                                        {
                                            tipoSN = "RUT";
                                        }

                                        //Valida que el tipo documento receptor sea valido
                                        if (!modoBusquedaNavegacion && (!frmDocumento.ValidarDocumentoReceptor() || !frmDocumento.ValidarReferncia(pVal.FormTypeEx)))
                                        {
                                            //Desactiva los eventos estandar
                                            BubbleEvent = false;
                                        }
                                        else if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                                        {
                                            bool bandera = true;

                                            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                                            {
                                                if (pVal.FormTypeEx.Equals("133"))
                                                {
                                                    bandera = frmDocumento.ValidarNumeroSerie(formularioActivo, "999");
                                                }
                                                else if (pVal.FormTypeEx.Equals("179"))
                                                {
                                                    bandera = frmDocumento.ValidarNumeroSerie(formularioActivo, "999");
                                                }
                                                else if (pVal.FormTypeEx.Equals("140"))
                                                {
                                                    bandera = frmDocumento.ValidarNumeroSerie(formularioActivo, "999");
                                                }
                                                else if (pVal.FormTypeEx.Equals("65303"))
                                                {
                                                    bandera = frmDocumento.ValidarNumeroSerie(formularioActivo, "999");
                                                }
                                                else if (pVal.FormTypeEx.Equals("141"))
                                                {
                                                    bandera = frmDocumento.ValidarNumeroSerie(formularioActivo, "999");
                                                }
                                                else if (pVal.FormTypeEx.Equals("181"))
                                                {
                                                    bandera = frmDocumento.ValidarNumeroSerie(formularioActivo, "999");
                                                }
                                            }

                                            if (!bandera)
                                            {
                                                mostrarMensaje(Mensaje.errNumRanUti, tipoMensajes.error);
                                                BubbleEvent = false;
                                            }
                                            else
                                            {
                                                //Activa los eventos estandar
                                                BubbleEvent = true;
                                            }
                                        }
                                        else if (!modoBusquedaNavegacion && !strTipoDoc.Equals("RUT") && CF == false)
                                        {
                                            #region FE_EXPORTACION
                                            /*mostrarMensaje("Error: No se puede seleccionar este tipo de Documento Receptor, el Socio de Negocio debe ser consumidor final", tipoMensajes.error);
                                            BubbleEvent = false;*/
                                            if (!manteDocumentos.ValidarClienteExtranjero(codigoSN.Value.ToString()))
                                            {
                                                mostrarMensaje("Error: No se puede seleccionar este tipo de Documento Receptor, el Socio de Negocio debe ser consumidor final", tipoMensajes.error);
                                                BubbleEvent = false;
                                            }
                                            else
                                            {
                                                //Validaciones FE Exportacion, campos obligatorios!
                                                int valExpo = 0;

                                                //Caso Entrega (Remito)
                                                if (pVal.FormTypeEx.Equals("140"))
                                                {
                                                    valExpo = frmDocumento.ValidarCamposExportacion(0);
                                                }
                                                //Caso factura 
                                                else if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("181"))
                                                {
                                                    valExpo = frmDocumento.ValidarCamposExportacion(1);
                                                }
                                                //Caso Nota Debito
                                                else if (pVal.FormTypeEx.Equals("65303"))
                                                {
                                                    valExpo = frmDocumento.ValidarCamposExportacion(2);
                                                }
                                                //Caso Nota Credito
                                                else if (pVal.FormTypeEx.Equals("179"))
                                                {
                                                    valExpo = frmDocumento.ValidarCamposExportacion(3);
                                                }

                                                if (valExpo == 0)
                                                {
                                                    BubbleEvent = true;
                                                }
                                                else
                                                {
                                                    switch (valExpo)
                                                    {
                                                        case 1:
                                                            mostrarMensaje(Mensaje.errorClaVenta, tipoMensajes.error);
                                                            break;
                                                        case 2:
                                                            mostrarMensaje(Mensaje.errorModVenta, tipoMensajes.error);
                                                            break;
                                                        case 3:
                                                            mostrarMensaje(Mensaje.errorViaTransporte, tipoMensajes.error);
                                                            break;
                                                        case 4:
                                                            mostrarMensaje(Mensaje.errorIndTipoBienes, tipoMensajes.error);
                                                            break;
                                                        case 7:
                                                            mostrarMensaje(Mensaje.errorLargoClaVenta, tipoMensajes.error);
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    BubbleEvent = false;
                                                }
                                            }
                                            #endregion FE_EXPORTACION
                                        }

                                        else if (!frmDocumento.ValidarCedulaJuridica(cedulaJuridica, tipoSN, etTotal.Value + "", etTipoCambio.Value + "", etMoneda.Value + ""))
                                        {
                                            #region Proceso_WebService
                                            //if (!modoBusquedaNavegacion)
                                            //{
                                            //    mostrarMensaje("Error de Formato: RUT 12 números y C.I. 8 números", tipoMensajes.error);
                                            //    BubbleEvent = false;
                                            //}
                                            if (strTipoDoc.Equals("RUT") || strTipoDoc.Equals("C.I."))
                                            {
                                                if (!modoBusquedaNavegacion)
                                                {
                                                    mostrarMensaje("Error de Formato: RUT 12 números y C.I. 8 números", tipoMensajes.error);

                                                    BubbleEvent = false;
                                                }

                                            }
                                            #endregion Proceso_WebService
                                        }
                                        else
                                        {
                                            //frmDocumento.AlmacenarAdenda(pVal.FormTypeEx);
                                            BubbleEvent = true;


                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Al disparase cualquiera de los eventos listados se debe bloquear los campos de CFE
                    if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("65303") ||
                        pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("140") || pVal.FormTypeEx.Equals("60091"))
                    {
                        if (pVal.EventType == BoEventTypes.et_CLICK || pVal.EventType == BoEventTypes.et_KEY_DOWN || pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (!pVal.BeforeAction)
                            {
                                //Recorrer la lista de instancias de documentos para ubicar la que corresponde al documento que se esta ejecutando y bloquear campos de CFE
                                foreach (FrmDocumento frmDocumento in listaInstanciasDocumentos)
                                {
                                    if (frmDocumento.IdFormulario == pVal.FormUID && frmDocumento.TypeEx == pVal.FormTypeEx)
                                    {
                                        if (FrmEstadoContingencia.estadoContingencia.Equals("N") || FrmEstadoContingencia.estadoContingencia.Equals(""))
                                        {
                                            frmDocumento.BloquearCamposCFE();
                                        }
                                        else
                                        {
                                            frmDocumento.DesbloquearCamposCFE();
                                        }


                                    }
                                }
                            }
                        }
                    }


                    //Formulario de Facturas de Sistema
                    if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("179")
                        || pVal.FormTypeEx.Equals("65303") || pVal.FormTypeEx.Equals("140")
                        || pVal.FormTypeEx.Equals("141") || pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("181"))
                    {

                        if (pVal.EventType != BoEventTypes.et_FORM_ACTIVATE && pVal.EventType != BoEventTypes.et_LOST_FOCUS)
                        {
                            string codigo2 = "";

                        }



                        ////Evento Focus Perdido
                        if (pVal.EventType == BoEventTypes.et_LOST_FOCUS || (pVal.EventType == BoEventTypes.et_COMBO_SELECT))
                        {
                            if (!pVal.BeforeAction)
                            {

                                foreach (FrmDocumento frmDocumento in listaInstanciasDocumentos)
                                {




                                    if (frmDocumento.IdFormulario == pVal.FormUID && frmDocumento.TypeEx == pVal.FormTypeEx)
                                    {


                                        if (pVal.ItemUID.Equals("8"))
                                        {
                                            seteoFormaPago(pVal.FormTypeEx);
                                        }


                                        if (pVal.ItemUID.Equals("4"))
                                        {

                                            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                                            {

                                                if (!pVal.FormTypeEx.Equals("141") || !pVal.FormTypeEx.Equals("181"))
                                                {


                                                    frmDocumento.DesbloquearCamposCFE();

                                                }


                                            }
                                            else
                                            {
                                                if (!pVal.FormTypeEx.Equals("141"))
                                                {
                                                    frmDocumento.BloquearCamposCFE();
                                                }


                                                if (!pVal.FormTypeEx.Equals("181"))
                                                {

                                                    frmDocumento.BloquearCamposCFE();
                                                }
                                            }

                                            //codigo
                                            app.Forms.Item(pVal.FormUID).Freeze(true);

                                            EditText prueba = (EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("4").Specific;

                                            ((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;


                                            seteoCheck();




                                            string codigo = prueba.Value;//((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("4").Specific).Value.ToString();

                                            if (!codigo.Equals(""))
                                            {
                                                //Se obtiene y asigna la cedula juridica del socio de negocio                                                        
                                                SocioNegocio datosSN = frmDocumento.ObtenerDatosSN(codigo);

                                                EditText txtEntregador = (EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("222").Specific;

                                                if (txtEntregador.Value.Equals("") || txtEntregador.Value.Equals(null))
                                                {
                                                    txtEntregador.Value = datosSN.Entregador;
                                                    comboBubbleEvent = true;
                                                }

                                                Item tipoDoc = app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc");
                                                string strTipoDoc = ((ComboBox)tipoDoc.Specific).Value.Trim();

                                                ((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;

                                                app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Enabled = false;

                                                if (datosSN.ConsumidorFinal.Equals("Y"))
                                                {
                                                    if (strTipoDoc.Equals("") || (codigoSNAnterior != codigo))
                                                    {

                                                        ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(1, BoSearchKey.psk_Index);

                                                    }


                                                }
                                                else
                                                {
                                                    if (strTipoDoc.Equals("") || (codigoSNAnterior != codigo))
                                                    {

                                                        ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(0, BoSearchKey.psk_Index);

                                                    }
                                                }


                                                if (((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc").Specific).Value.Equals(""))
                                                {
                                                    if ((RutClienteTxt != "") && (RutClienteTxt != datosSN.CedulaJuridica))
                                                    {
                                                        ((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc").Specific).Value = RutClienteTxt;


                                                        app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Enabled = true;

                                                        ((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;

                                                        app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Enabled = false;

                                                    }
                                                    else
                                                    {
                                                        ((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc").Specific).Value = datosSN.CedulaJuridica;
                                                    }


                                                }

                                                else

                                                    if (codigoSNAnterior != codigo)
                                                    {
                                                        ((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc").Specific).Value = datosSN.CedulaJuridica;
                                                    }

                                            }

                                            if (codigoSNAnterior != codigo)
                                            {
                                                codigoSNAnterior = codigo;
                                            }

                                            frmDocumento.CargarAdenda(pVal.FormTypeEx, codigo);

                                            app.Forms.Item(pVal.FormUID).Freeze(false);

                                        }
                                        else if (pVal.ItemUID.Equals("54"))
                                        {
                                            if (!cierreFormulario)
                                            {

                                                app.Forms.Item(pVal.FormUID).Freeze(true);

                                                EditText SN = (EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("4").Specific;

                                                string codigo = SN.Value;

                                                ((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;

                                                app.Forms.Item(frmDocumento.IdFormulario);

                                                if (!codigo.Equals(""))
                                                {
                                                    //Se obtiene y asigna la cedula juridica del socio de negocio                                                        
                                                    SocioNegocio datosSN = frmDocumento.ObtenerDatosSN(codigo);

                                                    EditText txtEntregador = (EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("222").Specific;

                                                    if (txtEntregador.Value.Equals("") || txtEntregador.Value.Equals(null))
                                                    {
                                                        txtEntregador.Value = datosSN.Entregador;
                                                        comboBubbleEvent = true;
                                                    }



                                                    if (datosSN.ConsumidorFinal.Equals("Y"))
                                                    {
                                                        ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(1, BoSearchKey.psk_Index);
                                                    }
                                                    else
                                                    {
                                                        #region FE_EXPORTACION
                                                        //((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(0, BoSearchKey.psk_Index);                                                        
                                                        if (!datosSN.ClienteExtranjero)
                                                        {
                                                            ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(0, BoSearchKey.psk_Index);
                                                        }
                                                        /*else
                                                        {
                                                            ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxTipDoc").Specific).Select(2, BoSearchKey.psk_Index);
                                                        }*/
                                                        #endregion FE_EXPORTACION
                                                    }




                                                    if ((RutClienteTxt != "") && (RutClienteTxt != datosSN.CedulaJuridica))
                                                    {
                                                        ((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc").Specific).Value = RutClienteTxt;

                                                        app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Enabled = true;

                                                        ((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;

                                                        app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Enabled = false;
                                                    }
                                                    else
                                                    {
                                                        ((EditText)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("txtTipDoc").Specific).Value = datosSN.CedulaJuridica;
                                                    }
                                                }

                                                app.Forms.Item(pVal.FormUID).Freeze(false);

                                            }

                                            else
                                            {
                                                cierreFormulario = false;
                                            }
                                        }

                                    }

                                }




                                //Formulario de Facturas de Sistema
                                if (pVal.FormTypeEx.Equals("133"))
                                {
                                    if (!pVal.BeforeAction)
                                    {





                                        formaPagoNC = ((ComboBox)app.Forms.Item(pVal.FormUID).Items.Item("cbxForPag").Specific).Value + "";
                                        RutClienteCbo = ((ComboBox)app.Forms.Item(pVal.FormUID).Items.Item("cbxTipDoc").Specific).Value + "";
                                        RutClienteTxt = ((EditText)app.Forms.Item(pVal.FormUID).Items.Item("txtTipDoc").Specific).Value + "";






                                        //((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Item.Enabled = true;

                                        //((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;





                                        //((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Item.Enabled = false;




                                    }
                                }




                                //Pone check FE
                                if (pVal.FormTypeEx.Equals("133") || pVal.FormTypeEx.Equals("179"))
                                {
                                    if (!pVal.BeforeAction)
                                    {
                                        if (!modoBusquedaNavegacion)
                                        {

                                            //((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Item.Enabled = true;

                                            //((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Checked = true;


                                            //((CheckBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbElc").Specific).Item.Enabled = false;

                                        }
                                    }
                                }

                            }


                        }
                    }

                    //Formulario de Facturas de Sistema
                    if (pVal.FormTypeEx.Equals("133"))
                    {
                        if (!pVal.BeforeAction)
                        {
                            if (pVal.EventType == BoEventTypes.et_FORM_DEACTIVATE)
                            {
                                formaPagoNC = ((ComboBox)app.Forms.Item(pVal.FormUID).Items.Item("cbxForPag").Specific).Value + "";
                            }
                        }
                    }
                }

                #endregion DOCUMENTOS

                #region ADMIN CAE

                if (pVal.FormTypeEx.Equals("frmCAE"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton nuevo rango
                            if (pVal.ItemUID.Equals("btnNue"))
                            {
                                frmNuevoCAE.MostarFormulario("frmNCAE", RutasFormularios.FRM_NUEVO_CAE);
                            }
                            //Boton abrir formulario actualiza rango seleccionado
                            else if (pVal.ItemUID.Equals("btnAct"))
                            {
                                //Valida que el formulario no esté activo
                                if (!frmActualizarCAE.FormularioActivo)
                                {
                                    //Obtiene el rango seleccionado
                                    string rangoSeleccionado = frmAdminCAE.ObtenerRangoSeleccionado();

                                    //Verifica que el rango no haya sido utilizado
                                    if (!frmAdminCAE.ValidarRangoUtilizado(rangoSeleccionado))
                                    {
                                        if (!rangoSeleccionado.Equals(""))
                                        {
                                            frmActualizarCAE.MostarFormulario("frmACAE", RutasFormularios.FRM_ACTUALIZAR_CAE);
                                            frmActualizarCAE.NumeroRangoSeleccionado = rangoSeleccionado;
                                            frmActualizarCAE.CrearComponentess("frmACAE", frmActualizarCAE.NumeroRangoSeleccionado);
                                        }
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.errRangoUtilizadoModificar, tipoError);
                                    }
                                }
                                else
                                {
                                    //Selecciona el formulario 
                                    frmAdminCAE.SeleccionarFormulario();
                                }
                            }
                            //Boton eliminar el rango seleccionado
                            else if (pVal.ItemUID.Equals("btnEli"))
                            {
                                //Obtiene el rango seleccionado
                                string rangoSeleccionado = frmAdminCAE.ObtenerRangoSeleccionado();

                                if (!rangoSeleccionado.Equals(""))
                                {
                                    //Verifica que el rango no haya sido utilizado
                                    if (!frmAdminCAE.ValidarRangoUtilizado(rangoSeleccionado))
                                    {
                                        //Preguntar al usuario si desea eliminar el rango seleccionado
                                        int respuesta = app.MessageBox(Mensaje.preConfirmaEliminarRango, 1, "Si", "No");

                                        //Si la respuesta es afirmativa...
                                        if (respuesta == 1)
                                        {
                                            //Elimina el rango selecciona y obtiene el resultado de la operacion
                                            bool resultado = frmAdminCAE.EliminarRango(rangoSeleccionado);

                                            if (resultado)
                                            {
                                                frmAdminCAE.RefrescarFormulario(FormUID);
                                                //Muestra mensaje de informacion
                                                mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                            }
                                            else if (respuesta == 1)//Error al almacenar
                                            {
                                                //Muestra mensaje de error
                                                mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Muestra mensaje de error
                                        mostrarMensaje(Mensaje.errRangoUtilizadoEliminar, tipoError);
                                    }
                                }
                            }
                            //Boton actualizar rangos activos
                            else if (pVal.ItemUID.Equals("cbAct"))
                            {
                                frmAdminCAE.RefrescarFormulario(FormUID);
                            }
                            //Boton OK
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Cerrar la ventana de rangos
                                frmAdminCAE.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmAdminCAE.CerrarFormulario();
                        }
                    }
                }

                #endregion ADMIN CAE

                #region NUEVO CAE

                if (pVal.FormUID.Equals("frmNCAE"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                int respuesta = frmNuevoCAE.Almacenar();

                                if (respuesta == 0)//Valores almacenado
                                {
                                    //Cierra el formulario
                                    frmNuevoCAE.CerrarFormulario();

                                    //Actualizar listado de rangos
                                    frmAdminCAE.RefrescarFormulario("frmCAE");

                                    //Muestra mensaje de informacion
                                    mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                }
                                else if (respuesta == 1)//Error al almacenar
                                {
                                    //Muestra mensaje de error
                                    mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                }
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                //Cierra el formulario
                                frmNuevoCAE.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmNuevoCAE.CerrarFormulario();
                        }
                    }
                }

                #endregion NUEVO CAE

                #region ACTUALIZAR CAE

                if (pVal.FormTypeEx.Equals("frmACAE"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                int respuesta = frmActualizarCAE.Actualizar(frmActualizarCAE.NumeroRangoSeleccionado);

                                if (respuesta == 0)//Valores almacenado
                                {
                                    //Cierra el formulario
                                    frmActualizarCAE.CerrarFormulario();

                                    //Actulizar listado de rangos
                                    frmAdminCAE.RefrescarFormulario("frmCAE");

                                    //Muestra mensaje de informacion
                                    mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                }
                                else if (respuesta == 1)//Error al almacenar
                                {
                                    //Muestra mensaje de error
                                    mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                }
                            }
                        }
                    }
                    else if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton cancelar
                            if (pVal.ItemUID.Equals("btnCan"))
                            {
                                //Cierra el formulario
                                frmActualizarCAE.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            //Cierra el formulario
                            frmActualizarCAE.CerrarFormulario();
                        }
                    }
                }

                #endregion ACTUALIZAR CAE

                #region VISUALIZAR RANGOS

                if (pVal.FormUID.Equals("frmVRan"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption.Equals("Guardar"))
                                {
                                    bool banderaAsig = false;

                                    if (frmVisualizarRangos.ActualizarRangosActivos("grdAsi", "Y"))
                                    {
                                        banderaAsig = true;
                                    }
                                    else
                                    {
                                        mostrarMensaje("Error al guardar los CAEs asignados", tipoMensajes.error);
                                    }

                                    if (frmVisualizarRangos.ActualizarRangosActivos("grdQui", "N") && banderaAsig)
                                    {
                                        ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "OK";
                                    }
                                    else
                                    {
                                        mostrarMensaje("Error al guardar los CAEs no asignados", tipoMensajes.error);
                                    }
                                }
                                else
                                {
                                    frmVisualizarRangos.CerrarFormulario();
                                }
                            }
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmVisualizarRangos.CerrarFormulario();
                            }
                            else if (pVal.ItemUID.Equals("btnQui"))
                            {
                                frmVisualizarRangos.QuitarCAE();
                            }
                            else if (pVal.ItemUID.Equals("btnAsi"))
                            {
                                int fila = -1;

                                if (frmVisualizarRangos.ValidarCAE(out fila))
                                {
                                    frmVisualizarRangos.AsignarCAE(fila);
                                }
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmVisualizarRangos.CerrarFormulario();
                        }
                    }
                }

                #endregion VISUALIZAR RANGOS

                #region CONFIGURACION FTP

                if (pVal.FormTypeEx.Equals("frmFTP"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Almacenar o acturalizar los datos
                                int resultado = frmFtp.Almacenar();

                                if (resultado == 0)
                                {
                                    //Cerrar el formulario
                                    frmFtp.CerrarFormulario();

                                    //Muestra mensaje de informacion
                                    mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                }
                                else if (resultado == 1)
                                {
                                    //Muestra mensaje de error
                                    mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                }
                            }
                            //Boton Cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmFtp.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmFtp.CerrarFormulario();
                        }
                    }
                }

                #endregion CONFIGURACION FTP

                #region TIPOS DE DOCUMENTOS A CONSERVAR

                //Almacenar los datos y cerrar el formulario
                if (pVal.FormTypeEx.Equals("frmDocCon"))
                {
                    if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOk"))
                            {
                                //Almacena la informacion
                                frmDocCon.Actualizar();

                                //Muestra mensaje de informacion
                                mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);

                                frmDocCon.CerrarFormulario();
                            }
                            //Boton Cancelar 
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmDocCon.CerrarFormulario();
                            }
                        }
                        //Evento Cierre Formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmDocCon.CerrarFormulario();
                        }
                    }
                }

                #endregion TIPOS DE DOCUMENTOS A CONSERVAR

                #region RETENCION/PERCEPCION

                if (pVal.FormTypeEx.Equals("frmRetPer"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento key down
                        if (pVal.EventType == BoEventTypes.et_KEY_DOWN)
                        {
                            if (pVal.ItemUID.Equals("mtxRetPer"))
                            {
                                frmRetPer.EstadoBotonOK(false);
                            }
                        }
                    }
                    else if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Cuando el estado del boton es OK
                                if (frmRetPer.BotonOK)
                                {
                                    //Cerrar el formulario
                                    frmRetPer.CerrarFormulario();
                                }
                                //Cuando el estado del boton es Actualizar
                                else
                                {
                                    if (frmRetPer.Almacenar())
                                    {
                                        //Muestra mensaje de informacion
                                        mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);

                                        frmRetPer.EstadoBotonOK(true);
                                    }
                                    else
                                    {
                                        //Muestra mensaje de error
                                        mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                    }
                                }
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmRetPer.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmRetPer.CerrarFormulario();
                        }
                    }
                }

                #endregion RETENCION/PERCEPCION


                #region Sucursal Direccion

                if (pVal.FormTypeEx.Equals("frmSucuDire"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento key down
                        if (pVal.EventType == BoEventTypes.et_KEY_DOWN)
                        {
                            if (pVal.ItemUID.Equals("mtxSucDir"))
                            {
                                frmSucuDire.EstadoBotonOK(false);
                            }
                        }
                    }
                    else if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Cuando el estado del boton es OK
                                if (frmSucuDire.BotonOK)
                                {
                                    //Cerrar el formulario
                                    frmSucuDire.CerrarFormulario();
                                }
                                //Cuando el estado del boton es Actualizar
                                else
                                {
                                    if (frmSucuDire.Almacenar())
                                    {
                                        //Muestra mensaje de informacion
                                        mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);

                                        frmSucuDire.EstadoBotonOK(true);
                                    }
                                    else
                                    {
                                        //Muestra mensaje de error
                                        mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                    }
                                }
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmSucuDire.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmSucuDire.CerrarFormulario();
                        }
                    }
                }

                #endregion RETENCION/PERCEPCION


                #region MONITOR DE CERTIFICADOS

                if (pVal.FormUID.Equals("frmMon"))
                {
                    if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            #region BOTON MOSTRAR XML

                            //Boton mostrar Xml
                            if (pVal.ItemUID.Equals("btnXml"))
                            {
                                try
                                {
                                    int paneLevel = app.Forms.Item("frmMon").PaneLevel;

                                    Grid grdComprobantes = null;
                                    grdComprobantes = frmMonitor.obtenerGridNivel(paneLevel, FormUID);

                                    if (grdComprobantes != null)
                                    {
                                        if (grdComprobantes.Rows.SelectedRows.Count == 0)
                                        {
                                            if (grdComprobantes.DataTable.IsEmpty)
                                            {
                                                mostrarMensaje(Mensaje.warNoComprobantesVisualizar, tipoAdvertencia);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.warSeleccionComprobanteVisualizar, tipoAdvertencia);
                                            }
                                        }
                                        else
                                        {
                                            if (grdComprobantes.DataTable.IsEmpty)
                                            {
                                                mostrarMensaje(Mensaje.warNoComprobantesVisualizar, tipoAdvertencia);
                                            }
                                            else
                                            {
                                                string nombreArchivo = "", serie = "", numComp = "", tipoCFE = "";
                                                int fila = 0;

                                                while (fila < grdComprobantes.Rows.Count)
                                                {
                                                    if (grdComprobantes.Rows.IsSelected(fila))
                                                    {
                                                        serie = grdComprobantes.DataTable.Columns.Item("Serie").Cells.Item(fila).
                                                                    Value.ToString();

                                                        numComp = grdComprobantes.DataTable.Columns.Item("Número de Documento").
                                                            Cells.Item(fila).Value.ToString();

                                                        tipoCFE = grdComprobantes.DataTable.Columns.Item("Tipo de Documento").
                                                            Cells.Item(fila).Value.ToString();
                                                    }
                                                    fila++;
                                                }

                                                nombreArchivo = tipoCFE + serie + numComp + ".xml";

                                                if (!frmVisualizarActivo)
                                                {
                                                    FTP ftp = new FTP();

                                                    if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 8))
                                                    {
                                                        frmVisualizar = new FrmVisualizar();
                                                        frmVisualizarActivo = true;

                                                        frmVisualizar.mostrarFormulario();
                                                        frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                            nombreArchivo);
                                                    }
                                                    else
                                                    {
                                                        mostrarMensaje(Mensaje.errNoDescargaComprobantes + nombreArchivo +
                                                            Mensaje.errVerificarConexionFTP, tipoError);
                                                    }
                                                }
                                                else
                                                {
                                                    FTP ftp = new FTP();
                                                    if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 1))
                                                    {
                                                        frmVisualizar.seleccionarFormulario(RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                            nombreArchivo);
                                                    }
                                                    else
                                                    {
                                                        mostrarMensaje(Mensaje.errNoDescargaComprobantes + nombreArchivo +
                                                            Mensaje.errVerificarConexionFTP, tipoError);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.errNoDatosComprobante, tipoError);
                                    }
                                }
                                catch (Exception)
                                {
                                    mostrarMensaje(Mensaje.errNoDatosComprobante, tipoError);
                                }
                            }
                            #endregion BOTON MOSTRAR XML

                            //Tabs
                            else if (pVal.ItemUID.Equals("tab1") || pVal.ItemUID.Equals("tab2") || pVal.ItemUID.Equals("tab3") || pVal.ItemUID.Equals("tab4") || pVal.ItemUID.Equals("tab6") || pVal.ItemUID.Equals("tab7"))
                            {
                                //Selecciona el tab correspondiente
                                frmMonitor.SeleccionarTab(pVal.ItemUID);

                                //Activa o desactiva botones segun sea el caso
                                frmMonitor.EstablecerBotonesActivos(pVal.ItemUID);
                            }
                            //Boton OK
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Cierra el formulario
                                frmMonitor.CerrarFormulario();
                            }
                            //Boton visualiza el certificado rechazado
                            else if (pVal.ItemUID.Equals("btnVis"))
                            {
                                int paneLevel = app.Forms.Item("frmMon").PaneLevel;

                                #region PANE LEVEL 3

                                if (paneLevel == 3)
                                {
                                    Grid gridRechazadosDGI = (Grid)app.Forms.Item("frmMon").Items.Item("grid3").Specific;
                                    DataTable dtRecDGI = gridRechazadosDGI.DataTable;
                                    bool seleccionLinea = false;

                                    if (!dtRecDGI.IsEmpty)
                                    {
                                        for (int i = 0; i < gridRechazadosDGI.Rows.Count; i++)
                                        {
                                            if (gridRechazadosDGI.Rows.IsSelected(i))
                                            {
                                                seleccionLinea = true;

                                                //Se obtiene datos para realizar la consulta
                                                string serie = dtRecDGI.Columns.Item("Serie").Cells.Item(i).Value.ToString();

                                                string numDoc = dtRecDGI.Columns.Item("Número de Documento").Cells.Item(i).Value.ToString();

                                                string tipoCFE = dtRecDGI.Columns.Item("Tipo de Documento").Cells.Item(i).Value.ToString();

                                                ManteUdoComprobantes udoCerRechazado = new ManteUdoComprobantes();

                                                string certificadoRechazado = udoCerRechazado.obtenerCertificadoRechazado(serie, numDoc, tipoCFE, CFE.ESTipoReceptor.DGI.ToString());

                                                if (!certificadoRechazado.Equals(""))
                                                {
                                                    if (!frmVisualizarCertificado.FormularioActivo)
                                                    {
                                                        frmVisualizarCertificado.MostarFormulario("frmVisCer", RutasFormularios.FRM_VISUALIZAR_CERTIFICADO);
                                                        frmVisualizarCertificado.CrearComponentess(certificadoRechazado);
                                                        certificadoRechazado = null;
                                                    }
                                                    else
                                                    {
                                                        frmVisualizarCertificado.CrearComponentess(certificadoRechazado);
                                                        frmVisualizarCertificado.SeleccionarFormulario();
                                                    }
                                                }
                                                else
                                                {
                                                    mostrarMensaje(Mensaje.errVisualizarCertificado, tipoError);
                                                }
                                            }
                                        }

                                        if (!seleccionLinea)
                                        {
                                            mostrarMensaje(Mensaje.warSeleccionCertificadoVisualizar, tipoAdvertencia);
                                        }
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                    }
                                }
                                #endregion PANE LEVEL 3

                                #region PANE LEVEL 4

                                else if (paneLevel == 4)
                                {
                                    Grid gridRechazadosRec = (Grid)app.Forms.Item("frmMon").Items.Item("grid4").Specific;
                                    DataTable dtRecRec = gridRechazadosRec.DataTable;
                                    bool seleccionLinea = false;

                                    if (!dtRecRec.IsEmpty)
                                    {
                                        for (int i = 0; i < gridRechazadosRec.Rows.Count; i++)
                                        {
                                            if (gridRechazadosRec.Rows.IsSelected(i))
                                            {
                                                seleccionLinea = true;

                                                string serie = dtRecRec.Columns.Item("Serie").Cells.Item(i).
                                                    Value.ToString();
                                                string numDoc = dtRecRec.Columns.Item("Número de Documento").
                                                    Cells.Item(i).Value.ToString();
                                                string tipoCFE = dtRecRec.Columns.Item("Tipo de Documento").
                                                    Cells.Item(i).Value.ToString();

                                                ManteUdoComprobantes udoCerRechazado = new ManteUdoComprobantes();
                                                string certificadoRechazado = udoCerRechazado.obtenerCertificadoRechazado(serie, numDoc, tipoCFE, CFE.ESTipoReceptor.Receptor.ToString());

                                                if (!certificadoRechazado.Equals(""))
                                                {
                                                    if (!frmVisualizarCertificado.FormularioActivo)
                                                    {
                                                        frmVisualizarCertificado.MostarFormulario("frmVisCer", RutasFormularios.FRM_VISUALIZAR_CERTIFICADO);
                                                        frmVisualizarCertificado.CrearComponentess(certificadoRechazado);
                                                        certificadoRechazado = null;
                                                    }
                                                    else
                                                    {
                                                        frmVisualizarCertificado.CrearComponentess(certificadoRechazado);
                                                        frmVisualizarCertificado.SeleccionarFormulario();
                                                    }
                                                }
                                                else
                                                {
                                                    mostrarMensaje(Mensaje.errVisualizarCertificado, tipoError);
                                                }
                                            }
                                        }

                                        if (!seleccionLinea)
                                        {
                                            mostrarMensaje(Mensaje.warSeleccionCertificadoVisualizar, tipoAdvertencia);
                                        }
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                    }
                                }
                                #endregion PANE LEVEL 4
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmMonitor.CerrarFormulario();
                        }
                    }
                }

                #endregion MONITOR DE CERTIFICADOS

                #region MONITOR DE REPORTES

                if (pVal.FormTypeEx.Equals("frmMonRp"))
                {
                    if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Tabs
                            if (pVal.ItemUID.Equals("tab1") || pVal.ItemUID.Equals("tab2") || pVal.ItemUID.Equals("tab3"))
                            {
                                //Selecciona el tab correspondiente
                                frmMonitorReporte.SeleccionarTab(pVal.ItemUID);

                                //Activa o desactiva botones segun sea el caso
                                frmMonitorReporte.EstablecerBotonesActivos(pVal.ItemUID);
                            }
                            //Boton Ok
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmMonitor.CerrarFormulario();
                            }

                            #region Boton mostrar Xml

                            //Boton mostrar Xml
                            else if (pVal.ItemUID.Equals("btnXml"))
                            {
                                Grid grdReportes = (Grid)app.Forms.Item(FormUID).Items.Item("grid3").Specific;

                                if (grdReportes.Rows.SelectedRows.Count == 0)
                                {
                                    if (grdReportes.DataTable.IsEmpty)
                                    {
                                        mostrarMensaje(Mensaje.warNoReportesVisualizar, tipoAdvertencia);
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.warSeleccionReporteVisualizar, tipoAdvertencia);
                                    }
                                }
                                else
                                {
                                    if (grdReportes.DataTable.IsEmpty)
                                    {
                                        mostrarMensaje(Mensaje.warNoReportesVisualizar, tipoAdvertencia);
                                    }
                                    else
                                    {
                                        string numeroEnvio = "", fechaEnvio = "", nombreArchivo = "";
                                        int fila = 0;

                                        while (fila < grdReportes.Rows.Count)
                                        {
                                            if (grdReportes.Rows.IsSelected(fila))
                                            {
                                                numeroEnvio = grdReportes.DataTable.Columns.Item("Número de Envío").Cells.Item(fila).
                                                            Value.ToString();
                                                fechaEnvio = grdReportes.DataTable.Columns.Item("Fecha de Envío").Cells.Item(fila).
                                                            Value.ToString();
                                            }
                                            fila++;
                                        }

                                        nombreArchivo = numeroEnvio + fechaEnvio + ".xml";

                                        if (!frmVisualizarActivo)
                                        {
                                            FTP ftp = new FTP();

                                            if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 4))
                                            {
                                                frmVisualizar = new FrmVisualizar();
                                                frmVisualizarActivo = true;

                                                frmVisualizar.mostrarFormulario();
                                                frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                    nombreArchivo);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errNoDescargaReportes + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                            }
                                        }
                                        else
                                        {
                                            FTP ftp = new FTP();
                                            if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 4))
                                            {
                                                frmVisualizar.seleccionarFormulario(RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                    nombreArchivo);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errNoDescargaReportes + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion Boton mostrar Xml
                        }
                        //Evento Cierre Formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmMonitor.CerrarFormulario();
                        }
                    }
                }

                #endregion MONITOR DE REPORTES

                #region MONITOR CERTIFICADOS CONTINGENCIA

                if (pVal.FormUID.Equals("frmCerCon"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (((Button)app.Forms.Item(FormUID).Items.Item(pVal.ItemUID).Specific).Caption.Equals("Enviar DGI"))
                                {
                                    if (frmMonCerContingencia.EnviarSobreDGI("grdCerCon"))
                                    {
                                        ((Button)app.Forms.Item(FormUID).Items.Item(pVal.ItemUID).Specific).Caption = "OK";
                                    }
                                }
                                else if (((Button)app.Forms.Item(FormUID).Items.Item(pVal.ItemUID).Specific).Caption.Equals("OK"))
                                {
                                    frmMonCerContingencia.CerrarFormulario();
                                }
                            }
                            else if (pVal.ItemUID.Equals("btnCancel"))
                            {
                                frmMonCerContingencia.CerrarFormulario();
                            }
                            else if (pVal.ItemUID.Equals("btnVisXml"))
                            {
                                #region MOSTRAR XML

                                try
                                {
                                    Grid grdSobres = (Grid)app.Forms.Item(FormUID).Items.Item("grdCerCon").Specific;

                                    if (grdSobres.Rows.SelectedRows.Count == 0)
                                    {
                                        if (grdSobres.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            mostrarMensaje(Mensaje.warSeleccionCertificadoVisualizar, tipoAdvertencia);
                                        }
                                    }
                                    else
                                    {
                                        if (grdSobres.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            string nombreArchivo = "";
                                            int fila = 0;

                                            while (fila < grdSobres.Rows.Count)
                                            {
                                                if (grdSobres.Rows.IsSelected(fila))
                                                {
                                                    nombreArchivo = grdSobres.DataTable.Columns.Item("Número de Documento").Cells.Item(fila).Value.ToString();
                                                    nombreArchivo = nombreArchivo + ".xml";

                                                    if (!frmVisualizarActivo)
                                                    {
                                                        FTP ftp = new FTP();

                                                        //Revisar 
                                                        if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 6))
                                                        {
                                                            frmVisualizar = new FrmVisualizar();
                                                            frmVisualizarActivo = true;

                                                            frmVisualizar.mostrarFormulario();
                                                            frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaArchivosVisualizados + nombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            mostrarMensaje(Mensaje.errNoDescargaComprobantes + nombreArchivo
                                                                + Mensaje.errVerificarConexionFTP, tipoError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FTP ftp = new FTP();

                                                        if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 6))
                                                        {
                                                            frmVisualizar.seleccionarFormulario(RutasCarpetas.RutaCarpetaContingenciaComprobantes + nombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            mostrarMensaje(Mensaje.errNoDescargaComprobantes + nombreArchivo
                                                                + Mensaje.errVerificarConexionFTP, tipoError);
                                                        }
                                                    }
                                                }
                                                fila++;
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    mostrarMensaje(Mensaje.errNoDatosCertificados, tipoError);
                                }

                                #endregion MOSTRAR XML
                            }
                        }
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmMonCerContingencia.CerrarFormulario();
                        }
                    }
                }

                #endregion MONITOR CERTIFICADOS CONTINGENCIA

                #region CONFIFGURACION CERTIFICADOS DIGITALES

                if (pVal.FormTypeEx.Equals("frmCerDig"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Eventos item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Evento del boton cancelar
                            if (pVal.ItemUID.Equals("btnCancel"))
                            {
                                frmCerDig.CerrarFormulario();
                            }
                            //Evento del boton Guardar/OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Se obtiene el boton del formulario
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;

                                if (btnOK.Caption.Equals("Guardar"))
                                {
                                    EditText txtRuta = (EditText)app.Forms.Item(FormUID).Items.Item("txtRuta").Specific;
                                    EditText txtClave = (EditText)app.Forms.Item(FormUID).Items.Item("txtClave").Specific;

                                    //Validacion de campos de texto
                                    if (txtClave.Value.Equals("") || txtRuta.Value.Equals(""))
                                    {
                                        app.MessageBox(Mensaje.warIngresaDatosAmbos);
                                    }
                                    else
                                    {
                                        if (ProcConexion.Comp.UserName.Equals(""))
                                        {
                                            mostrarMensaje(Mensaje.errNoUsuarioSistema, tipoError);
                                        }
                                        else
                                        {
                                            ManteUdoCertificadoDigital manteUdo = new ManteUdoCertificadoDigital();

                                            //Envio de datos para almacenar en la base de datos
                                            if (manteUdo.AlmacenarTFECERT(txtRuta.Value.ToString(), txtClave.Value.ToString()))
                                            {
                                                btnOK.Caption = "OK";
                                                mostrarMensaje(Mensaje.sucDatosGuardados, tipoExito);
                                            }
                                            else
                                            {
                                                if (ManteUdoCertificadoDigital.errorCertificado)
                                                {
                                                    ManteUdoCertificadoDigital.errorCertificado = false;
                                                    mostrarMensaje(Mensaje.errCertificadoErroneo, tipoError);
                                                }
                                                else
                                                {
                                                    mostrarMensaje(Mensaje.errNoGuardaDatosCertificado, tipoError);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (btnOK.Caption.Equals("OK"))
                                {
                                    frmCerDig.CerrarFormulario();
                                }
                            }
                            //Carga fileDialog
                            else if (pVal.ItemUID.Equals("btnCarga"))
                            {
                                //Se respalda el directorio actual del proyecto
                                string directorioActual = Environment.CurrentDirectory;

                                AbrirDialogo.DialogoAbrirArchivo abrirRuta = new AbrirDialogo.DialogoAbrirArchivo();
                                //Se crea el filtro para todos los archivos
                                string filtro = Mensaje.filGeneral;
                                //Se abre el dialogo de archivo
                                abrirRuta.AbrirDialogo(filtro);

                                EditText txtRutaCer = (EditText)app.Forms.Item(FormUID).Items.Item("txtRuta").Specific;
                                //Se agrega al campo de texto el path seleccionado en el dialogo de archivo
                                txtRutaCer.Value = AbrirDialogo.DialogoAbrirArchivo.nombreArchivo;

                                //Se reestablece el directorio actual
                                Environment.CurrentDirectory = directorioActual;
                            }
                        }
                        //Evento de cierre de formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmCerDig.CerrarFormulario();
                        }
                    }
                }

                #endregion CONFIGURACION CERTIFICADOS DIGITALES

                #region VISUALIZAR CERTIFICADOS RECHAZADOS

                if (pVal.FormTypeEx.Equals("frmVisCer"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmVisualizarCertificado.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmVisualizarCertificado.CerrarFormulario();
                        }
                    }
                }

                #endregion VISUALIZAR CERTIFICADOS RECHAZADOS

                #region MONITOR ANULADOS DGI

                if (pVal.FormTypeEx.Equals("frmMonAnu"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento obtener focus
                        if (pVal.EventType == BoEventTypes.et_GOT_FOCUS)
                        {
                            if (pVal.ItemUID.Equals("grdAnu"))
                            {
                                frmMonitorAnulado.CambiarEstadoBotonOK(false);
                            }
                        }
                        //Evento item presionado
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (frmMonitorAnulado.EstadoBotonOK)
                                {
                                    frmMonitorAnulado.CerrarFormulario();
                                }
                                else
                                {
                                    frmMonitorAnulado.Actualizar();
                                    frmMonitorAnulado.CambiarEstadoBotonOK();
                                }
                            }
                            #region BOTON MOSTRAR XML
                            //Boton mostrar Xml
                            else if (pVal.ItemUID.Equals("btnXml"))
                            {
                                Grid grdAnulados = (Grid)app.Forms.Item(FormUID).Items.Item("grdAnu").Specific;

                                if (grdAnulados.Rows.SelectedRows.Count == 0)
                                {
                                    if (grdAnulados.DataTable.IsEmpty)
                                    {
                                        mostrarMensaje(Mensaje.warNoReportesVisualizar, tipoAdvertencia);
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.warSeleccionReporteVisualizar, tipoAdvertencia);
                                    }
                                }
                                else
                                {
                                    if (grdAnulados.DataTable.IsEmpty)
                                    {
                                        mostrarMensaje(Mensaje.warNoReportesVisualizar, tipoAdvertencia);
                                    }
                                    else
                                    {
                                        string nombreArchivo = "", serie = "", numComp = "", tipoCFE = "";
                                        int fila = 0;

                                        while (fila < grdAnulados.Rows.Count)
                                        {
                                            if (grdAnulados.Rows.IsSelected(fila))
                                            {
                                                //Se debe definir parametros de busquedaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                                                serie = grdAnulados.DataTable.Columns.Item("Serie Comprobante").Cells.Item(fila).
                                                            Value.ToString();

                                                numComp = grdAnulados.DataTable.Columns.Item("Número Comprobante").
                                                    Cells.Item(fila).Value.ToString();

                                                tipoCFE = grdAnulados.DataTable.Columns.Item("Tipo CFE").
                                                    Cells.Item(fila).Value.ToString();
                                            }
                                            fila++;
                                        }

                                        nombreArchivo = tipoCFE + serie + numComp + ".xml";

                                        if (!frmVisualizarActivo)
                                        {
                                            FTP ftp = new FTP();

                                            if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 8))
                                            {
                                                frmVisualizar = new FrmVisualizar();
                                                frmVisualizarActivo = true;

                                                frmVisualizar.mostrarFormulario();
                                                frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                    nombreArchivo);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errNoDescargaReportes + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                            }
                                        }
                                        else
                                        {
                                            FTP ftp = new FTP();
                                            if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 1))
                                            {
                                                frmVisualizar.seleccionarFormulario(RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                    nombreArchivo);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errNoDescargaReportes + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion BOTON MOSTRAR XML
                        }
                        //Evento Cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmMonitorAnulado.CerrarFormulario();
                        }
                    }
                }

                #endregion MONITOR ANULADOS DGI

                #region MONITOR SOBRES

                if (pVal.FormTypeEx.Equals("frmMonSob"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmMonitorSobre.CerrarFormulario();
                            }

                            #region BOTON MOSTRAR XML

                            //Boton mostrar Xml
                            else if (pVal.ItemUID.Equals("btnXml"))
                            {
                                try
                                {
                                    Grid grdSobres = (Grid)app.Forms.Item(FormUID).Items.Item("grdSob").Specific;

                                    if (grdSobres.Rows.SelectedRows.Count == 0)
                                    {
                                        if (grdSobres.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoSobresVisualizar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            mostrarMensaje(Mensaje.warSeleccionSobreVisualizar, tipoAdvertencia);
                                        }
                                    }
                                    else
                                    {
                                        if (grdSobres.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoSobresVisualizar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            string nombreArchivo = "";
                                            int fila = 0;

                                            while (fila < grdSobres.Rows.Count)
                                            {
                                                if (grdSobres.Rows.IsSelected(fila))
                                                {
                                                    nombreArchivo = grdSobres.DataTable.Columns.Item("Nombre Archivo").Cells.Item(fila).Value.ToString();

                                                    if (!frmVisualizarActivo)
                                                    {
                                                        FTP ftp = new FTP();

                                                        if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 0))
                                                        {
                                                            frmVisualizar = new FrmVisualizar();
                                                            frmVisualizarActivo = true;

                                                            frmVisualizar.mostrarFormulario();
                                                            frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                                nombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            mostrarMensaje(Mensaje.errNoDescargaSobres + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FTP ftp = new FTP();
                                                        if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaArchivosVisualizados, 0))
                                                        {
                                                            frmVisualizar.seleccionarFormulario(RutasCarpetas.RutaCarpetaArchivosVisualizados +
                                                                nombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            mostrarMensaje(Mensaje.errNoDescargaSobres + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                                        }
                                                    }
                                                }
                                                fila++;
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    mostrarMensaje(Mensaje.errNoDatosSobres, tipoError);
                                }
                            }
                            #endregion BOTON MOSTRAR XML
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmMonitorSobre.CerrarFormulario();
                        }
                    }
                }

                #endregion MONITOR SOBRES

                #region CERTIFICADOS RECIBIDOS

                if (pVal.FormTypeEx.Equals("frmCerRec"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmCertificadoRecibido.CerrarFormulario();
                            }

                            #region BOTON MOSTRAR DETALLES

                            else if (pVal.ItemUID.Equals("btnShow"))
                            {
                                Grid grdSobresRecibidos = (Grid)app.Forms.Item(FormUID).Items.Item("grdSobRec").Specific;

                                if (grdSobresRecibidos.Rows.SelectedRows.Count == 0)
                                {
                                    if (grdSobresRecibidos.DataTable.IsEmpty)
                                    {
                                        mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                    }
                                }
                                else
                                {
                                    if (grdSobresRecibidos.DataTable.IsEmpty)
                                    {
                                        mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                    }
                                    else
                                    {
                                        int fila = 0;
                                        int filaSeleccionada = 0;

                                        while (fila < grdSobresRecibidos.Rows.Count)
                                        {
                                            if (grdSobresRecibidos.Rows.IsSelected(fila))
                                            {
                                                filaSeleccionada = fila;
                                            }

                                            fila++;
                                        }

                                        string nroSap = grdSobresRecibidos.DataTable.Columns.Item("NroSAP").Cells.Item(filaSeleccionada).Value.ToString();
                                        int intNroSap = int.Parse(nroSap);
                                        frmCertificadoRecibidoDet = new FrmCertificadoRecibidoDet(intNroSap);//filaSeleccionada +1
                                        frmCertificadoRecibidoDet.MostarFormulario("frmSRDet", RutasFormularios.FRM_CERTIFICADO_RECIBIDO_DETALLE);
                                    }
                                }
                            }

                            #endregion BOTON MOSTRAR DETALLES

                            #region BOTON MOSTRAR XML

                            //Boton mostrar Xml
                            else if (pVal.ItemUID.Equals("btnXml"))
                            {
                                try
                                {
                                    Grid grdSobresRecibidos = (Grid)app.Forms.Item(FormUID).Items.Item("grdSobRec").Specific;

                                    if (grdSobresRecibidos.Rows.SelectedRows.Count == 0)
                                    {
                                        if (grdSobresRecibidos.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                    }
                                    else
                                    {
                                        if (grdSobresRecibidos.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            string nombreArchivo = "";
                                            int fila = 0;

                                            while (fila < grdSobresRecibidos.Rows.Count)
                                            {
                                                if (grdSobresRecibidos.Rows.IsSelected(fila))
                                                {
                                                    nombreArchivo = grdSobresRecibidos.DataTable.Columns.Item("Nombre Sobre").Cells.Item(fila).Value.ToString();
                                                    //grdSobresRecibidos.DataTable.Columns.Item("Tipo CFE").Cells.Item(fila).Value.ToString() + grdSobresRecibidos.DataTable.Columns.Item("Serie Comprobante").Cells.Item(fila).Value.ToString() + grdSobresRecibidos.DataTable.Columns.Item("Número Comprobante").Cells.Item(fila).Value.ToString();
                                                    if (!frmVisualizarActivo)
                                                    {
                                                        FTP ftp = new FTP();

                                                        if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaBandejaEntrada, 3))
                                                        {
                                                            frmVisualizar = new FrmVisualizar();
                                                            frmVisualizarActivo = true;

                                                            frmVisualizar.mostrarFormulario();
                                                            frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaBandejaEntrada +
                                                                nombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            if (AbrirArchivo(RutasCarpetas.RutaCarpetaBandejaEntrada +
                                                                    nombreArchivo))
                                                            {
                                                                frmVisualizar = new FrmVisualizar();
                                                                frmVisualizarActivo = true;

                                                                frmVisualizar.mostrarFormulario();
                                                                frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaBandejaEntrada +
                                                                    nombreArchivo);
                                                            }
                                                            else
                                                            {
                                                                mostrarMensaje(Mensaje.errNoDescargaComprobantes + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FTP ftp = new FTP();
                                                        if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaBandejaEntrada, 3))
                                                        {
                                                            frmVisualizar.seleccionarFormulario(RutasCarpetas.RutaCarpetaBandejaEntrada +
                                                                nombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            if (AbrirArchivo(RutasCarpetas.RutaCarpetaBandejaEntrada +
                                                                    nombreArchivo))
                                                            {
                                                                frmVisualizar = new FrmVisualizar();
                                                                frmVisualizarActivo = true;

                                                                frmVisualizar.mostrarFormulario();
                                                                frmVisualizar.obtenerFormulario(FormUID, RutasCarpetas.RutaCarpetaBandejaEntrada +
                                                                    nombreArchivo);
                                                            }
                                                            else
                                                            {
                                                                mostrarMensaje(Mensaje.errNoDescargaComprobantes + nombreArchivo + Mensaje.errVerificarConexionFTP, tipoError);
                                                            }
                                                        }
                                                    }
                                                }
                                                fila++;
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    mostrarMensaje(Mensaje.errNoDatosComprobante, tipoError);
                                }
                            }
                            #endregion BOTON MOSTRAR XML

                            #region BOTON APROBACION

                            else if (pVal.ItemUID.Equals("btnApr"))
                            {
                                try
                                {
                                    Grid grdSobresRecibidos = (Grid)app.Forms.Item(FormUID).Items.Item("grdSobRec").Specific;

                                    if (grdSobresRecibidos.Rows.SelectedRows.Count == 0)
                                    {
                                        if (grdSobresRecibidos.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                    }
                                    else
                                    {
                                        if (grdSobresRecibidos.DataTable.IsEmpty)
                                        {
                                            mostrarMensaje(Mensaje.warNoCertificadoMostrar, tipoAdvertencia);
                                        }
                                        else
                                        {
                                            int fila = 0;

                                            while (fila < grdSobresRecibidos.Rows.Count)
                                            {
                                                if (grdSobresRecibidos.Rows.IsSelected(fila))
                                                {
                                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                                                    frmAprobacion.Tipo = grdSobresRecibidos.DataTable.Columns.Item("Tipo CFE").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.Serie = grdSobresRecibidos.DataTable.Columns.Item("Serie Comprobante").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.Numero = grdSobresRecibidos.DataTable.Columns.Item("Número Comprobante").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.IdConsecutivo = grdSobresRecibidos.DataTable.Columns.Item("IdReceptor").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.DocEntry = grdSobresRecibidos.DataTable.Columns.Item("NroSAP").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.Estado = grdSobresRecibidos.DataTable.Columns.Item("Estado").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.NumIniCAE = grdSobresRecibidos.DataTable.Columns.Item("NroCAE Desde").Cells.Item(fila).Value.ToString();
                                                    frmAprobacion.NumFinCAE = grdSobresRecibidos.DataTable.Columns.Item("NroCAE Hasta").Cells.Item(fila).Value.ToString();
                                                    //frmAprobacion.FechaCAE = grdSobresRecibidos.DataTable.Columns.Item("Fecha Vencimiento").Cells.Item(fila).Value.ToString();
                                                    //frmAprobacion.FechaFirma = grdSobresRecibidos.DataTable.Columns.Item("Fecha Emisión").Cells.Item(fila).Value.ToString();

                                                    //ManteUdoEstadoSobreRecibido manteSobreRecibido = new ManteUdoEstadoSobreRecibido();
                                                    //ArrayList sobreRecibido = manteSobreRecibido.ConsultarCFEProcesado(ProcConexion.Comp, frmAprobacion.DocEntry);

                                                    //if (sobreRecibido == null || sobreRecibido.Count == 0)
                                                    //{
                                                    if (frmAprobacion.Estado.Equals("Pendiente"))
                                                    {
                                                        //XmlDocument contenidoXml = ObtenerContenidoXml(frmAprobacion.Tipo, frmAprobacion.Serie, frmAprobacion.Numero);

                                                        //if (contenidoXml != null)
                                                        //{
                                                        //    ValidarCFE validarCFE = new ValidarCFE();
                                                        //    validarCFE.ValidarComprobanteFiscal(contenidoXml, Convert.ToInt16(frmAprobacion.Tipo), Convert.ToInt16(frmAprobacion.Numero), frmAprobacion.Serie, Convert.ToInt16(frmAprobacion.NumIniCAE), Convert.ToInt16(frmAprobacion.NumFinCAE), frmAprobacion.FechaFirma, frmAprobacion.FechaCAE, Convert.ToInt16(frmAprobacion.DocEntry));
                                                        //}
                                                        //else
                                                        //{
                                                        //    mostrarMensaje(Mensaje.errVerificarConexionFTP, tipoError);
                                                        //}
                                                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.AppStarting;
                                                        frmAprobacion.MostarFormulario("frmACR", RutasFormularios.FRM_APROBACION_RECIBIDO);
                                                    }
                                                    else
                                                    {
                                                        mostrarMensaje("No se puede cambiar estado. El sobre fue: " + frmAprobacion.Estado, tipoError);
                                                    }
                                                    //}                                                    
                                                }
                                                fila++;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error: " + ex.ToString());
                                    //mostrarMensaje(Mensaje.errNoDatosComprobante, tipoError);
                                }
                            }

                            #endregion BOTON APROBACION

                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmCertificadoRecibido.CerrarFormulario();
                        }
                    }
                }

                #endregion CERTIFICADOS RECIBIDOS

                #region DETALLE DE CERTIFICADOS RECIBIDOS

                if (pVal.FormTypeEx.Equals("frmSRDet"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmCertificadoRecibidoDet.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmCertificadoRecibidoDet.CerrarFormulario();
                        }
                    }
                }

                #endregion DETALLE DE CERTIFICADOS RECIBIDOS

                #region APROBACION CERTIFICADO RECIBIDO

                if (pVal.FormTypeEx.Equals("frmACR"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Ok
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                mostrarMensaje("Por favor espere mientras se envia el ACK de respuesta.", tipoMensajes.advertencia);

                                if (frmAprobacion.ActualizarAprobacion())
                                {
                                    frmAprobacion.CerrarFormulario();
                                    mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                    frmCertificadoRecibido.CargarGrid();
                                }
                                else
                                {
                                    mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                }
                            }
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmAprobacion.CerrarFormulario();
                            }
                            else if (pVal.ItemUID.Equals("btnAgr"))
                            {
                                frmMotivoRechazo.Tipo = FrmMotivoRechazo.ETipo.almancenar;
                                frmMotivoRechazo.IdConsecutivo = frmAprobacion.DocEntry;
                                frmMotivoRechazo.MostarFormulario("frmMotRech", RutasFormularios.FRM_MOTIVO_RECHAZO);
                            }
                            else if (pVal.ItemUID.Equals("btnEdi"))
                            {
                                //Valida que se haya seleccionado una fila
                                if (!frmAprobacion.ObtenerFilaSeleccionada().Equals("0"))
                                {
                                    //Valida que el motivo seleccionado no sea del sistema
                                    if (!frmAprobacion.ObtenerTipoMotivoSistema())
                                    {
                                        frmMotivoRechazo.Tipo = FrmMotivoRechazo.ETipo.editar;
                                        frmMotivoRechazo.IdConsecutivo = frmAprobacion.DocEntry;
                                        frmMotivoRechazo.IdMotivo = frmAprobacion.ObtenerFilaSeleccionada();
                                        frmMotivoRechazo.MostarFormulario("frmMotRech", RutasFormularios.FRM_MOTIVO_RECHAZO);
                                        frmMotivoRechazo.Consultar(frmMotivoRechazo.IdMotivo);
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.errNoMotivosSistea, tipoError);
                                    }
                                }
                                else
                                {
                                    mostrarMensaje(Mensaje.errNoMotivosMostrar, tipoError);
                                }
                            }
                            else if (pVal.ItemUID.Equals("btnElim"))
                            {
                                //Valida que se haya seleccionado una fila
                                if (!frmAprobacion.ObtenerFilaSeleccionada().Equals("0"))
                                {
                                    //Valida que el motivo seleccionado no sea del sistema
                                    if (!frmAprobacion.ObtenerTipoMotivoSistema())
                                    {
                                        //Preguntar al usuario si desea eliminar el motivo seleccionado
                                        int respuesta = app.MessageBox(Mensaje.preConfirmaEliminarMotivoRechazo, 1, "Si", "No");

                                        //Si la respuesta es afirmativa...
                                        if (respuesta == 1)
                                        {
                                            if (frmMotivoRechazo.Eliminar(frmAprobacion.ObtenerFilaSeleccionada()))
                                            {
                                                mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                                frmAprobacion.CargarGrid();
                                                frmAprobacion.AjustarOpciones();
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.errNoMotivosSistea, tipoError);
                                    }
                                }
                                else
                                {
                                    mostrarMensaje(Mensaje.errNoMotivosMostrar, tipoError);
                                }
                            }
                        }
                        else if (pVal.EventType == BoEventTypes.et_CLICK)
                        {
                            if (pVal.ItemUID.Equals("rbAprob"))
                            {
                                frmAprobacion.MostrarRechazo(false);
                            }
                            else if (pVal.ItemUID.Equals("rbRech"))
                            {
                                frmAprobacion.MostrarRechazo(true);
                            }
                        }

                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmAprobacion.CerrarFormulario();
                        }
                    }
                }

                #endregion APROBACION CERTIFICADO RECIBIDO

                #region MOTIVOS DE RECHAZO CERTIFICADO RECIBIDO

                if (pVal.FormTypeEx.Equals("frmMotRech"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Seleccion del combo
                        if (pVal.EventType == BoEventTypes.et_COMBO_SELECT)
                        {
                            frmMotivoRechazo.SeleccionMotivo();
                            frmMotivoRechazo.EstablecerEstadoBotonOK(false);
                        }
                        //Focus
                        else if (pVal.EventType == BoEventTypes.et_GOT_FOCUS && pVal.ItemUID.Equals("txtDetalle"))
                        {
                            frmMotivoRechazo.EstablecerEstadoBotonOK(false);
                        }
                        //Item pressed
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                //Boton en OK
                                if (frmMotivoRechazo.EstadoBotonOK)
                                {
                                    frmMotivoRechazo.CerrarFormulario();
                                    frmAprobacion.CargarGrid();
                                }
                                //Boton en Actualizar/Almacenar
                                else
                                {
                                    if (frmMotivoRechazo.ValidarCampos())
                                    {
                                        if (frmMotivoRechazo.Tipo == FrmMotivoRechazo.ETipo.almancenar)
                                        {
                                            if (frmMotivoRechazo.Almacenar())
                                            {
                                                mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                                frmMotivoRechazo.LimpiarCampos();
                                                frmMotivoRechazo.EstablecerEstadoBotonOK();
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                            }
                                        }
                                        else if (frmMotivoRechazo.Tipo == FrmMotivoRechazo.ETipo.editar)
                                        {
                                            if (frmMotivoRechazo.Actualizar())
                                            {
                                                mostrarMensaje(Mensaje.sucOperacionExitosa, tipoExito);
                                                frmMotivoRechazo.EstablecerEstadoBotonOK();
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errOperacionFallida, tipoError);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mostrarMensaje(Mensaje.errValIncompleto, tipoError);
                                    }
                                }
                            }
                            if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmMotivoRechazo.CerrarFormulario();
                                frmAprobacion.CargarGrid();
                            }
                        }
                        //form unload
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmMotivoRechazo.CerrarFormulario();
                        }
                    }
                }

                #endregion MOTIVOS DE RECHAZO CERTIFICADO RECIBIDO

                #region VISUALIZAR FACTURAS SOBRE

                if (pVal.FormTypeEx.Equals("frmVisSob"))
                {
                    if (pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmVisualizarSobreFactura.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmVisualizarSobreFactura.CerrarFormulario();
                        }
                    }
                }

                #endregion VISUALIZAR FACTURAS SOBRE

                #region CONTINGENCIA

                if (pVal.FormUID.Equals("frmEstCont"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento item presionado
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Check Box
                            if (pVal.ItemUID.Equals("cbCont"))
                            {
                                frmEstadoContingencia.CambiarEstadoBotonOK(false);
                            }
                            //Boton OK
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (frmEstadoContingencia.EstadoBotonOK)
                                {
                                    frmEstadoContingencia.CerrarFormulario();
                                }
                                else
                                {
                                    if (frmEstadoContingencia.Almacenar())
                                    {
                                        frmEstadoContingencia.CambiarEstadoBotonOK();
                                    }
                                }
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmEstadoContingencia.CerrarFormulario();
                            }
                        }
                        //Evento cierre formulario
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmEstadoContingencia.CerrarFormulario();
                        }
                    }
                }

                #endregion CONTINGENCIA

                #region CONFIGURACION CORREO ELECTRONICO

                if (pVal.FormUID.Equals("frmECA"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento de cierre de formulario
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmEnvioCorreoElectronico.CerrarFormulario();
                        }
                        //Evento item presionado
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;

                                if (btnOK.Caption.Equals("Actualizar"))
                                {
                                    if (verificarCampos(FormUID))
                                    {
                                        //Se crean referencia a los objetos de la interfaz
                                        OptionBtn oBOutlook = (OptionBtn)app.Forms.Item(FormUID).Items.Item("rbOutlook").Specific;
                                        EditText txtClave = (EditText)app.Forms.Item(FormUID).Items.Item("txtClave").Specific;
                                        EditText txtCorreo = (EditText)app.Forms.Item(FormUID).Items.Item("txtCorreo").Specific;

                                        //Se crean instancias de clases
                                        ManteUdoEnvioCorreoElectronico mantenimiento = new ManteUdoEnvioCorreoElectronico();
                                        Correo correo = new Correo();

                                        if (oBOutlook.Selected)
                                        {
                                            //Se asignan valores al objeto
                                            correo.Clave = "Outlook";
                                            correo.Cuenta = "Outlook";
                                            correo.Opcion = "1";
                                        }
                                        else
                                        {
                                            //Se asignan valores al objeto
                                            correo.Clave = txtClave.Value.ToString();
                                            correo.Cuenta = txtCorreo.Value.ToString();
                                            correo.Opcion = "0";
                                        }

                                        if (mantenimiento.datosCorreo(correo))
                                        {
                                            if (ManteUdoEnvioCorreoElectronico.metodoMantenimiento)
                                            {
                                                mostrarMensaje(Mensaje.sucDatosActualizados, tipoExito);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.sucDatosInsertados, tipoExito);
                                            }
                                            btnOK.Caption = "OK";
                                        }
                                        else
                                        {
                                            if (ManteUdoEnvioCorreoElectronico.metodoMantenimiento)
                                            {
                                                mostrarMensaje(Mensaje.errFalloActualizaDatos, tipoError);
                                            }
                                            else
                                            {
                                                mostrarMensaje(Mensaje.errFalloIngresaDatos, tipoError);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        app.StatusBar.SetSystemMessage(errorVerifica, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                    }
                                }
                                //Se cierra el formulario
                                else if (btnOK.Caption.Equals("OK"))
                                {
                                    frmEnvioCorreoElectronico.CerrarFormulario();
                                }
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCancel"))
                            {
                                frmEnvioCorreoElectronico.CerrarFormulario();
                            }
                            //Radiobutton Outlook
                            else if (pVal.ItemUID.Equals("rbOutlook"))
                            {
                                EditText txtClave = (EditText)app.Forms.Item(FormUID).Items.Item("txtClave").Specific;
                                EditText txtCorreo = (EditText)app.Forms.Item(FormUID).Items.Item("txtCorreo").Specific;

                                txtCorreo.Active = false;
                                txtCorreo.Item.Enabled = false;
                                txtCorreo.Active = true;
                                txtClave.Active = false;
                                txtClave.Item.Enabled = false;
                            }
                            //Radiobutton Gmail
                            else if (pVal.ItemUID.Equals("rbGmail"))
                            {
                                EditText txtClave = (EditText)app.Forms.Item(FormUID).Items.Item("txtClave").Specific;
                                EditText txtCorreo = (EditText)app.Forms.Item(FormUID).Items.Item("txtCorreo").Specific;

                                txtClave.Active = true;
                                txtCorreo.Item.Enabled = true;
                                txtClave.Active = false;
                                txtClave.Item.Enabled = true;
                            }
                        }
                    }
                }

                #endregion CONFIGURACION CORREO ELECTRONICO

                #region LOGO

                if (pVal.FormUID.Equals("frmLogo"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento de cierre de formualario
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmLogo.CerrarFormulario();
                        }
                        //Evento item presionado
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Evento del boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;
                                //Se guardo, se cierra el formulario
                                if (btnOK.Caption.Equals("OK"))
                                {
                                    frmLogo.CerrarFormulario();
                                }
                                //Se envia a guardar los datos
                                else if (btnOK.Caption.Equals("Guardar"))
                                {
                                    EditText txtRutalogo = (EditText)app.Forms.Item(FormUID).Items.Item("txtRuta").Specific;

                                    EditText txtTimeImp = (EditText)app.Forms.Item(FormUID).Items.Item("txtTimeImp").Specific;

                                    //Valida que se haya selecionado un archivo
                                    if (txtRutalogo.Value.Equals(""))
                                    {
                                        mostrarMensaje(Mensaje.errSeleccionLogo, tipoError);
                                    }
                                    else
                                    {
                                        ManteUdoLogo mante = new ManteUdoLogo();
                                        //Llamada para almacenar la ruta del logo y el correspondiente usuario
                                        if (mante.AlmacenarLOGO(txtRutalogo.Value.ToString(), Convert.ToInt32(txtTimeImp.Value.ToString())))
                                        {
                                            btnOK.Caption = "OK";
                                        }
                                        else
                                        {
                                            mostrarMensaje(Mensaje.errFalloGuardaRutaLogo, tipoError);
                                        }
                                    }
                                }
                            }
                            //Boton ...
                            else if (pVal.ItemUID.Equals("btnArc"))
                            {
                                //Se obtiene el directorio actual para el proyecto
                                string directorioActual = Environment.CurrentDirectory;

                                //Filtro para el dialogo de archivo
                                string filtro = Mensaje.filImagenes;

                                //Llamada al metodo para abrir el dialogo
                                DialogoAbrirArchivo dialogo = new DialogoAbrirArchivo();
                                dialogo.AbrirDialogo(filtro);

                                //Se obtiene el campo de texto de la UI
                                EditText rutaArchivo = (EditText)app.Forms.ActiveForm.Items.Item("txtRuta").Specific;

                                //Se asigna la ruta al campo de texto
                                rutaArchivo.Value = DialogoAbrirArchivo.nombreArchivo;

                                //Se ajusta al directotio para que mantega el path original
                                Environment.CurrentDirectory = directorioActual;

                                //Si se agrega un path nuevo se habilita la opcion de guardarlo
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;
                                btnOK.Caption = "Guardar";
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmLogo.CerrarFormulario();
                            }
                        }
                    }
                }

                #endregion LOGO

                #region VISUALIZAR ARCHIVOS XML

                if (pVal.FormUID.Equals("frmVisualizar"))
                {
                    if (!pVal.BeforeAction)
                    {
                        //Evento de cierre de formulario
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            if (frmVisualizarActivo)
                            {
                                frmVisualizarActivo = false;
                                frmVisualizar.cerrarFormulario(FormUID);
                            }
                        }
                    }
                }

                #endregion VISUALIZAR ARCHIVOS XML

                #region AUTORIZACION DOCUMENTOS NO ELECTRONICOS

                if (pVal.FormUID.Equals("frmAutoNoElec"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton OK
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (frmAutoDocNoElectronico.BotonOK)
                                {
                                    frmAutoDocNoElectronico.CerrarFormulario();
                                }
                                else
                                {
                                    frmAutoDocNoElectronico.Actualizar();
                                    frmAutoDocNoElectronico.EstadoBotonOK(true);
                                }
                            }
                            //Boton cancelar
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmAutoDocNoElectronico.CerrarFormulario();
                            }
                            //Grid
                            else if (pVal.ItemUID.Equals("grdUsr") && pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                            {
                                frmAutoDocNoElectronico.EstadoBotonOK(false);
                            }
                        }
                    }
                }

                #endregion AUTORIZACION DOCUMENTOS NO ELECTRONICOS

                #region SOCIOS NEGOCIOS

                if (!pVal.BeforeAction)
                {
                    if (pVal.FormTypeEx.Equals("134"))
                    {
                        if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                        {
                            Form formularioActivo = app.Forms.Item(pVal.FormUID);
                            frmSociosNegocio.AgregarCheckBoxClienteContado(formularioActivo);
                            frmSociosNegocio.AgregarTabAdenda(formularioActivo);
                        }
                    }
                }

                //Al dar clic en tab de adenda
                if (pVal.FormTypeEx.Equals("134"))
                {
                    if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                    {
                        if (pVal.ItemUID.Equals("tabAdn"))
                        {
                            if (pVal.BeforeAction)
                            {
                                Form formularioActivo = app.Forms.Item(pVal.FormUID);
                                frmSociosNegocio.SeleccionarTabAdenda(formularioActivo);
                                frmSociosNegocio.CargarAdenda(formularioActivo);
                            }
                        }

                        if (pVal.ItemUID.Equals("1"))
                        {
                            if (pVal.BeforeAction)
                            {
                                Form formularioActivo = app.Forms.Item(pVal.FormUID);
                                frmSociosNegocio.AlmacenarAdenda(formularioActivo);
                            }
                        }
                    }
                }

                #endregion USUARIOS

                #region USUARIOS

                if (!pVal.BeforeAction)
                {
                    if (pVal.FormTypeEx.Equals("20700"))
                    {
                        if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                        {
                            Form formularioActivo = app.Forms.Item(pVal.FormUID);
                            frmUsuarios.AgregarCheckBoxClienteContado(formularioActivo);
                        }



                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {


                            if (pVal.ItemUID.Equals("1"))
                            {

                                //if (pVal.BeforeAction)
                                //{

                                ManteUdoEstadoContingencia manteEstadoContingencia = new ManteUdoEstadoContingencia();
                                manteEstadoContingencia.ActualizarEstadoContingencia();
                                //}
                            }

                        }
                    }
                }



                #endregion USUARIOS

                #region INDICADORES IMPUESTOS

                if (pVal.BeforeAction)
                {
                    if (pVal.FormUID.Equals("frmImpDgi"))
                    {
                        //Evento cierre formulario
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmImpuestosdgiB1.CerrarFormulario();
                        }
                        //Evento del item presionado
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            //Boton Cancelar
                            if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmImpuestosdgiB1.CerrarFormulario();
                            }
                            //Boton OK
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (((Button)app.Forms.Item("frmImpDgi").Items.Item(pVal.ItemUID).Specific).Caption.Equals("Actualizar"))
                                {
                                    frmImpuestosdgiB1.ActualizarDatosGrid();
                                    ((Button)app.Forms.Item("frmImpDgi").Items.Item(pVal.ItemUID).Specific).Caption = "OK";
                                }
                                else if (((Button)app.Forms.Item("frmImpDgi").Items.Item(pVal.ItemUID).Specific).Caption.Equals("OK"))
                                {
                                    frmImpuestosdgiB1.CerrarFormulario();
                                }
                            }
                        }
                    }
                }

                #endregion INDICADORES IMPUESTOS

                #region ARTICULOS

                if (pVal.FormTypeEx.Equals("150"))
                {
                    if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                    {
                        if (!pVal.BeforeAction)
                        {
                            frmArticulos.CrearComponentes(pVal.FormUID, pVal.FormTypeEx);
                        }
                    }
                }

                #endregion ARTICULOS

                #region FORMA PAGO

                if (pVal.FormUID.Equals("frmFormaPago"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED))
                        {
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption.Equals("Almacenar"))
                                {
                                    if (((ComboBox)app.Forms.Item(pVal.FormUID).Items.Item("cbxFpg").Specific).Selected == null)
                                    {
                                        mostrarMensaje("Debe seleccionar una forma de pago", tipoMensajes.error);
                                    }
                                    else
                                    {
                                        ManteUdoFormaPago manteUdoFormaPago = new ManteUdoFormaPago();
                                        string docEntry = manteUdoFormaPago.ObtenerDocEntryFormaPago(true);

                                        if (docEntry.Equals(""))
                                        {
                                            if (manteUdoFormaPago.Almacenar(((ComboBox)app.Forms.Item(pVal.FormUID).Items.Item("cbxFpg").Specific).Selected.Value + ""))
                                            {
                                                ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "OK";
                                                mostrarMensaje("Forma de pago almacenada con éxito.", tipoMensajes.exito);
                                            }
                                            else
                                            {
                                                mostrarMensaje("Error al almacenar forma de pago.", tipoMensajes.error);
                                            }
                                        }
                                        else
                                        {
                                            if (manteUdoFormaPago.Actualizar(((ComboBox)app.Forms.Item(pVal.FormUID).Items.Item("cbxFpg").Specific).Selected.Value + "", docEntry))
                                            {
                                                ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "OK";
                                                mostrarMensaje("Forma de pago actualizada con éxito.", tipoMensajes.exito);
                                            }
                                            else
                                            {
                                                mostrarMensaje("Error al actualizar forma de pago.", tipoMensajes.error);
                                            }
                                        }

                                        frmFormaPago.GuardarCI();
                                    }
                                }
                                else
                                {
                                    frmFormaPago.CerrarFormulario();
                                }

                            }
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmFormaPago.CerrarFormulario();
                            }
                        }
                        else if (pVal.EventType.Equals(BoEventTypes.et_FORM_UNLOAD))
                        {
                            frmFormaPago.CerrarFormulario();
                        }
                        else if (pVal.EventType.Equals(BoEventTypes.et_COMBO_SELECT))
                        {
                            ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "Almacenar";
                        }
                    }
                }

                #endregion FORMA PAGO

                #region ADOBE

                if (pVal.FormUID.Equals("frmAdoRea"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmAdobe.CerrarFormulario();
                        }
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmAdobe.CerrarFormulario();
                            }
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;
                                //Se guardo, se cierra el formulario
                                if (btnOK.Caption.Equals("OK"))
                                {
                                    frmAdobe.CerrarFormulario();
                                }
                                //Se envia a guardar los datos
                                else if (btnOK.Caption.Equals("Guardar"))
                                {
                                    EditText txtRutaAdobe = (EditText)app.Forms.Item(FormUID).Items.Item("txtRuta").Specific;

                                    //Valida que se haya selecionado un archivo
                                    if (txtRutaAdobe.Value.Equals(""))
                                    {
                                        mostrarMensaje(Mensaje.errSeleccionAdobe, tipoError);
                                    }
                                    else
                                    {
                                        ManteUdoAdobe mante = new ManteUdoAdobe();
                                        //Llamada para almacenar la ruta del logo y el correspondiente usuario
                                        if (mante.AlmacenarAdobeReader(txtRutaAdobe.Value.ToString()))
                                        {
                                            btnOK.Caption = "OK";
                                        }
                                        else
                                        {
                                            mostrarMensaje(Mensaje.errFalloGuardaRutaAdobe, tipoError);
                                        }
                                    }
                                }
                            }
                            else if (pVal.ItemUID.Equals("btnCarga"))
                            {
                                //Se obtiene el directorio actual para el proyecto
                                string directorioActual = Environment.CurrentDirectory;

                                //Llamada al metodo para abrir el dialogo
                                DialogoAbrirArchivo dialogo = new DialogoAbrirArchivo();
                                dialogo.AbrirDialogo(Mensaje.filEjecutable);

                                //Se obtiene el campo de texto de la UI
                                EditText rutaArchivo = (EditText)app.Forms.ActiveForm.Items.Item("txtRuta").Specific;

                                //Se asigna la ruta al campo de texto
                                rutaArchivo.Value = DialogoAbrirArchivo.nombreArchivo;

                                //Se ajusta al directotio para que mantega el path original
                                Environment.CurrentDirectory = directorioActual;

                                //Si se agrega un path nuevo se habilita la opcion de guardarlo
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;
                                btnOK.Caption = "Guardar";
                            }
                        }
                    }
                }

                #endregion ADOBE

                #region CONFIGURACION ALERTAS FIN CAES

                if (pVal.FormUID.Equals("frmConCae"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmConfFinCae.CerrarFormulario();
                        }
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmConfFinCae.CerrarFormulario();
                            }
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                Button btnOK = (Button)app.Forms.Item(FormUID).Items.Item("btnOK").Specific;
                                //Se guardo, se cierra el formulario
                                if (btnOK.Caption.Equals("OK"))
                                {
                                    frmConfFinCae.CerrarFormulario();
                                }
                                //Se envia a guardar los datos
                                else if (btnOK.Caption.Equals("Guardar"))
                                {
                                    EditText txtCant = (EditText)app.Forms.Item(FormUID).Items.Item("txtCan").Specific;
                                    EditText txtDias = (EditText)app.Forms.Item(FormUID).Items.Item("txtDia").Specific;

                                    if (txtCant.Value.ToString().Equals("") || txtDias.Value.ToString().Equals(""))
                                    {
                                        mostrarMensaje("Error: Debe llenar todos los campos de texto", tipoError);
                                    }
                                    else
                                    {
                                        ManteUdoLogo mante = new ManteUdoLogo();
                                        //Llamada para almacenar la ruta del logo y el correspondiente usuario
                                        if (mante.Almacenar(txtCant.Value + "", txtDias.Value + ""))
                                        {
                                            btnOK.Caption = "OK";
                                        }
                                        else
                                        {
                                            mostrarMensaje("Error: Fallo al guardar parámetros de configuración", tipoError);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion CONFIGURACION ALERTAS FIN CAE

                #region ENVIO DGI COMPROBANTES PENDIENTES

                if (pVal.FormUID.Equals("frmEnvCom"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmEnvioDGICfes.CerrarFormulario();
                        }
                        else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnEnv"))
                            {
                                frmEnvioDGICfes.EnviarDGI();
                            }
                            else if (pVal.ItemUID.Equals("btnOK"))
                            {
                                frmEnvioDGICfes.CerrarFormulario();
                            }
                            else if (pVal.ItemUID.Equals("btnRef"))
                            {
                                frmEnvioDGICfes.Refrescar();
                            }
                        }
                    }
                }

                #endregion ENVIO DGI COMPROBANTES PENDIENTES

                #region RAZON REFERENCIA NC

                if (pVal.FormUID.Equals("frmRazRefNC"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (!((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption.Equals("OK"))
                                {
                                    if (frmRazonReferenciaNC.Almacenar())
                                    {
                                        ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "OK";
                                    }
                                }
                                else
                                {
                                    frmRazonReferenciaNC.CerrarFormulario();
                                }

                            }
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmRazonReferenciaNC.CerrarFormulario();
                            }
                        }
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmRazonReferenciaNC.CerrarFormulario();
                        }
                    }
                    else
                    {
                        if (pVal.EventType == BoEventTypes.et_KEY_DOWN)
                        {
                            if (pVal.ItemUID.Equals("matRazRef"))
                            {
                                ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "Guardar";
                            }
                        }
                    }
                }

                #endregion RAZON REFERENCIA NC

                #region CONFIGURACION REPORTE DIARIO

                if (pVal.FormUID.Equals("frmConfRptd"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (!((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption.Equals("OK"))
                                {
                                    if (frmConfRptd.AlmacenarBD())
                                    {
                                        ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "OK";
                                    }
                                }
                                else
                                {
                                    frmConfRptd.CerrarFormulario();
                                }

                            }
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmConfRptd.CerrarFormulario();
                            }
                        }
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmConfRptd.CerrarFormulario();
                        }
                    }
                }

                #endregion CONFIGURACION REPORTE DIARIO

                #region TIPO CAMBIO


                if (pVal.FormUID.Equals("frmConfTipC"))
                {
                    if (!pVal.BeforeAction)
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if (pVal.ItemUID.Equals("btnOK"))
                            {
                                if (!((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption.Equals("OK"))
                                {
                                    if (frmConfTipoCambio.AlmacenarBD())
                                    {
                                        ((Button)app.Forms.Item(pVal.FormUID).Items.Item("btnOK").Specific).Caption = "OK";
                                    }
                                }
                                else
                                {
                                    frmConfTipoCambio.CerrarFormulario();
                                }

                            }
                            else if (pVal.ItemUID.Equals("btnCan"))
                            {
                                frmConfTipoCambio.CerrarFormulario();
                            }
                        }
                        else if (pVal.EventType == BoEventTypes.et_FORM_UNLOAD)
                        {
                            frmConfTipoCambio.CerrarFormulario();
                        }
                    }
                }

                #endregion CONFIGURACION REPORTE DIARIO

            }
            catch (Exception ex)
            {
                //     SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }



        }

        private void seteoCheck()
        {
            try
            {
                if (modoBusquedaNavegacion != true)
                {

                    Form formularioActivo = app.Forms.ActiveForm;

                    formularioActivo.Items.Item("cbElc").Enabled = true;
                    ((CheckBox)formularioActivo.Items.Item("cbElc").Specific).Checked = true;
                    formularioActivo.Items.Item("cbElc").Enabled = false;
                }

            }
            catch (Exception ex)
            {
              //  SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("error: " + ex.ToString());
            }
        }


         private void seteoFormaPago(string pType)
        {
            string formaPago = string.Empty;

            try
            {
                Form formularioActivo = app.Forms.ActiveForm;

                if (pType.Equals("133") || pType.Equals("65303"))
                            {
                                docNumPDf = ((EditText)formularioActivo.Items.Item("8").Specific).Value + "";
                                formaPago = DocumentoB1.ObtenerFormaPago("OINV", docNumPDf);
                            }
                    else if (pType.Equals("179"))
                            {
                                formaPago = DocumentoB1.ObtenerFormaPago("ORIN", docNumPDf);
                            }

                            if (formularioActivo.TypeEx.Equals("179") || formularioActivo.TypeEx.Equals("65300"))
                            {
                                //formularioActivo.Items.Item("txtSeRef").Enabled = false;
                                //formularioActivo.Items.Item("txtNumRef").Enabled = false;

                                //formularioActivo.Items.Item("cbxRazRef").Enabled = false;
                                //formularioActivo.Items.Item("stRazRef").Enabled = false;
                            }

                            if (formaPago.Equals("Contado"))
                            {
                                ((ComboBox)formularioActivo.Items.Item("cbxForPag").Specific).
                                    Select("Contado", BoSearchKey.psk_ByValue);
                            }
                            else if (formaPago.Equals("Crédito"))
                            {
                                ((ComboBox)formularioActivo.Items.Item("cbxForPag").Specific).
                                    Select("Crédito", BoSearchKey.psk_ByValue);
                            }

            }
            catch (Exception ex)
            {
                //  SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("error: " + ex.ToString());
            }
        }


        #region FIRMA DIGITAL

        /// <summary>
        /// Metodo para obtener informacion de la firma digital
        /// </summary>
        public void ObtenerFirmaDigital()
        {
            ManteUdoCertificadoDigital manteUdoFirma = new ManteUdoCertificadoDigital();

            Certificado certificado = manteUdoFirma.Consultar();

            if (certificado != null)
            {
                RUTA_CERTIFICADO = certificado.RutaCertificado;
                CLAVE_CERTIFICADO = certificado.Clave;
            }
            else
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(Mensaje.warNoConfigFirmaDigital);
            }
        }

        #endregion FIRMA DIGITAL

        #region URL WEB SERVICE

        /// <summary>
        /// Obtiene las direcciones web de los web services de la DGI
        /// </summary>
        public void ObtenerUrlWebService()
        {
            try
            {
                ManteUdoFTP manteUdoFtp = new ManteUdoFTP();

                ConfigFTP configFtp = manteUdoFtp.ConsultarURLWebService();

                if (configFtp != null)
                {
                    URL_ENVIO = configFtp.RepoWebServiceEnvio;
                    URL_CONSULTAS = configFtp.RepoWebServiceConsulta;
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion URL WEB SERVICE
        
        #region CREAR CERTIFICADO

        /// <summary>
        /// Crea los archivos de xml para los certificados
        /// </summary>
        /// <param name="cfe"></param>
        //private void CrearCertificado(CFE cfe)
        //{
        //    try
        //    {
        //        int i = 0;
        //        bool estadoAdenda = false;

        //        if (cfe.Items != null)
        //        {
        //            while (i < cfe.Items.Count)
        //            {
        //                if (cfe.Items[i].UnidadMedida.Length > 4)
        //                {
        //                    cfe.Items[i].UnidadMedida = "N/A";
        //                }
        //                i++;
        //            }
        //        }

        //        //Limpia la lista de certificados creados
        //        listaCertificadosCreados.Clear();

        //        //Agregar certificado a lista de certificados creados
        //        listaCertificadosCreados.Add(cfe);

        //        String xmlCertificado = ProcSerializacion.CrearXmlCFE(cfe);
        //        string adenda = ProcTransformacion.ObtenerAdenda(cfe);
        //        ProcTransformacion.GuardarCertificadoPrevio(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), xmlCertificado);

        //        if (cfe.InfoReferencia.Count > 0)
        //        {
        //            //Valida que la referencia sea global o especifica
        //            if (cfe.InfoReferencia[0].IndicadorReferenciaGlobal == CFEInfoReferencia.ESIndicadorReferenciaGlobal.ReferenciaGlobal)
        //            {
        //                ProcTransformacion.TransformarCertificado(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.TipoCFE, cfe.TipoDocumentoReceptor, true);
        //            }
        //            else
        //            {
        //                ProcTransformacion.TransformarCertificado(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.TipoCFE, cfe.TipoDocumentoReceptor);
        //            }
        //        }
        //        else
        //        {
        //            ProcTransformacion.TransformarCertificado(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), cfe.TipoCFE, cfe.TipoDocumentoReceptor);
        //        }

        //        if (!adenda.Equals(""))
        //        {
        //            ProcTransformacion.GenerarCFEAdenda(cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), adenda);
        //            estadoAdenda = true;
        //        }

        //        ProcTransformacion.FirmarCertificado(RUTA_CERTIFICADO, cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), CLAVE_CERTIFICADO, false);

        //        if (estadoAdenda)
        //        {
        //            ProcTransformacion.FirmarCertificado(RUTA_CERTIFICADO, cfe.TipoCFEInt, cfe.SerieComprobante, cfe.NumeroComprobante.ToString(), CLAVE_CERTIFICADO, estadoAdenda);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
        //    }
        //}


        #endregion  CREAR CERTIFICADO

        #region CODIGO DE SEGURIDAD

        /// <summary>
        /// Metodo para obtener el codigo de seguridad
        /// </summary>
        /// <param name="numeroCertificado"></param>
        /// <returns></returns>
        private string ObtenerCodigoSeguridad(string numeroCertificado)
        {
            return ProcTransformacion.ObtenerCodigoSegurdad(numeroCertificado);
        }

        #endregion CODIGO DE SEGURIDAD

        #region CREAR SOBRE

        /// <summary>
        /// Metodo para crear sobre para el comprobante
        /// </summary>
        /// <param name="cfe"></param>
        private Sobre CrearSobre(CFE cfe, bool sobreDgi)
        {
            Sobre sobre = new Sobre(cfe);
            string infoCertificado = "";

            try
            {
                infoCertificado = ProcTransformacion.ObtenerCadenaCertificado();

                if (infoCertificado.Equals(""))
                {
                    sobre = null;
                }
                else
                {
                    if (sobreDgi)
                    {
                        //Proceso para DGI
                        sobre.RucReceptor = "214844360018";
                        sobre.X509Certificate = infoCertificado;
                        sobre.ObtenerCertificadosCreados(listaCertificadosCreados);

                        string xmlSobreDGI = ProcSerializacion.CrearXmlSobre(sobre);

                        ProcTransformacion.GuardarSobrePrevio(sobre.NombrePrev, xmlSobreDGI, true);
                        ProcTransformacion.TransformarSobre(sobre.NombrePrev, sobre.Nombre, sobre.ListaCertificados, "", true);
                    }
                    else
                    {
                        //Proceso para Tercero

                        sobre.X509Certificate = infoCertificado;
                        sobre.ObtenerCertificadosCreados(listaCertificadosCreados);

                        string xmlSobreCliente = ProcSerializacion.CrearXmlSobre(sobre);

                        ProcTransformacion.GuardarSobrePrevio(sobre.NombrePrev, xmlSobreCliente, false);

                        if (!cfe.TextoLibreAdenda.Equals(""))
                        {
                            ProcTransformacion.TransformarSobre(sobre.NombrePrev, sobre.Nombre, sobre.ListaCertificados, cfe.TextoLibreAdenda, false);
                        }
                        else
                        {
                            ProcTransformacion.TransformarSobre(sobre.NombrePrev, sobre.Nombre, sobre.ListaCertificados, "", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR: " + ex.ToString());
            }

            return sobre;
        }

        #endregion CREAR SOBRE

        #region VALIDAR

        /// <summary>
        /// Determina si el campo de Docnum esta nulo
        /// </summary>
        /// <param name="formularioActivo"></param>
        /// <returns></returns>
        private bool CampoNoNull(Form formularioActivo)
        {
            bool salida = false;

            try
            {
                docNumPDf = ((EditText)formularioActivo.Items.Item("8").Specific).Value + "";
                salida = true;
            }
            catch (Exception)
            {
            }
            return salida;
        }
        #endregion VALIDAR

        #region COMUNICACION DGI

        /// <summary>
        /// Realiza el envio de  los sobre por medio del web service de DGI.
        /// </summary>
        /// <param name="sobre"></param>
        public void EnviarSobre(Sobre sobre, Sobre sobreDgi, CFE cfe, CAE cae)
        {
            try
            {
                ParametrosJobWsDGI parametrosJobWsDGI = new ParametrosJobWsDGI(RUTA_CERTIFICADO, CLAVE_CERTIFICADO, URL_ENVIO, URL_CONSULTAS, cfe, cae);
                parametrosJobWsDGI.Sobre = sobre;
                parametrosJobWsDGI.SobreDgi = sobreDgi;

                comunicacionDGI.ConsumirWsEnviarSobre(parametrosJobWsDGI);
            }
            catch (Exception ex)
            {
                app.MessageBox("ERROR: " + ex.ToString());
            }
        }

        /// <summary>
        /// Inicia el proceso para consultar el estado de los sobres en transito
        /// </summary>
        public void ConsultarEstadoSobre()
        {
            ParametrosJobWsDGI parametrosJobWsDGI = new ParametrosJobWsDGI(RUTA_CERTIFICADO, CLAVE_CERTIFICADO, URL_ENVIO, URL_CONSULTAS, null, null);

            comunicacionDGI.ConsumirWsConsultarEstadoSobre(parametrosJobWsDGI);
        }


        public void ConsultarSobreEnvioTrancados()
        {
            ParametrosJobWsDGI parametrosJobWsDGI = new ParametrosJobWsDGI(RUTA_CERTIFICADO, CLAVE_CERTIFICADO, URL_ENVIO, URL_CONSULTAS, null, null);

            comunicacionDGI.ConsumirWsConsultarEstadoSobreTrancados(parametrosJobWsDGI);
        }

        #endregion COMUNICACION DGI

        #region VERIFICAR CORREO ELECTRONICO
        /// <summary>
        /// Metodo para verificar los campos de texto y los radioButtons
        /// </summary>
        /// <returns true="Se cumplen las validaciones"
        ///          false="Algun campo no cumple las reglas de validacion"></returns>
        public bool verificarCampos(string formUID)
        {
            bool resultado = false;

            OptionBtn oBOutlook = (OptionBtn)app.Forms.Item(formUID).Items.Item("rbOutlook").Specific;
            OptionBtn oBGmail = (OptionBtn)app.Forms.Item(formUID).Items.Item("rbGmail").Specific;
            EditText txtCorreo = (EditText)app.Forms.Item(formUID).Items.Item("txtCorreo").Specific;
            EditText txtClave = (EditText)app.Forms.Item(formUID).Items.Item("txtClave").Specific;

            if ((!oBOutlook.Selected) && (!oBGmail.Selected))
            {
                errorVerifica = Mensaje.errSeleccionMedioCorreo;
            }
            else
            {
                if (oBGmail.Selected)
                {
                    if ((txtCorreo.Value.Equals("")) || (txtClave.Value.Equals("")))
                    {
                        errorVerifica = Mensaje.warRellenarCampoCorreo;
                    }
                    else
                    {
                        bool correoValida =
                            Regex.IsMatch(txtCorreo.Value, Mensaje.expRegCorreo);

                        if (correoValida)
                        {
                            resultado = true;
                        }
                        else
                        {
                            errorVerifica = Mensaje.errFormatoCorreo;
                        }
                    }
                }
                else
                {
                    resultado = true;
                }

            }
            return resultado;
        }
        #endregion VERIFICAR CORREO ELECTRONICO

        #region FUNCIONES GENERALES

        /// <summary>
        /// Enum para tipo de mensajes
        /// </summary>
        public enum tipoMensajes
        {
            error,
            advertencia,
            exito
        };

        /// <summary>
        /// Variables para el manejo de tipo de mensajes
        /// </summary>
        public static tipoMensajes tipoError = tipoMensajes.error;
        public static tipoMensajes tipoExito = tipoMensajes.exito;
        public static tipoMensajes tipoAdvertencia = tipoMensajes.advertencia;

        /// <summary>
        /// Metodo para mostrar un mensaje en la barra de status
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="tipo">Enum de tipoMensajes = {error, advertencia,exito}</param>
        public static void mostrarMensaje(string mensaje, tipoMensajes tipo)
        {
            BoStatusBarMessageType tipoMensaje = new BoStatusBarMessageType();
            if (tipo.ToString().Equals("error"))
            {
                tipoMensaje = BoStatusBarMessageType.smt_Error;
            }
            else if (tipo.ToString().Equals("exito"))
            {
                tipoMensaje = BoStatusBarMessageType.smt_Success;
            }
            else if (tipo.ToString().Equals("advertencia"))
            {
                tipoMensaje = BoStatusBarMessageType.smt_Warning;
            }

            app.StatusBar.SetText(mensaje, BoMessageTime.bmt_Short, tipoMensaje);
        }

        /// <summary>
        /// Carga el contenido de un Xml
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="serie"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        private XmlDocument ObtenerContenidoXml(string tipo, string serie, string numero)
        {
            XmlDocument resultado = new XmlDocument();
            string nombreArchivo = tipo + serie + numero + ".xml";
            FTP ftp = new FTP();

            try
            {
                //Descargar el sobre a una carpeta temporal
                if (ftp.descargarArchivos(nombreArchivo, RutasCarpetas.RutaCarpetaSobreRecibidosTemporales, 0))
                {
                    resultado.Load(RutasCarpetas.RutaCarpetaSobreRecibidosTemporales + nombreArchivo);

                    if (System.IO.File.Exists(RutasCarpetas.RutaCarpetaSobreRecibidosTemporales + nombreArchivo))
                    {
                        System.IO.File.Delete(RutasCarpetas.RutaCarpetaSobreRecibidosTemporales + nombreArchivo);
                    }
                }
                else
                {
                    resultado = null;
                }
            }
            catch (Exception ex)
            {
                app.MessageBox("ERROR: " + ex.ToString());
                resultado = null;
            }

            return resultado;
        }
        //"Generando Documento Electronico..."

        
             //for (int i = 1; i <= 100000; i++)
             //   {
             //       ProgBar.Value =  i;
             //   }

       

      

        /// <summary>
        /// Se debe ingresar valores hasta llegar a Max 100000
        /// </summary>
        /// <param name="Texto"></param>
        /// <param name="Valor"></param>       
        /// <returns></returns>
      //  public void ProgressBar(SAPbouiCOM.ProgressBar ProgBar,string Texto, int Valor)
      //  { 
                     
      //try
      //      {
                         
                                                  
      //   if (ProgBar  != null)
      //      {
      //            if (Valor >= 100000)
      //            {
      //                ProgBar.Stop();

      //                if (ProgBar != null)
      //                {
      //                    GC.SuppressFinalize(ProgBar);

      //                    //Libera de memoria el objeto factura
      //                    GC.Collect();

      //                    ProgBar = null;
      //                }
      //            }
      //            else
      //            {
      //                //Se estable la conexion para el DI API
      //                //Esto se hace cada vez por que el addon estaba perdiendo la conexion.
      //                ProcConexion ProcConexion = new ProcConexion();
      //                ProcConexion.Comp.SetSboLoginContext(SAPbouiCOM.Framework.Application.SBO_Application.Company.GetConnectionContext(ProcConexion.Comp.GetContextCookie()));
      //                ProcConexion.Comp.Connect();


      //                ProgBar.Value = Valor;
      //                ProgBar.Text = Texto;  
      //            }             
      //                }  
      //          }
                    
      
      
      //          catch (Exception ex)
      //              {
      //                  app.MessageBox("ERROR: " + ex.ToString());
      //              }
                
                     
        
      // }

        /// <summary>
        /// Envia un documento a DGI
        /// </summary>
        /// <param name="cfe"></param>
        /// <param name="cae"></param>
        public void EnviarDocumento(CFE cfe, CAE cae, DatosPDF datosPDF, string tabla, List<ResguardoPdf> resguardoPdf, string tablaCabezal)//, SAPbouiCOM.ProgressBar ProgBar = null)
        {
            AccionesFueraHilo acciones = new AccionesFueraHilo();
            JobEnvioSobre jobEnvioSobre = new JobEnvioSobre();

            try
            {
                if (cfe != null)
                {
                    caePrueba = cae;
                    acciones.EjecucionTareas(cfe);

                    //Se agreo este Sleep para darle tiempo al proceso de Codigo de Seguridad 
                    //del QR para los PC mas lentos de Procesadores
                    /*if (Procesador86())
                        {
                          Thread.Sleep(5000);
                        }
                    else
                    {
                        Thread.Sleep(4000);
                    }*/
                    
                    jobEnvioSobre.CrearPDF(cfe, cae, datosPDF, tabla, resguardoPdf, tablaCabezal);

                   // if (ProgBar != null)
                   // {
                   //// ProgressBar(ProgBar,"Generando PDF Documento Electronico...", 90000);
                   // //for (int i = 90000; i <= 100000; i++)
                   // //{
                   // //    ProgressBar(ProgBar,"Imprimiendo  Documento Electronico...", i); 
                   // //}
                   // }
                }
                else
                {
                    app.MessageBox(Globales.Mensaje.errNoExisteCae);
                }
            }
            catch (Exception ex)
            {
                app.MessageBox("ERROR: " + ex.ToString());
            }
            finally
            {
                if (acciones != null)
                {
                    GC.SuppressFinalize(acciones);
                    //Libera de memoria el objeto factura
                    GC.Collect();
                }
                if (jobEnvioSobre != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(jobEnvioSobre);
                    GC.Collect();
                }
            }
        }


        private Boolean Procesador86 ()             
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return false ;
            }

            return true;
        }

        /// <summary>
        /// Comprueba si existe el archivo para abrirlo
        /// </summary>
        /// <returns></returns>
        public bool AbrirArchivo(string ruta)
        {
            bool salida = false;

            try
            {
                XmlDocument documento = new XmlDocument();
                documento.Load(ruta);
                salida = true;
            }
            catch (Exception)
            {
            }

            return salida;
        }


         public void ActulizoFormaPago ()
        {
               try
                    {
                                ManteUdoFormaPago manteUdoFormaPago = new ManteUdoFormaPago();
                                string formaPago = manteUdoFormaPago.ObtenerDocEntryFormaPago(false);


                                if (formaPago.ToString().Equals("Contado"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxForPag").Specific).Select(1, BoSearchKey.psk_Index);
                                }
                                else if (formaPago.ToString().Equals("Crédito"))
                                {
                                    ((ComboBox)app.Forms.Item(frmDocumento.IdFormulario).Items.Item("cbxForPag").Specific).Select(0, BoSearchKey.psk_Index);
                                }

                                RutClienteCbo =  "";
                                RutClienteTxt =  "";


                    }

            catch (Exception)
            {
            }
        }
        #endregion FUNCIOJNES GENERALES
    }
}
