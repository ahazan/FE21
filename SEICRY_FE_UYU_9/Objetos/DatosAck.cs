using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DatosAck
    {
        private string idEmisor;

        public string IdEmisor
        {
            get { return idEmisor; }
            set { idEmisor = value; }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}
