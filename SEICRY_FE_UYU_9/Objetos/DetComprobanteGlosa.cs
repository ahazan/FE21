using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DetComprobanteGlosa
    {
        private string codigoMotivoRechazo;

        public string CodigoMotivoRechazo
        {
            get { return codigoMotivoRechazo; }
            set { codigoMotivoRechazo = value; }
        }

        private string glosaMotivo;

        public string GlosaMotivo
        {
            get { return glosaMotivo; }
            set { glosaMotivo = value; }
        }

        private string detalleRechazo;

        public string DetalleRechazo
        {
            get { return detalleRechazo; }
            set { detalleRechazo = value; }
        }

    }
}