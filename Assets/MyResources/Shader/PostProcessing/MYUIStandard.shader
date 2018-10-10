// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "MOYU/UIStandard"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _GlossMapScale("Smoothness Factor", Range(0.0, 1.0)) = 1.0

        _SpecColor("Specular", Color) = (0.2,0.2,0.2)
        _SpecGlossMap("Specular", 2D) = "black" {}

		
        [HideInInspector] _Mode ("__mode", Float) = 0.0
    }



    SubShader
    {
        Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

          

            CGPROGRAM
            #pragma target 3.0

			#define _SPECGLOSSMAP
			#define SHADOWS_SOFT

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            #pragma vertex vertBase
            #pragma fragment fragBase
			#include "UnityStandardCore.cginc"
			uniform sampler2D _AlphaTex;

			inline FragmentCommonData CFragmentSetup(inout float4 i_tex, float3 i_eyeVec, half3 i_viewDirForParallax, float4 tangentToWorld[3], float3 i_posWorld)
			{
				i_tex = Parallax(i_tex, i_viewDirForParallax);

				half alpha = Alpha(i_tex.xy) * tex2D(_AlphaTex, i_tex.xy).r;
				clip(alpha - _Cutoff);

				FragmentCommonData o = UNITY_SETUP_BRDF_INPUT(i_tex);
				o.normalWorld = PerPixelWorldNormal(i_tex, tangentToWorld);
				o.eyeVec = NormalizePerPixelNormal(i_eyeVec);
				o.posWorld = i_posWorld;

				// NOTE: shader relies on pre-multiply alpha-blend (_SrcBlend = One, _DstBlend = OneMinusSrcAlpha)
				o.diffColor = PreMultiplyAlpha(o.diffColor, alpha, o.oneMinusReflectivity, /*out*/ o.alpha);
				return o;
			}

			VertexOutputForwardBase vertBase(VertexInput v) { return vertForwardBase(v); }
			half4 fragBase(VertexOutputForwardBase i) : SV_Target
			{ 
				UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

				FragmentCommonData s = CFragmentSetup(i.tex, i.eyeVec, IN_VIEWDIR4PARALLAX(i), i.tangentToWorldAndPackedData, IN_WORLDPOS(i));

				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				UnityLight mainLight = MainLight();
				UNITY_LIGHT_ATTENUATION(atten, i, s.posWorld);

				half occlusion = Occlusion(i.tex.xy);
				UnityGI gi = FragmentGI(s, occlusion, i.ambientOrLightmapUV, atten, mainLight);

				half4 c = UNITY_BRDF_PBS(s.diffColor, s.specColor, s.oneMinusReflectivity, s.smoothness, s.normalWorld, -s.eyeVec, gi.light, gi.indirect);
				c.rgb += Emission(i.tex.xy);

				UNITY_APPLY_FOG(i.fogCoord, c.rgb);
				return OutputForward(c, s.alpha);
			}

            ENDCG
        }

        Pass
        {
            Name "FORWARD_DELTA"
            Tags { "LightMode" = "ForwardAdd" }
            Blend One One
            Fog { Color (0,0,0,0) } // in additive pass fog should be black
            ZWrite Off
            ZTest LEqual

            CGPROGRAM
            #pragma target 3.0

			#define _ALPHATEST_ON
			#define _SPECGLOSSMAP
			//#define SHADOWS_SOFT
			
            #pragma multi_compile_fwdadd_fullshadows
            #pragma vertex vertAdd
            #pragma fragment fragAdd
			#include "UnityStandardCore.cginc"
			uniform sampler2D _AlphaTex;

			inline FragmentCommonData CFragmentSetup(inout float4 i_tex, float3 i_eyeVec, half3 i_viewDirForParallax, float4 tangentToWorld[3], float3 i_posWorld)
			{
				i_tex = Parallax(i_tex, i_viewDirForParallax);

				half alpha = Alpha(i_tex.xy) * tex2D(_AlphaTex, i_tex.xy).r;
				clip(alpha - _Cutoff);

				FragmentCommonData o = UNITY_SETUP_BRDF_INPUT(i_tex);
				o.normalWorld = PerPixelWorldNormal(i_tex, tangentToWorld);
				o.eyeVec = NormalizePerPixelNormal(i_eyeVec);
				o.posWorld = i_posWorld;

				// NOTE: shader relies on pre-multiply alpha-blend (_SrcBlend = One, _DstBlend = OneMinusSrcAlpha)
				o.diffColor = PreMultiplyAlpha(o.diffColor, alpha, o.oneMinusReflectivity, /*out*/ o.alpha);
				return o;
			}

			VertexOutputForwardAdd vertAdd(VertexInput v) { return vertForwardAdd(v); }
			half4 fragAdd(VertexOutputForwardAdd i) : SV_Target
			{
				UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

				FragmentCommonData s = CFragmentSetup(i.tex, i.eyeVec, IN_VIEWDIR4PARALLAX_FWDADD(i), i.tangentToWorldAndLightDir, IN_WORLDPOS_FWDADD(i));

				UNITY_LIGHT_ATTENUATION(atten, i, s.posWorld)
				UnityLight light = AdditiveLight(IN_LIGHTDIR_FWDADD(i), atten);
				UnityIndirect noIndirect = ZeroIndirect();

				half4 c = UNITY_BRDF_PBS(s.diffColor, s.specColor, s.oneMinusReflectivity, s.smoothness, s.normalWorld, -s.eyeVec, light, noIndirect);

				UNITY_APPLY_FOG_COLOR(i.fogCoord, c.rgb, half4(0,0,0,0)); // fog towards black in additive pass
				return OutputForward(c, s.alpha);
			}

            ENDCG
        }
    }


    //FallBack "VertexLit"
   // CustomEditor "StandardShaderGUI"
}
