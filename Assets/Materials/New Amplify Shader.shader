// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield Shader"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (1,0.5882353,0.5882353,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.05
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input {
			half filler;
		};
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner8 = ( 1.0 * _Time.y * float2( 0.5,0.2 ) + i.uv_texcoord);
			float4 color19 = IsGammaSpace() ? float4(0,1,0.5448275,0) : float4(0,1,0.2579036,0);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 color15 = IsGammaSpace() ? float4(0,0.5862069,1,0) : float4(0,0.3026842,1,0);
			o.Emission = ( ( tex2D( _Texture0, panner8 ) * color19 ) * tex2D( _TextureSample1, uv_TextureSample1 ) * color15 ).rgb;
			float temp_output_16_0 = 0.75;
			o.Alpha = temp_output_16_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
381;55;1200;751;860;-3.5;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1257,-148.5;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;14;-1154,9.5;Float;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0.5,0.2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;8;-909,9.5;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;17;-908,-326.5;Float;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;43043ad53b32d26459522088fff7b4bc;15275da4cb12b7e47a71a0d71b12c1ad;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;5;-709,-30.5;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;9a033f7462e2fcf41abd09cddbaaadaa;9a033f7462e2fcf41abd09cddbaaadaa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;-672,193.5;Float;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;False;0;0,1,0.5448275,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-756,-448.5;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;15275da4cb12b7e47a71a0d71b12c1ad;43043ad53b32d26459522088fff7b4bc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-424,146.5;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;15;-390,492.5;Float;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;0,0.5862069,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-217,78.5;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-184,343.5;Float;True;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.75;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-1,1;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shield Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.05;1,0.5882353,0.5882353,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;13;0
WireConnection;8;2;14;0
WireConnection;5;0;17;0
WireConnection;5;1;8;0
WireConnection;20;0;5;0
WireConnection;20;1;19;0
WireConnection;3;0;20;0
WireConnection;3;1;6;0
WireConnection;3;2;15;0
WireConnection;0;2;3;0
WireConnection;0;9;16;0
WireConnection;0;10;16;0
ASEEND*/
//CHKSM=6DE3C3CFD5929F3A1B0D943EFC122D10664903B0