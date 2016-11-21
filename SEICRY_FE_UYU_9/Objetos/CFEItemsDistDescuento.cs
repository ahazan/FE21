using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de distribucion de descuentos para cada item agregado al CFE.
    /// </summary>
    public class CFEItemsDistDescuento
    {
        public enum ESTipoSubdescuento
        {
            Monto = 1,
            Porcentaje = 2,
        }
        
        /// <summary>
        /// Indica si el sub-descuento está en $ o %
        /// <para>Tipo: NUM 1</para>
        /// </summary>
        public ESTipoSubdescuento TipoSubdescuento { get; set; }

        private double valorSubdescuento;

        /// <summary>
        /// Total de sub-descuentos otorgado por ítem.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double ValorSubdescuento
        {
            get 
            { 
                if(valorSubdescuento.ToString().Length > 17)
                    return double.Parse( valorSubdescuento.ToString().Substring(0,17));
                return double.Parse(valorSubdescuento.ToString());
            }
            set { valorSubdescuento = value; }
        }
    }
}
