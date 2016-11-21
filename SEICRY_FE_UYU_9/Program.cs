using System;
using System.Collections.Generic;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Interfaz;
using SEICRY_FE_UYU_9.EnvioCorreo;
using SAPbouiCOM.Framework;
using SEICRY_FE_UYU_9.Metodos_FTP;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.ComunicacionDGI;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Certificados.Xml.Transformacion;
using SEICRY_FE_UYU_9.WS;
using SAPbobsCOM;

namespace SEICRY_FE_UYU_9
{
    class Program
    {
        static BandejaElectronica bandejaElectronica = new BandejaElectronica();
        static JobACKConsultaEnvio jobACKConsultaEnvio = new JobACKConsultaEnvio();
        static HiloFTP hiloFTP = new HiloFTP();

        #region Proceso_WebService
        static HiloWS hiloWS = new HiloWS();
        public static Queue<String> colaImpresion = new Queue<string>();
        #endregion Proceso_WebService

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application oApp = null;

            if (args.Length < 1)
            {
                oApp = new Application();
            }
            else
            {
                oApp = new Application(args[0]);
            }

            //Se estable la conexion para el DI API
            ProcConexion ProcConexion = new ProcConexion();
            ProcConexion.Comp.SetSboLoginContext(Application.SBO_Application.Company.GetConnectionContext(ProcConexion.Comp.GetContextCookie()));
            ProcConexion.Comp.Connect();

            ManteUdoUI manteUdoUi = new ManteUdoUI();
            Globales.ValorUI.valorUI = manteUdoUi.ConsultarValorUI();

            RutasCarpetas rutasCarpetas = new RutasCarpetas();
            rutasCarpetas.generarCarpetas();

            ManteUdoEstadoContingencia manteEstadoContingencia = new ManteUdoEstadoContingencia(); 
            manteEstadoContingencia.ActualizarEstadoContingencia();

            ProcCreacionMenus procCreacionMenus = new ProcCreacionMenus();            
            //False Usuario 32 | True Administrador
            procCreacionMenus.CrearMenusFE(true);
       
            AdminEventosUI adminEventosUI = new AdminEventosUI();
            adminEventosUI.ObtenerFirmaDigital();
            adminEventosUI.ObtenerUrlWebService();
            adminEventosUI.ConsultarEstadoSobre();

            bandejaElectronica.descargaContinua();
            jobACKConsultaEnvio.IniciarProceso();
            hiloFTP.subidaContinua();

            adminEventosUI.ConsultarSobreEnvioTrancados();


            //if ( manteUdoUi.ConsultarWSTransaccionesPeriodicas())
            //{
                    #region Proceso_WebService
                    hiloWS.envioContinuo();            
                    #endregion Proceso_WebService

                    #region Transacciones_Periodicas
                    hiloWS.procesarColaImpresion();
                    #endregion Transacciones_Periodicas
            //}

            oApp.Run();
        }

        public static void Cerrar()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
