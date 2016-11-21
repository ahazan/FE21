using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Impuesto
    {
        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private string tipoImpuestoDgi;

        public string TipoImpuestoDgi
        {
            get { return tipoImpuestoDgi; }
            set { tipoImpuestoDgi = value; }
        }

        private string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private string codigoImpuestoB1;

        public string CodigoImpuestoB1
        {
            get { return codigoImpuestoB1; }
            set { codigoImpuestoB1 = value; }
        }

    }
}
