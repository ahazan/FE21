using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SEICRY_FE_UYU_9.Objetos
{
    class CertificadosRecProcesados
    {
        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private bool aprobado;

        public bool Aprobado
        {
            get { return aprobado; }
            set { aprobado = value; }
        }

        private string tipoCom;

        public string TipoCom
        {
            get { return tipoCom; }
            set { tipoCom = value; }
        }

        private string serieCom;

        public string SerieCom
        {
            get { return serieCom; }
            set { serieCom = value; }
        }

        private string numCom;

        public string NumCom
        {
            get { return numCom; }
            set { numCom = value; }
        }

        private string fechaEmision;

        public string FechaEmision
        {
            get { return fechaEmision; }
            set { fechaEmision = value; }
        }

        private string fechaFirma;

        public string FechaFirma
        {
            get { return fechaFirma; }
            set { fechaFirma = value; }
        }

        private ArrayList motivosRechazo = new ArrayList();

        public ArrayList MotivosRechazo
        {
            get { return motivosRechazo; }
            set { motivosRechazo = value; }
        }
    }
}
