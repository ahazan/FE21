using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmImpuestosDgiB1 : FrmBase
    {
        /// <summary>
        /// Agrega los data sources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            CargarGrid(formUID);
        }

        /// <summary>
        /// Establece el data bind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Carga el grid en el formulario
        /// </summary>
        /// <param name="formUid"></param>
        private void CargarGrid(string formUid)
        {
            Grid grdIndImp = (Grid)Formulario.Items.Item("grdDgiB1").Specific;

            ManteUdoImpuestos manteUdoImpuestos = new ManteUdoImpuestos();
            List<Impuesto> listaImpuestos = manteUdoImpuestos.ObtenerRegistros();

            if (listaImpuestos.Count == 0)
            {
                GenerarDatos();
            }

            CargarDatos(grdIndImp);
            BloquearGrid(grdIndImp);
        }

        /// <summary>
        /// Carga los datos al grid
        /// </summary>
        private void CargarDatos(Grid grdImp)
        {
            int j= 0;

            ManteUdoImpuestos manteUdoImpuestos = new ManteUdoImpuestos();
            List<Impuesto> listaImpuestos = manteUdoImpuestos.ObtenerRegistros();
            
            grdImp.DataTable.Rows.Add(listaImpuestos.Count);

            foreach (Impuesto impuesto in listaImpuestos)
            {
                grdImp.DataTable.Columns.Item(0).Cells.Item(j).Value = impuesto.TipoImpuestoDgi;
                grdImp.DataTable.Columns.Item(1).Cells.Item(j).Value = impuesto.Descripcion;
                grdImp.DataTable.Columns.Item(2).Cells.Item(j).Value = impuesto.CodigoImpuestoB1;
                j++;
            }                                
        }

        /// <summary>
        /// Genera los datos por default para la tabla [@TFEIMPDGIB1]
        /// </summary>
        private void GenerarDatos()
        {
            ManteUdoImpuestos manteUdoImpuestos = new ManteUdoImpuestos();
            Impuesto impuesto = null;

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "1";
            impuesto.Descripcion = "Exento de IVA";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "2";
            impuesto.Descripcion = "Gravado a Tasa Mínima";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "3";
            impuesto.Descripcion = "Gravado a Tasa Básica";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "4";
            impuesto.Descripcion = "Gravado a otra Tasa";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "10";
            impuesto.Descripcion = "Exportación y Asimiladas";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "11";
            impuesto.Descripcion = "Impuesto Percibido";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);

            impuesto = new Impuesto();
            impuesto.TipoImpuestoDgi = "12";
            impuesto.Descripcion = "IVA en suspenso";
            impuesto.CodigoImpuestoB1 = "";
            manteUdoImpuestos.Almacenar(impuesto);
        }

        /// <summary>
        /// Actualiza los datos del grid
        /// </summary>
        public void ActualizarDatosGrid()
        {
            Grid gridActualizar = (Grid)Formulario.Items.Item("grdDgiB1").Specific;
            ManteUdoImpuestos manteUdoImpuesto = new ManteUdoImpuestos();
            List<Impuesto> listaDocEntries = manteUdoImpuesto.ObtenerRegistros();
            foreach (Impuesto impuesto in listaDocEntries)
            {
                manteUdoImpuesto.Eliminar(impuesto.DocEntry);
            }

            Impuesto impuestoNuevo = null;
            int f = 0;

            while(f < gridActualizar.DataTable.Rows.Count)
            {
                impuestoNuevo = new Impuesto();
                impuestoNuevo.TipoImpuestoDgi = gridActualizar.DataTable.Columns.Item(0).Cells.Item(f).Value + "";
                impuestoNuevo.Descripcion = gridActualizar.DataTable.Columns.Item(1).Cells.Item(f).Value + "";
                impuestoNuevo.CodigoImpuestoB1 = gridActualizar.DataTable.Columns.Item(2).Cells.Item(f).Value + "";
                manteUdoImpuesto.Almacenar(impuestoNuevo);
                f++;
            }
        }

        /// <summary>
        /// Bloque las celdas de un grid
        /// </summary>
        /// <param name="noBloqueado"></param>
        private void BloquearGrid(Grid noBloqueado)
        {
            int i = 1;

            while(i <= noBloqueado.DataTable.Rows.Count)
            {
                noBloqueado.CommonSetting.SetCellEditable(i, 1, false);
                noBloqueado.CommonSetting.SetCellEditable(i, 2, false);
                i++;
            }
        }
    }
}
