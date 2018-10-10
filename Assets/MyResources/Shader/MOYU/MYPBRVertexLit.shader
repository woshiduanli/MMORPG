Shader "MOYU/PBR/VertexLit"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("主贴图", 2D) = "white"{}
		_NormalTex("法线", 2D) = "bump"{}
		_MetalnessTex("金属遮罩", 2D) = "black"{}
		_Metalness("金属系数", Range(0,1)) = 0.35
		_Smoothness("Smoothness", Range(0,1)) = 0.35
	}

	SubShader
	{
		Tags{ "RenderType" = "Geometry" "IgnoreProjector" = "False" }
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			ColorMask RGB
			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _NormalTex;
			uniform sampler2D _MetalnessTex;

			uniform fixed4 _Color;
			uniform fixed _Reflect;
			uniform fixed _Metalness;
			uniform fixed _Smoothness;
			uniform fixed _MatCapPower;

			struct v2f_surf
			{
				UNITY_POSITION(pos);
				half4 uv : TEXCOORD0;
				half4 tSpace0 : TEXCOORD1;
				half4 tSpace1 : TEXCOORD2;
				half4 tSpace2 : TEXCOORD3;
				half3 sh : TEXCOORD4;
				LIGHTING_COORDS(5, 6)
				half4 uv1 : TEXCOORD7;
			};

			struct PBROutput
			{
				fixed3 Albedo;
				fixed3 Normal;
				fixed3 Emission;
				fixed Metallic;
				fixed Smoothness;
				fixed Alpha;
			};

			inline fixed3 BRDF_PBS(PBROutput s, fixed3 viewDir, UnityGI gi)
			{
				fixed reflect;
				fixed3 specColor;
				fixed3 Albedo = s.Albedo;
				s.Albedo = DiffuseAndSpecularFromMetallic(s.Albedo, s.Metallic, specColor, reflect);
				fixed3 normal = normalize(s.Normal);
				fixed3 lightdir = gi.light.dir;
				fixed3 lightcolor = gi.light.color;

				fixed Roughness = 1 - s.Smoothness;
				fixed3 halfDir = normalize(lightdir + viewDir);
				fixed nv = abs(dot(normal, viewDir));
				fixed nl = saturate(dot(normal, lightdir));
				fixed nh = saturate(dot(normal, halfDir));
				fixed lv = saturate(dot(lightdir, viewDir));
				fixed lh = saturate(dot(lightdir, halfDir));

				fixed diffuseTerm = DisneyDiffuse(nv, nl, lh, Roughness) * nl;
				fixed roughness = Roughness*Roughness;

				roughness = max(roughness, 0.002);
				fixed V = SmithJointGGXVisibilityTerm(nl, nv, roughness);
				fixed D = GGXTerm(nh, roughness);

				fixed specularTerm = V*D * UNITY_PI;
				specularTerm = sqrt(max(1e-4h, specularTerm));
				specularTerm = max(0, specularTerm * nl);

				return s.Albedo * (gi.indirect.diffuse + lightcolor * diffuseTerm) + specularTerm * lightcolor * FresnelTerm(specColor, lh);
			}


			v2f_surf vert_surf(appdata_full v)
			{
				v2f_surf o = (v2f_surf)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				half3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = half4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = half4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = half4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
				o.sh = ShadeSHPerVertex(worldNormal, o.sh);
				return o;
			}

			fixed4 frag_surf(v2f_surf IN) : SV_Target
			{
				fixed4 albedo = tex2D(_MainTex, IN.uv.xy);
				fixed4 c = albedo;
				fixed3 normal = UnpackNormal(tex2D(_NormalTex, IN.uv.xy));
				fixed2 metalnessTexed = tex2D(_MetalnessTex, IN.uv.xy).ra * _Metalness;
				metalnessTexed.g *= _Smoothness;
				fixed metallic = metalnessTexed.r;

				//边缘光
				half3 worldPos = half3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				PBROutput o = (PBROutput)0;
				o.Albedo = albedo.rgb * _Color.rgb;
				o.Normal = normal;
				o.Metallic = metallic;
				o.Smoothness = metalnessTexed.g;
				o.Emission = 0 ;

				fixed3 worldN;
				worldN.x = dot(IN.tSpace0.xyz, o.Normal);
				worldN.y = dot(IN.tSpace1.xyz, o.Normal);
				worldN.z = dot(IN.tSpace2.xyz, o.Normal);
				worldN = normalize(worldN);
				o.Normal = worldN;

				UnityGI gi;
				gi.indirect.diffuse = ShadeSHPerPixel(o.Normal, IN.sh, worldPos);
				gi.indirect.specular = 0;
				gi.light.color = _LightColor0.rgb;
				gi.light.dir = normalize(UnityWorldSpaceLightDir(worldPos));

				c.rgb = BRDF_PBS(o, worldViewDir, gi);
				c.rgb += o.Emission;
				c.a *= _Color.a;
				return c;
			}
			ENDCG
		}
	}
}