using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class ParametrosJobWsDGIMasivo
    {
         public ParametrosJobWsDGIMasivo()
        {
            this.RutaCertificado = "";
            this.ClaveCertificado = "";
        }

        public ParametrosJobWsDGIMasivo(string pRutaSobre, string pClaveSobre, string pUrlEnvio, string pUrlConsultas)
        {
            this.RutaCertificado = pRutaSobre;
            this.ClaveCertificado = pClaveSobre;
            this.UrlEnvio = pUrlEnvio;
            this.UrlConsultas = pUrlConsultas;
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

        private string nombreSobre;

        public string NombreSobre
        {
            get { return nombreSobre; }
            set { nombreSobre = value; }
        }

        private SobreTransito sobreTransito;

        public SobreTransito SobreTransito
        {
            get { return sobreTransito; }
            set { sobreTransito = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private int numero;

        public int Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        private int tipo;

        public int Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
