<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:output method="html" indent="yes" />
  <xsl:param name="Mode"  select="default"/>
  <xsl:param name="StyleSheetHref"/>
  <xsl:param name="captionIndex" />

  <!--html描画時とJs描画時とで改行コードの有無を分けます。-->
  <xsl:template name="WriteLR">
    <xsl:if test="$Mode!='Movie'">
      <xsl:text disable-output-escaping="yes">&#10;</xsl:text>
    </xsl:if>
  </xsl:template>

  <!-- アスキーアート用テンプレート -->
  <xsl:template name="WriteAsciiArt">
    <xsl:for-each select="/AsciiArt/line">
      <xsl:value-of select="." disable-output-escaping="yes" />
      <xsl:element name="br"/>

      <!--<xsl:text disable-output-escaping="yes">&#10;</xsl:text>-->
      <xsl:call-template name="WriteLR" />
    </xsl:for-each>
  </xsl:template>

  <!-- カラー文字アート用テンプレート -->
  <xsl:template name="WriteColorCharsArt">
    <!--AsciiArt要素を行単位に分ける-->
    <xsl:for-each select="/AsciiArt/line">
      <!--各行からchar要素をとりだしfont要素として出力する-->
      <xsl:for-each select="./char">
        <!--font要素を作る-->
        <xsl:element name="font">
          <xsl:attribute name="color">
            <xsl:value-of select="./@color"/>
          </xsl:attribute>

          <!--本体を記述-->
          <xsl:value-of select="."/>
        </xsl:element>
      </xsl:for-each>

      <!--br と改行コード-->
      <xsl:element name="br" />
      <xsl:call-template name="WriteLR"/>
    </xsl:for-each>
  </xsl:template>

  <!-- アスキーアート動画用テンプレート -->
  <xsl:template name="WriteCaptionJs">
    <xsl:text>captions[</xsl:text>
    <xsl:value-of select="$captionIndex" />
    <xsl:text>]="</xsl:text>
    <xsl:call-template name="WriteAsciiArt" />";
  </xsl:template>

  <!-- アスキーアート動画再生側のスクリプト参照-->
  <xsl:template match="import-files">
    <xsl:for-each select="./file">
      <!--script要素で書き出す-->
      <xsl:element name="script">
        <xsl:attribute name="type">text/JavaScript</xsl:attribute>
        <!--Captions.js(スクリプト)参照先-->
        <xsl:attribute name="src">
          <xsl:value-of select="." />
        </xsl:attribute>
      </xsl:element>

      <!--改行コード-->
      <xsl:call-template name="WriteLR" />
    </xsl:for-each>
  </xsl:template>

  <!-- エントリーポイント -->
  <xsl:template match="/">
    <xsl:choose>
      <!-- Mode="Movie"以外はhtmlとして出力します -->
      <xsl:when test="$Mode!='Movie'">
        <html>
          <head>
            <xsl:element name="link">
              <xsl:attribute name="rel">stylesheet</xsl:attribute>
              <xsl:attribute name="type">text/css</xsl:attribute>

              <!--スタイルシートの参照先を入れます-->
              <xsl:attribute name="href">
                <xsl:value-of select="$StyleSheetHref"/>
              </xsl:attribute>
            </xsl:element>

            <!--改行コード-->
            <xsl:call-template name="WriteLR" />
            <!--Playerモード出力の場合はフレーム参照と制御用スクリプトへの参照を出力します-->
            <xsl:apply-templates select="/import-files"/>
          </head>
          <xsl:element name="body">
            <!--Mode="MoviePlayer"の場合はonLoadイベントに指定のメソッドを登録-->
            <xsl:if test="$Mode='MoviePlayer'">
              <xsl:attribute name="onLoad">page_Loaded()</xsl:attribute>
            </xsl:if>

            <!-- div id=AsciiArt -->
            <xsl:element name="div">
              <!--div要素のid属性-->
              <xsl:attribute name="id">AsciiArt</xsl:attribute>

              <!-- テンプレートでAsciiArtエレメント内の文字列を取り出します -->
              <xsl:choose>
                <!--AsciiArtモードでの出力時-->
                <xsl:when test="$Mode='AsciiArt'">
                  <xsl:call-template name="WriteAsciiArt" />
                </xsl:when>
                <!--HtmlArtモードでの出力時-->
                <xsl:when test="$Mode='HtmlArt'">
                  <xsl:call-template name="WriteColorCharsArt" />
                </xsl:when>
                <xsl:otherwise>
                  
                </xsl:otherwise>
              </xsl:choose>
            </xsl:element>
          </xsl:element>
        </html>
      </xsl:when>

      <!-- Mode = "Movie"の場合はhtmlではなくJsファイルを出力します -->
      <xsl:when test="$Mode='Movie'">
        <xsl:call-template name="WriteCaptionJs" />
      </xsl:when>
    </xsl:choose>

  </xsl:template>
</xsl:stylesheet>