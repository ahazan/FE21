using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SAPbobsCOM;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmEnvioDGICfes : FrmBase
    {
        string RUTA_CERTIFICADO = "", CLAVE_CERTIFICADO = "", URL_ENVIO = "", URL_CONSULTAS = "";
        SAPbouiCOM.Application app = SAPbouiCOM.Framework.Application.SBO_Application;


        /// <summary>
        /// Agrega dataSources
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
            Refrescar();
        }

        /// <summary>
        /// Establece el databind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        }

        /// <summary>
        /// Carga los datos del grid
        /// </summary>
        public void Refrescar()
                    {
            string consulta = "";

            FrmUsuarios ClseUser = new FrmUsuarios();


            JobEnvioSobreMasivo Usuario = new JobEnvioSobreMasivo();


            //Establecer consulta
            if (Usuario.SuperUsuario())
         
            {
                consulta = "SELECT CASE WHEN (U_Tipo = '111' OR U_Tipo = '101' OR U_Tipo = '103' OR U_Tipo = '113') THEN " +
                            "(SELECT DocNum FROM OINV WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '112' OR U_Tipo = '102') THEN " +
                            "(SELECT DocNum FROM ORIN WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '181') THEN (SELECT DocNum FROM " +
                            "ODLN WHERE DocEntry = U_DocSap) ELSE U_DocSap END AS 'Número de Documento SAP', U_Tipo AS 'Tipo Documento', " +
                            "U_Serie AS 'Serie', U_Numero AS 'Número CFE', CreateDate AS 'Fecha Creación' FROM [@TFECONSOB]" +
                            "WHERE U_Estado = 'Pendiente' ";
            }
            else
                 {
                  consulta = "SELECT CASE WHEN (U_Tipo = '111' OR U_Tipo = '101' OR U_Tipo = '103' OR U_Tipo = '113') THEN " +
                              "(SELECT DocNum FROM OINV WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '112' OR U_Tipo = '102') THEN " +
                              "(SELECT DocNum FROM ORIN WHERE DocEntry = U_DocSap) WHEN (U_Tipo = '181') THEN (SELECT DocNum FROM " +
                              "ODLN WHERE DocEntry = U_DocSap) ELSE U_DocSap END AS 'Número de Documento SAP', U_Tipo AS 'Tipo Documento', " +
                              "U_Serie AS 'Serie', U_Numero AS 'Número CFE', CreateDate AS 'Fecha Creación' FROM [@TFECONSOB]" +
                              "WHERE U_Estado = 'Pendiente' AND U_Usuario = '"+ ProcConexion.Comp.UserName +"' AND CreateDate BETWEEN '" + 
                              DateTime.Now.ToString("yyyy-MM-dd") +
                              "' AND '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                 }



            try
            {
                Formulario.Freeze(true);
                Formulario.DataSources.DataTables.Item("dtGrdEnv").ExecuteQuery(consulta);

                Formulario.Items.Item("btnEnv").Enabled = true;

                ((Grid)Formulario.Items.Item("grdEnv").Specific).Columns.Item(0).Editable = false;
                ((Grid)Formulario.Items.Item("grdEnv").Specific).Columns.Item(1).Editable = false;
                ((Grid)Formulario.Items.Item("grdEnv").Specific).Columns.Item(2).Editable = false;
                ((Grid)Formulario.Items.Item("grdEnv").Specific).Columns.Item(3).Editable = false;
                ((Grid)Formulario.Items.Item("grdEnv").Specific).Columns.Item(4).Editable = false;

                Formulario.Freeze(false);
            }
            catch (Exception)
            {

            }
        }
            

        /// <summary>
        /// Envio a DGI
        /// </summary>
        public void EnviarDGI()
        {
            try
            {
                Formulario.Freeze(true);
                ObtenerFirmaDigital();
                ObtenerUrlWebService();

                Formulario.Items.Item("btnEnv").Enabled = false;

                List<SobresMasivos> sobresMasivos = ObtenerSobresGrid();

                foreach (SobresMasivos sobreMasivo in sobresMasivos)
                {
                    EnviarSobre(sobreMasivo.Tipo, sobreMasivo.Serie, sobreMasivo.Numero);
                }

                

                Formulario.Freeze(false);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Realiza el envio de los sobre por medio del web service de DGI.
        /// </summary>
        /// <param name="sobre"></param>
        public void EnviarSobre(int tipo, string serie, int numero)
        {
            ComunicacionDgi comunicacionDGI = new ComunicacionDgi();

            try
            {
                ParametrosJobWsDGIMasivo parametrosJobWsDGI =
                    new ParametrosJobWsDGIMasivo(RUTA_CERTIFICADO, CLAVE_CERTIFICADO, URL_ENVIO, URL_CONSULTAS);
                parametrosJobWsDGI.NombreSobre = tipo + serie + numero;
                parametrosJobWsDGI.Tipo = tipo;
                parametrosJobWsDGI.Serie = serie;
                parametrosJobWsDGI.Numero = numero;

                comunicacionDGI.ConsumirWsEnviarSobreMasivo(parametrosJobWsDGI);
            }
            catch (Exception ex)
            {
                app.MessageBox("ERROR: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo para obtener informacion de la firma digital
        /// </summary>
        public void ObtenerFirmaDigital()
        {
            ManteUdoCertificadoDigital manteUdoFirma = new ManteUdoCertificadoDigital();

            Certificado certificado = manteUdoFirma.Consultar();

            if (certificado != null)
            {
                RUTA_CERTIFICADO = certificado.RutaCertificado;
                CLAVE_CERTIFICADO = certificado.Clave;
            }
            else
            {
                app.MessageBox(Mensaje.warNoConfigFirmaDigital);
            }
        }

        /// <summary>
        /// Obtiene las direcciones web de los web services de la DGI
        /// </summary>
        public void ObtenerUrlWebService()
        {
            try
            {
                ManteUdoFTP manteUdoFtp = new ManteUdoFTP();

                ConfigFTP configFtp = manteUdoFtp.ConsultarURLWebService();

                if (configFtp != null)
                {
                    URL_ENVIO = configFtp.RepoWebServiceEnvio;
                    URL_CONSULTAS = configFtp.RepoWebServiceConsulta;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Obtiene los sobres en el grid
        /// </summary>
        /// <returns></returns>
        private List<SobresMasivos> ObtenerSobresGrid()
        {
            List<SobresMasivos> resultado = new List<SobresMasivos>();
            
            int cantidadSobres = 0, i = 0;

            try
            {
                Grid grdEnv = (Grid)Formulario.Items.Item("grdEnv").Specific;
                cantidadSobres = grdEnv.DataTable.Rows.Count;

                while (i < cantidadSobres)
                {
                    if (!grdEnv.DataTable.Columns.Item(1).Cells.Item(i).Value.ToString().Equals(""))
                    {
                        SobresMasivos temp = new SobresMasivos();

                        temp.Tipo = int.Parse(grdEnv.DataTable.Columns.Item(1).Cells.Item(i).Value + "");
                        temp.Serie = grdEnv.DataTable.Columns.Item(2).Cells.Item(i).Value + "";
                        temp.Numero = int.Parse(grdEnv.DataTable.Columns.Item(3).Cells.Item(i).Value + "");
                        resultado.Add(temp);
                    }
                    else
                    {
                        AdminEventosUI.mostrarMensaje("No hay sobres para enviar a DGI", AdminEventosUI.tipoMensajes.advertencia);
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                app.MessageBox("Error:" + ex.ToString());
            }

            return resultado;
        }
    }
}
