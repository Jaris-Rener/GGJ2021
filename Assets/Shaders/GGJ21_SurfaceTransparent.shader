// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GGJ21/GGJ21_SurfaceTransparent"
{
	Properties
	{
		[HDR]_Colour("Colour", Color) = (1,0,0,0)
		[Toggle]_PropFade("Prop Fade", Float) = 0
		_SurfaceDetail("Surface Detail", 2D) = "white" {}
		_WorldtoObjectSwitch("World to Object Switch", Range( 0 , 1)) = 0
		_Tile("Tile", Vector) = (1,1,0,0)
		_Alpha("Alpha", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 vertexColor : COLOR;
		};

		uniform float _PropFade;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_SurfaceDetail);
		SamplerState sampler_SurfaceDetail;
		uniform float2 _Tile;
		uniform float _WorldtoObjectSwitch;
		uniform float4 _Colour;
		uniform float _Alpha;


		inline float4 TriplanarSampling1( UNITY_DECLARE_TEX2D_NOSAMPLER(topTexMap), SamplerState samplertopTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = SAMPLE_TEXTURE2D( topTexMap, samplertopTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = SAMPLE_TEXTURE2D( topTexMap, samplertopTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = SAMPLE_TEXTURE2D( topTexMap, samplertopTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling13( UNITY_DECLARE_TEX2D_NOSAMPLER(topTexMap), SamplerState samplertopTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = SAMPLE_TEXTURE2D( topTexMap, samplertopTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = SAMPLE_TEXTURE2D( topTexMap, samplertopTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = SAMPLE_TEXTURE2D( topTexMap, samplertopTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar1 = TriplanarSampling1( _SurfaceDetail, sampler_SurfaceDetail, ase_worldPos, ase_worldNormal, 1.0, _Tile, 1.0, 0 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 triplanar13 = TriplanarSampling13( _SurfaceDetail, sampler_SurfaceDetail, ase_vertex3Pos, ase_vertexNormal, 1.0, _Tile, 1.0, 0 );
			float4 lerpResult17 = lerp( triplanar1 , triplanar13 , _WorldtoObjectSwitch);
			float4 TriPlanarNoise20 = lerpResult17;
			float4 temp_output_4_0 = ( TriPlanarNoise20 * _Colour * i.vertexColor );
			float4 color5 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float clampResult31 = clamp( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 1.0 ) , 3.0 ) , 0.0 , 1.0 );
			float DistanceMask32 = clampResult31;
			float4 lerpResult7 = lerp( temp_output_4_0 , color5 , DistanceMask32);
			o.Albedo = (( _PropFade )?( lerpResult7 ):( temp_output_4_0 )).rgb;
			float4 temp_cast_7 = (_Colour.a).xxxx;
			float4 lerpResult9 = lerp( temp_cast_7 , color5 , DistanceMask32);
			o.Emission = (( _PropFade )?( lerpResult9 ):( ( _Colour * _Colour.a ) )).rgb;
			float lerpResult37 = lerp( 0.0 , 1.0 , DistanceMask32);
			o.Metallic = (( _PropFade )?( lerpResult37 ):( 0.0 ));
			o.Smoothness = ( TriPlanarNoise20 * 0.0 ).x;
			o.Alpha = _Alpha;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
-1134;287;1279;614;1977.826;-113.3023;2.55416;True;True
Node;AmplifyShaderEditor.WorldSpaceCameraPos;24;-3409.797,245.0933;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;23;-3377.217,-65.66292;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;27;-3197.256,422.6671;Inherit;False;Constant;_ShadowTransitionDistance;Shadow Transition Distance;3;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;15;-2189.617,-532.4207;Inherit;False;Property;_Tile;Tile;4;0;Create;True;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DistanceOpNode;25;-3146.656,79.69089;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;16;-2283.589,-750.5283;Inherit;True;Property;_SurfaceDetail;Surface Detail;2;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;29;-2863.831,477.327;Inherit;False;Constant;_ShadowTransitionFalloff;Shadow Transition Falloff;3;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;13;-1954.806,-571.439;Inherit;True;Spherical;Object;False;Top Texture 1;_TopTexture1;white;0;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-1859.588,-380.5284;Inherit;False;Property;_WorldtoObjectSwitch;World to Object Switch;3;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-2902.09,138.4359;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;1;-1952.957,-756.5906;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;28;-2628.792,202.2057;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;17;-1543.588,-630.5283;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;31;-2444.77,194.9178;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-1384.588,-632.5283;Inherit;False;TriPlanarNoise;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-2280.791,191.2739;Inherit;False;DistanceMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-1482.438,64.32629;Inherit;False;Property;_Colour;Colour;0;1;[HDR];Create;True;0;0;False;0;False;1,0,0,0;0.6037736,0.4253623,0.3503026,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;3;-1447.085,249.1863;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1462.53,-39.91726;Inherit;False;20;TriPlanarNoise;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-891.8914,897.4532;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1188.963,98.28241;Inherit;False;3;3;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-1377.082,941.9366;Inherit;False;32;DistanceMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1472.692,656.6883;Inherit;False;Constant;_Shadowed;Shadowed;1;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-519.9877,665.0229;Inherit;False;Constant;_Metallic;Metallic;3;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-417.2009,1098.67;Inherit;False;Constant;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1006.763,491.4158;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;7;-995.3427,276.7142;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;37;-498.3696,874.8165;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-1051.984,692.3392;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-446.1791,998.2692;Inherit;False;20;TriPlanarNoise;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ToggleSwitchNode;35;-305.092,744.2239;Inherit;True;Property;_PropFade;Prop Fade;2;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;6;-736.7297,91.01363;Inherit;True;Property;_PropFade;Prop Fade;1;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-221.9954,1018.99;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ToggleSwitchNode;8;-747.6899,531.0427;Inherit;True;Property;_PropFade;Prop Fade;1;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-224.3343,1124.439;Inherit;False;Property;_Alpha;Alpha;5;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;41.53402,415.3399;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;GGJ21/GGJ21_SurfaceTransparent;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;30;-2816.459,-479.2207;Inherit;False;100;100;la;0;;1,1,1,1;0;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;13;0;16;0
WireConnection;13;3;15;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;1;0;16;0
WireConnection;1;3;15;0
WireConnection;28;0;26;0
WireConnection;28;1;29;0
WireConnection;17;0;1;0
WireConnection;17;1;13;0
WireConnection;17;2;19;0
WireConnection;31;0;28;0
WireConnection;20;0;17;0
WireConnection;32;0;31;0
WireConnection;4;0;21;0
WireConnection;4;1;2;0
WireConnection;4;2;3;0
WireConnection;34;0;2;0
WireConnection;34;1;2;4
WireConnection;7;0;4;0
WireConnection;7;1;5;0
WireConnection;7;2;33;0
WireConnection;37;0;10;0
WireConnection;37;1;36;0
WireConnection;37;2;33;0
WireConnection;9;0;2;4
WireConnection;9;1;5;0
WireConnection;9;2;33;0
WireConnection;35;0;10;0
WireConnection;35;1;37;0
WireConnection;6;0;4;0
WireConnection;6;1;7;0
WireConnection;12;0;22;0
WireConnection;12;1;11;0
WireConnection;8;0;34;0
WireConnection;8;1;9;0
WireConnection;0;0;6;0
WireConnection;0;2;8;0
WireConnection;0;3;35;0
WireConnection;0;4;12;0
WireConnection;0;9;38;0
ASEEND*/
//CHKSM=D572469C7D277DAFEBB06DFA99B2E7153CDEBECB