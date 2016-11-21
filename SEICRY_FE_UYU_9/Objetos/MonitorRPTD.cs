using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class MonitorRPTD
    {
        private string version;
        
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string rucReceptor;

        public string RucReceptor
        {
            get { return rucReceptor; }
            set { rucReceptor = value; }
        }

        private string rucEmisor;

        public string RucEmisor
        {
            get { return rucEmisor; }
            set { rucEmisor = value; }
        }

        private string nombreArchivo;

        public string NombreArchivo
        {
            get { return nombreArchivo; }
            set { nombreArchivo = value; }
        }

        private string fechaRptd;

        public string FechaRptd
        {
            get { return fechaRptd; }
            set { fechaRptd = value; }
        }

        private string idEmisor;

        public string IdEmisor
        {
            get { return idEmisor; }
            set { idEmisor = value; }
        }

        private string idReceptor;

        public string IdReceptor
        {
            get { return idReceptor; }
            set { idReceptor = value; }
        }

        private string fechaResumen;

        public string FechaResumen
        {
            get { return fechaResumen; }
            set { fechaResumen = value; }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private List<MonitorRPTDDET> detalle;

        public List<MonitorRPTDDET> Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        private string secuenciaEnvio;

        public string SecuenciaEnvio
        {
            get { return secuenciaEnvio; }
            set { secuenciaEnvio = value; }
        }
    }
}
