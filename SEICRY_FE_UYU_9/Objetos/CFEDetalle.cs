using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSerializar
{
    public class CFEDetalle
    {
        private string codigoItem;

        public string CodigoItem
        {
            get { return codigoItem; }
            set { codigoItem = value; }
        }

        private string cantidadItem;

        public string CantidadItem
        {
            get { return cantidadItem; }
            set { cantidadItem = value; }
        }

        private string unidadMedidaItem;

        public string UnidadMedidaItem
        {
            get { return unidadMedidaItem; }
            set { unidadMedidaItem = value; }
        }
    }
}
