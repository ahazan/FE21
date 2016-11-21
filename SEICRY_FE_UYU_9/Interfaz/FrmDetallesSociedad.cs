using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Globales;

namespace SEICRY_FE_UYU_9.Interfaz
{   
    class FrmDetallesSociedad : FrmBase
    {
        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Agrega un nuevo tab a la ventana de Detalles de la Compañia y ajusta las propiedades de tamaño
        /// </summary>
        /// <param name="FormUID"></param>
        public void AgregarTabFE(string FormUID)
        {
            //Obtener formulario de Detalles de la Compania
            Formulario = SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item(FormUID);

            //Congelar el formulario
            Formulario.Freeze(true);

            //Modificar propiedades del formulario de Detalles de la Compania
            Formulario.Width = 530;

            //Crear nuevo item para el tab de facturacion electronica
            Item itemTabFe = Formulario.Items.Add("tabFE", BoFormItemTypes.it_FOLDER);

            //Obtener tab de inicialización básica
            Item itemTabIniciBasic = Formulario.Items.Item("36");

            //Establecer propiedades al nuevo tab respecto al tab de inicialización basica
            itemTabFe.Top = itemTabIniciBasic.Top;
            itemTabFe.Left = itemTabIniciBasic.Left;
            itemTabFe.Width = 300;
            itemTabFe.Height = itemTabIniciBasic.Height;
           

            //Crear forlder especifico
            Folder tabFe = (Folder)itemTabFe.Specific;

            //Establecer propiedades espcificas al tab 
            tabFe.Caption = "Factura Elecrtrónica";
            tabFe.Pane = 26;
            //Agrupar el nuevo tab con el tab de inicializacion basica
            tabFe.GroupWith("36");

            AgregarComponentesTabFE();
            MostarValores();
            Formulario.PaneLevel = 2;

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Invocado cuando se presiona el nuevo tab
        /// </summary>
        public void SeleccionarTabFe()
        {
            //Cambia el nivel del panel al del nuevo tab
            Formulario.PaneLevel = 26;
        }

        /// <summary>
        /// Crea los componentes de interfaz dentro del nuevo tab
        /// </summary>
        private void AgregarComponentesTabFE()
        {
            //Tomar item de referencia para ubicacion de nuevos items
            Item itemReferencia = Formulario.Items.Item("19");

            #region ETIQUETAS

            //RUC...Crear y establecer propiedades
            Item itemLbRuc = Formulario.Items.Add("lbDigVe", BoFormItemTypes.it_STATIC);
            itemLbRuc.Left = itemReferencia.Left;
            itemLbRuc.Top = itemReferencia.Top;
            itemLbRuc.Width = 180;
            itemLbRuc.FromPane = 26;
            itemLbRuc.ToPane = 26;

            ((StaticText)(itemLbRuc.Specific)).Caption = "Dígito Verificador";

            //NOMBRE...Crear y establecer propiedades
            Item itemLbNombre = Formulario.Items.Add("lbNomb", BoFormItemTypes.it_STATIC);
            itemLbNombre.Left = itemReferencia.Left;
            itemLbNombre.Top = itemReferencia.Top + itemLbRuc.Height + 1;
            itemLbNombre.Width = 180;
            itemLbNombre.FromPane = 26;
            itemLbNombre.ToPane = 26;

            ((StaticText)(itemLbNombre.Specific)).Caption = "Nombre del Emisor";

            //NOMBRE COMERCIAL...Crear y establecer propiedades
            Item itemLbNombreComercial = Formulario.Items.Add("lbNombC", BoFormItemTypes.it_STATIC);
            itemLbNombreComercial.Left = itemReferencia.Left;
            itemLbNombreComercial.Top = itemLbNombre.Top + itemLbNombre.Height + 1;
            itemLbNombreComercial.Width = 180;
            itemLbNombreComercial.FromPane = 26;
            itemLbNombreComercial.ToPane = 26;

            ((StaticText)(itemLbNombreComercial.Specific)).Caption = "Nombre Comercial";

            //NUMERO DE RESOLUCION...Crear y establecer propiedades
            Item itemLbNumResolucion = Formulario.Items.Add("lbNumRes", BoFormItemTypes.it_STATIC);
            itemLbNumResolucion.Left = itemReferencia.Left;
            itemLbNumResolucion.Top = itemLbNombreComercial.Top + itemLbNombre.Height + 1;
            itemLbNumResolucion.Width = 180;
            itemLbNumResolucion.FromPane = 26;
            itemLbNumResolucion.ToPane = 26;

            ((StaticText)(itemLbNumResolucion.Specific)).Caption = "Número de Resolución";
            
            #endregion

            #region CAMPOS DE TEXO

            //RUC...Crear y establecer propiedades
            Item itemTxtRuc = Formulario.Items.Add("txtRuc", BoFormItemTypes.it_EDIT);
            itemTxtRuc.Left = itemLbRuc.Left + itemLbRuc.Width + 20;
            itemTxtRuc.Top = itemLbRuc.Top;
            itemTxtRuc.FromPane = 26;
            itemTxtRuc.ToPane = 26;
            itemTxtRuc.Width = 250;
 
            //NOMBRE...Crear y establecer propiedades
            Item itemTxtNombre = Formulario.Items.Add("txtNomb", BoFormItemTypes.it_EDIT);
            itemTxtNombre.Left = itemTxtRuc.Left;
            itemTxtNombre.Top = itemLbNombre.Top;
            itemTxtNombre.FromPane = 26;
            itemTxtNombre.ToPane = 26;
            itemTxtNombre.Width = 250;

            //NOMBRE COMERCIAL...Crear y establecer propiedades
            Item itemTxtNombreComercial = Formulario.Items.Add("txtNombC", BoFormItemTypes.it_EDIT);
            itemTxtNombreComercial.Left = itemTxtRuc.Left;
            itemTxtNombreComercial.Top = itemLbNombreComercial.Top;
            itemTxtNombreComercial.FromPane = 26;
            itemTxtNombreComercial.ToPane = 26;
            itemTxtNombreComercial.Width = 250;

            //NUMERO DE RESOLUCION...Crear y establecer propiedades
            Item itemTxtNumeroResolucion = Formulario.Items.Add("txtNumRes", BoFormItemTypes.it_EDIT);
            itemTxtNumeroResolucion.Left = itemTxtRuc.Left;
            itemTxtNumeroResolucion.Top = itemLbNumResolucion.Top;
            itemTxtNumeroResolucion.FromPane = 26;
            itemTxtNumeroResolucion.ToPane = 26;
            itemTxtNumeroResolucion.Width = 250;

            #endregion

            //Relacionar etiquetas con campos de texto
            itemLbRuc.LinkTo = "txtRuc";
            itemLbNombre.LinkTo = "txtNomb";
            itemLbNombreComercial.LinkTo = "txtNombC";
            itemLbNumResolucion.LinkTo = "txtNumRes";
        }

        /// <summary>
        /// Carga los valores existentes en la base datos
        /// </summary>
        /// <param name="emisor"></param>
        private void MostarValores()
        {
            Item itemTxtRuc = Formulario.Items.Item("txtRuc");
            Item itemTxtNomb = Formulario.Items.Item("txtNomb");
            Item itemTxtNombC = Formulario.Items.Item("txtNombC");
            Item itemTxtNumRes = Formulario.Items.Item("txtNumRes");

            //Obtener instancia de mantenimiento de emisor
            ManteUdoEmisor manteEmisor = new ManteUdoEmisor();
            
            //Obtener objeto emisor consultado desde la base de datos
            Emisor emisor = manteEmisor.Consultar();

            //Validar que se haya obtenido informacion
            if (emisor != null)
            {
                //Establecer los valores a los campos de texto
                ((EditText)(itemTxtRuc.Specific)).String = emisor.Ruc.ToString();
                ((EditText)(itemTxtNomb.Specific)).String = emisor.Nombre;
                ((EditText)(itemTxtNombC.Specific)).String = emisor.NombreComercial;
                ((EditText)(itemTxtNumRes.Specific)).String = emisor.NumeroResolucion;
            }
        }

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
        /// Ajustar Formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {            
        }

        #endregion INTERFAZ DE USUARIO

        #region VALIDACIONES

        /// <summary>
        /// Realiza todas las validaciones requeridas
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            if (!ValidarFormatoRUC())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoRuc, AdminEventosUI.tipoError);                
                return false;
            }
            else if (!ValiarFormatoNombre())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoNombreEmisor, AdminEventosUI.tipoError);
                return false;
            }
            else if (!ValidarFormatoNombreComercial())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoNombreComercial, AdminEventosUI.tipoError);
                return false;
            }
            else if (!ValidarFormatoNumeroResolucion())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errFormatoNumResolucion, AdminEventosUI.tipoError);                
                return false;
            }
            else if (!ValidarRUC())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errRucObligatorio, AdminEventosUI.tipoError);               
                return false;
            }
            else if (!ValidarNombre())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errNombreEmisorObligatorio, AdminEventosUI.tipoError);
                return false;
            }
            else if (!ValidarNombreComercial())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errNombreComercialObligatorio, AdminEventosUI.tipoError);
                return false;
            }
            else if (!ValidarNumeroResolucion())
            {
                AdminEventosUI.mostrarMensaje(Mensaje.errNumResolucionObligatorio, AdminEventosUI.tipoError);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida que el ruc haya sido ingresado
        /// </summary>
        /// <returns></returns>
        public bool ValidarRUC()
        {
            if (((EditText)(Formulario.Items.Item("txtRuc").Specific)).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el nombre haya sido ingresado
        /// </summary>
        /// <returns></returns>
        private bool ValidarNombre()
        {
            if (((EditText)(Formulario.Items.Item("txtNomb").Specific)).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el nombre comercial haya sido ingresado
        /// </summary>
        /// <returns></returns>
        private bool ValidarNombreComercial()
        {
            if (((EditText)(Formulario.Items.Item("txtNombC").Specific)).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el numero de resolucion haya sido ingresado
        /// </summary>
        /// <returns></returns>
        private bool ValidarNumeroResolucion()
        {
            if (((EditText)(Formulario.Items.Item("txtNumRes").Specific)).String.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida que el formato del campo ruc sea correcto
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoRUC()
        {
            double resultado;

            //Valida que el tamaño no sea mayor a doce caracteres
            if (((EditText)(Formulario.Items.Item("txtRuc").Specific)).String.Length > 12)
            {
                return false;
            }

            //Valida que sea un valor numerico
            if (!double.TryParse(((EditText)(Formulario.Items.Item("txtRuc").Specific)).String, out resultado))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida que el formato del nombre sea correto
        /// </summary>
        /// <returns></returns>
        private bool ValiarFormatoNombre()
        {
            if (((EditText)(Formulario.Items.Item("txtNomb").Specific)).String.Length > 150)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida que el formato del nombre comercial sea correcto
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoNombreComercial()
        {
            if (((EditText)(Formulario.Items.Item("txtNombC").Specific)).String.Length > 30)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida que el formato del numero de resolucion sea correcto
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormatoNumeroResolucion()
        {
            if (((EditText)(Formulario.Items.Item("txtNumRes").Specific)).String.Length > 30)
            {
                return false;
            }

            return true;
        }

        #endregion VALIDACIONES

        #region MANTENIMIENTO

        /// <summary>
        /// Consulta los datos del emisor de facturas electronicas
        /// </summary>
        /// <returns></returns>
        public void Consutar()
        {
            //Obtener items de campos de texto
            Item itemTxtRuc = Formulario.Items.Item("txtRuc");
            Item itemTxtNomb = Formulario.Items.Item("txtNomb");
            Item itemTxtNombC = Formulario.Items.Item("txtNombC");
            Item itemTxtNumRes = Formulario.Items.Item("txtNumRes");

            //Crear nueva instancia de mantenimiento de emisores
            ManteUdoEmisor manteUdoEmisor = new ManteUdoEmisor();

            //Crear nuevo objeto emisor a partir de consulta de datos
            Emisor emisor = manteUdoEmisor.Consultar();

            //Valida que se hayan consultado datos
            if (emisor != null)
            {
                //Ingresa los valores obtenidos en cada uno de los campos
                ((EditText)(itemTxtRuc.Specific)).String = emisor.Ruc.ToString();
                ((EditText)(itemTxtNomb.Specific)).String = emisor.Nombre;
                ((EditText)(itemTxtNombC.Specific)).String = emisor.NombreComercial;
                ((EditText)(itemTxtNumRes.Specific)).String = emisor.NumeroResolucion;
            }
        }

        public bool ExistenDatos()
        {
            //Crear nueva instancia de mantenimiento de emisores
            ManteUdoEmisor manteUdoEmisor = new ManteUdoEmisor();

            //Crear nuevo objeto emisor a partir de consulta de datos
            Emisor emisor = manteUdoEmisor.Consultar();

            if (emisor != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Almacena los datos del emisor en la base de datos
        /// </summary>
        /// <returns></returns>
        public bool Almacenar()
        {
            //Obtener items de campos de texto
            Item itemTxtRuc = Formulario.Items.Item("txtRuc");
            Item itemTxtNomb = Formulario.Items.Item("txtNomb");
            Item itemTxtNombC = Formulario.Items.Item("txtNombC");
            Item itemTxtNumRes = Formulario.Items.Item("txtNumRes");

            //Crear nuevo objeto Emisor y asignar valores
            Emisor emisor = new Emisor();
            emisor.Ruc = long.Parse(((EditText)(itemTxtRuc.Specific)).String);
            emisor.Nombre = ((EditText)(itemTxtNomb.Specific)).String;
            emisor.NombreComercial = ((EditText)(itemTxtNombC.Specific)).String;
            emisor.NumeroResolucion = ((EditText)(itemTxtNumRes.Specific)).String;

            //Crear nueva instancia de mantenimiento de emisores
            ManteUdoEmisor manteUdoEmisor = new ManteUdoEmisor();

            //Consume el metodo de almacenar
            return manteUdoEmisor.Almacenar(emisor);
        }

        /// <summary>
        /// Actualiza los datos del emisor en la base de datos
        /// </summary>
        /// <returns></returns>
        public bool Actualizar()
        {
            //Obtener items de campos de texto
            Item itemTxtRuc = Formulario.Items.Item("txtRuc");
            Item itemTxtNomb = Formulario.Items.Item("txtNomb");
            Item itemTxtNombC = Formulario.Items.Item("txtNombC");
            Item itemTxtNumRes = Formulario.Items.Item("txtNumRes");

            //Crear nuevo objeto Emisor y asignar valores
            Emisor emisor = new Emisor();
            emisor.Ruc = long.Parse(((EditText)(itemTxtRuc.Specific)).String);
            emisor.Nombre = ((EditText)(itemTxtNomb.Specific)).String;
            emisor.NombreComercial = ((EditText)(itemTxtNombC.Specific)).String;
            emisor.NumeroResolucion = ((EditText)(itemTxtNumRes.Specific)).String;

            //Crear nueva instancia de mantenimiento de emisores
            ManteUdoEmisor manteUdoEmisor = new ManteUdoEmisor();

            //Consume el metodo de actualizar
            return manteUdoEmisor.Actualizar(emisor);
        }

        #endregion MATENIMMIENTO
    }
}
