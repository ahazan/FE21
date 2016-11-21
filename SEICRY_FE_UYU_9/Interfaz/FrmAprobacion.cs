using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SEICRY_FE_UYU_9.ACKS;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmAprobacion : FrmBase
    {
        #region INTERFAZ DE USUARIO

        public void MostrarRechazo(bool visible)
        {
            if (visible)
            {
                Formulario.PaneLevel = 1;
            }
            else
            {
                Formulario.PaneLevel = 0;
            }
        }

        /// <summary>
        /// Carga la informacion del grid
        /// </summary>
        /// <param name="formUID"></param>
        public void CargarGrid()
        {
            int j = 0;

            //se crea la consulta
            string query = "select DocEntry, U_Motivo as 'Motivo', U_Glosa as 'Glosa', U_Detalle as 'Detalle' from [@TFEESTCFER] where U_ConsRec = " + DocEntry;

            //Se valida si existen datables registrados
            if (Formulario.DataSources.DataTables.Count == 0)
            {
                //Se agrega el dataTable al formulario
                Formulario.DataSources.DataTables.Add("dtAprob");

                //Se obtiene el grid del formulario
                Grid gridSobres = (Grid)Formulario.Items.Item("grdMotRec").Specific;

                //Se asigna el dataTable del formulario al grid
                gridSobres.DataTable = Formulario.DataSources.DataTables.Item("dtAprob");
                //Se ejecuta la consulta
                gridSobres.DataTable.ExecuteQuery(query);
            }
            else
            {
                //Se obtiene el grid del formulariol
                Grid gridSobres = (Grid)Formulario.Items.Item("grdMotRec").Specific;

                //Se asigna el dataTable del formulario al grid
                gridSobres.DataTable = Formulario.DataSources.DataTables.Item("dtAprob");
                //Se ejecuta la consulta
                gridSobres.DataTable.ExecuteQuery(query);
            }

            //Configura filas a modo no editable
            while (j < ((Grid)Formulario.Items.Item("grdMotRec").Specific).Columns.Count)
            {
                ((Grid)Formulario.Items.Item("grdMotRec").Specific).Columns.Item(j).Editable = false;
                j++;
            }

            ((Grid)Formulario.Items.Item("grdMotRec").Specific).Columns.Item("DocEntry").Visible = false;
            ((Grid)Formulario.Items.Item("grdMotRec").Specific).AutoResizeColumns();
        }

        /// <summary>
        /// Retorna el DocEntry de la fila seleccionada
        /// </summary>
        /// <returns></returns>
        public string ObtenerFilaSeleccionada()
        {
            if (((Grid)Formulario.Items.Item("grdMotRec").Specific).Rows.SelectedRows.Count > 0)
            {
                int filaSeleccionada = ((Grid)Formulario.Items.Item("grdMotRec").Specific).Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);

                string docEntry = ((Grid)Formulario.Items.Item("grdMotRec").Specific).DataTable.Columns.Item("DocEntry").Cells.Item(filaSeleccionada).Value.ToString();
                return docEntry;
            }

            return "0";
        }

        /// <summary>
        /// Valida que el motivo seleccionado sea generado por las validaciones del sistema
        /// </summary>
        /// <returns></returns>
        public bool ObtenerTipoMotivoSistema()
        {
            int filaSeleccionada = ((Grid)Formulario.Items.Item("grdMotRec").Specific).Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
            string motivo = ((Grid)Formulario.Items.Item("grdMotRec").Specific).DataTable.Columns.Item("Motivo").Cells.Item(filaSeleccionada).Value.ToString();

            if (motivo.Equals("E02") || motivo.Equals("E03") || motivo.Equals("E04") || motivo.Equals("E05") || motivo.Equals("E02"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que no existan registros en el grid para activar los radio button y seleccionar el de aprobado
        /// </summary>
        public void AjustarOpciones()
        {
            if (((Grid)Formulario.Items.Item("grdMotRec").Specific).Rows.Count > 1)
            {
                Formulario.Items.Item("rbRech").Enabled = true;
                Formulario.Items.Item("rbAprob").Enabled = true;
                ((OptionBtn)Formulario.Items.Item("rbAprob").Specific).Selected = true;
            }

        }

        protected override void AgregarDataSources()
        {

        }

        protected override void EstablecerDataBind()
        {
            Formulario.DataSources.UserDataSources.Item("udsSerie").Value = Serie;
            Formulario.DataSources.UserDataSources.Item("udsTipo").Value = CFE.ObtenerStringTipoCFECFC(Tipo);
            Formulario.DataSources.UserDataSources.Item("udsNum").Value = Numero;
        }

        protected override void AjustarFormulario(string formUID)
        {
            Formulario.Freeze(true);

            Formulario.Items.Item("lbDatosCer").TextStyle = 5;
            Formulario.Items.Item("lbEstado").TextStyle = 5;
            Formulario.Items.Item("lbMotRec").TextStyle = 5;

            ((OptionBtn)Formulario.Items.Item("rbRech").Specific).GroupWith("rbAprob");

            CargarGrid();

            if (Estado.Equals("Pendiente"))
            {
                if (((Grid)Formulario.Items.Item("grdMotRec").Specific).Rows.Count > 1)
                {
                    ((OptionBtn)Formulario.Items.Item("rbRech").Specific).Selected = true;
                }
                else
                {
                    ((OptionBtn)Formulario.Items.Item("rbAprob").Specific).Selected = true;
                }
            }
            else if (Estado.Equals("Rechazado"))
            {
                ((OptionBtn)Formulario.Items.Item("rbRech").Specific).Selected = true;
            }
            else if (Estado.Equals("Aprobado"))
            {
                ((OptionBtn)Formulario.Items.Item("rbAprob").Specific).Selected = true;
            }

            EstablecerDataBind();

            Formulario.Freeze(false);
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        /// <summary>
        /// Actualiza el estado de aprobacion del certificado
        /// </summary>
        /// <returns></returns>
        public bool ActualizarAprobacion()
        {
            ManteUdoCertificadoRecibido manteUdoSobreRecibido = new ManteUdoCertificadoRecibido();
            RespuestaSobre respuestaSobre = new RespuestaSobre();
            List<CertificadoRecibido> listaCertificadosRecibidos = new List<CertificadoRecibido>();
            List<ErrorValidarSobre> listaErrores = new List<ErrorValidarSobre>();
            string estadoAprobacion = "";


            try
            {
                listaCertificadosRecibidos = manteUdoSobreRecibido.ObtenerCertificadoRecibido(DocEntry);
                estadoAprobacion = ((OptionBtn)Formulario.Items.Item("rbAprob").Specific).Selected ? "Y" : "N";                

                if (estadoAprobacion.Equals("Y"))
                {
                    //Sobre validado
                    string idReceptor = respuestaSobre.GenerarACK("AS", listaCertificadosRecibidos[0], listaCertificadosRecibidos[0].CorreoEmisor, null, listaCertificadosRecibidos[0].NombreSobre);

                    foreach (CertificadoRecibido certRecibido in listaCertificadosRecibidos)
                    {
                        certRecibido.IdConsecutio = idReceptor;

                        //Se insertan los datos en la tabla de la BD
                        manteUdoSobreRecibido.ActualizarIdConsecutivo(idReceptor, docEntry);
                    }

                    FTP ftp = new FTP();

                    //Se sube el archivo al servidor FTP
                    ftp.CargarArchivos(listaCertificadosRecibidos[0].NombreSobre, RutasCarpetas.RutaCarpetaBandejaEntrada + listaCertificadosRecibidos[0].NombreSobre, 3);
                }
                else
                {
                    listaErrores = manteUdoSobreRecibido.ObtenerErroresSobre(DocEntry);
                    //Sobre rechazado
                    respuestaSobre.GenerarACK("BS", listaCertificadosRecibidos[0], listaCertificadosRecibidos[0].CorreoEmisor, listaErrores, listaCertificadosRecibidos[0].NombreSobre);
                }
            }
            catch (Exception)
            {
            }

            return manteUdoSobreRecibido.ActualizarEstado(DocEntry, estadoAprobacion);
        }

        #endregion MANTENIMIENTO

        #region PROPIEDADES

        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private string numero;

        public string Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        private string idConsecutivo;

        public string IdConsecutivo
        {
            get { return idConsecutivo; }
            set { idConsecutivo = value; }
        }

        private string docEntry;

        public string DocEntry
        {
            get { return docEntry; }
            set { docEntry = value; }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string numIniCAE;

        public string NumIniCAE
        {
            get { return numIniCAE; }
            set { numIniCAE = value; }
        }

        private string numFinCAE;

        public string NumFinCAE
        {
            get { return numFinCAE; }
            set { numFinCAE = value; }
        }

        private string fechaFirma;

        public string FechaFirma
        {
            get { return fechaFirma; }
            set { fechaFirma = value; }
        }

        private string fechaCAE;

        public string FechaCAE
        {
            get { return fechaCAE; }
            set { fechaCAE = value; }
        }

        #endregion PROPIEDADES
    }
}
