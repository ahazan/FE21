using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SEICRY_FE_UYU_9.Objetos.ISO3166;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Certificados.Validaciones
{
    public static class Validacion
    {
        #region CAMPOS OBLIGATORIOS

        /// <summary>
        /// Campos obligatorios
        /// </summary>
        /// <param name="cfe"></param>
        /// <returns></returns>
        public static bool CamposObligatorios(ref CFE cfe) 
        {
            bool salida = true;

            //Valida Version
            if (cfe.Version == "")
            {
                cfe.CampoFaltante = "Version";
                salida = false;
            }

            //Valida Tipo CFE
            if (cfe.TipoCFE == 0)
            {
                cfe.CampoFaltante = "TipoCFE";
                salida = false;
            }

            //Valida Serie del Comprobante
            if (cfe.SerieComprobante == "")
            {
                cfe.CampoFaltante = "SerieComprobante";
                salida = false;
            }

            //Valida Número de Comprobante
            if (cfe.NumeroComprobante == 0)
            {
                cfe.CampoFaltante = "NumeroComprobante";
                salida = false;
            }

            //Valida Fecha del Comprobante
            if (cfe.FechaComprobante.Equals(""))
            {
                cfe.CampoFaltante = "FechaComprobante";
                salida = false;
            }

            //Valida RUC Emisor
            if (cfe.RucEmisor == 0)
            {
                cfe.CampoFaltante = "RucEmisor";
                salida = false;
            }

            //Valida Código Casa Principal /Sucursal
            if (cfe.CodigoCasaPrincipalEmisor.Equals("0"))
            {
                cfe.CampoFaltante = "CodigoCasaPrincipalEmisor";
                salida = false;
            }

            //Valida Tipo Documento Receptor
            if (cfe.TipoDocumentoReceptor == 0)
            {
                cfe.CampoFaltante = "TipoDocumentoReceptor";
                salida = false;
            }

            //Valida Código País Receptor
            if (cfe.CodigoPaisReceptor == "")
            {
                cfe.CampoFaltante = "CodigoPaisReceptor";
                salida = false;
            }

            //Valida No Documento Receptor 
            if (cfe.NumDocReceptor == "")
            {
                cfe.CampoFaltante = "NumDocReceptor";
                salida = false;
            }

            //Valida Tipo moneda transacción
            if (cfe.TipoModena == "")
            {
                cfe.CampoFaltante = "TipoModena";
                salida = false;
            }

            //Valida Tipo de Cambio
            if (cfe.TipoCambio == 0)
            {
                cfe.CampoFaltante = "TipoCambio";
                salida = false;
            }

            //Valida Total IVA – Tasa Mínima
            if (cfe.TotalIVATasaMinima == 0)
            {
                cfe.CampoFaltante = "TotalIVATasaMinima";
                salida = false;
            }

            //Valida Total IVA – Tasa Básica
            if (cfe.TotalIVATasaBasica == 0)
            {
                cfe.CampoFaltante = "TotalIVATasaBasica";
                salida = false;
            }

            //Valida Total Monto Total
            if (cfe.TotalMontoTotal == 0)
            {
                cfe.CampoFaltante = "TotalMontoTotal";
                salida = false;
            }

            //Valida Total Monto Retenido/Percibido
            if (cfe.TotalMontoRetenidoPercibido == 0)
            {
                cfe.CampoFaltante = "TotalMontoRetenidoPercibido";
                salida = false;
            }

            //Valida Líneas
            if (cfe.Lineas == 0)
            {
                cfe.CampoFaltante = "Lineas";
                salida = false;
            }

            //Valida Código de Retención/ Percepción
            foreach (CFERetencPercep retencPercep in cfe.RetencionPercepcion)
	        {
                if (retencPercep.CodigoRetencPercep == "")
                {
                    cfe.CampoFaltante = "RetencionPercepcion.CodigoRetencPercep";
                    salida = false;
                }
	        }

            //Valida Valor de la retención/ percepción
            foreach (CFERetencPercep retencPercep in cfe.RetencionPercepcion)
            {
                if (retencPercep.ValorRetencPercep == 0)
                {
                    cfe.CampoFaltante = "RetencionPercepcion.ValorRetencPercep";
                    salida = false;
                }
            }

            return salida;
        }

        #endregion CAMPOS OBLIGATORIOS

        #region VALIDACIONES

        /// <summary>
        /// Realiza todas las validaciones indicadas en la documentacion oficial Formato_CFE_v10
        /// </summary>
        /// <param name="cfe"></param>
        /// <returns></returns>
        public static bool ValidarCFE(ref CFE cfe)
        {
            //Valida la Fecha del Comprobante
            if (!FechaComprobante(cfe.FechaComprobante.ToString()))
            {
                cfe.CampoErroneo = "FechaComprobante";
                return false;
            }

            //Valida el Perdiodo Desde
            if (!PerdiodoDesde(cfe.PeriodoDesde.ToString()))
            {
                cfe.CampoErroneo = "PeriodoDesde";
                return false;
            }

            //Valida PeriodoHasta
            if (!PeriodoHasta(cfe.PeriodoDesde.ToString(), cfe.PeriodoHasta.ToString()))
            {
                cfe.CampoErroneo = "PeriodoHasta";
                return false;
            }

            //Valida FechaVencimiento
            if (!FechaVencimiento(cfe.FechaVencimiento.ToString()))
            {
                cfe.CampoErroneo = "FechaVencimiento";
                return false;
            }

            //Valida FechaComprobanteReferencia
            foreach (CFEInfoReferencia infoReferencia in cfe.InfoReferencia)
            {
                if (!FechaCFEReferencia(infoReferencia.FechaComprobanteReferencia.ToString()))
                {
                    cfe.CampoErroneo = "InfoReferencia.FechaComprobanteReferencia";
                    return false;
                }
            }

            //Valida FechaVencimientoCAE
            if (!FechaVencimientoCAE(cfe.FechaVencimientoCAE.ToString()))
            {
                cfe.CampoErroneo = "FechaVencimientoCAE";
                return false;
            }

            //Valida TipoDocumentoReceptor
            if (!TipoDocumentoReceptor(cfe.TipoCFE, cfe.TipoDocumentoReceptor, cfe.CodigoPaisReceptor, cfe.NumDocReceptorUruguayo, cfe.NumDocReceptorExtrangero,
                cfe.TotalMontoNoGravado, cfe.TotalMontoExportacionAsimilados,cfe.TotalMontoImpuestoPercibido, cfe.TotalMontoIVASuspenso, cfe.TotalMontoNetoIVATasaMinima,
                cfe.TotalMontoNetoIVATasaBasica, cfe.TotalMontoNetoIVAOtraTasa))

            {
                cfe.CampoErroneo = "TipoDocumentoReceptor";
                return false;
            }

            //Valida CodigoPaisReceptor
            if (!CodigoPaisReceptor(cfe.TipoDocumentoReceptor, cfe.CodigoPaisReceptor, cfe.NumDocReceptorUruguayo, cfe.NumDocReceptorExtrangero))
            {
                cfe.CampoErroneo = "CodigoPaisReceptor";
                return false;
            }

            ////Valida NumDocReceptorUruguayo
            //if (!NumDocReceptorUruguayo(cfe.TipoDocumentoReceptor, cfe.CodigoPaisReceptor, cfe.NumDocReceptorUruguayo, cfe.NumDocReceptorExtrangero))
            //{
            //    cfe.CampoErroneo = "NumDocReceptorUruguayo";
            //    return false;
            //}

            ////Valida NumDocReceptorExtrangero
            //if (!NumDocReceptorExtrangero(cfe.TipoDocumentoReceptor, cfe.NumDocReceptorUruguayo, cfe.NumDocReceptorExtrangero))
            //{
            //    cfe.CampoErroneo = "NumDocReceptorExtrangero";
            //    return false;
            //}

            //Valida TipoCambio
            if (!TipoCambio(cfe.TipoModena, cfe.TipoCambio))
            {
                cfe.CampoErroneo = "TipoCambio";
                return false;
            }

            //Valida MontoDescuentoItem
            foreach (CFEItems item in cfe.Items)
            {
                if (!MontoDescuentoItem(item.PorcentajeDescuentoItem, item.MontoDescuentoItem))
                {
                    cfe.CampoErroneo = "Items.MontoDescuentoItem";
                    return false;
                }
            }

            //Valida MontoRecargoItem
            foreach (CFEItems item in cfe.Items)
            {
                if (!MontoRecargoItem(item.PorcentajeRecargoItem, item.MontoRecargoItem))
                {
                    cfe.CampoErroneo = "Items.MontoRecargoItem";
                    return false;
                }
            }

            //Valida RazonReferencia
            foreach (CFEInfoReferencia infoReferencia in cfe.InfoReferencia)
            {
                if (!RazonReferencia(infoReferencia.IndicadorReferenciaGlobal, infoReferencia.RazonReferencia))
                {
                    cfe.CampoErroneo = "InfoReferencia.RazonReferencia";
                    return false;
                }
            }

            return true;
        }

        #region FECHAS

        /// <summary>
        /// Valida la fecha del comprobante segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion A-5.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static bool FechaComprobante(string fecha)
        {
            DateTime fechaMinima = new DateTime(2011, 10, 11);//Fecha mimina segun documentacion oficial Formato CFE A-5
            DateTime fechaMaxima = new DateTime(2050, 12, 31);//Fecha mimina segun documentacion oficial Formato CFE A-5

            return ValidarFecha(fecha, fechaMinima, fechaMaxima);
        }

        /// <summary>
        /// Valida la fecha inicial del servicio facturado segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion A-7.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static bool PerdiodoDesde(string fecha)
        {
            DateTime fechaMinima = new DateTime(2000, 01, 01);//Fecha mimina segun documentacion oficial Formato CFE A-7
            DateTime fechaMaxima = new DateTime(2050, 12, 31);//Fecha mimina segun documentacion oficial Formato CFE A-7

            return ValidarFecha(fecha, fechaMinima, fechaMaxima);
        }

        /// <summary>
        ///  Valida la fecha final del servicio facturado segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion A-8.
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public static bool PeriodoHasta(string fechaDesde, string fechaHasta)
        {
            DateTime fechaMinima = new DateTime(2000, 01, 01);//Fecha mimina segun documentacion oficial Formato CFE A-7
            DateTime fechaMaxima = new DateTime(2050, 12, 31);//Fecha mimina segun documentacion oficial Formato CFE A-7

            if (ValidarFecha(fechaDesde, fechaMinima, fechaMaxima) && ValidarFecha(fechaHasta, fechaMinima, fechaMaxima))
            {
                return ValidarFecha(fechaDesde, fechaHasta);
            }

            return false;
        }

        /// <summary>
        /// Valida la fecha de vencimiento del servicio facturado segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion A-12.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static bool FechaVencimiento(string fecha)
        {
            DateTime fechaMinima = new DateTime(2000, 01, 01);//Fecha mimina segun documentacion oficial Formato CFE A-12
            DateTime fechaMaxima = new DateTime(2050, 12, 31);//Fecha mimina segun documentacion oficial Formato CFE A-12

            return ValidarFecha(fecha, fechaMinima, fechaMaxima);
        }

        /// <summary>
        /// Valida la fecha del comprobante de referencia  segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion F-7.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static bool FechaCFEReferencia(string fecha)
        {
            DateTime fechaMinima = new DateTime(2000, 01, 01);//Fecha mimina segun documentacion oficial Formato CFE F-7
            DateTime fechaMaxima = new DateTime(2050, 12, 31);//Fecha mimina segun documentacion oficial Formato CFE F-7

            return ValidarFecha(fecha, fechaMinima, fechaMaxima);
        }

        /// <summary>
        /// Valida la fecha de vencimiento del CAE segun el formato especificado en documentacion oficial Formato_CFE_v10 seccion CFE G-4.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static bool FechaVencimientoCAE(string fecha)
        {
            DateTime fechaMinima = new DateTime(2000, 01, 01);//Fecha mimina segun documentacion oficial Formato CFE G-4
            DateTime fechaMaxima = new DateTime(2050, 12, 31);//Fecha mimina segun documentacion oficial Formato CFE G-4

            return ValidarFecha(fecha, fechaMinima, fechaMaxima);
        }

        /// <summary>
        /// Valida que una fecha cumpla con el formato AAAAMMDD y que este dentro del rango establecido
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        private static bool ValidarFecha(string fecha, DateTime fechaDesde, DateTime fechaHasta)
        {
            bool salida = false;

            try
            {
                //Valida que el valor ingresado sea de 8 valores numericos
                if (Regex.IsMatch(fecha, "^[0-9]{8}$"))
                {
                    //Crea fecha basada en el valor ingresado
                    DateTime fechaActual = new DateTime(int.Parse(fecha.Substring(0, 4)), int.Parse(fecha.Substring(4, 2)), int.Parse(fecha.Substring(6, 2)));
                    //Valida que la fecha este en el rango permitido
                    if (fechaActual.Date >= fechaDesde && fechaActual <= fechaHasta)
                    {
                        salida = true;
                    }
                }
            }
            catch (Exception)
            {
                salida = false;
            }

            return salida;
        }

        /// <summary>
        /// Valida que la "fecha desde" sea menor que la "fecha hasta" y que ambas cumplan con el formato AAAAMMDD.
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        private static bool ValidarFecha(string fechaDesde, string fechaHasta)
        {
            bool salida = false;

            try
            {
                //Valida que el valor ingresado sea de 8 valores numericos
                if (Regex.IsMatch(fechaDesde, "^[0-9]{8}$") && Regex.IsMatch(fechaHasta, "^[0-9]{8}$"))
                {
                    //Crea fecha basada en el valor ingresado
                    DateTime fechaMinima = new DateTime(int.Parse(fechaDesde.Substring(0, 4)), int.Parse(fechaDesde.Substring(4, 2)), int.Parse(fechaDesde.Substring(6, 2)));

                    //Crea fecha basada en el valor ingresado
                    DateTime fechaMaxima = new DateTime(int.Parse(fechaHasta.Substring(0, 4)), int.Parse(fechaHasta.Substring(4, 2)), int.Parse(fechaHasta.Substring(6, 2)));

                    //Valida que la fecha hasta sea mayor que la fecha desde
                    if (fechaMaxima.Date >= fechaMinima.Date)
                    {
                        salida = true;
                    }
                }
            }
            catch (Exception)
            {
                salida = false;
            }

            return salida;
        }

        #endregion FECHAS

        #region DOCUMENTO RECEPTOR

        /// <summary>
        /// Valida tipo de documento receptor segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion A-60
        /// </summary>
        /// <param name="tipoCFE"></param>
        /// <param name="tipoDocumentoReceptor"></param>
        /// <param name="codigoPaisReceptor"></param>
        /// <param name="numDocReceptorUruguayo"></param>
        /// <param name="numDocReceptorExtrangero"></param>
        /// <param name="totalMontoNoGravado"></param>
        /// <param name="totalMontoExportacionAsimilados"></param>
        /// <param name="totalMontoImpuestoPercibido"></param>
        /// <param name="totalMontoIVASuspenso"></param>
        /// <param name="totalMontoNetoIVATasaMinima"></param>
        /// <param name="totalMontoNetoIVATasaBasica"></param>
        /// <param name="totalMontoNetoIVAOtraTasa"></param>
        /// <returns></returns>
        public static bool TipoDocumentoReceptor(CFE.ESTipoCFECFC tipoCFE, CFE.ESTipoDocumentoReceptor tipoDocumentoReceptor, string codigoPaisReceptor, string numDocReceptorUruguayo,
            string numDocReceptorExtrangero, double totalMontoNoGravado, double totalMontoExportacionAsimilados, double totalMontoImpuestoPercibido,
            double totalMontoIVASuspenso, double totalMontoNetoIVATasaMinima, double totalMontoNetoIVATasaBasica, double totalMontoNetoIVAOtraTasa)
        {

            double montoNeto = 0;

            if (tipoDocumentoReceptor != 0)
            {
                if (codigoPaisReceptor != "" && (numDocReceptorUruguayo != "" || numDocReceptorExtrangero != ""))
                {
                    if ((tipoCFE == CFE.ESTipoCFECFC.EFactura || tipoCFE == CFE.ESTipoCFECFC.NCEFactura || tipoCFE == CFE.ESTipoCFECFC.NDEFactura) && tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.RUC)
                    {
                        return true;
                    }
                    else if ((tipoCFE == CFE.ESTipoCFECFC.ETicket || tipoCFE == CFE.ESTipoCFECFC.NCETicket || tipoCFE == CFE.ESTipoCFECFC.NDETicket))
                    {
                        montoNeto = totalMontoNoGravado + totalMontoExportacionAsimilados + totalMontoImpuestoPercibido + totalMontoIVASuspenso + totalMontoNetoIVATasaMinima + totalMontoNetoIVAOtraTasa;

                        if (montoNeto > 10000)
                        {
                            if (tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.CI || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Otros
                                || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Pasaporte || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.DNI)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Valida el codigo de pais receptor segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion A-61
        /// </summary>
        /// <param name="tipoDocumentoReceptor"></param>
        /// <param name="codigoPaisReceptor"></param>
        /// <param name="numDocReceptorUruguayo"></param>
        /// <param name="numDocReceptorExtrangero"></param>
        /// <returns></returns>
        public static bool CodigoPaisReceptor(CFE.ESTipoDocumentoReceptor tipoDocumentoReceptor, string codigoPaisReceptor, string numDocReceptorUruguayo, string numDocReceptorExtrangero)
        {
            if (codigoPaisReceptor != "")
            {
                if (tipoDocumentoReceptor != 0 && (numDocReceptorUruguayo != "" || numDocReceptorExtrangero != ""))
                {
                    if ((tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.CI) && codigoPaisReceptor == "UY")
                    {
                        return true;
                    }
                    else if (tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.DNI && (codigoPaisReceptor == "AR" || codigoPaisReceptor == "BR" || codigoPaisReceptor == "CL" || codigoPaisReceptor == "PY"))
                    {
                        return true;
                    }
                    else if ((tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Otros || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Pasaporte) && (ValidacionISO3166.ValidarCodigoPais(codigoPaisReceptor) || codigoPaisReceptor == "99"))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///  Valida el numero de documento receptor uruguayo segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion A-62
        /// </summary>
        /// <param name="tipoDocumentoReceptor"></param>
        /// <param name="codigoPaisReceptor"></param>
        /// <param name="numDocReceptorUruguayo"></param>
        /// <param name="numDocReceptorExtrangero"></param>
        /// <returns></returns>
        public static bool NumDocReceptorUruguayo(CFE.ESTipoDocumentoReceptor tipoDocumentoReceptor, string codigoPaisReceptor, string numDocReceptorUruguayo, string numDocReceptorExtrangero)
        {
            if (tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.CI || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.RUC)
            {
                if (numDocReceptorUruguayo == "")
                {
                    return false;
                }
            }

            if (numDocReceptorUruguayo != "")
            {
                if (tipoDocumentoReceptor != 0 && codigoPaisReceptor != "" && numDocReceptorExtrangero == "")
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Valida el numero de documento receptor extrangero segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion A-62.1
        /// </summary>
        /// <param name="tipoDocumentoReceptor"></param>
        /// <param name="numDocReceptorUruguayo"></param>
        /// <param name="numDocReceptorExtrangero"></param>
        /// <returns></returns>
        public static bool NumDocReceptorExtrangero(CFE.ESTipoDocumentoReceptor tipoDocumentoReceptor, string numDocReceptorUruguayo, string numDocReceptorExtrangero)
        {
            if (tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.DNI || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Otros || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Pasaporte)
            {
                if (numDocReceptorExtrangero == "")
                {
                    return false;
                }
            }

            if (numDocReceptorExtrangero != "" && numDocReceptorUruguayo == "")
            {
                return true;
            }

            return false;
        }

        #endregion DOCUMENTO RECEPTOR

        #region TOTALES ENCABEZADO

        /// <summary>
        /// Valida el tipo de cambio segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion A-111 
        /// </summary>
        /// <param name="tipoModena"></param>
        /// <param name="tipoCambio"></param>
        /// <returns></returns>
        public static bool TipoCambio(string tipoModena, double tipoCambio)
        {
            if (tipoModena != "UYU" && tipoCambio == 0)
            {
                return false;
            }

            return true;
        }

        #endregion TOTALES ENCABEZADO

        #region ITEMS

        /// <summary>
        /// Validar el monto de descuento de cada item segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion B-13
        /// </summary>
        /// <param name="porcentajeDescuentoItem"></param>
        /// <param name="montoDescuentoItem"></param>
        /// <returns></returns>
        public static bool MontoDescuentoItem(double porcentajeDescuentoItem, double montoDescuentoItem)
        {
            if (porcentajeDescuentoItem != 0)
            {
                if (montoDescuentoItem != 0)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validar el monto de recargo de cada item segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion B-17
        /// </summary>
        /// <param name="porcentajeRecargoItem"></param>
        /// <param name="montoRecargoItem"></param>
        /// <returns></returns>
        public static bool MontoRecargoItem(double porcentajeRecargoItem, double montoRecargoItem)
        {
            if (montoRecargoItem != 0)
            {
                if (porcentajeRecargoItem != 0)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        #endregion ITEMS

        #region INFORMACION DE REFERENCIA

        /// <summary>
        /// Valida la razon de referencia segun la especificacion en la documentacion oficial Formato_CFE_v10 seccion F-6
        /// </summary>
        /// <param name="indicadorReferenciaGlobal"></param>
        /// <param name="razonReferencia"></param>
        /// <returns></returns>
        public static bool RazonReferencia(CFEInfoReferencia.ESIndicadorReferenciaGlobal indicadorReferenciaGlobal, string razonReferencia)
        {
            if (indicadorReferenciaGlobal == CFEInfoReferencia.ESIndicadorReferenciaGlobal.ReferenciaGlobal)
            {
                if (razonReferencia != "")
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        #endregion INFORMACION DE REFERENCIA

        #endregion VALIDACIONES

    }
}
