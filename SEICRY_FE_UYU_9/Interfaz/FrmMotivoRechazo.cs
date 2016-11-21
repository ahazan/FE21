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
    class FrmMotivoRechazo : FrmBase
    {
        ManteUdoEstadoSobreRecibido mante = new ManteUdoEstadoSobreRecibido();

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Carga la glosa del rechazo segun el motivo seleccionado
        /// </summary>
        public void SeleccionMotivo()
        {
            Formulario.DataSources.UserDataSources.Item("udsGlosa").Value = ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).Selected.Description;
        }

        /// <summary>
        /// Establece la visualizacion del boton OK
        /// </summary>
        /// <param name="estadoBotonOK"></param>
        public void EstablecerEstadoBotonOK(bool estadoBotonOK = true)
        {
            this.EstadoBotonOK = estadoBotonOK;

            if (Tipo == ETipo.editar)
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = EstadoBotonOK ? "OK" : "Actualizar";
            }
            else if (Tipo == ETipo.almancenar)
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = EstadoBotonOK ? "OK" : "Almacenar";
            }
        }

        protected override void AgregarDataSources()
        {
        }

        protected override void EstablecerDataBind()
        {
        }

        protected override void AjustarFormulario(string formUID)
        {
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("", "");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E20", "Orden de compra vencida");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E21", "Mercadería en mal estado");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E22", "Proveedor inhabilitado por organismo de contralor");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E23", "Contraprestación no recibida");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E24", "Diferencia precios y/o descuentos");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E25", "Factura con error cálculos");
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).ValidValues.Add("E26", "Diferencia con plazos");
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        /// <summary>
        /// Almancena un nuevo motivo asociado al certificado seleccionado (dado por consecutivo)
        /// </summary>
        public bool Almacenar()
        {
            EstadoCertificadoRecibido estadoCertificadoRecibido = new EstadoCertificadoRecibido();

            estadoCertificadoRecibido.IdConsecutivo = IdConsecutivo;
            estadoCertificadoRecibido.Motivo = ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).Selected.Value;
            estadoCertificadoRecibido.Glosa = Formulario.DataSources.UserDataSources.Item("udsGlosa").Value;
            estadoCertificadoRecibido.Detalle = Formulario.DataSources.UserDataSources.Item("udsDetalle").Value;

            return mante.Almacenar(estadoCertificadoRecibido);
        }

        /// <summary>
        /// Edita un motivo seleccionado
        /// </summary>
        public bool Actualizar()
        {
            EstadoCertificadoRecibido estadoCertificadoRecibido = new EstadoCertificadoRecibido();

            estadoCertificadoRecibido.DocEntry = IdMotivo;
            estadoCertificadoRecibido.IdConsecutivo = IdConsecutivo;
            estadoCertificadoRecibido.Motivo = ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).Selected.Value;
            estadoCertificadoRecibido.Glosa = Formulario.DataSources.UserDataSources.Item("udsGlosa").Value;
            estadoCertificadoRecibido.Detalle = Formulario.DataSources.UserDataSources.Item("udsDetalle").Value;

            return mante.Actualizar(estadoCertificadoRecibido);
        }

        /// <summary>
        /// Consulta los datos del motivo seleccionado
        /// </summary>
        public void Consultar(string idMotivo)
        {
            EstadoCertificadoRecibido estadoCerRecibido = mante.Consultar(idMotivo);

            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).Select(estadoCerRecibido.Motivo, BoSearchKey.psk_ByValue);

            Formulario.DataSources.UserDataSources.Item("udsGlosa").Value = estadoCerRecibido.Glosa;
            Formulario.DataSources.UserDataSources.Item("udsDetalle").Value = estadoCerRecibido.Detalle;
        }

        /// <summary>
        /// Elimina el motivo seleccionado
        /// </summary>
        public bool Eliminar(string idMotivo)
        {
            return mante.Eliminar(idMotivo);
        }

        /// <summary>
        /// Limpia los campos de formulario
        /// </summary>
        public void LimpiarCampos()
        {
            ((ComboBox)Formulario.Items.Item("cbxMotivo").Specific).Select("", BoSearchKey.psk_ByValue);

            Formulario.DataSources.UserDataSources.Item("udsGlosa").Value = "";
            Formulario.DataSources.UserDataSources.Item("udsDetalle").Value = "";
        }

        #endregion MANTENIMIENTO

        #region VALIDACIONES

        /// <summary>
        /// Valida que se hayan ingresado todos los valores
        /// </summary>
        /// <returns></returns>
        public bool ValidarCampos()
        {
            if (Formulario.DataSources.UserDataSources.Item("udsGlosa").Value.Equals("") || Formulario.DataSources.UserDataSources.Item("udsDetalle").Value.Equals(""))
            {
                return false;
            }

            return true;
        }

        #endregion VALIDACIONES

        #region PROPIEDADES

        private string idConsecutivo;

        public string IdConsecutivo
        {
            get { return idConsecutivo; }
            set { idConsecutivo = value; }
        }

        public enum ETipo
        {
            almancenar = 1,
            editar = 2
        }

        private ETipo tipo;

        internal ETipo Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private string idMotivo;

        public string IdMotivo
        {
            get { return idMotivo; }
            set { idMotivo = value; }
        }

        private bool estadoBotonOK;

        public bool EstadoBotonOK
        {
            get { return estadoBotonOK; }
            set { estadoBotonOK = value; }
        }

        #endregion PROPIEDADES

    }
}
