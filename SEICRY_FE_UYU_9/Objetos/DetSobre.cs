using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class DetSobre
    {
        private string estadoRecepcion;

        public string EstadoRecepcion
        {
            get { return estadoRecepcion; }
            set { estadoRecepcion = value; }
        }

        private string token;

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        private string fechaHoraConsulta;

        public string FechaHoraConsulta
        {
            get { return fechaHoraConsulta; }
            set { fechaHoraConsulta = value; }
        }


        private string codigoMotivoRechazo;

        public string CodigoMotivoRechazo
        {
            get { return codigoMotivoRechazo; }
            set { codigoMotivoRechazo = value; }
        }       

        private string glosaMotivoRechazo;

        public string GlosaMotivoRechazo
        {
            get { return glosaMotivoRechazo; }
            set { glosaMotivoRechazo = value; }
        }

        private string detalleRechazo;

        public string DetalleRechazo
        {
            get { return detalleRechazo; }
            set { detalleRechazo = value; }
        }
    }
}