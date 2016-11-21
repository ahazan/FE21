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
    class FrmMonitorSobre : FrmBase
    {
        #region INTERFAZ DE USUARIO
        
        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {            
        }

        /// <summary>
        /// Establece el DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {           
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
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
            string query = "SELECT case sd.U_EstRecEnv when 'BS' then 'Rechazado' else 'Aprobado' end AS 'Estado Recepcion', s.U_VerSobre AS Versión, s.U_RucRec AS 'RUC Receptor', s.U_RucEmi AS 'RUC Emisor', s.U_IdResp AS 'ID Respuesta', s.U_NomArc AS 'Nombre Archivo', s.U_FeHoEnRe AS 'FechaHora Recepcion', s.U_IdEmi AS 'ID Emisor', s.U_IdRec AS 'ID Receptor', s.U_CantComp As 'Cantidad Comprobantes', s.U_FeHoFiEl AS 'HoraFirma Electronica', sd.U_CodMotRec AS 'Codigo Motivo Rechazo', sd.U_GloMotRec AS 'Glosa Motivo Rechazo', sd.U_DetRec AS 'Detalle Rechazo' FROM [@TFESOB] AS s LEFT JOIN [@TFESOBDET] AS sd ON (s.DocEntry = sd.DocEntry OR s.DocEntry IS NULL) ";

            //Se valida si existen datables registrados
            if (Formulario.DataSources.DataTables.Count == 0)
            {
                //Se agrega el dataTable al formulario
                Formulario.DataSources.DataTables.Add("Sobre");

                //Se obtiene el grid del formulario
                Grid gridSobres = (Grid)Formulario.Items.Item("grdSob").Specific;

                //Se asigna el dataTable del formulario al grid
                gridSobres.DataTable = Formulario.DataSources.DataTables.Item("Sobre");
                //Se ejecuta la consulta
                gridSobres.DataTable.ExecuteQuery(query);

                //Configura filas a modo no editable
                while (j < gridSobres.Columns.Count)
                {
                    gridSobres.Columns.Item(j).Editable = false;
                    j++;
                }
            }
            else
            {
                //Se obtiene el grid del formulariol
                Grid gridSobres = (Grid)Formulario.Items.Item("grdSob").Specific;

                //Se asigna el dataTable del formulario al grid
                gridSobres.DataTable = Formulario.DataSources.DataTables.Item("Sobre");
                //Se ejecuta la consulta
                gridSobres.DataTable.ExecuteQuery(query);

                //Configura filas a modo no editable
                while (j <= gridSobres.Rows.Count)
                {
                    gridSobres.CommonSetting.SetRowEditable(j, false);
                    j++;
                }
            }
        }
        #endregion INTERFAZ DE USUARIO
    }
}
