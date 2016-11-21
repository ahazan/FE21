using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura para la lista de descuentos o recargos informativos agregados al CFE.
    /// <para>Pueden ser de 0 hasta 20 líneas.</para>
    /// </summary>
    [Serializable]
    public class CFEDescRecInfo
    {
        private int numeroLinea;

        /// <summary>
        /// Número de descuento o recargo.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public int NumeroLinea
        {
            get 
            { 
                if(numeroLinea.ToString().Length > 2)
                    return int.Parse( numeroLinea.ToString().Substring(0,2)); 
                return int.Parse( numeroLinea.ToString());
            }
            set { numeroLinea = value; }
        }

        public enum ESTipoMovimiento
        {
            Descuento = 'D',
            Recargo = 'R',
        }

        /// <summary>
        /// D(descuento) o R(recargo).
        /// <para>Tipo: ALFA 1</para>
        /// </summary>
        public ESTipoMovimiento TipoMovimiento { set; get; }

        public enum ESTipoDescRec
        {
            Monto = 1,
            Porcentaje = 2,
        }
        
        /// <summary>
        /// Indica si el descuento o recargo en $ o %.
        /// <para>Tipo: NUM 1</para>
        /// </summary>
        public ESTipoDescRec TipoDescRec { set; get; }

        private int codigoDescRec;

        /// <summary>
        /// Código asignado al concepto.
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        public int CodigoDescRec
        {
            get
            { 
                if(codigoDescRec.ToString().Length > 3)
                    return int.Parse( codigoDescRec.ToString().Substring(0,3));
                return int.Parse(codigoDescRec.ToString());
            }
            set { codigoDescRec = value; }
        }

        private string glosa;

        /// <summary>
        /// Especificación de descuento o recargo.
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

        private double valor;

        /// <summary>
        /// Valor del descuento o recargo.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double  Valor
        {
            get
            { 
                if(valor.ToString().Length > 17)
                    return double.Parse( valor.ToString().Substring(0,17));
                return double.Parse(valor.ToString());
            }
            set { valor = value; }
        }

        public enum ESIndicadorFacturacion
        {
            ExentoIva = 1,
            GravadoTasaMinima = 2,
            GravadoTasaBasica = 3,
            GravadoOtraTasa = 4,

            /// <summary>
            /// Por ejemplo docenas de trece
            /// </summary>
            EntregaGratuita = 5,
            ProductoServicioNoFacturable = 6,
            ProductoServicioNoFacturableNegativo = 7,

            /// <summary>
            /// Solo para remitos. En área de referencia se debe indicar el N° de remito que ajusta
            /// </summary>
            ItemRebajarRemitos = 8,

            /// <summary>
            /// Solo para resguardos. En área de referencia se debe indicar el N° de resguardo que anular
            /// </summary>
            ItemAnularResguardo = 9,
            ExportacionAsimilidas = 10,
            ImpuestoPercibido = 11,
            IVASuspenso = 12
        }

        /// <summary>
        /// Indica si el descuento o recargo aplica a productos o servicios exentos, o a que tasa están gravados o si corresponde a un concepto no facturable.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public ESIndicadorFacturacion IndicadorFacturacion { get; set; }
    }
}
