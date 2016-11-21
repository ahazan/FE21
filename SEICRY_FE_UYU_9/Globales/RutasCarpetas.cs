using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SEICRY_FE_UYU_9.Objetos
{
    class RutasCarpetas
    {
        //NOTA INFORMATIVA: Todas las rutas deben terminar en \\

        //Ruta de la carpeta que almacena las carpetas con reportes diarios, sobres y comprobantes
        public static string RutaCarpetaTemporal = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Temporal\\";
        //Nombre de carpeta para almacenar los archivos xml de reporte diario
        public static string RutaCarpetaReporteDiario = RutaCarpetaTemporal + "Reporte Diario\\";
        //Nombre de carpeta para almacenar los archivos xml de sobres de comprobantes
        public static string RutaCarpetaSobres = RutaCarpetaTemporal + "Sobres\\";
        //Nombre de carpeta para almacenar los archivos xml de sobres de comprobantes dgi
        public static string RutaCarpetaSobresDgi = RutaCarpetaTemporal + "SobresDGI\\";
        //Nombre de carpeta para almacenar los archivos pdf y xml de comprobantes
        public static string RutaCarpetaComprobantes = RutaCarpetaTemporal + "Comprobantes\\";   
        //Nombre de carpeta para almacenar los archivos bajados de la bandeja de entrada electronica
        public static string RutaCarpetaBandejaEntrada = RutaCarpetaTemporal + "BandejaEntrada\\";
        //Nombre de carpeta para almacenar los archivos que se bajan del ftp para visualizar
        public static string RutaCarpetaArchivosVisualizados = RutaCarpetaTemporal + "Visualizar\\";
        //Nombre de carpeta para almacenar los archivos firmados que se crear para cada documento
        public static string RutaCarpetaCertificadosTemporales = RutaCarpetaTemporal + "CertificadosTemporales\\";
        //Nombre de carpeta para almacenar los archivos de acuse de recibo de sobres generados como respuesta al receptor
        public static string RutaCarpetaAcuseRecibidoSobre = RutaCarpetaTemporal + "AcuseReciboSobre\\";
        //Nombre de carpeta para almacenar los archivos de acuse de recibo de certificados generados como respuesta al receptor
        public static string RutaCarpetaAcuseRecibidoCertificado = RutaCarpetaTemporal + "AcuseReciboCertificado\\";
        //Nombre de carpeta para almacenar los archivos de sobre temportales
        public static string RutaCarpetaSobresTemporales = RutaCarpetaTemporal + "SobresTemp\\";
        //Nombre de carpeta para reportes diarios en contingencia
        public static string RutaCarpetaContingenciaReporteDiario = RutaCarpetaTemporal + "Contingencia\\ReporteDiario\\";
        //Nombre de carpeta para sobres en contingencia
        public static string RutaCarpetaContingenciaSobres = RutaCarpetaTemporal + "Contingencia\\Sobres\\";
        //Nombre de carpeta para sobres en contingencia dgi
        public static string RutaCarpetaContingenciaSobresDgi = RutaCarpetaTemporal + "Contingencia\\SobresDGI\\";
        //Nombre de carpeta para sobres en contingencia
        public static string RutaCarpetaContingenciaSobresTemporales = RutaCarpetaTemporal + "Contingencia\\SobresTemp\\";
        //Nombre de carpeta para comprobantes en contingencia
        public static string RutaCarpetaContingenciaComprobantes = RutaCarpetaTemporal + "Contingencia\\Comprobantes\\";
        //Nombre de carpeta para adjuntos de la restauracion de contingencia
        public static string RutaCarpetaAdjuntos = RutaCarpetaTemporal + "Adjuntos\\";
        //Nombre de carpeta para ACKSobreReceptor
        public static string RutaCarpetaACKSobreReceptor = RutaCarpetaTemporal + "ACKSobreReceptor\\";
        //Nombre de carpeta para ACKCFEReceptor
        public static string RutaCarpetaACKCFEReceptor = RutaCarpetaTemporal + "ACKCFEReceptor\\";
        //Nombre de carpeta para consulta de estado de CFE recibido
        public static string RutaCarpetaConsultaEstado = RutaCarpetaTemporal + "ConsultaEstado\\";
        /// <summary>
        /// Nombre de carpeta para Sobres Recibidos Temporales
        /// </summary>
        public static string RutaCarpetaSobreRecibidosTemporales = RutaCarpetaTemporal + "SobreRecibidoTemporal\\";
        /// <summary>
        /// Nombre de carpeta para comprobantes con Adenda
        /// </summary>
        public static string RutaCarpetaComprobantesAdenda = RutaCarpetaTemporal + "ComprobantesAdenda\\";
        /// <summary>
        /// Nombre de carpeta para sobres con comprobantes con adenda
        /// </summary>
        public static string RutaCarpetaSobresComprobantesAdenda = RutaCarpetaTemporal + "SobresAdenda\\";
        /// <summary>
        /// Nombre de carpeta para comprobantes con adenda en contingencia
        /// </summary>
        public static string RutaCarpetaContingenciaComprobantesAdenda = RutaCarpetaTemporal + "Contingencia\\ComprobantesAdenda\\";
        /// <summary>
        /// Nombre de carpeta para sobres con adenda en contingencia
        /// </summary>
        public static string RutaCarpetaContingenciaSobresAdenda = RutaCarpetaTemporal + "Contingencia\\SobresAdenda\\";
        /// <summary>
        /// Nombre de carpeta para logs de impresion
        /// </summary>
        public static string RutaCarpetaLogImpresion = RutaCarpetaTemporal + "LogImpresion\\";

        #region Proceso_WebService
        public static string RutaCarpetaImpresion = RutaCarpetaTemporal + "Imprimir\\";
        #endregion Proceso_WebService

        /// <summary>
        /// Metodos para generar carpetas
        /// </summary>
        public void generarCarpetas()
        {
            try
            {
                //Crea carpeta para almacenar carpetas de Facturacion Eletronica
                comprobarCrearCarpeta(RutaCarpetaTemporal);
                
                //Crea carpeta para almacenar los comprobantes
                comprobarCrearCarpeta(RutaCarpetaComprobantes);

                //Crea carpeta para almacenar reportes diarios
                comprobarCrearCarpeta(RutaCarpetaReporteDiario);
                
                //Crea carpeta para almacenar sobres de comprobantes
                comprobarCrearCarpeta(RutaCarpetaSobres);

                //Crea carpeta para almacenar sobres de comprobantes dgi
                comprobarCrearCarpeta(RutaCarpetaSobresDgi);

                //Crea carpeta para almacenar archivos de bajados de la bandeja de entrada
                comprobarCrearCarpeta(RutaCarpetaBandejaEntrada);

                //Crea carpeta para almacenar archivos a visualizar
                comprobarCrearCarpeta(RutaCarpetaArchivosVisualizados);

                //Crea carpeta para almacenar certificados temporalmente
                comprobarCrearCarpeta(RutaCarpetaCertificadosTemporales);

                //Crea carpeta para almacenar acuses de recibo de sobres
                comprobarCrearCarpeta(RutaCarpetaAcuseRecibidoSobre);

                //Crea carpeta para almacenar acuses de recibo de certificados
                comprobarCrearCarpeta(RutaCarpetaAcuseRecibidoCertificado);

                //Crea carpeta para almacenar sobres temporales
                comprobarCrearCarpeta(RutaCarpetaSobresTemporales);

                //Crea carpeta para almacenar reportes diarios en contingencia 
                comprobarCrearCarpeta(RutaCarpetaContingenciaReporteDiario);

                //Crea carpeta para almacenar sobres en contingencia
                comprobarCrearCarpeta(RutaCarpetaContingenciaSobres);

                //Crea carpeta para almacenar sobres en contingencia dgi
                comprobarCrearCarpeta(RutaCarpetaContingenciaSobresDgi);

                //Crea carpeta para almacenar sobre temporales
                comprobarCrearCarpeta(RutaCarpetaContingenciaSobresTemporales);

                //Crea carpeta para almacenar comprobantes en contingencia
                comprobarCrearCarpeta(RutaCarpetaContingenciaComprobantes);

                //Crea carpeta para almacenar archivos adjuntos
                comprobarCrearCarpeta(RutaCarpetaAdjuntos);

                //Crea carpeta para almacenar el ACKSobreReceptor
                comprobarCrearCarpeta(RutaCarpetaACKSobreReceptor);

                //Crea carpeta para almacenar el ACKCFEReceptor
                comprobarCrearCarpeta(RutaCarpetaACKCFEReceptor);

                //Crea carpeta para almacenar consulta de Estado
                comprobarCrearCarpeta(RutaCarpetaConsultaEstado);

                //Crea carpeta para almacenar sobres recibidos temporalmente
                comprobarCrearCarpeta(RutaCarpetaSobreRecibidosTemporales);

                //Crea carpeta para almacenar comprobantes con adenda
                comprobarCrearCarpeta(RutaCarpetaComprobantesAdenda);

                //Crea carpeta para almacenar sobres con comprobantes con adenda
                comprobarCrearCarpeta(RutaCarpetaSobresComprobantesAdenda);

                //Crea carpeta para almacenar comprobantess con adenda en contingencia
                comprobarCrearCarpeta(RutaCarpetaContingenciaComprobantesAdenda);

                //Crea carpeta para almacenar sobres con adenda en contingencia
                comprobarCrearCarpeta(RutaCarpetaContingenciaSobresAdenda);

                //Crea carpeta para almacenar logs de impresion
                comprobarCrearCarpeta(RutaCarpetaLogImpresion);

                #region Proceso_WebService
                //Crea carpeta para almacenar PDF hasta que se imprima por cola de impresion
                comprobarCrearCarpeta(RutaCarpetaImpresion);
                #endregion Proceso_WebService
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("rutasCarpetas " + ex.ToString());
            }
        }

        /// <summary>
        /// Crea carpetas si no existen
        /// </summary>
        /// <param name="rutaCarpeta"></param>
        private void comprobarCrearCarpeta(string rutaCarpeta)
        {
            try
            {
                //Valida que no exista el directorio
                if (!Directory.Exists(quitarUltimos(rutaCarpeta)))
                {
                    //Crea el directorio
                    Directory.CreateDirectory(quitarUltimos(rutaCarpeta));
                }
            }
            catch(Exception ex)
            {            
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ComprobarCarpetas " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo para eliminar el doble slash al final de una ruta
        /// </summary>
        /// <param name="nombreRuta"></param>
        /// <returns></returns>
        private string quitarUltimos(string nombreRuta)
        {
            string resultado = "";

            if (nombreRuta.Length > 2)
            {
                resultado = nombreRuta.Substring(0, nombreRuta.Length - 1);
            }
            else
            {
                resultado = nombreRuta;
            }

            return resultado;
        }
    }
}
