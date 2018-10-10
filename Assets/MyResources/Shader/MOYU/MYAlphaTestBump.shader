Shader "MOYU/AlphaTestBump" 
{
	Properties 
	{ 
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("高光颜色", Color) = (1.0, 1.0, 1.0, 1.0)

		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

		_bump("BumpTex", 2D) = "white" {}
		_BunpZ("法线深度调节", Range(0.1, 4.0)) = 0.1
		_Shininess("高光范围", Range(0.01, 5)) = 1.0
		_Gloss("高光强度", Range(1, 10)) = 1.0
		_Contrast("对比度", Range(0.01, 1)) = 0.35
	}

	SubShader 
	{
		Lod 200
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "false" }
		ColorMask RGB
		Pass 
		{ 
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct v2f_vertex 
			{
				half4 pos : SV_POSITION;
				half4 uv : TEXCOORD0;
				half4 tSpace0 : TEXCOORD1;
				half4 tSpace1 : TEXCOORD2;
				half4 tSpace2 : TEXCOORD3;
				LIGHTING_COORDS(5, 6)
				UNITY_FOG_COORDS(7)
				UNITY_VERTEX_OUTPUT_STEREO
			};
			uniform half _Shininess;
			uniform half _Contrast;
			uniform half _Gloss;
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _AlphaTex;
			uniform half4 _AlphaTex_ST;
			uniform fixed _Cutoff;

			uniform fixed _BunpZ;
			uniform sampler2D _bump;

			v2f_vertex vert (appdata_full v)
			{
				v2f_vertex o ;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				#ifdef LIGHTMAP_ON
					o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = half4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = half4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = half4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
		
			fixed4 frag (v2f_vertex i) : COLOR
			{ 
				fixed alpha = tex2D(_AlphaTex, i.uv.xy).r;
				fixed4 c = tex2D(_MainTex, i.uv.xy);
				c.a *= alpha;
			    c.rgb *= _Color.rgb;
				#ifdef LIGHTMAP_ON
					c.rgb *= DecodeLightmap (UNITY_SAMPLE_TEX2D (unity_Lightmap, i.uv.zw));
				#endif

				SurfaceOutput o = (SurfaceOutput)0;
				o.Albedo = c.rgb;
				o.Specular = _Shininess;
				o.Gloss = _Gloss;

				half3 Normal = UnpackNormal(tex2D(_bump, i.uv.xy));
				Normal.z = 8 / (_BunpZ * 8);
				o.Normal = normalize(Normal);

				fixed3 worldN;
				worldN.x = dot(i.tSpace0.xyz, o.Normal);
				worldN.y = dot(i.tSpace1.xyz, o.Normal);
				worldN.z = dot(i.tSpace2.xyz, o.Normal);
				o.Normal = normalize(worldN);

				half3 worldPos = half3(i.tSpace0.w, i.tSpace1.w, i.tSpace2.w);
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				fixed atten = LIGHT_ATTENUATION(i);
				UnityLight light;
				light.color = _LightColor0.rgb * atten;
				light.dir = lightDir;

				c.rgb = c.rgb * _Contrast + UnityBlinnPhongLight(o, viewDir, light);

				clip( c.a - _Cutoff );
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

	SubShader
	{
		Lod 100
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "false" }
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }


			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct v2f_vertex
			{
				half4 pos : SV_POSITION;
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

			v2f_vertex vert(appdata_full v)
			{
				v2f_vertex o;
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
				fixed4 c = tex2D(_MainTex, i.uv.xy);
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
	}
}