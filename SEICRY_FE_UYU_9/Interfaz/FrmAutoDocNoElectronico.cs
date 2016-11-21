using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmAutoDocNoElectronico:FrmBase
    {
        #region INTERFAZ DE USUARIO        

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources(){}

        /// <summary>
        /// Establece DataBind
        /// </summary>
        protected override void EstablecerDataBind(){}
        
        /// <summary>
        /// Ajusta formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            ((Grid)Formulario.Items.Item("grdUsr").Specific).Columns.Item("Autorizado").Type = BoGridColumnType.gct_CheckBox;
            ((Grid)Formulario.Items.Item("grdUsr").Specific).Columns.Item("Usuario").Editable = false;
            ((Grid)Formulario.Items.Item("grdUsr").Specific).Columns.Item("Nombre").Editable = false;
            ((Grid)Formulario.Items.Item("grdUsr").Specific).Columns.Item("USERID").Visible = false;
            ((Grid)Formulario.Items.Item("grdUsr").Specific).AutoResizeColumns();
        }

        /// <summary>
        /// Establece el estado del botón OK para que sea Actualizar u OK dependiendo del caso
        /// </summary>
        /// <param name="estadoOK"></param>
        public void EstadoBotonOK(bool estadoOK)
        {
            BotonOK = estadoOK;

            //Congelar el Formulario
            Formulario.Freeze(true);

            if (estadoOK)
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = "OK";
            }
            else
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = "Actualizar";
            }

            //Descongelar el Formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Actualiza los datos de los usuarios
        /// </summary>
        public void Actualizar()
        {
            DataTable dtUsuarios = Formulario.DataSources.DataTables.Item("udtUsr");
            ManteUdoUsuarios manteUdoUsuario = new ManteUdoUsuarios();

            for (int i = 0; i < dtUsuarios.Rows.Count; i++)
            {
                manteUdoUsuario.Actualizar(int.Parse(dtUsuarios.Columns.Item(0).Cells.Item(i).Value.ToString()), dtUsuarios.Columns.Item(3).Cells.Item(i).Value.ToString());
                   
            }

            //Muestra mensaje de informacion
            AdminEventosUI.mostrarMensaje(Mensaje.sucOperacionExitosa, AdminEventosUI.tipoExito);
        }

        #endregion INTERFAZ DE USUARIO

        #region PROPIEDADES

        private bool botonOK = true;

        public bool BotonOK
        {
            get { return botonOK; }
            set { botonOK = value; }
        }

        #endregion PROPIEDADES
    }
}
