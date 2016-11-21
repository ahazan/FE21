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

      <ns1:eFact>

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

            <ns1:FmaPago>
              <xsl:value-of select="FormaPagoInt"/>
            </ns1:FmaPago>

            <ns1:FchVenc>
              <xsl:value-of select="FechaVencimiento"/>
            </ns1:FchVenc>

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

            <ns1:TpoMoneda>
              <xsl:value-of select="TipoModena"/>
            </ns1:TpoMoneda>

            <ns1:TpoCambio>
              <xsl:value-of select="TipoCambio"/>
            </ns1:TpoCambio>

            <ns1:MntNoGrv>
              <xsl:value-of select="TotalMontoNoGravado"/>
            </ns1:MntNoGrv>

            <ns1:MntNetoIvaTasaMin>
              <xsl:value-of select="TotalMontoNetoIVATasaMinima"/>  
            </ns1:MntNetoIvaTasaMin>

            <ns1:MntNetoIVATasaBasica>
              <xsl:value-of select="TotalMontoNetoIVATasaBasica"/>
            </ns1:MntNetoIVATasaBasica>

			      <ns1:IVATasaMin>
              <xsl:value-of select="TasaMinimaIVA"/>
            </ns1:IVATasaMin>

            <ns1:IVATasaBasica>
              <xsl:value-of select="TasaBasicaIVA"/>
            </ns1:IVATasaBasica>   

            <ns1:MntIVATasaMin>
              <xsl:value-of select="TotalIVATasaMinima"/>
            </ns1:MntIVATasaMin>         

            <ns1:MntIVATasaBasica>
              <xsl:value-of select="TotalIVATasaBasica"/>
            </ns1:MntIVATasaBasica>

            <ns1:MntTotal>
              <xsl:value-of select="TotalMontoTotal"/>
            </ns1:MntTotal>

            <ns1:CantLinDet>
              <xsl:value-of select="Lineas"/>
            </ns1:CantLinDet>

            <ns1:MontoNF>
              <xsl:value-of select=" MontoNoFacturable"/>
            </ns1:MontoNF>

            <ns1:MntPagar>
              <xsl:value-of select="MontoTotalPagar"/>
            </ns1:MntPagar>

          </ns1:Totales>

        </ns1:Encabezado>

        <ns1:Detalle>

          <xsl:for-each select="Items/CFEItems">

            <ns1:Item>

              <ns1:NroLinDet>
                <xsl:value-of select="NumeroLinea"/>
              </ns1:NroLinDet>

              <ns1:IndFact>
                <xsl:value-of select="IndicadorFacturacion"/>
              </ns1:IndFact>

              <ns1:NomItem>
                <xsl:value-of select="NombreItem"/>
              </ns1:NomItem>

              <ns1:Cantidad>
                <xsl:value-of select="CantidadItem"/>
              </ns1:Cantidad>

              <ns1:UniMed>
                <xsl:value-of select="UnidadMedida"/>
              </ns1:UniMed>

              <ns1:PrecioUnitario>
                <xsl:value-of select="PrecioUnitarioItem"/>
              </ns1:PrecioUnitario>

              <ns1:MontoItem>
                <xsl:value-of select="MontoItem"/>
              </ns1:MontoItem>

            </ns1:Item>

          </xsl:for-each>

        </ns1:Detalle>

        <xsl:for-each select="InfoReferencia/CFEInfoReferencia">

          <ns1:Referencia>
              <ns1:Referencia>
                  <ns1:NroLinRef>
                    <xsl:value-of select="NumeroLinea"/>
                  </ns1:NroLinRef>

                  <ns1:TpoDocRef>
                    <xsl:value-of select="TipoCFEReferencia"/>
                  </ns1:TpoDocRef>

                  <ns1:Serie>
                    <xsl:value-of select="SerieComprobanteReferencia"/>
                  </ns1:Serie>

                  <ns1:NroCFERef>
                    <xsl:value-of select="NumeroComprobanteReferencia"/>
                  </ns1:NroCFERef>
              </ns1:Referencia>
          </ns1:Referencia>

        </xsl:for-each>

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

      </ns1:eFact>

    </ns1:CFE>

  </xsl:template>
</xsl:stylesheet>
