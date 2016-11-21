using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class MonitorRPTDDET
    {
        private string codigoRechazo;
       
        public string CodigoRechazo
        {
            get { return codigoRechazo; }
            set { codigoRechazo = value; }
        }

        private string glosaRechazo;

        public string GlosaRechazo
        {
            get { return glosaRechazo; }
            set { glosaRechazo = value; }
        }

        private string detalleRechazo;

        public string DetalleRechazo
        {
            get { return detalleRechazo; }
            set { detalleRechazo = value; }
        }
    }
}
