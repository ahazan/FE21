using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Metodos_FTP
{
    class HiloFTP
    {
        /// <summary>
        /// Metodo que crea un hilo para descargar los adjuntos de la bandeja de entrada
        /// de la cuenta electronica configurada en el envio de correos
        /// </summary>
        public void subidaContinua()
        {            
            Thread threadSubirFTP = null;

            FTP ftp = new FTP();

            //Se define el hilo
            threadSubirFTP = new Thread(ftp.subirBorrarArchivosFTP);
            //Se pone como background para que se muera cuando se cierra la aplicacion
            threadSubirFTP.IsBackground = true;
            //Inicia el hilo
            threadSubirFTP.Start();            
        }   
    }
}
