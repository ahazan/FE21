using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SEICRY_FE_UYU_9.Udos;
using SEICRY_FE_UYU_9.Conexion;
using SEICRY_FE_UYU_9.Objetos;
using SAPbobsCOM;

namespace SEICRY_FE_UYU_9.Interfaz
{
    class FrmUsuarios
    {

        /// <summary>
        /// Agrega un checkbox al formulario de Usuarios
        /// </summary>
        /// <param name="formulario"></param>
        public void AgregarCheckBoxClienteContado(Form formulario)
        {
            //Se agregan items nuevo
            formulario.Items.Add("cbxSNConti", BoFormItemTypes.it_CHECK_BOX);
          
            //Se obtienen items de referencia
            Item referenciaTextBox = formulario.Items.Item("1320000001");

            //Se obtiene el item creado
            Item cbxSNConti = formulario.Items.Item("cbxSNConti");
         
            //Se asignan propiedades del objeto
            cbxSNConti.Left = referenciaTextBox.Left;
            cbxSNConti.Top = referenciaTextBox.Top + referenciaTextBox.Height + 2;
            cbxSNConti.Width = 300;

            ((CheckBox)cbxSNConti.Specific).Caption = "Contingencia - Factura Electónica ";
       
         

            //-------------------------------------------


            //--------------------------------------------------

            //Se agregan items nuevo
            formulario.Items.Add("cbxSupUser", BoFormItemTypes.it_CHECK_BOX);



            //Se obtienen items de referencia
            Item referenciaSNConti = formulario.Items.Item("cbxSNConti");


           //Se obtiene el item creado
            Item cbxSuperU = formulario.Items.Item("cbxSupUser");

            //Se asignan propiedades del objeto
            cbxSuperU.Left = referenciaSNConti.Left;
            cbxSuperU.Top = referenciaSNConti.Top + referenciaSNConti.Height + 2;
            cbxSuperU.Width = 300;

            ((CheckBox)cbxSuperU.Specific).Caption = "Super Usuario - Factura Electónica ";

            //------------------------------------------------------------


            //Crear binding con base de datos
            AgregarDataSources(formulario);
            CheckBox cbxSNC = (CheckBox)cbxSNConti.Specific;
            EstablecerDataBinds(formulario, cbxSNC, "U_SNConti");

            CheckBox cbxSU = (CheckBox)cbxSuperU.Specific;
            EstablecerDataBinds(formulario, cbxSU, "U_SUperUser");
       

        }

            



        /// <summary>
        /// Agrega los dataSources al formulario
        /// </summary>
        /// <param name="formulario"></param>
        private void AgregarDataSources(Form formulario)
        {
            formulario.DataSources.DBDataSources.Add("OUSR");
        }


        private void EstablecerDataBinds(Form formulario, CheckBox cbxSnc, string Campo)
        {
            cbxSnc.DataBind.SetBound(true, "OUSR", Campo);
            
        }

    }
}
