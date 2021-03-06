﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    public class DatosPDF
    {
        public DatosPDF()
        {
            this.Titular = "";
            this.kilosFactura = "";
            this.DescuentoExtranjero = 0;
            this.DocNum = "";
            this.ImpuestoCalculado = "";
            this.Ciudad = "";
            this.FormaPago = "";
            this.Estado = "";
            this.DocumentoBase = "";
            this.DescuentoGeneral = 0;
            this.montoTotalPagar = "";
            this.NombreVendedor = "";
            this.NombreExtranjero = "";
            this.SocioNegocio = "";
            this.PorcentajeDescuento = 0;
            this.Telefono = "";
            this.Telefono2 = "";
            this.Comentarios = "";
            this.CodigoDireccion = "";
            this.NumeroOrden = "";
            this.DireccionEntrega = "";
        }

        string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        string direccionEntrega;

        public string DireccionEntrega
        {
            get { return direccionEntrega; }
            set { direccionEntrega = value; }
        }

        string titular;

        public string Titular
        {
            get { return titular; }
            set { titular = value; }
        }

        string kilosFactura;

        public string KilosFactura
        {
            get { return kilosFactura; }
            set { kilosFactura = value; }
        }

        double descuentoExtranjero;

        public double DescuentoExtranjero
        {
            get { return descuentoExtranjero; }
            set { descuentoExtranjero = value; }
        }

        private string docNum;

        public string DocNum
        {
            get { return docNum; }
            set { docNum = value; }
        }

        private string impuestoCalculado;

        public string ImpuestoCalculado
        {
            get { return impuestoCalculado; }
            set { impuestoCalculado = value; }
        }

        private double descuentoGeneral;

        public double DescuentoGeneral
        {
            get { return descuentoGeneral; }
            set { descuentoGeneral = value; }
        }

        string montoTotalPagar;

        public string MontoTotalPagar
        {
            get { return montoTotalPagar; }
            set { montoTotalPagar = value; }
        }

        string montoTotalPagarPesos;

        public string MontoTotalPagarPesos
        {
            get { return montoTotalPagarPesos; }
            set { montoTotalPagarPesos = value; }
        }

        string nombreVendedor;

        public string NombreVendedor
        {
            get { return nombreVendedor; }
            set { nombreVendedor = value; }
        }

        private string nombreExtranjero;

        public string NombreExtranjero
        {
            get { return nombreExtranjero; }
            set { nombreExtranjero = value; }
        }

        private string socioNegocio;

        public string SocioNegocio
        {
            get { return socioNegocio; }
            set { socioNegocio = value; }
        }

        private double porcentajeDescuento;

        public double PorcentajeDescuento
        {
            get { return porcentajeDescuento; }
            set { porcentajeDescuento = value; }
        }

        private string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string numeroOrden;

        public string NumeroOrden
        {
            get { return numeroOrden; }
            set { numeroOrden = value; }
        }

        private string telefono2;

        public string Telefono2
        {
            get { return telefono2; }
            set { telefono2 = value; }
        }

        private string formaPago;

        public string FormaPago
        {
            get { return formaPago; }
            set { formaPago = value; }
        }

        private string comentarios;

        public string Comentarios
        {
            get { return comentarios; }
            set { comentarios = value; }
        }

        private string codigoDireccion;

        public string CodigoDireccion
        {
            get { return codigoDireccion; }
            set { codigoDireccion = value; }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string ciudad;

        public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }

        private string documentoBase;

        public string DocumentoBase
        {
            get { return documentoBase; }
            set { documentoBase = value; }
        }

        private string redondeo;

        public string Redondeo
        {
            get { return redondeo; }
            set { redondeo = value; }
        }
    }
}
