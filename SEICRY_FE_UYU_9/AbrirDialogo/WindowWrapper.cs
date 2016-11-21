using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.AbrirDialogo
{
    class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private IntPtr _hwnd;

        public virtual IntPtr Handle
        {
            get { return _hwnd; }
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="handle"></param>
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }
    }
}
