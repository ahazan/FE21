using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SEICRY_FE_UYU_9.AbrirDialogo
{
    class DialogoArchivo
    {
        [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();
            OpenFileDialog _oFileDialog;
            
            /// <summary>
            /// Variable para almacenar el nombre de archivo
            /// </summary>
            public string nombreArchivo
            {
                get { return _oFileDialog.FileName; }
                set { _oFileDialog.FileName = value; }                
            }

            /// <summary>
            /// Get y set para el filtro de extensiones
            /// </summary>
            public string filtro
            {
                get { return _oFileDialog.Filter; }
                set { _oFileDialog.Filter = value; }
            }

            /// <summary>
            /// Get y set para el directorio inicial
            /// </summary>
      
            public string directorioInicial
            {
                get { return _oFileDialog.InitialDirectory; }
                set { _oFileDialog.InitialDirectory = value; }
            }

            /// <summary>
            /// Constructor de la clase
            /// </summary>
            public DialogoArchivo()
            {
                _oFileDialog = new OpenFileDialog();               
            }

            /// <summary>
            /// Obtiene el nombre del archivo
            /// </summary>
            public void obtenerNombreArchivo()
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);
                if (_oFileDialog.ShowDialog(oWindow) != DialogResult.OK)
                {
                    _oFileDialog.FileName = string.Empty;
                    System.Windows.Forms.Application.ExitThread();
                }
                else
                {
                    _oFileDialog.Dispose();
                }
                oWindow = null;
            }
    }
}
