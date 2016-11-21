using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmVisualizarSobreFactura : FrmBase
    {
        #region INTERFAZ DE USUARIO
        
        /// <summary>
        /// Agrega Data Sourcer
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

        /// <summary>
        /// Metodo para crear los componentes del formulario
        /// </summary>
        /// <param name="FormUID"></param>
        public void CrearComponentess(string query)
        {
            LlenarDatos(query);
        }

        /// <summary>
        /// Metodo para llenar los campos del formulario
        /// </summary>
        /// <param name="cerRechazado"></param>
        /// <param name="formUID"></param>
        private void LlenarDatos(string query)
        {
            string formUID = "frmVisSob";
            Application app = SAPbouiCOM.Framework.Application.SBO_Application;

            //Se agrega un dataTable al formulario
            app.Forms.Item(formUID).DataSources.DataTables.Add("SobFac");
            //Se ejecuta una consulta para llenar el dataTable
            app.Forms.Item(formUID).DataSources.DataTables.Item("SobFac").ExecuteQuery(query);
 
            Grid grdCertificadosRechazados = (Grid)app.Forms.Item(formUID).Items.Item("grdSobFac").Specific;

            //Se llena el grid con la informacion del dataTable
            grdCertificadosRechazados.DataTable = app.Forms.Item(formUID).DataSources.DataTables.Item("SobFac");

            int cantFilas = grdCertificadosRechazados.Columns.Count, j = 0;
            //Se hacen no editables las filas del grid
            while(j < cantFilas)
            {
                grdCertificadosRechazados.Columns.Item(j).Editable = false;
                j++;
            }

        }
        #endregion INTERFAZ DE USUARIO
    }
}
