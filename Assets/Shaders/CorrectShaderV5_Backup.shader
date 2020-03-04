// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CorrectShaderV5"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (1,1,1,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.15
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_Teleport("Teleport", Range( -20 , 20)) = 2
		_Range("Range", Range( -20 , 20)) = 2.611275
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_DetailAlbedo("Detail Albedo", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
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
			v.vertex.xyz *= ( 1 + _ASEOutlineWidth);
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
		#include "UnityPBSLighting.cginc"
		#pragma target 3.5
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
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
			half3 Translucency;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _DetailAlbedo;
		uniform float4 _DetailAlbedo_ST;
		uniform float _Teleport;
		uniform float _Range;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
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
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 color11 = IsGammaSpace() ? float4(0.9852941,0.8522894,0.1086721,0) : float4(0.9668716,0.6962805,0.01142176,0);
			float clampResult33 = clamp( abs( _CosTime.w ) , 0.5 , 1.0 );
			float2 uv_DetailAlbedo = i.uv_texcoord * _DetailAlbedo_ST.xy + _DetailAlbedo_ST.zw;
			float temp_output_9_0_g1 = 0.0;
			float temp_output_18_0_g1 = ( 1.0 - temp_output_9_0_g1 );
			float3 appendResult16_g1 = (float3(temp_output_18_0_g1 , temp_output_18_0_g1 , temp_output_18_0_g1));
			float4 blendOpSrc26 = tex2D( _TextureSample1, uv_TextureSample1 );
			float4 blendOpDest26 = float4( ( ( tex2D( _TextureSample2, uv_TextureSample2 ) * color11 * clampResult33 ).rgb * ( ( ( (tex2D( _DetailAlbedo, uv_DetailAlbedo )).rgb * (unity_ColorSpaceDouble).rgb ) * temp_output_9_0_g1 ) + appendResult16_g1 ) ) , 0.0 );
			float4 temp_output_26_0 = (( blendOpDest26 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest26 ) * ( 1.0 - blendOpSrc26 ) ) : ( 2.0 * blendOpDest26 * blendOpSrc26 ) );
			o.Albedo = temp_output_26_0.rgb;
			float2 panner68 = ( 10.0 * float2( 0,0 ) + temp_output_26_0.rg);
			o.Metallic = panner68.x;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform60 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float4 YGradient66 = saturate( ( ( transform60 + _Teleport ) / _Range ) );
			float4 blendOpSrc36 = temp_output_26_0;
			float4 blendOpDest36 = YGradient66;
			o.Smoothness = ( 0.5 - 2.0 * ( blendOpSrc36 - 0.5 ) * ( blendOpDest36 - 0.5 ) ).x;
			float clampResult55 = clamp( abs( _SinTime.w ) , 0.2 , 0.6 );
			float3 temp_cast_8 = (clampResult55).xxx;
			o.Translucency = temp_cast_8;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
387;242;955;615;2331.139;422.7828;2.115001;True;False
Node;AmplifyShaderEditor.CommentaryNode;58;-1881.444,1454.826;Float;False;1376.5;623.0248;YGradient;8;66;65;64;63;62;61;60;59;;0.7647059,0.5065982,0.0449827,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;59;-1807.468,1504.826;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosTime;31;-1404.302,235.6708;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;60;-1566.069,1507.726;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-1831.444,1704.351;Float;False;Property;_Teleport;Teleport;7;0;Create;True;0;0;False;0;2;1;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;32;-1208.551,224.8708;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1295.194,2001.351;Float;False;Property;_Range;Range;8;0;Create;True;0;0;False;0;2.611275;2.611275;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-1391.444,1613.601;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;11;-1539.551,-27.97956;Float;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;0.9852941,0.8522894,0.1086721,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;51;-1400.15,-484.7289;Float;True;Property;_TextureSample2;Texture Sample 2;9;0;Create;True;0;0;False;0;18b687e2045d542489ad43a7957c7805;18b687e2045d542489ad43a7957c7805;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;64;-1014.694,1763.476;Float;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;33;-1033.052,150.6208;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1059.102,-122.3795;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;65;-754.8187,1800.601;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;29;-1463.902,546.621;Float;True;Property;_TextureSample1;Texture Sample 1;14;0;Create;True;0;0;False;0;9a033f7462e2fcf41abd09cddbaaadaa;fdeab82779759b940bd521c5c6dbbb83;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;52;-410.5998,577.7209;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;15;-876.8526,-100.7798;Float;False;Detail Albedo;10;;1;29e5a290b15a7884983e27c8f1afaa8c;0;3;12;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-713.7834,1527.136;Float;True;YGradient;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BlendOpsNode;26;-578.1524,58.52056;Float;True;Overlay;False;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;54;-175.7005,581.0211;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-1300.278,872.7643;Float;False;66;YGradient;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-717.5118,1010.795;Float;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;36;-904.7919,596.6155;Float;True;Exclusion;False;3;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;55;-5.60043,558.0711;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;68;-390.2867,811.1957;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-4.053116E-06,67.49998;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;CorrectShaderV5;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.15;1,1,1,0;VertexScale;True;False;Cylindrical;False;Relative;0;;-1;0;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;60;0;59;0
WireConnection;32;0;31;4
WireConnection;62;0;60;0
WireConnection;62;1;61;0
WireConnection;64;0;62;0
WireConnection;64;1;63;0
WireConnection;33;0;32;0
WireConnection;18;0;51;0
WireConnection;18;1;11;0
WireConnection;18;2;33;0
WireConnection;65;0;64;0
WireConnection;15;12;18;0
WireConnection;66;0;65;0
WireConnection;26;0;29;0
WireConnection;26;1;15;0
WireConnection;54;0;52;4
WireConnection;36;0;26;0
WireConnection;36;1;67;0
WireConnection;55;0;54;0
WireConnection;68;0;26;0
WireConnection;68;1;69;0
WireConnection;0;0;26;0
WireConnection;0;3;68;0
WireConnection;0;4;36;0
WireConnection;0;7;55;0
ASEEND*/
//CHKSM=1189CF8830DB4255819CFC7493193C307D119F51