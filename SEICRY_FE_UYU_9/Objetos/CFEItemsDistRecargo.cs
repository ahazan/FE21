using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de distribucion de recargo para cada item agregado el CFE. Se pueden incluir 5 repeticiones de pares Tipo-Valor.
    /// </summary>
    public class CFEItemsDistRecargo
    {
        public enum ESTipoSubrecargo
        {
            Monto = 1, 
            Porcentaje = 2,
        }

        /// <summary>
        /// Indica si el subrecargo está en $ o %.
        /// <para>Tipo: NUM 1</para>
        /// </summary>
        public ESTipoSubrecargo TipoSubrecargo { set; get; }

        private double valorSubrecargo;

        /// <summary>
        /// Total de subrecargo otorgado por ítem.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double ValorSubrecargo
        {
            get
            { 
                if(valorSubrecargo.ToString().Length > 17)
                    return double.Parse( valorSubrecargo.ToString().Substring(0,17));
                return double.Parse(valorSubrecargo.ToString());
            }
            set { valorSubrecargo = value; }
        }
    }
}
