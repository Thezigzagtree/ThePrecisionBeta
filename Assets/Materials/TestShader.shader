// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PathShader"
{
	Properties
	{
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_Color1("Color 1", Color) = (0.2989295,0.336074,0.7132353,0)
		_Color0("Color 0", Color) = (0.6650087,0.7558168,0.9044118,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform sampler2D _TextureSample4;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner49 = ( _Time.y * float2( 1,0 ) + i.uv_texcoord);
			float2 panner38 = ( 2.0 * _Time.y * float2( 0.1,0.1 ) + panner49);
			float clampResult50 = clamp( ( _CosTime.w * 1.0 ) , 0.5 , 1.0 );
			float4 temp_output_28_0 = ( tex2D( _TextureSample4, panner38 ) * abs( clampResult50 ) );
			float4 lerpResult18 = lerp( _Color1 , _Color0 , temp_output_28_0);
			o.Albedo = lerpResult18.rgb;
			o.Emission = temp_output_28_0.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
433;43;1200;751;2494.037;-13.30117;2.125643;True;False
Node;AmplifyShaderEditor.Vector2Node;48;-1938.904,719.1042;Float;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CosTime;27;-1495.985,573.3253;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1968.545,459.7911;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;46;-1995.848,923.874;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1340.969,632.0199;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;49;-1611.667,774.2531;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;50;-994.474,644.8581;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;38;-1661.535,217.8438;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.1;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;29;-1482.44,240.7197;Float;True;Property;_TextureSample4;Texture Sample 4;1;0;Create;True;0;0;False;0;None;5798ded558355430c8a9b13ee12a847c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;45;-1164.524,530.0734;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-1064.62,-337.1891;Float;False;Property;_Color1;Color 1;2;0;Create;True;0;0;False;0;0.2989295,0.336074,0.7132353,0;0.2989295,0.336074,0.7132353,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-1075.249,-95.50427;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.6650087,0.7558168,0.9044118,0;0.6650087,0.7558168,0.9044118,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-886.4591,463.46;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;-656.5005,-140.7245;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PathShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.3;1,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;27;4
WireConnection;49;0;47;0
WireConnection;49;2;48;0
WireConnection;49;1;46;0
WireConnection;50;0;31;0
WireConnection;38;0;49;0
WireConnection;29;1;38;0
WireConnection;45;0;50;0
WireConnection;28;0;29;0
WireConnection;28;1;45;0
WireConnection;18;0;44;0
WireConnection;18;1;43;0
WireConnection;18;2;28;0
WireConnection;0;0;18;0
WireConnection;0;2;28;0
ASEEND*/
//CHKSM=657BE4CF3C5D5D07CFB266CCB8D1529A3381D35C