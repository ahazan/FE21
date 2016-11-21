using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class ControlSobres
    {
        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private string numero;

        public string Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string usuarioSap;

        public string UsuarioSap
        {
            get { return usuarioSap; }
            set { usuarioSap = value; }
        }

        private string documentoSap;

        public string DocumentoSap
        {
            get { return documentoSap; }
            set { documentoSap = value; }
        }
    }
}
