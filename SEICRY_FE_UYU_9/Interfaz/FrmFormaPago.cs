using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmFormaPago : FrmBase
    {
        public static string docEntry = "";

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Agregar DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Establece el DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Ajustar el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            ManteUdoFormaPago manteUdoFormaPago = new ManteUdoFormaPago();
            string formaPago = manteUdoFormaPago.ObtenerDocEntryFormaPago(false);

            if (!formaPago.Equals(""))
            {
                ComboBox cbxFpg = (ComboBox)Formulario.Items.Item("cbxFpg").Specific;
                cbxFpg.Select(formaPago, BoSearchKey.psk_ByValue);
            }

            ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();

            string ciGenerico = manteUdoAdobe.ObtenerCiGenerico();
            if (!ciGenerico.Equals(""))
            {
                ((EditText)Formulario.Items.Item("txtCi").Specific).Value = ciGenerico;
            }
        }

        /// <summary>
        /// Guarda los datos de C.I
        /// </summary>
        public void GuardarCI()
        {
            string CIGenerico = "";

            try
            {
                CIGenerico = ((EditText)Formulario.Items.Item("txtCi").Specific).Value.ToString();
                ManteUdoAdobe manteUdoAdobe = new ManteUdoAdobe();
                manteUdoAdobe.AlmacenarCI(CIGenerico);
            }
            catch (Exception)
            {
            }
        }

        #endregion INTERFAZ DE USUARIO

        #region VALIDACION
        
        #endregion VALIDACION
    }
}
