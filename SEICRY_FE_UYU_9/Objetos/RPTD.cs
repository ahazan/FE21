using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la esctructura del reporte diario
    /// </summary>
    public class RPTD
    {
        #region CARATULA

        private string nombre = "";

        [XmlIgnore]
        public string Nombre
        {
            get
            {
                string fechaNombre = this.FechaResumen.Substring(0, 4) + "" + this.FechaResumen.Substring(5, 2) + "" + this.FechaResumen.Substring(8, 2);

                nombre = @"Rep_" + this.RucEmisor + "_" + fechaNombre + "_" + SecuenciaEnvio;

                return nombre;
            }
            set { nombre = value; }
        }

        private string nombrePrev = "";

        [XmlIgnore]
        public string NombrePrev
        {
            get
            {
                string fechaNombre = this.FechaResumen.Substring(0, 4) + "" + this.FechaResumen.Substring(5, 2) + "" + this.FechaResumen.Substring(8, 2);

                nombrePrev = @"Rep_" + this.RucEmisor + "_" + fechaNombre + "_" + SecuenciaEnvio;

                return nombrePrev;
            }
            set { nombrePrev = value; }
        }

        private string version = "";

        /// <summary>
        /// Versión del formato utilizado
        /// <para>Tipo: ALFA 3</para>
        /// </summary>
        public string Version
        {
            get 
            {
                if (version.Length > 3)
                {
                    return version.Substring(0, 3);
                }
                return version; 
            }
            set { version = value; }
        }

        private long rucEmisor;

        /// <summary>
        /// Corresponde al RUC del emisor electrónico.
        /// <para>Tipo: NUM 12</para>
        /// </summary>
        public long RucEmisor
        {
            get 
            {
                if ( rucEmisor.ToString().Length > 12)
                    return long.Parse(rucEmisor.ToString().Substring(0, 12));
                return rucEmisor; 
            }
            set { rucEmisor = value; }
        }

        private string fechaResumen;

        /// <summary>
        /// Fecha a la cual corresponde el Resumen. Es la fecha que se genera el CFE, o sea la indicada en la Zona H: “Fecha y hora de firma electrónica avanzada del comprobante” del Formato de CFE.
        /// <para>Tipo: CHAR 10</para>
        /// <para>Formato: AAAA-MM-DD</para>
        /// </summary>
        public string FechaResumen
        {
            get 
            {
                //Toma la fecha y la hora actual
                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                //Convierte la fecha y la hora al formato correcto
                fechaResumen = String.Format("{0:s}", dt);

                if (fechaResumen.Length > 10)
                    return fechaResumen.Substring(0, 10);
                return fechaResumen; }
            set { fechaResumen = value; }
        }

        private string idEmisor = "";

        /// <summary>
        /// Número asignado por el emisor al envío
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public string IdEmisor
        {
            get 
            {
                if (idEmisor.Length > 10)
                    return idEmisor.Substring(0, 10);
                return idEmisor; 
            }
            set { idEmisor = value; }
        }

        private int secuenciaEnvio;

        /// <summary>
        /// El primer envío del día trae el numero1. Si se desea corregir información se debe enviar el archivo completo con secuencia de envío =secuencia anterior +1.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        public int SecuenciaEnvio
        {
            get 
            {
                if (secuenciaEnvio.ToString().Length > 2)
                    return int.Parse(secuenciaEnvio.ToString().Substring(0, 2));
                return secuenciaEnvio; 
            }
            set { secuenciaEnvio = value; }
        }

        private string fechaHoraFirma;

        /// <summary>
        /// Fecha y hora de la firma electrónica avanzada del CFE.
        /// <para>Tipo: ALFA 19</para>
        /// <para>Formato: AAAA-MM-DDTHH:MM:SS</para>
        /// </summary>
        public string FechaHoraFirma
        {
            get
            {
                //Toma la fecha y la hora actual
                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                //Convierte la fecha y la hora al formato correcto
                fechaHoraFirma = String.Format("{0:s}", dt);

                return fechaHoraFirma;
            }
            set { fechaHoraFirma = value; }
        }

        private int cantComprobantes = 0;

        /// <summary>
        /// Cantidad total de todos los tipos de CFE utilizados (emitidos más anulados) y de todos los tipos de CFC emitidos Si C7=0, puede omitirse la Zona B Resumen.
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public int CantComprobantes
        {
            get 
            {
                return cantComprobantes;
            }
            set { cantComprobantes = value; }
        }

        public enum ESEstadoRPTD
        {
            pendiente = 1,
            aprobado = 2,
            rechazado = 3
        }

        private ESEstadoRPTD estado;

        private ESEstadoRPTD Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        #endregion CARATULA

        #region RESUMEN

        private List<RPTDResumen> rptdResumen = new List<RPTDResumen>();

        /// <summary>
        /// Lista de montos para cada certificado
        /// </summary>
        public List<RPTDResumen> RptdResumen
        {
            get { return rptdResumen; }
            set { rptdResumen = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de RPTDResumen. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarMontos(RPTDResumen rptdResumen)
        /// </summary>
        /// <returns></returns>
        public RPTDResumen NuevoResumen()
        {
            return new RPTDResumen();
        }

        /// <summary>
        /// Agrega un nuevo listado de montos a la lista de resumen.
        /// </summary>
        /// <param name="rptdResumen"></param>
        public void AgregarResumen(RPTDResumen rptdResumen)
        {
            if (RptdResumen.Count < 200)
            {
                RptdResumen.Add(rptdResumen);
            }
        }

        #endregion RESUMEN
    }
}

