// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SceneryShader"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.05
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Lines("Lines", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (0.1445177,0.4641323,0.7279412,1)
		_PannerTexture("Panner Texture", 2D) = "white" {}
		_BGTexture("BG Texture", 2D) = "white" {}
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
			v.vertex.xyz *= ( 1 + _ASEOutlineWidth);
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform sampler2D _BGTexture;
		uniform sampler2D _PannerTexture;
		uniform float4 _PannerTexture_ST;
		uniform float4 _BaseColor;
		uniform sampler2D _Lines;
		uniform float4 _Lines_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_PannerTexture = v.texcoord * _PannerTexture_ST.xy + _PannerTexture_ST.zw;
			float2 panner5 = ( _Time.y * float2( 0.15,0.15 ) + tex2Dlod( _PannerTexture, float4( uv_PannerTexture, 0, 0.0) ).rg);
			float4 temp_output_12_0 = ( tex2Dlod( _BGTexture, float4( panner5, 0, 0.0) ) * _BaseColor );
			float4 temp_output_15_0 = ( 1.0 - temp_output_12_0 );
			v.normal = temp_output_15_0.rgb;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_PannerTexture = i.uv_texcoord * _PannerTexture_ST.xy + _PannerTexture_ST.zw;
			float2 panner5 = ( _Time.y * float2( 0.15,0.15 ) + tex2D( _PannerTexture, uv_PannerTexture ).rg);
			float4 temp_output_12_0 = ( tex2D( _BGTexture, panner5 ) * _BaseColor );
			float4 temp_output_58_0 = saturate( temp_output_12_0 );
			o.Albedo = temp_output_58_0.rgb;
			float2 uv_Lines = i.uv_texcoord * _Lines_ST.xy + _Lines_ST.zw;
			float clampResult86 = clamp( abs( _SinTime.w ) , 0.2 , 0.8 );
			float4 temp_output_37_0 = ( temp_output_12_0 * ( tex2D( _Lines, uv_Lines ) + clampResult86 ) );
			o.Emission = temp_output_37_0.rgb;
			float4 temp_output_15_0 = ( 1.0 - temp_output_12_0 );
			o.Smoothness = temp_output_15_0.r;
			o.Occlusion = 0.0;
			o.Transmission = temp_output_58_0.rgb;
			o.Alpha = 1;
			clip( ( 1.0 - temp_output_37_0 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
86;226;1200;751;988.408;338.7517;1.425;True;False
Node;AmplifyShaderEditor.SamplerNode;62;-1783.085,-430.2298;Float;True;Property;_PannerTexture;Panner Texture;3;0;Create;True;0;0;False;0;e088873534c03f8469cd9c682359463b;05aa43dbbeddb24419e5b5b6e734d595;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;83;-1291.9,592.1251;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;14;-1785.966,-79.72496;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;9;-1781.134,-226.5299;Float;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0.15,0.15;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1015.64,-576.0195;Float;True;Property;_BGTexture;BG Texture;4;0;Create;True;0;0;False;0;e088873534c03f8469cd9c682359463b;e088873534c03f8469cd9c682359463b;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.AbsOpNode;76;-1135,578.8251;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;5;-1423.619,-263.71;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;63;-1121.95,115.1351;Float;True;Property;_Lines;Lines;1;0;Create;True;0;0;False;0;43043ad53b32d26459522088fff7b4bc;3616d5268629b894582bc4d44e714c36;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;13;-728.5,-12;Float;False;Property;_BaseColor;BaseColor;2;0;Create;True;0;0;False;0;0.1445177,0.4641323,0.7279412,1;0.1445177,0.4641323,0.7279412,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;86;-985.1249,573.1251;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-729.5,-359;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;61b82206f72041544881f34b586e783d;e088873534c03f8469cd9c682359463b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-847.2251,409.4249;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-361.5,-76;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-438.385,189.0049;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;15;-124.5,-22;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;58;-162.7,-83.67499;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;233.824,366.3154;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;90;-58.05102,243.2404;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;475.675,-156.35;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SceneryShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.05;0,0,0,0;VertexScale;False;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;76;0;83;4
WireConnection;5;0;62;0
WireConnection;5;2;9;0
WireConnection;5;1;14;0
WireConnection;86;0;76;0
WireConnection;10;0;89;0
WireConnection;10;1;5;0
WireConnection;71;0;63;0
WireConnection;71;1;86;0
WireConnection;12;0;10;0
WireConnection;12;1;13;0
WireConnection;37;0;12;0
WireConnection;37;1;71;0
WireConnection;15;0;12;0
WireConnection;58;0;12;0
WireConnection;90;0;37;0
WireConnection;0;0;58;0
WireConnection;0;2;37;0
WireConnection;0;4;15;0
WireConnection;0;5;92;0
WireConnection;0;6;58;0
WireConnection;0;10;90;0
WireConnection;0;12;15;0
ASEEND*/
//CHKSM=B892CE6B40887855DC011907945BD911F21BEC80