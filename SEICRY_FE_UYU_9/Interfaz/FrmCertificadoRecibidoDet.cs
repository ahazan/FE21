using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmCertificadoRecibidoDet : FrmBase
    {
        public FrmCertificadoRecibidoDet(int pNumeroComprobante)
        {
            this.NumeroComprobante = pNumeroComprobante;
        }

        /// <summary>
        /// Carga la informacion del grid
        /// </summary>
        /// <param name="formUID"></param>
        public void cargarGrid(string formUID)
        {
            int j = 0;

            //se crea la consulta
            string query = "SELECT DocEntry AS 'NroSap', U_NomItem AS 'Item', U_Cant as 'Cantidad', U_PreUni AS 'Precio Unitario', U_MonIte AS 'Monto Item', U_TpoMon AS 'Tipo Moneda' FROM [@TFESOBRECDET] WHERE DocEntry = " + NumeroComprobante;

            //Se valida si existen datables registrados
            if (Formulario.DataSources.DataTables.Count == 0)
            {
                //Se agrega el dataTable al formulario
                Formulario.DataSources.DataTables.Add("SobreRecDet");

                //Se obtiene el grid del formulario
                Grid gridSobres = (Grid)Formulario.Items.Item("grdSRDet").Specific;

                //Se asigna el dataTable del formulario al grid
                gridSobres.DataTable = Formulario.DataSources.DataTables.Item("SobreRecDet");
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
                Grid gridSobres = (Grid)Formulario.Items.Item("grdSRDet").Specific;

                //Se asigna el dataTable del formulario al grid
                gridSobres.DataTable = Formulario.DataSources.DataTables.Item("SobreRecDet");
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

        #region PROPIEDADES

        private int numeroComprobante;

        public int NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = value; }
        }

        #endregion PROPIEDADES

        protected override void AgregarDataSources()
        {
        }

        protected override void EstablecerDataBind()
        {
        }

        protected override void AjustarFormulario(string formUID)
        {
            cargarGrid(formUID);
        }
    }
}
