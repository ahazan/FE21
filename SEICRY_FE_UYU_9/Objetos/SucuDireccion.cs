using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de los datos de Sucursales Direccion
    /// </summary>
    class SucuDireccion
    {

        private string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private string idSucuDire;

        public string IdidSucuDire
        {
            get { return idSucuDire; }
            set { idSucuDire = value; }
        }

        private string calle;

        public string Calle
        {
            get { return calle; }
            set { calle = value; }
        }

        private string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string ciudad;

        public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }

      
    }
}
