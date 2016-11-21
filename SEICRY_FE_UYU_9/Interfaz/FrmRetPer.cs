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
    class FrmRetPer : FrmBase  
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
            itemMatriz = Formulario.Items.Add("mtxRetPer", BoFormItemTypes.it_MATRIX);
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

            columna = columnas.Add("cSuPas", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Sujeto Pasivo";
            columna.Width = 150;

            //---------------------------------------//
            //********CONTRIBUYENTE RETENIDO*********//
            //---------------------------------------//

            columna = columnas.Add("cConRet", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Contribuyente Retenido";
            columna.Width = 150;

            //-----------------------------------//
            //********AGENTE/RESPONSABLE*********//
            //-----------------------------------//

            columna = columnas.Add("cAgente", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Agente/Responsable";
            columna.Width = 150;

            //------------------------------------------//
            //********Formulario/LINEA DEL BETA*********//
            //------------------------------------------//

            columna = columnas.Add("cFormBeta", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Formulario/Línea del Beta";
            columna.Width = 150;

            //---------------------------------//
            //********CODIGO RETENCION*********//
            //---------------------------------//

            columna = columnas.Add("cCodRet", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Código Retención";
            columna.Width = 100;

            Formulario.Freeze(false);
        }

        /// <summary>
        /// Agrega DataSources
        /// </summary>
        protected override void AgregarDataSources()
        {
            //Asigna la tabla de usuario de CAE's como data source
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Add("@TFERP");
        }

        protected override void EstablecerDataBind()
        {
            columnas = ((Matrix)(Formulario.Items.Item("mtxRetPer")).Specific).Columns;

            //Asignara data source a las columnas

            columna = columnas.Item("#");

            columna = columnas.Item("cDocEntry");
            columna.DataBind.SetBound(true, "@TFERP", "DocEntry");

            columna = columnas.Item("cSuPas");
            columna.DataBind.SetBound(true, "@TFERP", "U_SuPas");

            columna = columnas.Item("cConRet");
            columna.DataBind.SetBound(true, "@TFERP", "U_ConRet");

            columna = columnas.Item("cAgente");
            columna.DataBind.SetBound(true, "@TFERP", "U_Agente");

            columna = columnas.Item("cFormBeta");
            columna.DataBind.SetBound(true, "@TFERP", "U_FormBeta");

            columna = columnas.Item("cCodRet");
            columna.DataBind.SetBound(true, "@TFERP", "U_CodRet");

        }

        /// <summary>
        /// Obtener los datos de la base de datos y cargar la matriz
        /// </summary>
        public void CargarMatriz()
        {
            matriz = ((Matrix)(Formulario.Items.Item("mtxRetPer")).Specific);

            //Limpiar la matriz
            matriz.Clear();

            //Realizar la consulta 
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Item("@TFERP");

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
            RetencionPercepcion retPer;
            ArrayList listaRetencionPercepcion = new ArrayList();

            //Valida que la matriz contenga información. Si no tiene se ingresa los datos como registros nuevos
            if (matriz.RowCount > 0)
            {
                //Carga el data source con los datos de la matriz
                matriz.FlushToDataSource();

                //Recorre el data source
                for (int i = 0; i < dataSourceMatriz.Size; i++)
                {
                    //Crea un nuevo objeto retencion percepcion
                    retPer = new RetencionPercepcion();

                    //Establce las propiedades del objeto
                    retPer.IdRetencionPercepcion = dataSourceMatriz.GetValue("DocEntry", i);
                    retPer.SujetoPasivo =  dataSourceMatriz.GetValue("U_SuPas", i).Trim();
                    retPer.ContribuyenteRetenido = dataSourceMatriz.GetValue("U_ConRet", i).Trim();
                    retPer.AgenteResponsable = dataSourceMatriz.GetValue("U_Agente", i).Trim();
                    retPer.FormularioLineaBeta = dataSourceMatriz.GetValue("U_FormBeta", i).Trim();
                    retPer.CodigoRetencion = dataSourceMatriz.GetValue("U_CodRet", i).Trim();

                    //Agrega el objeto a la lista
                    listaRetencionPercepcion.Add(retPer);
                }

                //Crea una nueva instancia de adminstracion del udo de retencion/percepcion
                ManteUdoRetencionPercepcion manteRecPer = new ManteUdoRetencionPercepcion();

                //Elimina los registros existentes
                manteRecPer.Eliminar(listaRetencionPercepcion);
                
                //Agrega los nuevos registros
                if (manteRecPer.Almacenar(listaRetencionPercepcion))
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
