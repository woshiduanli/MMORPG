Shader "MOYU/Water"
{
	Properties
	{
		_Color("Main Color", Color) = (0,0,0,0)
		_Specular("Specular", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.01, 1.0)) = 1.0

		_MainTex("Diffuse (RGBA)", 2D) = "black" {}
		_MainPower("MainPower", Range(0.0, 1.0)) = 0.8

		_WaterTex("Normal Map (RGB), Foam (A)", 2D) = "white" {}
		_Tiling("Tiling", Range(0.025, 0.25)) = 0.25
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			half4 _Color;
			half4 _Specular;
			half _Shininess;
			half _Tiling;
			half _MainPower;

			sampler2D _WaterTex;
			sampler2D _MainTex;
			half4 _MainTex_ST;

			half4 LightingPPL(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 nNormal = normalize(s.Normal);
				fixed diff = max(0, dot(s.Normal, lightDir));
				fixed nh = max(0, dot(s.Normal, viewDir));
				fixed spec = pow(nh, s.Specular * 256);

				half4 c;
				c.rgb = (s.Albedo * diff + _Specular.rgb * spec) * _LightColor0.rgb * atten;
				c.a = s.Alpha;
				return c;
			}

			struct v2f_surf
			{
				UNITY_POSITION(pos);
				half2 uv : TEXCOORD0;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				half3 worldPos  : TEXCOORD4;
				half4 lmap : TEXCOORD5;
				LIGHTING_COORDS(7, 8)
				UNITY_FOG_COORDS(9)
			};

			v2f_surf vert_surf(appdata_full v)
			{
				v2f_surf o = (v2f_surf)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				half3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
				o.worldPos = half3(o.tSpace0.w, o.tSpace1.w, o.tSpace2.w);
				float offset = _Time.x * 0.5;
				half2 tiling = worldPos.xz * _Tiling;

				#ifdef LIGHTMAP_ON
				o.lmap.xy = (v.texcoord1.xy) * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			void surf(v2f_surf i, inout SurfaceOutput o)
			{
				half3 worldView = (i.worldPos - _WorldSpaceCameraPos);
				half offset = _Time.x * 0.5;
				half2 tiling = i.worldPos.xz * _Tiling;
				half4 nmap = (tex2D(_WaterTex, tiling + offset) + tex2D(_WaterTex, half2(-tiling.y, tiling.x) - offset)) * 0.5;
				half3 worldNormal = o.Normal.xzy;
				worldNormal.z = -worldNormal.z;

				half fresnel = dot(-normalize(worldView), worldNormal);
				fresnel *= fresnel;

				half4 reflection = tex2D(_MainTex, i.uv);
				reflection.rgb = lerp(fixed3(0, 0, 0), reflection.rgb, _MainPower);
				o.Albedo = reflection.rgb * _Color.rgb;
				o.Albedo *= 1.0 - fresnel;

				o.Alpha = _Color.a * reflection.a;
				o.Normal = nmap.xyz * 2.0 - 1.0;
				o.Specular = _Shininess;
			}

			fixed4 frag_surf(v2f_surf i) : SV_Target
			{
				#ifndef USING_DIRECTIONAL_LIGHT
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				#else
				fixed3 lightDir = _WorldSpaceLightPos0.xyz;
				#endif
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				worldViewDir = normalize(worldViewDir + lightDir);
				SurfaceOutput o = (SurfaceOutput)0;
				o.Normal = fixed3(0,0,1);

				surf(i, o);
				fixed atten = LIGHT_ATTENUATION(i);
				fixed4 c = 0;
				fixed3 worldN;
				worldN.x = dot(i.tSpace0.xyz, o.Normal);
				worldN.y = dot(i.tSpace1.xyz, o.Normal);
				worldN.z = dot(i.tSpace2.xyz, o.Normal);
				worldN = normalize(worldN);
				o.Normal = worldN;

				c += LightingPPL(o, lightDir, worldViewDir, atten);

				#ifdef LIGHTMAP_ON
				fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy));
				c.rgb += o.Albedo * lm;
				#endif

				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
}
