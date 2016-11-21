<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="RPTD">

      <ns1:Reporte xmlns:ns1="http://cfe.dgi.gub.uy">

        <ns1:Caratula>

          <xsl:attribute name="version">
            <xsl:value-of select="Version"/>
          </xsl:attribute>

          <ns1:RUCEmisor>
            <xsl:value-of select="RucEmisor"/>
          </ns1:RUCEmisor>

          <ns1:FechaResumen>
            <xsl:value-of select="FechaResumen"/>
          </ns1:FechaResumen>

          <ns1:SecEnvio>
            <xsl:value-of select="SecuenciaEnvio"/>
          </ns1:SecEnvio>

          <ns1:TmstFirmaEnv>
            <xsl:value-of select="FechaHoraFirma"/>
          </ns1:TmstFirmaEnv>

          <ns1:CantComprobantes>
            <xsl:value-of select="CantComprobantes"/>
          </ns1:CantComprobantes>
          
        </ns1:Caratula>

        <ns1:Rsmn_Tck>

          <ns1:TipoComp>101</ns1:TipoComp>
          



          <xsl:for-each select ="asdf/asdfa">

            <xsl:choose>
              <xsl:when test="tipo = eTck">mixed</xsl:when>
            </xsl:choose>
            
          </xsl:for-each>
          
        </ns1:Rsmn_Tck>
        
      </ns1:Reporte>

    </xsl:template>
</xsl:stylesheet>
