Shader "MOYU/T4M4Textures"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_Splat0("Layer 0 (R)", 2D) = "white" {}
		_Splat1("Layer 1 (G)", 2D) = "white" {}
		_Splat2("Layer 2 (B)", 2D) = "white" {}
		_Splat3("Layer 3 (B)", 2D) = "white" {}
		_Control("Control (RGBA)", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry" "IgnoreProjector" = "False" }
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

			struct v2f_vertex
			{
				fixed4 pos : POSITION;
				half4 uv0 : TEXCOORD0;
				half4 uv1 : TEXCOORD1;
				half4 uv2 : TEXCOORD2;
				UNITY_FOG_COORDS(3)
			};

			uniform fixed4 _Color;
			uniform sampler2D _Control;
			uniform half4 _Control_ST;

			uniform sampler2D _Splat0, _Splat1, _Splat2, _Splat3;
			uniform half4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;

			v2f_vertex vert(appdata_full v)
			{
				v2f_vertex o = (v2f_vertex)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.texcoord.xy, _Control);
				o.uv0.zw = half2(0,0);
				#ifdef LIGHTMAP_ON
				o.uv0.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				o.uv1.xy = TRANSFORM_TEX(v.texcoord.xy, _Splat0);
				o.uv1.zw = TRANSFORM_TEX(v.texcoord.xy, _Splat1);
				o.uv2.xy = TRANSFORM_TEX(v.texcoord.xy, _Splat2);
				o.uv2.zw = TRANSFORM_TEX(v.texcoord.xy, _Splat3);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(v2f_vertex i) : COLOR
			{
				fixed4 splat_control = tex2D(_Control, i.uv0.xy);

				fixed3 splat_color = splat_control.r * tex2D(_Splat0, i.uv1.xy).rgb;
				splat_color += splat_control.g  * tex2D(_Splat1, i.uv1.zw).rgb;
				splat_color += splat_control.b  * tex2D(_Splat2, i.uv2.xy).rgb;
				splat_color += splat_control.a * tex2D(_Splat3, i.uv2.zw).rgb;

				splat_color *= _Color.rgb;
				#ifdef LIGHTMAP_ON
				splat_color *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv0.zw));
				#endif

				UNITY_APPLY_FOG(i.fogCoord, splat_color);
				return fixed4(splat_color, 0.0);
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

			v2f_surf vert_surf(appdata_full v)
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
	}
}
