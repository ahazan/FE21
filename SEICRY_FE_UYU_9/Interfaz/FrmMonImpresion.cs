using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmMonImpresion:FrmBase
    {
        DataTable dtPendientesPdf;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Crea los componentes del formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            AgregarDataSources();
            CargarGrid();
            EstablecerDataBind();
            BloquearColumnasGrid("grdPdf");
        }

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
            dtPendientesPdf = Formulario.DataSources.DataTables.Add("dsPenPdf");            
        }

        /// <summary>
        /// Establece DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
            ((Grid)Formulario.Items.Item("grdPdf").Specific).DataTable = dtPendientesPdf;
            
        }

        /// <summary>
        /// Carga los grids con los datos dependiendo de cada estado
        /// </summary>
        private void CargarGrid()
        {
            dtPendientesPdf.ExecuteQuery("SELECT U_ArcPdf AS 'Nombre Archivo', CreateDate AS 'Fecha Creación' FROM [@TFEPDF]");                
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
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        #endregion INTERFAZ DE USUARIO        
    }
}
