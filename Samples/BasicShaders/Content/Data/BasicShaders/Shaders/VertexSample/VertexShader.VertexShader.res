﻿<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint />
  </assetInfo>
  <source dataType="String">uniform float _GameTime;
uniform float _CameraFocusDist;
uniform bool _CameraParallax;

uniform float FloatStrength;

void main()
{
	// Duality uses software pre-transformation of vertices
	// gl_Vertex is already in parallax (scaled) view space when arriving here.
	vec4 vertex = gl_Vertex;
	
	// Reverse-engineer the scale that was previously applied to the vertex
	float scale = 1.0;
	if (_CameraParallax)
	{
		scale = _CameraFocusDist / vertex.z;
	}
	
	// Move the vertex around, keeping scale in mind
	vertex.xy += FloatStrength * scale * vec2(
		sin(_GameTime + mod(gl_VertexID, 4)), 
		cos(_GameTime + mod(gl_VertexID, 4)));
	
	gl_Position = gl_ProjectionMatrix * gl_ModelViewMatrix * vertex;
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
