Shader "MOYU/VertexLit" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (0,0,0,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Brightness("Brightness", Range(0,5)) = 1
	}

	SubShader 
	{
		Tags {"RenderType"="Geometry" "IgnoreProjector"="False"}
		ColorMask RGB
		Pass 
		{ 
			Name "FORWARD"
			ColorMask RGB
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"

			struct appdata_lightmap
			{
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				half2 texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
		
			struct v2f_vertex 
			{
				half4 pos : POSITION;
				half4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};
		
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half _Brightness;

			v2f_vertex vert (appdata_lightmap v)
			{
				v2f_vertex o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv.xy = TRANSFORM_TEX (v.texcoord.xy, _MainTex);
				#ifdef LIGHTMAP_ON
					o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
		
			fixed4 frag (v2f_vertex i) : COLOR
			{
				fixed3 c = tex2D(_MainTex, i.uv.xy) * _Color.rgb * _Brightness;
				#ifdef LIGHTMAP_ON
					c *= DecodeLightmap (UNITY_SAMPLE_TEX2D (unity_Lightmap, i.uv.zw));
				#endif
				UNITY_APPLY_FOG(i.fogCoord, c);
				return fixed4 (c, 0.0);
			}

			ENDCG
 		}

		Pass
		{
			Tags{ "LightMode" = "ForwardAdd" }
			ZWrite Off Blend One One

			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd

			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct v2f_surf
			{
				half4 pos : SV_POSITION;
				half3 worldNormal : TEXCOORD0;
				half3 worldPos : TEXCOORD1;
				half3 lightDir : TEXCOORD2;
				half lightpos: TEXCOORD3;
			};

			v2f_surf vert_surf(appdata_base v)
			{
				v2f_surf o = (v2f_surf)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				half3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.worldPos = worldPos;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				o.lightpos = _WorldSpaceLightPos0.w;
				return o;
			}

			fixed4 frag_surf(v2f_surf IN) : SV_Target
			{
				fixed4 c = 0;
				SurfaceOutput o;
				o.Albedo = fixed3(1,1,1);
				o.Emission = 0.0;
				o.Specular = 0.0;
				o.Alpha = 0.0;
				o.Gloss = 0.0;
				o.Normal = IN.worldNormal;

				UNITY_LIGHT_ATTENUATION(atten, IN, IN.worldPos)
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
				gi.indirect.diffuse = 0;
				gi.indirect.specular = 0;
				gi.light.color = _LightColor0.rgb;
				gi.light.dir = IN.lightDir;
				gi.light.color *= atten;
				c += LightingLambert(o, gi)* IN.lightpos;
				c.a = 0.0;
				return c;
			}
			ENDCG
		}

		Pass
		{
			Name "SHADOWCASTER"
			Tags{ "LIGHTMODE" = "SHADOWCASTER" "SHADOWSUPPORT" = "true" "RenderType" = "Opaque" }
			CGPROGRAM
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct v2f 
			{
				V2F_SHADOW_CASTER;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}