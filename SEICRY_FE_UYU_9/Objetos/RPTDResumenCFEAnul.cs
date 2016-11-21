using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de CFE's anulado para cada tipo de certificado incluido en el reporte diario
    /// </summary>
    public class RPTDResumenCFEAnul
    {
        private string serie;

        /// <summary>
        /// No hay cambio de serie en un rango. Una serie distinta implica un nuevo rango.
        /// <para>Tipo: ALFA 2</para>
        /// </summary>
        public string Serie
        {
            get
            {
                if (serie.Length > 2)
                    return serie.Substring(0, 2);
                return serie; }
            set { serie = value; }
        }

        private int numInicialAnulado;

        /// <summary>
        /// Número inicial del rango de documentos anulados en el periodo.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumInicialAnulado
        {
            get 
            {
                if (numInicialAnulado.ToString().Length > 7)
                    return int.Parse(numInicialAnulado.ToString().Substring(0, 7));
                return numInicialAnulado;
            }
            set { numInicialAnulado = value; }
        }

        private int numFinalAnulado;

        /// <summary>
        /// Número utilizado final del rango debe ser igual o mayor al número anulado inicial.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumFinalAnulado
        {
            get 
            {
                if (numInicialAnulado.ToString().Length > 7)
                    return int.Parse(numInicialAnulado.ToString().Substring(0, 7));
                return numFinalAnulado;
            }
            set { numFinalAnulado = value; }
        }
    }
}
