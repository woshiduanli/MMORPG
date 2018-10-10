Shader "MOYU/PBR/Water2"
{
	Properties
	{ 
		_Color("Color", Color) = (1,1,1,1)
		_LightColor("LightColor", Color) = (1,1,1,1)
		_BorderColor("边缘颜色", Color) = (0,0,0,0)
		_Metallic("Metallic", Range(0, 1)) = 0
		_Roughness("Roughness", Range(0, 1)) = 0.7503998

		[NoScaleOffset]_WaterAlphaTexture("WaterAlphaTexture", 2D) = "white" {}
		_WaterAlphaPower("WaterAlphaPower", Range(0, 1)) = 0.7863253

		[NoScaleOffset]_Normal_A_Texture("Normal_A_Texture", 2D) = "bump" {}
		_Normal_A_Scale("Normal_A_Scale(int)", int) = 10
		_NormalA_Time_U("NormalA_Time_U", Float) = 0
		_NormalA_Time_V("NormalA_Time_V", Float) = 0

		[NoScaleOffset]_Normal_B_Texture("Normal_B_Texture", 2D) = "bump" {}
		_Normal_B_Scale("Normal_B_Scale(int)", int) = 10
		_NormalB_Time_U("NormalB_Time_U", Float) = 0
		_NormalB_Time_V("NormalB_Time_V", Float) = 0
		_CausticsPower("CausticsPower", Range(0, 1)) = 1
		_Border("亮度, 边缘, 反射偏移", Vector) = (1, 0, -0.8, -0.2)

		_ReflectionTex("Reflection", 2D) = "white" { TexGen ObjectLinear }
		_LightDir("Light Dir",Vector) = (1,1,1,1)
	}
	SubShader
	{
		Lod 300
		Tags{ "IgnoreProjector" = "True" "Queue" = "Geometry+10" "RenderType" = "Transparent" }
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
			Tags {"LightMode" = "ForwardBase"}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fog

			#pragma multi_compile_fwdbase
			uniform fixed4 _Color;
			uniform fixed4 _LightColor;
			uniform fixed4 _BorderColor;
			uniform fixed _Metallic;
			uniform fixed _Roughness;
			uniform fixed _WaterAlphaPower;

			uniform sampler2D _WaterAlphaTexture;
			uniform fixed4 _WaterAlphaTexture_ST;

			uniform sampler2D _Normal_A_Texture;
			uniform fixed4 _Normal_A_Texture_ST;
			uniform sampler2D _Normal_B_Texture;
			uniform fixed4 _Normal_B_Texture_ST;

			uniform float _NormalA_Time_U;
			uniform float _NormalA_Time_V;
			uniform int _Normal_A_Scale;

			uniform float _NormalB_Time_U;
			uniform float _NormalB_Time_V;
			uniform int _Normal_B_Scale;

			uniform fixed _CausticsPower;
			uniform float4 _Border;
			uniform float4 _LightDir;

			uniform sampler2D _ReflectionTex;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
			};


			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 proj0  : TEXCOORD1;
				float3 worldPos  : TEXCOORD3;
				float2 uv1 : TEXCOORD4;
				float4 uv2 : TEXCOORD5;
				float4 posWorld : TEXCOORD6;
				fixed3 normalDir : TEXCOORD7;
				fixed3 tangentDir : TEXCOORD8;
				fixed3 bitangentDir : TEXCOORD9;
				UNITY_FOG_COORDS(10)
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0.xy, _WaterAlphaTexture);

				float2 UV = TRANSFORM_TEX(v.texcoord1.xy, _Normal_A_Texture);
				o.uv1.xy = UV;

				float NATU = fmod(_Time*_NormalA_Time_U, 1);
				float NATV = fmod(_Time*_NormalA_Time_V, 1);
				o.uv1.xy = (UV + float2(NATU, NATV)) * _Normal_A_Scale;

				float NBTU = fmod(_Time*_NormalB_Time_U, 1);
				float NBTV = fmod(_Time*_NormalB_Time_V, 1);
				o.uv2.xy = (UV + float2(NBTU, NBTV)) * _Normal_B_Scale;

				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, fixed4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = v.vertex.xyz;
				o.proj0 = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.proj0.z);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}


			fixed4 frag(VertexOutput i) : COLOR
			{
				fixed3 diffuseColor = _Color.rgb * _Border.x;
				fixed3 specular = fixed3(0, 0, 0);
				fixed4 _WaterAlpha = tex2D(_WaterAlphaTexture, i.uv0);
				fixed alpha = saturate(_WaterAlpha.rgb);

				i.normalDir = normalize(i.normalDir) + normalize(_LightDir.xyz);
				fixed3x3 tangentTransform = fixed3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);

				fixed3 _Normal_A = UnpackNormal(tex2D(_Normal_A_Texture, i.uv1));
				fixed3 _Normal_B = UnpackNormal(tex2D(_Normal_B_Texture, i.uv2));

				fixed3 normal = normalize(mul(_Normal_A.rgb * _Normal_B.rgb, tangentTransform));

				float3 ranges = _Border.xyz * alpha;
				ranges.y = lerp(ranges.y, ranges.y * ranges.y * ranges.y, 0.5);
				//反射
				i.proj0.xy += normal.xy + _Border.zw;
				float3 reflection = tex2Dproj(_ReflectionTex, i.proj0).rgb;

				float3 worldView = (i.worldPos - _WorldSpaceCameraPos);
				float3 worldNormal = normal.xzy;
				worldNormal.z = -worldNormal.z;
				float fresnel = sqrt(1.0 - dot(-normalize(worldView), worldNormal));
				fresnel *= fresnel * fresnel;
				fresnel = (0.8 * fresnel + 0.2);

				diffuseColor = diffuseColor + _BorderColor.rgb * _WaterAlpha.a * ranges.y;
				diffuseColor = lerp(reflection * diffuseColor, reflection, fresnel * 0.5);

				fixed3 lightdir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 floatDir = normalize(viewDir + lightdir);

				fixed gloss = _Roughness;
				fixed perceptualRoughness = 1.0 - _Roughness;
				fixed roughness = perceptualRoughness * perceptualRoughness;

				fixed3 specColor;
				fixed smoothness;
				diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, _Metallic, specColor, smoothness);

				smoothness = 1.0 - smoothness;
				fixed NL = saturate(dot(normal, lightdir));
				fixed LH = saturate(dot(lightdir, floatDir));
				fixed NV = abs(dot(normal, viewDir));
				fixed NH = saturate(dot(normal, floatDir));
				fixed V = SmithJointGGXVisibilityTerm(NL, NV, roughness);
				fixed D = GGXTerm(NH, roughness);
				fixed specularTerm = V*D * UNITY_PI;
				specularTerm = sqrt(max(1e-4h, specularTerm));

				specular = _LightColor0.xyz*specularTerm*FresnelTerm(specColor, LH);
				UnityLight light;
				light.color = lightColor;
				light.dir = lightdir;

				Unity_GlossyEnvironmentData ugls_en_data;
				ugls_en_data.roughness = 1.0 - gloss;
				ugls_en_data.reflUVW = reflect(-viewDir, normal);

				fixed3 indirectSpecular = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, ugls_en_data);
				specular = (specular + indirectSpecular)* specColor;

				fixed WAWP = alpha * _WaterAlphaPower;
				fixed CTP = (1.0 - WAWP)*_CausticsPower;
				fixed3 emissive = fixed3(CTP, CTP, CTP);
				fixed3 finalColor = fixed3(diffuseColor + specular * _LightColor + emissive);
				fixed Alpha = (CTP + WAWP)*_LightColor.a;
				fixed4 finalRGBA = fixed4(finalColor,Alpha);

				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}
			ENDCG
		}
	}

	SubShader
	{
		Lod 200
		Tags{ "IgnoreProjector" = "True" "Queue" = "Geometry" "RenderType" = "Transparent" }
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fog

			#pragma multi_compile_fwdbase
			uniform fixed4 _Color;
			uniform fixed4 _LightColor;
			uniform fixed4 _BorderColor;
			uniform fixed _Metallic;
			uniform fixed _Roughness;
			uniform fixed _WaterAlphaPower;

			uniform sampler2D _WaterAlphaTexture;
			uniform fixed4 _WaterAlphaTexture_ST;

			uniform sampler2D _Normal_A_Texture;
			uniform fixed4 _Normal_A_Texture_ST;
			uniform sampler2D _Normal_B_Texture;
			uniform fixed4 _Normal_B_Texture_ST;

			uniform float _NormalA_Time_U;
			uniform float _NormalA_Time_V;
			uniform int _Normal_A_Scale;

			uniform float _NormalB_Time_U;
			uniform float _NormalB_Time_V;
			uniform int _Normal_B_Scale;

			uniform fixed _CausticsPower;
			uniform float4 _Border;
			uniform float4 _LightDir;

			uniform sampler2D _ReflectionTex;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
			};


			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 proj0  : TEXCOORD1;
				float3 worldPos  : TEXCOORD3;
				float2 uv1 : TEXCOORD4;
				float4 uv2 : TEXCOORD5;
				float4 posWorld : TEXCOORD6;
				fixed3 normalDir : TEXCOORD7;
				fixed3 tangentDir : TEXCOORD8;
				fixed3 bitangentDir : TEXCOORD9;
				UNITY_FOG_COORDS(10)
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0.xy, _WaterAlphaTexture);

				float2 UV = TRANSFORM_TEX(v.texcoord1.xy, _Normal_A_Texture);
				o.uv1.xy = UV;

				float NATU = fmod(_Time*_NormalA_Time_U, 1);
				float NATV = fmod(_Time*_NormalA_Time_V, 1);
				o.uv1.xy = (UV + float2(NATU, NATV)) * _Normal_A_Scale;

				float NBTU = fmod(_Time*_NormalB_Time_U, 1);
				float NBTV = fmod(_Time*_NormalB_Time_V, 1);
				o.uv2.xy = (UV + float2(NBTU, NBTV)) * _Normal_B_Scale;

				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, fixed4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = v.vertex.xyz;
				o.proj0 = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.proj0.z);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}


			fixed4 frag(VertexOutput i) : COLOR
			{
				fixed3 diffuseColor = _Color.rgb * _Border.x;
				fixed3 specular = fixed3(0, 0, 0);
				fixed4 _WaterAlpha = tex2D(_WaterAlphaTexture, i.uv0);
				fixed alpha = saturate(_WaterAlpha.rgb);

				i.normalDir = normalize(i.normalDir) + normalize(_LightDir.xyz);
				fixed3x3 tangentTransform = fixed3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);

				fixed3 _Normal_A = UnpackNormal(tex2D(_Normal_A_Texture, i.uv1));
				fixed3 _Normal_B = UnpackNormal(tex2D(_Normal_B_Texture, i.uv2));

				fixed3 normal = normalize(mul(_Normal_A.rgb * _Normal_B.rgb, tangentTransform));

				float3 ranges = _Border.xyz * alpha;
				ranges.y = lerp(ranges.y, ranges.y * ranges.y * ranges.y, 0.5);
				//反射
				i.proj0.xy += normal.xy + _Border.zw;
				fixed3 reflection = fixed3(1,1,1);

				float3 worldView = (i.worldPos - _WorldSpaceCameraPos);
				float3 worldNormal = normal.xzy;
				worldNormal.z = -worldNormal.z;
				float fresnel = sqrt(1.0 - dot(-normalize(worldView), worldNormal));
				fresnel *= fresnel * fresnel;
				fresnel = (0.8 * fresnel + 0.2);

				diffuseColor = diffuseColor + _BorderColor.rgb * _WaterAlpha.a * ranges.y;
				diffuseColor = lerp(reflection * diffuseColor, reflection, fresnel * 0.5);

				fixed3 lightdir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 floatDir = normalize(viewDir + lightdir);

				fixed gloss = _Roughness;
				fixed perceptualRoughness = 1.0 - _Roughness;
				fixed roughness = perceptualRoughness * perceptualRoughness;

				fixed3 specColor;
				fixed smoothness;
				diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, _Metallic, specColor, smoothness);

				smoothness = 1.0 - smoothness;
				fixed NL = saturate(dot(normal, lightdir));
				fixed LH = saturate(dot(lightdir, floatDir));
				fixed NV = abs(dot(normal, viewDir));
				fixed NH = saturate(dot(normal, floatDir));
				fixed V = SmithJointGGXVisibilityTerm(NL, NV, roughness);
				fixed D = GGXTerm(NH, roughness);
				fixed specularTerm = V*D * UNITY_PI;
				specularTerm = sqrt(max(1e-4h, specularTerm));

				specular = _LightColor0.xyz*specularTerm*FresnelTerm(specColor, LH);
				UnityLight light;
				light.color = lightColor;
				light.dir = lightdir;

				Unity_GlossyEnvironmentData ugls_en_data;
				ugls_en_data.roughness = 1.0 - gloss;
				ugls_en_data.reflUVW = reflect(-viewDir, normal);

				fixed3 indirectSpecular = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, ugls_en_data);
				specular = (specular + indirectSpecular)* specColor;

				fixed WAWP = alpha * _WaterAlphaPower;
				fixed CTP = (1.0 - WAWP)*_CausticsPower;
				fixed3 emissive = fixed3(CTP, CTP, CTP);
				fixed3 finalColor = fixed3(diffuseColor + specular * _LightColor + emissive);
				fixed Alpha = (CTP + WAWP)*_LightColor.a;
				fixed4 finalRGBA = fixed4(finalColor,Alpha);

				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}
			ENDCG
		}
	}

	SubShader
	{
		Lod 100
		Tags{ "IgnoreProjector" = "True" "Queue" = "Geometry" "RenderType" = "Transparent" }
		ColorMask RGB
		Pass
		{
			Name "FORWARD"
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#pragma multi_compile_fog

			#pragma multi_compile_fwdbase

			uniform fixed4 _Color;
			uniform fixed4 _LightColor;
			uniform fixed _WaterAlphaPower;
			uniform sampler2D _WaterAlphaTexture;
			uniform fixed4 _WaterAlphaTexture_ST;
			uniform fixed _CausticsPower;
			uniform half4 _Border;

			struct VertexInput
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 tangent : TANGENT;
				half2 texcoord0 : TEXCOORD0;
				half2 texcoord1 : TEXCOORD1;
				half4 texcoord2 : TEXCOORD2;
			};

			struct VertexOutput
			{
				half4 pos : SV_POSITION;
				half2 uv0 : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0.xy, _WaterAlphaTexture);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}


			fixed4 frag(VertexOutput i) : COLOR
			{
				fixed3 diffuseColor = _Color.rgb * _Border.x;
				fixed3 specular = fixed3(0, 0, 0);
				fixed4 _WaterAlpha = tex2D(_WaterAlphaTexture, i.uv0);
				fixed alpha = saturate(_WaterAlpha.rgb);

				fixed WAWP = alpha * _WaterAlphaPower;
				fixed CTP = (1.0 - WAWP)*_CausticsPower;
				fixed3 emissive = fixed3(CTP, CTP, CTP);
				fixed3 finalColor = fixed3(diffuseColor + specular * _LightColor + emissive);
				fixed Alpha = (CTP + WAWP)*_LightColor.a;
				fixed4 finalRGBA = fixed4(finalColor,Alpha);

				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}
			ENDCG
		}
	}
}
