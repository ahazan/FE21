using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Globales;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmConfFinCae : FrmBase
    {
        ManteUdoCAE manteUdoCAE = new ManteUdoCAE();
        /// <summary>
        /// Agrega los datasources
        /// </summary>
        protected override void AgregarDataSources()
        {

       
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
            ConfCAEFin confCaeFin = manteUdoCAE.ObtenerConfiguracionCaeFin();

            ((EditText)Formulario.Items.Item("txtCan").Specific).Value = confCaeFin.NumCaeFin;
            ((EditText)Formulario.Items.Item("txtDia").Specific).Value = confCaeFin.FechaCaeFin;
        }

        /// <summary>
        /// Establece el databind
        /// </summary>
        protected override void EstablecerDataBind()
        {
        
        }
    }
}
