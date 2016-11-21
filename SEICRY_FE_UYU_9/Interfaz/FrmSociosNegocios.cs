using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmSociosNegocios
    {
        private Item itemAreaTexto;
        private Adenda adenda = new Adenda();
       
        /// <summary>
        /// Agrega un checkbox al formulario de socios de negocios
        /// </summary>
        /// <param name="formulario"></param>
        public void AgregarCheckBoxClienteContado(Form formulario)
        {
            //Se agregan items nuevo
            formulario.Items.Add("cbxSNCont",BoFormItemTypes.it_CHECK_BOX);
            formulario.Items.Add("lblSNC", BoFormItemTypes.it_STATIC);

            //Se obtienen items de referencia
            Item referenciaLabel = formulario.Items.Item("148");
            Item referenciaTextBox = formulario.Items.Item("149");

            //Se obtiene el item creado
            Item cbxSNContado = formulario.Items.Item("cbxSNCont");
            Item lblContado = formulario.Items.Item("lblSNC");

            //Se asignan propiedades del objeto
            cbxSNContado.Left = referenciaTextBox.Left - 1;
            cbxSNContado.Top = referenciaTextBox.Top + referenciaTextBox.Height + 1;

            lblContado.Left = referenciaLabel.Left;
            lblContado.Top = referenciaLabel.Top + referenciaLabel.Height + 1;
            ((StaticText)lblContado.Specific).Caption = "Consumidor Final";
            lblContado.LinkTo = "cbxSNCont";

            //Crear binding con base de datos
            AgregarDataSources(formulario);
            CheckBox cbxSNC = (CheckBox)cbxSNContado.Specific;
            EstablecerDataBinds(formulario, cbxSNC);
        }

        /// <summary>
        /// Crea un nuevo tab para agregar la adenda
        /// </summary>
        public void AgregarTabAdenda(Form Formulario)
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Crear un nuevo tab para adenda
            Item itemTabAdenda = Formulario.Items.Add("tabAdn", BoFormItemTypes.it_FOLDER);

            //Obtener tab estandar de finanzas para agrupar el nuevo tab de adenda
            Item itemTabFinanzas = Formulario.Items.Item("4");

            //Establecer propidedades al nuevo tab de adenda respecto al tab de finanzas
            itemTabAdenda.Top = itemTabFinanzas.Top;
            itemTabAdenda.Left = itemTabFinanzas.Left;
            itemTabAdenda.Height = itemTabFinanzas.Height;
            itemTabAdenda.Width = 300;

            //Establecer propiedades especificas al tab de adenta
            ((Folder)itemTabAdenda.Specific).Caption = "Adenda";
            ((Folder)itemTabAdenda.Specific).Pane = 26;

            //Agrupar el nuevo tab de adenda con el tab de finanzas
            ((Folder)itemTabAdenda.Specific).GroupWith("4");

            //Establecer nivel de pane
            Formulario.PaneLevel = 1;

            CrearAreaTextoAdenda(Formulario);

            //Desongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crea el area de texto donde se ingresara la adenda
        /// </summary>
        private void CrearAreaTextoAdenda(Form Formulario)
        {
            //Se congelar el formulario
            Formulario.Freeze(true);

            //Obtener item de referencia
            Item itemReferencia = Formulario.Items.Item("21");

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
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Invocado cuando se presiona el nuevo tab
        /// </summary>
        public void SeleccionarTabAdenda(Form Formulario)
        {
            //Cambia el nivel del panel al del nuevo tab
            Formulario.PaneLevel = 26;
        }

        /// <summary>
        /// Agrega los dataSources al formulario
        /// </summary>
        /// <param name="formulario"></param>
        private void AgregarDataSources(Form formulario)
        {
            formulario.DataSources.DBDataSources.Add("OCRD");
        }

        /// <summary>
        /// Establece los databinds
        /// </summary>
        /// <param name="formulario"></param>
        /// <param name="cbxSnc"></param>
        private void EstablecerDataBinds(Form formulario, CheckBox cbxSnc)
        {
            cbxSnc.DataBind.SetBound(true, "OCRD", "U_SNCont");
        }

        /// <summary>
        /// Consulta la adenda del socion de negocio y la carga en el campo de adenda
        /// </summary>
        /// <param name="formulario"></param>
        public void CargarAdenda(Form formulario)
        {
            ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();

            adenda = manteUdoAdenda.ObtenerAdenda(Adenda.ESTipoObjetoAsignado.SN, ((EditText)formulario.Items.Item("5").Specific).String);

            ((EditText)formulario.Items.Item("txtAdn").Specific).String = adenda.CadenaAdenda;
        }

        /// <summary>
        /// Alamcena la adenda
        /// </summary>
        /// <param name="formulario"></param>
        public void AlmacenarAdenda(Form formulario)
        {
            ManteUdoAdenda manteUdoAdenda = new ManteUdoAdenda();

            if (formulario.Mode == BoFormMode.fm_ADD_MODE || formulario.Mode == BoFormMode.fm_UPDATE_MODE)
            {
                adenda.TipoObjetoAsignado = Adenda.ESTipoObjetoAsignado.SN;
                adenda.ObjetoAsignado = ((EditText)formulario.Items.Item("5").Specific).String;
                adenda.CadenaAdenda = ((EditText)formulario.Items.Item("txtAdn").Specific).String;

                manteUdoAdenda.AlmacenarAdenda(adenda);
            }
        }
    }
}
