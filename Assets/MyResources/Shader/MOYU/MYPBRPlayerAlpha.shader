Shader "MOYU/PBR/PlayerAlpha"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_RimColor("RimColor", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_GlossMapScale("Smoothness Factor", Range(0.0, 1.0)) = 1.0
		_SpecGlossMap("Specular", 2D) = "black" {}
	}

	SubShader
	{ 
		Lod 200
		Tags{ "Queue" = "AlphaTest" "LightMode" = "ForwardBase" "RenderType" = "AlphaTest" "IgnoreProjector" = "True" }
		Pass
		{
			CGPROGRAM
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma vertex vert
			#pragma fragment frag
			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _AlphaTex;
			uniform half4 _AlphaTex_ST;
			uniform sampler2D  _SpecGlossMap;
			uniform fixed _GlossMapScale;
			uniform fixed4 _Color;
			uniform fixed _MatCapPower;
			uniform fixed3 _RimColor;

			struct v2f_surf
			{
				UNITY_POSITION(pos);
				half2 uv				   : TEXCOORD0;
				half3 eyeVec              : TEXCOORD1;
				half3 normalWorld		   : TEXCOORD2;
				half3 posWorld            : TEXCOORD4;
				half3 viewDir : TEXCOORD5;
				half3 normal			   :TEXCOORD6;
				LIGHTING_COORDS(7, 8)
			};

			struct PBROutput
			{
				fixed3 diffColor, specColor;
				fixed oneMinusReflectivity, smoothness;
				fixed3 normalWorld;
				fixed3 eyeVec;
				fixed alpha;
				half3 posWorld;
			};

			half RoughnessToSP(half perceptualRoughness)
			{
				half m = perceptualRoughness * perceptualRoughness;
				half sq = max(1e-4f, m*m);
				half n = (2.0 / sq) - 2.0;
				n = max(n, 1e-4f);
				return n;
			}

			fixed4 BRDF_PBS(PBROutput s,UnityLight light, UnityIndirect gi)
			{
				fixed3 halfDir = Unity_SafeNormalize(half3(light.dir) - s.eyeVec);
				fixed nl = saturate(dot(s.normalWorld, light.dir));
				fixed nh = saturate(dot(s.normalWorld, halfDir));
				fixed nv = saturate(dot(s.normalWorld, -s.eyeVec));
				fixed lh = saturate(dot(light.dir, halfDir));
				fixed perceptualRoughness = 1 - s.smoothness;
				fixed roughness = perceptualRoughness * perceptualRoughness;
				half specularPower = RoughnessToSP(perceptualRoughness);
				half invV = lh * lh * s.smoothness + perceptualRoughness * perceptualRoughness;
				half invF = lh;

				half specularTerm = ((specularPower + 1) * pow(nh, specularPower)) / (8 * invV * invF + 1e-4h);
				specularTerm = sqrt(max(1e-4f, specularTerm));
				specularTerm = clamp(specularTerm, 0.0, 100.0);
				fixed surfaceReduction = 0.28;
				surfaceReduction = 1.0 - roughness * perceptualRoughness * surfaceReduction;
				fixed grazingTerm = saturate(s.smoothness + (1 - s.oneMinusReflectivity));

				fixed3 Fresnel = lerp(s.specColor, grazingTerm, Pow4(1 - nv));
				fixed3 color = (s.diffColor + specularTerm * s.specColor) * light.color * nl + gi.diffuse * s.diffColor + surfaceReduction * gi.specular * Fresnel;

				return fixed4(color, 1);
			}

			v2f_surf vert(appdata_full v)
			{
				v2f_surf o = (v2f_surf)0;
				UNITY_INITIALIZE_OUTPUT(v2f_surf, o);

				half4 posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.eyeVec = normalize(posWorld.xyz - _WorldSpaceCameraPos);
				half3 normalWorld = UnityObjectToWorldNormal(v.normal);
				o.normalWorld = normalWorld;

				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex).xyz));
				return o;
			}

			fixed4 frag(v2f_surf i) : SV_Target
			{
				fixed4 mian = tex2D(_MainTex, i.uv);
				fixed alpha = mian.a * tex2D(_AlphaTex, i.uv).r * _Color.a;
				clip(alpha - 0.5);
				fixed4 specGloss = tex2D(_SpecGlossMap, i.uv);
				specGloss.a *= _GlossMapScale;

				fixed3 specColor = specGloss.rgb;
				fixed smoothness = specGloss.a;

				fixed oneMinusReflectivity = 1 - max(max(specColor.r, specColor.g), specColor.b);
				fixed3 diffColor = _Color.rgb * mian.rgb * oneMinusReflectivity;

				PBROutput s = (PBROutput)0;
				s.diffColor = diffColor;
				s.specColor = specColor;
				s.oneMinusReflectivity = oneMinusReflectivity;
				s.smoothness = smoothness;

				s.normalWorld = normalize(i.normalWorld);
				s.eyeVec = normalize(i.eyeVec);
				s.alpha = alpha;

				UnityLight mainLight;
				mainLight.color = _LightColor0.rgb;
				mainLight.dir = _WorldSpaceLightPos0.xyz;

				half atten = LIGHT_ATTENUATION(i);
				Unity_GlossyEnvironmentData g;
				g.roughness = 1 - s.smoothness;
				g.reflUVW = reflect(s.eyeVec, s.normalWorld);

				UnityGI gi;
				ResetUnityGI(gi);
				gi.light = mainLight;
				gi.light.color *= atten;
				gi.indirect.diffuse = ShadeSHPerPixel(s.normalWorld, fixed3(0.3, 0.3, 0.3), s.posWorld);

				//边缘光
				fixed3 rim = _RimColor * saturate(1 - saturate(dot(i.normal, i.viewDir)) * 1.8) *(0.2 + _MatCapPower);

				fixed4 c = BRDF_PBS(s, gi.light, gi.indirect);
				c.rgb += rim;
				return fixed4(c.rgb, s.alpha);
			}
			ENDCG
		}
	}

	SubShader
	{
		Lod 100
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_fwdbase

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _AlphaTex;
			uniform half4 _Color;
			uniform half3 _RimColor;
			uniform half _MatCapPower;

			struct v2f_surf
			{
				UNITY_POSITION(pos);
				half2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
				half3 viewDir : TEXCOORD2;
			};

			v2f_surf vert_surf(appdata_full v)
			{
				v2f_surf o = (v2f_surf)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				half3 worldN = UnityObjectToWorldNormal(v.normal);
				o.normal = worldN;
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex).xyz));
				return o;
			}

			fixed4 frag_surf(v2f_surf IN) : SV_Target
			{ 
				fixed alpha = tex2D(_AlphaTex, IN.uv).r;
				fixed4 c = tex2D(_MainTex, IN.uv);
				c.a *= alpha;
				c.rgb *= _Color.rgb;
				c.rgb += _RimColor * saturate(1 - saturate(dot(IN.normal, IN.viewDir)) * 1.8)  *(0.2 + _MatCapPower);
				clip(c.a - 0.5);
				return c;
			}
			ENDCG
		}
	}
}
