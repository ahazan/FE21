using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SEICRY_FE_UYU_9.Objetos
{
    /// <summary>
    /// Representa la estructura de cada item (producto o servicio) de un CFE
    /// </summary>
    [Serializable]
    public class CFEItems
    {
        #region DETALLES DE PRODUCTOS

        private int lineNum;
        [XmlIgnore]
        public int LineNum
        {
            get { return lineNum; }
            set { lineNum = value; }
        }
        
        
        private int numeroLinea;

        /// <summary>
        /// Número del ítem
        /// <para>Tipo: NUM 3</para>
        /// </summary>
        public int NumeroLinea
        {
            get 
            { 
                if(numeroLinea.ToString().Length > 3)
                    return int.Parse( numeroLinea.ToString().Substring(0,3));
                return int.Parse(numeroLinea.ToString());
            }
            set { numeroLinea = value; }
        }

        private int indicadorFacturacion;

        public int IndicadorFacturacion
        {
            get { return indicadorFacturacion; }
            set { indicadorFacturacion = value; }
        }

        //public enum ESIndicadorFacturacion
        //{
        //    ExentoIva = 1,
        //    GravadoTasaMinima = 2,
        //    GravadoTasaBasica = 3,
        //    GravadoOtraTasa = 4,

        //    /// <summary>
        //    /// Por ejemplo docenas de trece
        //    /// </summary>
        //    EntregaGratuita = 5,
        //    ProductoServicioNoFacturable = 6,
        //    ProductoServicioNoFacturableNegativo = 7,

        //    /// <summary>
        //    /// Solo para remitos. En área de referencia se debe indicar el N° de remito que ajusta
        //    /// </summary>
        //    ItemRebajarRemitos = 8,

        //    /// <summary>
        //    /// Solo para resguardos. En área de referencia se debe indicar el N° de resguardo que anular
        //    /// </summary>
        //    ItemAnularResguardo = 9,
        //    ExportacionAsimilidas = 10,
        //    ImpuestoPercibido = 11,
        //    IVASuspenso = 12
        //}

        /// <summary>
        /// Indica si el producto o servicio es exento, o a que tasa esta gravado o si corresponde a un concepto no facturable.
        /// <para>Tipo: NUM 2</para>
        /// </summary>
        //[XmlIgnore]
        //public ESIndicadorFacturacion IndicadorFacturacion { get; set; }

        //public int IndicadorFacturacionInt
        //{
        //    get { return (int)IndicadorFacturacion; }
        //    set { IndicadorFacturacion = (ESIndicadorFacturacion)value; }
        //}

        public enum ESIndicadorAgenteResponsable
        {
            Responsable = 'R',
            NoResponsable = ' ',
        }

        /// <summary>
        /// Obligatorio para agentes/responsables, indica para cada transacción si es agente/responsable del producto que está vendiendo.
        /// <para>Tipo: ALFA 1</para>
        /// </summary>
        [XmlIgnore]
        public ESIndicadorAgenteResponsable IndicadorResponsable { get; set; }

        public char IndicadorResponsableInt
        {
            get { return (char)IndicadorResponsable; }
            set { IndicadorResponsable = (ESIndicadorAgenteResponsable)value; }
        }

        private string nombreItem = "";

        /// <summary>
        /// Nombre del producto o servicio.
        /// <para>Tipo: ALFA 80</para>
        /// </summary>
        public string NombreItem
        {
            get 
            { 
                if(nombreItem.Length > 80)
                    return nombreItem.Substring(0,80);
                return nombreItem;
            }
            set { nombreItem = value; }
        }

        private string descripcionItem = "";

        /// <summary>
        /// Descripción Adicional del producto o servicio. Se utiliza para pack, servicios con detalle.
        /// <para>Tipo: ALFA 1000</para>
        /// </summary>
        public string DescripcionItem
        {
            get 
            { 
                if(descripcionItem.Length > 1000)
                    return descripcionItem.Substring(0,1000);
                return descripcionItem;
            }
            set { descripcionItem = value; }
        }

        private double cantidadItem = 0;

        /// <summary>
        /// Cantidad del ítem. 
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double CantidadItem
        {
            get 
            { 
                if(cantidadItem.ToString().Length > 17)
                    return double.Parse( cantidadItem.ToString().Substring(0,17));
                return double.Parse(cantidadItem.ToString());
            }
            set { cantidadItem = value; }
        }

        private string unidadMedida = "";

        /// <summary>
        /// Indica la unidad de medida en que está expresada la cantidad. En caso que no corresponda, poner N/A.
        /// <para>Tipo: ALFA 4</para>
        /// </summary>
        public string UnidadMedida
        {
            get 
            {
                //if (unidadMedida.Length > 4)
                //{
                //    return "N/A"; //.Substring(0,4);
                //}
                //else
                //{
                    return unidadMedida;
                //}
            }
            set { unidadMedida = value; }
        }

        private string unidadMedidaPDF;

        [XmlIgnore]
        public string UnidadMedidaPDF
        {
            get { return unidadMedidaPDF; }
            set { unidadMedidaPDF = value; }
        }



        private double precioUnitarioItem;

        /// <summary>
        /// Indica la unidad de medida en que está expresada la cantidad.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double PrecioUnitarioItem
        {
            get 
            {
                if(precioUnitarioItem.ToString().Length > 17)
                    return Math.Round(double.Parse( precioUnitarioItem.ToString().Substring(0,17)), 2);
                return Math.Round(double.Parse(precioUnitarioItem.ToString()), 2);
            }
            set { precioUnitarioItem = value; }
        }


        private double precioUnitarioItemFC;

        /// <summary>
        /// Indica la unidad de medida en que está expresada la cantidad.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double PrecioUnitarioItemFC
        {
            get
            {
                if (precioUnitarioItemFC.ToString().Length > 17)
                    return Math.Round(double.Parse(precioUnitarioItemFC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(precioUnitarioItemFC.ToString()), 2);
            }
            set { precioUnitarioItemFC = value; }
        }

        private double precioUnitarioItemPDF;

        /// <summary>
        /// Indica la unidad de medida en que esta expresada la cantidad
        /// </summary>
        public double PrecioUnitarioItemPDF
        {
            get
            {
                if (precioUnitarioItemPDF.ToString().Length > 17)
                {
                    return Math.Round(double.Parse(precioUnitarioItemPDF.ToString().Substring(0,17)), 2);
                }
                else
                {
                    return Math.Round(double.Parse(precioUnitarioItemPDF.ToString()), 2);
                }
            }

            set
            {
                precioUnitarioItemPDF = value;
            }
        }        


       

        private double precioUnitarioItemPDF_FC;

        /// <summary>
        /// Indica la unidad de medida en que esta expresada la cantidad
        /// </summary>
        public double PrecioUnitarioItemPDF_FC
        {
            get
            {
                if (precioUnitarioItemPDF_FC.ToString().Length > 17)
                {
                    return Math.Round(double.Parse(precioUnitarioItemPDF_FC.ToString().Substring(0, 17)), 2);
                }
                else
                {
                    return Math.Round(double.Parse(precioUnitarioItemPDF_FC.ToString()), 2);
                }
            }

            set
            {
                precioUnitarioItemPDF_FC = value;
            }
        }
           

        private double porcentajeDescuentoItem;

        /// <summary>
        /// Descuento por ítem en %.
        /// <para>Tipo: NUM6</para>
        /// </summary>
        public double PorcentajeDescuentoItem
        {
            get 
            { 
                if(porcentajeDescuentoItem.ToString().Length > 6)
                    return double.Parse( porcentajeDescuentoItem.ToString().Substring(0,6));
                return double.Parse(porcentajeDescuentoItem.ToString());
            }
            set { porcentajeDescuentoItem = value; }
        }

        private double montoDescuentoItem;

        /// <summary>
        /// Correspondiente al anterior. Totaliza todos los descuentos otorgados al ítem.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoDescuentoItem
        {
            get 
            { 
                if(montoDescuentoItem.ToString().Length > 17)
                    return double.Parse(montoDescuentoItem.ToString().Substring(0, 17));
                return double.Parse(montoDescuentoItem.ToString());
            }
            set { montoDescuentoItem = value; }
        }


        private double montoDescuentoItemFC;

        /// <summary>
        /// Correspondiente al anterior. Totaliza todos los descuentos otorgados al ítem.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoDescuentoItemFC
        {
            get
            {
                if (montoDescuentoItemFC.ToString().Length > 17)
                    return double.Parse(montoDescuentoItemFC.ToString().Substring(0, 17));
                return double.Parse(montoDescuentoItemFC.ToString());
            }
            set { montoDescuentoItemFC = value; }
        }

        private double porcentajeRecargoItem;

        /// <summary>
        /// Recargo en % por ítem.
        /// <para>Tipo: NUM6</para>
        /// </summary>
        public double PorcentajeRecargoItem
        {
            get 
            { 
                if(porcentajeRecargoItem.ToString().Length > 6)
                    return double.Parse( porcentajeRecargoItem.ToString().Substring(0,6));
                return double.Parse(porcentajeRecargoItem.ToString());
            }
            set { porcentajeRecargoItem = value; }
        }

        private double montoRecargoItem;

        /// <summary>
        /// Correspondiente al PorcentajeRecargo. Totaliza todos los recargos otorgados al ítem.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoRecargoItem
        {
            get 
            { 
                if(montoRecargoItem.ToString().Length > 17)
                    return double.Parse( montoRecargoItem.ToString().Substring(0,17));
                return double.Parse(montoRecargoItem.ToString());
            }
            set { montoRecargoItem = value; }
        }


        private double montoRecargoItemFC;

        /// <summary>
        /// Correspondiente al PorcentajeRecargo. Totaliza todos los recargos otorgados al ítem.
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoRecargoItemFC
        {
            get
            {
                if (montoRecargoItemFC.ToString().Length > 17)
                    return double.Parse(montoRecargoItemFC.ToString().Substring(0, 17));
                return double.Parse(montoRecargoItemFC.ToString());
            }
            set { montoRecargoItemFC = value; }
        }

        private double montoItemPDF = 0;

        public double MontoItemPDF
        {
            get
            {
                //if (this.IndicadorFacturacion == ESIndicadorFacturacion.EntregaGratuita)
                //{
                //    montoItem = 0;
                //}
                //else
                //{
                montoItemPDF = (CantidadItem * precioUnitarioItemPDF) - MontoDescuentoItem + MontoRecargoItem;
                //}

                if (montoItemPDF.ToString().Length > 17)
                    return Math.Round(double.Parse(montoItemPDF.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(montoItemPDF.ToString()), 2);
            }
            set { montoItem = value; }
        }


        private double montoItemPDF_FC = 0;

        public double MontoItemPDF_FC
        {
            get
            {
                //if (this.IndicadorFacturacion == ESIndicadorFacturacion.EntregaGratuita)
                //{
                //    montoItem = 0;
                //}
                //else
                //{
                montoItemPDF_FC = (precioUnitarioItemPDF_FC * cantidadItem) - MontoDescuentoItemFC + MontoRecargoItemFC;
                //}

                if (montoItemPDF_FC.ToString().Length > 17)
                    return Math.Round(double.Parse(montoItemPDF_FC.ToString().Substring(0, 17)), 2);
                return Math.Round(double.Parse(montoItemPDF_FC.ToString()), 2);
            }
            set { montoItemPDF_FC = value; }
        }


        private double articulosXUnidad = 0;

        public double ArticulosXUnidad
        {
            get { return articulosXUnidad; }
            set { articulosXUnidad = value; }
        }



        private double montoItem = 0;

        /// <summary>
        /// Valor por línea de detalle. Calcula el valor automaticamente segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion B-24
        /// <para>Tipo: NUM 17</para>
        /// </summary>
        public double MontoItem
        {
            get 
            {
                //if (this.IndicadorFacturacion == ESIndicadorFacturacion.EntregaGratuita)
                //{
                //    montoItem = 0;
                //}
                //else
                //{
                    montoItem = (CantidadItem * PrecioUnitarioItem) - MontoDescuentoItem + MontoRecargoItem;
                //}

                if(montoItem.ToString().Length > 17)
                    return Math.Round(double.Parse( montoItem.ToString().Substring(0,17)),2);
                return Math.Round(double.Parse(montoItem.ToString()), 2);
            }
            set { montoItem = value; }
        }

        #endregion DETALLES DE PRODUCTOS

        #region CODIGOS DE ITEM

        private List<CFEItemsCodigos> itemsCodigos = new List<CFEItemsCodigos>();

        /// <summary>
        /// Lista de codigos del item
        /// </summary>
        public List<CFEItemsCodigos> ItemCodigos
        {
            get { return itemsCodigos; }
            set { itemsCodigos = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de DFEItemsCodigos. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarItemCodigos(CFEItemsCodigos cfeItemCodigos)
        /// </summary>
        /// <returns></returns>
        public CFEItemsCodigos NuevoItemCodigos()
        {
            return new CFEItemsCodigos();
        }

        /// <summary>
        /// Agrega un nuevo codigo de item a la lista de codigos de items.
        /// </summary>
        /// <param name="cfeItemCodigos"></param>
        public void AgregarItemCodigos(CFEItemsCodigos cfeItemCodigos)
        {
            if (ItemCodigos.Count < 5)
            {
                ItemCodigos.Add(cfeItemCodigos);
            }
        }

        #endregion CODIGOS DE ITEM

        #region DISTRIBUCION DE DESCUENTO

        private List<CFEItemsDistDescuento> itemsDistDescuentos = new List<CFEItemsDistDescuento>();

        /// <summary>
        /// Lista de distribucion de descuentos
        /// </summary>
        public List<CFEItemsDistDescuento> ItemsDistDescuentos
        {
            get { return itemsDistDescuentos; }
            set { itemsDistDescuentos = value; }
        }

        private string tipoImpuesto;

        public string TipoImpuesto
        {
            get { return tipoImpuesto; }
            set { tipoImpuesto = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFEItemsDistDescuento. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarItemDistDescuento(CFEItemsDistDescuento cfeItemDistDescuento)
        /// </summary>
        /// <returns></returns>
        public CFEItemsDistDescuento NuevoItemDistDescuento()
        {
            return new CFEItemsDistDescuento();
        }

        /// <summary>
        /// Agrega un nuevo objeto distribucion de descuento a la lista.
        /// </summary>
        /// <param name="cfeItemDistDescuento"></param>
        public void AgregarItemDistDescuento(CFEItemsDistDescuento cfeItemDistDescuento)
        {
            if (ItemsDistDescuentos.Count < 5)
            {
                ItemsDistDescuentos.Add(cfeItemDistDescuento);
            }
        }

        #endregion DISTRIBUCION DE DESCUENTO

        #region DISTRIBUCION DE RECARGO

        private List<CFEItemsDistRecargo> itemsDistRecargo = new List<CFEItemsDistRecargo>();

        /// <summary>
        /// Lista de distribucion de regargos
        /// </summary>
        public List<CFEItemsDistRecargo> ItemsDistRecargos
        {
            get { return itemsDistRecargo; }
            set { itemsDistRecargo = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFEItemsDistRecargo. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarItemDistRecargo(CFEItemsDistRecargo cfeItemDistRecargo)
        /// </summary>
        /// <returns></returns>
        public CFEItemsDistRecargo NuevoItemDistRecargo()
        {
            return new CFEItemsDistRecargo();
        }

        /// <summary>
        /// Agrega un nuevo objeto distribucion de descuento a la lista.
        /// </summary>
        /// <param name="cfeItemDistRecargo"></param>
        public void AgregarItemDistRecargo(CFEItemsDistRecargo cfeItemDistRecargo)
        {
            if (ItemsDistRecargos.Count < 5)
            {
                ItemsDistRecargos.Add(cfeItemDistRecargo);
            }
        }

        #endregion DISTRIBUCION DE RECARGO

        #region RETENCION/RECEPCION

        private List<CFEItemsRetencPercep> itemsRecepPercep = new List<CFEItemsRetencPercep>();

        /// <summary>
        /// Lista de recepcion/percepcion
        /// </summary>
        public List<CFEItemsRetencPercep> ItemsRecepPercep
        {
            get { return itemsRecepPercep; }
            set { itemsRecepPercep = value; }
        }

        /// <summary>
        /// Retorna una nueva instancia de CFEItemsRecepPercep. A esta instancia se asignan los valores en los campos respectivos y se agrega a la lista
        /// mediante el metodo AgregarItemRecepPercep(CFEItemsRecepPercep cfeItemsRecepPercep)
        /// </summary>
        /// <returns></returns>
        public CFEItemsRetencPercep NuevoItemsRetencPercep()
        {
            return new CFEItemsRetencPercep();
        }

        /// <summary>
        /// Agrega un nuevo objeto recepcion/percepcion a la lista.
        /// </summary>
        /// <param name="cfeItemDistRecargo"></param>
        public void AgregarItemRetencPercep(CFEItemsRetencPercep cfeItemsRecepPercep)
        {
            if (ItemsRecepPercep.Count < 5)
            {
                ItemsRecepPercep.Add(cfeItemsRecepPercep);
            }
        }

        #endregion RETENCION/RECEPCION

    }
}
