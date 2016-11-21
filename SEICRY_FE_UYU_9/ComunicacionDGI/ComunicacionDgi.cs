using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;

namespace SEICRY_FE_UYU_9.ComunicacionDGI
{
    /// <summary>
    /// Contiene los metodos para el inicio de los distintos jobs que consumen los web services de la DGI
    /// </summary>
    class ComunicacionDgi
    {
        /// <summary>
        /// Inicia el job para el envio de un Sobre a la DGI
        /// </summary>
        /// <param name="parametros"></param>
        public void ConsumirWsEnviarSobre(object parametros)
        {
            JobEnvioSobre jobEnvioSobre = new JobEnvioSobre();
            ParameterizedThreadStart inicioParametrizado = new ParameterizedThreadStart(jobEnvioSobre.Trabajar);

            try
            {          
                             
                Thread threadEnvioSobre = new Thread(inicioParametrizado);
                threadEnvioSobre.IsBackground = true;
                threadEnvioSobre.Start(parametros);
            }
            catch(Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ComunicacionDGI/Error: " + ex.ToString());
            }

            finally
            {

                if (inicioParametrizado != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(inicioParametrizado);                   
                    GC.Collect();
                }

             
                if (jobEnvioSobre != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(jobEnvioSobre);                 
                    GC.Collect();
                }

            }
        }

        /// <summary>
        /// Inicia el job para el envio de un Sobre a la DGI
        /// </summary>
        /// <param name="parametros"></param>
        public void ConsumirWsEnviarSobreMasivo(object parametros)
        {

            JobEnvioSobreMasivo jobEnvioSobreMasivo = new JobEnvioSobreMasivo();

            ParameterizedThreadStart inicioParametrizado = new ParameterizedThreadStart(jobEnvioSobreMasivo.Trabajar);
            Thread threadEnvioSobreMasivo = new Thread(inicioParametrizado);

            try
            {
                
                threadEnvioSobreMasivo.IsBackground = true;
                threadEnvioSobreMasivo.Start(parametros);
            }
            catch (Exception)
            {
            }

            finally
            {

                if (threadEnvioSobreMasivo != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(threadEnvioSobreMasivo);
                    GC.Collect();
                }


                if (jobEnvioSobreMasivo != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(jobEnvioSobreMasivo);
                    GC.Collect();
                }


                if (inicioParametrizado != null)
                {
                    //Libera de memoria el objeto factura
                    GC.SuppressFinalize(inicioParametrizado);
                    GC.Collect();
                }

            }
        }

        /// <summary>
        /// Inicia el job que consulta el estado de los sobres en transito
        /// </summary>
        /// <param name="parametros"></param>
        public void ConsumirWsConsultarEstadoSobre(object parametros)
        {
            JobConsultaEnvio jobConsultaEnvio = new JobConsultaEnvio();

            ParameterizedThreadStart inicioParametrizado = new ParameterizedThreadStart(jobConsultaEnvio.Trabajar);

            Thread threadConsultaEnvio = new Thread(inicioParametrizado);
            threadConsultaEnvio.IsBackground = true;

            threadConsultaEnvio.Start(parametros);

            
        }


        /// <summary>
        /// Inicia el job que consulta el estado de los sobres Trancados
        /// </summary>
        /// <param name="parametros"></param>
        public void ConsumirWsConsultarEstadoSobreTrancados(object parametros)
        {
            JobConsultaEnvio jobConsultaEnvio = new JobConsultaEnvio();

            ParameterizedThreadStart inicioParametrizado = new ParameterizedThreadStart(jobConsultaEnvio.TrabajarCFE_Trancados);

            Thread threadConsultaEnvio = new Thread(inicioParametrizado);
            threadConsultaEnvio.IsBackground = true;

            threadConsultaEnvio.Start(parametros);


        }
    }
}
