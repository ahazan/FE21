using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de un CAE emitido por la DGI
    /// </summary>
    public class CAE
    {
        #region ENUMERACIONES

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
            /// E-Ticket Contingencia. CFC
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
            ERemitoExportacionContingencia = 224,

            /// <summary>
            /// Contingencia
            /// </summary>
            Contingencia = 999
        }

        #endregion ENUMERACIONES

        #region PROPIEDADES

        private string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string rucEmisor;

        public string RucEmisor
        {
            get { return rucEmisor; }
            set { rucEmisor = value; }
        }

        private ESTipoCFECFC tipoCFE;

        public ESTipoCFECFC TipoCFE
        {
            get { return tipoCFE; }
            set { tipoCFE = value; }
        }

        private string nombreDocumento;

        public string NombreDocumento
        {
            get { return nombreDocumento; }
            set { nombreDocumento = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private int numeroDesde;

        public int NumeroDesde
        {
            get { return numeroDesde; }
            set { numeroDesde = value; }
        }

        private int numeroHasta;

        public int NumeroHasta
        {
            get { return numeroHasta; }
            set { numeroHasta = value; }
        }


        private string  tipoAutorizacion;

        public string TipoAutorizacion
        {
            get { return tipoAutorizacion; }
            set { tipoAutorizacion = value; }
        }

        private string numeroAutorizacion;

        public string NumeroAutorizacion
        {
            get { return numeroAutorizacion; }
            set { numeroAutorizacion = value; }
        }

        private string fechaEmision;

        public string FechaEmision
        {
            get { return fechaEmision; }
            set { fechaEmision = value; }
        }

        private string fechaVencimiento;

        public string FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        private string indicadorConservar;

        public string IndicadorConservar
        {
            get { return indicadorConservar; }
            set { indicadorConservar = value; }
        }

        private string sucursal;

        public string Sucursal
        {
            get { return sucursal; }
            set { sucursal = value; }
        }

        private string caja;

        public string Caja
        {
            get { return caja; }
            set { caja = value; }
        }

        #endregion PROPIEDADES

        #region FUNCIONES

        /// <summary>
        /// Retorna el Tipo de CAE o CFE según un valor en string
        /// </summary>
        /// <param name="numeroCAECFE"></param>
        /// <returns></returns>
        public static ESTipoCFECFC ObtenerTipoCFECFC(string numeroCFECFC)
        {
            switch (numeroCFECFC)
            {
                    //CFE
                case "101":
                    return CAE.ESTipoCFECFC.ETicket;
                case "102":
                    return CAE.ESTipoCFECFC.NCETicket;
                case "103":
                    return CAE.ESTipoCFECFC.NDETicket;
                case "111":
                    return CAE.ESTipoCFECFC.EFactura;
                case "112":
                    return CAE.ESTipoCFECFC.NCEFactura;
                case "113":
                    return CAE.ESTipoCFECFC.NDEFactura;
                case "181":
                    return CAE.ESTipoCFECFC.ERemito;
                case "182":
                    return CAE.ESTipoCFECFC.EResguardo;
                case "121":
                    return CAE.ESTipoCFECFC.EFacturaExportacion;
                case "122":
                    return CAE.ESTipoCFECFC.NCEFacturaExportacion;
                case "123":
                    return CAE.ESTipoCFECFC.NDEFacturaExportacion;
                case "124":
                    return CAE.ESTipoCFECFC.ERemitoExportacion;

                    //CFC
                case "999":
                    return CAE.ESTipoCFECFC.Contingencia;
                case "201":
                    return CAE.ESTipoCFECFC.ETicketContingencia;
                case "202":
                    return CAE.ESTipoCFECFC.NCETicketContingencia;
                case "203":
                    return CAE.ESTipoCFECFC.NDETicketContingencia;
                case "211":
                    return CAE.ESTipoCFECFC.EFacturaContingencia;
                case "212":
                    return CAE.ESTipoCFECFC.NCEFacturaContingencia;
                case "213":
                    return CAE.ESTipoCFECFC.NDEFacturaContingencia;
                case "281":
                    return CAE.ESTipoCFECFC.ERemitoContingencia;
                case "282":
                    return CAE.ESTipoCFECFC.EResguardoContingencia;
                case "221":
                    return CAE.ESTipoCFECFC.EFacturaExportacionContingencia;
                case "222":
                    return CAE.ESTipoCFECFC.NCEFacturaExportacionContingencia;
                case "223":
                    return CAE.ESTipoCFECFC.NDEFacturaExportacionContingencia;
                case "224":
                    return CAE.ESTipoCFECFC.ERemitoExportacionContingencia;
                default:
                    return CAE.ESTipoCFECFC.EResguardoContingencia;
            }
        }

        /// <summary>
        /// Retorna el codigo del CFE/CFE en string
        /// </summary>
        /// <param name="numeroCFECFC"></param>
        /// <returns></returns>
        public static string ObtenerStringTipoCFECFC(CAE.ESTipoCFECFC numeroCFECFC)
        {
            switch (numeroCFECFC)
            {
                //CFE
                case CAE.ESTipoCFECFC.ETicket:
                    return "101";
                case CAE.ESTipoCFECFC.NCETicket:
                    return "102";
                case CAE.ESTipoCFECFC.NDETicket:
                    return "103";
                case CAE.ESTipoCFECFC.EFactura:
                    return "111";
                case CAE.ESTipoCFECFC.NCEFactura:
                    return "112";
                case CAE.ESTipoCFECFC.NDEFactura:
                    return "113";
                case CAE.ESTipoCFECFC.ERemito:
                    return "181";
                case CAE.ESTipoCFECFC.EResguardo:
                    return "182";
                case CAE.ESTipoCFECFC.EFacturaExportacion:
                    return "121";
                case CAE.ESTipoCFECFC.NCEFacturaExportacion:
                    return "122";
                case CAE.ESTipoCFECFC.NDEFacturaExportacion:
                    return "123";
                case CAE.ESTipoCFECFC.ERemitoExportacion:
                    return "124";

                //CFC
                case CAE.ESTipoCFECFC.Contingencia:
                    return "999";
                case CAE.ESTipoCFECFC.ETicketContingencia:
                    return "201";
                case CAE.ESTipoCFECFC.NCETicketContingencia:
                    return "202";
                case CAE.ESTipoCFECFC.NDETicketContingencia:
                    return "203";
                case CAE.ESTipoCFECFC.EFacturaContingencia:
                    return "211";
                case CAE.ESTipoCFECFC.NCEFacturaContingencia:
                    return "212";
                case CAE.ESTipoCFECFC.NDEFacturaContingencia:
                    return "213";
                case CAE.ESTipoCFECFC.ERemitoContingencia:
                    return "281";
                case CAE.ESTipoCFECFC.EResguardoContingencia:
                    return "282";
                case CAE.ESTipoCFECFC.EFacturaExportacionContingencia:
                    return "221";
                case CAE.ESTipoCFECFC.NCEFacturaExportacionContingencia:
                    return "222";
                case CAE.ESTipoCFECFC.NDEFacturaExportacionContingencia:
                    return "223";
                case CAE.ESTipoCFECFC.ERemitoExportacionContingencia:
                    return "224";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Retorna el Nombre de CAE o CFE según un valor en string
        /// </summary>
        /// <param name="numeroCAECFE"></param>
        /// <returns></returns>
        public static string ObtenerNombreCFECFC(string numeroCFECFC)
        {
            switch (numeroCFECFC)
            {
                //CFE
                case "101":
                    return "e-Ticket";
                case "102":
                    return "NC. e-Ticket";
                case "103":
                    return "ND. e-Ticket";
                case "111":
                    return "e-Factura";
                case "112":
                    return "NC. e-Factura";
                case "113":
                    return "ND. e-Factura";
                case "181":
                    return "e-Remito";
                case "182":
                    return "e-Resguardo";
                case "121":
                    return "e-Factura Exportacion";
                case "122":
                    return "NC. e-Factura Exportacion";
                case "123":
                    return "ND. e-Factura Exportacion";
                case "124":
                    return "e-Remito Exportacion";

                //CFC
                case "999": 
                    return "Contingencia";
                case "201":
                    return "e-Ticket Contingencia";
                case "202":
                    return "NC. e-Ticket Contingencia";
                case "203":
                    return "ND. e-Ticket Contingencia";
                case "211":
                    return "e-Factura Contingencia";
                case "212":
                    return "NC. e-Factura Contingencia";
                case "213":
                    return "ND. e-Factura Contingencia";
                case "281":
                    return "e-Remito Contingencia";
                case "282":
                    return "e-Resguardo Contingencia";
                case "221":
                    return "e-Factura Exportacion Contingencia";
                case "222":
                    return "NC. e-Factura Exportacion Contingencia";
                case "223":
                    return "ND. e-Factura Exportacion Contingencia";
                case "224":
                    return "e-Remito Exportacion Contingencia";
                default:
                    return "";
            }
        }

        #endregion FUNCIONES

    }
}
