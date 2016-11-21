using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DetCertificadoRecibido
    {
        private string nombreItem;

        public string NombreItem
        {
            get { return nombreItem; }
            set { nombreItem = value; }
        }

        private double cantidad;

        public double Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        private string precioUnitario;

        public string PrecioUnitario
        {
            get { return precioUnitario; }
            set { precioUnitario = value; }
        }

        private string montoItem;

        public string MontoItem
        {
            get { return montoItem; }
            set { montoItem = value; }
        }       

        private string numeroComprobante;

        public string NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = value; }
        }

        private string serieComprobante;

        public string SerieComprobante
        {
            get { return serieComprobante; }
            set { serieComprobante = value; }
        }

        private string tipoCFE;

        public string TipoCFE
        {
            get { return tipoCFE; }
            set { tipoCFE = value; }
        }

        private string tipoMoneda;

        public string TipoMoneda
        {
            get { return tipoMoneda; }
            set { tipoMoneda = value; }
        }
    }
}