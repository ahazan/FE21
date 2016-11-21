using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de recepcion / percepcion para cada CFE. Se pueden incluir 20 repeticiones de pares código-valor.
    /// </summary>
    public class CFERetencPercep
    {
        private string codigoRetencPercep = "";

        /// <summary>
        /// Código Retención/Percepción de acuerdo a codificación DGI: No de Formulario más No de línea. Códigos 9999001 a 9999999 para retenciones/percepciones de impuestos no administrados por DGI.
        /// <para>Tipo: ALFA 8</para>
        /// </summary>
        public string CodigoRetencPercep
        {
            get 
            { 
                if(codigoRetencPercep.Length > 8)
                    return codigoRetencPercep.Substring(0,8);
                return codigoRetencPercep;
            }
            set { codigoRetencPercep = value; }
        }

        private double valorRetencPercep;

         /// Valor de la Retención/Percepción asociado al código indicado en CodigoRecepPercep
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double ValorRetencPercep
        {
            get
            { 
                if(valorRetencPercep.ToString().Length > 17)
                    return double.Parse( valorRetencPercep.ToString().Substring(0,17));
                return double.Parse(valorRetencPercep.ToString());
            }
            set { valorRetencPercep = value; }
        }
    }
}
