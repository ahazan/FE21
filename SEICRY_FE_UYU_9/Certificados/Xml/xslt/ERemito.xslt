<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

  <xsl:template match="CFE">

    <ns1:CFE xmlns:ns1="http://cfe.dgi.gub.uy">

      <xsl:attribute name="version">
        <xsl:value-of select="Version"/>
      </xsl:attribute>

      <ns1:eRem>
        
        <ns1:TmstFirma>
          <xsl:value-of select="FechaHoraFirma"/>
        </ns1:TmstFirma>
        
        <ns1:Encabezado>

          <ns1:IdDoc>

            <ns1:TipoCFE>
              <xsl:value-of select="TipoCFEInt"/>
            </ns1:TipoCFE>

            <ns1:Serie>
              <xsl:value-of select="SerieComprobante"/>
            </ns1:Serie>

            <ns1:Nro>
              <xsl:value-of select="NumeroComprobante"/>
            </ns1:Nro>

            <ns1:FchEmis>
              <xsl:value-of select="FechaComprobante"/>
            </ns1:FchEmis>

            <ns1:TipoTraslado>
              <xsl:value-of select="TipoTrasladoBienesInt"/>
            </ns1:TipoTraslado>

          </ns1:IdDoc>

          <ns1:Emisor>

            <ns1:RUCEmisor>
              <xsl:value-of select="RucEmisor"/>
            </ns1:RUCEmisor>

            <ns1:RznSoc>
              <xsl:value-of select="NombreEmisor"/>
            </ns1:RznSoc>

            <ns1:CdgDGISucur>
              <xsl:value-of select="CodigoCasaPrincipalEmisor"/>
            </ns1:CdgDGISucur>

            <ns1:DomFiscal>
              <xsl:value-of select="DomicilioFiscalEmisor"/>
            </ns1:DomFiscal>

            <ns1:Ciudad>
              <xsl:value-of select="CiuidadEmisor"/>
            </ns1:Ciudad>

            <ns1:Departamento>
              <xsl:value-of select="DepartamentoEmisor"/>
            </ns1:Departamento>

          </ns1:Emisor>

          <ns1:Receptor>

            <ns1:TipoDocRecep>
              <xsl:value-of select="TipoDocumentoReceptorInt"/>
            </ns1:TipoDocRecep>

            <ns1:CodPaisRecep>
              <xsl:value-of select="CodigoPaisReceptor"/>
            </ns1:CodPaisRecep>

            <ns1:DocRecep>
              <xsl:value-of select="NumDocReceptor"/>
            </ns1:DocRecep>

            <ns1:RznSocRecep>
              <xsl:value-of select="NombreReceptor"/>
            </ns1:RznSocRecep>

            <ns1:DirRecep>
              <xsl:value-of select="DireccionReceptor"/>
            </ns1:DirRecep>

            <ns1:CiudadRecep>
              <xsl:value-of select="CiuidadReceptor"/>
            </ns1:CiudadRecep>

            <ns1:DeptoRecep>
              <xsl:value-of select="DepartamentoReceptor"/>
            </ns1:DeptoRecep>

            <xsl:if test="NumeroCompraReceptor > 0">
              <ns1:CompraID>
                <xsl:value-of select="NumeroCompraReceptor"/>
              </ns1:CompraID>
            </xsl:if>

          </ns1:Receptor>

          <ns1:Totales>

            <ns1:CantLinDet>
              <xsl:value-of select="Lineas"/>
            </ns1:CantLinDet>

          </ns1:Totales>

        </ns1:Encabezado>

        <ns1:Detalle>

          <xsl:for-each select="Items/CFEItems">

            <ns1:Item>

              <ns1:NroLinDet>
                <xsl:value-of select="NumeroLinea"/>
              </ns1:NroLinDet>

              <ns1:NomItem>
                <xsl:value-of select="NombreItem"/>
              </ns1:NomItem>

              <ns1:Cantidad>
                <xsl:value-of select="CantidadItem"/>
              </ns1:Cantidad>

              <ns1:UniMed>
                <xsl:value-of select="UnidadMedida"/>
              </ns1:UniMed>

            </ns1:Item>

          </xsl:for-each>

        </ns1:Detalle>

        <ns1:CAEData>

          <ns1:CAE_ID>
            <xsl:value-of select="NumeroCAE"/>
          </ns1:CAE_ID>

          <ns1:DNro>
            <xsl:value-of select="NumeroInicialCAE"/>
          </ns1:DNro>

          <ns1:HNro>
            <xsl:value-of select="NumeroFinalCAE"/>
          </ns1:HNro>

          <ns1:FecVenc>
            <xsl:value-of select="FechaVencimientoCAE"/>
          </ns1:FecVenc>

        </ns1:CAEData>

      </ns1:eRem>

    </ns1:CFE>

  </xsl:template>

</xsl:stylesheet>
