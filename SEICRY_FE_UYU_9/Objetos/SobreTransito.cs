using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class SobreTransito
    {
        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private string nombreSobre;

        public string NombreSobre
        {
            get { return nombreSobre; }
            set { nombreSobre = value; }
        }

        private string token;

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        private string idReceptor;

        public string IdReceptor
        {
            get { return idReceptor; }
            set { idReceptor = value; }
        }

        private string correoReceptor = "";

        public string CorreoReceptor
        {
            get { return correoReceptor; }
            set { correoReceptor = value; }
        }


        public enum ETipoReceptor
        {
            DGI = 1,
            Receptor = 2
        }

        private ETipoReceptor tipoReceptor;

        internal ETipoReceptor TipoReceptor
        {
            get { return tipoReceptor; }
            set { tipoReceptor = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private int numero;

        public int  Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        private int tipo;

        public int Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
