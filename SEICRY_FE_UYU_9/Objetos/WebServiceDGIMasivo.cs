using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPWSDGI_1;

namespace SEICRY_FE_UYU_9.Objetos
{
    class WebServiceDGIMasivo
    {
        public WebServiceDGIMasivo(object parametros)
        {
            wSDGI = new clsWSDGI();

            ParametrosJobWsDGIMasivo parametrosJob = parametros as ParametrosJobWsDGIMasivo;

            wSDGI.CertPass = parametrosJob.ClaveCertificado;
            wSDGI.CertPatch = parametrosJob.RutaCertificado;
            wSDGI.Proxy = false;
            wSDGI.TimeOut = 30000;
            wSDGI.URL = parametrosJob.UrlEnvio;
            wSDGI.URLQueries = parametrosJob.UrlConsultas;
            //wSDGI.URL = "https://efactura.dgi.gub.uy:6443/ePrueba/ws_eprueba";
            //wSDGI.URLQueries = "https://efactura.dgi.gub.uy:6460/ePrueba/ws_consultasPrueba";
            wSDGI.Init();
        }

        clsWSDGI wSDGI;

        public clsWSDGI WSDGI
        {
            get { return wSDGI; }
            set { wSDGI = value; }
        }
    }
}
