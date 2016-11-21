using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class EstadoCertificadoRecibido
    {
        private string motivo;

        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        private string glosa;

        public string Glosa
        {
            get { return glosa; }
            set { glosa = value; }
        }

        private string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        private string idConsecutivo;

        public string IdConsecutivo
        {
            get { return idConsecutivo; }
            set { idConsecutivo = value; }
        }

        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }
    }
}
