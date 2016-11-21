using System.Linq;
using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using Microsoft.Win32;

namespace SEICRY_FE_UYU_9.GenerarPDF
{
    public class Imprimir
    {
        /// <summary>
        /// Imprime PDF a la impresora por default o por red*
        /// *(usa la ruta de la impresora)
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public Boolean ImprimirPdf(object nombreArchivo, out List<string> log)
        {
            bool salida = false;
            log = new List<string>();

            try
            {
                log.Add("Ingreso a imprimir, usuario: " + ProcConexion.Comp.UserName + " hora: " + DateTime.Now);
                Form Actual = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;

                Process proc = new Process();

                var adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("AcroRd32.exe");
                string rutaAdobe = adobe.GetValue("").ToString();

                //Se oculta la ventana
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                /* Fin Mod 12.08.216 - Obtener ruta de Adobe desde el registro de Windows */

                #region IMRPRESION DEFAULT

                //Se setea adobe para que abra en modo de impresion
                proc.StartInfo.Verb = "print";
                //Se carga la ruta del ejecutable de adobe
                proc.StartInfo.FileName = rutaAdobe;

                log.Add("Archivo a imprimir: " + nombreArchivo + " hora: " + DateTime.Now);

                //Se cargan los argumentos, se envia a imprimir a la impresora x default
                proc.StartInfo.Arguments = String.Format(@"/p /h {0}", nombreArchivo);

                #endregion IMPRESION DEFAULT

                #region IMPRESION POR RED

                //Se setea adobe para que abra en modo de impresion
                //proc.StartInfo.Verb = "printto";
                ////Se carga la ruta del ejecutable de adobe
                //proc.StartInfo.FileName = rutaAdobe;//String.Format(@"/p /h {0}", nombreArchivo);//rutaAdobe;                  
                ////Se cargan los argumentos, se envia a imprimir a la impresora x default               
                //proc.StartInfo.Arguments = "\t" + String.Format(@"/p /h {0}", nombreArchivo) + " " + @"\\192.168.1.119\Lexmark Grande";
                ////Shell(String.Format("rundll32 printui.dll,PrintUIEntry /y /n ""{0}""", "\\printermachine\samsung laser"));

                #endregion IMPRESION POR RED

                proc.StartInfo.UseShellExecute = false;
                //Evita crear la venta de impresion
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                //Inicia la impresion
                proc.Start();
                int idSesion = proc.SessionId;
                log.Add("Inicio proceso impresion, idSesion: " + idSesion + " hora: " + DateTime.Now);

                if (proc.HasExited == false)
                {
                    //Espera 12 segundos para cerrar la ventana de adobe
                    //proc.WaitForExit(21000);
                    proc.WaitForExit(10000);
                }
                log.Add("termina ciclo de espera" + DateTime.Now);
                proc.EnableRaisingEvents = true;
                //Cierra el proceso
                proc.Close();
                //
                log.Add("Antes de eliminar el proceso, hora: " + DateTime.Now);
                EliminarProcesoAdobe("AcroRd32", idSesion);
                salida = true;

                Actual = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
                log.Add("termina proceso de impresion hora: " + DateTime.Now + " usuario: " + ProcConexion.Comp.UserName + " archivo: "
                     + nombreArchivo);
            }
            catch (Exception ex)
            {
                log.Add("Excepcion: " + ex.InnerException + " //ex.toString: " + ex.ToString() + " hora: " + DateTime.Now);
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Error: " + ex.ToString());
            }

            return salida;
        }


        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        /// <summary>
        /// Elimina el proceso de adobe creado para la impresion
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static void EliminarProcesoAdobe(string name, int idSesion)
        {
            bool primerProceso = false;

            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.StartsWith(name))
                    {
                        if (clsProcess.SessionId == idSesion)
                        {
                            if (!primerProceso)
                            {
                                clsProcess.Kill();
                                primerProceso = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ERROR ELIMINAR:" + ex.ToString());   
            }
        }
    }
}
