using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmMonitorReporte:FrmBase
    {
        DataTable dtPendientes;
        DataTable dtRechazados;
        DataTable dtAprobados;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Crea los componentes del formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {            
            SeleccionarTab("tab1");
            AgregarDataSources();
            EstablecerDataBind();
            CargarGrids();
            BloquearColumnas();
        }

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
            dtPendientes = Formulario.DataSources.DataTables.Add("dsPen");
            dtRechazados = Formulario.DataSources.DataTables.Add("dsRec");
            dtAprobados = Formulario.DataSources.DataTables.Add("dsApr");
        }

        /// <summary>
        /// Establece DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
            ((Grid)Formulario.Items.Item("grid1").Specific).DataTable = dtPendientes;
            ((Grid)Formulario.Items.Item("grid2").Specific).DataTable = dtRechazados;
            ((Grid)Formulario.Items.Item("grid3").Specific).DataTable = dtAprobados;
        }

        /// <summary>
        /// Carga los grids con los datos dependiendo de cada estado
        /// </summary>
        private void CargarGrids()
        {
            string a = "";


            dtPendientes.ExecuteQuery("select U_FecResum as 'Fecha del Resumen', U_FecRPTD as 'Fecha de Envío', U_SecEnvio as 'Número de Envío' from [@TFERPTD] where U_Estado = '" + RPTD.ESEstadoRPTD.pendiente + "'");

            dtRechazados.ExecuteQuery("select U_FecResum as 'Fecha del Resumen', U_FecRPTD as 'Fecha de Envío', U_SecEnvio as 'Número de Envío' from [@TFERPTD] where U_Estado = '" + RPTD.ESEstadoRPTD.rechazado + "'");

            dtAprobados.ExecuteQuery("select U_FecResum as 'Fecha del Resumen', U_FecRPTD as 'Fecha de Envío', U_SecEnvio as 'Número de Envío' from [@TFERPTD] where U_Estado = '" + RPTD.ESEstadoRPTD.aprobado + "'");
                                  
        }

        /// <summary>
        /// Bloquea las columnas de las tablas
        /// </summary>
        private void BloquearColumnas()
        {
            BloquearColumnasGrid("grid1");
            BloquearColumnasGrid("grid2");
            BloquearColumnasGrid("grid3");
        }

        /// <summary>
        /// Bloque las columnas para un grid determinado
        /// </summary>
        /// <param name="idGrid"></param>
        private void BloquearColumnasGrid(string idGrid)
        {
            int j = 0;
            Grid grd = (Grid)Formulario.Items.Item(idGrid).Specific;

            foreach (GridColumn columna in grd.Columns)
            {
                grd.Columns.Item(j).Editable = false;
                j++;
            }
        }

        /// <summary>
        /// Establece el pane visible en el formulario segun el tab que se seleccione
        /// </summary>
        /// <param name="idTab"></param>
        public void SeleccionarTab(string idTab)
        {
            switch (idTab)
            {
                case "tab1":
                    ((Folder)Formulario.Items.Item(idTab).Specific).Select();
                    Formulario.PaneLevel = 1;
                    break;
                case "tab2":
                    Formulario.PaneLevel = 2;
                    break;
                case "tab3":
                    Formulario.PaneLevel = 3;
                    break;
            }
        }

        /// <summary>
        /// Establece cuales botones estan activos segun el tab seleccionado
        /// </summary>
        /// <param name="modo"></param>
        public void EstablecerBotonesActivos(string modo)
        {
            switch (modo)
            {
                case "tab1":
                    Formulario.Items.Item("btnVis").Enabled = false;
                    Formulario.Items.Item("btnXml").Enabled = false;
                    break;
                case "tab2":
                    Formulario.Items.Item("btnVis").Enabled = false;
                    Formulario.Items.Item("btnXml").Enabled = false;
                    break;
                case "tab3":
                    Formulario.Items.Item("btnVis").Enabled = true;
                    Formulario.Items.Item("btnXml").Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Ajusta Formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        #endregion INTERFAZ DE USUARIO
    }
}
