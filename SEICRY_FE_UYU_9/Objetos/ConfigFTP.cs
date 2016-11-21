using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de la configuracion para la comunicacion con el servidor FTP
    /// </summary>
    class ConfigFTP
    {
        private string servidor;

        public string Servidor
        {
            get { return servidor; }
            set { servidor = value; }
        }

        private string repoComp;

        public string RepoComp
        {
            get { return repoComp; }
            set { repoComp = value; }
        }

        private string repoSob;

        public string RepoSob
        {
            get { return repoSob; }
            set { repoSob = value; }
        }

        private string repoBandejaEntrada;

        public string RepoBandejaEntrada
        {
            get { return repoBandejaEntrada; }
            set { repoBandejaEntrada = value; }
        }

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        private string clave;

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        private string ruta;

        public string Ruta
        {
            get { return ruta; }
            set { ruta = value; }
        }

        private string repoResp;

        public string RepoResp
        {
            get { return repoResp; }
            set { repoResp = value; }
        }

        private string repoRepDi;

        public string RepoRepDi
        {
            get { return repoRepDi; }
            set { repoRepDi = value; }
        }

        private string repoContingenciaSobres;

        public string RepoContingenciaSobres
        {
            get { return repoContingenciaSobres; }
            set { repoContingenciaSobres = value; }
        }

        private string repoContingenciaSobreDgi;

        public string RepoContingenciaSobreDgi
        {
            get { return repoContingenciaSobreDgi; }
            set { repoContingenciaSobreDgi = value; }
        }


        private string fileDelete;

        public string FileDelete
        {
            get { return fileDelete; }
            set { fileDelete = value; }
        }
               


        private string rutDgi;

        public string RutDgi
        {
            get { return rutDgi; }
            set { rutDgi = value; }
        }
        



        private string repoContingenciaComprobantes;

        public string RepoContingenciaComprobantes
        {
            get { return repoContingenciaComprobantes; }
            set { repoContingenciaComprobantes = value; }
        }

        private string repoContingenciaReportesDiarios;

        public string RepoContingenciaReportesDiarios
        {
            get { return repoContingenciaReportesDiarios; }
            set { repoContingenciaReportesDiarios = value; }
        }

        private string repoCertificadosAnulados;

        public string RepoCertificadosAnulados
        {
            get { return repoCertificadosAnulados; }
            set { repoCertificadosAnulados = value; }
        }


        private string repoWebServiceEnvio;

        public string RepoWebServiceEnvio
        {
            get { return repoWebServiceEnvio; }
            set { repoWebServiceEnvio = value; }
        }

        private string repoWebServiceConsulta;

        public string RepoWebServiceConsulta
        {
            get { return repoWebServiceConsulta; }
            set { repoWebServiceConsulta = value; }
        }
        
        private string repoCFEs;

        public string RepoCFEs
        {
            get { return repoCFEs; }
            set { repoCFEs = value; }
        }
    }
}
