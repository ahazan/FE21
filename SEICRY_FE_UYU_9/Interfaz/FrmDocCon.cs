using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Xml;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;

namespace SEICRY_FE_UYU_9.Interfaz
{
    /// <summary>
    /// Contiene los metodos para mostrar el formulario de tipos de documentos que se deben conservar
    /// </summary>
    class FrmDocCon : FrmBase
    {
        //Varibles visibles en toda la clase
        Item itemMatriz;
        Matrix matriz;
        Columns columnas;
        Column columna;
        DBDataSource dbdsMatriz;

        #region INTERFAZ DE USUARIO

        /// <summary>
        /// Crea el formulario y los demas componentes
        /// </summary>
        /// <param name="formUID"></param>
        public override void CrearComponentes(string formUID)
        {            
            AgregarMatriz();
            AgregarDataSources();
            EstablecerDataBind();
            CargarMatriz();
            Almacenar();
        }

        /// <summary>
        /// Crea la matriz de tipos de documentos
        /// </summary>
        private void AgregarMatriz()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Crear nuevo item matriz
            itemMatriz =  Formulario.Items.Add("mtxTipDoc", BoFormItemTypes.it_MATRIX);

            //Establecer las propiedades a la matriz
            itemMatriz.Left = 10;
            itemMatriz.Top = 20;
            itemMatriz.Width = Formulario.Width - 30;
            itemMatriz.Height = Formulario.Height - 100;

            //Obtener objeto especifico
            matriz = ((Matrix)itemMatriz.Specific);

            //Obtener lista de columnas
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
            columna.Width = 100;
            columna.Editable = false;

            //------------------------------------//
            //********NOMBRE DE DOCUMENTO*********//
            //------------------------------------//

            columna = columnas.Add("cNombDoc", BoFormItemTypes.it_EDIT);
            columna.TitleObject.Caption = "Nombre de Documento";
            columna.Width = 160;
            columna.Editable = false;

            //------------------------------------------//
            //********INDICADOR DE CONSERVACION*********//
            //------------------------------------------//

            columna = columnas.Add("cIndCon", BoFormItemTypes.it_CHECK_BOX);
            columna.TitleObject.Caption = "Conservar";
            columna.Width = 70;
            columna.Editable = true;

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Crea el data source para los componentes
        /// </summary>
        protected  override void AgregarDataSources()
        {
            //Crea el db data source para la matriz
            dbdsMatriz = Formulario.DataSources.DBDataSources.Add("@TFETDCON");
        }

        /// <summary>
        /// Asigna los data sources a los componentes
        /// </summary>
        protected override void EstablecerDataBind()
        {
            columnas = ((Matrix)(Formulario.Items.Item("mtxTipDoc")).Specific).Columns;

            //Asignara data source a las columnas

            columna = columnas.Item("#");
            columna.DataBind.SetBound(true, "@TFETDCON", "DocEntry");

            columna = columnas.Item("cTipDoc");
            columna.DataBind.SetBound(true, "@TFETDCON", "U_TipoDoc");

            columna = columnas.Item("cNombDoc");
            columna.DataBind.SetBound(true, "@TFETDCON", "U_NombDoc");

            columna = columnas.Item("cIndCon");
            columna.DataBind.SetBound(true, "@TFETDCON", "U_IndCon");
        }

        /// <summary>
        /// Carga la matriz con los datos a travez del db data source
        /// </summary>
        private void CargarMatriz()
        {
            //Congelar el formulario
            Formulario.Freeze(true);

            //Limpia la tabla
            matriz.Clear();

            //Carga el data source
            dbdsMatriz.Query();

            //Cargar la matriz 
            matriz.LoadFromDataSource();

            //Descongelar el formulario
            Formulario.Freeze(false);
        }

        /// <summary>
        /// Ajustar el formulariol
        /// </summary>
        /// <param name="formUID"></param>
        protected override void AjustarFormulario(string formUID)
        {                
        }

        #endregion INTERFAZ DE USUARIO

        #region MANTENIMIENTO

        /// <summary>
        /// Ingresa los valores iniciales a la tabla
        /// </summary>
        private void Almacenar()
        {
            CAE cae;

            string valor;

            //Valida que la matriz contenga información. Si no tiene se ingresa los datos como registros nuevos
            if (matriz.RowCount == 0)
            {
                foreach (CAE.ESTipoCFECFC tipoCae in Enum.GetValues(typeof(CAE.ESTipoCFECFC)))
                {
                    valor = CAE.ObtenerStringTipoCFECFC(tipoCae);

                    //Crear nueva instanacia del mantenimiento de tipos de documentos a conservar
                    ManteUdoDocCon manteUdoDocCon = new ManteUdoDocCon();

                    //Crea una nueva instancia del objeto cae
                    cae = new CAE();
                    cae.TipoCFE = tipoCae;
                    cae.NombreDocumento = CAE.ObtenerNombreCFECFC(((int)tipoCae).ToString());
                    cae.IndicadorConservar = "Y";

                    //Almacenar el registro
                    manteUdoDocCon.Almacenar(cae);
                }
            }
        }

        /// <summary>
        /// Actualiza los datos en la tabla de la base de datos
        /// </summary>
        public void Actualizar()
        {
            CAE cae;
            string numeroRegistro;

            //Actualizar data source
            matriz.FlushToDataSource();

            //Crear nueva instanacia del mantenimiento de tipos de documentos a conservar
            ManteUdoDocCon manteUdoDocCon = new ManteUdoDocCon();

            //Obtener valores del data source
            for (int i = 0; i < dbdsMatriz.Size ; i++)
            {
                //Crear nuevo objeto cae
                cae = new CAE();

                //Obtener valores por linea
                numeroRegistro = dbdsMatriz.GetValue("DocEntry", i);

                cae.TipoCFE = CAE.ObtenerTipoCFECFC(dbdsMatriz.GetValue("U_TipoDoc", i));
                cae.NombreDocumento = dbdsMatriz.GetValue("U_NombDoc", i);
                cae.IndicadorConservar = dbdsMatriz.GetValue("U_IndCon", i);

                //Actualizar la información del registro recorrido
                manteUdoDocCon.Actualizar(cae, numeroRegistro);
            }
        }

        #endregion MANTENIMIENTO
    }
}
