using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Correo
    {
        #region Propiedades

        private string clave;

        public string Clave
        {
            get{return clave;}
            set { clave = value; }
        }

        private string cuenta;

        public string Cuenta
        {
            get { return cuenta; }
            set { cuenta = value; }
        }

        private string opcion;

        public string Opcion
        {
            get { return opcion; }
            set { opcion = value; }
        }

        #endregion Propiedades

    }
}
