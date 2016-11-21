using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Consecutivo
    {
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
    }
}
