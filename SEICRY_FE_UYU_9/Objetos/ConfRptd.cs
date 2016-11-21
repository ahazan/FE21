using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class ConfRptd
    {
        public ConfRptd()
        {
            this.DiaEjecucion = "";
            this.ModoEjecucion = "";
            this.DocEntry = "";
            this.DiaEjecucion = "";
            this.CAEGenerico = "";
            this.HoraEjec = "";
            this.AutoGenerar = "";
            this.DiaEjecucion = "";
        }

        private string secuenciaEnvio;

        public string SecuenciaEnvio
        {
            get { return secuenciaEnvio; }
            set { secuenciaEnvio = value; }
        }

        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private string diaEjecucion;

        public string DiaEjecucion
        {
            get { return diaEjecucion; }
            set { diaEjecucion = value; }
        }

        private string modoEjecucion;

        public string ModoEjecucion
        {
            get { return modoEjecucion; }
            set { modoEjecucion = value; }
        }



        private string CaeGenerico;


        public string CAEGenerico
        {
            get { return CaeGenerico; }
            set { CaeGenerico = value; }
        }


        private string autoGenerar;


        public string AutoGenerar
        {
            get { return autoGenerar; }
            set { autoGenerar = value; }
        }

        private string diaFin;


        public string DiaFin
        {
            get { return diaFin; }
            set { diaFin = value; }
        }

        private string horaEjec;


        public string HoraEjec
        {
            get { return horaEjec; }
            set { horaEjec = value; }
        }

    }

    class ConfCAEFin
    {
        public ConfCAEFin()
        {
            this.NumCaeFin = "";
            this.FechaCaeFin = "";
           
        }

        private string numCaeFin;

        public string NumCaeFin
        {
            get { return numCaeFin; }
            set { numCaeFin = value; }
        }

        private string fechaCaeFin;

        public string FechaCaeFin
        {
            get { return fechaCaeFin; }
            set { fechaCaeFin = value; }
        }      

    }

}
