using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SEICRY_FE_UYU_9.AbrirDialogo
{
    class DialogoAbrirArchivo
    {
        public static string nombreArchivo = "";

        /// <summary>
        /// Metodo para abrir un archivo de dialogo
        /// Recibe un filtro para los archivos a buscar:
        /// Formato
        /// </summary>
        public void AbrirDialogo(string pFiltro)
        {
            DialogoArchivo oGetFileName = new DialogoArchivo();
            oGetFileName.filtro = pFiltro;                
                
            oGetFileName.directorioInicial = Environment.GetFolderPath(Environment.SpecialFolder.Personal);                

            Thread threadGetExcelFile = new Thread(new ThreadStart(oGetFileName.obtenerNombreArchivo));
            threadGetExcelFile.SetApartmentState(ApartmentState.STA);

            //if (threadGetExcelFile.ThreadState == ThreadState.Unstarted)
            //{
            //    threadGetExcelFile.SetApartmentState(ApartmentState.STA);
            //    threadGetExcelFile.Start();         
            //}
            //else if (threadGetExcelFile.ThreadState == ThreadState.Stopped)
            //{
            //    threadGetExcelFile.Start();
            //    threadGetExcelFile.Join();         
            //}

            try
            {
                threadGetExcelFile.Start();
                //while(threadGetExcelFile.ThreadState == ThreadState.Running)
                //Espera a que inicie el hilo
                while (!threadGetExcelFile.IsAlive) ;
                Thread.Sleep(1);
                ///Espera a que termine el hilo
                threadGetExcelFile.Join();
                nombreArchivo = oGetFileName.nombreArchivo;                
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.ToString());
            }
            threadGetExcelFile = null;
            oGetFileName = null;
        }
    }
}