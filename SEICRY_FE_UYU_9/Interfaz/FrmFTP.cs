using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Conexion;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene los metodos para mostrar y administrar el formulario de configuracion del FTP
    /// </summary>
    class FrmFTP : FrmBase
    {
        //Variables utilizadas en toda la clase
        UserDataSource udsTxtServidor;
        UserDataSource udsTxtComp;
        UserDataSource udsTxtSob;
        UserDataSource udsTxtResp;
        UserDataSource udsTxtRepDi;
        UserDataSource udsTxtRepConSob;
        UserDataSource udsTxtRepConCom;
        UserDataSource udsTxtRepConReDi;
        UserDataSource udsTxtUsuario;
        UserDataSource udsTxtClave;
        UserDataSource udsTxtBan;
        UserDataSource udsTxtRepWsE;
        UserDataSource udsTxtRepWsC;
        UserDataSource udsTxtRepCfe;
        UserDataSource udsTxtRepCerAnu;
        UserDataSource udsTxtRepConSobDgi;
        UserDataSource udsTxtRutDgi;
        UserDataSource udschkFileDelete;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Ajustar el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        public override void CrearComponentes(string formUID)
        {
            AgregarDataSources();
            EstablecerDataBind();
            Consultar();
        }
              
        /// <summary>
        /// Crear los data sources del formulario
        /// </summary>
        protected override void AgregarDataSources()
        {           
            //Congelar el formulario
            Formulario.Freeze(true);

            udsTxtServidor = Formulario.DataSources.UserDataSources.Add("udsTxtSer", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtComp = Formulario.DataSources.UserDataSources.Add("udsTxtComp", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtSob = Formulario.DataSources.UserDataSources.Add("udsTxtSob", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtBan = Formulario.DataSources.UserDataSources.Add("udsTxtBan", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtResp = Formulario.DataSources.UserDataSources.Add("udsTxtResp", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepDi = Formulario.DataSources.UserDataSources.Add("udsTxtReDi", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtUsuario = Formulario.DataSources.UserDataSources.Add("udsTxtUsr", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtClave = Formulario.DataSources.UserDataSources.Add("udsTxtClv", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepWsE = Formulario.DataSources.UserDataSources.Add("udsTxReWsE", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepWsC = Formulario.DataSources.UserDataSources.Add("udsTxReWsC", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepConSob = Formulario.DataSources.UserDataSources.Add("udsConSob", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepConCom = Formulario.DataSources.UserDataSources.Add("udsConCom", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepConReDi = Formulario.DataSources.UserDataSources.Add("udsConReDi", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepCfe = Formulario.DataSources.UserDataSources.Add("udsTxReCf", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepCerAnu = Formulario.DataSources.UserDataSources.Add("udsCerAnu", BoDataType.dt_SHORT_TEXT, 254);
            udsTxtRepConSobDgi = Formulario.DataSources.UserDataSources.Add("udsSobDgi", BoDataType.dt_SHORT_TEXT,254);
            udsTxtRutDgi = Formulario.DataSources.UserDataSources.Add("udsRutDgi", BoDataType.dt_SHORT_TEXT, 254);
            udschkFileDelete = Formulario.DataSources.UserDataSources.Add("udFileD", BoDataType.dt_SHORT_TEXT, 1);

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Asigna los data sources a los componentes
        /// </summary>
        protected override void EstablecerDataBind()
        {
            ((EditText)Formulario.Items.Item("txtServ").Specific).DataBind.SetBound(true, "", "udsTxtSer");
            ((EditText)Formulario.Items.Item("txtComp").Specific).DataBind.SetBound(true, "", "udsTxtComp");
            ((EditText)Formulario.Items.Item("txtSob").Specific).DataBind.SetBound(true, "", "udsTxtSob");
            ((EditText)Formulario.Items.Item("txtBan").Specific).DataBind.SetBound(true, "", "udsTxtBan");
            ((EditText)Formulario.Items.Item("txtUsuario").Specific).DataBind.SetBound(true, "", "udsTxtUsr");
            ((EditText)Formulario.Items.Item("txtClave").Specific).DataBind.SetBound(true, "", "udsTxtClv");
            ((EditText)Formulario.Items.Item("txtResp").Specific).DataBind.SetBound(true, "", "udsTxtResp");
            ((EditText)Formulario.Items.Item("txtRepDi").Specific).DataBind.SetBound(true, "", "udsTxtReDi");
            ((EditText)Formulario.Items.Item("txtRutWsE").Specific).DataBind.SetBound(true, "", "udsTxReWsE");
            ((EditText)Formulario.Items.Item("txtRutWsC").Specific).DataBind.SetBound(true, "", "udsTxReWsC");
            ((EditText)Formulario.Items.Item("txtConSob").Specific).DataBind.SetBound(true, "", "udsConSob");
            ((EditText)Formulario.Items.Item("txtConCom").Specific).DataBind.SetBound(true, "", "udsConCom");
            ((EditText)Formulario.Items.Item("txtConRep").Specific).DataBind.SetBound(true, "", "udsConReDi");
            ((EditText)Formulario.Items.Item("txtRepCfe").Specific).DataBind.SetBound(true, "", "udsTxReCf");
            ((EditText)Formulario.Items.Item("txtCerAnu").Specific).DataBind.SetBound(true, "", "udsCerAnu");
            ((EditText)Formulario.Items.Item("txtConSDg").Specific).DataBind.SetBound(true, "", "udsSobDgi");
            ((EditText)Formulario.Items.Item("txtRutDgi").Specific).DataBind.SetBound(true, "", "udsRutDgi");
            ((CheckBox)Formulario.Items.Item("chkFileD").Specific).DataBind.SetBound(true, "", "udFileD");
        }

        #endregion INTERFAZ DE USUARIO

        #region VALIDACIONES

        //Validar que los campos tengan el formato correcto

        /// <summary>
        /// Validar el formato de la url
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoUrl()
        {
            if (udsTxtServidor.Value.Length > 254)
            {
                return false;
            }

            return true;
        }


        private bool ValidarFormatoRytDgi()
        {
            if (udsTxtServidor.Value.Length > 254 ||  udsTxtServidor.Value.Length == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validar el formato del usuario
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoUsuario()
        {
            if (udsTxtUsuario.Value.Length > 254)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validar el formato de la clave
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoClave()
        {
            if (udsTxtClave.Value.Length > 254)
            {
                return false;
            }

            return true;
        }

        //Validar que los campos tengan contenido

        /// <summary>
        /// Validar que se haya ingresado un valor para la url
        /// </summary>
        /// <returns></returns>
        private bool ValidarUrl()
        {
            if (udsTxtServidor.Value != "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validar que se haya ingresado un valor para el usuario
        /// </summary>
        /// <returns></returns>
        private bool ValidarUsuario()
        {
            if (udsTxtUsuario.Value != "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validar que se haya ingresado un valor para la clave
        /// </summary>
        /// <returns></returns>
        private bool ValidarClave()
        {
            if (udsTxtClave.Value != "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Realizar las validaciones y mostrar mensajes de error
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            //------------------//
            //********URL*******//
            //------------------//

            //Valor ingresado
            if (!ValidarUrl())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncUrlRepositorio, AdminEventosUI.tipoError);
                return false;
            }

            //----------------------//
            //********USUARIO*******//
            //----------------------//

            //Valor ingresado
            if (!ValidarUsuario())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncUsuario, AdminEventosUI.tipoError);
                return false;
            }

            //--------------------//
            //********CLAVE*******//
            //--------------------//

            //Valor ingresado
            if (!ValidarClave())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errValIncClave, AdminEventosUI.tipoError);
                return false;
            }

            //------------------//
            //********URL*******//
            //------------------//

            //Formato correcto
            if (!ValidarUrl())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoUrlRepositorio, AdminEventosUI.tipoError);
                return false;
            }

            //----------------------//
            //********USUARIO*******//
            //----------------------//

            //Formato correcto
            if (!ValidarUsuario())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoUsuario, AdminEventosUI.tipoError);
                return false;
            }

            //--------------------//
            //********CLAVE*******//
            //--------------------//

            //Formato correcto
            if (!ValidarClave())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoClave, AdminEventosUI.tipoError);
                return false;
            }

            return true;

        }

        #endregion VALIDACIONES

        #region MANENIMIENTO

        /// <summary>
        /// Consulta los datos de la configuracion 
        /// </summary>
        private void Consultar()
        {
            //Crea una nueva instanacia al mantenimiento de configuracion de FTP
            ManteUdoFTP manteUdoFtp = new ManteUdoFTP();

            //Obtiene un nuevo objeto configuracion FTP
            ConfigFTP configFtp = manteUdoFtp.Consultar();

            //Validar que se haya obtenido un registro
            if (configFtp != null)
            {
                //Cargar los data sourcers
                udsTxtServidor.Value = configFtp.Servidor;
                udsTxtComp.Value = configFtp.RepoComp;
                udsTxtSob.Value = configFtp.RepoSob;
                udsTxtBan.Value = configFtp.RepoBandejaEntrada;
                udsTxtUsuario.Value = configFtp.Usuario;
                udsTxtClave.Value = configFtp.Clave;
                udsTxtResp.Value = configFtp.RepoResp;
                udsTxtRepDi.Value = configFtp.RepoRepDi;
                udsTxtRepWsE.Value = configFtp.RepoWebServiceEnvio;
                udsTxtRepWsC.Value = configFtp.RepoWebServiceConsulta;
                udsTxtRepConSob.Value = configFtp.RepoContingenciaSobres;
                udsTxtRepConCom.Value = configFtp.RepoContingenciaComprobantes;
                udsTxtRepConReDi.Value = configFtp.RepoContingenciaReportesDiarios;
                udsTxtRepCfe.Value = configFtp.RepoCFEs;
                udsTxtRepCerAnu.Value = configFtp.RepoCertificadosAnulados;
                udsTxtRepConSobDgi.Value = configFtp.RepoContingenciaSobreDgi;
                udsTxtRutDgi.Value = configFtp.RutDgi;
                udschkFileDelete.Value = configFtp.FileDelete;
            }
            else
            {
                if (ManteUdoFTP.errorManteFTP)
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(Mensaje.errTablaConfigFtp);
                    ManteUdoFTP.errorManteFTP = false;
                }
            }
        }

        /// <summary>
        /// Almacena o actualiza la configuracion de la conexion con el ftp
        /// <para>0=Valores ingresados.</para>
        /// <para>1=Valores no ingresados.</para>
        /// <para>2=Valores no validos</para>
        /// <para>3=Rango no ingresado</para>
        /// </summary>
        public int Almacenar()
        {
            //Valida que los campos tenga valores y el formato correcto
            if (Validar())
            {
                //Crea un nuevo objeto de configuracion de ftp
                ConfigFTP configFtp = new ConfigFTP();

                configFtp.Servidor = udsTxtServidor.Value;
                configFtp.RepoComp = udsTxtComp.Value;
                configFtp.RepoSob = udsTxtSob.Value;
                configFtp.Usuario = udsTxtUsuario.Value;
                configFtp.RepoBandejaEntrada = udsTxtBan.Value;
                configFtp.Clave = udsTxtClave.Value;
                configFtp.RepoResp = udsTxtResp.Value;
                configFtp.RepoRepDi = udsTxtRepDi.Value;
                configFtp.RepoWebServiceEnvio = udsTxtRepWsE.Value;
                configFtp.RepoWebServiceConsulta = udsTxtRepWsC.Value;
                configFtp.RepoContingenciaSobres = udsTxtRepConSob.Value;
                configFtp.RepoContingenciaComprobantes = udsTxtRepConCom.Value;
                configFtp.RepoContingenciaReportesDiarios = udsTxtRepConReDi.Value;
                configFtp.RepoCFEs = udsTxtRepCfe.Value;
                configFtp.RepoCertificadosAnulados = udsTxtRepCerAnu.Value;
                configFtp.RepoContingenciaSobreDgi = udsTxtRepConSobDgi.Value;
                //udsTxtRutDgi.Value = configFtp.RutDgi;
                configFtp.FileDelete = udschkFileDelete.Value;

                //Crea una nueva instanacia al mantenimiento de configuracion de FTP
                ManteUdoFTP manteUdoFtp = new ManteUdoFTP();

                //Valida que la configuracion ya exista. Si existe acutializa los datos, si no existe los almacena
                if (manteUdoFtp.ExisteConfiguracion())
                {
                    //Actualizar los datos
                    if (manteUdoFtp.Actualizar(configFtp))
                    {
                        return 0;//Valores ingresados
                    }
                }
                else
                {
                    //Almacenar los datos
                    if (manteUdoFtp.Almacenar(configFtp))
                    {
                        return 0;//Valores ingresados
                    }
                }
            }
            else
            {
                return 2; //Valores no validos
            }

            return 1; //Valores no ingresados
        }

        #endregion MANENIMIENTO
    }
}
