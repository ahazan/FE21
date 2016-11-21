using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Objetos
{
    class EmpresaDatos
    {

            #region CONSTRUCTOR

        public EmpresaDatos()
        {
            this.Direccion = "";
            this.Phone = "";
            this.E_Mail = "";
            this.Web = "";
            this.Nombre = "";
            this.NombreComercial = "";
            this.Ciudad = "";
        }

        #endregion


        #region PROPIEDADES

        private string eMail;

        public string E_Mail
        {
            get { return eMail; }
            set { eMail = value; }
        }
            
       
        private string phone;

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        private string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }


        private string web;

        public string Web
        {
            get { return web; }
            set { web = value; }
        }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }


        private string nombreComercial;

        public string NombreComercial
        {
            get { return nombreComercial; }
            set { nombreComercial = value; }
        }


        private string ciudad;

         public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }

        

       

        #endregion


    }
}
