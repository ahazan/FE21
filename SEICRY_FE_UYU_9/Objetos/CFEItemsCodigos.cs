using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estrucutra para la lista de codigos de item para cada uno de los items agregados a un CFE. e-Ticket y sus notas de corrección: Hasta 700. Otros CFE: Hasta 200
    /// </summary>
    public class CFEItemsCodigos
    {
        private string tipoCodigo;

        /// <summary>
        /// Tipo de codificación utilizada para el ítem, Standard: EAN, PLU, DUN, INT1, INT2.
        /// <para>Tipo: ALFA 10</para>
        /// </summary>
        public string TipoCodigo
        {
            get
            {
                if(tipoCodigo.Length > 10)
                    return tipoCodigo.Substring(0,10);
                return tipoCodigo;
            }
            set { tipoCodigo = value; }
        }

        private string codigoItem;

        /// <summary>
        /// Código del producto de acuerdo a tipo de codificación indicada en campo anterior.
        /// <para>Tipo: ALFA 35</para>
        /// </summary>
        public string CodigoItem
        {
            get 
            { 
                if(codigoItem.Length > 35)
                    return codigoItem.Substring(0,35);
                return codigoItem;
            }
            set { codigoItem = value; }
        }
    }
}
