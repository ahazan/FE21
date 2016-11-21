using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Objetos;


namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene las operaciones para mostrar el Formulario de administración de CAE's
    /// </summary>
    class FrmConfRptd : FrmBase
    {
        ManteUdoConfRptd manteUdoConfRptd = new ManteUdoConfRptd();

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Asigna los data sources al formualario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Asigna los data sources a cada una de las columnas de la matriz y al check box
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            try
            {
                ((OptionBtn)Formulario.Items.Item("rdbMan").Specific).GroupWith("rdbAut");
                ((OptionBtn)Formulario.Items.Item("rdbNo").Specific).GroupWith("rdbSi");

                ConfRptd confRptd = manteUdoConfRptd.ObtenerConfiguracion();
                if (confRptd != null)
                {
                    if (confRptd.ModoEjecucion.Equals("A"))
                    {
                        ((OptionBtn)Formulario.Items.Item("rdbAut").Specific).Selected = true;
                    }
                    else
                    {
                        ((OptionBtn)Formulario.Items.Item("rdbMan").Specific).Selected = true;
                    }


                    if (confRptd.CAEGenerico.Equals("Y"))
                    {
                        ((OptionBtn)Formulario.Items.Item("rdbSi").Specific).Selected = true;
                    }
                    else
                    {
                        ((OptionBtn)Formulario.Items.Item("rdbNo").Specific).Selected = true;
                    }

                    if (confRptd.AutoGenerar.Equals("Y"))
                    {
                        ((CheckBox)Formulario.Items.Item("chkGen").Specific).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)Formulario.Items.Item("chkGen").Specific).Checked = false ;
                    }


                    ((EditText)Formulario.Items.Item("txtFecEje").Specific).Value = confRptd.DiaEjecucion;
                    ((EditText)Formulario.Items.Item("txtSecEnv").Specific).Value = confRptd.SecuenciaEnvio;
                    ((EditText)Formulario.Items.Item("txtFecEjeF").Specific).Value = confRptd.DiaFin;
                    ((EditText)Formulario.Items.Item("txtHora").Specific).Value = confRptd.HoraEjec;
                }
            }
            catch (Exception)
            {                
            }
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        public bool AlmacenarBD()
        {
            bool salida = false;

            ConfRptd confRptd = ObtenerDatos();

            salida = manteUdoConfRptd.AlmacenarConfiguracion(confRptd);

            return salida;
        }

        /// <summary>
        /// Obtiene los datos de la interfaz
        /// </summary>
        /// <returns></returns>
        private ConfRptd ObtenerDatos()
        {
            ConfRptd confRptd = new ConfRptd();

            try
            {
                confRptd.DiaEjecucion = ((EditText)Formulario.Items.Item("txtFecEje").Specific).Value + "";
                confRptd.SecuenciaEnvio = ((EditText)Formulario.Items.Item("txtSecEnv").Specific).Value + "";
                confRptd.HoraEjec = ((EditText)Formulario.Items.Item("txtHora").Specific).Value + "";
                confRptd.DiaFin = ((EditText)Formulario.Items.Item("txtFecEjeF").Specific).Value + "";


                if (((OptionBtn)Formulario.Items.Item("rdbSi").Specific).Selected)
                {
                    confRptd.CAEGenerico = "Y";
                }
                else
                {
                    confRptd.CAEGenerico = "N";
                }

                
                if (((OptionBtn)Formulario.Items.Item("rdbAut").Specific).Selected)
                {
                    confRptd.ModoEjecucion = "A";
                }
                else
                {
                    confRptd.ModoEjecucion = "M";
                }



                if (((CheckBox)Formulario.Items.Item("chkGen").Specific).Checked == true)
                {
                    confRptd.AutoGenerar = "Y";
                }
                else
                {
                    confRptd.ModoEjecucion = "N";
                }




              
            }
            catch (Exception)
            {                
            }

            return confRptd;
        }

        #endregion MANTENIMIENTO


    }
}
