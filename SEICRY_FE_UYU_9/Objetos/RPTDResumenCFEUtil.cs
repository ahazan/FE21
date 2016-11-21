using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de CFE's utilizados para cada tipo de certificado incluido en el reporte diario
    /// </summary>
    public class RPTDResumenCFEUtil
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

        private int numInicialUtilizado;

        /// <summary>
        /// Número inicial del rango de documentos utilizados en el periodo.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumInicialUtilizado
        {
            get 
            {
                if (numInicialUtilizado.ToString().Length > 7)
                    return int.Parse(numInicialUtilizado.ToString().Substring(0, 7));
                return numInicialUtilizado;
            }
            set { numInicialUtilizado = value; }
        }

        private int numFinalUtilizado;

        /// <summary>
        /// Número utilizado final del rango debe ser igual o mayor al número utilizado inicial.
        /// <para>Tipo: NUM 7</para>
        /// </summary>
        public int NumFinalUtilizado
        {
            get 
            {
                if (numInicialUtilizado.ToString().Length > 7)
                    return int.Parse(numInicialUtilizado.ToString().Substring(0, 7));
                return numFinalUtilizado;
            }
            set { numFinalUtilizado = value; }
        }
    }
}
