using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de retencion/perecepcion para cada item agregado al CFE. 
    /// <para>Se pueden incluir 5 repeticiones de códigos de percepción o retención-tasa por item.</para>
    /// </summary>
    public class CFEItemsRetencPercep
    {
        private string codigoRetencPercep;

        /// <summary>
        /// ódigo Retención/Percepción de acuerdo a codificación DGI: No de Formulario más No de línea. Por ejemplo: 2183211 (Formulario 2183- IVA Frigoríficos y Mataderos), 2181410 (formulario 2181-Reducción Alícuota Ley 17.934). Códigos 9999001 a 9999999 para retenciones/percepciones de impuestos no administrados por DGI.
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

        private double tasa;

        /// <summary>
        /// Tasa vigente a la fecha del comprobante en %
        /// <para>Tipo: NUM 6</para>
        /// </summary>
        public double Tasa
        {
            get 
            { 
                if(tasa.ToString().Length > 6)
                    return double.Parse( tasa.ToString().Substring(0,6));
                return double.Parse(tasa.ToString());
            }
            set { tasa = value; }
        }

        private double montoSujetoRetencPercep;

        /// <summary>
        /// Monto informado asociado al código y tasa indicados.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoSujetRetencPercep
        {
            get 
            { 
                if(montoSujetoRetencPercep.ToString().Length > 17)
                    return double.Parse( montoSujetoRetencPercep.ToString().Substring(0,17));
                return double.Parse(montoSujetoRetencPercep.ToString());
            }
            set { montoSujetoRetencPercep = value; }
        }

        private double valorRetencPercep;

        /// <summary>
        /// Valor de la Retención/Percepción asociada al código y tasa indicados.
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
