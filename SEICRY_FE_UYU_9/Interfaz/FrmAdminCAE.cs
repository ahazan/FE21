using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Globales;


namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene las operaciones para mostrar el Formulario de administración de CAE's
    /// </summary>
    class FrmAdminCAE : FrmBase
    {
        //Variables visibles en toda la clase
        private Matrix matriz;
        private Columns columnas;
        private Column columna;
        private DBDataSource dataSourceMatriz;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Invoca todos los metodos encargados de crear los diferentes componentes del Formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            AgregarMatriz();
            AgregarDataSources();
            EstablecerDataBind();
            CargarMatriz();
        }

        /// <summary>
        /// Crea la matriz para administrar los rangos para cada tipo de documento
        /// </summary>
        /// <param name="formUID"></param>
        private void AgregarMatriz()
        {
            Item itemMatriz;

            Formulario.Freeze(true);

            //Crea una nueva matriz
            itemMatriz = Formulario.Items.Add("mtxRangos", BoFormItemTypes.it_MATRIX);
            itemMatriz.AffectsFormMode = false;

            //Estable las propiedades a la matriz
            itemMatriz.Visible = true;
            itemMatriz.Left = 30;
            itemMatriz.Top = 15;
            itemMatriz.Width = 550;
            itemMatriz.Height = 245;

           

            //Se obtiene el item específico para la matriz
            matriz = ((Matrix)(itemMatriz.Specific));

            //Se establece el modo de seleccion
            matriz.SelectionMode = BoMatrixSelect.ms_Auto;

            //Se obtiene la lista de columnas de la matriz
            columnas = matriz.Columns;

            //Se crean cada una de las columnas y se establecen sus propiedades

            //---------------------------//
            //*************#*************//
            //---------------------------//

            columna = columnas.Add("#", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "#";
            columna.Width = 25;
            columna.Editable = false;

            //----------------------------------//
            //********TIPO DE DOCUMENTO*********//
            //----------------------------------//

            columna = columnas.Add("cTipDoc", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Tipo de Documento";
            columna.Width = 50;
            columna.Editable = false;

            //------------------------------------//
            //********NOMBRE DE DOCUMENTO*********//
            //------------------------------------//

            columna = columnas.Add("cNombDoc", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Nombre de Documento";
            columna.Width = 100;
            columna.Editable = false;

            //-----------------------------------//
            //********NUMERACION INICIAL*********//
            //-----------------------------------//

            columna = columnas.Add("cNumIn", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Número Inicial";
            columna.Width = 110;
            columna.Editable = false;

            //---------------------------------//
            //********NUMERACION FINAL*********//
            //---------------------------------//

            columna = columnas.Add("cNumFn", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Número Final";
            columna.Width = 110;
            columna.Editable = false;

            //----------------------//
            //********SERIE*********//
            //----------------------//

            columna = columnas.Add("cSerie", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Serie";
            columna.Width = 40;
            columna.Editable = false;

            //-----------------------------//
            //********VALIDO HASTA*********//
            //-----------------------------//

            columna = columnas.Add("cValHa", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Válido Hasta";
            columna.Width = 97;
            columna.Editable = false;

            Formulario.Freeze(false);
        }

        /// <summary>
        /// Asigna los data sources al formualario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AgregarDataSources()
        {
            //Asigna la tabla de usuario de CAE's como data source
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Add("@TFECAE");

            //Asigna el user data source 
            Formulario.DataSources.UserDataSources.Add("dsCbAct", BoDataType.dt_SHORT_TEXT, 1);
        }

        /// <summary>
        /// Asigna los data sources a cada una de las columnas de la matriz y al check box
        /// </summary>
        protected override void EstablecerDataBind()
        {
            columnas = ((Matrix)(Formulario.Items.Item("mtxRangos")).Specific).Columns;

            //Asignara data source a las columnas

            columna = columnas.Item("#");
            columna.DataBind.SetBound(true, "@TFECAE", "DocEntry");

            columna = columnas.Item("cTipDoc");
            columna.DataBind.SetBound(true, "@TFECAE", "U_TipoDoc");

            columna = columnas.Item("cNombDoc");
            columna.DataBind.SetBound(true, "@TFECAE", "U_NombDoc");

            columna = columnas.Item("cNumIn");
            columna.DataBind.SetBound(true, "@TFECAE", "U_NumIni");

            columna = columnas.Item("cNumFn");
            columna.DataBind.SetBound(true, "@TFECAE", "U_NumFin");

            columna = columnas.Item("cSerie");
            columna.DataBind.SetBound(true, "@TFECAE", "U_Serie");

            columna = columnas.Item("cValHa");
            columna.DataBind.SetBound(true, "@TFECAE", "U_ValHasta");

            //Asignar data soruce al check box
            ((CheckBox)(Formulario.Items.Item("cbAct")).Specific).DataBind.SetBound(true, "", "dsCbAct");
        }

        /// <summary>
        /// Obtener los datos de la base de datos y cargar la matriz
        /// </summary>
        public void CargarMatriz(bool activos = false)
        {
            Conditions condiciones;
            Condition condicion;
            
            matriz = ((Matrix)(Formulario.Items.Item("mtxRangos")).Specific);  

            //Limpiar la matriz
            matriz.Clear();

            //Realizar la consulta 
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Item("@TFECAE");

            //Validar si se deben mostar unicamente los rangos activos
            if (activos)
            {
                //Crear objeto condiciones
                condiciones = (Conditions)SAPbouiCOM.Framework.Application.SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions);
                
                //Crear instanacia de mantenimiento de CAE's
                ManteUdoCAE manteUdoCae = new ManteUdoCAE();

                //Conslutar los rangos activos y almacenarlos en arreglos de string. Contiene los DocEntry seleccionados
                string[] rangosActivos = manteUdoCae.ConsultarRangosActivos();

                //Recorrer el arreglo de rangos activos para crear las condiciones
                for (int i = 0; i < rangosActivos.Length; i++)
                {
                    //Agregar una nueva condicion
                    condicion = condiciones.Add();

                    //Establecer las propiedades de la condicion
                    condicion.Alias = "DocEntry";
                    condicion.Operation = BoConditionOperation.co_EQUAL;
                    condicion.CondVal = rangosActivos[i];

                    //Validar que no sea el ultimo valor del arreglo para no agregar la relacion OR a las condiciones
                    if (i + 1 < rangosActivos.Length)
                    {
                        condicion.Relationship = BoConditionRelationship.cr_OR;
                    }
                }

                //Ejecutar la consulta del data source de la matriz con las condiciones creadas
                dataSourceMatriz.Query(condiciones);

            }
            //Si no se deben filtrar los rangos...
            else
            {
                //Ejectuar la consulta del data source de la matriz sin condiciones
                dataSourceMatriz.Query(null);
            }

            //Congelar Formulario
            Formulario.Freeze(true);

            //Cargar la matriz desde el data source
            matriz.LoadFromDataSource();

            //Descongelar Formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Obtiene el valor del DocEntry para el rango seleccionado. Si no hay ningún rango seleccionado muestra un mensaje de error
        /// </summary>
        public string ObtenerRangoSeleccionado()
        {
            string docEntrySeleccionado = "";

            //Obtiene el Formulario
            ObtenerFormulario("frmCAE");
            
            //Obtiene la matriz
            matriz = ((Matrix)(Formulario.Items.Item("mtxRangos")).Specific);

            //Obtiene la fila seleccionada
            int numeroFila = matriz.GetNextSelectedRow();

            //Validar que exista una fila seleccionada
            if (numeroFila > 0)
            {
                //Obtiene el DocEntry de la fila seleccionada
                docEntrySeleccionado = ((EditText)matriz.Columns.Item("#").Cells.Item(numeroFila).Specific).String;
            }
            else
            {
                //Muestra el mensaje de error cuando no se ha seleccionado ninguna fila
                AdminEventosUI.mostrarMensaje(Mensaje.errNingunRangoSelecccionado, AdminEventosUI.tipoError);                
            }

            return docEntrySeleccionado;
        }

        /// <summary>
        /// Acutualiza los datos del la matriz
        /// </summary>
        /// <param name="formUID"></param>
        public void RefrescarFormulario(string formUID)
        {
            ObtenerFormulario(formUID);
            EstablecerDataBind();

            //Obtener el valor del check box para mostrar solo los rangos activos o todos
            bool checkSeleccionado = ((ICheckBox)Formulario.Items.Item("cbAct").Specific).Checked;

            CargarMatriz(checkSeleccionado);
        }

        /// <summary>
        /// Ajusta el formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        /// <summary>
        /// Valida que el rango haya sido utlizado
        /// </summary>
        /// <param name="numeroRango"></param>
        /// <returns></returns>
        public bool ValidarRangoUtilizado(string numeroRango)
        {
            //Crea una nueva instancia de mantenimiento de rangos
            ManteUdoRango manteUdoRango = new ManteUdoRango();

            //Consulta si el rango ha sido utilizado
            return manteUdoRango.RangoUtilizado(numeroRango);
        }

        /// <summary>
        /// Elimina el rango seleccionado
        /// </summary>
        /// <param name="numeroCae"></param>
        /// <returns></returns>
        public bool EliminarRango(string numeroCae)
        {
            SEICRY_FE_UYU_9.Udos.ManteUdoCAE manteUdoCae = new Udos.ManteUdoCAE();

            return manteUdoCae.Eliminar(numeroCae);
        }


        #endregion MANTENIMIENTO

        
    }
}
