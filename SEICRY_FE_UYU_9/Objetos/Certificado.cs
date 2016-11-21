using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Certificado
    {     
        private string rutaCertificado;

        public string RutaCertificado
        {
            get { return rutaCertificado; }
            set { rutaCertificado = value; }
        }

        private string clave;

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }
    }
}
