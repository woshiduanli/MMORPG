Shader "MOYU/VertexLitBumpDir" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (0,0,0,1)
		_SpecColor("高光颜色", Color) = (1.0, 1.0, 1.0, 1.0)

		_Blend("混合系数", Range(0, 1)) = 0

		_MainTex2 ("Base (RGB)", 2D) = "white" {}
		_bump2("BumpTex", 2D) = "bump" {}

		_MainTex("叠加贴图", 2D) = "white" {}
		_bump("叠加法线", 2D) = "bump" {}

		_BunpZ("法线深度调节", Range(0.1, 4.0)) = 0.1
		_Shininess("高光范围", Range(0.01, 5)) = 1.0

		_Gloss("高光强度", Range(0.1, 10)) = 1.0
		_Contrast("对比度", Range(0.01, 1)) = 0.35

		_Brightness("Brightness", Range(0,5)) = 1
	}

	SubShader 
	{
	    Lod 200
		Tags {"RenderType"="Geometry" "IgnoreProjector"="False"}
		ColorMask RGB
		Pass 
		{ 
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			ColorMask RGB
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
				half2 uv : TEXCOORD0;
				half4 uv2 : TEXCOORD1;
				half4 tSpace0 : TEXCOORD2;
				half4 tSpace1 : TEXCOORD3;
				half4 tSpace2 : TEXCOORD4;
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

			uniform sampler2D _MainTex2;
			uniform half4 _MainTex2_ST;

			uniform half _BunpZ;
			uniform sampler2D _bump;
			uniform sampler2D _bump2;
			uniform half _Blend;
			uniform half _Brightness;

			v2f_vertex vert (appdata_full v)
			{
				v2f_vertex o = (v2f_vertex)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv.xy = TRANSFORM_TEX (v.texcoord.xy, _MainTex);
				o.uv2.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex2);
				#ifdef LIGHTMAP_ON
				o.uv2.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
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
				fixed3 c = tex2D(_MainTex, i.uv.xy);
				fixed3 c2 = tex2D(_MainTex2, i.uv2.xy);
				c = lerp(c2, 2 * c.rgb * c2.rgb , _Blend);
				c *= _Color.rgb*_Brightness;

				#ifdef LIGHTMAP_ON
				c *= DecodeLightmap (UNITY_SAMPLE_TEX2D (unity_Lightmap, i.uv2.zw));
				#endif

				SurfaceOutput o = (SurfaceOutput)0;
				o.Albedo = c.rgb;
				o.Specular = _Shininess;
				o.Gloss = _Gloss;
				o.Emission = 0.0;

				half3 Normal = UnpackNormal(tex2D(_bump, i.uv.xy));
				half3 Normal2 = UnpackNormal(tex2D(_bump2, i.uv2.xy));
				Normal = lerp(Normal2, 1 - 2 * (1 - Normal) * (1 - Normal2)  , _Blend);

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

	SubShader
	{
		Lod 100
		Tags{ "RenderType" = "Geometry" "IgnoreProjector" = "False" }
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

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"

			struct v2f_vertex
			{
				half4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half4 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(7)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;

			uniform sampler2D _MainTex2;
			uniform half4 _MainTex2_ST;
			uniform half _Blend;

			v2f_vertex vert(appdata_full v)
			{
				v2f_vertex o = (v2f_vertex)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				o.uv2.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex2);
				#ifdef LIGHTMAP_ON
				o.uv2.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			fixed4 frag(v2f_vertex i) : COLOR
			{
				fixed3 c = tex2D(_MainTex, i.uv.xy);
				fixed3 c2 = tex2D(_MainTex2, i.uv2.xy);
				c = lerp(c2, 2 * c.rgb * c2.rgb , _Blend);
				c *= _Color.rgb;

				#ifdef LIGHTMAP_ON
				c *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2.zw));
				#endif

				UNITY_APPLY_FOG(i.fogCoord, c);
				return fixed4(c, 0.0);
			}
			ENDCG
		}
	}
}