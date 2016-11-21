using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Objetos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmCerDig : FrmBase
    {

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Agregar DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
        }

        /// <summary>
        /// Establecer DataBind
        /// </summary>
        protected override void EstablecerDataBind()
        {            
        }

        /// <summary>
        /// Ajustar el formulario
        /// </summary>
        protected override void AjustarFormulario(string formUID)
        {
            ManteUdoCertificadoDigital mantenimiento = new ManteUdoCertificadoDigital();

            try
            {
                //Se obtiene datos del certificado, si existen
                Certificado certificado = mantenimiento.Consultar();
                if (certificado != null)
                {
                    //Se asigna la ruta del certificado
                    ((EditText)SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item("frmCerDig").Items.Item("txtRuta").Specific).Value = certificado.RutaCertificado;
                }
            }
            catch(Exception)
            {
            }
        }

        #endregion INTERFAZ DE USUARIO
    }
}
