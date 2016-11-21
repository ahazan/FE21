using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using System.Collections;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmSucuDire : FrmBase  
    {
        Matrix matriz;
        private Columns columnas;
        private Column columna;
        private DBDataSource dataSourceMatriz;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Crea los componentes del formulario
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {
            ObtenerFormulario(formUID);
            AgregarMatriz();
            AgregarDataSources();
            EstablecerDataBind();
            CargarMatriz();
            AgregarNuevaLinea();
        }

        /// <summary>
        /// Crea la matriz para administrar los datos de retencion/percepcion
        /// </summary>
        /// <param name="formUID"></param>
        private void AgregarMatriz()
        {
            Item itemMatriz;

            Formulario.Freeze(true);

            //Crea una nueva matriz
            itemMatriz = Formulario.Items.Add("mtxSucDir", BoFormItemTypes.it_MATRIX);
            itemMatriz.AffectsFormMode = false;

            //Estable las propiedades a la matriz
            itemMatriz.Visible = true;
            itemMatriz.Left = 10;
            itemMatriz.Top = 15;
            itemMatriz.Width = 743;
            itemMatriz.Height = 360;

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

            //---------------------------//
            //*************#*************//
            //---------------------------//

            columna = columnas.Add("cDocEntry", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "";
            columna.Width = 25;
            columna.Visible = false;

            //----------------------------------//
            //********SUJETO PASIVO*********//
            //----------------------------------//

            columna = columnas.Add("cCodigo", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Código Sucursal";
            columna.Width = 85;

            //---------------------------------------//
            //********CONTRIBUYENTE RETENIDO*********//
            //---------------------------------------//

            columna = columnas.Add("cCalle", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Dirección";
            columna.Width = 250;

            //-----------------------------------//
            //********Numero de Calle************//
            //-----------------------------------//

            columna = columnas.Add("cTelefono", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Teléfono";
            columna.Width = 80;


            //-----------------------------------//
            //********Ciudad*********************//
            //-----------------------------------//

            columna = columnas.Add("cCiudad", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Ciudad";
            columna.Width = 150;   

           

            Formulario.Freeze(false);
        }

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
            //Asigna la tabla de usuario de CAE's como data source
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Add("@TSUCDIRE");
        }

        protected override void EstablecerDataBind()
        {
            columnas = ((Matrix)(Formulario.Items.Item("mtxSucDir")).Specific).Columns;

            //Asignara data source a las columnas

            columna = columnas.Item("#");

            columna = columnas.Item("cDocEntry");
            columna.DataBind.SetBound(true, "@TSUCDIRE", "DocEntry");

            columna = columnas.Item("cCodigo");
            columna.DataBind.SetBound(true, "@TSUCDIRE", "U_codigo");

            columna = columnas.Item("cCalle");
            columna.DataBind.SetBound(true, "@TSUCDIRE", "U_Calle");

            columna = columnas.Item("cTelefono");
            columna.DataBind.SetBound(true, "@TSUCDIRE", "U_Telefono");

            columna = columnas.Item("cCiudad");
            columna.DataBind.SetBound(true, "@TSUCDIRE", "U_Ciudad");


           

        }

        /// <summary>
        /// Obtener los datos de la base de datos y cargar la matriz
        /// </summary>
        public void CargarMatriz()
        {
            matriz = ((Matrix)(Formulario.Items.Item("mtxSucDir")).Specific);

            //Limpiar la matriz
            matriz.Clear();

            //Realizar la consulta 
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Item("@TSUCDIRE");

            //Ejectuar la consulta del data source de la matriz sin condiciones
            dataSourceMatriz.Query(null);
            
            //Congelar Formulario
            Formulario.Freeze(true);

            //Cargar la matriz desde el data source
            matriz.LoadFromDataSource();

            //Descongelar Formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Establece el estado del botón OK para que sea Actualizar u OK dependiendo del caso
        /// </summary>
        /// <param name="estadoOK"></param>
        public void EstadoBotonOK(bool estadoOK)
        {
            BotonOK = estadoOK;

            //Congelar el Formulario
            Formulario.Freeze(true);

            if (estadoOK)
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = "OK";
            }
            else
            {
                ((Button)Formulario.Items.Item("btnOK").Specific).Caption = "Actualizar";
            }

            //Descongelar el Formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Agrega una nueva linea para ingresar un nuevo registro 
        /// </summary>
        private void AgregarNuevaLinea()
        {
            matriz.AddRow();
            matriz.ClearRowData(matriz.RowCount);

            matriz.FlushToDataSource();
            dataSourceMatriz.SetValue("DocEntry", matriz.RowCount - 1, "");
            
            matriz.Clear();
            matriz.LoadFromDataSource();
        }

        /// <summary>
        /// Ajusta Formulario
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        public bool Almacenar()
        {
            SucuDireccion SucDire;
            ArrayList listaSucuDire = new ArrayList();

            //Valida que la matriz contenga información. Si no tiene se ingresa los datos como registros nuevos
            if (matriz.RowCount > 0)
            {
                //Carga el data source con los datos de la matriz
                matriz.FlushToDataSource();

                //Recorre el data source
                for (int i = 0; i < dataSourceMatriz.Size; i++)
                {
                    //Crea un nuevo objeto retencion percepcion
                    SucDire = new SucuDireccion();

                    //Establce las propiedades del objeto
                    SucDire.IdidSucuDire = dataSourceMatriz.GetValue("DocEntry", i);
                    SucDire.Codigo = dataSourceMatriz.GetValue("U_codigo", i).Trim();
                    SucDire.Calle = dataSourceMatriz.GetValue("U_Calle", i).Trim();
                    SucDire.Telefono  = dataSourceMatriz.GetValue("U_Telefono", i).Trim();
                    SucDire.Ciudad = dataSourceMatriz.GetValue("U_Ciudad", i).Trim();
                  

                    //Agrega el objeto a la lista
                    listaSucuDire.Add(SucDire);
                }

                //Crea una nueva instancia de adminstracion del udo de SucDire
                ManteUdoSucuDire manteSucDire = new ManteUdoSucuDire();

                //Elimina los registros existentes
                manteSucDire.Eliminar(listaSucuDire);
                
                //Agrega los nuevos registros
                if (manteSucDire.Almacenar(listaSucuDire))
                {
                    CargarMatriz();
                    AgregarNuevaLinea();
                    return true;
                }
            }

            return false;
        }

        #endregion MANTENIMIENTO

        #region PROPIEDADES

        private bool botonOK = true;

        public bool BotonOK
        {
            get { return botonOK; }
            set { botonOK = value; }
        }

        #endregion PROPIEDADES

        
    }
}
