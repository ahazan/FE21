using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Objetos;
using SEICRY_FE_UYU_9.Udos;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmRazonReferenciaNC : FrmBase
    {
        Matrix matriz = null;
        DBDataSource dataSourceMatriz = null;

        /// <summary>
        /// Agrega los dataSources
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
            CargarMatriz();
            AgregarNuevaLinea();
        }

        /// <summary>
        /// Establece el databind
        /// </summary>
        protected override void EstablecerDataBind()
        {            
        }

        /// <summary>
        /// Obtener los datos de la base de datos y cargar la matriz
        /// </summary>
        public void CargarMatriz()
        {
            matriz = ((Matrix)(Formulario.Items.Item("matRazRef")).Specific);

            //Limpiar la matriz
            matriz.Clear();

            //Realizar la consulta 
            dataSourceMatriz = Formulario.DataSources.DBDataSources.Item("@TFERZR");

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
        /// Almacena las razones de referencia
        /// </summary>
        /// <returns></returns>
        public bool Almacenar()
        {
            bool salida = false;

            RazonReferencia razonReferencia;
            List<RazonReferencia> listaRazones = new List<RazonReferencia>();

            //Valida que la matriz contenga información. Si no tiene se ingresa los datos como registros nuevos
            if (matriz.RowCount > 0)
            {
                //Carga el data source con los datos de la matriz
                matriz.FlushToDataSource();

                //Recorre el data source
                for (int i = 0; i < dataSourceMatriz.Size; i++)
                {
                    //Crea un nuevo objeto retencion percepcion
                    razonReferencia = new RazonReferencia();

                    //Establce las propiedades del objeto
                    razonReferencia.CodigoRazon = dataSourceMatriz.GetValue("U_Codigo", i);
                    razonReferencia.RazonReferenciaNC = dataSourceMatriz.GetValue("U_Razon", i).Trim();

                    //Agrega el objeto a la lista
                    listaRazones.Add(razonReferencia);
                }

                ManteUdoRazonReferencia manteRazRef = new ManteUdoRazonReferencia();
                manteRazRef.Eliminar();

                //Agrega los nuevos registros
                if (manteRazRef.Almacenar(listaRazones))
                {
                    CargarMatriz();
                    AgregarNuevaLinea();
                    salida = true;
                }
            }

            return salida;
        }
    }
}
