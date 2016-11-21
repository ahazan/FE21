using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    public class ResguardoPdf
    {
        string docEntry = "";

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        string numeroFactura = "";

        public string NumeroFactura
        {
            get { return numeroFactura; }
            set { numeroFactura = value; }
        }

        string fechaFactura = "";

        public string FechaFactura
        {
            get { return fechaFactura; }
            set { fechaFactura = value; }
        }

        string montoImponible = "";

        public string MontoImponible
        {
            get { return montoImponible; }
            set { montoImponible = value; }
        }

        string impuesto = "";

        public string Impuesto
        {
            get { return impuesto; }
            set { impuesto = value; }
        }

        string porcentajeRetencion = "";

        public string PorcentajeRetencion
        {
            get { return porcentajeRetencion; }
            set { porcentajeRetencion = value; }
        }

        string importeRetencion = "";

        public string ImporteRetencion
        {
            get { return importeRetencion; }
            set { importeRetencion = value; }
        }
    }
}
