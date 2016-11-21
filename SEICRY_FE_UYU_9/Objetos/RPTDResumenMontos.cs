using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de montos por fecha y sucursal para cada tipo de certificado agregado al reporte diario
    /// </summary>
    public class RPTDResumenMontos
    {
        private string fechaComprobante;

        /// <summary>
        /// Fecha de Comprobante: Campo A-C5 de Formato de los CFE
        /// <para>Tipo: CHAR10</para>
        /// </summary>
        public string FechaComprobante
        {
            get
            {
                //Toma la fecha y la hora actual
                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                //Convierte la fecha y la hora al formato correcto
                fechaComprobante = String.Format("{0:s}", dt);

                if (fechaComprobante.Length > 10)
                    return fechaComprobante.Substring(0, 10);
                return fechaComprobante;
            }
            set { fechaComprobante = value; }
        }

        private int codigoCasaPrincipal = 0;

        /// <summary>
        /// Código numérico entregado por la DGI, que identifica a la casa principal o a la sucursal desde donde se realiza la operación. Campo A-C47 de Formato de los CFE
        /// <para>Tipo: NUM 4</para>
        /// </summary>
        public int CodigoCasaPrincipal
        {
            get
            {
                if (codigoCasaPrincipal.ToString().Length > 4)
                    return int.Parse(codigoCasaPrincipal.ToString().Substring(0, 4));
                return int.Parse(codigoCasaPrincipal.ToString());
            }
            set { codigoCasaPrincipal = value; }
        }

        private double totalMontoNoGravado;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoNoGravado
        {
            get
            {
                if (totalMontoNoGravado.ToString().Length > 17)
                    return double.Parse(totalMontoNoGravado.ToString().Substring(0, 17));
                return double.Parse(totalMontoNoGravado.ToString());
            }
            set { totalMontoNoGravado = value; }
        }

        private double totalMontoExportacionAsimilados;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoExportacionAsimilados
        {
            get
            {
                if (totalMontoExportacionAsimilados.ToString().Length > 17)
                    return double.Parse(totalMontoExportacionAsimilados.ToString().Substring(0, 17));
                return double.Parse(totalMontoExportacionAsimilados.ToString());
            }
            set { totalMontoExportacionAsimilados = value; }
        }

        private double totalMontoImpuestoPercibido;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoImpuestoPercibido
        {
            get
            {
                if (totalMontoImpuestoPercibido.ToString().Length > 17)
                    return double.Parse(totalMontoImpuestoPercibido.ToString().Substring(0, 17));
                return double.Parse(totalMontoImpuestoPercibido.ToString());
            }
            set { totalMontoImpuestoPercibido = value; }
        }

        private double totalMontoIVASuspenso;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoIVASuspenso
        {
            get
            {
                if (totalMontoIVASuspenso.ToString().Length > 17)
                    return double.Parse(totalMontoIVASuspenso.ToString().Substring(0, 17));
                return double.Parse(totalMontoIVASuspenso.ToString());
            }
            set { totalMontoIVASuspenso = value; }
        }

        private double totalMontoIVATasaMinima;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoIVATasaMinima
        {
            get
            {
                if (totalMontoIVATasaMinima.ToString().Length > 17)
                    return double.Parse(totalMontoIVATasaMinima.ToString().Substring(0, 17));
                return double.Parse(totalMontoIVATasaMinima.ToString());
            }
            set { totalMontoIVATasaMinima = value; }
        }

        private double totalMontoIVATasaBasica;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoIVATasaBasica
        {
            get
            {
                if (totalMontoIVATasaBasica.ToString().Length > 17)
                    return double.Parse(totalMontoIVATasaBasica.ToString().Substring(0, 17));
                return double.Parse(totalMontoIVATasaBasica.ToString());
            }
            set { totalMontoIVATasaBasica = value; }
        }

        private double totalMontoIVAOtraTasa;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoIVAOtraTasa
        {
            get
            {
                if (totalMontoIVAOtraTasa.ToString().Length > 17)
                    return double.Parse(totalMontoIVAOtraTasa.ToString().Substring(0, 17));
                return double.Parse(totalMontoIVAOtraTasa.ToString());
            }
            set { totalMontoIVAOtraTasa = value; }
        }

        private double totalIVATasaMinima;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal. Calcula el valor automaticamente ( TotalMontoNetoIVATasaMinima * TasaMinimaIVA ) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-121.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVATasaMinima
        {
            get
            {
                totalIVATasaMinima = TotalMontoIVATasaMinima * TasaMinimaIVA;

                if (totalIVATasaMinima.ToString().Length > 17)
                    return double.Parse(totalIVATasaMinima.ToString().Substring(0, 17));
                return double.Parse(totalIVATasaMinima.ToString());
            }
            set { totalIVATasaMinima = value; }
        }

        private double totalIVATasaBasica;

        /// <summary>
        /// Importe del concepto en el CFE. Calcula el valor automaticamente ( TotalMontoNetoIVATasaBasica * TasaBasicaIVA ) segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion B-122.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVATasaBasica
        {
            get
            {
                totalIVATasaBasica = TotalMontoIVATasaBasica * TasaBasicaIVA;

                if (totalIVATasaBasica.ToString().Length > 17)
                    return double.Parse(totalIVATasaBasica.ToString().Substring(0, 17));
                return double.Parse(totalIVATasaBasica.ToString());
            }
            set { totalIVATasaBasica = value; }
        }

        private double totalIVAOtraTasa;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalIVAOtraTasa
        {
            get
            {
                if (totalIVAOtraTasa.ToString().Length > 17)
                    return double.Parse(totalIVAOtraTasa.ToString().Substring(0, 17));
                return double.Parse(totalIVAOtraTasa.ToString());
            }
            set { totalIVAOtraTasa = value; }
        }

        private double tasaMinimaIVA;

        /// <summary>
        /// Tasa mínima vigente en la fecha del comprobante en %.
        /// <para>Tipo: NUM 6</para>
        /// </summary>
        public double TasaMinimaIVA
        {
            get
            {
                if (tasaMinimaIVA.ToString().Length > 6)
                    return double.Parse(tasaMinimaIVA.ToString().Substring(0, 6));
                return double.Parse(tasaMinimaIVA.ToString());
            }
            set { tasaMinimaIVA = value; }
        }

        private double tasaBasicaIVA;

        /// <summary>
        /// Tasa básica vigente en la fecha del comprobante en %.
        /// <para>Tipo: NUM 6</para>
        /// </summary>
        public double TasaBasicaIVA
        {
            get
            {
                if (tasaBasicaIVA.ToString().Length > 6)
                    return double.Parse(tasaBasicaIVA.ToString().Substring(0, 6));
                return double.Parse(tasaBasicaIVA.ToString());
            }
            set { tasaBasicaIVA = value; }
        }

        private double totalMontoTotal;

        /// <summary>
        /// Suma de totales
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoTotal
        {
            get
            {
                if (totalMontoTotal.ToString().Length > 17)
                    return double.Parse(totalMontoTotal.ToString().Substring(0, 17));
                return double.Parse(totalMontoTotal.ToString());

            }
            set { totalMontoTotal = value; }
        }

        private double totalMontoRetenido = 0;

        /// <summary>
        /// Importe del concepto en los comprobantes emitidos informados por fecha y sucursal.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double TotalMontoRetenido
        {
            get
            {

                if (totalMontoRetenido.ToString().Length > 17)
                    return double.Parse(totalMontoRetenido.ToString().Substring(0, 17));
                return double.Parse(totalMontoRetenido.ToString());
            }
            set { totalMontoRetenido = value; }
        }
    }

}
