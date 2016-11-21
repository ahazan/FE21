using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class SocioNegocio
    {
        private string cedulaJuridica;

        public string CedulaJuridica
        {
            get { return cedulaJuridica; }
            set { cedulaJuridica = value; }
        }

        private string entregador;

        public string Entregador
        {
            get { return entregador; }
            set { entregador = value; }
        }

        private string consumidorFinal;

        public string ConsumidorFinal
        {
            get { return consumidorFinal; }
            set { consumidorFinal = value; }
        }

        #region FE_EXPORTACION
        private bool clienteExtranjero;

        public bool ClienteExtranjero
        {
            get { return clienteExtranjero; }
            set { clienteExtranjero = value; }
        }
        #endregion FE_EXPORTACION
    }
}
