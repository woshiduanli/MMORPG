Shader "MOYU/T4M4Bump" 
{
	Properties 
	{ 
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("高光颜色", Color) = (1.0, 1.0, 1.0, 1.0)

		_Splat0 ("Layer 0 (R)", 2D) = "white" {}
		_BumpMap0("Bump0 (RGB)", 2D) = "black" {}
		_BunpZ0("法线深度调节", Range(0.1, 4.0)) = 0.1

		_Splat1 ("Layer 1 (G)", 2D) = "white" {}
		_BumpMap1("Bump1 (RGB)", 2D) = "black" {}
		_BunpZ1("法线深度调节", Range(0.1, 4.0)) = 0.1

		_Splat2 ("Layer 2 (B)", 2D) = "white" {}
		_BumpMap2("Bump2 (RGB)", 2D) = "black" {}
		_BunpZ2("法线深度调节", Range(0.1, 4.0)) = 0.1

		_Splat3 ("Layer 3 (B)", 2D) = "white" {}
		_BumpMap3("Bump3 (RGB)", 2D) = "black" {}
		_BunpZ3("法线深度调节", Range(0.1, 4.0)) = 0.1

		_Control("Control (RGBA)", 2D) = "white" {}
		_Shininess("高光范围", Range(0.01, 5)) = 1.0
		_Gloss("高光强度", Range(0.1, 10)) = 1.0
		_Contrast("对比度", Range(0.01, 1)) = 0.35
	}

	SubShader 
	{
		Lod 300
		Tags {"Queue"="Geometry" "IgnoreProjector"="False"}
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
				fixed4 pos : POSITION;
				half4 uv0 : TEXCOORD0;
				half4 uv1 : TEXCOORD1;
				half4 uv2 : TEXCOORD2;
				half3 worldPos : TEXCOORD4;
				half4 tSpace0 : TEXCOORD5;
				half4 tSpace1 : TEXCOORD6;
				half4 tSpace2 : TEXCOORD7;
				LIGHTING_COORDS(8, 9)
				UNITY_FOG_COORDS(10)
			};
			uniform half _Shininess;
			uniform half _Gloss;
			uniform half _Contrast;

		    uniform fixed4 _Color;
			uniform sampler2D _Control;
			uniform half4 _Control_ST;
		
			uniform sampler2D _Splat0, _Splat1, _Splat2, _Splat3;
			uniform half4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;

			uniform sampler2D _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3;
			uniform half _BunpZ0, _BunpZ1, _BunpZ2, _BunpZ3;

			fixed3 CalNormal(sampler2D bump, half2 uv, half BunpZ)
			{
				fixed4 color = tex2D(bump, uv);
				fixed3 Normal = UnpackNormal(color);
				Normal.z = 8 / (BunpZ * 8);
				return normalize(Normal);
			}
		
			v2f_vertex vert (appdata_full v)
			{
				v2f_vertex o = (v2f_vertex)0;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv0.xy = TRANSFORM_TEX (v.texcoord.xy, _Control);
				o.uv0.zw = half2(0,0);
				#ifdef LIGHTMAP_ON
					o.uv0.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				
				o.uv1.xy = TRANSFORM_TEX(v.texcoord.xy, _Splat0);
				o.uv1.zw = TRANSFORM_TEX(v.texcoord.xy, _Splat1);
				o.uv2.xy = TRANSFORM_TEX(v.texcoord.xy, _Splat2);
				o.uv2.zw = TRANSFORM_TEX(v.texcoord.xy, _Splat3);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = half4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = half4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = half4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
		
			fixed4 frag (v2f_vertex i) : COLOR
			{
				fixed4 splat_control = tex2D(_Control, i.uv0.xy);
				fixed3 splat_color = splat_control.r * tex2D (_Splat0, i.uv1.xy).rgb;
				splat_color += splat_control.g  * tex2D(_Splat1, i.uv1.zw).rgb;
				splat_color += splat_control.b  * tex2D(_Splat2, i.uv2.xy).rgb;
				splat_color += splat_control.a * tex2D(_Splat3, i.uv2.zw).rgb;

				splat_color *= _Color.rgb;
				#ifdef LIGHTMAP_ON
					splat_color *= DecodeLightmap (UNITY_SAMPLE_TEX2D (unity_Lightmap, i.uv0.zw));
				#endif

				SurfaceOutput o = (SurfaceOutput)0;
				o.Albedo = splat_color;

				fixed3 sp1B = CalNormal(_BumpMap0, i.uv1.xy, _BunpZ0);
				fixed3 sp2B = CalNormal(_BumpMap1, i.uv1.zw, _BunpZ1);
				fixed3 sp3B = CalNormal(_BumpMap2, i.uv2.xy, _BunpZ2);
				fixed3 sp4B = CalNormal(_BumpMap3, i.uv2.zw, _BunpZ3);

				fixed3 Normal = sp1B * splat_control.r + sp2B * splat_control.g + sp3B * splat_control.b + sp4B * splat_control.a;
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

				o.Gloss = _Gloss;
				o.Specular = _Shininess;

				splat_color.rgb = splat_color.rgb * _Contrast + UnityBlinnPhongLight(o, viewDir, light);

				UNITY_APPLY_FOG(i.fogCoord, splat_color);
				return fixed4 (splat_color, 0.0);
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
	}
}
