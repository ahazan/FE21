using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Adenda
    {
        #region CONSTRUCTOR

        public Adenda()
        {
            this.ArregloAdenda = new string[10];
            this.CadenaAdenda = "";
            this.ObjetoAsignado = "";
            this.DocEntry = "";
        }

        #endregion

        #region PROPIEDADES

        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        public enum ESTipoObjetoAsignado
        {
            SN = 1, 
            DocFiscal = 2, 
            TipoCFE111 = 3,
            TipoCFE112 = 4, 
            TipoCFE113 = 5, 
            TipoCFE181 = 6
        }

        private ESTipoObjetoAsignado tipoObjetoAsignado;

        public ESTipoObjetoAsignado TipoObjetoAsignado
        {
            get { return tipoObjetoAsignado; }
            set { tipoObjetoAsignado = value; }
        }

        private string objetoAsignado;

        public string ObjetoAsignado
        {
            get { return objetoAsignado; }
            set { objetoAsignado = value; }
        }

        private string cadenaAdenda;

        public string CadenaAdenda
        {
            get { return cadenaAdenda; }
            set { cadenaAdenda = value; }
        }

        private string[] arregloAdenda;

        public string[] ArregloAdenda
        {
            get { return arregloAdenda; }
            set { arregloAdenda = value; }
        }

        #endregion
    }
}
