using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SEICRY_FE_UYU_9.Objetos
{
    public class CFE
    {
        #region IDENTIFICACIÓN DEL COMPROBANTE

        private string version = "1.0";

        /// <summary>
        /// Versión del formato utilizado. 
        /// <para>Tipo: ALFA 3</para> 
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public enum ESTipoCFECFC
        {
            /// <summary>
            /// E-Ticket. CFE
            /// </summary>
            ETicket = 101,

            /// <summary>
            /// Nota de credito E-Ticket. CFE
            /// </summary>
            NCETicket = 102,

            /// <summary>
            /// Noda de debito E-Ticket. CFE
            /// </summary>
            NDETicket = 103,

            /// <summary>
            /// E-Factura. CFE
            /// </summary>
            EFactura = 111,

            /// <summary>
            /// Nota de credito E-Factura. CFE
            /// </summary>
            NCEFactura = 112,

            /// <summary>
            /// Nota de debito E-Factura. CFE
            /// </summary>
            NDEFactura = 113,

            /// <summary>
            /// E-Remito. CFE
            /// </summary>
            ERemito = 181,

            /// <summary>
            /// E-Resguardo. CFE
            /// </summary>
            EResguardo = 182,

            /// <summary>
            /// E-Tiket Contingencia. CFC
            /// </summary>
            ETicketContingencia = 201,

            /// <summary>
            /// /// Nota de credito E-Ticket Contingencia. CFC 
            /// </summary>
            NCETicketContingencia = 202,

            /// <summary>
            /// Noda de debito E-Ticket Contingencia. CFC
            /// </summary>
            NDETicketContingencia = 203,

            /// <summary>
            /// E-Factura Contingencia. CFC
            /// </summary>
            EFacturaContingencia = 211,

            /// <summary>
            /// Nota de credito E-Factura Contingencia. CFC
            /// </summary>
            NCEFacturaContingencia = 212,

            /// <summary>
            /// Nota de debito E-Factura Contingencia. CFC
            /// </summary>
            NDEFacturaContingencia = 213,

            /// <summary>
            /// E-Remito Contingencia. CFC
            /// </summary>
            ERemitoContingencia = 281,

            /// <summary>
            /// E-Resguardo Contingencia. CFC
            /// </summary>
            EResguardoContingencia = 282,

            /// <summary>
            /// E-Factura Exportacion. CFC
            /// </summary>
            EFacturaExportacion = 121,

            /// <summary>
            /// Nota Credito E-Factura Exportacion. CFC
            /// </summary>
            NCEFacturaExportacion = 122,

            /// <summary>
            /// Nota de debito E-Factura Exportacion. CFC
            /// </summary>
            NDEFacturaExportacion = 123,

            /// <summary>
            /// E-Remito Exportacion
            /// </summary>
            ERemitoExportacion = 124,

            /// <summary>
            /// E-Factura Exportacion Contingencia
            /// </summary>
            EFacturaExportacionContingencia = 221,

            /// <summary>
            /// NCEFacturaExportacionContingencia
            /// </summary>
            NCEFacturaExportacionContingencia = 222,

            /// <summary>
            /// NDEFacturaExportacionContingencia
            /// </summary>
            NDEFacturaExportacionContingencia = 223,

            /// <summary>
            /// ERemitoExportancionContingencia
            /// </summary>
            ERemitoExportacionContingencia = 224
        }

        /// <summary>
        /// Indica el CFE informado. 
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        [XmlIgnore]
        public ESTipoCFECFC TipoCFE { get; set; }

        public int TipoCFEInt
        {
            get
            {
                return (int)TipoCFE;
            }
            set
            {
                TipoCFE = (ESTipoCFECFC)value;
            }

        }

        private string serieComprobante = "";

        /// <summary>
        /// Serie asignada al comprobante. 
        /// <para>Tipo: ALFA2</para>
        /// </summary>
        public string SerieComprobante
        {
            get 
            {
                if (serieComprobante.Length > 2)
                    return serieComprobante.Substring(0,2);
                return serieComprobante;
            }
            set { serieComprobante = value; }
        }
       
        private int numeroComprobante = 0;

        /// <summary>
        /// Numero asignado al comprobante. 
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumeroComprobante
        {
            get 
            {   
                if(numeroComprobante.ToString().Length > 7)
                    return int.Parse(numeroComprobante.ToString().Substring(0,7));
                return int.Parse(numeroComprobante.ToString());
            }
            set { numeroComprobante = value; }
        }

        private string comprobanteReferencia = "";

        public string ComprobanteReferencia
        {
            get { return comprobanteReferencia; }
            set { comprobanteReferencia = value; }
        }

        private string fechaComprobante = "";

        /// <summary>
        /// Fecha del comprobante.  
        /// <para>Tipo: NUM 8.</para>
        /// <para>Formato: AAAAMMDD</para>
        /// </summary>
        public string FechaComprobante
        {
            get 
            {
                if (fechaComprobante.ToString().Length > 8)
                {                    
                    return fechaComprobante; 
                }

                return fechaComprobante;
                
            }
            set { fechaComprobante = value; }
        }
       
        public enum ESTipoTrasladoBienes
        {
            Venta = 1,
            TrasladoInterno = 2,
        }

        /// <summary>
        /// Obligatorio para remitos. 
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        [XmlIgnore]
        public ESTipoTrasladoBienes TipoTrasladoBienes { get; set; }

        public int TipoTrasladoBienesInt
        {
            get { return 1;/*(int)TipoTrasladoBienes;*/}
            set { TipoTrasladoBienes = (ESTipoTrasladoBienes)1;/*(ESTipoTrasladoBienes)value;*/}
        }

        private int periodoDesde = 0;

        /// <summary>
        /// Período de facturación para Servicios. Fecha desde (Fecha inicial del servicio facturado).  
        /// <para>Tipo: NUM 8.</para>
        /// <para>Formato: AAAAMMDD.</para>
        /// </summary>
        public int PeriodoDesde
        {
            get { return periodoDesde; }
            set { periodoDesde = value; }
        }


     

        private int periodoHasta = 0;

        /// <summary>
        /// Período de facturación para Servicios. Fecha hasta (Fecha final del servicio facturado).  
        /// <para>Tipo: NUM 8.</para>
        /// <para>Formato: AAAAMMDD.</para>
        /// </summary>
        public int PeriodoHasta
        {
            get { return periodoHasta; }
            set { periodoHasta = value; }
        }

        private string codigoSeguridad = "";

        [XmlIgnore]
        public string CodigoSeguridad
        {
            get { return codigoSeguridad; }
            set { codigoSeguridad = value; }
        }

        public enum ESIndMontosBrutos
        {
            /// <summary>
            ///  Líneas de detalle no se expresan con IVA incluido
            /// </summary>
            IVANoIncluido = 0,

            /// <summary>
            ///  Líneas de detalle se expresan con IVA incluido
            /// </summary>
            IVAIncluido = 1,
        }

        /// <summary>
        /// Indica si las líneas de detalle se expresan en montos brutos (IVA incluido)
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        [XmlIgnore]
        public ESIndMontosBrutos IndMontosBrutos { get; set; }

         [XmlIgnore]
        public int IndMontosBrutosInt
        {
            get { return (int)IndMontosBrutos; }
            set { IndMontosBrutos = (ESIndMontosBrutos)value; }
        }

        public enum ESFormaPago
        {
            Contado = 1,
            Credito = 2,
        }

        /// <summary>
        /// Indica en qué forma se pagará.
        /// <para>Tipo: NUM 1</para>
        /// </summary>
        [XmlIgnore]
        public ESFormaPago FormaPago { get; set; }

        public int FormaPagoInt
        {
            get { return (int)FormaPago; }
            set { FormaPago = (ESFormaPago)value; }
        }

        private string fechaVencimiento = "";

        /// <summary>
        /// Fecha de vencimiento.
        /// <para>Tipo: NUM 8.</para>
        /// <para>Formato: AAAAMMDD.</para>
        /// </summary>
        public string FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        private string documentoSAP = "";

        /// <summary>
        /// Numero de documento (DocEntry) creado en SAP
        /// </summary>
        public string DocumentoSAP
        {
            get { return documentoSAP; }
            set { documentoSAP = value; }
        }

        public enum ESTIpoDocumentoSAP
        {
            Articulo = 1,
            Servicio = 2
        }

        private ESTIpoDocumentoSAP tipoDocumentoSAP;

        /// <summary>
        /// Tipo de documento SAP
        /// </summary>
        public ESTIpoDocumentoSAP TipoDocumentoSAP
        {
            get { return tipoDocumentoSAP; }
            set { tipoDocumentoSAP = value; }
        }

        public enum ESEstadoCFE
        {
            PendienteRespuesta = 1,
            PendienteDGI = 2,
            PendienteReceptor = 3,
            AprobadoDGI = 4,
            AprobadoReceptor = 5,
            RechazadoDGI = 6,
            RechazadoReceptor = 7,
            MenorUI = 8
        }

        /// <summary>
        /// Estado del certificado ante la DGI
        /// </summary>
        [XmlIgnore]
        public ESEstadoCFE EstadoDGI{ get; set; }

        /// <summary>
        /// Estado del certificado ante el receptor
        /// </summary>
        [XmlIgnore]
        public ESEstadoCFE EstadoReceptor { get; set; }

        
        public enum ESViaTransporte
        {
            Maritimo = 1,
            Aereo = 2,
            Terrestre = 3,
            NA = 8,
            Otro = 9
        }

        private ESViaTransporte viaTransporte;

         [XmlIgnore]
        public ESViaTransporte ViaTransporte
        {
            get { return viaTransporte; }
            set { viaTransporte = value; }
        }

         public int ViaTransporteInt
         {
             get { return (int)ViaTransporte; }
             set { ViaTransporte = (ESViaTransporte)value; }
         }

        private string clausulaVenta = "";

        public string ClausulaVenta
        {
            get { return clausulaVenta; }
            set { clausulaVenta = value; }
        }

        public enum ESModalidadVenta
        {
            RegimenGeneral = 1,
            Consignacion = 2, 
            PrecioRevisable = 3,
            BienesPropiosExclavesAduaneros = 4,
            RegimenGeneralExportacionServicios = 90, 
            OtrasTransacciones = 99
        }

        private ESModalidadVenta modalidadVenta;

        [XmlIgnore]
        public ESModalidadVenta ModalidadVenta
        {
            get { return modalidadVenta; }
            set { modalidadVenta = value; }
        }

        public int ModalidadVentaInt
        {
            get { return (int)ModalidadVenta; }
            set { ModalidadVenta = (ESModalidadVenta)value; }
        }

        #endregion IDENTIFICACIÓN DEL COMPROBANTE

        #region EMISOR

        private long rucEmisor = 0;

        /// <summary>
        /// Corresponde al RUC del emisor electrónico.
        /// <para>Tipo: NUM 12</para>
        /// </summary>
        public long RucEmisor
        {
            get 
            { 
                if (rucEmisor.ToString().Length > 12)
                    return long.Parse(rucEmisor.ToString().Substring(0,12));
                return long.Parse(rucEmisor.ToString());
            }
            set { rucEmisor = value; }
        }

        private string nombreEmisor = "";

        /// <summary>
        /// Nombre o Denominación Emisor.
        /// <para>Tipo: ALFA 150</para>
        /// </summary>
        public string NombreEmisor
        {
            get 
            {
                if (nombreEmisor != null)
                {
                    if (nombreEmisor.Length > 150)
                    {
                        return nombreEmisor.Substring(0, 150);
                    }
                }
                return nombreEmisor;
            }
            set { nombreEmisor = value; }
        }

        private string nombreComercialEmisor = "";

        /// <summary>
        /// Nombre comercial Emisor
        /// <para>Tipo: ALFA 30</para>
        /// </summary>
        public string NombreComercialEmisor
        {
            get 
            { 
                if(nombreComercialEmisor.Length > 30)
                    return nombreComercialEmisor.Substring(0,30);
                return nombreComercialEmisor;
            }
            set { nombreComercialEmisor = value; }
        }

        private string giroNegocioEmisor = "";

        /// <summary>
        /// Glosa indicando giro del emisor. No es preciso registrar todos los giros, sino que se podrá registrar sólo el giro que corresponde a la transacción.
        /// <para>Tipo: ALFA 60</para>
        /// </summary>
        public string GiroNegocioEmisor
        {
            get 
            { 
                if(giroNegocioEmisor.Length > 60)
                    return giroNegocioEmisor.Substring(0,60);
                return giroNegocioEmisor;
            }
            set { giroNegocioEmisor = value; }
        }

        private string telefonoEmisor = "";

        /// <summary>
        /// Telefono Emisor
        /// <para>Tipo: ALFA 20</para>
        /// </summary>
        public string TelefonoEmisor
        {
            get 
            { 
                if(telefonoEmisor.Length > 20)
                    return telefonoEmisor.Substring(0,20);
                return telefonoEmisor;
            }
            set { telefonoEmisor = value; }
        }

        private string correoEmisor = "";

        /// <summary>
        /// Correo electrónico.
        /// <para>Tipo: ALFA 80</para>
        /// </summary>
        public string CorreoEmisor
        {
            get 
            { 
                if(correoEmisor.Length > 80)
                    return correoEmisor.Substring(0,80);
                return correoEmisor;
            }
            set { correoEmisor = value; }
        }

        private string nombreCasaPrincipalEmisor = "";

        /// <summary>
        /// Indica nombre de la casa principal o sucursal que emite el CFE. Corresponde a un dato administrado por el emisor que puede ser un texto o un número.
        /// <para>Tipo: ALFA 20</para>
        /// </summary>
        public string NombreCasaPrincipalEmisor
        {
            get 
            { 
                if(nombreCasaPrincipalEmisor.Length > 20)
                    return nombreCasaPrincipalEmisor.Substring(0,20);
                return nombreCasaPrincipalEmisor;
            }
            set { nombreCasaPrincipalEmisor = value; }
        }

        private string codigoCasaPrincipalEmisor = "0";

        /// <summary>
        /// Código numérico entregado por la DGI que identifica a la casa principal o a la sucursal desde donde se realiza la operación.
        /// <para>Tipo: NUM 4</para>
        /// </summary>
        public string CodigoCasaPrincipalEmisor
        {
            get 
            { 
                if(codigoCasaPrincipalEmisor.Length > 4)
                    return codigoCasaPrincipalEmisor.Substring(0,4);
                return codigoCasaPrincipalEmisor;
            }
            set { codigoCasaPrincipalEmisor = value; }
        }




        private string fechaFirma = "0";

        /// <summary>
        /// Código numérico entregado por la DGI que identifica a la casa principal o a la sucursal desde donde se realiza la operación.
        /// <para>Tipo: NUM 4</para>
        /// </summary>
        public string FechaFirma
        {
            get
            {
                DateTime dt = DateTime.Parse(DateTime.Now.ToString() );
                fechaFirma = dt.ToString("yyyy'-'MM'-'dd");


                return fechaFirma;
            }
            set { fechaFirma = value; }
        }
        
        /// <summary>
        /// Caja del emisor. Agregado por necesidad del requerimiento. No existe en documentacion oficial
        /// </summary>
        private string cajaEmisor = "";

        public string CajaEmisor
        {
            get { return cajaEmisor; }
            set { cajaEmisor = value; }
        }

        private string domicilioFiscalEmisor = "";

        /// <summary>
        /// Domicilio fiscal declarado en el RUC, que identifica a la casa principal o a la sucursal desde donde se realiza la operación
        /// <para>Tipo: ALFA 70</para>
        /// </summary>
        public string DomicilioFiscalEmisor
        {
            get 
            {
                if (domicilioFiscalEmisor.Length > 70)
                    return domicilioFiscalEmisor.Substring(0, 70);
                return domicilioFiscalEmisor;
                
            }
            set { domicilioFiscalEmisor = value; }
        }

        private string ciuidadEmisor = "";

        /// <summary>
        /// Análogo a Domicilio Fiscal
        /// <para>Tipo:  ALFA 30</para>
        /// </summary>
        public string CiuidadEmisor
        {
            get 
            { 
                if(ciuidadEmisor.Length > 30)
                    return ciuidadEmisor.Substring(0,30);
                return ciuidadEmisor;
            }
            set { ciuidadEmisor = value; }
        }

        private string departamentoEmisor = "";

        /// <summary>
        /// Análogo a Domicilio Fiscal
        /// <para>Tipo: ALFA 30</para>
        /// </summary>
        public string DepartamentoEmisor
        {
            get 
            { 
                if(departamentoEmisor.Length > 30)
                    return departamentoEmisor.Substring(0,30);
                return departamentoEmisor;
            }
            set { departamentoEmisor = value; }
        }

        private string numeroResolucion = "";

        /// <summary>
        /// Numero de resolucion emitido por DGI
        /// </summary>
        [XmlIgnore]
        public string NumeroResolucion
        {
            get { return numeroResolucion; }
            set { numeroResolucion = value; }
        }

        #endregion EMISOR

        #region RECEPTOR

        public enum ESTipoReceptor
        {
            DGI = 1,
            Receptor = 2,
        }

        public enum ESTipoDocumentoReceptor
        {
            RUC = 2,
            CI = 3,
            Otros = 4,
            Pasaporte = 5,
            DNI = 6,
            Nada = 7,
        }

        /// <summary>
        /// Indica si se trata de RUC, C.I. u Otro.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        [XmlIgnore]
        public ESTipoDocumentoReceptor TipoDocumentoReceptor { get; set; }
        
        public int TipoDocumentoReceptorInt 
        {
            get { return (int)TipoDocumentoReceptor; }
            set {  TipoDocumentoReceptor = (ESTipoDocumentoReceptor)value;}
        } 
        
        private string codigoPaisReceptor = "";

        /// <summary>
        /// Se utiliza el Estándar Internacional ISO 3166-1 alfa-2, códigos de países de 2 letras.
        /// <para>Tipo: ALFA 2</para>
        /// </summary>
        public string CodigoPaisReceptor
        {
            get 
            { 
                if(codigoPaisReceptor.Length > 2)
                    return codigoPaisReceptor.Substring(0, 2);
                return codigoPaisReceptor;
            }
            set { codigoPaisReceptor = value; }
        }

        private string numDocReceptorUruguayo = "";

        /// <summary>
        /// Corresponde al número del documento RUC o C.I.
        /// <para>Tipo ALFA 12</para>
        /// </summary>
        public string NumDocReceptorUruguayo
        {
            get 
            { 
                if(numDocReceptorUruguayo.Length > 12)
                    return numDocReceptorUruguayo.Substring(0,12);
                return numDocReceptorUruguayo; 
            }
            set { numDocReceptorUruguayo = value; }
        }

        private string numDocReceptorExtrangero = "";

        /// <summary>
        /// Corresponde al número del documento: Otros, Pasaporte o DNI
        /// <para>Tipo: ALFA 20</para>
        /// </summary>
        public string NumDocReceptorExtrangero
        {
            get 
            { 
                if(numDocReceptorExtrangero.Length > 20)
                    return numDocReceptorExtrangero.Substring(0,20);
                return numDocReceptorExtrangero;
            }
            set { numDocReceptorExtrangero = value; }
        }

        private string numDocReceptor = "";

        /// <summary>
        /// Toma el valor dependiendo del codigo de pais, puede ser NumDocReceptorUruguayo o NumDocReceptorExtrangero. 
        /// </summary>
        public string NumDocReceptor
        {
            get
            {
                if (NumDocReceptorUruguayo != "")
                {
                    numDocReceptor = NumDocReceptorUruguayo;
                }
                else
                {
                    numDocReceptor = NumDocReceptorExtrangero;
                }

                return numDocReceptor; 
            }
            set { numDocReceptor = value; }
        }

        private string nombreReceptor = "";

        /// <summary>
        /// Nombre o Denominación Receptor
        /// <para>Tipo: ALFA 150</para>
        /// </summary>
        public string NombreReceptor
        {
            get 
            { 
                if(nombreReceptor.Length > 150)
                    return nombreReceptor.Substring(0,150);
                return nombreReceptor;
            }
            set { nombreReceptor = value; }
        }

        private string direccionReceptor= "";

        /// <summary>
        /// Domicilio del Receptor
        /// <para>Tipo: ALFA 70</para>
        /// </summary>
        public string DireccionReceptor 
        {
            get 
            { 
                if(direccionReceptor.Length > 70)
                    return direccionReceptor.Substring(0,70);
                return direccionReceptor;
            }
            set { direccionReceptor = value; }
        }

        private string ciuidadReceptor = "";

        /// <summary>
        /// Análogo a Dirección Receptor.
        /// <para>Tipo: ALFA 30</para>
        /// </summary>
        public string CiuidadReceptor
        {
            get 
            { 
                if(ciuidadReceptor.Length > 30)
                    return ciuidadReceptor.Substring(0,30);
                return ciuidadReceptor;
            }
            set { ciuidadReceptor = value; }
        }

        private string departamentoReceptor = "" ;

        /// <summary>
        /// Análogo a Dirección Receptor
        /// <para>Tipo: ALFA 30</para>
        /// </summary>
        public string DepartamentoReceptor
        {
            get 
            { 
                if(departamentoReceptor.Length > 30)
                    return departamentoReceptor.Substring(0,30);
                return departamentoReceptor;
            }
            set { departamentoReceptor = value; }
        }

        private string paisReceptor = "";

        /// <summary>
        /// Análogo a Dirección Receptor
        /// <para>Tipo: ALFA 30</para>
        /// </summary>
        public string PaisReceptor
        {
            get
            { 
                if(paisReceptor.Length > 30)
                    return paisReceptor.Substring(0,30);
                return paisReceptor;
            }
            set { paisReceptor = value; }
        }

        private int codigoPostalReceptor = 0;

        /// <summary>
        /// Análogo a Dirección Receptor
        /// <para>Tipo: NUM 5</para>
        /// </summary>
        public int CodigoPostalReceptor
        {
            get 
            { 
                if(codigoPostalReceptor.ToString().Length > 5)
                    return int.Parse(codigoPostalReceptor.ToString().Substring(0,5));
                return int.Parse(codigoPostalReceptor.ToString());
            }
            set { codigoPostalReceptor = value; }
        }

        private string infoAdicionalReceptor = "";

        /// <summary>
        /// Otra información relativa al Receptor.
        /// <para>Tipo: ALFA 150</para>
        /// </summary>
        public string InfoAdicionalReceptor
        {
            get 
            { 
                if(infoAdicionalReceptor.Length > 150)
                    return infoAdicionalReceptor.Substring(0,150);
                return infoAdicionalReceptor;
            }
            set { infoAdicionalReceptor = value; }
        }

        private string destinoReceptor = "";

        /// <summary>
        /// Indicación de donde se entrega la mercadería o se presa el servicio (Dirección, Sucursal, Puerto, etc.)
        /// <para>Tipo: ALFA 100</para>
        /// </summary>
        public string DestinoReceptor
        {
            get
            { 
                if(destinoReceptor.Length > 100)
                    return destinoReceptor.Substring(0,100);
                return destinoReceptor;
            }
            set { destinoReceptor = value; }
        }

        private string numeroCompraReceptor = "";

        /// <summary>
        /// Número que identifica la compra: número de pedido, número orden de compra etc.
        /// <para>Tipo: ALFA 50</para>
        /// </summary>
        public string NumeroCompraReceptor
        {
            get 
            { 
                if(numeroCompraReceptor.Length > 50)
                    return numeroCompraReceptor.Substring(0,50);
                return numeroCompraReceptor;
            }
            set { numeroCompraReceptor = value; }
        }

        private string correoReceptor = "";

        /// <summary>
        /// Correo del receptor
        /// </summary>
        [XmlIgnore]
        public string CorreoReceptor
        {
            get { return correoReceptor; }
            set { correoReceptor = value; }
        }


        #endregion RECEPTOR

        #region TOTALES ENCABEZADO

        private string tipoModena = "";

        /// <summary>
        /// Código que corresponde a la moneda en que se registra la transacción.
        /// <para>Tipo: ALFA 3</para>
        /// </summary>
        public string TipoModena
        {
            get
            {
                if (tipoModena.Length > 3)
                    return tipoModena.Substring(0, 3);
                return tipoModena;
            }
            set { tipoModena = value; }
        }

        private double tipoCambio = 0;

        /// <summary>
        /// Factor de conversión utilizado: de otra moneda a pesos
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public double TipoCambio
        {
            get
            {
                if (tipoCambio.ToString().Length > 7)
                    return double.Parse(tipoCambio.ToString().Substring(0, 7));
                return double.Parse(tipoCambio.ToString());
            }
            set { tipoCambio = value; }
        }

        private double totalMontoNoGravado = 0;

        /// <summary>
        /// Suma de ítems no gravados menos descuentos globales más recargos globales (asignados a ítems no gravados)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNoGravado
        {
            get
            {
                if (totalMontoNoGravado.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNoGravado.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNoGravado.ToString()), 2);
            }
            set { totalMontoNoGravado = value; }
        }


        private double totalMontoNoGravadoFC = 0;

        /// <summary>
        /// Suma de ítems no gravados menos descuentos globales más recargos globales (asignados a ítems no gravados)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNoGravadoFC
        {
            get
            {
                if (totalMontoNoGravadoFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNoGravadoFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNoGravadoFC.ToString()), 2);
            }
            set { totalMontoNoGravadoFC = value; }
        }

        private double totalMontoNoGravadoExtranjero = 0;

        /// <summary>
        /// Suma de ítems no gravados extranjeros menos descuentos globales más recargos globales (asignados a ítems no gravados)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNoGravadoExtranjero
        {
            get
            {
                if (totalMontoNoGravadoExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNoGravadoExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNoGravadoExtranjero.ToString()), 2);
            }
            set { totalMontoNoGravadoExtranjero = value; }
        }


        private double totalMontoNoGravadoExtranjeroFC = 0;

        /// <summary>
        /// Suma de ítems no gravados extranjeros menos descuentos globales más recargos globales (asignados a ítems no gravados)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNoGravadoExtranjeroFC
        {
            get
            {
                if (totalMontoNoGravadoExtranjeroFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNoGravadoExtranjeroFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNoGravadoExtranjeroFC.ToString()), 2);
            }
            set { totalMontoNoGravadoExtranjeroFC = value; }
        }

        private double totalMontoExportacionAsimilados = 0;

        /// <summary>
        /// Suma de ítems de exportación y asimilados, menos descuentos globales más recargos globales (asignados a ítems de exportación y asimilados).
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoExportacionAsimilados
        {
            get
            {
                if (totalMontoExportacionAsimilados.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoExportacionAsimilados.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoExportacionAsimilados.ToString()), 2);
            }
            set { totalMontoExportacionAsimilados = value; }
        }

        private double totalMontoImpuestoPercibido = 0;
        
        /// <summary>
        /// Importe del concepto en el CFE.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoImpuestoPercibido
        {
            get
            {
                if (totalMontoImpuestoPercibido.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoImpuestoPercibido.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoImpuestoPercibido.ToString()), 2);
            }
            set { totalMontoImpuestoPercibido = value; }
        }

        private double totalMontoImpuestoPercibidoFC = 0;
        /// <summary>
        /// Importe del concepto en el CFE.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoImpuestoPercibidoFC
        {
            get
            {
                if (totalMontoImpuestoPercibidoFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoImpuestoPercibidoFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoImpuestoPercibidoFC.ToString()), 2);
            }
            set { totalMontoImpuestoPercibidoFC = value; }
        }


        private double totalMontoImpuestoPercibidoExtranjero = 0;

        /// <summary>
        /// Importe del concepto en el CFE.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoImpuestoPercibidoExtranjero
        {
            get
            {
                if (totalMontoImpuestoPercibidoExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoImpuestoPercibidoExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoImpuestoPercibidoExtranjero.ToString()), 2);
            }
            set { totalMontoImpuestoPercibidoExtranjero = value; }
        }



        private double totalMontoImpuestoPercibidoExtranjeroFC = 0;

        /// <summary>
        /// Importe del concepto en el CFE.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoImpuestoPercibidoExtranjeroFC
        {
            get
            {
                if (totalMontoImpuestoPercibidoExtranjeroFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoImpuestoPercibidoExtranjeroFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoImpuestoPercibidoExtranjeroFC.ToString()), 2);
            }
            set { totalMontoImpuestoPercibidoExtranjeroFC = value; }
        }

        private double totalMontoIVASuspenso = 0;

        /// <summary>
        /// Importe del concepto en el CFE
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoIVASuspenso
        {
            get
            {
                if (totalMontoIVASuspenso.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoIVASuspenso.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoIVASuspenso.ToString()), 2);
            }
            set { totalMontoIVASuspenso = value; }
        }


        private double totalMontoIVASuspensoFC = 0;

        /// <summary>
        /// Importe del concepto en el CFE
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoIVASuspensoFC
        {
            get
            {
                if (totalMontoIVASuspensoFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoIVASuspensoFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoIVASuspensoFC.ToString()), 2);
            }
            set { totalMontoIVASuspensoFC = value; }
        }

        private double totalMontoIVASuspensoExtranjero = 0;

        public double TotalMontoIVASuspensoExtranjero
        {
            get
            {
                if (totalMontoIVASuspensoExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoIVASuspensoExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoIVASuspensoExtranjero.ToString()), 2);
            }
            set { totalMontoIVASuspensoExtranjero = value; }
        }


        private double totalMontoIVASuspensoExtranjeroFC = 0;

        public double TotalMontoIVASuspensoExtranjeroFC
        {
            get
            {
                if (totalMontoIVASuspensoExtranjeroFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoIVASuspensoExtranjeroFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoIVASuspensoExtranjeroFC.ToString()), 2);
            }
            set { totalMontoIVASuspensoExtranjeroFC = value; }
        }


        private double totalMontoNetoIVATasaMinima = 0;

        /// <summary>
        /// Suma de ítems gravados a tasa mínima menos descuentos globales más recargos globales (asignados a ítems gravados a tasa mínima)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVATasaMinima
        {
            get
            {
                if (totalMontoNetoIVATasaMinima.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaMinima.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaMinima.ToString()), 2);
            }
            set { totalMontoNetoIVATasaMinima = value; }
        }



        private double totalMontoNetoIVATasaMinimaFC = 0;

        /// <summary>
        /// Suma de ítems gravados a tasa mínima menos descuentos globales más recargos globales (asignados a ítems gravados a tasa mínima)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVATasaMinimaFC
        {
            get
            {
                if (totalMontoNetoIVATasaMinimaFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaMinimaFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaMinimaFC.ToString()), 2);
            }
            set { totalMontoNetoIVATasaMinimaFC = value; }
        }

        private double totalMontoNetoIVATasaMinimaExtranjero;

        /// <summary>
        /// Suma de ítems gravados a tasa mínima menos descuentos globales más recargos globales (asignados a ítems gravados a tasa mínima)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVATasaMinimaExtranjero
        {
            get
            {
                if (totalMontoNetoIVATasaMinimaExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaMinimaExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaMinimaExtranjero.ToString()), 2);
            }
            set { totalMontoNetoIVATasaMinimaExtranjero = value; }
        }


          private double totalMontoNetoIVATasaMinimaExtranjeroFC;

        public double TotalMontoNetoIVATasaMinimaExtranjeroFC
        {
            get
            {
                if (totalMontoNetoIVATasaMinimaExtranjeroFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaMinimaExtranjeroFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaMinimaExtranjeroFC.ToString()), 2);
            }
            set { totalMontoNetoIVATasaMinimaExtranjeroFC = value; }
        }

        private double totalMontoNetoIVATasaBasica = 0;

        /// <summary>
        /// Suma de ítems gravados a tasa básica menos descuentos globales más recargos globales (asignados a ítems gravados a tasa básica)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVATasaBasica
        {
            get
            {
                if (totalMontoNetoIVATasaBasica.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaBasica.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaBasica.ToString()), 2);
            }
            set { totalMontoNetoIVATasaBasica = value; }
        }




        private double totalMontoNetoIVATasaBasicaFC = 0;

        /// <summary>
        /// Suma de ítems gravados a tasa básica menos descuentos globales más recargos globales (asignados a ítems gravados a tasa básica)
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVATasaBasicaFC
        {
            get
            {
                if (totalMontoNetoIVATasaBasicaFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaBasicaFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaBasicaFC.ToString()), 2);
            }
            set { totalMontoNetoIVATasaBasicaFC = value; }
        }

        private double totalMontoNetoIVATasaBasicaExtranjero = 0;

        public double TotalMontoNetoIVATasaBasicaExtranjero
        {
            get
            {
                if (totalMontoNetoIVATasaBasicaExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaBasicaExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaBasicaExtranjero.ToString()), 2);
            }
            set { totalMontoNetoIVATasaBasicaExtranjero = value; }
        }



        private double totalMontoNetoIVATasaBasicaExtranjeroFC = 0;

        public double TotalMontoNetoIVATasaBasicaExtranjeroFC
        {
            get
            {
                if (totalMontoNetoIVATasaBasicaExtranjeroFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVATasaBasicaExtranjeroFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVATasaBasicaExtranjeroFC.ToString()), 2);
            }
            set { totalMontoNetoIVATasaBasicaExtranjeroFC = value; }
        }

        private double totalMontoNetoIVAOtraTasa = 0;

        /// <summary>
        /// Importe neto del concepto en el CFE
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVAOtraTasa
        {
            get
            {
                if (totalMontoNetoIVAOtraTasa.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVAOtraTasa.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVAOtraTasa.ToString()), 2);
            }
            set { totalMontoNetoIVAOtraTasa = value; }
        }


        private double totalMontoNetoIVAOtraTasaFC = 0;

        /// <summary>
        /// Importe neto del concepto en el CFE
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNetoIVAOtraTasaFC
        {
            get
            {
                if (totalMontoNetoIVAOtraTasaFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVAOtraTasaFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVAOtraTasaFC.ToString()), 2);
            }
            set { totalMontoNetoIVAOtraTasaFC = value; }
        }

        private double totalMontoNetoIVAOtraTasaExtranjero = 0;

        public double TotalMontoNetoIVAOtraTasaExtranjero
        {
            get
            {
                if (totalMontoNetoIVAOtraTasaExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVAOtraTasaExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVAOtraTasaExtranjero.ToString()), 2);
            }
            set { totalMontoNetoIVAOtraTasaExtranjero = value; }
        }



        private double totalMontoNetoIVAOtraTasaExtranjeroFC = 0;

        public double TotalMontoNetoIVAOtraTasaExtranjeroFC
        {
            get
            {
                if (totalMontoNetoIVAOtraTasaExtranjeroFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoNetoIVAOtraTasaExtranjeroFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoNetoIVAOtraTasaExtranjeroFC.ToString()), 2);
            }
            set { totalMontoNetoIVAOtraTasaExtranjeroFC = value; }
        }

        private double tasaMinimaIVA = 0;

        /// <summary>
        /// Tasa mínima vigente a la fecha del comprobante en %
        /// <para>Tipo: NUM 6</para>
        /// </summary>
        public double TasaMinimaIVA
        {
            get
            {
                if (tasaMinimaIVA.ToString().Length > 6)
                    return double.Parse(tasaMinimaIVA.ToString().Substring(0, 6));
                return double.Parse(tasaMinimaIVA.ToString());
            }
            set { tasaMinimaIVA = value; }
        }

        private double tasaBasicaIVA = 0;

        /// <summary>
        /// Tasa básica vigente a la fecha del comprobante en %
        /// <para>Tipo: NUM 6</para>
        /// </summary>
        public double TasaBasicaIVA
        {
            get
            {
                if (tasaBasicaIVA.ToString().Length > 6)
                    return double.Parse(tasaBasicaIVA.ToString().Substring(0, 6));
                return double.Parse(tasaBasicaIVA.ToString());
            }
            set { tasaBasicaIVA = value; }
        }

        private double totalIVATasaMinimaExtranjero = 0;

        public double TotalIVATasaMinimaExtranjero
        {
            get
            {
                totalIVATasaMinimaExtranjero = TotalMontoNetoIVATasaMinimaExtranjero * (TasaMinimaIVA / 100);

                if (totalIVATasaMinimaExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVATasaMinimaExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVATasaMinimaExtranjero.ToString()), 2);
            }
            set { totalIVATasaMinimaExtranjero = value; }
        }


        private double totalIVATasaMinima = 0;

        /// <summary>
        /// Importe del concepto en el CFE. Calcula el valor automaticamente ( TotalMontoNetoIVATasaMinima * TasaMinimaIVA ) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-121.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVATasaMinima
        {
            get
            {
                totalIVATasaMinima = TotalMontoNetoIVATasaMinima * (TasaMinimaIVA / 100);

                if (totalIVATasaMinima.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVATasaMinima.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVATasaMinima.ToString()), 2);
            }
            set { totalIVATasaMinima = value; }
        }



        private double totalIVATasaMinimaFC = 0;

        /// <summary>
        /// Importe del concepto en el CFE. Calcula el valor automaticamente ( TotalMontoNetoIVATasaMinima * TasaMinimaIVA ) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-121.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVATasaMinimaFC
        {
            get
            {
                totalIVATasaMinimaFC = TotalMontoNetoIVATasaMinimaFC * (TasaMinimaIVA / 100);

                if (totalIVATasaMinimaFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVATasaMinimaFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVATasaMinimaFC.ToString()), 2);
            }
            set { totalIVATasaMinimaFC = value; }
        }

        private double totalIVATasaBasicaExtranjero = 0;

        public double TotalIVATasaBasicaExtranjero
        {
            get
            {
                totalIVATasaBasicaExtranjero = TotalMontoNetoIVATasaBasicaExtranjero * (TasaBasicaIVA / 100);

                if (totalIVATasaBasicaExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVATasaBasicaExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVATasaBasicaExtranjero.ToString()), 2);

            }
            set { totalIVATasaBasicaExtranjero = value; }
        }

        private double totalIVATasaBasica = 0;

        /// <summary>
        /// Importe del concepto en el CFE. Calcula el valor automaticamente ( TotalMontoNetoIVATasaBasica * TasaBasicaIVA ) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-122.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVATasaBasica
        {
            get
            {
                totalIVATasaBasica = TotalMontoNetoIVATasaBasica * (TasaBasicaIVA / 100);

                if (totalIVATasaBasica.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVATasaBasica.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVATasaBasica.ToString()), 2);

            }
            set { totalIVATasaBasica = value; }
        }


        private double totalIVATasaBasicaFC = 0;

        /// <summary>
        /// Importe del concepto en el CFE. Calcula el valor automaticamente ( TotalMontoNetoIVATasaBasica * TasaBasicaIVA ) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-122.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVATasaBasicaFC
        {
            get
            {
                totalIVATasaBasicaFC = TotalMontoNetoIVATasaBasicaFC * (TasaBasicaIVA / 100);

                if (totalIVATasaBasicaFC.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVATasaBasicaFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVATasaBasicaFC.ToString()), 2);

            }
            set { totalIVATasaBasicaFC = value; }
        }

        private double totalIVAOtraTasaExtranjero = 0;

        public double TotalIVAOtraTasaExtranjero
        {
            get
            {
                if (totalIVAOtraTasaExtranjero.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVAOtraTasaExtranjero.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalIVAOtraTasaExtranjero.ToString()), 2);
            }
            set { totalIVAOtraTasaExtranjero = value; }
        }

        private double totalIVAOtraTasa = 0;

        /// <summary>
        /// Importe del concepto en el CFE
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVAOtraTasa
        {
            get
            {
                if (totalIVAOtraTasa.ToString().Length > 17)
                    return Math.Round(double.Parse(totalIVAOtraTasa.ToString().Substring(0, 17)),2);
                return Math.Round(double.Parse(totalIVAOtraTasa.ToString()), 2);
            }
            set { totalIVAOtraTasa = value; }
        }

        private double totalMontoTotal = 0;

        /// <summary>
        /// Importe de la sumatoria de los montos netos e IVAs. Calcula el valor automaticamente (TotalMontoNoGravado + TotalMontoExportacionAsimilados + TotalMontoImpuestoPercibido + TotalMontoIVASuspenso + TotalMontoNetoIVATasaMinima + TotalMontoNetoIVATasaBasica + TotalMontoNetoIVAOtraTasa)
        /// + (TotalIVATasaMinima + TotalIVATasaBasica + TotalIVAOtraTasa) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-124.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoTotal
        {
            get
            {
                totalMontoTotal = (TotalMontoNoGravado + TotalMontoExportacionAsimilados + TotalMontoImpuestoPercibido + TotalMontoIVASuspenso + TotalMontoNetoIVATasaMinima + TotalMontoNetoIVATasaBasica + TotalMontoNetoIVAOtraTasa)
                    + (TotalIVATasaMinima + TotalIVATasaBasica + TotalIVAOtraTasa);

                if (totalMontoTotal.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoTotal.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(totalMontoTotal.ToString()), 2);

            }
             set { totalMontoTotal = value; }
        }

        private double totalMontoRetenidoPercibido = 0;

        /// <summary>
        /// Importe de la sumatoria de los valores de retención/percepción segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-125.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoRetenidoPercibido
        {
            get
            {
                foreach (CFERetencPercep retencPercep in RetencionPercepcion)
                {
                    if (retencionPercepcion != null)
                    {
                        totalMontoRetenidoPercibido += retencPercep.ValorRetencPercep;
                    }
                }

                if (totalMontoRetenidoPercibido.ToString().Length > 17)
                    return Math.Round(double.Parse(totalMontoRetenidoPercibido.ToString().Substring(0, 17)),2);
                return Math.Round(double.Parse(totalMontoRetenidoPercibido.ToString()),2);
            }
            set { totalMontoRetenidoPercibido = value; }
        }

        private int lineas = 0;

        /// <summary>
        /// Número de líneas de detalle
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        public int Lineas
        {
            get
            {
                if (lineas.ToString().Length > 3)
                    return int.Parse(lineas.ToString().Substring(0, 3));
                return int.Parse(lineas.ToString());
            }
            set { lineas = value; }
        }

        private double montoNoFacturable = 0;

        /// <summary>
        ///  Sumatoria de Montos de bienes o servicios con Indicador de facturación menos descuentos globales asignados a ítems con indicador de facturación  más recargos globales asignados a ítems con indicador de facturación  menos montos de bienes o servicios con Indicador de facturación  más descuentos globales asignados a ítems con indicador de facturación  menos recargos globales asignados a ítems con indicador de facturación 
        ///  segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-129.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoNoFacturable
        {
            get
            {
                 montoNoFacturable = CalcularMontoNoFacturable();

                if (montoNoFacturable.ToString().Length > 17)
                    return Math.Round(double.Parse(montoNoFacturable.ToString().Substring(0, 17)),2);
                return Math.Round(double.Parse(montoNoFacturable.ToString()),2);
            }
            set { montoNoFacturable = value; }
        }

        private double montoTotalPagar = 0;

        /// <summary>
        /// Suma de (monto total + valor de la retención/percepción + monto no facturable) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-130.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoTotalPagar
        {
            get
            {
                montoTotalPagar = TotalMontoTotal + TotalMontoRetenidoPercibido + MontoNoFacturable;

                if (montoTotalPagar.ToString().Length > 17)
                    return Math.Round(double.Parse(montoTotalPagar.ToString().Substring(0, 17)),2);
                return Math.Round(double.Parse(montoTotalPagar.ToString()),2);
            }
            set { montoTotalPagar = value; }
        }

        #endregion TOTALES ENCABEZADO

        #region RETENCION/PERCEPCION

        private List<CFERetencPercep> retencionPercepcion = new List<CFERetencPercep>();

        /// <summary>
        /// Para agregar un par retencion/percepcion-valor utilice AgregarRetencPercep(CFERetencPercep cfeIRetencPercep).
        /// </summary>
        public List<CFERetencPercep> RetencionPercepcion
        {
            get { return retencionPercepcion; }
            set { retencionPercepcion = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFERetencPercep. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarRetencPercep(CFERetencPercep cfeIRetencPercep)
        /// </summary>
        /// <returns></returns>
        public CFERetencPercep NuevoRetencionPercepcion()
        {
            return new CFERetencPercep();
        }

        /// <summary>
        /// Agrega un nuevo objeto retencion/percepcion de descuento a la lista.
        /// </summary>
        /// <param name="cfeIRetencPercep"></param>
        public void AgregarRetencPercep(CFERetencPercep cfeIRetencPercep)
        {
            if (RetencionPercepcion.Count < 20)
            {
                RetencionPercepcion.Add(cfeIRetencPercep);
            }
        }

        #endregion RETENCION/PERCEPCION

        #region ITEMS

        private List<CFEItems> items = new List<CFEItems>();

        /// <summary>
        /// Lista de items
        /// </summary>
        public List<CFEItems> Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de DFEItems. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarItem(CFEItems cfeItem)
        /// </summary>
        /// <returns></returns>
        public CFEItems NuevoItem()
        {
            return new CFEItems();
        }

        /// <summary>
        /// Agrega un nuevo item a la lista de items.
        /// </summary>
        /// <param name="cfeItem"></param>
        public void AgregarItem(CFEItems cfeItem)
        {
            Items.Add(cfeItem);
        }

        #endregion ITEMS

        #region SUBTOTALES INFORMATIVOS

        private List<CFESubtotalesInfo> subtotalesInfo = new List<CFESubtotalesInfo>();

        /// <summary>
        /// Lista de subtotales informativos
        /// </summary>
        public List<CFESubtotalesInfo> SubtotalesInfo
        {
            get { return subtotalesInfo; }
            set { subtotalesInfo = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFESubtotalesInfo. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarSubtotalInfo(CFESubtotalesInfo subtotalInfo)
        /// </summary>
        /// <returns></returns>
        public CFESubtotalesInfo NuevoSubtotalInfo()
        {
            return new CFESubtotalesInfo();
        }

        /// <summary>
        /// Agrega un nuevo subtotal informativo a la lista de subtotales informativos para el CFE.
        /// </summary>
        /// <param name="subtotalInfo"></param>
        public void AgregarSubtotalInfo(CFESubtotalesInfo subtotalInfo)
        {
            if (SubtotalesInfo.Count < 20)
            {
                SubtotalesInfo.Add(subtotalInfo);
            }
        }

        #endregion SUBTOTALES INFORMATIVOS

        #region DESCUENTOS/RECARGOS INFORMATIVOS

        private List<CFEDescRecInfo> recDescInfo = new List<CFEDescRecInfo>();

        /// <summary>
        /// Lista de descuentos o recargos informativos
        /// </summary>
        public List<CFEDescRecInfo> DescRecInfo
        {
            get { return recDescInfo; }
            set { recDescInfo = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFEDescRecInfo. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarRecDescInfo(CFEDescRecInfo recsDescsInfo)
        /// </summary>
        /// <returns></returns>
        public CFEDescRecInfo NuevoDescRecInfo()
        {
            return new CFEDescRecInfo();
        }

        /// <summary>
        /// Agrega un nuevo descuento o recargo informativo a la lista de descuentos o recargos informativos para el CFE.
        /// </summary>
        /// <param name="recDescInfo"></param>
        public void AgregarDescRecInfo(CFEDescRecInfo recsDescsInfo)
        {
            if (DescRecInfo.Count < 20)
            {
                DescRecInfo.Add(recsDescsInfo);
            }
        }

        #endregion DESCUENTOS/RECARGOS INFORMATIVOS

        #region MEDIOS DE PAGO

        private List<CFEMediosPago> mediosPago = new List<CFEMediosPago>();

        /// <summary>
        /// Lista de medios de pago
        /// </summary>
        public List<CFEMediosPago> MediosPago
        {
            get { return mediosPago; }
            set { mediosPago = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFEMediosPago. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarMediosPago(CFEMediosPago medioPago)
        /// </summary>
        /// <returns></returns>
        public CFEMediosPago NuevoMediosPago()
        {
            return new CFEMediosPago();
        }

        /// <summary>
        /// Agrega un nuevo medio de pago a la lista de medios de pago para el CFE.
        /// </summary>
        /// <param name="medioPago"></param>
        public void AgregarMediosPago(CFEMediosPago medioPago)
        {
            if (DescRecInfo.Count < 40)
            {
                MediosPago.Add(medioPago);
            }
        }

        #endregion MEDIOS DE PAGO

        #region INFORMACION DE REFERENCIA

        private List<CFEInfoReferencia> infoReferencia = new List<CFEInfoReferencia>();

        /// <summary>
        /// Lista de informacion de referencia
        /// </summary>
        public List<CFEInfoReferencia> InfoReferencia
        {
            get { return infoReferencia; }
            set { infoReferencia = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFEInfoReferencia. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarInfoReferencia(CFEInfoReferencia infoReferencia)
        /// </summary>
        /// <returns></returns>
        public CFEInfoReferencia NuevoInfoReferencia()
        {
            return new CFEInfoReferencia();
        }

        /// <summary>
        /// Agrega una nueva informacion de referencia a la lista de informacion de referencia.
        /// </summary>
        /// <param name="medioPago"></param>
        public void AgregarInfoReferencia(CFEInfoReferencia infoReferencia)
        {
            if (InfoReferencia.Count < 40)
            {
                InfoReferencia.Add(infoReferencia);
            }
        }

        #endregion INFORMACION DE REFERENCIA

        #region CAE

        private long numeroCAE = 0;

        /// <summary>
        /// Número asignado por DGI a la Constancia de autorización de emisión.
        /// <para>Tipo: NUM 11</para>
        /// </summary>
        public long NumeroCAE
        {
            get
            {
                if (numeroCAE.ToString().Length > 11)
                    return long.Parse(numeroCAE.ToString().Substring(0, 11));
                return long.Parse(numeroCAE.ToString());
            }
            set { numeroCAE = value; }
        }

        private int numeroInicialCAE = 0;

        /// <summary>
        /// Número inicial del rango autorizado en el CAE.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumeroInicialCAE
        {
            get
            {
                if (numeroInicialCAE.ToString().Length > 7)
                    return int.Parse(numeroInicialCAE.ToString().Substring(0, 7));
                return int.Parse(numeroInicialCAE.ToString());
            }
            set { numeroInicialCAE = value; }
        }

        private int numeroFinalCAE = 0;

        /// <summary>
        /// Número final del rango autorizado en el CAE.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumeroFinalCAE
        {
            get
            {
                if (numeroFinalCAE.ToString().Length > 7)
                    return int.Parse(numeroFinalCAE.ToString().Substring(0, 7));
                return int.Parse(numeroFinalCAE.ToString());
            }
            set { numeroFinalCAE = value; }
        }

        private string fechaVencimientoCAE = "";

        /// <summary>
        /// Fecha de vencimiento del CAE.
        /// <para>Tipo: CHAR</para>
        /// <para>Formato: AAAA-MM-DD</para>
        /// </summary>
        public string FechaVencimientoCAE
        {
            get
            {
                DateTime dt = DateTime.Parse(fechaVencimientoCAE);
                fechaVencimientoCAE = dt.ToString("yyyy'-'MM'-'dd");

                return fechaVencimientoCAE.ToString();
            }
            set { fechaVencimientoCAE = value; }
        }

        #endregion CAE

        #region FECHA Y HORA DE FIRMA ELECTRONICA

        private string fechaHoraFirma = "";

        /// <summary>
        /// Fecha y hora de la firma electrónica avanzada del CFE.
        /// <para>Tipo: ALFA 19</para>
        /// <para>Formato: AAAA-MM-DDTHH:MM:SS</para>
        /// </summary>
        public string FechaHoraFirma
        {
            get
            {
                DateTime dt = DateTime.Now;
                fechaHoraFirma = dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

                return fechaHoraFirma;
            }
            set { fechaHoraFirma = value; }
        }

        #endregion FECHA Y HORA DE FIRMA ELECTRONICA

        #region ADENDA

        private string texto = "";

        /// <summary>
        /// Texto libre.
        /// <para>Tipo: ALFA</para>
        /// </summary>
        public string TextoLibreAdenda
        {
            get { return texto; }
            set { texto = value; }
        }

        private byte[] otro;

        /// <summary>
        /// Cualquier objeto binario.
        /// <para>Tipo: BINARIO</para>
        /// </summary>
        public byte[] Otro
        {
            get { return otro; }
            set { otro = value; }
        }

        #endregion ADENDA

        #region VALIDACION DE CAMPOS

        private string campoFaltante;

        /// <summary>
        /// Contiene el valor del ultimo campo obligatorio que no se encontro.
        /// </summary>
        public string CampoFaltante
        {
            get { return campoFaltante; }
            set { campoFaltante = value; }
        }

        private string campoErroneo;

        public string CampoErroneo
        {
            get { return campoErroneo; }
            set { campoErroneo = value; }
        }

        #endregion VALIDACION DE CAMPOS

        #region  FUNCIONES ADICIONALES

        /// <summary>
        /// Calcula el monto no factura segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-129.
        /// </summary>
        /// <returns></returns>
        private double CalcularMontoNoFacturable()
        {
            double x = 0; //Montos de bienes o servicios con Indicador de facturación = 6 (Producto o servicio no facturable)
            double y = 0; //descuentos globales asignados a ítems con indicador de facturación = 6 (Producto o servicio no facturable)
            double z = 0; //recargos globales asignados a ítems con indicador de facturación = 6 (Producto o servicio no facturable)
            double m = 0; //Montos de bienes o servicios con Indicador de facturación = 7 (Producto o servicio no facturable negativo)
            double n = 0; //descuentos globales asignados a ítems con indicador de facturación = 7 (Producto o servicio no facturable negativo)
            double o = 0; //recargos globales asignados a ítems con indicador de facturación = 7 (Producto o servicio no facturable negativo)

            //Calcula x y m
            foreach (CFEItems item in Items)
            {
                CFEItems cfeItems = new CFEItems();

                if (item.IndicadorFacturacion == 6)
                {
                    x += item.MontoItem;
                }
                else if (item.IndicadorFacturacion == 7)
                {
                    m += item.MontoItem;
                }
            }

            //Calcula y, z, n y o
            foreach (CFEDescRecInfo descRecInfo in DescRecInfo)
            {
                if (descRecInfo.IndicadorFacturacion == CFEDescRecInfo.ESIndicadorFacturacion.ProductoServicioNoFacturable && descRecInfo.TipoMovimiento == CFEDescRecInfo.ESTipoMovimiento.Descuento && descRecInfo.TipoDescRec == CFEDescRecInfo.ESTipoDescRec.Monto)
                {
                    y += descRecInfo.Valor;
                }
                else if (descRecInfo.IndicadorFacturacion == CFEDescRecInfo.ESIndicadorFacturacion.ProductoServicioNoFacturable && descRecInfo.TipoMovimiento == CFEDescRecInfo.ESTipoMovimiento.Recargo && descRecInfo.TipoDescRec == CFEDescRecInfo.ESTipoDescRec.Monto)
                {
                    z += descRecInfo.Valor;
                }
                else if (descRecInfo.IndicadorFacturacion == CFEDescRecInfo.ESIndicadorFacturacion.ProductoServicioNoFacturableNegativo && descRecInfo.TipoMovimiento == CFEDescRecInfo.ESTipoMovimiento.Descuento && descRecInfo.TipoDescRec == CFEDescRecInfo.ESTipoDescRec.Monto)
                {
                    n += descRecInfo.Valor;
                }
                else if (descRecInfo.IndicadorFacturacion == CFEDescRecInfo.ESIndicadorFacturacion.ProductoServicioNoFacturableNegativo && descRecInfo.TipoMovimiento == CFEDescRecInfo.ESTipoMovimiento.Recargo && descRecInfo.TipoDescRec == CFEDescRecInfo.ESTipoDescRec.Monto)
                {
                    o += descRecInfo.Valor;
                }
            }


            return x - y + z - m + n - o;
        }

        /// <summary>
        /// Retorna el tipo de documento receptor según el valor ingresado
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        public static CFE.ESTipoDocumentoReceptor ObtenerTipoDocumentoReceptor(string tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case "RUC":
                    return ESTipoDocumentoReceptor.RUC;
                case "C.I.":
                    return ESTipoDocumentoReceptor.CI;
                case "Otros":
                    return ESTipoDocumentoReceptor.Otros;
                case "Pasaporte":
                    return ESTipoDocumentoReceptor.Pasaporte;
                case "DNI":
                    return ESTipoDocumentoReceptor.DNI;

                default:
                    return ESTipoDocumentoReceptor.RUC;
            }
        }


        /// <summary>
        /// Retorna el Tipo de CAE o CFE según un valor en string
        /// </summary>
        /// <param name="numeroCAECFE"></param>
        /// <returns></returns>
        public static CFE.ESTipoCFECFC ObtenerTipoCFECFC(string numeroCFECFC)
        {
            switch (numeroCFECFC)
            {
                //CFE
                case "101":
                    return CFE.ESTipoCFECFC.ETicket;
                case "102":
                    return CFE.ESTipoCFECFC.NCETicket;
                case "103":
                    return CFE.ESTipoCFECFC.NDETicket;
                case "111":
                    return CFE.ESTipoCFECFC.EFactura;
                case "112":
                    return CFE.ESTipoCFECFC.NCEFactura;
                case "113":
                    return CFE.ESTipoCFECFC.NDEFactura;
                case "181":
                    return CFE.ESTipoCFECFC.ERemito;
                case "182":
                    return CFE.ESTipoCFECFC.EResguardo;
                case "121":
                    return CFE.ESTipoCFECFC.EFacturaExportacion;
                case "122":
                    return CFE.ESTipoCFECFC.NCEFacturaExportacion;
                case "123":
                    return CFE.ESTipoCFECFC.NDEFacturaExportacion;
                case "124":
                    return CFE.ESTipoCFECFC.ERemitoExportacion;

                //CFC
                case "201":
                    return CFE.ESTipoCFECFC.ETicketContingencia;
                case "202":
                    return CFE.ESTipoCFECFC.NCETicketContingencia;
                case "203":
                    return CFE.ESTipoCFECFC.NDETicketContingencia;
                case "211":
                    return CFE.ESTipoCFECFC.EFacturaContingencia;
                case "212":
                    return CFE.ESTipoCFECFC.NCEFacturaContingencia;
                case "213":
                    return CFE.ESTipoCFECFC.NDEFacturaContingencia;
                case "281":
                    return CFE.ESTipoCFECFC.ERemitoContingencia;
                case "282":
                    return CFE.ESTipoCFECFC.EResguardoContingencia;
                case "221":
                    return CFE.ESTipoCFECFC.EFacturaExportacionContingencia;
                case "222":
                    return CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia;
                case "223":
                    return CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia;
                case "224":
                    return CFE.ESTipoCFECFC.ERemitoExportacionContingencia;
                default:
                    return CFE.ESTipoCFECFC.EResguardoContingencia;
            }
        }

        /// <summary>
        /// Retorna el codigo del CFE/CFE en string
        /// </summary>
        /// <param name="numeroCFECFC"></param>
        /// <returns></returns>
        public static string ObtenerStringTipoCFECFC(CFE.ESTipoCFECFC numeroCFECFC)
        {
            switch (numeroCFECFC)
            {
                //CFE
                case CFE.ESTipoCFECFC.ETicket:
                    return "101";
                case CFE.ESTipoCFECFC.NCETicket:
                    return "102";
                case CFE.ESTipoCFECFC.NDETicket:
                    return "103";
                case CFE.ESTipoCFECFC.EFactura:
                    return "111";
                case CFE.ESTipoCFECFC.NCEFactura:
                    return "112";
                case CFE.ESTipoCFECFC.NDEFactura:
                    return "113";
                case CFE.ESTipoCFECFC.ERemito:
                    return "181";
                case CFE.ESTipoCFECFC.EResguardo:
                    return "182";
                case CFE.ESTipoCFECFC.EFacturaExportacion:
                    return "121";
                case CFE.ESTipoCFECFC.NCEFacturaExportacion:
                    return "122";
                case CFE.ESTipoCFECFC.NDEFacturaExportacion:
                    return "123";
                case CFE.ESTipoCFECFC.ERemitoExportacion:
                    return "124";

                //CFC
                case CFE.ESTipoCFECFC.ETicketContingencia:
                    return "201";
                case CFE.ESTipoCFECFC.NCETicketContingencia:
                    return "202";
                case CFE.ESTipoCFECFC.NDETicketContingencia:
                    return "203";
                case CFE.ESTipoCFECFC.EFacturaContingencia:
                    return "211";
                case CFE.ESTipoCFECFC.NCEFacturaContingencia:
                    return "212";
                case CFE.ESTipoCFECFC.NDEFacturaContingencia:
                    return "213";
                case CFE.ESTipoCFECFC.ERemitoContingencia:
                    return "281";
                case CFE.ESTipoCFECFC.EResguardoContingencia:
                    return "282";
                case CFE.ESTipoCFECFC.EFacturaExportacionContingencia:
                    return "221";
                case CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia:
                    return "222";
                case CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia:
                    return "223";
                case CFE.ESTipoCFECFC.ERemitoExportacionContingencia:
                    return "224";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Retorna el Tipo de CAE o CFE según un valor en string
        /// </summary>
        /// <param name="numeroCAECFE"></param>
        /// <returns></returns>
        public static string ObtenerStringTipoCFECFC(string numeroCFECFC)
        {
            switch (numeroCFECFC)
            {
                //CFE
                case "101":
                    return CFE.ESTipoCFECFC.ETicket.ToString();
                case "102":
                    return CFE.ESTipoCFECFC.NCETicket.ToString();
                case "103":
                    return CFE.ESTipoCFECFC.NDETicket.ToString();
                case "111":
                    return CFE.ESTipoCFECFC.EFactura.ToString();
                case "112":
                    return CFE.ESTipoCFECFC.NCEFactura.ToString();
                case "113":
                    return CFE.ESTipoCFECFC.NDEFactura.ToString();
                case "181":
                    return CFE.ESTipoCFECFC.ERemito.ToString();
                case "182":
                    return CFE.ESTipoCFECFC.EResguardo.ToString();
                case "121":
                    return CFE.ESTipoCFECFC.EFacturaExportacion.ToString();
                case "122":
                    return CFE.ESTipoCFECFC.NCEFacturaExportacion.ToString();
                case "123":
                    return CFE.ESTipoCFECFC.NDEFacturaExportacion.ToString();
                case "124":
                    return CFE.ESTipoCFECFC.ERemitoExportacion.ToString();

                //CFC
                case "201":
                    return CFE.ESTipoCFECFC.ETicketContingencia.ToString();
                case "202":
                    return CFE.ESTipoCFECFC.NCETicketContingencia.ToString();
                case "203":
                    return CFE.ESTipoCFECFC.NDETicketContingencia.ToString();
                case "211":
                    return CFE.ESTipoCFECFC.EFacturaContingencia.ToString();
                case "212":
                    return CFE.ESTipoCFECFC.NCEFacturaContingencia.ToString();
                case "213":
                    return CFE.ESTipoCFECFC.NDEFacturaContingencia.ToString();
                case "281":
                    return CFE.ESTipoCFECFC.ERemitoContingencia.ToString();
                case "282":
                    return CFE.ESTipoCFECFC.EResguardoContingencia.ToString();
                case "221":
                    return CFE.ESTipoCFECFC.EFacturaExportacionContingencia.ToString();
                case "222":
                    return CFE.ESTipoCFECFC.NCEFacturaExportacionContingencia.ToString();
                case "223":
                    return CFE.ESTipoCFECFC.NDEFacturaExportacionContingencia.ToString();
                case "224":
                    return CFE.ESTipoCFECFC.ERemitoExportacionContingencia.ToString();
                default:
                    return CFE.ESTipoCFECFC.EResguardoContingencia.ToString();
            }
        }

        #endregion FUNCIONES ADICIONALES

        #region Proceso_WebService
        private string origenFE = "";

        public string OrigenFE
        {
            get { return origenFE; }
            set { origenFE = value; }
        }
        #endregion Proceso_WebService

        #region Redondeo
        /*private CFEItems redondeo;

        public CFEItems Redondeo
        {
            get { return redondeo; }
            set { redondeo = value; }
        }*/
        #endregion Redondeo
    }
}
