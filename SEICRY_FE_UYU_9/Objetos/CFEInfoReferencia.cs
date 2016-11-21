using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de informacion de referencia agregada a un CFE.
    /// <para>De 0 a 40 posibles repeticiones.</para>
    /// </summary>
    public class CFEInfoReferencia
    {
        private int numeroLinea;

        /// <summary>
        /// Número de la referencia.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public int NumeroLinea
        {
            get
            {
                if (numeroLinea.ToString().Length > 2)
                    return int.Parse(numeroLinea.ToString().Substring(0, 2));
                return int.Parse(numeroLinea.ToString());
            }
            set { numeroLinea = value; }
        }

        public enum ESIndicadorReferenciaGlobal
        {
            ReferenciaGlobal = 1,
            NoReferenciaGlobal = 0,
        }

        /// <summary>
        /// Se utiliza cuando no se puede identificar los CFE de referencia. Por ejemplo: -cuando el CFE afecta a un número de más de 40 CFE de referencia, -cuando se referencia a un documento no codificado, etc. Se debe explicitar el motivo en RazónReferencia
        /// <tipo>Tipo: NUM 1</tipo>
        /// </summary>
        public ESIndicadorReferenciaGlobal IndicadorReferenciaGlobal { set; get; }

        public int IndicadorReferenciaGlobalInt
        {
            get
            {
                return (int)IndicadorReferenciaGlobal;
            }
            set
            {
                IndicadorReferenciaGlobal = (ESIndicadorReferenciaGlobal)value;
            }

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
            ETiketContingencia = 201,

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
            NDFacturaExportacion = 123,

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
        /// Indica el CFE o CFC de referencia.
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        //public ESTipoCFECFC TipoCFEReferncia { get; set; }

        public int TipoCFEReferencia { get; set; }    

        //public int TipoCFEInt
        //{
        //    get
        //    {
        //        return (int)TipoCFEReferncia;
        //    }
        //    set
        //    {
        //        TipoCFEReferncia = (ESTipoCFECFC)value;
        //    }

        //}

        private string serieComprobanteReferencia = "";

        /// <summary>
        /// Serie asignada al comprobante al que se hace referencia.
        /// <para>Tipo: ALFA 2</para>
        /// </summary>
        public string SerieComprobanteReferencia
        {
            get
            {
                if (serieComprobanteReferencia.Length > 2)
                    return serieComprobanteReferencia.Substring(0, 2);
                return serieComprobanteReferencia;
            }
            set { serieComprobanteReferencia = value; }
        }

        private string numeroComprobanteReferencia;

        /// <summary>
        /// Numero asignado al comprobante al que se hace referencia.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public string NumeroComprobanteReferencia
        {
            get
            {
                if (numeroComprobanteReferencia.Length > 7)
                    return numeroComprobanteReferencia.Substring(0, 7);
                return numeroComprobanteReferencia;
            }
            set { numeroComprobanteReferencia = value; }
        }

        private string razonReferencia = "";

        /// <summary>
        /// Razón por la cual se hace la referencia.
        /// <para>Tipo: ALFA 90</para>
        /// </summary>
        public string RazonReferencia
        {
            get
            {
                if (razonReferencia.Length > 90)
                    return razonReferencia.Substring(0, 90);
                return razonReferencia;

            }
            set { razonReferencia = value; }
        }

        private int fechaComprobanteReferencia;

        /// <summary>
        /// Fecha del comprobante de referencia.
        /// <para>Tipo: NUM 8.</para>
        /// <para>Formato: AAAAMMDD</para>
        /// </summary>
        public int FechaComprobanteReferencia
        {
            get
            {
                if (fechaComprobanteReferencia.ToString().Length > 8)
                    return int.Parse(fechaComprobanteReferencia.ToString().Substring(0, 8));
                return int.Parse(fechaComprobanteReferencia.ToString());
            }
            set { fechaComprobanteReferencia = value; }
        }
    }
}
