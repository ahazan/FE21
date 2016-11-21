using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de medios de pago asignada a un CFE.
    /// <para>De 1 a 40 lineas permitidas</para>
    /// </summary>
    public class CFEMediosPago
    {
        private int numeroLinea;

        /// <summary>
        /// Número de la referencia. 
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public int NumeroLinea
        {
            get
            { 
                if(numeroLinea.ToString().Length > 2)
                    return int.Parse(numeroLinea.ToString().Substring(0, 2));
                return int.Parse(numeroLinea.ToString());
            }
            set { numeroLinea = value; }
        }

        private int codigoMedioPago;

        /// <summary>
        /// Código asignado al concepto.
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        public int CodigoMedioPago
        {
            get 
            { 
                if(codigoMedioPago.ToString().Length > 3)
                    return int.Parse( codigoMedioPago.ToString().Substring(0,3));
                return int.Parse(codigoMedioPago.ToString());
            }
            set { codigoMedioPago = value; }
        }

        private string glosa;

        /// <summary>
        /// Ubicación para Impresión.
        /// <para>Tipo: ALFA 50</para>
        /// </summary>
        public string Glosa
        {
            get 
            { 
                if(glosa.Length > 50)
                    return glosa.Substring(0,50);
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

        private double valorPago = 0;

        /// <summary>
        /// Valor del pago.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double ValorPago
        {
            get 
            { 
                if(valorPago.ToString().Length > 17)
                    return double.Parse( valorPago.ToString().Substring(0,17));
                return double.Parse(valorPago.ToString());
            }
            set { valorPago = value; }
        }
    }
}
