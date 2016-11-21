using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Globales
{
    class Constantes
    {
        #region TABLAS
        public static string TablaFactura = "OINV";
        public static string TablaNC      = "ORIN";
        public static string TablaND      = "OINV";
        public static string TablaRemito  = "ODLN";
        #endregion TABLAS

        #region CAMPOS DE USUARIO

        #region UDFGenerales
        public static string UDFIndTipoDeBienes = "U_IndTipoBienes";
        #endregion UDFGenerales

        #region UDFFactura
        public static string UDFViaTransporteFA = "U_ViaTransFA";        
        #endregion UDFFactura

        #region UDFNC
        public static string UDFViaTransporteNC = "U_ViaTransNC";
        #endregion UDFNC

        #region UDFND
        public static string UDFViaTransporteND = "U_ViaTransND";
        #endregion UDFND

        #region UDFRemito
        public static string UDFViaTransporteRM = "U_ViaTransRM";
        #endregion UDFRemito

        #endregion CAMPOS DE USUARIO

        #region PDF
        public static int TamanoLetraEmisor     = 10;
        public static int TamanoLetraReceptor   = 10;
        #endregion PDF

    }
}
