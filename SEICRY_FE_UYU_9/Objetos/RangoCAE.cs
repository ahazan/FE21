using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estrucutura de un rango de numeros utlizando para los distintos tipos de documentos electronicos
    /// </summary>
    class RangoCAE
    {
        #region CONSTRUCTOR        

        public RangoCAE()
        {
            this.TipoDocumento = "";
            this.Serie = "";
            this.NumeroInicial = "";
            this.NumeroFinal = "";
            this.NumeroActual = "";
            this.FechaVencimiento = "";
        }

        #endregion CONSTRUCTOR

        #region PROPIEDAES

        private string tipoDocumento;

        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        private string numeroInicial;

        public string NumeroInicial
        {
            get { return numeroInicial; }
            set { numeroInicial = value; }
        }

        private string numeroFinal;

        public string NumeroFinal
        {
            get { return numeroFinal; }
            set { numeroFinal = value; }
        }

        private string numeroActual;

        public string NumeroActual
        {
            get { return numeroActual; }
            set { numeroActual = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private string fechaVencimiento;

        public string FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }
      
        #endregion PROPIEDADES
    }
}
