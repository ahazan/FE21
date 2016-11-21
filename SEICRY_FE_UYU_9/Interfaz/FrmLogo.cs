using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmLogo : FrmBase
    {
        #region INTERFAZ DE USUARIO
        
        /// <summary>
        /// Agregar DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {            
        }

        /// <summary>
        /// Establece el DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {            
        }

        /// <summary>
        /// Ajustar el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            string rutaLogo = "";
            int TimeImp = 0;

            try
            {

                ManteUdoLogo mante = new ManteUdoLogo();
                
                rutaLogo = mante.Consultar(true);
                TimeImp = mante.ConsultarTimeImp(true);


                ((EditText)Formulario.Items.Item("txtTimeImp").Specific).Value =  TimeImp.ToString() ;
                ((EditText)Formulario.Items.Item("txtRuta").Specific).Value = rutaLogo;
             
            }
            catch (Exception)
            {
            }
        }

        #endregion INTERFAZ DE USUARIO
    }
}
