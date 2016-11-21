using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Representa la estructura base para todos los Formularios de la aplicacion
    /// </summary>
    public abstract class FrmBase 
    {
        #region METODOS_VIRTUALES

        /// <summary>
        /// Invoca todos los metodos encargados de crear los diferentes componentes del Formulario
        /// </summary>
        /// <param name="formUID"></param>
        public virtual void MostarFormulario(string formUID, string rutaFormulario)
        {
            if (FormularioActivo)
            {
                SeleccionarFormulario();
            }
            else
            {
                CrearFormulario(rutaFormulario);//Crear el formulario nuevo
                ObtenerFormulario(formUID);//Obtiene el formulario para que sea accesible en otras operaciones
                CrearComponentes(formUID);                
                AjustarFormulario(formUID);//Ajusta el formulario de acuerdo a las necesidades de cada uno
            }           
        }
       
        /// <summary>
        /// Cargar el xml y muestara el Formulario 
        /// </summary>
        public virtual void CrearFormulario(string rutaFormulario)
        {
            //documento xml donde se cargará el xml de la ventana
            XmlDocument xmlDocumento = new XmlDocument();

            //Ubicacion del xml
            string rutaFrm = rutaFormulario;

            //Cargar documento xml
            xmlDocumento.Load(rutaFrm);

            //Mostar la ventana
            SAPbouiCOM.Framework.Application.SBO_Application.LoadBatchActions(xmlDocumento.InnerXml);

            FormularioActivo = true;
        }

        /// <summary>
        /// Obtiene el Formulario para que pueda ser utilizado en toda la clase
        /// </summary>
        /// <param name="formUID"></param>
        public virtual void ObtenerFormulario(string formUID)
        {
            //Obtiene el Formulario de administracion de CAE's
            Formulario = SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item(formUID);
        }

        /// <summary>
        /// Establecer el Formulario como seleccionado para que muestre en el frente de la pantalla
        /// </summary>
        public virtual void SeleccionarFormulario()
        {
            Formulario.Select();
        }

        /// <summary>
        /// Cierra el Formulario de CAE's
        /// </summary>
        /// <param name="formUID"></param>
        public virtual void CerrarFormulario()
        {
            if (FormularioActivo)
            {
                FormularioActivo = false;
                //Cerrar ventana
                Formulario.Close();
            }
        }

        /// <summary>
        /// Crea los componentes propios de cada formulario
        /// </summary>
        /// <param name="formUID"></param>
        public virtual void CrearComponentes(string formUID){}

        #endregion METODOS_VIRTUALES

        #region METODOS_ABSTRACTOS

        /// <summary>
        /// Asigna los data sources al formualario
        /// </summary>
        /// <param name="formUID"></param>
        protected abstract void AgregarDataSources();

        /// <summary>
        /// Asigna los data sources a cada uno de los componentes
        /// </summary>
        protected abstract void EstablecerDataBind();

        /// <summary>
        /// Realiza los ajustes propios de cada formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected abstract void AjustarFormulario(string formUID);

        #endregion METODOS_ABSTRACTOS

        #region PROPIEDADES

        private Form formulario;

        protected Form Formulario
        {
            get { return formulario; }
            set { formulario = value; }
        }

        private bool formularioActivo = false;

        public bool FormularioActivo
        {
            get { return formularioActivo; }
            set { formularioActivo = value; }
        }

        #endregion PROPIEDADES

        #region FE_EXPORTACION
        /// <summary>
        /// Agrega un control a un formulario
        /// </summary>
        public Item AgregarControl(Item item, string referencia, string UID, BoFormItemTypes tipo, int fromPane, int toPane)
        {
            try
            {
                Item itemReferencia = Formulario.Items.Item(referencia);

                item = Formulario.Items.Add(UID, tipo);
                item.Left = itemReferencia.Left;
                item.Top = itemReferencia.Top + itemReferencia.Height + 1;
                item.Width = itemReferencia.Width;
                item.Height = itemReferencia.Height;
                item.ToPane = toPane;
                item.FromPane = fromPane;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //itemReferencia = null;
            }
            return item;
        }

        /// <summary>
        /// Crea una etiqueta al formulario
        /// </summary>
        public void CrearEtiqueta(Item item, string etiqueta, string linkTo)
        {
            ((StaticText)item.Specific).Caption = etiqueta;
            item.LinkTo = linkTo;
        }
        #endregion FE_EXPORTACION
    }
}
