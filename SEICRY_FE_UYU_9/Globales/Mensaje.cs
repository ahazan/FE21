using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEICRY_FE_UYU_9.Globales
{
    class Mensaje
    {
        #region ERRORES

        /// <summary>
        /// Error : 
        /// </summary>
        public static string err = "Error : ";

        /// <summary>
        /// Error: No se cargo el archivo Xml
        /// </summary>
        public static string errCargaArchivoXml = "Error: No se cargo el archivo Xml";
        /// <summary>
        /// Error: El archivo no corresponde a un certificado y/o clave de certificado no válida
        /// </summary>
        public static string errCertificadoErroneo = "Error: El archivo no corresponde a un certificado y/o clave de certificado no válida";
        /// <summary>
        /// Certificado Digital no Configurado: No se recomienda el uso de Facturación Electrónica 
        /// </summary>
        public static string errCertificadoNoConfig = "Certificado Digital no Configurado: No se recomienda el uso de Facturación Electrónica ";
        /// <summary>
        /// Debe Configurar la cuenta de correo electrónico para el socio de negocio
        /// </summary>
        public static string errConfCuentaCorreo = "Debe Configurar la cuenta de correo electrónico para el socio de negocio";
        /// <summary>
        /// Error: Configuración erronea de datos del CAE
        /// </summary>
        public static string errConfErroneaCae = "Error: Configuración erronea de datos del CAE";
        /// <summary>
        /// Error: Configuración erronea de Correos Electrónicos
        /// </summary>
        public static string errConfErroneaCorreo = "Error: Configuración erronea de Correos Electrónicos";

        /// <summary>
        /// E07
        /// </summary>
        public static string errCompValFechaCAECod = "E07";
        /// <summary>
        /// Fecha Firma de CFE no se corresponde con fecha CAE
        /// </summary>
        public static string errCompValFechaCAEDetGlo = "Fecha Firma de CFE no se corresponde con fecha CAE";
        /// <summary>
        /// E04
        /// </summary>
        public static string errCompValFirmaCod = "E04";
        /// <summary>
        /// Firma electrónica no es válida
        /// </summary>
        public static string errCompValFirmaDetGlo = "Firma electrónica no es válida";
        /// <summary>
        /// E03
        /// </summary>
        public static string errCompValNumCAECFECod = "E03";
        /// <summary>
        /// Tipo y Número de CFE no se corresponden con el CAE
        /// </summary>
        public static string errCompValNumCAECFEDetGlo = "Tipo y Número de CFE no se corresponden con el CAE";
        /// <summary>
        /// E02
        /// </summary>
        public static string errCompValNumCFECod = "E02";
        /// <summary>
        /// Tipo y Número de CFE ya existen los registros
        /// </summary>
        public static string errCompValNumCFEDetGlo = "Tipo y Número de CFE ya existen los registros";
        /// <summary>
        /// E05
        /// </summary>
        public static string errCompValXsdCod = "E05";
        /// <summary>
        /// No cumple validaciones (*) de Formato Comprobantes
        /// </summary>
        public static string errCompValXsdDet = "No cumple validaciones (*) de Formato Comprobantes";

        /// <summary>
        /// No se ha configurado datos de envío de correo
        /// </summary>
        public static string errCorreoDatosConfigurados = "No se ha configurado datos de envío de correo";
        /// <summary>
        /// Fallo en la conexion a Internet o fallo de Autenticación.
        /// </summary>
        public static string errCorreoFalloConexion = "Fallo en la conexión a Internet o fallo de Autenticación.";
        /// <summary>
        /// El mensaje es nulo.
        /// </summary>
        public static string errCorreoMensajeNulo = "El mensaje es nulo.";
        /// <summary>
        /// El correo no pudo ser entregado
        /// </summary>
        public static string errCorreoNoEntrega = "El correo no pudo ser entregado";

        /// <summary>
        /// Error: Datos no configurados. Debe configurar la ruta y clave del certificado digital
        /// </summary>
        public static string errDatosNoConfigurados = "Error: Datos no configurados. Debe configurar la ruta y clave del certificado digital";
        /// <summary>
        /// Los datos de referencia no son válidos.
        /// </summary>
        public static string errDatosNoValidos = "Los datos de referencia no son válidos.";
        /// <summary>
        /// Debe indicar el valor para el tipo de documento receptor seleccionado.
        /// </summary>
        public static string errDocReceptorSeleccionado = "Debe indicar el valor para el tipo de documento receptor seleccionado.";
        /// <summary>
        /// Tipo de documento receptor no válido para el socio de negocios. Válido para Argentina, Brasil, Chile o Paraguay.
        /// </summary>
        public static string errDocReceptorNoValArBrChPa = "Tipo de documento receptor no válido para el socio de negocios. Válido para Argentina, Brasil, Chile o Paraguay.";
        /// <summary>
        /// Tipo de documento receptor no válido para el socio de negocios. Válido únicamente para Uruguay.
        /// </summary>
        public static string errDocReceptorNoValUr = "Tipo de documento receptor no válido para el socio de negocios. Válido únicamente para Uruguay.";

        /// <summary>
        /// Error: Fallo al actualizar datos de Correo
        /// </summary>
        public static string errFalloActualizaDatos = "Error: Fallo al actualizar datos de Correo";
        /// <summary>
        /// Error: Fallo al generar archivo PDF
        /// </summary>
        public static string errFalloGenerarPdf = "Error: Fallo al generar archivo PDF";
        /// <summary>
        /// Error: Fallo al imprimr archivo PDF
        /// </summary>
        public static string errFalloImprimirPdf = "Error: Fallo al imprimir archivo PDF";
        /// <summary>
        /// Error: Fallo al guardar ruta de logo.
        /// </summary>
        public static string errFalloGuardaRutaLogo = "Error: Fallo al guardar ruta de logo.";
        /// <summary>
        /// Error: Fallo al guardar ruta de Adobe Reader
        /// </summary>
        public static string errFalloGuardaRutaAdobe = "Error: Fallo al guardar ruta de Adobe Reader";
        /// <summary>
        /// Error: Fallo al firmar el documento PDF
        /// </summary>
        public static string errFalloFirmaPdf = "Error: Fallo al firmar el documento PDF";
        /// <summary>
        /// Error: Fallo al insertar datos de Correo
        /// </summary>
        public static string errFalloIngresaDatos = "Error: Fallo al insertar datos de Correo";
        /// <summary>
        /// Formato Incorrecto - \"Caja\". Tamaño máximo permitido es de 40 caracteres.
        /// </summary>
        public static string errFormatoCaja = "Formato Incorrecto - \"Caja\". Tamaño máximo permitido es de 40 caracteres.";
        /// <summary>
        /// Formato Incorrecto - \"Clave\" - Tamaño máximo es de 254 caracteres
        /// </summary>
        public static string errFormatoClave = "Formato Incorrecto - \"Clave\" - Tamaño máximo es de 254 caracteres";
        /// <summary>
        /// El formato del correo debe ser correo@gmail.com
        /// </summary>
        public static string errFormatoCorreo = "El formato del correo debe ser correo@gmail.com";
        /// <summary>
        /// Formato Incorrecto - \"Fecha de Emisión\"
        /// </summary>
        public static string errFormatoFechaEmision = "Formato Incorrecto - \"Fecha de Emisión\"";
        /// <summary>
        /// Formato Incorrecto - \"Fecha de Vencimiento\"
        /// </summary>
        public static string errFormatoFechaVencimiento = "Formato Incorrecto - \"Fecha de Vencimiento\"";
        /// <summary>
        /// Formato del campo \"Nombre Comercial\" incorrecto. El tamaño máximo permitido es de 30 caracteres.
        /// </summary>
        public static string errFormatoNombreComercial = "Formato del campo \"Nombre Comercial\" incorrecto. El tamaño máximo permitido es de 30 caracteres.";
        /// <summary>
        /// Formato del campo \"Nombre del Emisor\" incorrecto. El tamaño máximo permitido es de 150 caracteres.
        /// </summary>
        public static string errFormatoNombreEmisor = "Formato del campo \"Nombre del Emisor\" incorrecto. El tamaño máximo permitido es de 150 caracteres.";
        /// <summary>
        /// Formato Incorrecto - \"Número de Autorización\"
        /// </summary>
        public static string errFormatoNumAutorizacion = "Formato Incorrecto - \"Número de Autorización\"";
        /// <summary>
        /// Formato Incorrecto - \"Número Final\"
        /// </summary>
        public static string errFormatoNumFinal = "Formato Incorrecto - \"Número Final\"";
        /// <summary>
        /// Formato Incorrecto - \"Número Inicial\"
        /// </summary>
        public static string errFormatoNumInicial = "Formato Incorrecto - \"Número Inicial\"";
        /// <summary>
        /// Formato del campo \"Número de Resolución\" incorrecto. El tamaño máximo permitido es de 30 caracteres.
        /// </summary>
        public static string errFormatoNumResolucion = "Formato del campo \"Número de Resolución\" incorrecto. El tamaño máximo permitido es de 30 caracteres.";
        /// <summary>
        /// Fallo al realizar la operacion
        /// </summary>
        public static string errFalloOperacion = "Fallo al realizar la operacion";
        /// <summary>
        /// Formato del campo \"Dígito Verificador\" incorrecto. El tamaño máximo permitido es de 12 caracteres. El valor debe ser numérico.
        /// </summary>
        public static string errFormatoRuc = "Formato del campo \"Dígito Verificador\" incorrecto. El tamaño máximo permitido es de 12 caracteres. El valor debe ser numérico.";
        /// <summary>
        /// Formato Incorrecto - \"Serie\"
        /// </summary>
        public static string errFormatoSerie = "Formato Incorrecto - \"Serie\"";
        /// <summary>
        /// Formato Incorrecto - \"Url del Repositorio\" - Tamaño máximo es de 254 caracteres
        /// </summary>
        public static string errFormatoUrlRepositorio = "Formato Incorrecto - \"Url del Repositorio\" - Tamaño máximo es de 254 caracteres";
        /// <summary>
        /// Formato Incorrecto - \"Usuario\" - Tamaño máximo es de 254 caracteres
        /// </summary>
        public static string errFormatoUsuario = "Formato Incorrecto - \"Usuario\" - Tamaño máximo es de 254 caracteres";
        /// <summary>
        /// Formato Incorrecto - \"Tipo de Autorización\"
        /// </summary>
        public static string errFormatoTipoAutorizacion = "Formato Incorrecto - \"Tipo de Autorización\"";

        /// <summary>
        /// Ha ocurrido un error: 
        /// </summary>
        public static string errGenerico = "Ha ocurrido un error: ";

        /// <summary>
        /// Ningún Rango Seleccionado
        /// </summary>
        public static string errNingunRangoSelecccionado = "Ningún Rango Seleccionado";
        /// <summary>
        /// Error: No se pudo obtener datos del certificado
        /// </summary>
        public static string errNoDatosCertificados = "Error: No se pudo obtener datos del certificado";
        /// <summary>
        /// Error: No se pudo obtener datos del Comprobante
        /// </summary>
        public static string errNoDatosComprobante = "Error: No se pudo obtener datos del Comprobante";
        /// <summary>
        /// Error: No se pudo obtener datos del sobre.
        /// </summary>
        public static string errNoDatosSobres = "Error: No se pudo obtener datos del sobre.";
        /// <summary>
        /// Error: No se pudo descargar archivo: 
        /// </summary>
        public static string errNoDescargaArchivos = "Error: No se pudo descargar archivo: ";
        /// <summary>
        /// Error: No se pudo descargar comprobante: 
        /// </summary>
        public static string errNoDescargaComprobantes = "Error: No se pudo descargar comprobante: ";
        /// <summary>
        /// Error: No se pudo descargar el reporte: 
        /// </summary>
        public static string errNoDescargaReportes = "Error: No se pudo descargar el reporte: ";
        /// <summary>
        /// Error: No se pudo descargar el sobre:
        /// </summary>
        public static string errNoDescargaSobres = "Error: No se pudo descargar el sobre: ";
        /// <summary>
        /// Error: No se pudo descargar el archivo PDF:
        /// </summary>
        public static string errNoDescargaPdf = "Error: No se pudo descargar el archivo PDF: ";
        /// <summary>
        /// Error:No existe un CAE para este tipo de documento
        /// </summary>
        public static string errNoExisteCae = "Error:No existe un CAE para este tipo de documento";
        /// <summary>
        /// Error: No se pudo guardar datos del certificado
        /// </summary>
        public static string errNoGuardaDatosCertificado = "Error: No se pudo guardar datos del certificado";
        /// <summary>
        /// El campo \"Nombre Comercial\" es obligatorio.
        /// </summary>
        public static string errNombreComercialObligatorio = "El campo \"Nombre Comercial\" es obligatorio.";
        /// <summary>
        /// El campo \"Nombre del Emisor\" es obligatorio.
        /// </summary>
        public static string errNombreEmisorObligatorio = "El campo \"Nombre del Emisor\" es obligatorio.";
        /// <summary>
        /// Debe seleccionar un motivo de rechazo
        /// </summary>
        public static string errNoMotivosMostrar = "Debe seleccionar un motivo de rechazo";
        /// <summary>
        /// No se permite editar un motivo generado por el sistema
        /// </summary>
        public static string errNoMotivosSistea = "No se permite editar un motivo generado por el sistema";
        /// <summary>
        /// Error: No se pudo obtener el usuario de Sistema
        /// </summary>
        public static string errNoUsuarioSistema = "Error: No se pudo obtener el usuario de Sistema";
        /// <summary>
        /// Error: El número de rango ha sido utilizado en otro documento.
        /// </summary>
        public static string errNumRanUti = "Error: El número de rango ha sido utilizado en otro documento.";
        /// <summary>
        /// El campo \"Número de Resolución\" es obligatorio.
        /// </summary>
        public static string errNumResolucionObligatorio = "El campo \"Número de Resolución\" es obligatorio.";
        
        /// <summary>
        /// La Operación ha Fallado.
        /// </summary>
        public static string errOperacionFallida = "La Operación ha Fallado.";

        /// <summary>
        /// El rango seleccionado ya se ha utilizado y no se puede modificar.
        /// </summary>
        public static string errRangoUtilizadoModificar = "El rango seleccionado ya se ha utilizado y no se puede modificar.";
        /// <summary>
        /// El rango seleccionado ya se ha utilizado y no se puede eliminar.
        /// </summary>
        public static string errRangoUtilizadoEliminar = "El rango seleccionado ya se ha utilizado y no se puede eliminar.";
        /// <summary>
        /// Error: Ruta del logo incorrecta
        /// </summary>
        public static string errRutaLogoIncorrecta = "Error: Ruta del logo incorrecta";
        /// <summary>
        /// El campo \"Dígito Verificador\" es obligatorio.
        /// </summary>
        public static string errRucObligatorio = "El campo \"Dígito Verificador\" es obligatorio.";

        /// <summary>
        /// Debe seleccionar un tipo de documento receptor.
        /// </summary>
        public static string errSeleccionDocReceptor = "Debe seleccionar un tipo de documento receptor.";
        /// <summary>
        /// Debe seleccionar un logo
        /// </summary>
        public static string errSeleccionLogo = "Debe seleccionar un logo";
        /// <summary>
        /// Debe seleccionarl el ejecutable de Adobe Reader
        /// </summary>
        public static string errSeleccionAdobe = "Debe seleccionar el ejecutable de Adobe Reader";
        /// <summary>
        /// Debe seleccionar un medio para el envio de correo
        /// </summary>
        public static string errSeleccionMedioCorreo = "Debe seleccionar un medio para el envio de correo";
        /// <summary>
        /// S02
        /// </summary>
        public static string errSobValRucCod = "S02";
        /// <summary>
        /// 
        /// </summary>
        public static string errSobValRucDetGlo = "No coincide RUC de sobre, Certificado, envío o CFE";
        /// <summary>
        /// S05
        /// </summary>
        public static string errSobValCantCfeCod = "S05";
        /// <summary>
        /// No coincide cantidad CFE de carátula y contenido
        /// </summary>
        public static string errSobValCantCfeDetGlo = "No coincide cantidad CFE de carátula y contenido";
        /// <summary>
        /// S01
        /// </summary>
        public static string errSobValXsdCod = "S01";
        /// <summary>
        /// Formato del archivo no es el indicado
        /// </summary>
        public static string errSobValXsdDet = "Formato del archivo no es el indicado";

        /// <summary>
        /// No hay registros de Configuración.
        /// </summary>
        public static string errTablaConfigFtp = "No hay registros de Configuración.";
        /// <summary>
        /// . Verificar Conexión a Internet o la Configuración del servidor FTP.
        /// </summary>
        public static string errVerificarConexionFTP = ". Verificar Conexión a Internet o la Configuración del servidor FTP.";
        /// <summary>
        /// No se ha configurado CAE de contigencia
        /// </summary>
        public static string errValCAEsContingencia = "No se ha configurado CAE de contigencia";
        /// <summary>
        /// Valores Incompletos - \"Clave\"
        /// </summary>
        public static string errValIncClave = "Valores Incompletos - \"Clave\"";
        /// <summary>
        /// Valores Incompletos - \"Fecha de Emisión\"
        /// </summary>
        public static string errValIncFechaEmision = "Valores Incompletos - \"Fecha de Emisión\"";
        /// <summary>
        /// Valores Incompletos - \"Fecha de Vencimiento\"
        /// </summary>
        public static string errValIncFechaVencimiento = "Valores Incompletos - \"Fecha de Vencimiento\"";
        /// <summary>
        /// Valores Incompletos - \"Número de Autorización\"
        /// </summary>
        public static string errValIncNumAutorizacion = "Valores Incompletos - \"Número de Autorización\"";
        /// <summary>
        /// Valores Incompletos - \"Número Final\"
        /// </summary>
        public static string errValIncNumFinal = "Valores Incompletos - \"Número Final\"";
        /// <summary>
        /// Valores Incompletos - \"Número Inicial\"
        /// </summary>
        public static string errValIncNumInicial = "Valores Incompletos - \"Número Inicial\"";
        /// <summary>
        /// Debe ingresar todos los datos del rechazo
        /// </summary>
        public static string errValIncompleto = "Debe ingresar todos los datos del rechazo";
        /// <summary>
        /// Valores Incompletos - \"Serie\"
        /// </summary>
        public static string errValIncSerie = "Valores Incompletos - \"Serie\"";
        /// <summary>
        /// Valores Incompletos - \"Sucursal\"
        /// </summary>
        public static string errValIncSucursal = "Valores Incompletos - \"Sucursal\"";
        /// <summary>
        /// Valores Incompletos - \"Tipo de Autorización\"
        /// </summary>
        public static string errValIncTipoAutorizacion = "Valores Incompletos - \"Tipo de Autorización\"";
        /// <summary>
        /// Valores Incompletos - \"Tipo de Documento\"
        /// </summary>
        public static string errValIncTipoDoc = "Valores Incompletos - \"Tipo de Documento\"";
        /// <summary>
        /// Valores Incompletos - \"Url del Repositorio\"
        /// </summary>
        public static string errValIncUrlRepositorio = "Valores Incompletos - \"Url del Repositorio\"";
        /// <summary>
        /// Valores Incompletos - \"Usuario\"
        /// </summary>
        public static string errValIncUsuario = "Valores Incompletos - \"Usuario\"";
        /// <summary>
        /// Error: El número debe ser mayor: 
        /// </summary>
        public static string errValNumAct = "Error: El número debe ser mayor: ";
        /// <summary>
        /// Error: El número ingresado excede el límite del rango
        /// </summary>
        public static string errValNumFin = "Error: El número ingresado excede el límite del rango";
        /// <summary>
        /// Error: Serie y/o rango inválido
        /// </summary>
        public static string errValRangoSerie = "Error: Serie y/o rango inválido";
        /// <summary>
        /// Error al visualizar Certificado
        /// </summary>
        public static string errVisualizarCertificado = "Error al visualizar Certificado";
        /// <summary>
        /// Error al visualizar detalle de Comprobante
        /// </summary>
        public static string errVisualizarDetComprobante = "Error al visualizar detalle de Comprobante";

        #region FE_EXPORTACION

        /// <summary>
        ///  cliente extranjero no puede ser CF.
        /// </summary>
        public static string errorDocExpoCF = "El cliente Extranjero no puede ser Consumidor Final.";


        /// <summary>
        ///  cliente extranjero Tipo documento error.
        /// </summary>
        public static string errorTipoDocExpo = "El tipo de documento es incorrecto para el Cliente Extranjero.";

        /// <summary>
        /// FE Expo: Modalidad de Venta obligatorio.
        /// </summary>
        public static string errorModVenta = "Error: Falta indicar Modalidad de Venta (Logística)";

        /// <summary>
        /// FE Expo: Cláusula de Venta obligatorio.
        /// </summary>
        public static string errorClaVenta = "Error: Falta indicar Cláusula de Venta (Logística)";

        /// <summary>
        /// FE Expo: Vía de Transporte obligatorio.
        /// </summary>
        public static string errorViaTransporte = "Error: Falta indicar Vía de Transporte (Logística)";

        /// <summary>
        /// FE Expo: Ind. Tipo de Bienes obligatorio para eRemito.
        /// </summary>
        public static string errorIndTipoBienes = "Error: Falta completar Indicador Tipo de Bienes (Logística)";

        public static string errorLargoClaVenta = "Error: Cláusula de Venta no puede superar los 3 caracteres (Logística)";

        #endregion FE_EXPORTACION

        #endregion FIN ERRORES

        #region EXPRESIONES REGULARES

        /// <summary>
        /// @"^[0-9]{3}[A-Za-z]{0,2}[0-9]*.pdf$"
        /// </summary>
        public static string expRegCfePdf = @"^[0-9]{3}[A-Za-z]{0,2}[0-9]*.pdf$";
        /// <summary>
        /// @"^[0-9]{3}[A-Za-z]{1,2}[0-9]*.xml$"
        /// </summary>
        public static string expRegCfePdfXml = @"^[0-9]{3}[A-Za-z]{1,2}[0-9]*.xml$";
        /// <summary>
        /// @"^[0-9]*.xml$"
        /// </summary>
        public static string expRegCfeXml = @"^[0-9]*.xml$";
        /// <summary>
        /// @"^[a-zA-z0-9.]*@gmail.com$"
        /// </summary>
        public static string expRegCorreo = @"^[a-zA-z0-9.]*@gmail.com$";
        /// <summary>
        /// @"^[0-9]{11}$"
        /// </summary>
        public static string expRegNumeroAutorizacion = @"^[0-9]{11}$";
        /// <summary>
        /// @"^[0-9]{1,7}$"
        /// </summary>
        public static string expRegNumeroFinal = @"^[0-9]{1,7}$";
        /// <summary>
        /// @"^0?[0-9]{1,7}$"
        /// </summary>
        public static string expRegNumeroInicial = @"^0?[0-9]{1,7}$";
        /// <summary>
        /// @"^Rep_[0-9]*_[0-9]*_[0-9]*.xml$"
        /// </summary>
        public static string expRegReportesDiarios = @"^Rep_[0-9]*_[0-9]*_[0-9]*.xml$";
        /// <summary>
        /// @"^[a-zA-Z]{1,2}$"
        /// </summary>
        public static string expRegSerie = @"^[a-zA-Z]{1,2}$";
        /// <summary>
        /// @"^[0-9]{3}[A-Za-z]{0,2}[0-9]*.xml$"
        /// </summary>
        public static string expRegSobre = @"^[0-9]{3}[A-Za-z]{0,2}[0-9]*.xml$";
        /// <summary>
        /// @"^SOB_[0-9]*_[0-9]{8}_[0-9]*.xml$"
        /// </summary>
        public static string expRegSobreNombre = @"^SOB_[0-9]*_[0-9]{8}_[0-9]*.xml$";
        /// <summary>
        /// @"^Sob_[0-9]*_[0-9]{8}_[0-9]*.xml$"
        /// </summary>
        public static string expRegSobreNombreMinu = @"^Sob_[0-9]*_[0-9]{8}_[0-9]*.xml$";
        /// <summary>
        /// @"^M_[0-9]*_Sob_[0-9]*_[0-9]{8}_[0-9]*.xml$"
        /// </summary>
        public static string expRegACKSobreNombre = @"^M_[0-9]*_Sob_[0-9]*_[0-9]{8}_[0-9]*.xml$";
        /// <summary>
        /// @"^M_[0-9]*_SOB_[0-9]*_[0-9]{8}_[0-9]*.xml$"
        /// </summary>
        public static string expRegACKSobreNombreMay = @"^M_[0-9]*_SOB_[0-9]*_[0-9]{8}_[0-9]*.xml$";
        /// <summary>
        /// @"^ME_[0-9]*_Sob_[0-9]*_[0-9]{8}_[0-9]*.xml$"
        /// </summary>
        public static string expRegACKCFENombre = @"^ME_[0-9]*_Sob_[0-9]*_[0-9]{8}_[0-9]*.xml$";
        /// <summary>
        /// @"^ME_[0-9]*_SOB_[0-9]*_[0-9]{8}_[0-9]*.xml$"
        /// </summary>
        public static string expRegACKCFENombreMay = @"^ME_[0-9]*_SOB_[0-9]*_[0-9]{8}_[0-9]*.xml$";
        /// <summary>
        /// @"^[0-9]+[A-Za-z]+[0-9]+sf.pdf$"
        /// </summary>
        public static string expRegPdfNoFirmado = @"^[0-9]+[A-Za-z]+[0-9]+sf.pdf$";
        /// <summary>
        /// @"^[0-9]+[A-Za-z]+[0-9]+sf.xml$"
        /// </summary>
        public static string expRegXmlNoFirmado = @"^[0-9]+[A-Za-z]+[0-9]+sf.xml$";
        /// <summary>
        /// @"^[0-9]+[A-Za-z]+[0-9]+sf.xml$"
        /// </summary>
        public static string expRegSobreNoFirmado = @"^[0-9]+[A-Za-z]+[0-9]+nf.xml$";

        #endregion FIN EXPRESIONES REGULARES

        #region GENERAL

        /// <summary>
        /// Acuse de Comprobante
        /// </summary>
        public static string cACKAsunto = "Acuse de Comprobante";
        /// <summary>
        /// Adjunto Acuse de Recibo para Comprobante Fiscal
        /// </summary>
        public static string cACKMensaje = "Adjunto Acuse de Recibo para Comprobante Fiscal";
        /// <summary>
        /// imap.gmail.com
        /// </summary>
        public static string cliGmail = "imap.gmail.com";

        /// <summary>
        /// ftp://
        /// </summary>
        public static string dirFtp = "ftp://";

        /// <summary>
        ///  Todos los archivos (*.*) | *.*
        /// </summary>
        public static string filGeneral = " Todos los archivos (*.*) | *.*";
        /// <summary>
        /// Todos los archivos imágenes (*.bmp, *.jpg, *.jpeg, *.png, *.tif) | *.bmp; *.jpg; *.jpeg; *.png; *.tif
        /// </summary>
        public static string filImagenes = "Todos los archivos imágenes (*.bmp, *.jpg, *.jpeg, *.png, *.tif) | *.bmp; *.jpg; *.jpeg; *.png; *.tif";
        /// <summary>
        /// *.pdf
        /// </summary>
        public static string filPdf = "*.pdf";
        /// <summary>
        /// *.exe
        /// </summary>
        public static string filEjecutable = "Todos los archivos .exe | *.exe";
        /// <summary>
        /// *.xml
        /// </summary>
        public static string filXml = "*.xml";

        /// <summary>
        /// CAE para E-Factura Contingencia: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeFacExpFechaEmision = "CAE para E-Factura Exportación: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Factura Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeFacExpFechaVence = "CAE para E-Factura Exportación: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Factura-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeFacturaExpNoConfig = "CAE para E-Factura-Exportación: No configurado \n";
        /// <summary>
        /// CAE para E-Factura: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeFacturaFechaEmision = "CAE para E-Factura: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Factura: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeFacturaFechaVence = "CAE para E-Factura: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Factura: No configurado \n
        /// </summary>
        public static string infCaeFacturaNoConfig = "CAE para E-Factura: No configurado \n";

        /// <summary>
        /// CAE para NC. E-Factura Contingencia: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNCFacExpFechaEmision = "CAE para NC. E-Factura Exportación: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para NC. E-Factura Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNCFacExpFechaVence = "CAE para NC. E-Factura Exportación: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para NC. E-Factura-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeNCFacturaExpNoConfig = "CAE para NC. E-Factura-Exportación: No configurado \n";
        /// <summary>
        /// CAE para NC. E-Factura: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNCFacturaFechaEmision = "CAE para NC. E-Factura: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para NC. E-Factura: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNCFacturaFechaVence = "CAE para NC. E-Factura: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para NC. E-Factura: No configurado \n
        /// </summary>
        public static string infCaeNCFacturaNoConfig = "CAE para NC. E-Factura: No configurado \n";
        /// <summary>
        /// CAE para NC. E-Ticket Contingencia: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNCTicContFechaEmision = "CAE para NC. E-Ticket Contingencia: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para NC. E-Ticket Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNCTicContFechaVence = "CAE para NC. E-Ticket Contingencia: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para NC. E-Ticket-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeNCTicContNoConfig = "CAE para NC. E-Ticket-Contingencia: No configurado \n";
        /// <summary>
        /// CAE para NC. E-Ticket: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNCTicketFechaEmision = "CAE para NC. E-Ticket: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para NC. E-Ticket: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNCTicketFechaVence = "CAE para NC. E-Ticket: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para NC. E-Ticket: No configurado \n
        /// </summary>
        public static string infCaeNCTicketNoConfig = "CAE para NC. E-Ticket: No configurado \n";

        /// <summary>
        /// CAE para ND. E-Factura Contingencia: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNDFacExpFechaEmision = "CAE para ND. E-Factura Exportación: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para ND. E-Factura Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNDFacExpFechaVence = "CAE para ND. E-Factura Exportación: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para ND. E-Factura-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeNDFacturaExpNoConfig = "CAE para ND. E-Factura-Exportación: No configurado \n";
        /// <summary>
        /// CAE para ND. E-Factura: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNDFacFechaEmision = "CAE para ND. E-Factura: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para ND. E-Factura: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNDFacFechaVence = "CAE para ND. E-Factura: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para ND. E-Factura: No configurado \n
        /// </summary>
        public static string infCaeNDFacturaNoConfig = "CAE para ND. E-Factura: No configurado \n";
        /// <summary>
        /// CAE para ND. E-Ticket Contingencia: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNDTicContFechaEmision = "CAE para ND. E-Ticket Contingencia: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para ND. E-Ticket Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNDTicContFechaVence = "CAE para ND. E-Ticket Contingencia: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para ND. E-Ticket-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeNDTicContNoConfig = "CAE para ND. E-Ticket-Contingencia: No configurado \n";
        /// <summary>
        /// CAE para ND. E-Ticket: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeNDTicketFechaEmision = "CAE para ND. E-Ticket: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para ND. E-Ticket: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeNDTicketFechaVence = "CAE para ND. E-Ticket: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para ND. E-Ticket: No configurado \n
        /// </summary>
        public static string infCaeNDTicketNoConfig = "CAE para ND. E-Ticket: No configurado \n";

        /// <summary>
        /// No se ha configurado los CAEs para los siguientes tipos de documento(s): \n e-Factura, e-Factura Contingencia, e-Ticket y e-Ticket Contingencia. \n No se recomienda el uso de la Facturación Electrónica
        /// </summary>
        public static string infCaeNoConfig = "No se ha configurado los CAEs para los siguientes tipos de documento(s): \n e-Factura, e-Factura Contingencia, e-Ticket y e-Ticket Contingencia. \n No se recomienda el uso de la Facturación Electrónica";

        /// <summary>
        /// CAE para E-Remito Contingencia: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeRemExpFechaEmision = "CAE para E-Remito Exportación: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Remito Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeRemExpFechaVence = "CAE para E-Remito Exportación: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Remito-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeRemitoExpNoConfig = "CAE para E-Remito-Exportación: No configurado \n";
        /// <summary>
        /// CAE para E-Resguardo Contingencia: Fecha de Emisión no aplicable \n
        /// </summary>
        public static string infCaeResContFechaEmision = "CAE para E-Resguardo Contingencia: Fecha de Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Resguardo Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeResContFechaVence = "CAE para E-Resguardo Contingencia: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Resguardo Contingencia: No configurado \n
        /// </summary>
        public static string infCaeResContNoConfig = "CAE para E-Resguardo Contingencia: No configurado \n";
        /// <summary>
        /// CAE para E-Remito: Fecha Emisión no aplicable \n
        /// </summary>
        public static string infCaeRemitoFechaEmision = "CAE para E-Remito: Fecha Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Remito: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeRemitoFechaVence = "CAE para E-Remito: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Remito: No configurado \n
        /// </summary>
        public static string infCaeRemitoNoConfig = "CAE para E-Remito: No configurado \n";
        /// <summary>
        /// CAE para E-Resguardo: Fecha de Emisión no aplicable \n
        /// </summary>
        public static string infCaeResguardoFechaEmision = "CAE para E-Resguardo: Fecha de Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Resguardo: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeResguardoFechaVence = "CAE para E-Resguardo: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Resguardo: No configurado \n
        /// </summary>
        public static string infCaeResguardoNoConfig = "CAE para E-Resguardo: No configurado \n";

        /// <summary>
        /// CAE para E-Ticket Contingencia: Fecha de Emisión no aplicable \n
        /// </summary>
        public static string infCaeTicContFechaEmision = "CAE para E-Ticket Contingencia: Fecha de Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Ticket Contingencia: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeTicContFechaVence = "CAE para E-Ticket Contingencia: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Ticket-Contingencia: No configurado \n
        /// </summary>
        public static string infCaeTicContNoConfig = "CAE para E-Ticket-Contingencia: No configurado \n";
        /// <summary>
        /// CAE para E-Ticket: Fecha de Emisión no aplicable \n
        /// </summary>
        public static string infCaeTicketFechaEmision = "CAE para E-Ticket: Fecha de Emisión no aplicable \n";
        /// <summary>
        /// CAE para E-Ticket: Fecha de Vencimiento cumplida \n
        /// </summary>
        public static string infCaeTicketFechaVence = "CAE para E-Ticket: Fecha de Vencimiento cumplida \n";
        /// <summary>
        /// CAE para E-Ticket: No configurado \n
        /// </summary>
        public static string infCaeTicketNoConfig = "CAE para E-Ticket: No configurado \n";

        /// <summary>
        /// Falta información de CAE(s): \n
        /// </summary>
        public static string infFaltaInfoCae = "Falta información de CAE(s): \n";

        /// <summary>
        /// No se recomienda el uso de la Facturación Electrónica
        /// </summary>
        public static string infNoUsoFE = "No se recomienda el uso de la Facturación Electrónica";

        /// <summary>
        /// qrcode.png
        /// </summary>
        public static string nomImagenQr = "qrcode.png";

        /// <summary>
        /// ¿Realmente desea eliminar el rango seleccionado?
        /// </summary>
        public static string preConfirmaEliminarRango = "¿Realmente desea eliminar el rango seleccionado?";
        /// <summary>
        /// ¿Realmente desea eliminar el motivo de rechazo?
        /// </summary>
        public static string preConfirmaEliminarMotivoRechazo = "¿Realmente desea eliminar el motivo de rechazo?";

        #endregion FIN GENERAL

        #region EXITO

        /// <summary>
        /// Datos actualizados con éxito
        /// </summary>
        public static string sucDatosActualizados = "Datos actualizados con éxito";
        /// <summary>
        /// Datos Guardados Exitosamente
        /// </summary>
        public static string sucDatosGuardados = "Datos Guardados Exitosamente";
        /// <summary>
        /// Datos insertados con éxito
        /// </summary>
        public static string sucDatosInsertados = "Datos insertados con éxito";
        /// <summary>
        /// Factura creada
        /// </summary>
        public static string sucFactura = "Factura creada";
        /// <summary>
        /// Factura creada y correo enviado con éxito
        /// </summary>
        public static string sucFacturaCorreo = "Factura creada y correo enviado con éxito";
        /// <summary>
        /// Operación Finalizada con Éxito.
        /// </summary>
        public static string sucOperacionExitosa = "Operación Finalizada con Éxito.";

        #endregion FIN EXITO

        #region ALERTAS

        /// <summary>
        /// Debe ingresar datos en ambos campos de texto
        /// </summary>
        public static string warIngresaDatosAmbos = "Debe ingresar datos en ambos campos de texto";
        /// <summary>
        /// No hay certificados para mostrar
        /// </summary>
        public static string warNoCertificadoMostrar = "No hay certificados para mostrar";

        /// <summary>
        /// No existen comprobantes para visualizar.
        /// </summary>
        public static string warNoComprobantesVisualizar = "No existen comprobantes para visualizar.";
        /// <summary>
        /// No se ha configurado la ubicación de la firma digital. No se recomiendo utiizar la facturación electrónica.
        /// </summary>
        public static string warNoConfigFirmaDigital = "No se ha configurado la ubicación de la firma digital. No se recomiendo utiizar la facturación electrónica.";
        /// <summary>
        /// No existen reportes para visualizar.
        /// </summary>
        public static string warNoReportesVisualizar = "No existen reportes para visualizar.";
        /// <summary>
        /// No hay sobres para mostrar
        /// </summary>
        public static string warNoSobresMostrar = "No hay sobres para mostrar";
        /// <summary>
        /// No existen sobres para visualizar.
        /// </summary>
        public static string warNoSobresVisualizar = "No existen sobres para visualizar.";
        /// <summary>
        /// No existen archivos PDF para visualizar
        /// </summary>
        public static string warNoPdfVisualizar = "No existen archivos PDF para visualizar";
        /// <summary>
        /// Debe rellenar los campos de correo y clave para actualizar
        /// </summary>
        public static string warRellenarCampoCorreo = "Debe rellenar los campos de correo y clave para actualizar";
        /// <summary>
        /// Debe seleccionar un archivo para visualizar.
        /// </summary>
        public static string warSeleccionArchivoVisualizar = "Debe seleccionar un archivo para visualizar.";
        /// <summary>
        /// Debe seleccionar un certificado para visualizar.
        /// </summary>
        public static string warSeleccionCertificadoVisualizar = "Debe seleccionar un certificado para visualizar.";
        /// <summary>
        /// Debe seleccionar un comprobante para visualizar.
        /// </summary>
        public static string warSeleccionComprobanteVisualizar = "Debe seleccionar un comprobante para visualizar.";
        /// <summary>
        /// Debe seleccionar un reporte para visualizar.
        /// </summary>
        public static string warSeleccionReporteVisualizar = "Debe seleccionar un reporte para visualizar.";
        /// <summary>
        /// Debe seleccionar un sobre para visualizar.
        /// </summary>
        public static string warSeleccionSobreVisualizar = "Debe seleccionar un sobre para visualizar.";
        /// <summary>
        /// Debe seleccionar un archivo PDF para visualizar
        /// </summary>
        public static string warSeleccionPdfVisualizar = "Debe seleccionar un archivo PDF para visualizar";

        #endregion FIN ALERTAS

        #region PDF FACTURAS

        /// <summary>
        /// ADENDA
        /// </summary>
        public static string pdfADENDA = "ADENDA";
        /// <summary>
        /// Asunto
        /// </summary>
        public static string pdfAsunto = "Asunto";
        /// <summary>
        /// Cantidad
        /// </summary>
        public static string pdfCantidad = "Cantidad";
        /// <summary>
        /// CodSeg:
        /// </summary>
        public static string pdfCodSeg = "Cod:";
        /// <summary>
        /// Código Seguridad
        /// </summary>
        public static string pdfCodigoSeguridad = "Código Seguridad";
        /// <summary>
        /// Contenido
        /// </summary>
        public static string pdfContenido = "Envío automático Sistema de Facturación Electrónica"; //"Contenido";
        /// <summary>
        /// Detalle Mercaderia
        /// </summary>
        public static string pdfDetalleMercaderia = "Detalle Mercaderia";
        /// <summary>
        /// https://www.efactura.dgi.gub.uy/consultaQR/cfe
        /// </summary>
        public static string pdfDirCodigoQrPrd = "https://www.efactura.dgi.gub.uy/consultaQR/cfe";
        /// <summary>
        /// https://www.efactura.dgi.gub.uy/consultaQRPrueba/cfe
        /// </summary>
        public static string pdfDirCodigoQrQas = "https://www.efactura.dgi.gub.uy/consultaQR/cfe";
        /// <summary>
        /// Exportacion y Asimiladas
        /// </summary>
        public static string pdfExportacionAsimiladas = "Exportación y Asimiladas";
        /// <summary>
        /// .pdf
        /// </summary>
        public static string pdfExt = ".pdf";
        /// <summary>
        /// Fecha de Vencimiento: 
        /// </summary>
        public static string pdfFechaVencimiento = "Fecha de Vencimiento  ";
        /// <summary>
        /// Impuesto Percibido
        /// </summary>
        public static string pdfImpuestoPercibido = "Impuesto Percibido";
        /// <summary>
        /// IVA en Suspenso
        /// </summary>
        public static string pdfIVASuspenso = "IVA en Suspenso";
        /// <summary>
        /// IVA - Tasa Básica
        /// </summary>
        public static string pdfIvaTasBas = "IVA - Tasa Básica";
        /// <summary>
        /// IVA - Tasa Mínima\
        /// </summary>
        public static string pdfIvaTasMin = "IVA - Tasa Mínima";
        /// <summary>
        /// IVA - Otra Tasa
        /// </summary>
        public static string pdfIvaTasOtr = "IVA - Otra Tasa";
        /// <summary>
        ///  Puede verificar comprobante en www.efactura.dgi.uy
        /// </summary>
        public static string pdfLinkDGI = "Puede verificar comprobante en www.efactura.dgi.gub.uy";
        /// <summary>
        /// Monto
        /// </summary>
        public static string pdfMonto = "Monto";
        /// <summary>
        /// Monto Neto - IVA Tasa Básica
        /// </summary>
        public static string pdfMontoNetoIvaTasBas = "Monto Neto - IVA Tasa Básica";
        /// <summary>
        /// Monto Neto - IVA Tasa Mínima
        /// </summary>
        public static string pdfMontoNetoIvaTasMin = "Monto Neto - IVA Tasa Mínima";
        /// <summary>
        /// Monto Neto - IVA Otra Tasa
        /// </summary>
        public static string pdfMontoNetoIvaTasOtr = "Monto Neto - IVA Otra Tasa";
        /// <summary>
        /// Total Monto
        /// </summary>
        public static string pdfMontoTotal = "Total Monto";
        /// <summary>
        /// No Facturable
        /// </summary>
        public static string pdfNoFacturable = "No Facturable";
        /// <summary>
        /// No gravado
        /// </summary>
        public static string pdfNoGravado = "No gravado";
        /// <summary>
        /// Precio Final
        /// </summary>
        public static string pdfPrecioFinal = "Importe";
        /// <summary>
        /// Precio Unitario
        /// </summary>
        public static string pdfPrecioUnitario = "Precio";
        /// <summary>
        /// Retenido/Percibido
        /// </summary>
        public static string pdfRetenidoPercibido = "Retenido/Percibido";
        /// <summary>
        /// Res N°
        /// </summary>
        public static string pdfRes = "Res N°";
        /// <summary>
        /// RUC COMPRADOR
        /// </summary>
        public static string pdfRUCCOMPRADOR = "RUT Comprador:";
        /// <summary>
        /// CONSUMO FINAL
        /// </summary>
        public static string pdfCONSUMOFINAL = "Consumo Final";
        /// <summary>
        /// smtp.gmail.com
        /// </summary>
        public static string pdfServidorGmail = "smtp.gmail.com";
        /// <summary>
        /// Tasa Basica IVA
        /// </summary>
        public static string pdfTasaBasIva = "Tasa Básica IVA";
        /// <summary>
        /// Tasa Minima IVA
        /// </summary>
        public static string pdfTasaMinIva = "Tasa Mínima IVA";
        /// <summary>
        /// Total
        /// </summary>
        public static string pdfTotal = "Total";
        /// <summary>
        /// Total a pagar
        /// </summary>
        public static string pdfTotalPagar = "Total a pagar";
        /// <summary>
        /// IVA al día
        /// </summary>
        public static string pdfIVADia = "IVA al día";
        /// <summary>
        /// N° de CAE 
        /// </summary>
        public static string pdfNumCAE = "N° de CAE ";
        /// <summary>
        /// Rango Autorizado
        /// </summary>
        public static string pdfRanAut = "Rango Autorizado ";
        /// <summary>
        /// Precio S/Desc
        /// </summary>
        public static string pdfPrecioAnDesc = "Precio c/Desc";

        #endregion PDF FACTURAS

        #region ACK

        /// <summary>
        /// Consulta de Estado CFE
        /// </summary>
        public static string ACKAsunto = "Consulta de Estado CFE";

        #endregion
    }
}
