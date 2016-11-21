using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Globalization;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.DocumentosB1
{
    /// <summary>
    /// Contiene los metodos para las operaciones de los documentos
    /// </summary>
    class ManteDocumentos
    {
        /// <summary>
        /// Valida que el un documento determinado sea factura o nota de credito. Retorna true si es factura.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool ValidarDocumentoFactura(int numeroDocumento, string tabla)
        {
            Recordset recSet = null;
            string consulta = "";
            bool salida = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry FROM [" + tabla + "] WHERE DocEntry = '" + numeroDocumento + "' AND DocSubType = '--'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return salida;
        }

        /// <summary>
        /// Consulta el codigo del pais de un socio de negocios determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoSocioNegocio"></param>
        /// <returns></returns>
        public string ConsultarCodPaisSocioNegocio(string codigoSocioNegocio, CFE.ESTipoDocumentoReceptor tipoDocumentoReceptor = CFE.ESTipoDocumentoReceptor.Nada)
        {
            Recordset recSet = null;
            string consulta = "", codigoPais = "";

            try
            {

                if ((tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.RUC || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.CI || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Nada ))
                {
                    //Obtener objeto estandar de record set
                    recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                    //Establecer consulta
                    consulta = "SELECT Country FROM [OCRD] WHERE CardCode = '" + codigoSocioNegocio + "'";

                    //Ejecutar consulta
                    recSet.DoQuery(consulta);

                    //Validar que se hayan obtenido registros
                    if (recSet.RecordCount > 0)
                    {
                        codigoPais = recSet.Fields.Item("Country").Value + "";
                        codigoPais = codigoPais.ToUpper();

                        if (codigoPais.Equals("ZF"))
                        {
                            codigoPais = "UY";
                        }
                    }
                }
                else if (tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.DNI )
                {
                    codigoPais = "AR";
                }
                else if ((tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Otros || tipoDocumentoReceptor == CFE.ESTipoDocumentoReceptor.Pasaporte) )
                {
                    codigoPais = "99";
                }


               

            }
            catch (Exception)
            {
                codigoPais = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto recSet
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return codigoPais;
        }

        /// <summary>
        /// Consulta el nombre del pais de un socio de negocios determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoSocioNegocio"></param>
        /// <returns></returns>
        public string ConsultarNombPaisSocioNegocio(string codigoSocioNegocio)
        {
            Recordset recSet = null;
            string consulta = "", nombrePais = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT Name FROM [OCRD] AS T1 INNER JOIN [OCRY] AS T2 ON T1.Country = T2.Code WHERE CardCode = '" + codigoSocioNegocio + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    nombrePais = recSet.Fields.Item("Name").Value + "";
                    if (nombrePais.Equals("Zona Franca"))
                    {
                        nombrePais = "Uruguay";
                    }
                }
            }
            catch (Exception)
            {
                nombrePais = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return nombrePais;
        }

        /// <summary>
        /// Consulta los datos del emisor almacenados en Detalles de la Sociedad
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public Emisor ConsultarDatosEmisor()
        {
            Emisor emisor = new Emisor();
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "SELECT U_Ruc, U_Nombre, U_NombreC, U_NumRes FROM [@TFEEMI]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    emisor.Ruc = long.Parse(recSet.Fields.Item("U_Ruc").Value + "");
                    emisor.Nombre = recSet.Fields.Item("U_Nombre").Value + "";
                    emisor.NombreComercial = recSet.Fields.Item("U_NombreC").Value + "";
                    emisor.NumeroResolucion = recSet.Fields.Item("U_NumRes").Value + "";
                }
            }
            catch (Exception)
            {
                emisor = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return emisor;
        }

        /// <summary>
        /// Consulta la sucursal del usuario que confecciono el documento
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        public string ConsultarSucursalEmisor(string tablaDocumentos, string codigoDocumento)
        {
            string sucursal = "", consulta = "";
            Recordset recSet = null;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT T3.Name FROM [OUSR] AS T1 INNER JOIN [" + tablaDocumentos + "] AS T2 ON T1.USERID = T2.UserSign INNER JOIN [OUBR] AS T3 ON T1.Branch = T3.Code WHERE T2.DocEntry = '" + codigoDocumento + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    sucursal = recSet.Fields.Item("Name").Value + "";
                }
            }
            catch (Exception)
            {
                sucursal = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return sucursal;
        }


        public double ObtenerDocRate()
        {
            Recordset recSet = null;
            string consulta = "";
            double resultado = 0;
            double prueba = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta 
                consulta = "select Rate from ORTT where DAY(GETDATE()) = DAY(RateDate) AND MONTH(GETDATE()) = MONTH(RateDate) AND YEAR(GETDATE()) = YEAR(RateDate) AND (Currency = 'UY' OR Currency = 'UYU')";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    prueba = recSet.Fields.Item("Rate").Value;

                    //Obtener los valores para las propiedades del objeto emiso
                    resultado = double.Parse(recSet.Fields.Item("Rate").Value + "");
                }
            }
            catch (Exception)
            {
                resultado = 0;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el indicador de facturacion de acuerdo a un tipo de impuesto
        /// </summary>
        /// <param name="tipoImpuesto"></param>
        /// <returns></returns>
        public string ObtenerIndicadorFacturacion(string tipoImpuesto)
        {
            Recordset registro = null;
            string resultado = "", consulta = "SELECT U_TipImpDgi FROM [@TFEIMPDGIB1] WHERE U_CodImpB1 = '" + tipoImpuesto + "'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("U_TipImpDgi").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria al objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Consulta la caja del usuario que confecciono el documento
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        public string ConsultarCajaEmisor(string tablaDocumentos, string codigoDocumento)
        {
            string caja = "", consulta = "";
            Recordset recSet = null;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT FormatCode FROM OACT AS TOACT INNER JOIN (SELECT CashAcct FROM [OUDG] AS T1 INNER JOIN [OUSR] AS T2 ON T1.Code = T2.DfltsGroup INNER JOIN [" + tablaDocumentos + "] AS T3 ON T2.USERID = T3.UserSign WHERE T3.DocEntry = '" + codigoDocumento + "') AS C1 ON TOACT.AcctCode = C1.CashAcct";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    caja = recSet.Fields.Item("FormatCode").Value + "";
                }
            }
            catch (Exception)
            {
                caja = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return caja;
        }

        /// <summary>
        /// Consulta la direccion del emisor
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ConsultarDireccionEmisor()
        {
            Recordset recSet = null;
            string consulta = "", direccion = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT CompnyAddr FROM [OADM]";
                //consulta = "SELECT Street, StreetNo, City FROM ADM1";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    direccion = recSet.Fields.Item("CompnyAddr").Value + "";
                    //Obtener los valores para las propiedades del objeto emiso
                    //direccion += recSet.Fields.Item("Street").Value + " ";
                    //direccion += recSet.Fields.Item("StreetNo").Value + " ";
                    //direccion += recSet.Fields.Item("City").Value + "";
                }
            }
            catch (Exception)
            {
                direccion = "Sin registrar";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            if (direccion.Equals(""))
            {
                direccion = "Sin registrar";
                return direccion;
            }
            else
            {
                return direccion.Replace("\r", " ");
            }
        }

        /// <summary>
        /// Consulta la ciudad del emisor
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ConsultarCiudadEmisor()
        {
            Recordset recSet = null;
            string consulta = "", direccion = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT CityF FROM [ADM1]";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    direccion = recSet.Fields.Item("CityF").Value + "";
                }
            }
            catch (Exception)
            {
                direccion = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return direccion;
        }

        /// <summary>
        /// Consulta el codigo ISO de una modena determinada
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoModena"></param>
        /// <returns></returns>
        public string ConsultarISOMoneda(string codigoModena)
        {
            Recordset recSet = null;
            string consulta = "", monedaISO = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT ISOCurrCod FROM [OCRN] WHERE CurrCode = '" + codigoModena + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    monedaISO = recSet.Fields.Item("ISOCurrCod").Value + "";
                }
            }
            catch (Exception)
            {
                monedaISO = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return monedaISO;
        }

        /// <summary>
        /// Consulta el valor de una tasa indicada
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public double ConsultarMontoTasa(string tipoTasa)
        {
            Recordset recSet = null;
            string consulta = "";
            double montoTasa = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT Rate FROM [OSTA] WHERE Code = '" + tipoTasa + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    montoTasa = double.Parse(recSet.Fields.Item("Rate").Value + "");
                }
            }
            catch (Exception)
            {
                montoTasa = 0;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return montoTasa;
        }

        /// <summary>
        /// Consulta el total de una tasa determinada de cada linea de un documento
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public double ConsultaTotalTasa(string codigoDocumento, string tipoTasa, string tabla)
        {
            Recordset recSet = null;
            string consulta = "";
            double totalTasa = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT SUM(VatSum) AS 'VatSum' FROM [" + tabla + "] WHERE TaxCode = '" + tipoTasa + "' AND DocEntry = " + codigoDocumento;

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    totalTasa = double.Parse(recSet.Fields.Item("VatSum").Value + "");
                }
            }
            catch (Exception)
            {
                totalTasa = 0;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return totalTasa;
        }


        public string ObtenerConfiguracionCAEGlobal(string ModoCAE_generico)
        {
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_CAE_General  FROM [@TFEDRPTD]";


                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {

                    ModoCAE_generico = recSet.Fields.Item("U_CAE_General").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return ModoCAE_generico;
        }

        /// <summary>
        /// Consulta la serie y numero siguiente del rango de CFE para un tipo de documento determinado
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        public Rango ConsultarDatosRangoCFE(string tipoDocumento, string sucursal, string caja)
        {
            Recordset recSet = null;
            string consulta = "";
            Rango rango = new Rango();

            string CAE_General = "N";


            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                
                
                CAE_General = ObtenerConfiguracionCAEGlobal(CAE_General);



                if (CAE_General.Equals("Y"))
                {
                    //Establecer consulta
                    consulta = "EXECUTE ObtenerNumeroElectronicoGenerico @tipo = '" + tipoDocumento + "', @sucursal = '" + sucursal + "', @caja = '" + caja + "'";
                }
                else
                {
                    //Establecer consulta
                    consulta = "EXECUTE ObtenerNumeroElectronico @tipo = '" + tipoDocumento + "', @sucursal = '" + sucursal + "', @caja = '" + caja + "'";
                }


                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto rango
                    rango.NumeroActual = int.Parse(recSet.Fields.Item("U_NumAct").Value + "") - 1;
                    rango.Serie = recSet.Fields.Item("U_Serie").Value + "";
                }
            }
            catch (Exception)
            {
                rango = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return rango;
        }

        /// <summary>
        /// Consulta la unidad de medida de un material
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ConsultaUnidadMedidaItem(string codigoItem, string docEntry, string numlinea, string tabla)
        {
            Recordset recSet = null;
            string consulta = "SELECT UomCode FROM " + tabla + "  WHERE ItemCode = '" + codigoItem + "' AND DocEntry = '" +
                docEntry + "' AND LineNum = '" + numlinea + "'", unidadMedida = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    unidadMedida = recSet.Fields.Item("UomCode").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return unidadMedida;
        }

        /// <summary>
        /// Consulta el tipo de pago utilizado
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ConsultarMedioPago(string tablaDocumentos, string codigoDocumento)
        {
            Recordset recSet = null;
            string consulta = "", salida = "";
            double pagoEfectivo = 0, pagoCheque = 0, pagoTransferencia = 0, pagoTarjetaCredito = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT CashSum, CheckSum, TrsfrSum, CreditSum FROM [ORCT] AS T1 INNER JOIN [" +
                    tablaDocumentos + "] AS T2 ON T1.DocEntry = T2.ReceiptNum WHERE T2.DocEntry = '" + codigoDocumento + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtiene los valores para los distintos tipos de pago
                    pagoEfectivo = double.Parse(recSet.Fields.Item("CashSum").Value + ""); //Efectivo
                    pagoCheque = double.Parse(recSet.Fields.Item("CheckSum").Value + "");//Cheque
                    pagoTransferencia = double.Parse(recSet.Fields.Item("TrsfrSum").Value + "");//transferencia bancaria
                    pagoTarjetaCredito = double.Parse(recSet.Fields.Item("CreditSum").Value + "");//Tarjeta de credito

                    ////Valida cual fue el tipo de pago utilizado
                    if (pagoEfectivo != 0)
                    {
                        salida = "Contado";
                    }
                    else if (pagoCheque != 0)
                    {
                        salida = "Contado";
                    }
                    else if (pagoTransferencia != 0)
                    {
                        salida = "Contado";
                    }
                    else if (pagoTarjetaCredito != 0)
                    {
                        salida = "Contado";
                    }
                    else
                    {
                        salida = "Crédito";
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Libera de memoria  el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            //Si no existe ningun tipo de pago se establece credito
            return salida;
        }

        /// <summary>
        /// Consulta la direccion de correo electronico de un cliente
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        public string ConsultarCorreoReceptor(string codigoCliente)
        {
            Recordset recSet = null;
            string consulta = "", correoReceptor = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT E_Mail FROM [OCRD] WHERE CardCode = '" + codigoCliente + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    correoReceptor = recSet.Fields.Item("E_Mail").Value + "";//Correo del cliente
                }
            }
            catch (Exception)
            {
                correoReceptor = "";
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return correoReceptor;
        }

        /// <summary>
        /// Consulta la serie y el numero de CFE del documento al que se le aplicó una nota de credito
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tablaDetalleDocumentos"></param>
        /// <param name="codigoDocumento"></param>
        /// <returns></returns>
        //public CFE ConsultarDocumentoReferencia(string tablaDetalleDocumentos, string codigoDocumento)
        public CFEInfoReferencia ConsultarDocumentoReferencia(string tablaDetalleDocumentos, string codigoDocumento)
        {
            Recordset recSet = null;
            string consulta = "";
            CFEInfoReferencia cfeInfoRef = null; ;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_Serie, U_NumCFE, U_TipoDoc FROM [@TFECFE] AS T1 INNER JOIN [" + tablaDetalleDocumentos + "] AS T2 ON T1.U_DocSap = T2.BaseEntry WHERE T2.DocEntry = '" + codigoDocumento + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    cfeInfoRef = new CFEInfoReferencia();
                    cfeInfoRef.SerieComprobanteReferencia = recSet.Fields.Item("U_Serie").Value + "";//Serie del documento de referencia
                    cfeInfoRef.NumeroComprobanteReferencia = int.Parse(recSet.Fields.Item("U_NumCFE").Value + "");//Numero de CFE del documento de referencia
                    cfeInfoRef.TipoCFEReferencia = int.Parse(recSet.Fields.Item("U_TipoDoc").Value + "");
                }
            }
            catch (Exception)
            {
                cfeInfoRef = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return cfeInfoRef;
        }

        /// <summary>
        /// Consulta la serie y el numero de CFE del documento al que se le aplicó una nota de debito
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroNotaDebito"></param>
        /// <returns></returns>
        public CFE ConsultarDocumentoReferencia(CFE.ESTipoCFECFC tipoDocumento, string numeroNotaDebito)
        {
            Recordset recSet = null;
            string consulta = "";
            CFE cfe = null;
            string tabla = "";
            string campoNumero = "";
            string campoSerie = "";

            try
            {
                //Establecer consulta
                switch (tipoDocumento)
                {
                    case CFE.ESTipoCFECFC.NCEFactura:
                        tabla = "ORIN";
                        campoSerie = "U_SerieRefNC";
                        campoNumero = "U_NumRefNC";
                        break;
                    case CFE.ESTipoCFECFC.NCETicket:
                        tabla = "ORIN";
                        campoSerie = "U_SerieRefNC";
                        campoNumero = "U_NumRefNC";
                        break;
                    case CFE.ESTipoCFECFC.NDEFactura:
                        tabla = "OINV";
                        campoSerie = "U_SerieRefND";
                        campoNumero = "U_NumRefND";
                        break;
                    case CFE.ESTipoCFECFC.NDETicket:
                        tabla = "OINV";
                        campoSerie = "U_SerieRefND";
                        campoNumero = "U_NumRefND";
                        break;
                }

                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                consulta = "SELECT " + campoSerie + ", " + campoNumero + " FROM " + tabla + " WHERE DocEntry = '" + numeroNotaDebito + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    cfe = new CFE();
                    cfe.SerieComprobante = recSet.Fields.Item(campoSerie).Value + "";//Serie del documento de referencia
                    cfe.ComprobanteReferencia = recSet.Fields.Item(campoNumero).Value + "";//Numero de CFE del documento de referencia
                }
            }
            catch (Exception)
            {
                cfe = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return cfe;
        }

        /// <summary>
        /// Consulta los datos de referencia utilizados y retorna true si existen
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="serieReferencia"></param>
        /// <param name="numeroReferencia"></param>
        /// <returns></returns>
        public bool ValidarReferencia(string serieReferencia, string numeroReferencia)
        {
            Recordset recSet = null;
            string consulta = "";
            bool salida = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT DocEntry FROM [OINV] WHERE U_SerieFA = '" + serieReferencia + "' and U_NumeroFA = '" + numeroReferencia + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return salida;
        }

        /// <summary>
        /// Consulta los resguardos aplicados a una factura
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public bool ValidarDocumentoResguardo(int numeroDocumento, string tabla)
        {
            Recordset recSet = null;
            string consulta = "";
            bool salida = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT AbsEntry FROM [" + tabla + "] WHERE AbsEntry = '" + numeroDocumento + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }
            return salida;
        }

        /// <summary>
        /// Valida que un cliente tenga el tipo de pago de contado establecido
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        public bool ValidarClienteContado(string codigoCliente)
        {
            Recordset recSet = null;
            string consulta = "";
            bool salida = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT CardCode FROM [OCRD] WHERE CardCode = '" + codigoCliente + "' AND U_SNCont = 'Y'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    salida = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return salida;
        }

        /// <summary>
        /// Consulta los datos del CAE para cada tipo de documento
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        public CAE ConsultarDatosCAE(string tipoDocumento)
        {
            Recordset recSet = null;
            string consulta = "";
            CAE cae = new CAE();

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                //consulta = "SELECT * FROM [OINV]";
                consulta = "SELECT U_NumAut, T2.U_NumIni, T1.U_NumFin, CONVERT(VARCHAR(10), T1.U_ValHasta, 23) AS 'U_ValHasta' FROM [@TFERANGO] T1" +
                            " INNER JOIN [@TFECAE] AS T2 ON T2.DocEntry =  U_idCAE WHERE T1.U_TipoDoc =  '" + tipoDocumento + "' AND T1.U_ValDesde <= GETDATE() AND T1.U_ValHasta >= GETDATE() AND U_Activo = 'Y'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto CAE
                    cae.NumeroAutorizacion = recSet.Fields.Item("U_NumAut").Value + "";
                    cae.NumeroDesde = int.Parse(recSet.Fields.Item("U_NumIni").Value + "");
                    cae.NumeroHasta = int.Parse(recSet.Fields.Item("U_NumFin").Value + "");
                    cae.FechaVencimiento = recSet.Fields.Item("U_ValHasta").Value + "";
                }
            }
            catch (Exception)
            {
                cae = null;
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return cae;
        }

        /// <summary>
        /// Valida que el cliente sea extranjero. 
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        public bool ValidarClienteExtranjero(string codigoCliente)
        {
            Recordset recSet = null;
            string consulta = "";
            bool clienteExtranjero = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select VatStatus from OCRD where CardCode = '" + codigoCliente + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    clienteExtranjero = recSet.Fields.Item("VatStatus").Value == "Y" ? false : true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return clienteExtranjero;
        }

        /// <summary>
        /// consulta el indicador de facturacion del articulo
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        public int ConsultarIndicadorFacturacionArticulo(string codigoArticulo)
        {
            Recordset recSet = null;
            string consulta = "";
            int indicador = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select U_IndFacNF from OITM where ItemCode = '" + codigoArticulo + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    indicador = int.Parse(recSet.Fields.Item("U_IndFacNF").Value + "");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return indicador;
        }

        /// <summary>
        /// Consulta el valor del iva tasa basica
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public int ConsultarIvaTasaBasica()
        {
            Recordset recSet = null;
            string consulta = "";
            int ivaTasaBasica = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select ostc.Rate from OSTC inner join [@TFEIMPDGIB1] as t1 on ostc.Code = t1.U_CodImpB1 and t1.U_TipImpDgi = '3'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    ivaTasaBasica = int.Parse(recSet.Fields.Item("Rate").Value + "");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return ivaTasaBasica;
        }

        /// <summary>
        /// Consulta el valor del iva tasa minima
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public int ConsultarIvaTasaMinima()
        {
            Recordset recSet = null;
            string consulta = "";
            int ivaTasaBasica = 0;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select ostc.Rate from OSTC inner join [@TFEIMPDGIB1] as t1 on ostc.Code = t1.U_CodImpB1 and t1.U_TipImpDgi = '2'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    ivaTasaBasica = int.Parse(recSet.Fields.Item("Rate").Value + "");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return ivaTasaBasica;
        }

        /// <summary>
        /// Consulta el codigo de impuesto para cada tipo 
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string ConsultarCodigoImpuesto(string tipoImpuesto)
        {
            Recordset recSet = null;
            string consulta = "";
            string codigoImpuesto = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "select U_CodImpB1 from [@TFEIMPDGIB1] where U_TipImpDgi = '" + tipoImpuesto + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    codigoImpuesto = recSet.Fields.Item("U_CodImpB1").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return codigoImpuesto;
        }

        /// <summary>
        /// Consulta el codigo y valor y el monto de la retencion 
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public List<CFERetencPercep> ConsultarRetencionPerecepcion(string numeroDocumento, string tabla, string moneda)
        {
            Recordset recSet = null;
            string consulta = "";
            CFERetencPercep cfeRetPer = null;

            List<CFERetencPercep> ListcfeRetPer = new List<CFERetencPercep>();

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                if (moneda == "UYU" || moneda == "$")
                {
                    //Establecer consulta
                    consulta = "SELECT T2.U_FormBeta, T1.WTAmnt FROM " + tabla + " AS T1 INNER JOIN  [@TFERP] AS T2 ON T1.WTCode = T2.U_CodRet AND T1.AbsEntry = '" + numeroDocumento + "'";
                }
                else
                {
                    //Establecer consulta
                    consulta = "SELECT T2.U_FormBeta, T1.WTAmntFC AS WTAmnt FROM " + tabla + " AS T1 INNER JOIN  [@TFERP] AS T2 ON T1.WTCode = T2.U_CodRet AND T1.AbsEntry = '" + numeroDocumento + "'";
                }

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                recSet.MoveFirst();

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    for (int i = 0; i < recSet.RecordCount; i++)
                    {
                        cfeRetPer = new CFERetencPercep();

                        cfeRetPer.CodigoRetencPercep = recSet.Fields.Item("U_FormBeta").Value + "";
                        cfeRetPer.ValorRetencPercep = double.Parse(recSet.Fields.Item("WTAmnt").Value + "");


                        ListcfeRetPer.Add(cfeRetPer);

                        recSet.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return ListcfeRetPer;
        }

        /// <summary>
        /// Consulta la lista de retenciones percepciones en un documento
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public List<CFEItemsRetencPercep> ConsultarItemRetencionPercepcion(string numeroDocumento, string tabla, string moneda)
        {
            List<CFEItemsRetencPercep> listaRetPer = new List<CFEItemsRetencPercep>();
            CFEItemsRetencPercep cfeItemRetPer;
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                if (moneda == "UYU" || moneda == "$")
                {
                    //Establecer consulta
                    consulta = "select T2.U_FormBeta, T1.Rate, T1.TaxbleAmnt, T1.WTAmnt from " + tabla + " as T1 inner join  [@TFERP] as T2 on T1.WTCode = T2.U_CodRet and T1.AbsEntry = '" + numeroDocumento + "'";
                }
                else
                {
                    //Establecer consulta
                    consulta = "select T2.U_FormBeta, T1.Rate, T1.TxblAmntFC AS TaxbleAmnt, T1.WTAmntFC AS WTAmnt from " + tabla + " as T1 inner join  [@TFERP] as T2 on T1.WTCode = T2.U_CodRet and T1.AbsEntry = '" + numeroDocumento + "'";
                }

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                recSet.MoveFirst();

                //Validar que se hayan obtenido registros
                for (int i = 0; i < recSet.RecordCount; i++)
                {
                    cfeItemRetPer = new CFEItemsRetencPercep();

                    cfeItemRetPer.CodigoRetencPercep = recSet.Fields.Item("U_FormBeta").Value + "";
                    cfeItemRetPer.Tasa = double.Parse(recSet.Fields.Item("Rate").Value + "");
                    cfeItemRetPer.MontoSujetRetencPercep = double.Parse(recSet.Fields.Item("TaxbleAmnt").Value + "");
                    cfeItemRetPer.ValorRetencPercep = double.Parse(recSet.Fields.Item("WTAmnt").Value + "");

                    listaRetPer.Add(cfeItemRetPer);

                    recSet.MoveNext();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return listaRetPer;
        }

        /// <summary>
        /// Consulta la informacion de referncia de un resguardo
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public List<CFEInfoReferencia> ConsultarReferenciaResguardo(string numeroDocumento, string tabla)
        {
            List<CFEInfoReferencia> listaRef = new List<CFEInfoReferencia>();
            CFEInfoReferencia referencia = null;
            Recordset recSet = null;
            string consulta = "";

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT U_SerieFA, U_NumeroFA FROM " + tabla + " WHERE DocEntry = '" + numeroDocumento + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                recSet.MoveFirst();

                //Validar que se hayan obtenido registros
                for (int i = 0; i < recSet.RecordCount; i++)
                {
                    referencia = new CFEInfoReferencia();

                    referencia.NumeroLinea = i + 1;
                    referencia.TipoCFEReferencia = 111;
                    referencia.SerieComprobanteReferencia = recSet.Fields.Item("U_SerieFA").Value + "";
                    referencia.NumeroComprobanteReferencia = int.Parse(recSet.Fields.Item("U_NumeroFA").Value + "");

                    listaRef.Add(referencia);

                    recSet.MoveNext();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (recSet != null)
                {
                    //Liberar memoria utilizada por el objeto record set
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                    System.GC.Collect();
                }
            }

            return listaRef;
        }



        /// <summary>
        /// Consulta si articulo es sujeto a impuesto
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool ConsultarSujetoImpuesto(string articulo)
        {
            Recordset recSet = null;
            string consulta = "";
            bool sujetoImpuesto = false;

            try
            {
                //Obtener objeto estandar de record set
                recSet = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Establecer consulta
                consulta = "SELECT VATLiable FROM OITM WHERE ItemCode = '" + articulo + "'";

                //Ejecutar consulta
                recSet.DoQuery(consulta);

                //Validar que se hayan obtenido registros
                if (recSet.RecordCount > 0)
                {
                    //Obtener los valores para las propiedades del objeto emiso
                    sujetoImpuesto = ((recSet.Fields.Item("VATLiable").Value + "") == "Y") ? true : false;
                }
            }
            catch (Exception)
            {
                sujetoImpuesto = false;
            }
            if (recSet != null)
            {
                //Liberar memoria utilizada por el objeto record set
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);
                System.GC.Collect();
            }

            return sujetoImpuesto;
        }

        /// <summary>
        /// Actualiza la referencia3 de los asientos con los datos del CAE
        /// </summary>
        /// <param name="numeroAsiento"></param>
        /// <param name="serieCAE"></param>
        public void ActualizarLineaRef3(int numeroAsiento, string serieCAE)
        {
            Recordset registro = null;
            string consulta = "UPDATE JDT1  SET Ref3Line = '" + serieCAE + "' WHERE  TransId = '" + numeroAsiento + "'";

            try
            {
                //Obtener objeto estandar de record set
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Ejecutar consulta
                registro.DoQuery(consulta);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    System.GC.Collect();
                }
            }
        }

        /// <summary>
        /// Obtiene el numero de CAE para alertar fin de rango
        /// </summary>
        /// <returns></returns>
        public FinCae ObtenerAlertaCAE()
        {
            FinCae resultado = new FinCae();
            Recordset registro = null;
            string consulta = "SELECT U_Cant, U_Dia FROM [@TLOGO]";

            try
            {
                //Obtener objeto estandar de record set
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Ejecutar consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado.Cantidad = int.Parse(registro.Fields.Item("U_Cant").Value + "");
                    resultado.Dias = int.Parse(registro.Fields.Item("U_Dia").Value + "");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el numero de lote para un producto
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="codigoArticulo"></param>
        /// <param name="lineNum"></param>
        /// <returns></returns>
        public string ObtenerLote(string baseEntry, string codigoArticulo, int lineNum, string tipoObjeto)
        {
            string resultado = "";
            Recordset registro = null;
            string consulta = "SELECT BatchNum FROM IBT1 WHERE BaseType = " + tipoObjeto + " AND ItemCode =  '" + codigoArticulo + "' AND BaseEntry = " + baseEntry + " AND BaseLinNum = " + lineNum;

            try
            {
                //Obtener objeto estandar de record set
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                //Ejecutar consulta
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("BatchNum").Value + "";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera de memoria el objeto registro
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

    }
}
