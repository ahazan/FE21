using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class CertificadoRecibido
    {
        private string tipoCFE;

        public string TipoCFE
        {
            get { return tipoCFE; }
            set { tipoCFE = value; }
        }

        private string cantCFE;

        public string CantCFE
        {
            get { return cantCFE; }
            set { cantCFE = value; }
        }


        private string serieComprobante;

        public string SerieComprobante
        {
            get { return serieComprobante; }
            set { serieComprobante = value; }
        }

        private string numeroComprobante;

        public string NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = value; }
        }

        private string idEmisor;

        public string IdEmisor
        {
            get { return idEmisor; }
            set { idEmisor = value; }
        }

        private string rucEmisor;

        public string RucEmisor
        {
            get { return rucEmisor; }
            set { rucEmisor = value; }
        }

        private string rucReceptor;

        public string RucReceptor
        {
            get { return rucReceptor; }
            set { rucReceptor = value; }
        }

        private string fechaFirma;

        public string FechaFirma
        {
            get { return fechaFirma; }
            set { fechaFirma = value; }
        }

        private string fechaSobre;

        public string FechaSobre
        {
            get { return fechaSobre; }
            set { fechaSobre = value; }
        }

        private string fechaEmision;

        public string FechaEmision
        {
            get
            {
                DateTime dt = DateTime.Now;
                fechaEmision = dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

                return fechaEmision; 
            
            }

            set { fechaEmision = value; }
        }

        private string fechaVencimiento;

        public string FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        private string razonSocial;

        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }

        private string tipoMoneda;

        public string TipoMoneda
        {
            get { return tipoMoneda; }
            set { tipoMoneda = value; }
        }

        private string montoNetoIVATasaBasica;

        public string MontoNetoIVATasaBasica
        {
            get { return montoNetoIVATasaBasica; }
            set { montoNetoIVATasaBasica = value; }
        }

        private string iVATasaBasica;

        public string IVATasaBasica
        {
            get { return iVATasaBasica; }
            set { iVATasaBasica = value; }
        }

        private string montoIVATasaBasica;

        public string MontoIVATasaBasica
        {
            get { return montoIVATasaBasica; }
            set { montoIVATasaBasica = value; }
        }

        private string montoTotal;

        public string MontoTotal
        {
            get { return montoTotal; }
            set { montoTotal = value; }
        }

        private int cantidadLineaDetalle;

        public int CantidadLineaDetalle
        {
            get { return cantidadLineaDetalle; }
            set { cantidadLineaDetalle = value; }
        }

        private string montoNoFacturado;

        public string MontoNoFacturado
        {
            get { return montoNoFacturado; }
            set { montoNoFacturado = value; }
        }

        private string montoPagar;

        public string MontoPagar
        {
            get { return montoPagar; }
            set { montoPagar = value; }
        }

        private string dNroCAE;

        public string DNroCAE
        {
            get { return dNroCAE; }
            set { dNroCAE = value; }
        }

        private string hNroCAE;

        public string HNroCAE
        {
            get { return hNroCAE; }
            set { hNroCAE = value; }
        }

        private string fVenCAE;

        public string FVenCAE
        {
            get { return fVenCAE; }
            set { fVenCAE = value; }
        }

        private List<DetCertificadoRecibido> detalleCertificadoRecibido;

        public List<DetCertificadoRecibido> DetalleCertificadoRecibido
        {
            get {return detalleCertificadoRecibido;}
            set {detalleCertificadoRecibido = value;}
        }

        private string idConsecutio;

        public string IdConsecutio
        {
            get { return idConsecutio; }
            set { idConsecutio = value; }
        }

        private string nombreSobre;

        public string NombreSobre
        {
            get { return nombreSobre; }
            set { nombreSobre = value; }
        }

        private string correoEmisor;

        public string CorreoEmisor
        {
            get { return correoEmisor; }
            set { correoEmisor = value; }
        }
    }
}
