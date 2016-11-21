using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene los metodos para crear los menus requeridos en la aplicación
    /// </summary>
    class ProcCreacionMenus
    {
        /// <summary>
        /// Crea los menús de la aplicación a partir de un archivo xml
        /// </summary>
        public void CrearMenusFE(bool tipoCompilado)
        {
            string sPath = System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath).ToString();

            XmlDocument xmlDocumento = new XmlDocument();
            //Se usa menu del administrador
            if (tipoCompilado)
            {
                AdminEventosUI.modoUsuario = true;
                xmlDocumento.Load(@"Interfaz\Formularios\MenusFEAdmin.xml");
            }
            //Se usa el menu de usuario sin permisos
            else
            {
                AdminEventosUI.modoUsuario = false;
                xmlDocumento.Load(@"Interfaz\Formularios\MenusFEUsuario.xml");
            }

            SAPbouiCOM.Framework.Application.SBO_Application.LoadBatchActions(xmlDocumento.InnerXml);

            try
            {
                if (System.IO.File.Exists(sPath + "\\invoice.bmp"))
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("mFE").Image = sPath + "\\invoice.bmp";
                }

                String[] files = Directory.GetFiles(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory), "MenuFE.png");

                //System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath).ToString();


                if (System.IO.File.Exists(files[0]))
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("mFE").Image = files[0];
                }

                //else if (System.IO.File.Exists(sPath + "\\FEUruguay\\invoice.bmp"))
                //{
                //    SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("mFE").Image = sPath + "\\FEUruguay\\invoice.bmp";
                //}
            }
            catch (Exception)
            {
            }     
        }
    }
}
