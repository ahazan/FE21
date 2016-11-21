using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEICRY_FE_UYU_9;
using SEICRY_FE_UYU_9.Globales;
using SAPbouiCOM;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmEstadoContingencia : FrmBase
    {
        //Valor de estado de contingencia(Y = Activado, N = Desactivado)
        public static string estadoContingencia = "";

        private UserDataSource udsEstCon;
        private SEICRY_FE_UYU_9.Udos.ManteUdoEstadoContingencia manteUdoEstadoContingencia = new Udos.ManteUdoEstadoContingencia();

        #region INTERFAZ DE USUARIO
        

        protected override void AgregarDataSources()
        {
            udsEstCon = Formulario.DataSources.UserDataSources.Add("udsEstCon", BoDataType.dt_SHORT_TEXT);
        }

        protected override void EstablecerDataBind()
        {
            ((CheckBox)Formulario.Items.Item("cbCont").Specific).DataBind.SetBound(true, "", "udsEstCon");
        }

        /// <summary>
        /// Cambia el estado del botón para saber si está en modo OK o Actualizar
        /// </summary>
        /// <param name="estadoOK"></param>
        public void CambiarEstadoBotonOK(bool estadoOK = true)
        {
            EstadoBotonOK = estadoOK;

            if (EstadoBotonOK)
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = "OK";
            }
            else
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = "Actualizar";
            }      
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        /// <summary>
        /// Almacena el estado cuando no existe, o lo actualiza cuando existe
        /// </summary>
        public bool Almacenar()
        {
            bool resultado = false;

            if (manteUdoEstadoContingencia.Consultar().Equals(""))
            {
                resultado = manteUdoEstadoContingencia.Almacenar(((CheckBox)Formulario.Items.Item("cbCont").Specific).Checked ? "Y" : "N");
            }
            else
            {
                resultado = manteUdoEstadoContingencia.Actualizar(((CheckBox)Formulario.Items.Item("cbCont").Specific).Checked ? "Y" : "N");
            }

            if (resultado)
            {
                AdminEventosUI.mostrarMensaje(Mensaje.sucOperacionExitosa, AdminEventosUI.tipoExito);
            }
            else
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errOperacionFallida, AdminEventosUI.tipoError);                
            }

            return resultado;
        }

        /// <summary>
        /// Consulta el estado de contingencia
        /// </summary>
        public void Consultar()
        {
            Formulario.Freeze(true);
            udsEstCon.Value = manteUdoEstadoContingencia.Consultar();
            Formulario.Freeze(false);
        }

        #endregion MANTENIMIENTO

        #region PROPIEDADES

        private bool estadoBotonOK = true;

        public bool EstadoBotonOK
        {
            get { return estadoBotonOK; }
            set { estadoBotonOK = value; }
        }

        #endregion PROPIEDADES

        protected override void AjustarFormulario(string formUID)
        {
            ObtenerFormulario(formUID);
            AgregarDataSources();
            EstablecerDataBind();
            Consultar();
        }
    }
}
