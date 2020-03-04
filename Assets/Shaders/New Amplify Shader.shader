// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WorldMapShader"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
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
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 hsvTorgb21 = HSVToRGB( float3(( _Time.y / 100.0 ),4.0,1.0) );
			float mulTime7 = _Time.y * 0.05;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 panner5 = ( mulTime7 * float2( 2,1 ) + tex2D( _TextureSample1, uv_TextureSample1 ).rg);
			float clampResult41 = clamp( abs( _SinTime.w ) , 0.0 , 0.5 );
			float3 desaturateInitialColor38 = ( float4( hsvTorgb21 , 0.0 ) * tex2D( _MainTex, panner5 ) ).rgb;
			float desaturateDot38 = dot( desaturateInitialColor38, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar38 = lerp( desaturateInitialColor38, desaturateDot38.xxx, clampResult41 );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float clampResult33 = clamp( abs( _CosTime.w ) , 0.0 , 0.6 );
			o.Metallic = ( float4( desaturateVar38 , 0.0 ) / ( tex2D( _TextureSample0, uv_TextureSample0 ) / clampResult33 ) ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
300;486;1200;751;1851.706;318.6523;2.01;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;37;-1130.624,-507.8348;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1642.97,-86.05999;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;70bc209cca802d2439a916060556badb;94397b93335fd474bb994fc976122851;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;46;-1551.738,246.9717;Float;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;2,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;7;-1549.285,422.7047;Float;False;1;0;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;47;-876.0477,-488.1032;Float;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-868.348,-68.92814;Float;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;42;-690.6733,251.3803;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;11;-1262.405,-267.915;Float;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;15c23374f1eb6034ca9856e25db3c30a;e088873534c03f8469cd9c682359463b;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;5;-1273.5,175.505;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1002.408,-372.6783;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosTime;31;-860.2697,655.5551;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-979.9005,139.0499;Float;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;False;0;ca76d28e85748384cbba4adde1fd0683;ca76d28e85748384cbba4adde1fd0683;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;21;-822.2294,-308.125;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;32;-581.3098,750.6551;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;40;-530.5883,335.3852;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;33;-349.8998,666.6501;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-677.5402,43.94996;Float;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;29;-606.6697,422.5602;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;7fc2e7c692b39a4428ba357b13306a1d;e088873534c03f8469cd9c682359463b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;41;-435.4883,130.9202;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;30;-215.1747,546.1902;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;38;-394.2784,-40.25975;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;35;-272.2346,50.08535;Float;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-294.4244,241.8703;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;WorldMapShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;47;0;37;0
WireConnection;5;0;2;0
WireConnection;5;2;46;0
WireConnection;5;1;7;0
WireConnection;4;0;11;0
WireConnection;4;1;5;0
WireConnection;21;0;47;0
WireConnection;21;1;48;0
WireConnection;21;2;45;0
WireConnection;32;0;31;4
WireConnection;40;0;42;4
WireConnection;33;0;32;0
WireConnection;8;0;21;0
WireConnection;8;1;4;0
WireConnection;41;0;40;0
WireConnection;30;0;29;0
WireConnection;30;1;33;0
WireConnection;38;0;8;0
WireConnection;38;1;41;0
WireConnection;35;0;38;0
WireConnection;35;1;30;0
WireConnection;0;3;35;0
ASEEND*/
//CHKSM=42238E94793C85FBE4337610057FCD899ED3F0ED