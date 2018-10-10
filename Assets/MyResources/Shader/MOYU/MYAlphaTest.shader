Shader "MOYU/AlphaTest"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_Brightness("Brightness", Range(0,5)) = 1
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "false" }
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
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
			uniform sampler2D _AlphaTex;
			uniform half4 _AlphaTex_ST;
			uniform fixed _Cutoff;
			uniform half _Brightness;
			v2f_vertex vert(appdata_lightmap v)
			{
				v2f_vertex o = (v2f_vertex)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				#ifdef LIGHTMAP_ON
				o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			fixed4 frag(v2f_vertex i) : COLOR
			{ 
				fixed alpha = tex2D(_AlphaTex, i.uv.xy).r;
				fixed4 c = tex2D(_MainTex, i.uv.xy)*_Brightness;
				c.a *= alpha;
				c.rgb *= _Color.rgb;
				#ifdef LIGHTMAP_ON
				c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv.zw));
				#endif

				clip(c.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}

		Pass
		{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardAdd" }
			ZWrite Off Blend One One
			Blend SrcAlpha One
			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			fixed _Cutoff;

			struct v2f_surf
			{
				half4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half3 worldNormal : TEXCOORD1;
				half3 worldPos : TEXCOORD2;
				half3 lightDir : TEXCOORD3;
				half lightpos: TEXCOORD4;
			};

			v2f_surf vert_surf(appdata_full v)
			{
				v2f_surf o = (v2f_surf)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				half3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = worldPos;
				o.worldNormal = worldNormal;
				o.lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				o.lightpos = _WorldSpaceLightPos0.w;
				return o;
			}

			fixed4 frag_surf(v2f_surf IN) : SV_Target
			{
				fixed4 c = 0;
				half3 worldPos = IN.worldPos;
				SurfaceOutput o = (SurfaceOutput)0;
				o.Albedo = fixed3(1,1,1);
				o.Emission = 0.0;
				o.Specular = 0.0;
				o.Alpha = 0.0;
				o.Gloss = 0.0;
				fixed3 normalWorldVertex = fixed3(0,0,1);
				o.Normal = IN.worldNormal;
				normalWorldVertex = IN.worldNormal;

				UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)

					UnityGI gi;
				UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
				gi.indirect.diffuse = 0;
				gi.indirect.specular = 0;
				gi.light.color = _LightColor0.rgb;
				gi.light.dir = IN.lightDir;
				gi.light.color *= atten;
				c += LightingLambert(o, gi) * IN.lightpos;
				c.a = tex2D(_MainTex, IN.uv).a * tex2D(_AlphaTex, IN.uv).r;
				clip(c.a - _Cutoff);
				return c;
			}
			ENDCG
		}
	}
}