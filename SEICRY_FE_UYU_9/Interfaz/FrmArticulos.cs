using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmArticulos : FrmBase
    {
        private Item cbxIndFac;
        private Item stIndFac;

        #region INTERFAZ DE USUARIO

        public void CrearComponentes(string formUID, string formTypeEx)
        {
            IdFormulario = formUID;

            //Obtiene el formulario
            ObtenerFormulario(formUID);

            Formulario.Freeze(true);

            Item itemReferencia = Formulario.Items.Item("162");

            cbxIndFac = Formulario.Items.Add("cbxIndFac", BoFormItemTypes.it_COMBO_BOX);
            cbxIndFac.Left = itemReferencia.Left;
            cbxIndFac.Top = itemReferencia.Top + itemReferencia.Height + 1;
            cbxIndFac.Width = itemReferencia.Width;
            cbxIndFac.Height = itemReferencia.Height;
            cbxIndFac.ToPane = 6;
            cbxIndFac.FromPane = 6;

            ((ComboBox)cbxIndFac.Specific).ValidValues.Add("-", "-");
            ((ComboBox)cbxIndFac.Specific).ValidValues.Add("6", "Producto no facturable");
            ((ComboBox)cbxIndFac.Specific).ValidValues.Add("7", "Producto no facturable negativo");

            itemReferencia = Formulario.Items.Item("161");

            stIndFac = Formulario.Items.Add("lbIndFac", BoFormItemTypes.it_STATIC);
            stIndFac.Left = itemReferencia.Left;
            stIndFac.Top = itemReferencia.Top + itemReferencia.Height + 1;
            stIndFac.Width = itemReferencia.Width;
            stIndFac.Height = itemReferencia.Height;
            stIndFac.ToPane = 6;
            stIndFac.FromPane = 6;
            ((StaticText)stIndFac.Specific).Caption = "Indicador de Facturación";
            stIndFac.LinkTo = "cbxIndFac";

            ((ComboBox)cbxIndFac.Specific).DataBind.SetBound(true, "OITM", "U_IndFacNF");

            Formulario.Freeze(false);
        }

        protected override void AgregarDataSources()
        {
        }

        protected override void EstablecerDataBind()
        {
            
        }

        protected override void AjustarFormulario(string formUID)
        {

        }

        #endregion INTERFAZ DE USUARIO

        #region PROPIEDADES

        private string idFormulario;

        public string IdFormulario
        {
            get { return idFormulario; }
            set { idFormulario = value; }
        }

        #endregion PROPIEDADES
    }
}
