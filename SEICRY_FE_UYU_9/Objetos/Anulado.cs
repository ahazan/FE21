using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Anulado
    {
        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private string serieCertificado;

        public string SerieCertificado
        {
            get { return serieCertificado; }
            set { serieCertificado = value; }
        }

        private string numeroDocumento;

        public string NumeroDocumento
        {
            get { return numeroDocumento; }
            set { numeroDocumento = value; }
        }

        private string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private int rucEmisor;

        public int RucEmisor
        {
            get { return rucEmisor; }
            set { rucEmisor = value; }
        }

        private int rucReceptor;

        public int RucReceptor
        {
            get { return rucReceptor; }
            set { rucReceptor = value; }
        }

        private int cantidadComprobantes;

        public int CantidadComprobantes
        {
            get { return cantidadComprobantes; }
            set { cantidadComprobantes = value; }
        }

        private string fechaHoraFirma;

        public string FechaHoraFirma
        {
            get { return fechaHoraFirma; }
            set { fechaHoraFirma = value; }
        }

        private string corregidoCon;

        public string CorregidoCon
        {
            get { return corregidoCon; }
            set { corregidoCon = value; }
        }

        private List<DetAnulado> detalleRechazo;

        public List<DetAnulado> DetalleRechazo
        {
            get {return detalleRechazo;}
            set {detalleRechazo = value;}
        }
    }
}
