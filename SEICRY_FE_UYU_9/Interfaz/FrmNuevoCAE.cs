using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using System.Text.RegularExpressions;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene los metodos para mostrar el formulario que permite agregar un nuevo rango de CAE
    /// </summary>
    class FrmNuevoCAE : FrmBase
    {
        //variables visibles en toda la clase
        private UserDataSource udsTiposDocumento;
        private UserDataSource udsSucursales;
        private UserDataSource udsFechaEmision;
        private UserDataSource udsFechaValido;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Invoca todos los metodos encargados de crear los diferentes componentes del formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            AgregarDataSources();
            EstablecerDataBind();
        }

        /// <summary>
        /// Crea y asigna los data sources al formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AgregarDataSources()
        {
            //Crea el user data source para el combo box de tipsos de documento
            udsTiposDocumento = Formulario.DataSources.UserDataSources.Add("dsCbxTDoc", BoDataType.dt_LONG_TEXT, 40);

            //crea el user data source para el combo box de sucursales
            udsSucursales = Formulario.DataSources.UserDataSources.Add("dsCbxSuc", BoDataType.dt_LONG_TEXT, 100);

            udsFechaEmision = Formulario.DataSources.UserDataSources.Add("dsFecEmi", BoDataType.dt_DATE);

            udsFechaValido = Formulario.DataSources.UserDataSources.Add("dsFecVal", BoDataType.dt_DATE);
        }

        /// <summary>
        /// Asigna los data sources a cada uno de los componentes
        /// </summary>
        protected override void EstablecerDataBind()
        {
            //Establecer data bind a los combo box
            Item cbxTipoDocumento = Formulario.Items.Item("cmbTipDoc");
            ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "", "dsCbxTDoc");

            Item cbxSucursal = Formulario.Items.Item("cmbSuc");
            ((ComboBox)cbxSucursal.Specific).DataBind.SetBound(true, "", "dsCbxSuc");

            //Asigna los valores válidos a los combo box
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("101", "e-Ticket");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("102", "NC. e-Ticket");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("103", "ND. e-Ticket");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("111", "e-Factura");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("112", "NC. e-Factura");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("113", "ND. e-Factura");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("181", "e-Remito");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("182", "e-Resguardo");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("121", "e-Factura Exportación");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("122", "NC. e-Factura Exportación");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("123", "ND. e-Factura Exportación");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("124", "e-Remito Exportación");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("999", "Contingencia");

            //Carga el combo box de sucursales de acuerdo a la consulta realizada
            Sucursal[] sucursales = ConsultarListaSucursales();

            Regex expRegSucursal = new Regex("^([0-9])*$");

            foreach (Sucursal sucursal in sucursales)
            {
                if (expRegSucursal.IsMatch(sucursal.Nombre))
                {
                    ((ComboBox)cbxSucursal.Specific).ValidValues.Add(sucursal.Nombre, sucursal.Descripcion);
                }
            }

            ((EditText)Formulario.Items.Item("txtValDes").Specific).DataBind.SetBound(true, "", "dsFecEmi");
            ((EditText)Formulario.Items.Item("txtValHa").Specific).DataBind.SetBound(true, "", "dsFecVal");
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            //((EditText)Formulario.Items.Item("txtTipAut").Specific).Value = "E";
        }

        #endregion INTERFAZ DE USUARIOS

        #region VALIDACIONES

        //Validar el formato de los campos

        /// <summary>
        /// Valida que la caja contenga maximo 40 caracteres string
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoCaja()
        {
            //Valida que los valores sean maximo dos string
            if (((EditText)Formulario.Items.Item("txtSerie").Specific).String.Length < 40)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que la serie contenga maximo dos caracteres string
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoSerie()
        {
            //Valida que los valores sean maximo dos string
            if (Regex.IsMatch(((EditText)Formulario.Items.Item("txtSerie").Specific).String, Mensaje.expRegSerie))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el numero inicial sea un valor numerico de 1 a 7 caracteres mayores a 0
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoNumeroInicial()
        {
            string valor = ((EditText)Formulario.Items.Item("txtNumIn").Specific).String;

            if (Regex.IsMatch(valor, Mensaje.expRegNumeroInicial))
            {
                if (int.Parse(((EditText)Formulario.Items.Item("txtNumIn").Specific).String) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Valida que el numero final sea un valor numerico de 1 a 7 caracteres mayores a 0
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoNumeroFinal()
        {
            if (Regex.IsMatch(((EditText)Formulario.Items.Item("txtNumFn").Specific).String, Mensaje.expRegNumeroFinal))
            {
                if (int.Parse(((EditText)Formulario.Items.Item("txtNumFn").Specific).String) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Valida que el tipo de autorización sea igual a E
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoTipoAutorizacion()
        {
            if (((EditText)Formulario.Items.Item("txtTipAut").Specific).String.Equals("E") || ((EditText)Formulario.Items.Item("txtTipAut").Specific).String.Equals("F") || ((EditText)Formulario.Items.Item("txtTipAut").Specific).String.Equals("G") || ((EditText)Formulario.Items.Item("txtTipAut").Specific).String.Equals("H"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el numero de autorizacion sea un valor numerico de 11 caracteres
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoNumeroAutorizacion()
        {
            if (Regex.IsMatch(((EditText)Formulario.Items.Item("txtNumAut").Specific).String, Mensaje.expRegNumeroAutorizacion))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que la fecha de emision sea ingresada en un formato correcto
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoFechaEmision()
        {
            //DateTime valor;
            //if (DateTime.TryParse(((EditText)Formulario.Items.Item("txtValDes").Specific).String,  out valor))
            //{
            return true;
            //}

            //return false;
        }

        /// <summary>
        /// Valida que la fecha de vencimiento sea ingresada en un formato correcto
        /// </summary>
        /// <returns></returns>
        public bool ValidarFormatoFechaVencimiento()
        {
            //DateTime valor;

            //string temp = ((EditText)Formulario.Items.Item("txtValHa").Specific).String;

            //if (DateTime.TryParse(temp, out valor))
            //{
            //    return true;
            //}

            return true;
        }

        //Validar que los campos tengan contenido

        /// <summary>
        /// Validar que se haya seleccionado algun tipo d edocumento
        /// </summary>
        /// <returns></returns>
        private bool ValidarTipoDocumento()
        {
            if (((ComboBox)Formulario.Items.Item("cmbTipDoc").Specific).Value.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validar que se haya seleccionado alguna sucursal
        /// </summary>
        /// <returns></returns>
        private bool ValidarSucursal()
        {
            if (((ComboBox)Formulario.Items.Item("cmbSuc").Specific).Value.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para la serie
        /// </summary>
        /// <returns></returns>
        private bool ValidarSerie()
        {
            if (((EditText)Formulario.Items.Item("txtSerie").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para el numero inicial
        /// </summary>
        /// <returns></returns>
        private bool ValidarNumeroInicial()
        {
            if (((EditText)Formulario.Items.Item("txtNumIn").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para el numero final
        /// </summary>
        /// <returns></returns>
        private bool ValidarNumeroFinal()
        {
            if (((EditText)Formulario.Items.Item("txtNumFn").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para el tipo de autorizacion
        /// </summary>
        /// <returns></returns>
        private bool ValidarTipoAutorizacion()
        {
            if (((EditText)Formulario.Items.Item("txtTipAut").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para el numero de autorizacion
        /// </summary>
        /// <returns></returns>
        private bool ValidarNumeroAutorizacion()
        {
            if (((EditText)Formulario.Items.Item("txtNumAut").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para la fecha de emision
        /// </summary>
        /// <returns></returns>
        private bool ValidarFechaEmision()
        {
            if (((EditText)Formulario.Items.Item("txtValDes").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que se haya ingresado un valor para la fecha de vencimiento
        /// </summary>
        /// <returns></returns>
        private bool ValidarFechaVencimiento()
        {
            if (((EditText)Formulario.Items.Item("txtValHa").Specific).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ejecuta todas las validaciones y muestra los mensajes de error necesarios
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            //--------------------------------//
            //********TIPO DE DOCUMENTO*******//
            //--------------------------------//

            //Valor ingresado
            if (!ValidarTipoDocumento())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncTipoDoc, AdminEventosUI.tipoError);
                return false;
            }

            //-----------------------//
            //********SUCURSAL*******//
            //-----------------------//

            //Valor ingresado
            if (!ValidarSucursal())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncSucursal, AdminEventosUI.tipoError);
                return false;
            }

            //-------------------//
            //********CAJA*******//
            //-------------------//

            //Valor ingresado
            if (!ValidarFormatoCaja())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoCaja, AdminEventosUI.tipoError);
                return false;
            }

            //--------------------//
            //********SERIE*******//
            //--------------------//

            //Valor ingresado
            if (!ValidarSerie())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncSerie, AdminEventosUI.tipoError);
                return false;
            }

            //Formato valido
            if (!ValidarFormatoSerie())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoSerie, AdminEventosUI.tipoError);
                return false;
            }

            //-----------------------------//
            //********NUMERO INICIAL*******//
            //-----------------------------//

            //Valor ingresado
            if (!ValidarNumeroInicial())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncNumInicial, AdminEventosUI.tipoError);
                return false;
            }

            //Formato correcto
            if (!ValidarFormatoNumeroInicial())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoNumInicial, AdminEventosUI.tipoError);
                return false;
            }

            //---------------------------//
            //********NUMERO FINAL*******//
            //---------------------------//

            //Valor ingresado
            if (!ValidarNumeroFinal())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncNumFinal, AdminEventosUI.tipoError);
                return false;
            }

            //Formato correcto
            if (!ValidarFormatoNumeroFinal())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoNumFinal, AdminEventosUI.tipoError);
                return false;
            }

            //-----------------------------------//
            //********TIPO DE AUTORIZACION*******//
            //-----------------------------------//

            //Valor ingresado
            if (!ValidarTipoAutorizacion())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncTipoAutorizacion, AdminEventosUI.tipoError);
                return false;
            }

            //Formato correcto
            if (!ValidarFormatoTipoAutorizacion())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoTipoAutorizacion, AdminEventosUI.tipoError);
                return false;
            }

            //-------------------------------------//
            //********NUMERO DE AUTORIZACION*******//
            //-------------------------------------//

            //Valor ingresado
            if (!ValidarNumeroAutorizacion())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncNumAutorizacion, AdminEventosUI.tipoError);
                return false;
            }

            //Formato correcto
            if (!ValidarFormatoNumeroAutorizacion())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoNumAutorizacion, AdminEventosUI.tipoError);
                return false;
            }

            //-------------------------------//
            //********FECHA DE EMISION*******//
            //-------------------------------//

            //Valor ingresado
            if (!ValidarFechaEmision())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncFechaEmision, AdminEventosUI.tipoError);
                return false;
            }

            //Formato correcto
            if (!ValidarFormatoFechaEmision())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoFechaEmision, AdminEventosUI.tipoError);
                return false;
            }

            //-----------------------------------//
            //********FECHA DE VENCIMIENTO*******//
            //-----------------------------------//

            //Valor ingresado
            if (!ValidarFechaVencimiento())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncFechaVencimiento, AdminEventosUI.tipoError);
                return false;
            }

            //Formato correcto
            if (!ValidarFormatoFechaVencimiento())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoFechaVencimiento, AdminEventosUI.tipoError);
                return false;
            }
            return true;
        }

        #endregion VALIDACIONES

        #region MANTENIMIENTO

        /// <summary>
        /// Toma los valores del formulario para almacenarlos en la base de datos.
        /// <para>0=Valores ingresados.</para>
        /// <para>1=Valores no ingresados.</para>
        /// <para>2=Valores no validos</para>
        /// <para>3=Rango no ingresado</para>
        /// </summary>
        /// <returns></returns>
        public int Almacenar()
        {
            bool resultado;
            string idCae;

            if (Validar())
            {
                //Crea un nuevo objeto CAE
                CAE cae = new CAE();

                //Obtiene el valor del tipo y el nombre del documento según la seleccion en el combo box
                cae.TipoCFE = CAE.ObtenerTipoCFECFC(((ComboBox)Formulario.Items.Item("cmbTipDoc").Specific).Selected.Value);
                cae.NombreDocumento = CAE.ObtenerNombreCFECFC(((ComboBox)Formulario.Items.Item("cmbTipDoc").Specific).Selected.Value);

                //Obtiene el nombre de la sucursal selccionada
                cae.Sucursal = ((ComboBox)Formulario.Items.Item("cmbSuc").Specific).Selected != null ? ((ComboBox)Formulario.Items.Item("cmbSuc").Specific).Selected.Value : "";

                //Establece los otros valores del objeto cae con los valores ingresados en los campos de texto
                cae.Caja = ((EditText)Formulario.Items.Item("txtCaja").Specific).String;
                cae.Serie = ((EditText)Formulario.Items.Item("txtSerie").Specific).String;
                cae.NumeroDesde = int.Parse(((EditText)Formulario.Items.Item("txtNumIn").Specific).String);
                cae.NumeroHasta = int.Parse(((EditText)Formulario.Items.Item("txtNumFn").Specific).String);
                cae.TipoAutorizacion = ((EditText)Formulario.Items.Item("txtTipAut").Specific).String;
                cae.NumeroAutorizacion = ((EditText)Formulario.Items.Item("txtNumAut").Specific).String;
                cae.FechaEmision = ((EditText)Formulario.Items.Item("txtValDes").Specific).String;
                cae.FechaVencimiento = ((EditText)Formulario.Items.Item("txtValHa").Specific).String;

                //Crea nueva instancia de mantenimiento de CAE,s
                ManteUdoCAE manteUdoCae = new ManteUdoCAE();

                resultado = manteUdoCae.Almacenar(cae, out idCae);

                if (resultado)
                {
                    //Crear una nueva instancia de mantenimiento de rangos
                    ManteUdoRango manteUdoRango = new ManteUdoRango();

                    //Creaer un nuevo objeto rango
                    Rango rango = new Rango(cae.TipoCFE, cae.NumeroDesde, cae.NumeroHasta, cae.NumeroDesde, cae.Serie, cae.FechaEmision, cae.FechaVencimiento, idCae, "N");

                    //Alamcenar Rango en tabla de rangos
                    if (manteUdoRango.Almacenar(rango))
                    {
                        return 0;//Valores ingresados
                    }
                    else
                    {
                        return 3;//Rango no ingresado
                    }
                }
                else
                {
                    return 1;//Valores no ingresados
                }
            }

            return 2;//Valores no validos
        }

        /// <summary>
        /// Consultar las sucursales configuradas en B1
        /// </summary>
        /// <returns></returns>
        public Sucursal[] ConsultarListaSucursales()
        {
            //Crea nueva instancia de mantenimiento de CAE,s
            ManteUdoCAE manteUdoCae = new ManteUdoCAE();

            return manteUdoCae.ConsultarListaSucursales();
        }

        #endregion MANTENIMIENTO
    }
}
