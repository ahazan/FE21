using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmEnvioCorreoElectronico : FrmBase
    {
        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Metodo para crear los componentes del formulario de Envio Correo
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            AgregarDataSources();
            EstablecerDataBind();
            LlenarDatos();
        }

        /// <summary>
        /// Metodo para agregar los data sources al formulario
        /// </summary>
        protected override void AgregarDataSources()
        {                     
            Formulario.DataSources.UserDataSources.Add("rbG", SAPbouiCOM
                .BoDataType.dt_SHORT_TEXT, 30);

            Formulario.DataSources.UserDataSources.Add("rbO", SAPbouiCOM.
                BoDataType.dt_SHORT_TEXT, 30);

            Formulario.DataSources.UserDataSources.Add("udsCor",SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);
        }

        /// <summary>
        /// Metodo para agregar los dataBind al formulario
        /// </summary>
        protected override void EstablecerDataBind()
        {
            //Se enlaza el option button para Gmail
            OptionBtn oBGmail = (SAPbouiCOM.OptionBtn)Formulario.Items.Item("rbGmail").Specific;
            oBGmail.DataBind.SetBound(true, "", "rbG");

            //Se enlaza el option button para Outlook
            OptionBtn oBOutlook = (SAPbouiCOM.OptionBtn)Formulario.Items.Item("rbOutlook").Specific;
            oBOutlook.DataBind.SetBound(true, "", "rbO");

            //Se agrupa los optionButton
            oBOutlook.GroupWith("rbGmail");           

            //Se enlaza el campo de texto
            EditText  txtCorreo = (SAPbouiCOM.EditText)Formulario.Items.Item("txtCorreo").Specific;            
            txtCorreo.DataBind.SetBound(true, "", "udsCor");
        }
       
        /// <summary>
        /// Metodo para llenar los campos de texto y los radio buttons
        /// </summary>
        /// <param name="comp"></param>
        private void LlenarDatos()
        {
            ManteUdoCorreos mantenimiento = new ManteUdoCorreos();
            Correo correo = mantenimiento.Consultar();
            if (correo != null)
            {
                EditText txtCorreo = (EditText)Formulario.Items.Item("txtCorreo").Specific;
                txtCorreo.Value = correo.Cuenta;

                if(correo.Opcion.Equals("0"))
                {
                    OptionBtn oBGmail = (OptionBtn)Formulario.Items.Item("rbGmail").Specific;
                    oBGmail.Selected = true;
                }
                else
                {
                    OptionBtn oBOutlook = (OptionBtn)Formulario.Items.Item("rbOutlook").Specific;
                    oBOutlook.Selected = true;
                }
            }
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {            
        }

        #endregion INTERFAZ DE USUARIO
    }
}
