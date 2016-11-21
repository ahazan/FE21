using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene las operaciones para mostrar el formulario de rangos de documentos electronicos
    /// </summary>
    class FrmVisualizarRangos : FrmBase
    {
        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Agrega los dataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Asigna el data sources al grid
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Obtener los datos de la base de datos y cargar el grid
        /// </summary>
        public void CargarGrid(bool activos, Grid grid)
        {
            string consulta;

            if (activos)
            {
                consulta = "select T1.U_TipoDoc as 'Tipo de Documento', T2.U_NombDoc as 'Nombre de Documento', T2.U_Serie as 'Serie', T1.U_NumIni as 'Número Inicial', T1.U_NumFin as 'Número Final', T1.U_NumAct as 'Número Actual', T1.U_ValHasta as 'Fecha de Vencimiento', T2.U_Sucursal as 'Sucursal', T2.U_Caja as 'Caja' from [@TFERANGO] as T1 inner join [@TFECAE] as T2 on T1.U_IdCAE = T2.DocEntry and T1.U_Activo = 'Y'";
            }
            else
            {
                //Establecer la consulta 
                consulta = "select T1.U_TipoDoc as 'Tipo de Documento', T2.U_NombDoc as 'Nombre de Documento', T2.U_Serie as 'Serie', T1.U_NumIni as 'Número Inicial', T1.U_NumFin as 'Número Final', T1.U_NumAct as 'Número Actual', T1.U_ValHasta as 'Fecha de Vencimiento', T2.U_Sucursal as 'Sucursal', T2.U_Caja as 'Caja' from [@TFERANGO] as T1 inner join [@TFECAE] as T2 on T1.U_IdCAE = T2.DocEntry";
            }

            //Ejecuta la consulta a la base de datos
            grid.DataTable.ExecuteQuery(consulta);
        }

        /// <summary>
        /// Ajusta las propiedades de las columnas para que no sean editables.
        /// </summary>
        private void AjustarColumnas(Grid grid)
        {
            Formulario.Freeze(true);

            grid.Columns.Item("Tipo de Documento").Editable = false;
            grid.Columns.Item("Nombre de Documento").Editable = false;
            grid.Columns.Item("Serie").Editable = false;
            grid.Columns.Item("Número Inicial").Editable = false;
            grid.Columns.Item("Número Final").Editable = false;
            grid.Columns.Item("Número Actual").Editable = false;
            grid.Columns.Item("Fecha de Vencimiento").Editable = false;
            grid.Columns.Item("Sucursal").Editable = false;
            grid.Columns.Item("Caja").Editable = false;

            Formulario.Freeze(false);
        }

        /// <summary>
        /// Asigna un CAE
        /// </summary>
        public void AsignarCAE(int filaSeleccionada)
        {
            Formulario.Freeze(true);

            Grid gridDisponible = (Grid)Formulario.Items.Item("grdQui").Specific;
            Grid gridAsignados = (Grid)Formulario.Items.Item("grdAsi").Specific;

            string tipoDocumento = gridDisponible.DataTable.Columns.Item(0).Cells.Item(filaSeleccionada).Value + "";
            string nombreDocumento = gridDisponible.DataTable.Columns.Item(1).Cells.Item(filaSeleccionada).Value + "";
            string serie = gridDisponible.DataTable.Columns.Item(2).Cells.Item(filaSeleccionada).Value + "";
            string numeroInicial = gridDisponible.DataTable.Columns.Item(3).Cells.Item(filaSeleccionada).Value + "";
            string numeroFinal = gridDisponible.DataTable.Columns.Item(4).Cells.Item(filaSeleccionada).Value + "";
            string numeroActual = gridDisponible.DataTable.Columns.Item(5).Cells.Item(filaSeleccionada).Value + "";
            string fechaVencimiento = gridDisponible.DataTable.Columns.Item(6).Cells.Item(filaSeleccionada).Value + "";
            string sucursal = gridDisponible.DataTable.Columns.Item(7).Cells.Item(filaSeleccionada).Value + "";
            string caja = gridDisponible.DataTable.Columns.Item(8).Cells.Item(filaSeleccionada).Value + "";

            if (gridAsignados.DataTable.Rows.Count > 0)
            {
                if (!gridAsignados.DataTable.Columns.Item(0).Cells.Item(0).Value.ToString().Equals(""))
                {
                    gridAsignados.DataTable.Rows.Add(1);
                }
            }
            else
            {
                gridAsignados.DataTable.Rows.Add(1);
            }

            gridAsignados.DataTable.SetValue(0, (gridAsignados.Rows.Count - 1), tipoDocumento);
            gridAsignados.DataTable.SetValue(1, (gridAsignados.Rows.Count - 1), nombreDocumento);
            gridAsignados.DataTable.SetValue(2, (gridAsignados.Rows.Count - 1), serie);
            gridAsignados.DataTable.SetValue(3, (gridAsignados.Rows.Count - 1), numeroInicial);
            gridAsignados.DataTable.SetValue(4, (gridAsignados.Rows.Count - 1), numeroFinal);
            gridAsignados.DataTable.SetValue(5, (gridAsignados.Rows.Count - 1), numeroActual);
            gridAsignados.DataTable.SetValue(6, (gridAsignados.Rows.Count - 1), DateTime.Parse(fechaVencimiento));
            gridAsignados.DataTable.SetValue(7, (gridAsignados.Rows.Count - 1), sucursal);
            gridAsignados.DataTable.SetValue(8, (gridAsignados.Rows.Count - 1), caja);

            gridDisponible.DataTable.Rows.Remove(filaSeleccionada);

            Formulario.Freeze(false);
        }

        /// <summary>
        /// Quita un CAE de la seccion de activos
        /// </summary>
        public void QuitarCAE()
        {
            Formulario.Freeze(true);

            Grid gridDisponible = (Grid)Formulario.Items.Item("grdQui").Specific;
            Grid gridAsignados = (Grid)Formulario.Items.Item("grdAsi").Specific;

            if (gridAsignados.Rows.SelectedRows.Count > 0)
            {
                //Obtiene el numero de la fila seleccionada
                int filaSeleccionada = gridAsignados.Rows.SelectedRows.Item(0, BoOrderType.ot_SelectionOrder);
                string tipoDocumento = gridAsignados.DataTable.Columns.Item(0).Cells.Item(filaSeleccionada).Value + "";
                string nombreDocumento = gridAsignados.DataTable.Columns.Item(1).Cells.Item(filaSeleccionada).Value + "";
                string serie = gridAsignados.DataTable.Columns.Item(2).Cells.Item(filaSeleccionada).Value + "";
                string numeroInicial = gridAsignados.DataTable.Columns.Item(3).Cells.Item(filaSeleccionada).Value + "";
                string numeroFinal = gridAsignados.DataTable.Columns.Item(4).Cells.Item(filaSeleccionada).Value + "";
                string numeroActual = gridAsignados.DataTable.Columns.Item(5).Cells.Item(filaSeleccionada).Value + "";
                string fechaVencimiento = gridAsignados.DataTable.Columns.Item(6).Cells.Item(filaSeleccionada).Value + "";
                string sucursal = gridAsignados.DataTable.Columns.Item(7).Cells.Item(filaSeleccionada).Value + "";
                string caja = gridAsignados.DataTable.Columns.Item(8).Cells.Item(filaSeleccionada).Value + "";

                //Agrega una fila al grid
                if (gridDisponible.DataTable.Rows.Count > 0)
                {
                    if (!gridDisponible.DataTable.Columns.Item(0).Cells.Item(0).Value.ToString().Equals(""))
                    {
                        gridDisponible.DataTable.Rows.Add(1);
                    }
                }
                else
                {
                    gridDisponible.DataTable.Rows.Add(1);
                }

                //Agrega los valores al grid
                gridDisponible.DataTable.SetValue(0, (gridDisponible.Rows.Count - 1), tipoDocumento);
                gridDisponible.DataTable.SetValue(1, (gridDisponible.Rows.Count - 1), nombreDocumento);
                gridDisponible.DataTable.SetValue(2, (gridDisponible.Rows.Count - 1), serie);
                gridDisponible.DataTable.SetValue(3, (gridDisponible.Rows.Count - 1), numeroInicial);
                gridDisponible.DataTable.SetValue(4, (gridDisponible.Rows.Count - 1), numeroFinal);
                gridDisponible.DataTable.SetValue(5, (gridDisponible.Rows.Count - 1), numeroActual);
                gridDisponible.DataTable.SetValue(6, (gridDisponible.Rows.Count - 1), DateTime.Parse(fechaVencimiento));
                gridDisponible.DataTable.SetValue(7, (gridDisponible.Rows.Count - 1), sucursal);
                gridDisponible.DataTable.SetValue(8, (gridDisponible.Rows.Count - 1), caja);

                //Elimina la fila que se seleciono
                gridAsignados.DataTable.Rows.Remove(filaSeleccionada);
            }
            else
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Debe seleccionar una fila para poder quitar el CAE Asignado", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Valida si el CAE es valido para asignarlo
        /// </summary>
        /// <returns></returns>
        public bool ValidarCAE(out int filaSeleccionadaGrid)
        {
            bool salida = true;

            Formulario.Freeze(true);

            Grid gridDisponible = (Grid)Formulario.Items.Item("grdQui").Specific;
            Grid gridAsignados = (Grid)Formulario.Items.Item("grdAsi").Specific;

            if (gridDisponible.Rows.SelectedRows.Count > 0)
            {
                int filaSeleccionada = gridDisponible.Rows.SelectedRows.Item(0, BoOrderType.ot_SelectionOrder);
                filaSeleccionadaGrid = filaSeleccionada;
                string tipoDocumento = gridDisponible.DataTable.Columns.Item(0).Cells.Item(filaSeleccionada).Value + "";
                string numeroInicial = gridDisponible.DataTable.Columns.Item(3).Cells.Item(filaSeleccionada).Value + "";
                string numeroActual = gridDisponible.DataTable.Columns.Item(5).Cells.Item(filaSeleccionada).Value + "";
                string fechaVencimiento = gridDisponible.DataTable.Columns.Item(6).Cells.Item(filaSeleccionada).Value + "";

                if (DateTime.Parse(fechaVencimiento) < DateTime.Now)
                {
                    salida = false;
                    AdminEventosUI.mostrarMensaje("Error: No se puede asignar el CAE, la fecha del CAE: " + fechaVencimiento + " es inválida.", AdminEventosUI.tipoMensajes.error);
                }
                else if (int.Parse(numeroActual) > int.Parse(numeroInicial))
                {
                    salida = false;
                    AdminEventosUI.mostrarMensaje("Error: No se puede asignar el CAE, el CAE ya ha sido utilizado", AdminEventosUI.tipoMensajes.error);
                }
                else
                {
                    int i = 0;
                    while (i < gridAsignados.DataTable.Rows.Count)
                    {
                        if (tipoDocumento.Equals(gridAsignados.DataTable.Columns.Item(0).Cells.Item(i).Value))
                        {
                            salida = false;
                            AdminEventosUI.mostrarMensaje("Error: No se puede asignar el CAE, tipo de documento ya activado", AdminEventosUI.tipoMensajes.error);
                            break;
                        }
                        i++;
                    }
                }
            }
            else
            {
                filaSeleccionadaGrid = -1;
                AdminEventosUI.mostrarMensaje("Debe seleccionar una fila para poder realizar la asignación del CAE", AdminEventosUI.tipoMensajes.error);
            }

            Formulario.Freeze(false);

            return salida;
        }

        /// <summary>
        /// Ajusta Formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            AjustarColumnas(((Grid)Formulario.Items.Item("grdAsi").Specific));
            AjustarColumnas(((Grid)Formulario.Items.Item("grdQui").Specific));
        }

        /// <summary>
        /// Actualiza los rangos activos
        /// </summary>
        /// <returns></returns>
        public bool ActualizarRangosActivos(string nombreGrid, string estado)
        {
            bool salida = true, salidaTemporal = false;
            int i = 0;
            string docEntry = "";

            Grid gridRango = (Grid)Formulario.Items.Item(nombreGrid).Specific;

            while (i < gridRango.DataTable.Rows.Count)
            {
                RangoCAE rangoCAE = new RangoCAE();

                rangoCAE.TipoDocumento = gridRango.DataTable.Columns.Item(0).Cells.Item(i).Value + "";
                rangoCAE.Serie = gridRango.DataTable.Columns.Item(2).Cells.Item(i).Value + "";
                rangoCAE.NumeroInicial = gridRango.DataTable.Columns.Item(3).Cells.Item(i).Value + "";
                rangoCAE.NumeroFinal = gridRango.DataTable.Columns.Item(4).Cells.Item(i).Value + "";
                rangoCAE.NumeroActual = gridRango.DataTable.Columns.Item(5).Cells.Item(i).Value + "";
                rangoCAE.FechaVencimiento = gridRango.DataTable.Columns.Item(6).Cells.Item(i).Value + "";

                ManteUdoRango manteUdoRango = new ManteUdoRango();

                docEntry = manteUdoRango.ObtenerDocEntry(rangoCAE);

                if (!docEntry.Equals(""))
                {
                    salidaTemporal = manteUdoRango.ActivarRango(docEntry, estado);

                    if (!salidaTemporal)
                    {
                        salida = false;
                    }
                }
                else
                {
                    salida = false;
                }
                i++;
            }

            return salida;
        }

        #endregion INTERFAZ DE USUARIO
    }
}
