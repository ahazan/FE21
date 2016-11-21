using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.DocumentosB1;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Metodos_FTP;
using System.Text.RegularExpressions;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene las operaciones para modificar y manipular el formulario estandar de facturacion
    /// </summary>
    class FrmDocumento : FrmBase
    {
        //Variables visible en toda la clase
        private Item itemReferencia;
        private Item stNumCFE;
        private Item txtNumCFE;
        private Item txtSerieCFE;
        private Item stNumRef;
        private Item txtNumRef;
        private Item txtSerieRef;
        private Item itemAreaTexto;
        private Item stTipoDocumento;
        private Item cbxTipoDocumento;
        private Item txtTipoDocumento;
        private Item cbDocElectronico;
        private Item cbxFormaPago;
        private Item lblFormaPago;
        private Item btnImprimir;
        private Item stRazonReferencia;
        private Item cbxRazonReferencia;

        private Item txtClauVen;
        private Item stClauVen;
        private Item cbxModVen;
        private Item stModVen;

        #region FE_EXPORTACION

        private Item cbxViaTransporte;
        private Item lblViaTransporte;
        private Item cbxTipoBienes;
        private Item lblTipoBienes;
        
        #endregion FE_EXPORTACION

        private UserDataSource udsCbxTipoDocumento;
        private UserDataSource udsTxtTipoDocumento;
        private UserDataSource udsCbDocEletrocnico;
        private UserDataSource udsCbDocContingecia;
        private UserDataSource udsCbxRazonReferencia;
        private DBDataSource   dbCbxFormaPago;

        #region FE_EXPORTACION

        private UserDataSource udsStClauVen;        //Cláusula de Venta
        private UserDataSource udsCbxViaTransporte; // Vía de Transporte
        private UserDataSource udsCbxTipoBienes;    //Indicador Tipo de Bienes
        private UserDataSource udsStModVen;         //Modalidad de Venta

        #endregion FE_EXPORTACION

        private Adenda adenda = new Adenda();

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        /// <summary>
        /// Crea los componentes nuevos al formulario
        /// </summary>
        /// <param name="formUID"></param>
        public void CrearComponentes(string formUID, string formTypeEx)
        {
            try
            {

                IdFormulario = formUID;
                TypeEx = formTypeEx;

                //Obtiene el formulario
                ObtenerFormulario(formUID);
                if (!formTypeEx.Equals("141") || !formTypeEx.Equals("181"))
                {
                    //Crear los campos de serie y numero del rango de CFE
                    CrearCamposCFE();
                }

               
                //Si el documento es nota de debitos se debe agregar campos de información de referencia
                if (formTypeEx.Equals("65303") || formTypeEx.Equals("179"))
                {
                    CrearCamposReferencia();
                }

                //if (!formTypeEx.Equals("141"))
                //{ 
                //Agregar el nuevo tab para la adenda
                AgregarTabAdenda();
                //Crea el campo de texto para agregar la adenda
                CrearAreaTextoAdenda();
                //}

                //Crea los campos de exportacion
                CreaerCamposExportacion();
                //Agrega los controles para seleccionar y establecer el valor para el tipo de documento
                CrearControlesDocumentoReceptor();
                //Agregar los check box para los tipos de documento
                CrearCheckBoxTipoDocumento();

                if (!formTypeEx.Equals("140"))
                {
                    //Agregar ComboBox para forma de pago
                    CrearCamposFormaPago();
                }
                //Crea los data sources
                AgregarDataSources();
                //Carga los valores disponibles para los tipos de documentos
                CargarValoresIniciales();
                //Establece el data bind a los distintos compentes
                EstablecerDataBind(formTypeEx);
                //Selecciona campo Socio de Negocio
                SeleccionarCampoSocioNegocio();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("error: " + ex.ToString());
            }
        }

        /// <summary>
        /// Crear los campos de serie y numero del rango de CFE
        /// </summary>
        private void CrearCamposCFE()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Obtener item de referencia
            itemReferencia = Formulario.Items.Item("86");

            //Crear etiqueta de número y serie de CFE y establcer propiedades
            stNumCFE = Formulario.Items.Add("stCFE", BoFormItemTypes.it_STATIC);
            ((StaticText)stNumCFE.Specific).Caption = "Serie y Número de CFE";
            stNumCFE.Left = itemReferencia.Left;
            stNumCFE.Top = itemReferencia.Top + itemReferencia.Height + 1;
            stNumCFE.Width = itemReferencia.Width;
            stNumCFE.Height = itemReferencia.Height;

            //Obtener item de referencia
            itemReferencia = Formulario.Items.Item("46");

            //Crear campo de texto de serie de CFE
            txtSerieCFE = Formulario.Items.Add("txtSeCFE", BoFormItemTypes.it_EDIT);
            txtSerieCFE.Left = itemReferencia.Left;
            txtSerieCFE.Top = itemReferencia.Top + itemReferencia.Height + 1;
            txtSerieCFE.Width = 20;
            txtSerieCFE.Height = itemReferencia.Height;
            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
            {
                txtSerieCFE.Enabled = true;
            }
            else
            {
                txtSerieCFE.Enabled = false;
            }
            txtSerieCFE.AffectsFormMode = false;

            //Crear campo de texto de numero de CFE
            txtNumCFE = Formulario.Items.Add("txtNumCFE", BoFormItemTypes.it_EDIT);
            txtNumCFE.Left = txtSerieCFE.Left + txtSerieCFE.Width + 1;
            txtNumCFE.Top = itemReferencia.Top + itemReferencia.Height + 1;
            txtNumCFE.Width = itemReferencia.Width - txtSerieCFE.Width - 1;
            txtNumCFE.Height = itemReferencia.Height;
            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
            {
                txtNumCFE.Enabled = true;
            }
            else
            {
                txtNumCFE.Enabled = false;
            }

            txtNumCFE.AffectsFormMode = false;

            //Ligar etiqueta con campo serie
            stNumCFE.LinkTo = "txtSeCFE";

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crear los campos de serie y numero del rango del CFE de referencia
        /// </summary>
        private void CrearCamposReferencia()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Obtener item de referencia
            itemReferencia = Formulario.Items.Item("stCFE");

            //Crear etiqueta de número y serie de CFE y establcer propiedades
            stNumRef = Formulario.Items.Add("stRef", BoFormItemTypes.it_STATIC);
            ((StaticText)stNumRef.Specific).Caption = "Referencia";
            stNumRef.Left = itemReferencia.Left;
            stNumRef.Top = itemReferencia.Top + itemReferencia.Height + 1;
            stNumRef.Width = itemReferencia.Width;
            stNumRef.Height = itemReferencia.Height;

            //Obtener item de referencia
            itemReferencia = Formulario.Items.Item("txtSeCFE");

            //Crear campo de texto de serie de CFE
            txtSerieRef = Formulario.Items.Add("txtSeRef", BoFormItemTypes.it_EDIT);
            txtSerieRef.Left = itemReferencia.Left;
            txtSerieRef.Top = itemReferencia.Top + itemReferencia.Height + 1;
            txtSerieRef.Width = 20;
            txtSerieRef.Height = itemReferencia.Height;
            txtSerieRef.Enabled = true;
            txtSerieRef.AffectsFormMode = false;

            //Obtener item de referencia
            itemReferencia = Formulario.Items.Item("txtNumCFE");

            //Crear campo de texto de numero de CFE
            txtNumRef = Formulario.Items.Add("txtNumRef", BoFormItemTypes.it_EDIT);
            txtNumRef.Left = txtSerieRef.Left + txtSerieRef.Width + 1;
            txtNumRef.Top = txtSerieRef.Top;
            txtNumRef.Width = itemReferencia.Width;
            txtNumRef.Height = itemReferencia.Height;
            txtNumRef.Enabled = true;
            txtNumRef.AffectsFormMode = false;


            itemReferencia = Formulario.Items.Item("txtSeRef");

            cbxRazonReferencia = Formulario.Items.Add("cbxRazRef", BoFormItemTypes.it_COMBO_BOX);
            cbxRazonReferencia.Left = itemReferencia.Left;
            cbxRazonReferencia.Top = itemReferencia.Top + 15;
            cbxRazonReferencia.Width = 137;
            cbxRazonReferencia.Enabled = true;


            //Ligar etiqueta con campo serie
            stNumRef.LinkTo = "txtSeRef";

            itemReferencia = Formulario.Items.Item("stRef");

            stRazonReferencia = Formulario.Items.Add("stRazRef", BoFormItemTypes.it_STATIC);
            stRazonReferencia.Left = itemReferencia.Left;
            stRazonReferencia.Height = itemReferencia.Height;
            stRazonReferencia.Width = itemReferencia.Width;
            stRazonReferencia.Top = itemReferencia.Top + 15;
            stRazonReferencia.LinkTo = "cbxRazRef";
            stRazonReferencia.Enabled = true;

            ((StaticText)Formulario.Items.Item("stRazRef").Specific).Caption = "Razón Referencia";

            LLenarCbxRazonReferencia();
           
            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crea un nuevo tab para agregar la adenda
        /// </summary>
        private void AgregarTabAdenda()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Crear un nuevo tab para adenda
            Item itemTabAdenda = Formulario.Items.Add("tabAdn", BoFormItemTypes.it_FOLDER);

            //Obtener tab estandar de finanzas para agrupar el nuevo tab de adenda
            Item itemTabFinanzas = Formulario.Items.Item("138");

            //Establecer propidedades al nuevo tab de adenda respecto al tab de finanzas
            itemTabAdenda.Top = itemTabFinanzas.Top;
            itemTabAdenda.Left = itemTabFinanzas.Left;
            itemTabAdenda.Height = itemTabFinanzas.Height;
            itemTabAdenda.Width = 300;

            //Establecer propiedades especificas al tab de adenta
            ((Folder)itemTabAdenda.Specific).Caption = "Adenda";
            ((Folder)itemTabAdenda.Specific).Pane = 26;

            //Agrupar el nuevo tab de adenda con el tab de finanzas
            ((Folder)itemTabAdenda.Specific).GroupWith("138");

            //Establecer nivel de pane
            Formulario.PaneLevel = 1;

            //Desongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crea el area de texto donde se ingresara la adenda
        /// </summary>
        private void CrearAreaTextoAdenda()
        {
            //Se congelar el formulario
            Formulario.Freeze(true);

            //Obtener item de referencia
            Item itemReferencia = Formulario.Items.Item("62");

            //Crea el area de texto
            itemAreaTexto = Formulario.Items.Add("txtAdn", BoFormItemTypes.it_EXTEDIT);

            //Establece las propiedades al area de texto
            itemAreaTexto.Top = itemReferencia.Top;
            itemAreaTexto.Left = itemReferencia.Left;
            itemAreaTexto.Width = Formulario.Width - 58;
            itemAreaTexto.Height = 200;
            itemAreaTexto.ToPane = 26;
            itemAreaTexto.FromPane = 26;

            //Se descongelar el formulario
            //Formulario.Freeze(true);
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crea los campos para los cfe de exportacion
        /// </summary>
        private void CreaerCamposExportacion()
        {
            //Se congelar el formulario
            Formulario.Freeze(true);

            //Clausula de venta
            itemReferencia = Formulario.Items.Item("121");

            txtClauVen = Formulario.Items.Add("txtClaVen", BoFormItemTypes.it_EDIT);
            txtClauVen.Left = itemReferencia.Left;
            txtClauVen.Top = itemReferencia.Top + itemReferencia.Height + 1;
            txtClauVen.Width = itemReferencia.Width;
            txtClauVen.Height = itemReferencia.Height;
            txtClauVen.ToPane = 6;
            txtClauVen.FromPane = 6;

            itemReferencia = Formulario.Items.Item("122");
            stClauVen = Formulario.Items.Add("lbClaVen", BoFormItemTypes.it_STATIC);
            stClauVen.Left = itemReferencia.Left;
            stClauVen.Top = itemReferencia.Top + itemReferencia.Height + 1;
            stClauVen.Width = itemReferencia.Width;
            stClauVen.Height = itemReferencia.Height;
            stClauVen.ToPane = 6;
            stClauVen.FromPane = 6;

            ((StaticText)stClauVen.Specific).Caption = "Cláusula de Venta";
            stClauVen.LinkTo = "txtClaVen";

            //Modalidad de Venta
            itemReferencia = Formulario.Items.Item("txtClaVen");

            cbxModVen = Formulario.Items.Add("cbxModVen", BoFormItemTypes.it_COMBO_BOX);
            cbxModVen.Left = itemReferencia.Left;
            cbxModVen.Top = itemReferencia.Top + itemReferencia.Height + 1;
            cbxModVen.Width = itemReferencia.Width;
            cbxModVen.Height = itemReferencia.Height;
            cbxModVen.ToPane = 6;
            cbxModVen.FromPane = 6;

            itemReferencia = Formulario.Items.Item("lbClaVen");
            stModVen = Formulario.Items.Add("lbModVen", BoFormItemTypes.it_STATIC);
            stModVen.Left = itemReferencia.Left;
            stModVen.Top = itemReferencia.Top + itemReferencia.Height + 1;
            stModVen.Width = itemReferencia.Width;
            stModVen.Height = itemReferencia.Height;
            stModVen.ToPane = 6;
            stModVen.FromPane = 6;

            ((StaticText)stModVen.Specific).Caption = "Modalidad de Venta";
            stModVen.LinkTo = "cbxModVen";

            #region FE_EXPORTACION
            
            lblViaTransporte = AgregarControl(lblViaTransporte, "lbModVen", "lbViaTran", BoFormItemTypes.it_STATIC, 6, 6);
            cbxViaTransporte = AgregarControl(cbxViaTransporte, "cbxModVen", "cbxViaTran", BoFormItemTypes.it_COMBO_BOX, 6, 6);
            CrearEtiqueta(lblViaTransporte, "Vía de Transporte", "cbxViaTran");

            lblTipoBienes = AgregarControl(lblTipoBienes, "lbViaTran", "lbTipBien", BoFormItemTypes.it_STATIC, 6, 6);
            cbxTipoBienes = AgregarControl(cbxTipoBienes, "cbxViaTran", "cbxBienes", BoFormItemTypes.it_COMBO_BOX, 6, 6);
            CrearEtiqueta(lblTipoBienes, "Ind. Tipo de Bienes", "cbxBienes");
            
            #endregion FE_EXPORTACION

            //Se descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Agrega los componentes adicionales al formulario estandar de socio de negocios.
        /// </summary>
        private void CrearControlesDocumentoReceptor()
        {
            //Congela el formulario
            Formulario.Freeze(true);

            //Obtener item de referencia para la etiqueta de tipo de documento
            itemReferencia = Formulario.Items.Item("70");

            //Crear un nuevo static text para los tipos de documentos y establecer propiedades
            stTipoDocumento = Formulario.Items.Add("stTipDoc", BoFormItemTypes.it_STATIC);

            stTipoDocumento.Width = itemReferencia.Width;
            stTipoDocumento.Height = itemReferencia.Height;
            stTipoDocumento.Left = itemReferencia.Left;
            stTipoDocumento.Top = itemReferencia.Top + itemReferencia.Height + 1;
            ((StaticText)stTipoDocumento.Specific).Caption = "Documento Receptor";

            //Obtener item de referencia para el combo box de tipos de documentos
            itemReferencia = Formulario.Items.Item("14");

            //Crear un nuevo combo box para los tipos de documentos y establecer propiedades
            cbxTipoDocumento = Formulario.Items.Add("cbxTipDoc", BoFormItemTypes.it_COMBO_BOX);

            cbxTipoDocumento.Width = itemReferencia.Width;
            cbxTipoDocumento.Height = itemReferencia.Height;
            cbxTipoDocumento.Left = itemReferencia.Left;
            cbxTipoDocumento.Top = stTipoDocumento.Top;

            //Ligar etiquita con combo box
            stTipoDocumento.LinkTo = "cbxTipDoc";

            //Obtener item de referencia para el combo box de tipos de documentos
            itemReferencia = Formulario.Items.Item("cbxTipDoc");

            //Crear un nuevo campo de texto para el valor de los tipos de documentos y establecer propiedades
            txtTipoDocumento = Formulario.Items.Add("txtTipDoc", BoFormItemTypes.it_EDIT);

            txtTipoDocumento.Width = itemReferencia.Width;
            txtTipoDocumento.Height = itemReferencia.Height;
            txtTipoDocumento.Left = itemReferencia.Left + itemReferencia.Width + 1;
            txtTipoDocumento.Top = stTipoDocumento.Top;
            //txtTipoDocumento.Enabled = false;
            txtTipoDocumento.AffectsFormMode = true;

            //Ligar combo box con campo de texto
            cbxTipoDocumento.LinkTo = "txtTipDoc";

            //Descongela el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crea el combobox de forma de pago
        /// </summary>
        private void CrearCamposFormaPago()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Crea el check box para tipo de documento electronico
            cbxFormaPago = Formulario.Items.Add("cbxForPag", BoFormItemTypes.it_COMBO_BOX);
            cbxFormaPago.Top = stTipoDocumento.Top + stTipoDocumento.Height + 15;
            cbxFormaPago.Left = stTipoDocumento.Left + 70;
            cbxFormaPago.Width = 70;
            ((ComboBox)cbxFormaPago.Specific).ValidValues.Add("Crédito", "Crédito");
            ((ComboBox)cbxFormaPago.Specific).ValidValues.Add("Contado", "Contado");

            ManteUdoFormaPago manteUdoFormaPago = new ManteUdoFormaPago();
            string formaPago = manteUdoFormaPago.ObtenerDocEntryFormaPago(false);

            if (!formaPago.Equals(""))
            {
                ((ComboBox)cbxFormaPago.Specific).Select(formaPago, BoSearchKey.psk_ByValue);
            }

            cbxFormaPago.Visible = true;

            lblFormaPago = Formulario.Items.Add("lblForPag", BoFormItemTypes.it_STATIC);
            lblFormaPago.Top = stTipoDocumento.Top + stTipoDocumento.Height + 15;
            lblFormaPago.Left = stTipoDocumento.Left;
            lblFormaPago.Width = 70;

            ((StaticText)lblFormaPago.Specific).Caption = "Forma Pago";

            lblFormaPago.Visible = true;
            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Agrega los check box para indicar si es un tipo de documento electro o de contingencia. Si es contingecia siempre será electronico
        /// </summary>
        private void CrearCheckBoxTipoDocumento()
        {
            ManteUdoUsuarios manteUdoUsuario = new ManteUdoUsuarios();

            //Congelar el formulario
            Formulario.Freeze(true);

            //Crea el check box para tipo de documento electronico
            cbDocElectronico = Formulario.Items.Add("cbElc", BoFormItemTypes.it_CHECK_BOX);
            ((CheckBox)cbDocElectronico.Specific).Caption = "Documento Electrónico";
            cbDocElectronico.Top = stTipoDocumento.Top + stTipoDocumento.Height + 1;
            cbDocElectronico.Left = stTipoDocumento.Left;
            cbDocElectronico.Width = 130;
            ((CheckBox)cbDocElectronico.Specific).ValOn = "Y";
            ((CheckBox)cbDocElectronico.Specific).ValOff = "N";
           // ((CheckBox)cbDocElectronico.Specific).Checked = true;

         
            cbDocElectronico.Enabled = true;
          
            //if (!manteUdoUsuario.ConsultarAutorizacion())
            //{
                //cbDocElectronico.Visible = false;
            ((CheckBox)cbDocElectronico.Specific).Checked = true;

            //}

            //if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
            //{
            //    //cbDocElectronico.Visible = false;
            //}
            //else
            //{
            //    cbDocElectronico.Visible = true;
            //}

            Item referenciaCancelar = Formulario.Items.Item("2");

            btnImprimir = Formulario.Items.Add("btnImp", BoFormItemTypes.it_BUTTON);
            ((Button)btnImprimir.Specific).Caption = "Reimprimir";
            btnImprimir.Top = referenciaCancelar.Top;
            btnImprimir.Left = referenciaCancelar.Left + referenciaCancelar.Width + 5;
            btnImprimir.Width = 65;
            btnImprimir.Enabled = false;
            //btnImprimir.Visible = false;

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Agregar los user data sources para el tipo de documento y valor del tipo de documento
        /// </summary>
        protected override void AgregarDataSources()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Crea un nuevo user data source para los tipos de documentos
            udsCbxTipoDocumento = Formulario.DataSources.UserDataSources.Add("udsCbxTD", BoDataType.dt_SHORT_TEXT, 10);

            //Crea un nuevo user data source para el valor de los tipos de documentos
            //udsTxtTipoDocumento = Formulario.DataSources.UserDataSources.Add("udsTxtTD", BoDataType.dt_SHORT_TEXT, 100);

            //Crea un nuevo user data source para el check box de tipo de documento electronico
            udsCbDocEletrocnico = Formulario.DataSources.UserDataSources.Add("udsCbDE", BoDataType.dt_SHORT_TEXT, 1);

            //Crea un nuevo user data source para el check box de documento de contingencia
            udsCbDocContingecia = Formulario.DataSources.UserDataSources.Add("udsCbDC", BoDataType.dt_SHORT_TEXT, 1);

            udsCbxRazonReferencia = Formulario.DataSources.UserDataSources.Add("udsCbRR", BoDataType.dt_SHORT_TEXT, 100);

            Formulario.Items.Item("cbElc").Enabled = false;

            #region FE_EXPORTACION
            udsCbxTipoBienes    = Formulario.DataSources.UserDataSources.Add("udsCbBien", BoDataType.dt_SHORT_TEXT, 50);
            udsStClauVen        = Formulario.DataSources.UserDataSources.Add("udsStCVen", BoDataType.dt_SHORT_TEXT, 50);
            udsCbxViaTransporte = Formulario.DataSources.UserDataSources.Add("udsViaTran", BoDataType.dt_SHORT_TEXT, 50);
            udsStModVen         = Formulario.DataSources.UserDataSources.Add("udsModVen", BoDataType.dt_SHORT_TEXT, 50);
            #endregion FE_EXPORTACION

            /*if (AdminEventosUI.modoUsuario)
            {
             //   Formulario.Items.Item("cbElc").Enabled = false;
            }
            else
            {
               // Formulario.Items.Item("cbElc").Enabled = false;
            }*/

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Establece el DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Establece el data bind a los distintos componentes
        /// </summary>
        public void EstablecerDataBind(string formTypeEx)
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Estable el data bind al combo box de tipo de documento
            ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "", "udsCbxTD");

            //Establece el data bind al edit text del valor del tipo de documento
            //((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "", "udsTxtTD");

            //Estblece el data bind a los componentes segun el numero de formulario
            switch (formTypeEx)
            {
                case "133": //Factura
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "OINV", "U_TipDocFA");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "OINV", "U_ValDocFA");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "OINV", "U_DocEleFA");//Documento electronico
                    //((CheckBox)cbDocContingencia.Specific).DataBind.SetBound(true, "OINV", "U_DocContFA");//Documento de contingencia
                    ((EditText)txtSerieCFE.Specific).DataBind.SetBound(true, "OINV", "U_SerieFA");//Serie
                    ((EditText)txtNumCFE.Specific).DataBind.SetBound(true, "OINV", "U_NumeroFA");//Numero CFE
                    ((EditText)itemAreaTexto.Specific).DataBind.SetBound(true, "OINV", "U_Adenda"); //Adenda
                    ((EditText)txtClauVen.Specific).DataBind.SetBound(true, "OINV", "U_ClaVenFA"); //Clausula de venta
                    ((ComboBox)cbxModVen.Specific).DataBind.SetBound(true, "OINV", "U_ModVenFA");//Modalidad de venta

                    #region FE_EXPORTACION
                    ((ComboBox)cbxViaTransporte.Specific).DataBind.SetBound(true, Constantes.TablaFactura, Constantes.UDFViaTransporteFA); //Via de transporte
                    ((ComboBox)cbxTipoBienes.Specific).DataBind.SetBound(true, Constantes.TablaFactura, Constantes.UDFIndTipoDeBienes);  //Indicador Tipo de Bienes
                    #endregion FE_EXPORTACION

                    break;
                case "60091": //Factura de Reserva
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "OINV", "U_TipDocFA");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "OINV", "U_ValDocFA");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "OINV", "U_DocEleFA");//Documento electronico
                    //((CheckBox)cbDocContingencia.Specific).DataBind.SetBound(true, "OINV", "U_DocContFA");//Documento de contingencia
                    ((EditText)txtSerieCFE.Specific).DataBind.SetBound(true, "OINV", "U_SerieFA");//Serie
                    ((EditText)txtNumCFE.Specific).DataBind.SetBound(true, "OINV", "U_NumeroFA");//Numero CFE
                    ((EditText)itemAreaTexto.Specific).DataBind.SetBound(true, "OINV", "U_Adenda"); //Adenda
                    ((EditText)txtClauVen.Specific).DataBind.SetBound(true, "OINV", "U_ClaVenFA"); //Clausula de venta
                    ((ComboBox)cbxModVen.Specific).DataBind.SetBound(true, "OINV", "U_ModVenFA");//Modalidad de venta

                    #region FE_EXPORTACION
                    ((ComboBox)cbxViaTransporte.Specific).DataBind.SetBound(true, Constantes.TablaFactura, Constantes.UDFViaTransporteFA); //Via de transporte
                    ((ComboBox)cbxTipoBienes.Specific).DataBind.SetBound(true, Constantes.TablaFactura, Constantes.UDFIndTipoDeBienes);  //Indicador Tipo de Bienes
                    #endregion FE_EXPORTACION

                    break;
                case "179": //Notas de credito
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "ORIN", "U_TipDocNC");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "ORIN", "U_ValDocNC");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "ORIN", "U_DocEleNC");//Documento electronico
                    //((CheckBox)cbDocContingencia.Specific).DataBind.SetBound(true, "ORIN", "U_DocContNC");//Documento de contingencia
                    ((EditText)txtSerieCFE.Specific).DataBind.SetBound(true, "ORIN", "U_SerieNC");//Serie
                    ((EditText)txtNumCFE.Specific).DataBind.SetBound(true, "ORIN", "U_NumeroNC");//Numero CFE
                    ((EditText)itemAreaTexto.Specific).DataBind.SetBound(true, "ORIN", "U_Adenda");//Adenda
                    ((EditText)txtClauVen.Specific).DataBind.SetBound(true, "ORIN", "U_ClaVenNC"); //Clausula de venta
                    ((ComboBox)cbxModVen.Specific).DataBind.SetBound(true, "ORIN", "U_ModVenNC");//Modalidad de venta
                    ((EditText)txtSerieRef.Specific).DataBind.SetBound(true, "ORIN", "U_SerieRefNC");//Serie Referencia
                    ((EditText)txtNumRef.Specific).DataBind.SetBound(true, "ORIN", "U_NumRefNC");//Numero Referencia
                    ((ComboBox)cbxRazonReferencia.Specific).DataBind.SetBound(true, "ORIN", "U_RazRef");//Razon Referencia

                    #region FE_EXPORTACION
                    ((ComboBox)cbxViaTransporte.Specific).DataBind.SetBound(true, Constantes.TablaNC, Constantes.UDFViaTransporteNC); //Via de transporte
                    ((ComboBox)cbxTipoBienes.Specific).DataBind.SetBound(true, Constantes.TablaNC, Constantes.UDFIndTipoDeBienes);  //Indicador Tipo de Bienes
                    /* EXPORTACION */
                    #endregion FE_EXPORTACION
                    
                    break;
                case "140": ////Remito
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "ODLN", "U_TipDocRM");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "ODLN", "U_ValDocRM");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "ODLN", "U_DocEleRM");//Documento electronico
                    //((CheckBox)cbDocContingencia.Specific).DataBind.SetBound(true, "ODLN", "U_DocContRM");//Documento de contingencia
                    ((EditText)txtSerieCFE.Specific).DataBind.SetBound(true, "ODLN", "U_SerieRM");//Serie
                    ((EditText)txtNumCFE.Specific).DataBind.SetBound(true, "ODLN", "U_NumeroRM");//Numero CFE
                    ((EditText)itemAreaTexto.Specific).DataBind.SetBound(true, "ODLN", "U_Adenda");//Adenda
                    ((EditText)txtClauVen.Specific).DataBind.SetBound(true, "ODLN", "U_ClaVenRM"); //Clausula de venta
                    ((ComboBox)cbxModVen.Specific).DataBind.SetBound(true, "ODLN", "U_ModVenRM");//Modalidad de venta

                    #region FE_EXPORTACION
                    ((ComboBox)cbxViaTransporte.Specific).DataBind.SetBound(true, Constantes.TablaRemito, Constantes.UDFViaTransporteRM); //Via de transporte
                    ((ComboBox)cbxTipoBienes.Specific).DataBind.SetBound(true, Constantes.TablaRemito, Constantes.UDFIndTipoDeBienes);  //Indicador Tipo de Bienes
                    #endregion FE_EXPORTACION

                    break;
                case "65303": ////Notas de debito
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "OINV", "U_TipDocND");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "OINV", "U_ValDocND");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "OINV", "U_DocEleND");//Documento electronico
                    //((CheckBox)cbDocContingencia.Specific).DataBind.SetBound(true, "OINV", "U_DocContND");//Documento de contingencia
                    ((EditText)txtSerieCFE.Specific).DataBind.SetBound(true, "OINV", "U_SerieND");//Serie
                    ((EditText)txtNumCFE.Specific).DataBind.SetBound(true, "OINV", "U_NumeroND");//Numero CFE
                    ((EditText)itemAreaTexto.Specific).DataBind.SetBound(true, "OINV", "U_Adenda");//Adenda
                    ((EditText)txtSerieRef.Specific).DataBind.SetBound(true, "OINV", "U_SerieRefND");//Serie Referencia
                    ((EditText)txtNumRef.Specific).DataBind.SetBound(true, "OINV", "U_NumRefND");//Numero Referencia
                    ((EditText)txtClauVen.Specific).DataBind.SetBound(true, "OINV", "U_ClaVenND"); //Clausula de venta
                    ((ComboBox)cbxModVen.Specific).DataBind.SetBound(true, "OINV", "U_ModVenND");//Modalidad de venta
                    ((EditText)txtSerieRef.Specific).DataBind.SetBound(true, "OINV", "U_SerieRefND");//Serie Referencia
                    ((EditText)txtNumRef.Specific).DataBind.SetBound(true, "OINV", "U_NumRefND");//Numero Referencia
                    ((ComboBox)cbxRazonReferencia.Specific).DataBind.SetBound(true, "OINV", "U_RazRef");//Razon Referencia

                    #region FE_EXPORTACION
                    ((ComboBox)cbxViaTransporte.Specific).DataBind.SetBound(true, Constantes.TablaND, Constantes.UDFViaTransporteND); //Via de transporte
                    ((ComboBox)cbxTipoBienes.Specific).DataBind.SetBound(true, Constantes.TablaND, Constantes.UDFIndTipoDeBienes);  //Indicador Tipo de Bienes
                    #endregion FE_EXPORTACION

                    break;
                case "141": ///Resguardo Compras
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "OPCH", "U_TipDocFA");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "OPCH", "U_ValDocFA");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "OPCH", "U_DocEleFA");//Documento electronico
                    break;

                case "181": ///Resguardo Compras
                    ((ComboBox)cbxTipoDocumento.Specific).DataBind.SetBound(true, "ORPC", "U_TipDocNC");//Tipo de documento
                    ((EditText)txtTipoDocumento.Specific).DataBind.SetBound(true, "ORPC", "U_ValDocNC");//Valor del tipo de documento
                    ((CheckBox)cbDocElectronico.Specific).DataBind.SetBound(true, "ORPC", "U_DocEleNC");//Documento electronico
                    break;
            }

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Carga el combo box con los tipos de documentos permitidos y estblece los valores por defecto a los check box
        /// </summary>
        private void CargarValoresIniciales()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Carga los valores para el combo box
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("RUT", "");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("C.I.", "");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("Otros", "");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("Pasaporte", "");
            ((ComboBox)cbxTipoDocumento.Specific).ValidValues.Add("DNI", "");

            ((ComboBox)cbxModVen.Specific).ValidValues.Add("1", "Régimen General");
            ((ComboBox)cbxModVen.Specific).ValidValues.Add("2", "Consignación");
            ((ComboBox)cbxModVen.Specific).ValidValues.Add("3", "Precio Revisable");
            ((ComboBox)cbxModVen.Specific).ValidValues.Add("4", "Bienes Propios a Exclaves Aduaneros");
            ((ComboBox)cbxModVen.Specific).ValidValues.Add("90", "Régimen General Exportación de Servicios");
            ((ComboBox)cbxModVen.Specific).ValidValues.Add("99", "Otras Transacciones");

            #region FE_EXPORTACION
            //Carga los valores para el combo de Vía de Transporte
            ((ComboBox)cbxViaTransporte.Specific).ValidValues.Add("1", "Marítimo");
            ((ComboBox)cbxViaTransporte.Specific).ValidValues.Add("2", "Aéreo");
            ((ComboBox)cbxViaTransporte.Specific).ValidValues.Add("3", "Terrestre");
            ((ComboBox)cbxViaTransporte.Specific).ValidValues.Add("8", "N/A");
            ((ComboBox)cbxViaTransporte.Specific).ValidValues.Add("9", "Otro");

            //Carga los valores para el combo de Ind. Tipo de Bienes
            ((ComboBox)cbxTipoBienes.Specific).ValidValues.Add("1", "Venta");
            ((ComboBox)cbxTipoBienes.Specific).ValidValues.Add("2", "Traslados Internos");
            #endregion FE_EXPORTACION

            //Establecer valor por defecto en combo box
            udsCbxTipoDocumento.ValueEx = "RUT";

            //Establecer valor por defecto = seleccionado al check box de tipo de documento electronico
            udsCbDocEletrocnico.Value = "Y";

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Invocado cuando se presiona el nuevo tab
        /// </summary>
        public void SeleccionarTabAdenda()
        {
            //Cambia el nivel del panel al del nuevo tab
            Formulario.PaneLevel = 26;
        }

        /// <summary>
        /// Activa o desactiva el campo de texto de valor de documento segun sea la seleccion en el combo box
        /// </summary>
        public void CambiarEstadoValorDocumentoReceptor(string codigo, string Rut)
        {
            if (((ComboBox)cbxTipoDocumento.Specific).Selected.Value != null)
            {
                Formulario.Freeze(true);

                if (!((ComboBox)cbxTipoDocumento.Specific).Selected.Value.Equals("RUT") && !((ComboBox)cbxTipoDocumento.Specific).Selected.Value.Equals("C.I."))
                {
                    //Se borra la cedula juridica por que no es un RUT
                    ((EditText)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtTipDoc").Specific).Value = "";
                }
                else
                {
                    if (!codigo.Equals(""))
                    {
                        //Se asigna la cedula juridica del socio de negocio
                        if (Rut.ToString().Equals(""))

                           {
                            SocioNegocio datosSN = ObtenerDatosSN(codigo);
                            Rut =datosSN.CedulaJuridica;
                            }


                        if (((EditText)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtTipDoc").Specific).Value.ToString().Equals(""))
                        {
                            ((EditText)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Items.Item("txtTipDoc").Specific).Value = Rut; 
                        }
                                               
                    }
                }

                Formulario.Freeze(false);
            }
        }

        /// <summary>
        /// Activa o desactiva los campos de textos de info de referencia
        /// </summary>
        public void CambiarEstadoCamposReferencia(bool estado, string formTypeEx)
        {
            if (formTypeEx.Equals("65303"))
            {
                //Congelar en formulario
                Formulario.Freeze(true);

                txtNumRef.Enabled = estado;
                txtSerieRef.Enabled = estado;

                //Descongelar en formulario
                Formulario.Freeze(false);
            }
        }

        /// <summary>
        /// Activa o desactiva el check box de documento electronico segun sea la seleccion en el check box de contingencia
        /// </summary>
        public void CambiarEstadoDocumentoElectronico()
        {
            cbDocElectronico.Enabled = !cbDocElectronico.Enabled;
        }

        /// <summary>
        /// Bloque los campos de serie y numero de CFE
        /// </summary>
        public void BloquearCamposCFE()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            txtSerieCFE.Enabled = false;
            txtNumCFE.Enabled = false;

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Bloque los campos de serie y numero de CFE
        /// </summary>
        public void DesbloquearCamposCFE()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            txtSerieCFE.Enabled = true;
            txtNumCFE.Enabled = true;

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Establece el campo Socio de Negocio como seleccionado
        /// </summary>
        public void SeleccionarCampoSocioNegocio()
        {            
            try
            {
                Formulario.Freeze(true);
                ((CheckBox)Formulario.Items.Item("cbElc").Specific).Checked = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                Formulario.Freeze(false);
            }
        }

        /// <summary>
        /// Obtiene la cedula juridica de un socio de negocio
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public SocioNegocio ObtenerDatosSN(string codigo)
        {
            Recordset registro = null;
            SocioNegocio resultado = new SocioNegocio();

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);

                #region FE_EXPORTACION
                //registro.DoQuery("SELECT OCRD.LicTradNum, OHEM.lastName, OHEM.middleName, OCRD.U_SNCont FROM OCRD LEFT JOIN OHEM ON OCRD.AgentCode = OHEM.lastName WHERE OCRD.CardCode = '" + codigo + "'");

                //if (registro.RecordCount > 0)
                //{
                //    resultado.CedulaJuridica = registro.Fields.Item("LicTradNum").Value;
                //    resultado.Entregador = registro.Fields.Item("lastName").Value;
                //    resultado.ConsumidorFinal = registro.Fields.Item("U_SNCont").Value;
                //}

                registro.DoQuery("SELECT OCRD.LicTradNum, OHEM.lastName, OHEM.middleName, OCRD.U_SNCont, OCRD.VatStatus FROM OCRD LEFT JOIN OHEM ON OCRD.AgentCode = OHEM.lastName WHERE OCRD.CardCode = '" + codigo + "'");

                if (registro.RecordCount > 0)
                {
                    resultado.CedulaJuridica = registro.Fields.Item("LicTradNum").Value;
                    resultado.Entregador = registro.Fields.Item("lastName").Value;
                    resultado.ConsumidorFinal = registro.Fields.Item("U_SNCont").Value;
                    resultado.ClienteExtranjero = (registro.Fields.Item("VatStatus").Value == "N") ? true : false;
                }
                #endregion FE_EXPORTACION
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    System.GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Valida que se haya seleccionado la opcion de documento electronico
        /// </summary>
        /// <returns></returns>
        public bool ValidarDocumentoElectronico()
        {
            if (cbDocElectronico.Visible)
            {
                if (((CheckBox)cbDocElectronico.Specific).Checked)
                {
                    return true;
                }
                else
                {
                    //if (!AdminEventosUI.modoUsuario)
                    //{
                        return true;
                    //}
                }
            }

            return false;

        }

        #endregion INTERFAZ DE USUARIO

        #region VALIDACIONES

        /// <summary>
        /// Realiza las validaciones de del tipo de documento receptor
        /// </summary>
        /// <returns></returns>
        public bool ValidarDocumentoReceptor()
        {
            //Verifica que el check de documento electronico este seleccionado para realizar las validaciones del tipo de documento receptor
            if (((CheckBox)cbDocElectronico.Specific).Checked)
            {
                //Validar que se haya seleccionado un tipo de documento receptor
                //if (((ComboBox)cbxTipoDocumento.Specific).Value == "")
                //{
                //    //Muestra el mensaje de error
                //    //AdminEventosUI.mostrarMensaje(Mensaje.errSeleccionDocReceptor, AdminEventosUI.tipoError);

                //    //Selecciona el campo de valor de tipo de documento
                //    ((ComboBox)cbxTipoDocumento.Specific).Active = true;

                //   // return false;
                //}

                //Verifica que el tipo seleccionado no se RUT
                 if (((ComboBox)cbxTipoDocumento.Specific).Selected.Value != "RUT")
                {
                    //Si el tipo de documento no es RUT el valor no debe estar vacio
                    if (((EditText)txtTipoDocumento.Specific).Value == "" && ((ComboBox)cbxTipoDocumento.Specific).Selected.Value != "C.I.")
                    {
                        //Muestra el mensaje de error
                        AdminEventosUI.mostrarMensaje(Mensaje.errDocReceptorSeleccionado, AdminEventosUI.tipoError);

                        //Selecciona el campo de valor de tipo de documento
                        ((EditText)txtTipoDocumento.Specific).Active = true;

                        return false;
                    }
                    else
                    {
                        //Verifica que el tipo seleccionado sea DIN
                        if (((ComboBox)cbxTipoDocumento.Specific).Selected.Value == "DNI")
                        {
                            //Valida que el pais del socio de negocio sea AR,BR,CL o PY
                            if (!ValidarPaisDNI())
                            {
                                //Muestra el mensaje de error
                                AdminEventosUI.mostrarMensaje(Mensaje.errDocReceptorNoValArBrChPa, AdminEventosUI.tipoError);

                                //Selecciona el campo de valor de tipo de documento
                                ((EditText)txtTipoDocumento.Specific).Active = true;

                                return false;
                            }
                        }
                    }
                }

                //Valida que el tipo de documento seleccionado sea RUT o C.I.
                if (((ComboBox)cbxTipoDocumento.Specific).Selected.Value == "RUT" || ((ComboBox)cbxTipoDocumento.Specific).Selected.Value == "C.I.")
                {
                    //Valida que el codigo de pais se UY
                    if (!ValidarPaisCIDNI())
                    {
                        //Muestra el mensaje de error
                        AdminEventosUI.mostrarMensaje(Mensaje.errDocReceptorNoValUr, AdminEventosUI.tipoError);

                        //Selecciona el campo de valor de tipo de documento
                        ((EditText)txtTipoDocumento.Specific).Active = true;

                        return false;
                    }
                }

                #region FE_EXPORTACION
                string codigoSocioNegocio = ((EditText)Formulario.Items.Item("4").Specific).Value;

                //Crar instanacia del mantenimiento de documentos
                ManteDocumentos manteDocumentos = new ManteDocumentos();

                if (manteDocumentos.ValidarClienteExtranjero(codigoSocioNegocio))
                {

                  

                    if (manteDocumentos.ValidarClienteContado(codigoSocioNegocio))                    
                    {
                        AdminEventosUI.mostrarMensaje(Mensaje.errorDocExpoCF, AdminEventosUI.tipoError);
                        return false;
                    }
                    else
                        if (((ComboBox)cbxTipoDocumento.Specific).Selected.Value.Contains("C.I."))
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errorTipoDocExpo, AdminEventosUI.tipoError);
                            return false;
                        }
                }
                #endregion FE_EXPORTACION

            }

            return true;
        }

        /// <summary>
        /// Valida que el pais del socio de negocio sea AR,BR,CL o PY
        /// </summary>
        /// <returns></returns>
        private bool ValidarPaisDNI()
        {
            string codigoSocioNegocio = ((EditText)Formulario.Items.Item("4").Specific).Value;

            //Crar instanacia del mantenimiento de documentos
            ManteDocumentos manteDocumentos = new ManteDocumentos();

            //Obtener el codigo del pais del socio de negocio
            string codigoPais = manteDocumentos.ConsultarCodPaisSocioNegocio(codigoSocioNegocio, CFE.ESTipoDocumentoReceptor.Nada);

            //Valida que el codigo del pais sea AR,BR,CL o PY
            if (codigoPais.Equals("AR") || codigoPais.Equals("BR") || codigoPais.Equals("CL") || codigoPais.Equals("PY"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el codigo de pais se UY
        /// </summary>
        /// <returns></returns>
        private bool ValidarPaisCIDNI()
        {
            string codigoSocioNegocio = ((EditText)Formulario.Items.Item("4").Specific).Value;

            //Crar instanacia del mantenimiento de documentos
            ManteDocumentos manteDocumentos = new ManteDocumentos();

            //Obtener el codigo del pais del socio de negocio
            string codigoPais = manteDocumentos.ConsultarCodPaisSocioNegocio(codigoSocioNegocio);

            //Valida que el codigo del pais sea UY
            if (codigoPais.Equals("UY"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que los datos de referencia 
        /// </summary>
        /// <returns></returns>
        public bool ValidarReferncia(string formTypeEx)
        {
            if (formTypeEx.Equals("65303") || formTypeEx.Equals("170"))
            {
                //Crea una nueva instancia de Mante del udo de documento
                ManteDocumentos manteDocumentos = new ManteDocumentos();

                if (((EditText)txtSerieRef.Specific).Value.Equals("1"))
                {
                    if (!((EditText)txtNumRef.Specific).Value.Equals(""))
                    {
                        return true;
                    }
                }
                else
                {
                    if (manteDocumentos.ValidarReferencia(((EditText)txtSerieRef.Specific).Value, ((EditText)txtNumRef.Specific).Value))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            //Muestra el mensaje de error
            AdminEventosUI.mostrarMensaje(Mensaje.errDatosNoValidos, AdminEventosUI.tipoError);

            //Coloca el focus en el campo de serie de referencia
            ((EditText)txtSerieRef.Specific).Active = true;

            return false;
        }

        /// <summary>
        /// Valida que existan CAEs validos para los tipos de documentos:
        /// e-Factura
        /// e-FacturaContingencia
        /// e-Ticket
        /// e-TicketContingencia
        /// e-Resguardo
        /// e-Resguardo Contingencia
        /// </summary>
        /// <returns></returns>
        public string ValidarCAEs(int tipoDocumento)
        {
            string resultado = "", consulta = "", resultadoFinal = "CAE(s) Inválido(s): \n";
            bool eFactura = false, eTicket = false, eResguardo = false, eRemito = false,
                NDeTicket = false, NDeFactura = false, NCeTicket = false, NCeFactura = false;
                //eFacturaExportacion = false, NDeFacturaExportacion = false,
                //NCeFacturaExportacion = false, eRemitoExportacion = false;
            ManteUdoDocumento manteUdoDocumento = new ManteUdoDocumento();
            List<ValidacionCAE> listaCAEs = manteUdoDocumento.ObtenerListaCAEs();

            if (listaCAEs.Count != 0)
            {
                foreach (ValidacionCAE cae in listaCAEs)
                {
                    #region REMITOS

                    if (tipoDocumento == 0)
                    {
                        #region E-Remito

                        if (cae.TipoDocumento.Equals("181"))
                        {
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    eRemito = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeRemitoFechaVence;
                                    eRemito = true;
                                }
                            }
                            else
                            {
                                resultado = resultado + Mensaje.infCaeRemitoFechaEmision;
                                eRemito = true;
                            }
                        }

                        #endregion E-Remito

                        #region E-Remito-Exportacion

                        //else if (lista[0].Equals("e-Remito Exportacion"))
                        //{
                        //    //Se valida la fecha de emision
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        //Se valida la fecha de vencimiento
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            eRemitoExportacion = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = resultado + Mensaje.infCaeRemExpFechaVence;
                        //            eRemitoExportacion = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = resultado + Mensaje.infCaeRemExpFechaEmision;
                        //        eRemitoExportacion = true;
                        //    }
                        //}

                        #endregion E-Remito-Exportacion
                    }

                    #endregion REMITOS

                    #region FACTURA - TICKET - RESGUARDO

                    else if (tipoDocumento == 1)
                    {
                        #region E-Factura

                        if (cae.TipoDocumento.Equals("111"))
                        {
                            //Se valida la fecha de emision
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                //Se valida la fecha de vencimiento
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    eFactura = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeFacturaFechaVence;
                                    eFactura = true;
                                }
                            }
                            else
                            {

                                resultado = resultado + Mensaje.infCaeFacturaFechaEmision;
                                eFactura = true;
                            }
                        }

                        #endregion E-Factura

                        #region E-Factura-Exportacion

                        //else if (lista[0].Equals("e-Factura Exportacion"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            eFacturaExportacion = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = resultado + Mensaje.infCaeFacExpFechaVence;
                        //            eFacturaExportacion = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = resultado + Mensaje.infCaeFacExpFechaEmision;
                        //        eFacturaExportacion = true;
                        //    }
                        //}

                        #endregion E-Factura-Exportacion

                        #region E-Ticket

                        else if (cae.TipoDocumento.Equals("101"))
                        {
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                if (fechaValidaHasta(cae.ValidoDesde))
                                {
                                    eTicket = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeTicketFechaVence;
                                    eTicket = true;
                                }
                            }
                            else
                            {
                                resultado = resultado + Mensaje.infCaeTicketFechaEmision;
                                eTicket = true;
                            }
                        }

                        #endregion E-Ticket

                        #region E-Ticket-Contingencia

                        //else if (lista[0].Equals("e-Ticket Contingencia"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            eTicContingencia = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = Mensaje.infCaeTicContFechaVence;
                        //            eTicContingencia = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = Mensaje.infCaeTicContFechaEmision;
                        //        eTicContingencia = true;
                        //    }
                        //}

                        #endregion E-Ticket-Contingencia

                        #region E-Resguardo

                        else if (cae.TipoDocumento.Equals("182"))
                        {
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    eResguardo = true;
                                }
                                else
                                {
                                    resultado = Mensaje.infCaeResguardoFechaVence;
                                    eResguardo = true;
                                }
                            }
                            else
                            {
                                resultado = Mensaje.infCaeResguardoFechaEmision;
                                eResguardo = true;
                            }
                        }

                        #endregion E-Resguardo

                        #region E-Resguardo Contingencia

                        //else if (lista[0].Equals("e-Resguardo Contingencia"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            eResContingencia = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = Mensaje.infCaeResContFechaVence;
                        //            eResContingencia = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = Mensaje.infCaeResContFechaEmision;
                        //        eResContingencia = true;
                        //    }
                        //}

                        #endregion E-Resguardo Contingencia
                    }
                    #endregion FACTURA - TICKET - RESGUARDO

                    #region ND: FACTURA - TICKET

                    else if (tipoDocumento == 2)
                    {
                        #region ND. E-Factura

                        if (cae.TipoDocumento.Equals("113"))
                        {
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    NDeFactura = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeNDFacFechaVence;
                                    NDeFactura = true;
                                }
                            }
                            else
                            {
                                resultado = resultado + Mensaje.infCaeNDFacFechaEmision;
                                NDeFactura = true;
                            }
                        }

                        #endregion ND. E-Factura

                        #region ND. e-Factura-Exportacion

                        //else if (lista[0].Equals("ND. e-Factura Exportacion"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            NDeFacturaExportacion = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = resultado + Mensaje.infCaeNDFacExpFechaVence;
                        //            NDeFacturaExportacion = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = resultado + Mensaje.infCaeNDFacExpFechaEmision;
                        //        NDeFacturaExportacion = true;
                        //    }
                        //}

                        #endregion ND. E-Factura-Exportacion

                        #region ND. E-Ticket

                        else if (cae.TipoDocumento.Equals("103"))
                        {
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    NDeTicket = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeNDTicketFechaVence;
                                    NDeTicket = true;
                                }
                            }
                            else
                            {
                                resultado = resultado + Mensaje.infCaeNDTicketFechaEmision;
                                NDeTicket = true;
                            }
                        }

                        #endregion ND. E-Ticket

                        #region ND. E-Ticket-Contingencia

                        //else if (lista[0].Equals("ND. e-Ticket Contingencia"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            NDeTicContingencia = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = resultado + Mensaje.infCaeNDTicContFechaVence;
                        //            NDeTicContingencia = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = resultado + Mensaje.infCaeNDTicContFechaEmision;
                        //        NDeTicContingencia = true;
                        //    }
                        //}

                        #endregion ND. E-Ticket-Contingencia
                    }

                    #endregion ND: FACTURA - TICKET

                    #region NC: FACTURA - TICKET

                    else if (tipoDocumento == 3)
                    {
                        #region NC. E-Factura

                        if (cae.TipoDocumento.Equals("112"))
                        {
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    NCeFactura = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeNCFacturaFechaVence;
                                    NCeFactura = true;
                                }
                            }
                            else
                            {
                                resultado = resultado + Mensaje.infCaeNCFacturaFechaEmision;
                                NCeFactura = true;
                            }
                        }

                        #endregion NC. E-Factura

                        #region NC. E-Factura-Exportacion

                        //else if (lista[0].Equals("NC. e-Factura Exportacion"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            NCeFacturaExportacion = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = resultado + Mensaje.infCaeNCFacExpFechaVence;
                        //            NCeFacturaExportacion = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = resultado + Mensaje.infCaeNCFacExpFechaEmision;
                        //        NCeFacturaExportacion = true;
                        //    }
                        //}

                        #endregion NC. E-Factura-Exportacion

                        #region NC. E-Ticket

                        else if (cae.TipoDocumento.Equals("102"))
                        {
                            //Se valida la fecha de emision
                            if (fechaValidaDesde(cae.ValidoDesde))
                            {
                                //Se valida la fecha de vencimiento
                                if (fechaValidaHasta(cae.ValidoHasta))
                                {
                                    NCeTicket = true;
                                }
                                else
                                {
                                    resultado = resultado + Mensaje.infCaeNCTicketFechaVence;
                                    NCeTicket = true;
                                }
                            }
                            else
                            {
                                resultado = resultado + Mensaje.infCaeNCTicketFechaEmision;
                                NCeTicket = true;
                            }
                        }

                        #endregion NC. E-Ticket

                        #region NC. E-Ticket-Contingencia

                        //else if (lista[0].Equals("NC. e-Ticket Contingencia"))
                        //{
                        //    if (fechaValidaDesde(lista[1]))
                        //    {
                        //        if (fechaValidaHasta(lista[2]))
                        //        {
                        //            NCeTicContingencia = true;
                        //        }
                        //        else
                        //        {
                        //            resultado = resultado + Mensaje.infCaeNCTicContFechaVence;
                        //            NCeTicContingencia = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        resultado = resultado + Mensaje.infCaeNCTicContFechaEmision;
                        //        NCeTicContingencia = true;
                        //    }
                        //}

                        #endregion NC. E-Ticket-Contingencia
                    }

                    #endregion NC: FACTURA - TICKET
                }
            }
            else
            {
                if (tipoDocumento == 0)
                {
                    resultado = "No se ha configurado los CAEs para los siguientes tipos de documento(s): \n e-Remito, e-Remito Contingencia. \n No se recomienda el uso de la Facturación Electrónica";
                }
                else if (tipoDocumento == 1)
                {
                    resultado = "No se ha configurado los CAEs para los siguientes tipos de documento(s): \n e-Factura, e-Factura Contingencia, e-Ticket y e-Ticket Contingencia. \n No se recomienda el uso de la Facturación Electrónica";
                }
                else if (tipoDocumento == 2)
                {
                    resultado = "No se ha configurado los CAEs para los siguientes tipos de documento(s): \n e-NDFactura, e-NDFactura Contingencia, e-NDTicket y e-NDTicket Contingencia. \n No se recomienda el uso de la Facturación Electrónica";
                }
                else if (tipoDocumento == 3)
                {
                    resultado = "No se ha configurado los CAEs para los siguientes tipos de documento(s): \n e-NCFactura, e-NCFactura Contingencia, e-NCTicket y e-NCTicket Contingencia. \n No se recomienda el uso de la Facturación Electrónica";
                }

                //resultado = Mensaje.infCaeNoConfig;
                return resultado;
            }

            #region Control de errores

            if (resultado.Equals(""))
            {
                bool impresion = false;

                #region Entrega

                if (tipoDocumento == 0)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!eRemito)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeRemitoNoConfig;
                    }
                    //if (!eRemitoExportacion)
                    //{
                    //    impresion = true;
                    //    resultado = resultado + Mensaje.infCaeRemitoExpNoConfig;
                    //}
                }

                #endregion Entrega

                #region FACTURA - TICKET - RESGUARDO

                else if (tipoDocumento == 1)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!eFactura)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeFacturaNoConfig;
                    }
                    //if (!eFacturaExportacion)
                    //{
                    //    impresion = true;
                    //    resultado = resultado + Mensaje.infCaeFacturaExpNoConfig;
                    //}
                    if (!eTicket)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeTicketNoConfig;
                    }
                    if (!eResguardo)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeResguardoNoConfig;
                    }
                }

                #endregion FACTURA - TICKET - RESGUARDO

                #region ND: FACTURA - TICKET

                else if (tipoDocumento == 2)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!NDeFactura)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeNDFacturaNoConfig;
                    }
                    //if (!NDeFacturaExportacion)
                    //{
                    //    impresion = true;
                    //    resultado = resultado + Mensaje.infCaeNDFacturaExpNoConfig;
                    //}
                    if (!NDeTicket)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeNDTicketNoConfig;
                    }
                }

                #endregion ND: FACTURA - TICKET

                #region NC: FACTURA - TICKET

                else if (tipoDocumento == 3)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!NCeFactura)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeNCFacturaNoConfig;
                    }
                    //if (!NCeFacturaExportacion)
                    //{
                    //    impresion = true;
                    //    resultado = resultado + Mensaje.infCaeNCFacturaExpNoConfig;
                    //}
                    if (!NCeTicket)
                    {
                        impresion = true;
                        resultado = resultado + Mensaje.infCaeNCTicketNoConfig;
                    }
                }

                #endregion NC: FACTURA - TICKET

                if (impresion)
                {
                    resultado = Mensaje.infFaltaInfoCae + resultado + Mensaje.infNoUsoFE;
                }
                return resultado;
            }
            else
            {
                #region Entrega

                if (tipoDocumento == 0)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!eRemito)
                    {
                        resultado = resultado + Mensaje.infCaeRemitoNoConfig;
                    }
                    //else if (!eRemitoExportacion)
                    //{
                    //    resultado = resultado + Mensaje.infCaeRemitoExpNoConfig;
                    //}
                }

                #endregion Entrega

                #region Factura

                //En caso de que sea el formulario de Factura
                else if (tipoDocumento == 1)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!eFactura)
                    {
                        resultado = resultado + Mensaje.infCaeFacturaNoConfig;
                    }
                    //if (!eFacturaExportacion)
                    //{
                    //    resultado = resultado + Mensaje.infCaeFacturaExpNoConfig;
                    //}
                    if (!eTicket)
                    {
                        resultado = resultado + Mensaje.infCaeTicketNoConfig;
                    }
                    if (!eResguardo)
                    {
                        resultado = resultado + Mensaje.infCaeResguardoNoConfig;
                    }
                }

                #endregion Factura

                #region Nota Debito

                else if (tipoDocumento == 2)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!NDeFactura)
                    {
                        resultado = resultado + Mensaje.infCaeNDFacturaNoConfig;
                    }
                    if (!NDeTicket)
                    {
                        resultado = resultado + Mensaje.infCaeNDTicketNoConfig;
                    }
                    //if (!NDeFacturaExportacion)
                    //{
                    //    resultado = resultado + Mensaje.infCaeNDFacturaExpNoConfig;
                    //}
                }

                #endregion Nota Debito

                #region Nota Credito

                else if (tipoDocumento == 3)
                {
                    //Se valida si hay que mostrar errores para los diferentes tipos de CAEs
                    if (!NCeFactura)
                    {
                        resultado = resultado + Mensaje.infCaeNCFacturaNoConfig;
                    }
                    if (!NCeTicket)
                    {
                        resultado = resultado + Mensaje.infCaeNCTicketNoConfig;
                    }

                    //if (!NCeFacturaExportacion)
                    //{
                    //    resultado = resultado + Mensaje.infCaeNCFacturaExpNoConfig;
                    //}
                }

                #endregion Nota Credito

                //Se agregan datos al mensaje final
                resultadoFinal = resultadoFinal + resultado + Mensaje.infNoUsoFE;

                //Se retorna el mensaje de error
                return resultadoFinal;
            }
            #endregion Control de errores
        }

        /// <summary>
        /// Metodo que valida una fecha
        /// </summary>
        /// <param name="fechaValidar"></param>
        /// <returns></returns>
        private bool fechaValidaDesde(string fechaValidar)
        {
            bool resultado = false;
            DateTime fechaComparar = DateTime.Parse(fechaValidar);

            if (fechaComparar <= DateTime.Now)
            {
                resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Determina si un socio de negocio paga de contado
        /// </summary>
        /// <param name="codigoSN"></param>
        /// <returns></returns>
        public bool Contado(string codigoSN)
        {
            bool resultado = false;
            Recordset registro = null;
            string consulta = "SELECT U_SNCont FROM OCRD WHERE CardCode = '" + codigoSN + "'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    string contado = registro.Fields.Item("U_SNCont").Value + "";

                    if (contado.Equals("Y"))
                    {
                        resultado = true;
                    }
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
        /// Metodo que valida la serie y numero de un rango al crear documento
        /// en estado de contingencia 
        /// </summary>
        /// <param name="formulario"></param>
        /// <returns></returns>
        public bool ValidarNumeroSerie(Form formulario, string tipoDoc)
        {
            bool resultado = false;

            EditText txtNumCFE = (EditText)formulario.Items.Item("txtNumCFE").Specific;
            EditText txtSeCFE = (EditText)formulario.Items.Item("txtSeCFE").Specific;

            if (txtNumCFE.Value.Equals("") || txtSeCFE.Value.Equals(""))
            {
                AdminEventosUI.mostrarMensaje("Debe ingresar la serie y/o su correspondiente número para crear la factura", AdminEventosUI.tipoMensajes.error);
            }
            else if (ValidarRango(txtSeCFE.Value, txtNumCFE.Value, tipoDoc))
            {
                resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Valida si un rango ha sido utilizado
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        private bool ValidarRango(string pSerie, string pNumero, string pTipoDoc)
        {
            bool resultado = false;
            Recordset registro = null;
            string consulta = "SELECT U_NumAct, U_NumFin FROM [@TFERANGO] WHERE U_Serie = '" + pSerie + "' AND GETDATE() <= U_ValHasta AND GETDATE() >= U_ValDesde AND U_TipoDoc = '" + pTipoDoc + "'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    int numeroActual = Convert.ToInt32(registro.Fields.Item("U_NumAct").Value + "");
                    int numeroFinal = Convert.ToInt32(registro.Fields.Item("U_NumFin").Value + "");
                    int numero = Convert.ToInt32(pNumero);

                    if (numero > numeroActual)
                    {
                        if (numero <= numeroFinal)
                        {
                            if (FrmEstadoContingencia.estadoContingencia.Equals("Y"))
                            {
                                if (ValidarRangoUsadoContingencia(numero) && ValidarRangoUsadoContingenciaT(numero))
                                {
                                    resultado = true;
                                }
                            }
                            else
                            {
                                resultado = true;
                            }
                        }
                        else
                        {
                            AdminEventosUI.mostrarMensaje(Mensaje.errValNumFin, AdminEventosUI.tipoMensajes.error);
                        }
                    }
                    else
                    {
                        AdminEventosUI.mostrarMensaje(Mensaje.errValNumAct + numeroActual.ToString(), AdminEventosUI.tipoMensajes.error);
                    }
                }
                else
                {
                    AdminEventosUI.mostrarMensaje(Mensaje.errValRangoSerie, AdminEventosUI.tipoMensajes.error);
                }
            }
            catch (Exception ex)
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errGenerico + ex.ToString(), AdminEventosUI.tipoMensajes.error);
            }
            finally
            {
                if (registro != null)
                {
                    //Libera el objeto registro de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Valida si un rango ha sido usado en contingencia
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        private bool ValidarRangoUsadoContingencia(int numero)
        {
            bool resultado = false;
            Recordset registro = null;
            List<string> cfesContingencia = null;
            int i = 0;

            string consulta = "SELECT U_NumCFE from [@TFECFE] WHERE U_TipoDoc LIKE '2%'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                //Se realiza la consulta
                registro.DoQuery(consulta);

                //No se ha usado el rango
                if (registro.RecordCount == 0)
                {
                    resultado = true;
                }
                else
                {
                    //Se crea una nueva lista para numeoros derangos usados
                    cfesContingencia = new List<string>();

                    while (i < registro.RecordCount)
                    {
                        //Se obtiene el numero de rango usado
                        cfesContingencia.Add(registro.Fields.Item("U_NumCFE").Value + "");
                        //Se avanza de registro
                        registro.MoveNext();
                        i++;
                    }
                    //el numero no ha sido usado
                    if (!cfesContingencia.Contains(numero.ToString()))
                    {
                        resultado = true;
                    }

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
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Valida si un rango ha sido usado en contingencia
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        private bool ValidarRangoUsadoContingenciaT(int numero)
        {
            bool resultado = false;
            Recordset registro = null;
            List<string> cfesContingencia = null;
            int i = 0;

            string consulta = "SELECT U_NumCFE from [@TFECERCON] WHERE U_TipoDoc LIKE '2%'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                //Se realiza la consulta
                registro.DoQuery(consulta);

                //No se ha usado el rango
                if (registro.RecordCount == 0)
                {
                    resultado = true;
                }
                else
                {
                    //Se crea una nueva lista para numeoros derangos usados
                    cfesContingencia = new List<string>();

                    while (i < registro.RecordCount)
                    {
                        //Se obtiene el numero de rango usado
                        cfesContingencia.Add(registro.Fields.Item("U_NumCFE").Value + "");
                        //Se avanza de registro
                        registro.MoveNext();
                        i++;
                    }
                    //el numero no ha sido usado
                    if (!cfesContingencia.Contains(numero.ToString()))
                    {
                        resultado = true;
                    }
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
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LLenarCbxRazonReferencia()
        {
            List<Tuple<string, string>> razones = new List<Tuple<string,string>>();
            Recordset registro = null;
            string consulta = "SELECT U_Codigo, U_Razon FROM [@TFERZR]", docEntry = "", razon ="";
            int i = 0;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                while ( i < registro.RecordCount)
                {
                    docEntry = registro.Fields.Item("U_Codigo").Value + "";
                    razon = registro.Fields.Item("U_Razon").Value + "";
                    Tuple<string, string> temporal = new Tuple<string,string>(docEntry,razon);
                    razones.Add(temporal);
                    registro.MoveNext();
                    i++;
                }

                foreach (Tuple<string,string> idRazon in razones)
                {
                    ((ComboBox)Formulario.Items.Item("cbxRazRef").Specific).ValidValues.Add(idRazon.Item1, idRazon.Item2);
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
                    GC.Collect();
                }
            }
        }


        /// <summary>
        /// Determina si esta configurado el CAE de contingencia
        /// </summary>
        /// <returns></returns>
        public bool ValidarCAEContingencia()
        {
            bool resultado = false;
            Recordset registro = null;
            string consulta = "SELECT U_TipoDoc FROM [@TFERANGO] WHERE U_TipoDoc = '999'";

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (registro != null)
                {
                    //Libera el objeto registro de memoria
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Metodo que valida una fechade vencimiento
        /// </summary>
        /// <param name="fechaValidar"></param>
        /// <returns></returns>
        private bool fechaValidaHasta(string fechaValidar)
        {
            bool resultado = false;

            DateTime fechaComparar = DateTime.Parse(fechaValidar);

            if (fechaComparar >= DateTime.Now)
            {
                resultado = true;
            }

            return resultado;
        }

        #region FE_EXPORTACION
        public int ValidarCamposExportacion(int tipoDocumento)
        {
            int resultado = 0; //OK

            if (tipoDocumento == 0) // Remito
            {
                //Validar Modalidad de Venta (2)
                //Validar Via de Transporte (3)
                //Validar Indicador Tipo de Bienes (4)
                if (((ComboBox)cbxTipoBienes.Specific).Value == "")
                {
                    return 4;
                }

                if (((ComboBox)cbxModVen.Specific).Value == "")
                {
                    return 2;
                }

                if (((ComboBox)cbxViaTransporte.Specific).Value == "")
                {
                    return 3;
                }
            }
            else if (tipoDocumento == 1) // Factura
            {
                //Validar Clausula de Venta (1)
                //Validar Modalidad de Venta (2)
                //Validar Via de Transporte (3)
                //Largo > 3 en Clausula de Venta (7)
                if (((EditText)txtClauVen.Specific).Value == "")
                {
                    return 1;
                }
                else if (((EditText)txtClauVen.Specific).Value.Length > 3)
                {
                    return 7;
                }

                if (((ComboBox)cbxModVen.Specific).Value == "")
                {
                    return 2;
                }

                if (((ComboBox)cbxViaTransporte.Specific).Value == "")
                {
                    return 3;
                }
            }
            else if (tipoDocumento == 2 || tipoDocumento == 3) // NC y ND
            {
                //Validar Clausula de Venta
                //Validar Via de Transporte
                //Largo > 3 en Clausula de Venta (7)
                if (((EditText)txtClauVen.Specific).Value == "")
                {
                    return 1;
                }
                else if (((EditText)txtClauVen.Specific).Value.Length > 3)
                {
                    return 7;
                }

                if (((ComboBox)cbxViaTransporte.Specific).Value == "")
                {
                    return 3;
                }
            }

            return resultado;
        }
        #endregion FE_EXPORTACION

        #endregion VALIDACIONES

        #region PROPIEDADES

        private string idFormulario;

        public string IdFormulario
        {
            get { return idFormulario; }
            set { idFormulario = value; }
        }

        private string typeEx;

        public string TypeEx
        {
            get { return typeEx; }
            set { typeEx = value; }
        }

        #endregion PROPIEDADES

        #region MANTENIMIENTO

        /// <summary>
        /// Valida la cedula Juridica
        /// </summary>
        /// <param name="cedulaJuridica"></param>
        /// <param name="tipoSN"></param>
        /// <returns></returns>
        public bool ValidarCedulaJuridica(string cedulaJuridica, string tipoSN, string total, string tipoCambio, string Moneda)
        {
            bool salida = false;
            string ciGenerico = "";
            

            if (tipoSN.Equals("RUT")) 
            {
                if (ValidarNumero(cedulaJuridica))
                {
                    if (cedulaJuridica.Length == 12)
                    {
                        salida = true;
                    }
                }
            }
            else if (tipoSN.Equals("CI"))
            {
                ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                ciGenerico = manteUdoAdobe.ObtenerCiGenerico();

                if (cedulaJuridica.Equals(ciGenerico) && !ciGenerico.Equals(""))
                {
                    salida = ValidarPrecioMayorDiesMilUI(total, tipoCambio, Moneda);
                }
                else
                {
                    if (ValidarNumero(cedulaJuridica))
                    {
                        if (cedulaJuridica.Length == 8 || cedulaJuridica.Length == 7)
                        {
                            salida = true;
                        }
                    }
                }
            }

             else 
            {
                salida = true;
            }
            return salida;
        }


        private Boolean ValidarPrecioMayorDiesMilUI(string total, string tipoCambio, string Moneda)
        {
            Boolean salida = false;

            string [] arr ;

              arr = total.Split(',');  // declaro el array  

            string entero = arr[0];       
            
            string TotalEntero = Regex.Replace(entero, @"[^\d]", "");
            
            Int64 Total = Convert.ToInt64(TotalEntero);

            double  TipoCambio = Convert.ToDouble(tipoCambio);

            Moneda = Moneda.Trim();

            if (Moneda.Equals("UYU") || Moneda.Equals("$"))
            {
                if (ValorUI.valorUI * 10000 > Total)
                {
                    salida = true;
                }
            }
            else
            {

                ManteUdoTipoCambio manteUdoTipoCambio = new ManteUdoTipoCambio();
                string temp = "", confTC = manteUdoTipoCambio.ObtenerConfiguracion(out temp);

                if (confTC.Equals("N"))
                {
                                  
                        if (TipoCambio > 0)
                        {
                          TipoCambio = 1 / TipoCambio;
                          TipoCambio = Math.Round(TipoCambio, 2);
                        }
                }
                                             

                if (ValorUI.valorUI * 10000 > (Total) * TipoCambio)
                {
                    salida = true;
                }
            }


            return salida;
    
         }

        /// <summary>
        /// Valida que una cadena reprensente un numero entero
        /// </summary>
        /// <param name="numeroComprobar"></param>
        /// <returns></returns>
        private bool ValidarNumero(string numeroComprobar)
        {
            bool salida = false;

            try
            {
                long.Parse(numeroComprobar);
                salida = true;
            }
            catch (Exception)
            {
            }

            return salida;
        }

        /// Consulta la adenda del socion de negocio y el tipo de documento fiscal y la carga en el campo de adenda
        /// </summary>
        /// <param name="formulario"></param>
        public void CargarAdenda(string tipoDocumento, string codigo)
        {
            string adendaDocumento = "";
            ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();

            //Se almacena la adenda
            string adendaText = ((EditText)Formulario.Items.Item("txtAdn").Specific).Value;

            if (adendaText == "")
            {             

                //Carga la adenda asignada al socio de negocio
                adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.SN, codigo);

                adendaDocumento = adendaDocumento + adenda.CadenaAdenda + Environment.NewLine;

                ((EditText)Formulario.Items.Item("txtAdn").Specific).String = adendaDocumento;

            }
        }

        /// <summary>
        /// Retorna la adenda
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <returns></returns>
        public string ObtenerAdenda(string tipoDocumento)
        {
            string adendaDocumento = "";

            ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();

            if (Formulario.Mode == BoFormMode.fm_ADD_MODE)
            {
                //Carga la adenda asignada al socio de negocio
                adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.SN, ((EditText)Formulario.Items.Item("4").Specific).String);

                adendaDocumento = adendaDocumento + adenda.CadenaAdenda + Environment.NewLine;
            }

            if (Formulario.Mode == BoFormMode.fm_ADD_MODE)
            {
                //Switch para obtener la adenda para el tipo de documento
                switch (tipoDocumento)
                {
                    case "133":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.DocFiscal, "111");
                        break;

                    case "65303":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.DocFiscal, "113");
                        break;

                    case "179":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.DocFiscal, "112");
                        break;

                    case "140":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.DocFiscal, "181");
                        break;
                }

                adendaDocumento = adendaDocumento + adenda.CadenaAdenda;
            }

            if (Formulario.Mode == BoFormMode.fm_UPDATE_MODE)
            {
                //Siwtch para obtener adenda para el documento especifico
                switch (tipoDocumento)
                {
                    case "133":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.TipoCFE111, ((EditText)Formulario.Items.Item("8").Specific).String);
                        break;

                    case "65303":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.TipoCFE113, ((EditText)Formulario.Items.Item("8").Specific).String);
                        break;

                    case "179":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.TipoCFE112, ((EditText)Formulario.Items.Item("8").Specific).String);
                        break;

                    case "140":
                        adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.TipoCFE181, ((EditText)Formulario.Items.Item("8").Specific).String);
                        break;
                }

                adendaDocumento = adendaDocumento + adenda.CadenaAdenda;
            }

            return adendaDocumento;
        }

       

        /// <summary>
        /// Obtiene el nombre del archivo a reimprimir
        /// </summary>
        /// <param name="docNum"></param>
        /// <returns></returns>
        public string ObtenerPDFReimpresion(string docEntry, List<string> tipos)
        {
            string resultado = "";

            if (!docEntry.Equals(""))
            {
                resultado = ObtenerNombreArchivo(docEntry, tipos) + ".pdf";

                if (!resultado.Equals(""))
                {
                    FTP ftp = new FTP();
                    if (!ftp.descargarArchivos(resultado, RutasCarpetas.RutaCarpetaComprobantes, 1))
                    {
                      // SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("El Archivo no se encuentra en el servidor FTP, se buscara en PC cliente.");
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el numero de DocEntry en base al docNum y la tabla
        /// </summary>
        /// <param name="docNum"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public string ObtenerDocEntry(string docNum, string tabla)
        {
            string resultado = "", consulta = "SELECT DocEntry FROM " + tabla + " WHERE DocNum = '" + docNum + "'";
            Recordset registro = null;

            try
            {
                registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                registro.DoQuery(consulta);

                if (registro.RecordCount > 0)
                {
                    resultado = registro.Fields.Item("DocEntry").Value + "";
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
                    GC.Collect();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el nombre del archivo que se va a reimprimir
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="tipos"></param>
        /// <returns></returns>
        private string ObtenerNombreArchivo(string docEntry, List<string> tipos)
        {
            Recordset registro = null;
            string salida = "", consulta = "";

            try
            {
                foreach (string tipo in tipos)
                {
                    consulta = "SELECT U_NumCfe, U_Serie, U_TipoDoc FROM [@TFECFE] WHERE U_DocSap = '" + docEntry + "' " +
                     "AND U_TipoDoc = '" + tipo + "'";
                    registro = ProcConexion.Comp.GetBusinessObject(BoObjectTypes.BoRecordset);
                    registro.DoQuery(consulta);
                    if (registro.RecordCount > 0)
                    {
                        salida = registro.Fields.Item("U_TipoDoc").Value + "";
                        salida += registro.Fields.Item("U_Serie").Value + "";
                        salida += registro.Fields.Item("U_NumCfe").Value + "";
                        break;
                    } 
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (registro != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(registro);
                    GC.Collect();
                }
            }

            return salida;
        }

        #endregion
    }
}