using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;
using System.IO;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.Interfaz
{

    class FrmVisualizar
    {
        #region INTERFAZ DE USUARIO

        private Form visualiza;
        private SHDocVw.InternetExplorer oSHDocVw = null;

        /// <summary>
        /// Cargar el xml y muestra el formulario para visualizar archivos xml
        /// </summary>
        public void mostrarFormulario()
        {
            try
            {
                //documento xml donde se cargará el xml de la ventana
                XmlDocument xmlDocumento = new XmlDocument();

                //Cargar documento xml
                xmlDocumento.Load(RutasFormularios.FRM_VISUALIZAR);

                //Mostar la ventana
                SAPbouiCOM.Framework.Application.SBO_Application.LoadBatchActions(xmlDocumento.InnerXml);
            }
            catch(Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("FrmVisualizar/MostrarFormulario/Error " + ex.ToString());
            }
        }

        /// <summary>
        /// Obtiene el formulario para que pueda ser utilizado en toda la clase
        /// </summary>
        /// <param name="formUID"></param>
        public void obtenerFormulario(string formUID, string ruta)
        {
            try
            {
                //Obtiene el formulario de Visualizar XML
                visualiza = SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item("frmVisualizar");

                //Agrega el objeto Browser de tipo ActiveX en el formulario
                Item oItem = visualiza.Items.Add("Brwsr", SAPbouiCOM.BoFormItemTypes.it_ACTIVE_X);
                oItem.Height = visualiza.Height - 37;
                oItem.Width = visualiza.Width - 15;

                ActiveX axBrwsr = (ActiveX)oItem.Specific;

                //Se define la clase para el objeto ActiveX                       
                axBrwsr.ClassID = "Shell.Explorer.2";

                //Se crea el visor de Internet Explorer
                oSHDocVw = ((SHDocVw.InternetExplorer)(axBrwsr.Object));

                //Se carga el archivo xml al visor
                oSHDocVw.Navigate(ruta, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //Se borra el archivo cargado
                File.Delete(ruta);
            }
            catch(Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("ObtenerFormulario/Error:" + ex.ToString());
            }
        }

        /// <summary>
        /// Establecer el formulario como seleccionado para que muestre en el frente de la pantalla
        /// </summary>
        public void seleccionarFormulario(string ruta)
        {
            try
            {
                //Se comprueba que el objeto no este nulo
                if (oSHDocVw != null)
                {
                    //Se carga el archivo xml al visor
                    oSHDocVw.Navigate(ruta);
                }

                visualiza.Select();
            }
            catch(Exception)
            {
            }
        }

        /// <summary>
        /// Cierra el formulario de visualizacion de archivos xml
        /// </summary>
        /// <param name="formUID"></param>
        public void cerrarFormulario(string formUID)
        {
            try
            {
                if (oSHDocVw != null)
                {
                    oSHDocVw.Stop();                    
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSHDocVw);                    
                    System.GC.Collect();
                    oSHDocVw = null;
                }
                //Cerrar ventana
                visualiza.Close();
            }
            catch (Exception)
            {
            }
        }
        #endregion INTERFAZ DE USUARIO
    }
}
