using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmMonitorAnulado : FrmBase
    {
        private DataTable dtAnulados;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Crea componentes del formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            cargarGrid(formUID);
        }


         /// <summary>
         /// Metodo para cargar datos en el grid de certificados anulados por la DGI
         /// </summary>
         /// <param name="formUID"></param>
        public void cargarGrid(string formUID)
        {
            int j = 0;

            //se crea la consulta
            string query = "SELECT cr.DocEntry as 'DocEntry', cr.U_Version AS Versión, cr.U_RucEmisor AS RucEmisor, cr.U_RucRecep AS RucReceptor, cr.U_CantComp AS 'Cantidad Comprobantes'," +
            "cr.U_FeHoFir AS 'Fecha-Hora Firma', crd.U_TipoCFE AS TipoCFE , crd.U_SerieComp AS 'Serie Comprobante', crd.U_NumComp AS 'Número Comprobante',"+
            "crd.U_FecComp AS 'Fecha Comprobante', crd.U_CodAnu AS 'Código Anulación',crd.U_GlosaDoc AS 'Glosa Motivo Rechazo', cr.U_Corregido as 'Corregido Con' FROM [@TFECEANU] AS cr inner join [@TFECEANUDET] AS crd ON cr.DocEntry"+ 
            "= crd.LineId ";

           
            //Se valida si existen datables registrados
            if (Formulario.DataSources.DataTables.Count == 0)
            {
                //Se agrega el dataTable al formulario
                dtAnulados = Formulario.DataSources.DataTables.Add("Anulado");

                //Se obtiene el grid del formulario
                Grid gridAnulados = (Grid)Formulario.Items.Item("grdAnu").Specific;

                //Se asigna el dataTable del formulario al grid
                gridAnulados.DataTable = Formulario.DataSources.DataTables.Item("Anulado");

                //Se ejecuta la consulta
                gridAnulados.DataTable.ExecuteQuery(query);

                //Configura filas a modo no editable
                while (j < gridAnulados.Columns.Count - 1)
                {
                    if (j == 0)
                    {
                        gridAnulados.Columns.Item(j).Visible = false;
                    }

                    gridAnulados.Columns.Item(j).Editable = false;
                    j++;
                }
            }
            else
            {
                //Se obtiene el grid del formulariol
                Grid gridAnulados = (Grid)Formulario.Items.Item("grdAnu").Specific;

                //Se asigna el dataTable del formulario al grid
                gridAnulados.DataTable = Formulario.DataSources.DataTables.Item("Anulado");
                //Se ejecuta la consulta
                gridAnulados.DataTable.ExecuteQuery(query);

                //Configura filas a modo no editable
                while (j < gridAnulados.Columns.Count)
                {
                    gridAnulados.Columns.Item(j).Editable = false;
                    j++;
                }
            }
        }

        /// <summary>
        /// Modifica el estado del botón. OK o Actualizar dependiendo del caso
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

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Establece DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Ajusta Formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        /// <summary>
        /// Actualiza el valor ingresado en la columna "corregido con"
        /// </summary>
        public void Actualizar()
        {
            //Se obtiene el grid del formulariol
            Grid gridAnulados = (Grid)Formulario.Items.Item("grdAnu").Specific;
            Anulado anulado;
            System.Collections.ArrayList listaAnulados = new System.Collections.ArrayList();
            ManteUdoCertificadoAnulado manteAnulado = new ManteUdoCertificadoAnulado();

            for (int i = 0; i < gridAnulados.Rows.Count; i++)
            {
                anulado = new Anulado();

                anulado.DocEntry = dtAnulados.GetValue("DocEntry", i).ToString();
                anulado.CorregidoCon = dtAnulados.GetValue("Corregido Con", i).ToString();

                listaAnulados.Add(anulado);           
            }

            if (manteAnulado.ActualizarMaestro(listaAnulados))
            {
                AdminEventosUI.mostrarMensaje(Mensaje.sucOperacionExitosa, AdminEventosUI.tipoExito);
            }
            else
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errOperacionFallida, AdminEventosUI.tipoError);
            }
        }

        #endregion MANTENIMINETO

        #region PROPIEDADES

        private bool estadoBotonOK = true;

        public bool EstadoBotonOK
        {
            get { return estadoBotonOK; }
            set { estadoBotonOK = value; }
        }

        #endregion PROPIEDADES
    }
}
