using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmCertificadoRecibidos : FrmBase
    {
        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Carga la informacion del grid
        /// </summary>
        /// <param name="formUID"></param>
        public void CargarGrid()
        {
            try
            {
                //se crea la consulta
                string query = "SELECT case U_Aprobado when 'Y' then 'Aprobado' when 'N' then 'Rechazado' else 'Pendiente' end as 'Estado',  sf.U_TipoCFE AS 'Tipo CFE', sf.U_SerCom AS 'Serie Comprobante', sf.U_NumCom AS 'Número Comprobante', sf.U_RucEmi AS 'RUC Emisor',sf.U_RazSoc AS 'Razón Social', sf.U_RucRec As " +
                    "'RUC Receptor', sf.U_FeSob AS 'Fecha Sobre', U_IdCons AS 'IdReceptor', DocEntry AS 'NroSAP', sf.U_DNroCAE AS 'NroCAE Desde', sf.U_HNroCAE AS 'NroCAE Hasta', sf.U_NomSob AS 'Nombre Sobre' FROM [@TFESOBREC] AS sf";

                //Se valida si existen datables registrados
                if (Formulario.DataSources.DataTables.Count == 0)
                {
                    //Se agrega el dataTable al formulario
                    Formulario.DataSources.DataTables.Add("SobreRecibido");

                    //Se obtiene el grid del formulario
                    Grid gridSobres = (Grid)Formulario.Items.Item("grdSobRec").Specific;

                    //Se asigna el dataTable del formulario al grid
                    gridSobres.DataTable = Formulario.DataSources.DataTables.Item("SobreRecibido");
                    //Se ejecuta la consulta
                    gridSobres.DataTable.ExecuteQuery(query);

                }
                else
                {
                    //Se obtiene el grid del formulariol
                    Grid gridSobres = (Grid)Formulario.Items.Item("grdSobRec").Specific;

                    //Se asigna el dataTable del formulario al grid
                    gridSobres.DataTable = Formulario.DataSources.DataTables.Item("SobreRecibido");
                    //Se ejecuta la consulta
                    gridSobres.DataTable.ExecuteQuery(query);

                }

                int j = 0;

                //Configura filas a modo no editable
                while (j < ((Grid)Formulario.Items.Item("grdSobRec").Specific).Columns.Count)
                {
                    ((Grid)Formulario.Items.Item("grdSobRec").Specific).Columns.Item(j).Editable = false;
                    j++;
                }

                ((Grid)Formulario.Items.Item("grdSobRec").Specific).Columns.Item("IdReceptor").Visible = false;
                //((Grid)Formulario.Items.Item("grdSobRec").Specific).Columns.Item("DocEntry").Visible = false;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Cargargrid/Error: " + ex.ToString());
            }
        }

        /// <summary>
        /// Agregar Data Sources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Establece dataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Ajustes de formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            CargarGrid();
            //OcultarCeldas();
        }

        /// <summary>
        /// Oculta celdas de un grid
        /// </summary>
        private void OcultarCeldas()
        {
            ((Grid)Formulario.Items.Item("grdSobRec").Specific).Columns.Item("NroCAE").Visible = false;
            ((Grid)Formulario.Items.Item("grdSobRec").Specific).Columns.Item("U_HNroCAE").Visible = false;
        }

        #endregion INTERFAZ DE USUARIO
    }
}
