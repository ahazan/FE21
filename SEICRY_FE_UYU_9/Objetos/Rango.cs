using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estrucutura de un rango de numeros utlizando para los distintos tipos de documentos electronicos
    /// </summary>
    class Rango
    {
        #region CONSTRUCTOR

        public Rango(CAE.ESTipoCFECFC tipoDocumento,int numeroInicial, int numeroFinal, int numeroActual, string serie, string validoDesde, string validoHasta, string idCAE, string activo)
        {
            TipoDocumento = tipoDocumento;
            NumeroInicial = numeroInicial;
            NumeroFinal = numeroFinal;
            NumeroActual = numeroActual;
            Serie = serie;
            ValidoDesde = validoDesde;
            ValidoHasta = validoHasta;
            IdCAE = idCAE;
            Activo = activo;
        }

        public Rango()
        {

        }

        #endregion CONSTRUCTOR

        #region PROPIEDAES

        private CAE.ESTipoCFECFC tipoDocumento;

        public CAE.ESTipoCFECFC TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        private int numeroInicial;

        public int NumeroInicial
        {
            get { return numeroInicial; }
            set { numeroInicial = value; }
        }

        private int numeroFinal;

        public int NumeroFinal
        {
            get { return numeroFinal; }
            set { numeroFinal = value; }
        }

        private int numeroActual;

        public int NumeroActual
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

        private string validoDesde;

        public string ValidoDesde
        {
            get { return validoDesde; }
            set { validoDesde = value; }
        }

        private string validoHasta;

        public string ValidoHasta
        {
            get { return validoHasta; }
            set { validoHasta = value; }
        }

        private string idCAE;

        public string IdCAE
        {
            get { return idCAE; }
            set { idCAE = value; }
        }

        private string activo;

        public string Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        #endregion PROPIEDADES
    }
}
