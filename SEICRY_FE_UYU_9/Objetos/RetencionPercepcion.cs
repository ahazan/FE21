using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de los datos de retencion/percepcion
    /// </summary>
    class RetencionPercepcion
    {
        private string idRetencionPercepcion;

        public string IdRetencionPercepcion
        {
            get { return idRetencionPercepcion; }
            set { idRetencionPercepcion = value; }
        }

        private string sujetoPasivo;

        public string SujetoPasivo
        {
            get { return sujetoPasivo; }
            set { sujetoPasivo = value; }
        }

        private string contribuyenteRetenido;

        public string ContribuyenteRetenido
        {
            get { return contribuyenteRetenido; }
            set { contribuyenteRetenido = value; }
        }

        private string agenteResponsable;

        public string AgenteResponsable
        {
            get { return agenteResponsable; }
            set { agenteResponsable = value; }
        }

        private string formularioLineaBeta;

        public string FormularioLineaBeta
        {
            get { return formularioLineaBeta; }
            set { formularioLineaBeta = value; }
        }

        private string codigoRetencion;

        public string CodigoRetencion
        {
            get { return codigoRetencion; }
            set { codigoRetencion = value; }
        }
    }
}
