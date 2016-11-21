using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmMonitor:FrmBase
    {
        DataTable dtPendientesDGI;
        DataTable dtPendientesReceptor;
        DataTable dtRechazadosDGI;
        DataTable dtRechazadosReceptor;
        DataTable dtAprobadosDGI;
        DataTable dtAprobadosReceptor;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Crea los componentes del formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            SeleccionarTab("tab1");
            AgregarDataSources();
            CargarGrids();
            EstablecerDataBind();
            BloquearColumnas();
        }

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
            dtPendientesDGI = Formulario.DataSources.DataTables.Add("dsPenDGI");
            dtPendientesReceptor = Formulario.DataSources.DataTables.Add("dsPenRec");
            dtRechazadosDGI = Formulario.DataSources.DataTables.Add("dsRecDGI");
            dtRechazadosReceptor = Formulario.DataSources.DataTables.Add("dsRecRec");
            dtAprobadosDGI = Formulario.DataSources.DataTables.Add("dsAprDGI");
            dtAprobadosReceptor = Formulario.DataSources.DataTables.Add("dsAprRec");
        }

        /// <summary>
        /// Establece DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
            ((Grid)Formulario.Items.Item("grid1").Specific).DataTable = dtPendientesDGI;
            ((Grid)Formulario.Items.Item("grid2").Specific).DataTable = dtPendientesReceptor;
            ((Grid)Formulario.Items.Item("grid3").Specific).DataTable = dtRechazadosDGI;
            ((Grid)Formulario.Items.Item("grid4").Specific).DataTable = dtRechazadosReceptor;
            ((Grid)Formulario.Items.Item("grid6").Specific).DataTable = dtAprobadosDGI;
            ((Grid)Formulario.Items.Item("grid7").Specific).DataTable = dtAprobadosReceptor;
        }

        /// <summary>
        /// Carga los grids con los datos dependiendo de cada estado
        /// </summary>
        private void CargarGrids()
        {
            string consulta = "";

            consulta = ObtenerConsulta(CFE.ESEstadoCFE.PendienteDGI); 
            dtPendientesDGI.ExecuteQuery(consulta);

            consulta = ObtenerConsulta(CFE.ESEstadoCFE.PendienteReceptor); 
            dtPendientesReceptor.ExecuteQuery(consulta);

            consulta = ObtenerConsulta(CFE.ESEstadoCFE.RechazadoDGI); 
            dtRechazadosDGI.ExecuteQuery(consulta);

            consulta = ObtenerConsulta(CFE.ESEstadoCFE.RechazadoReceptor);
            dtRechazadosReceptor.ExecuteQuery(consulta);

            consulta = ObtenerConsulta56(CFE.ESEstadoCFE.AprobadoDGI, true);             
            dtAprobadosDGI.ExecuteQuery(consulta);

            consulta = ObtenerConsulta56(CFE.ESEstadoCFE.AprobadoReceptor, false);             
            dtAprobadosReceptor.ExecuteQuery(consulta);        

        }

        /// <summary>
        /// Bloquea las columnas de las tablas
        /// </summary>
        private void BloquearColumnas()
        {
            BloquearColumnasGrid("grid1");
            BloquearColumnasGrid("grid2");
            BloquearColumnasGrid("grid3");
            BloquearColumnasGrid("grid4");
            BloquearColumnasGrid("grid6");
            BloquearColumnasGrid("grid7");            
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
                case "tab4":
                    Formulario.PaneLevel = 4;
                    break;
                case "tab5":

                    Formulario.PaneLevel = 5;
                    break;
                case "tab6":

                    Formulario.PaneLevel = 6;
                    break;
                case "tab7":

                    Formulario.PaneLevel = 7;
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
                    break;
                case "tab2":
                    Formulario.Items.Item("btnVis").Enabled = false;
                    break;
                case "tab3":
                    Formulario.Items.Item("btnVis").Enabled = true;
                    break;
                case "tab4":
                    Formulario.Items.Item("btnVis").Enabled = true;
                    break;
                case "tab6":
                    Formulario.Items.Item("btnVis").Enabled = false;
                    break;
                case "tab7":
                    Formulario.Items.Item("btnVis").Enabled = false;
                    break;
                default:
                    break;
            }
        }
       
        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        /// <summary>
        /// Obtiene consulta para grids
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        private string ObtenerConsulta(CFE.ESEstadoCFE estado)
        {
            string resultado = "SELECT T1.U_TipoDoc AS 'Tipo de Documento', " +
            "CASE WHEN U_TipoDoc = '101' THEN 'e-Ticket' WHEN U_TipoDoc = '102' THEN 'NC e-Ticket' WHEN U_TipoDoc = '103' " +
            "THEN 'ND e-Ticket' WHEN U_TipoDoc = '111' THEN 'e-Factura' WHEN U_TipoDoc = '112' THEN 'NC e-Factura' WHEN " +
            "U_TipoDoc = '113' THEN 'ND e-Factura' WHEN U_TipoDoc = '181' THEN 'e-Remito' WHEN U_TipoDoc = '182' THEN " +
            "'e-Resguardo' WHEN U_TipoDoc = '121' THEN 'e-Factura Exportacion' WHEN U_TipoDoc = '122' THEN " +
            "'NC e-Factura Exportacion' WHEN U_TipoDoc = '123' THEN 'ND e-Factura Exportacion' WHEN U_TipoDoc = '124' THEN " +
            "'e-Remito Exportacion' WHEN U_TipoDoc = '201' THEN 'e-Ticket Contigencia' WHEN U_TipoDoc = '202' THEN " +
            "'NC e-Ticket Contingencia' WHEN U_TipoDoc = '203' THEN 'ND e-Ticket Contingencia' WHEN U_TipoDoc = '211' THEN " +
            "'e-Factura Contingencia' WHEN U_TipoDoc = '212' THEN 'NC e-Factura Contingencia' WHEN U_TipoDoc = '213' THEN " +
            "'ND e-Factura Contingencia' WHEN U_TipoDoc = '281' THEN 'e-Remito Contingencia' WHEN U_TipoDoc = '282' THEN " +
            "'e-Resguardo Contingencia' WHEN U_TipoDoc = '221' THEN 'e-Factura Exportacion Contingencia' WHEN U_TipoDoc " +
            "= '222' THEN 'NC e-Factura Exportacion Contingencia' WHEN U_TipoDoc = '223' THEN " +
            "'ND e-Factura Exportacion Contingencia' WHEN U_TipoDoc = '224' THEN 'e-Remito Exportacion Contingencia' ELSE " +
            "'Contingencia' END as 'Documento', T1.U_Serie AS 'Serie', U_NumCFE AS 'Número de Documento', " +
            "CASE WHEN (U_TipoDoc = '111' OR U_TipoDoc = '101' OR U_TipoDoc = '103' OR U_TipoDoc = '113') THEN " +
            "(SELECT DocNum FROM OINV WHERE DocEntry = U_DocSap) WHEN (U_TipoDoc = '112' OR U_TipoDoc = '102') THEN " +
            "(SELECT DocNum FROM ORIN WHERE DocEntry = U_DocSap) WHEN (U_TipoDoc = '181') THEN (SELECT DocNum FROM " +
            "ODLN WHERE DocEntry = U_DocSap) ELSE U_DocSap END AS 'Número de Documento SAP', T1.CreateDate AS " +
            "'Fecha de Creación' FROM [@TFECFE] AS T1 WHERE U_EstadoDgi = '" + estado + "' ORDER BY T1.U_TipoDoc";

            return resultado;
        }


        /// <summary>
        /// Obtiene consulta para grids
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        private string ObtenerConsulta56(CFE.ESEstadoCFE estado, bool cincoSeis)
        {
            string final = "";

            if (cincoSeis)
            {
                final = "CASE T1.U_EstadoRec WHEN 'AprobadoReceptor' " +
                        "THEN 'Aprobado' WHEN 'RechazadoReceptor' THEN 'Rechazado' WHEN 'PendienteReceptor' THEN 'Pendiente' END AS " +
                        "'Estado en Receptor' FROM [@TFECFE] AS T1 WHERE U_EstadoDgi = '"+ estado +"' ORDER BY T1.U_TipoDoc";
            }
            else
            {
                final = "CASE T1.U_EstadoDgi WHEN 'AprobadoDGI' THEN 'Aprobado' WHEN 'RechazadoDGI' THEN 'Rechazado' WHEN " +
                        "'PendienteDGI' THEN 'Pendiente' END AS 'Estado en DGI' FROM [@TFECFE] AS T1 WHERE U_EstadoRec = '" +
                        estado + "'";
            }

            string resultado = "SELECT T1.U_TipoDoc AS 'Tipo de Documento', " +
                "CASE WHEN U_TipoDoc = '101' THEN 'e-Ticket' WHEN U_TipoDoc = '102' THEN 'NC e-Ticket' WHEN U_TipoDoc = '103' " +
                "THEN 'ND e-Ticket' WHEN U_TipoDoc = '111' THEN 'e-Factura' WHEN U_TipoDoc = '112' THEN 'NC e-Factura' WHEN " +
                "U_TipoDoc = '113' THEN 'ND e-Factura' WHEN U_TipoDoc = '181' THEN 'e-Remito' WHEN U_TipoDoc = '182' THEN " +
                "'e-Resguardo' WHEN U_TipoDoc = '121' THEN 'e-Factura Exportacion' WHEN U_TipoDoc = '122' THEN " +
                "'NC e-Factura Exportacion' WHEN U_TipoDoc = '123' THEN 'ND e-Factura Exportacion' WHEN U_TipoDoc = '124' THEN " +
                "'e-Remito Exportacion' WHEN U_TipoDoc = '201' THEN 'e-Ticket Contigencia' WHEN U_TipoDoc = '202' THEN " +
                "'NC e-Ticket Contingencia' WHEN U_TipoDoc = '203' THEN 'ND e-Ticket Contingencia' WHEN U_TipoDoc = '211' THEN " +
                "'e-Factura Contingencia' WHEN U_TipoDoc = '212' THEN 'NC e-Factura Contingencia' WHEN U_TipoDoc = '213' THEN " +
                "'ND e-Factura Contingencia' WHEN U_TipoDoc = '281' THEN 'e-Remito Contingencia' WHEN U_TipoDoc = '282' THEN " +
                "'e-Resguardo Contingencia' WHEN U_TipoDoc = '221' THEN 'e-Factura Exportacion Contingencia' WHEN U_TipoDoc = " +
                "'222' THEN 'NC e-Factura Exportacion Contingencia' WHEN U_TipoDoc = '223' THEN " +
                "'ND e-Factura Exportacion Contingencia' WHEN U_TipoDoc = '224' THEN 'e-Remito Exportacion Contingencia' ELSE " +
                "'Contingencia' END AS 'Documento', T1.U_Serie AS 'Serie', U_NumCFE AS 'Número de Documento', CASE WHEN " +
                "(U_TipoDoc = '111' OR U_TipoDoc = '101' OR U_TipoDoc = '103' OR U_TipoDoc = '113') THEN " +
                "(SELECT DocNum FROM OINV WHERE DocEntry = U_DocSap) WHEN (U_TipoDoc = '112' OR U_TipoDoc = '102') THEN " +
                "(SELECT DocNum FROM ORIN WHERE DocEntry = U_DocSap) WHEN (U_TipoDoc = '181') THEN (SELECT DocNum FROM " +
                "ODLN WHERE DocEntry = U_DocSap) ELSE U_DocSap END AS " +
                "'Número de Documento SAP', T1.CreateDate AS 'Fecha de Creación', " + final;                
            
            return resultado;
        }

        #endregion INTERFAZ DE USUARIO        

        #region VALIDACIONES

        /// <summary>
        /// Obtiene el grid de acuerdo al nivel seleccionado
        /// </summary>
        /// <param name="paneLevel"></param>
        /// <param name="FormUID"></param>
        /// <returns></returns>
        public Grid obtenerGridNivel(int paneLevel, string FormUID)
        {
            Application app = SAPbouiCOM.Framework.Application.SBO_Application;
            Grid respuesta = null;

            if (paneLevel == 1)
            {
                respuesta = (Grid)app.Forms.Item(FormUID).Items.Item("grid1").Specific;
            }
            else if (paneLevel == 2)
            {
                respuesta = (Grid)app.Forms.Item(FormUID).Items.Item("grid2").Specific;
            }
            else if (paneLevel == 3)
            {
                respuesta = (Grid)app.Forms.Item(FormUID).Items.Item("grid3").Specific;
            }
            else if (paneLevel == 4)
            {
                respuesta = (Grid)app.Forms.Item(FormUID).Items.Item("grid4").Specific;
            }           
            else if (paneLevel == 6)
            {
                respuesta = (Grid)app.Forms.Item(FormUID).Items.Item("grid6").Specific;
            }
            else if (paneLevel == 7)
            {
                respuesta = (Grid)app.Forms.Item(FormUID).Items.Item("grid7").Specific;
            }

            return respuesta;
        }

        #endregion VALIDACIONES
    }
}
