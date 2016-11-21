using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class ValidacionCAE
    {
        string tipoDocumento;

        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        int numeroFinal;

        public int NumeroFinal
        {
            get { return numeroFinal; }
            set { numeroFinal = value; }
        }

        int numeroActual;

        public int NumeroActual
        {
            get { return numeroActual; }
            set { numeroActual = value; }
        }

        string validoDesde;

        public string ValidoDesde
        {
            get { return validoDesde; }
            set { validoDesde = value; }
        }

        string validoHasta;

        public string ValidoHasta
        {
            get { return validoHasta; }
            set { validoHasta = value; }
        }
    }
}
