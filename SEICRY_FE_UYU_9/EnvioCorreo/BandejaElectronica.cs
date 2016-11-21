using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.EnvioCorreo
{
    class BandejaElectronica
    {
        /// <summary>
        /// Metodo que crea un hilo para descargar los adjuntos de la bandeja de entrada
        /// de la cuenta electronica configurada en el envio de correos
        /// </summary>
        public void descargaContinua()
        {
            ManteUdoCorreos mantenimiento = new ManteUdoCorreos();
            //Se consulta la tabla de correos
            Correo correo = mantenimiento.Consultar();

            if (correo != null)
            {
                //Crea o valida la existencia de la carpeta para almacenar los archivos bajados
                RutasCarpetas rutasCarpetas = new RutasCarpetas();
                rutasCarpetas.generarCarpetas();

                Thread threadBandejaEntrada = null;

                //Si es una cuenta de Gmail
                if (correo.Opcion.Equals("0"))
                {
                  //Se crea una instancia para cuentas de gmail
                  Mail bandeja = new Mail(correo.Cuenta, correo.Cuenta, "",
                                     "", Mensaje.pdfServidorGmail, correo.Cuenta, correo.Clave, null, 587);
                  //Se define el hilo
                  threadBandejaEntrada = new Thread(bandeja.bandejaEntradaGmail);
                  //Se pone como background para que se muera cuando se cierra la aplicacion
                  threadBandejaEntrada.IsBackground = true;
                  //Inicia el hilo
                  threadBandejaEntrada.Start();
                }
                //Si es una cuenta de Outlook
                else if (correo.Opcion.Equals("1"))
                {
                  //Se crea una instancia para cuentas de Outlook
                  Mail bandeja = new Mail("", "", "", null);
                  //Se define el hilo
                  threadBandejaEntrada = new Thread(bandeja.bandejaEntradaOutlook);
                  //Se pone como background para que se muera cuando se cierra la aplicacion
                  threadBandejaEntrada.IsBackground = true;
                  //Se inicia el hilo
                  threadBandejaEntrada.Start();
                }                
            }
        }
    }
}
