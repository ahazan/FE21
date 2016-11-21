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
    class FrmConfTipoCambio : FrmBase
    {
        ManteUdoTipoCambio manteUdoTipoCambio = new ManteUdoTipoCambio();

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Agregar DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {

        }

        /// <summary>
        /// Establecer DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {            
        }

        /// <summary>
        /// Ajustar el formulario
        /// </summary>
        protected override void AjustarFormulario(string formUID)
        {
            string tipoCambio = "", docEntry = "";

            try
            {
                Formulario.Freeze(true);
                ((OptionBtn)Formulario.Items.Item("cbxDol").Specific).GroupWith("cbxLoc");

                tipoCambio = manteUdoTipoCambio.ObtenerConfiguracion(out docEntry);
                if (!tipoCambio.Equals(""))
                {
                    if (tipoCambio.Equals("Y"))
                    {
                        ((OptionBtn)Formulario.Items.Item("cbxLoc").Specific).Selected = true;
                    }
                    else
                    {
                        ((OptionBtn)Formulario.Items.Item("cbxDol").Specific).Selected = true;
                    }                    
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Formulario.Freeze(false);
            }
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        public bool AlmacenarBD()
        {
            bool salida = false;

            string tipoCambio = ObtenerDatos();

            salida = manteUdoTipoCambio.AlmacenarConfiguracion(tipoCambio);

            return salida;
        }

        /// <summary>
        /// Obtiene los datos de la interfaz
        /// </summary>
        /// <returns></returns>
        private string ObtenerDatos()
        {
            string resultado = "";

            try
            {
                if (((OptionBtn)Formulario.Items.Item("cbxLoc").Specific).Selected)
                {
                    resultado = "Y";
                }
                else
                {
                    resultado = "N";
                }
            }
            catch (Exception)
            {                
            }

            return resultado;
        }

        #endregion MANTENIMIENTO
    }
}
