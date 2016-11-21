using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Esta clase representa un objeto sobre. 
    /// </summary>
    public class Sobre
    {
        public Sobre()
        {
        }

        public Sobre(CFE cfe)
        {
            RucReceptor = cfe.NumDocReceptor;
            RUCEmisor = cfe.RucEmisor;

            Nombre = cfe.TipoCFEInt +"" + cfe.SerieComprobante + "" + cfe.NumeroComprobante;
            Idemisor = generarIdReceptor();
        }

        #region PROPIEDADES

        private string nombre="";

        [XmlIgnore]
        public string Nombre
        {
            get 
            {
                return nombre; 
            }
            set { nombre = value; }
        }

        private string nombrePrev="";

        [XmlIgnore]
        public string NombrePrev
        {
            get
            {
                string fechaNombre = this.FechaCreacionSobre.Substring(0, 4) + "" + this.FechaCreacionSobre.Substring(5, 2) + "" + this.FechaCreacionSobre.Substring(8, 2);

                nombrePrev = Nombre + "_prev";
                
                return nombrePrev;
            }
            set { nombrePrev = value; }
        }

        private string version = "1.0";

        /// <summary>
        /// Versión del formato utilizado.
        /// <para>Tipo ALFA 3</para>
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

        private string rucReceptor = "0";

        /// <summary>
        /// Corresponde al RUC receptor de los CFE o de DGI.
        /// <para>Tipo:  NUM 12</para>
        /// </summary>
        public string RucReceptor
        {
            get 
            {
                if (rucReceptor.ToString().Length > 12)
                {
                    return (rucReceptor.ToString().Substring(0, 12));
                }
                return rucReceptor; 
            }
            set { rucReceptor = value; }
        }

        private long rucEmisor = 0;

        /// <summary>
        /// Corresponde al RUC que emite los comprobantes
        /// <para>Tipo: NUM 12</para>
        /// </summary>
        public long RUCEmisor
        {
            get
            {
                if (rucEmisor.ToString().Length > 12)
                {
                    return long.Parse(rucEmisor.ToString().Substring(0,12));
                }
                return rucEmisor;
            }
            set { rucEmisor = value; }
        }

        private string idemisor;

        /// <summary>
        /// Número asignado por el emisor al envío
        /// <para>Tipo: NUM 10</para>
        /// </summary>
        public string Idemisor
        {
            get 
            {
                if (idemisor.Length > 10)
                {
                    return idemisor.Substring(0,10);
                }
                return idemisor; 
            }
            set { idemisor = value; }
        }

        private int cantCFE = 0;

        /// <summary>
        /// Cantidad de comprobantes en el sobre
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        public int CantCFE
        {
            get 
            {
                cantCFE = ListaCertificados.Count;

                if (cantCFE.ToString().Length > 3)
                {
                    return int.Parse(cantCFE.ToString().Substring(0,3));
                }
                return cantCFE; 
            }
            set { cantCFE = value; }
        }

        private string fechaCreacionSobre;

        /// <summary>
        /// Fecha en la que se crea el sobre
        /// <para>Tipo: ALFA 19</para>
        /// <para>Formato: AAAA-MM-DD HH:MM:SS</para>
        /// </summary>
        public string FechaCreacionSobre
        {
            get
            {
                DateTime dt = DateTime.Now;
                fechaCreacionSobre = dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

                return fechaCreacionSobre;
            }
            set { fechaCreacionSobre = value; }
        }

        private string x509Certificate;

        /// <summary>
        /// Firma
        /// </summary>
        public string X509Certificate
        {
            get { return x509Certificate; }
            set { x509Certificate = value; }
        }

        private List<CFE> listaCertificados = new List<CFE>();

        /// <summary>
        /// Contiene los certificados firmados
        /// </summary>
        [XmlIgnore]
        public List<CFE> ListaCertificados
        {
            get { return listaCertificados; }
            set { listaCertificados = value; }
        }


        #endregion

        #region FUNCIONES

        public void ObtenerCertificadosCreados(List<CFE> listaCertificadosCreados)
        {
            foreach (CFE cfe in listaCertificadosCreados)
            {
                ListaCertificados.Add(cfe);
            }
        }

        /// <summary>
        /// Generar consecutivo
        /// </summary>
        /// <returns></returns>
        public string generarIdReceptor()
        {
            string resultado = "";

            ManteUdoConseIdEmisor manteConseIdEmisor = new ManteUdoConseIdEmisor();
            //Se obtiene consecutivo anterior en caso de que exista
            string consecutivo = manteConseIdEmisor.obtenerConsecutivoAnterior();

            //Caso primer consecutivo
            if (consecutivo.Equals(""))
            {
                resultado = "0000000001";
                //Se inserta el resultado
                manteConseIdEmisor.Almacenar(resultado);
            }
            else
            {
                try
                {
                    double consec = Convert.ToDouble(consecutivo);
                    //Se incrementa el numero de consecutivo
                    consec += 1;
                    resultado = agregarCeros(consec, 10);
                    //Se inserta el resultado
                    manteConseIdEmisor.Almacenar(resultado);
                }
                catch (Exception)
                {
                }
            }

            return resultado;
        }

        /// <summary>
        /// Metodo para agregar ceros a la izquierda de un string
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cantCeros"></param>
        /// <returns></returns>
        private string agregarCeros(double num, int cantDigitos)
        {
            string resultado = "", temp = "";
            int j = 0, cant = 0;

            try
            {
                temp = Convert.ToString(num);
                cant = temp.Length;

                while (j < (cantDigitos - cant))
                {
                    temp = "0" + temp;
                    j++;
                }

                resultado = temp;
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        #endregion
    }
}
