using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de subtotales informativos agregados a CFE.
    /// <para>Pueden ser de 0 hasta 20 subtotales</para>
    /// </summary>
    public class CFESubtotalesInfo
    {
        private int numeroSubtotal;

        /// <summary>
        /// Número Subtotal.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public int NumeroSubtotal
        {
            get
            { 
                if(numeroSubtotal.ToString().Length > 2)
                    return int.Parse( numeroSubtotal.ToString().Substring(0,2));
                return int.Parse(numeroSubtotal.ToString());
            }
            set { numeroSubtotal = value; }
        }

        private string glosa;

        /// <summary>
        /// Título del Subtotal
        /// <para>Tipo: ALFA 40</para>
        /// </summary>
        public string Glosa
        {
            get 
            { 
                if(glosa.Length > 40)
                    return glosa.Substring(0,40);
                return glosa;
            }
            set { glosa = value; }
        }

        private int orden;

        /// <summary>
        /// Ubicación para Impresión.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public int Orden
        {
            get 
            { 
                if(orden.ToString().Length > 2)
                    return int.Parse( orden.ToString().Substring(0,2));
                return int.Parse(orden.ToString());
            }
            set { orden = value; }
        }

        private double valorSubtotal;

        /// <summary>
        /// Valor del Subtotal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double ValorSubtotal
        {
            get 
            { 
                if(valorSubtotal.ToString().Length > 17)
                    return double.Parse( valorSubtotal.ToString().Substring(0,17));
                return double.Parse(valorSubtotal.ToString());
            }
            set { valorSubtotal = value; }
        }
    }
}
