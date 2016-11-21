using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de un emisor de facturas electronicas
    /// </summary>
    class Emisor
    {
        #region PROPIEDADES

        private long ruc;

        public long Ruc
        {
            get { return ruc; }
            set { ruc = value; }
        }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string nombreComercial;

        public string NombreComercial
        {
            get { return nombreComercial; }
            set { nombreComercial = value; }
        }

        private string numeroResolucion;

        public string NumeroResolucion
        {
            get { return numeroResolucion; }
            set { numeroResolucion = value; }
        }

        #endregion PROPIEDADES
    }
}
