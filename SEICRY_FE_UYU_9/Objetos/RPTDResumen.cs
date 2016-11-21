using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la esctructura de cada comprobante incluido en el reporte diario
    /// </summary>
    public class RPTDResumen
    {
        #region COMPROBANTE
       
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

        private int cantCFEUtilizados = 0;

        /// <summary>
        /// Cantidad de CFE emitidos y anulados por tipo de comprobante. Si no hay comprobantes utilizados se debe registrar 0.No corresponde informar los CFC utilizados.
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public int CantCFEUtilizados
        {
            get 
            {
                if (cantCFEUtilizados.ToString().Length > 10)
                    return int.Parse(cantCFEUtilizados.ToString().Substring(0, 10));
                return cantCFEUtilizados; }
            set { cantCFEUtilizados = value; }
        }

        private int cantComprobantesMayorLimite;

        /// <summary>
        /// Cantidad de CFE y CFC con montos totales, excluido el IVA, mayores al tope establecido, que deben enviarse a la DGI uno a uno en oportunidad de la emisión. Si no hay documentos emitidos se debe registrar 0
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public int CantComprobantesMayorLimite
        {
            get
            {
                if (cantComprobantesMayorLimite.ToString().Length > 10)
                    return int.Parse(cantComprobantesMayorLimite.ToString().Substring(0, 10));
                return cantComprobantesMayorLimite; }
            set { cantComprobantesMayorLimite = value; }
        }

        private int cantComprobantesAnulados;

        /// <summary>
        /// Cantidad de CFE anulados por tipo de comprobante. Refiere a comprobantes no emitidos y no a los emitidos que se anulan con una Nota de Crédito. Si no hay comprobantes anulados se debe registrar 0. No corresponde informar la cantidad de CFC anulados en este campo.
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public int CantComprobantesAnulados
        {
            get 
            {
                if (cantComprobantesAnulados.ToString().Length > 10)
                    return int.Parse(cantComprobantesAnulados.ToString().Substring(0, 10));
                return cantComprobantesAnulados; }
            set { cantComprobantesAnulados = value; }
        }

        private int cantCFEEmitidos = 0;

        /// <summary>
        /// Cantidad de CFE emitidos por tipo de comprobante. Si no hay comprobantes emitidos se debe registrar 0. No corresponde informar la cantidad de CFC emitidos en este campo.
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public int CantCFEEmitidos
        {
            get 
            {
                if (cantCFEEmitidos.ToString().Length > 10)
                    return int.Parse(cantCFEEmitidos.ToString().Substring(0, 10));
                return cantCFEEmitidos; 
            }
            set { cantCFEEmitidos = value; }
        }

        private int cantCFCEmitidos;

        /// <summary>
        /// Cantidad de CFC emitidos por tipo de comprobante. Si no hay documentos emitidos se debe registrar 0. No corresponde informar la cantidad de CFE emitidos en este campo.
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public int CantCFCEmitidos
        {
            get
            {
                if (cantCFCEmitidos.ToString().Length > 10)
                    return int.Parse(cantCFCEmitidos.ToString().Substring(0, 10));
                return cantCFCEmitidos;
            }
            set { cantCFCEmitidos = value; }
        }
        
        #endregion  COMPROBANTE

        #region RESUMEN DE MONTOS

        private List<RPTDResumenMontos> montos = new List<RPTDResumenMontos>();

        /// <summary>
        /// Lista de montos para cada certificado
        /// </summary>
        public List<RPTDResumenMontos> Montos
        {
            get { return montos; }
            set { montos = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de RPTDResumenMontos. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarMontos(RPTDResumenMontos rptdResumenMontos)
        /// </summary>
        /// <returns></returns>
        public RPTDResumenMontos NuevoMontos()
        {
            return new RPTDResumenMontos();
        }

        /// <summary>
        /// Agrega un nuevo listado de montos a la lista de montos.
        /// </summary>
        /// <param name="rptdResumenMontos"></param>
        public void AgregarMontos(RPTDResumenMontos rptdResumenMontos)
        {
            if (Montos.Count < 200)
            {
                Montos.Add(rptdResumenMontos);
            }
        }

        #endregion RESUMEN DE MONTOS

        #region CFE's UTILIZADOS

        private List<RPTDResumenCFEUtil> cfeUtilizados = new List<RPTDResumenCFEUtil>();

        /// <summary>
        /// Lista de cfe's utilizados por tipo de certificado
        /// </summary>
        public List<RPTDResumenCFEUtil> CfeUtilizados
        {
            get { return cfeUtilizados; }
            set { CfeUtilizados = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de RPTDResumenCFEUtil. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarCFEUtil(RPTDResumenCFEUtil cfeUtil)
        /// </summary>
        /// <returns></returns>
        public RPTDResumenCFEUtil NuevoCFEUtil()
        {
            return new RPTDResumenCFEUtil();
        }

        /// <summary>
        /// Agrega un nuevo cfe utilizado.
        /// </summary>
        /// <param name="rptdResumenCfeUtil"></param>
        public void AgregarCFEUtil(RPTDResumenCFEUtil rptdResumenCfeUtil)
        {
            if (CfeUtilizados.Count < 5000)
            {
                CfeUtilizados.Add(rptdResumenCfeUtil);
            }
        }

        #endregion CFE's UTILIZADOS

        #region CFE's ANULADOS

        private List<RPTDResumenCFEAnul> cfeAnulados = new List<RPTDResumenCFEAnul>();

        /// <summary>
        /// Lista de cfe's anulados por tipo de certificado
        /// </summary>
        public List<RPTDResumenCFEAnul> CfeAnulados
        {
            get { return cfeAnulados; }
            set { cfeAnulados = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de RPTDResumenCFEAnul. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarCFEAnul(RPTDResumenCFEAnul cfeAnul)
        /// </summary>
        /// <returns></returns>
        public RPTDResumenCFEAnul NuevoCFEAnul()
        {
            return new RPTDResumenCFEAnul();
        }

        /// <summary>
        /// Agrega un nuevo cfe anulado.
        /// </summary>
        /// <param name="rptdResumenCfeAnul"></param>
        public void AgregarCFEAnul(RPTDResumenCFEAnul rptdResumenCfeAnul)
        {
            if (CfeUtilizados.Count < 5000)
            {
                CfeAnulados.Add(rptdResumenCfeAnul);
            }
        }

        #endregion CFE's ANULADOS
    }
}
