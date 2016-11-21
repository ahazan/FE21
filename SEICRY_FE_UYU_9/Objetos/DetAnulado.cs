using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DetAnulado
    {
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

        private string codigoAnulacion;

        public string CodigoAnulacion
        {
            get { return codigoAnulacion; }
            set { codigoAnulacion = value; }
        }

        private string glosaRechazo;

        public string GlosaRechazo
        {
            get { return glosaRechazo; }
            set { glosaRechazo = value; }
        }
    }
}