using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class SobreReporte
    {
        private string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private long rucEmisor;

        public long RucEmisor
        {
            get { return rucEmisor; }
            set { rucEmisor = value; }
        }

        private long rucReceptor;

        public long RucReceptor
        {
            get { return rucReceptor; }
            set { rucReceptor = value; }
        }

        private long idRespuesta;

        public long IdRespuesta
        {
            get { return idRespuesta; }
            set { idRespuesta = value; }
        }

        private string nombreArchivo;

        public string NombreArchivo
        {
            get { return nombreArchivo; }
            set { nombreArchivo = value; }
        }

        private string fechaHoraRecepcion;

        public string FechaHoraRecepcion
        {
            get { return fechaHoraRecepcion; }
            set { fechaHoraRecepcion = value; }
        }

        private long idEmisor;

        public long IdEmisor
        {
            get { return idEmisor; }
            set { idEmisor = value; }
        }

        private long idReceptor;

        public long IdReceptor
        {
            get { return idReceptor; }
            set { idReceptor = value; }
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

        private string idSobre;

        public string IdSobre
        {
            get { return idSobre; }
            set { idSobre = value; }
        }

        private List<DetSobre> detalleSobre;

        public List<DetSobre> DetalleSobre
        {
            get {return detalleSobre;}
            set {detalleSobre = value;}
        }
    }
}
