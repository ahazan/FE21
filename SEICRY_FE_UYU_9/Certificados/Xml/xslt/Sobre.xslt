<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="Sobre">
     
      <DGICFE:EnvioCFE version="1.0" xsi:schemaLocation="http://cfe.dgi.gub.uy EnvioCFE_v1.24.xsd" xmlns:DGICFE="http://cfe.dgi.gub.uy" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
      <!--<DGICFE:EnvioCFE version="1.0" xmlns:DGICFE="http://cfe.dgi.gub.uy" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">-->
        <DGICFE:Caratula version="1.0">
          
          <DGICFE:RutReceptor>
            <xsl:value-of select="RucReceptor"/>
          </DGICFE:RutReceptor>
          <DGICFE:RUCEmisor>
            <xsl:value-of select="RUCEmisor"/>
          </DGICFE:RUCEmisor>
          <DGICFE:Idemisor>
            <xsl:value-of select="Idemisor"/>
          </DGICFE:Idemisor>
          <DGICFE:CantCFE>
            <xsl:value-of select="CantCFE"/>
          </DGICFE:CantCFE>
          <DGICFE:Fecha>
            <xsl:value-of select="FechaCreacionSobre"/>
          </DGICFE:Fecha>
          <DGICFE:X509Certificate>
            <xsl:value-of select="X509Certificate"/>
          </DGICFE:X509Certificate>
         
        </DGICFE:Caratula> 
          
      </DGICFE:EnvioCFE>
      <!--<EnvioCFE xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" version="1.0" xmlns="http://cfe.dgi.gub.uy">
        <Caratula version="1.0">

          <RutReceptor>
            <xsl:value-of select="RucReceptor"/>
          </RutReceptor>
          <RUCEmisor>
            <xsl:value-of select="RUCEmisor"/>
          </RUCEmisor>
          <Idemisor>
            <xsl:value-of select="Idemisor"/>
          </Idemisor>
          <CantCFE>
            <xsl:value-of select="CantCFE"/>
          </CantCFE>
          <Fecha>
            <xsl:value-of select="FechaCreacionSobre"/>
          </Fecha>
          <X509Certificate>
            <xsl:value-of select="X509Certificate"/>
          </X509Certificate>

        </Caratula>

      </EnvioCFE>-->
      
    </xsl:template>
</xsl:stylesheet>
