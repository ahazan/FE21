using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DatosCorreo
    {
        private bool estadoAdenda;

        public bool EstadoAdenda
        {
            get { return estadoAdenda; }
            set { estadoAdenda = value; }
        }

        private string correoReceptor;

        public string CorreoReceptor
        {
            get { return correoReceptor; }
            set { correoReceptor = value; }
        }

        private string nombreCompuesto;

        public string NombreCompuesto
        {
            get { return nombreCompuesto; }
            set { nombreCompuesto = value; }
        }

        private string nombreEmisor;

        public string NombreEmisor
        {
            get { return nombreEmisor; }
            set { nombreEmisor = value; }
        }

        private string tipoCFE;

        public string TipoCFE
        {
            get { return tipoCFE; }
            set { tipoCFE = value; }
        }
    }
}
