using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class ParametrosJobWsDGI
    {
         public ParametrosJobWsDGI()
        {
            this.RutaCertificado = "";
            this.ClaveCertificado = "";
        }

        public ParametrosJobWsDGI(string pRutaSobre, string pClaveSobre, string pUrlEnvio, string pUrlConsultas, CFE cfe, CAE cae)
        {
            this.RutaCertificado = pRutaSobre;
            this.ClaveCertificado = pClaveSobre;
            this.UrlEnvio = pUrlEnvio;
            this.UrlConsultas = pUrlConsultas;
            this.cfe = cfe;
            this.cae = cae;
        }

        private string rutaCertificado;

        public string RutaCertificado
        {
            get { return rutaCertificado; }
            set { rutaCertificado = value; }
        }

        private string claveCertificado;

        public string ClaveCertificado
        {
            get { return claveCertificado; }
            set { claveCertificado = value; }
        }

        private string urlEnvio;

        public string UrlEnvio
        {
            get { return urlEnvio; }
            set { urlEnvio = value; }
        }

        private string urlConsultas;

        public string UrlConsultas
        {
            get { return urlConsultas; }
            set { urlConsultas = value; }
        }

        private Sobre sobre;

        public Sobre Sobre
        {
            get { return sobre; }
            set { sobre = value; }
        }

        private Sobre sobreDgi;

        public Sobre SobreDgi
        {
            get { return sobreDgi; }
            set { sobreDgi = value; }
        }

        private SobreTransito sobreTransito;

        public SobreTransito SobreTransito
        {
            get { return sobreTransito; }
            set { sobreTransito = value; }
        }

        private CFE cfe;

        public CFE Cfe
        {
            get { return cfe; }
            set { cfe = value; }
        }

        private CAE cae;

        public CAE Cae
        {
            get { return cae; }
            set { cae = value;  }
        }
    }
}
