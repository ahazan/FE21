using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class Comprobantes
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

        private int idRespuesta;

        public int IdRespuesta
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

        private int idEmisor;

        public int IdEmisor
        {
            get { return idEmisor; }
            set { idEmisor = value; }
        }

        private int idReceptor;

        public int IdReceptor
        {
            get { return idReceptor; }
            set { idReceptor = value; }
        }

        private int cantidadComprobantesSobre;

        public int CantidadComprobantesSobre
        {
            get { return cantidadComprobantesSobre; }
            set { cantidadComprobantesSobre = value; }
        }

        private int cantidadComprobantesResponden;

        public int CantidadComprobantesResponden
        {
            get { return cantidadComprobantesResponden; }
            set { cantidadComprobantesResponden = value; }
        }

        private int cantidadCFEAceptados;

        public int CantidadCFEAceptados
        {
            get { return cantidadCFEAceptados; }
            set { cantidadCFEAceptados = value; }
        }

        private int cantidadCFERechazados;

        public int CantidadCFERechazados
        {
            get { return cantidadCFERechazados; }
            set { cantidadCFERechazados = value; }
        }

        private int cantidadCFCAceptados;

        public int CantidadCFCAceptados
        {
            get { return cantidadCFCAceptados; }
            set { cantidadCFCAceptados = value; }
        }

        private int cantidadCFCObservados;

        public int CantidadCFCObservados
        {
            get { return cantidadCFCObservados; }
            set { cantidadCFCObservados = value; }
        }

        private int cantidadOtrosRechazados;

        public int CantidadOtrosRechazados
        {
            get { return cantidadOtrosRechazados; }
            set { cantidadOtrosRechazados = value; }
        }

        private string fechaHoraFirma;

        public string FechaHoraFirma
        {
            get { return fechaHoraFirma; }
            set { fechaHoraFirma = value; }
        }

        private List<DetComprobante> detalleComprobante = new List<DetComprobante>();

        public List<DetComprobante> DetalleComprobante
        {
            get { return detalleComprobante; }
            set { detalleComprobante = value; }
        }

        private List<DetComprobanteGlosa> detalleGlosa = new List<DetComprobanteGlosa> ();

        public List<DetComprobanteGlosa> DetalleGlosa
        {
            get { return detalleGlosa; }
            set { detalleGlosa = value; }
        }
    }
}
