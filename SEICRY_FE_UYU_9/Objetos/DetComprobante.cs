using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DetComprobante
    {
        private int numeroOrdinal;

        public int NumeroOrdinal
        {
            get { return numeroOrdinal; }
            set { numeroOrdinal = value; }
        }

        private int tipoCFE;

        public int TipoCFE
        {
            get { return tipoCFE; }
            set { tipoCFE = value; }
        }

        private string serieComprobante;

        public string SerieComprobante
        {
            get { return serieComprobante; }
            set { serieComprobante = value; }
        }

        private int numeroComprobante;

        public int NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = value; }
        }

        private string fechaComprobante;

        public string FechaComprobante
        {
            get { return fechaComprobante; }
            set { fechaComprobante = value; }
        }

        private string fechaHoraFirma;

        public string FechaHoraFirma
        {
            get { return fechaHoraFirma; }
            set { fechaHoraFirma = value; }
        }

        private string estadoRecepcion;

        public string EstadoRecepcion
        {
            get { return estadoRecepcion; }
            set { estadoRecepcion = value; }
        }

        private int idRespuestaSobre;

        public int IdRespuestaSobre 
        {
            get { return idRespuestaSobre; }
            set { idRespuestaSobre = value; }
        }

        private int idEmisorSobre;

        public int IdEmisorSobre
        {
            get { return idEmisorSobre; }
            set { idEmisorSobre = value; }
        }

        private int idReceptorSobre;

        public int IdReceptorSobre
        {
            get { return idReceptorSobre; }
            set { idReceptorSobre = value; }
        }

        private CFE.ESTipoReceptor tipoReceptor;

        public CFE.ESTipoReceptor TipoReceptor
        {
            get { return tipoReceptor; }
            set { tipoReceptor = value; }
        }
    }
}