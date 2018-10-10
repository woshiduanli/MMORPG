Shader "MOYU/PBR/Water" 
{
    Properties 
	{
		_LightColor("LightColor", Color) = (1,1,1,1)
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Roughness ("Roughness", Range(0, 1)) = 0.7503998

        [NoScaleOffset]_WaterAlphaTexture ("WaterAlphaTexture", 2D) = "white" {}
		_WaterAlphaPower("WaterAlphaPower", Range(0, 1)) = 0.7863253
        [NoScaleOffset]_WaterColorTexture ("WaterColorTexture", 2D) = "white" {}

        [NoScaleOffset]_Normal_A_Texture ("Normal_A_Texture", 2D) = "bump" {}
        _Normal_A_Scale ("Normal_A_Scale(int)", int ) = 10
        _NormalA_Time_U ("NormalA_Time_U", Float ) = 0
        _NormalA_Time_V ("NormalA_Time_V", Float ) = 0

        [NoScaleOffset]_Normal_B_Texture ("Normal_B_Texture", 2D) = "bump" {}
        _Normal_B_Scale ("Normal_B_Scale(int)", int ) = 10
        _NormalB_Time_U ("NormalB_Time_U", Float ) = 0
        _NormalB_Time_V ("NormalB_Time_V", Float ) = 0
        _CausticsPower ("CausticsPower", Range(0, 1)) = 1

		_LightDir("Light Dir",Vector) = (1,1,1,1)
    }
    SubShader 
	{
		Lod 200
		Tags{ "IgnoreProjector" = "True" "Queue" = "Geometry+10" "RenderType" = "Transparent" }
        Pass 
		{
            Name "FORWARD"
            Tags {"LightMode"="ForwardBase"}
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

			uniform fixed4 _LightColor;
            uniform fixed _Metallic;
            uniform fixed _Roughness;
			uniform fixed _WaterAlphaPower;

            uniform sampler2D _WaterAlphaTexture;
			uniform fixed4 _WaterAlphaTexture_ST;
            uniform sampler2D _WaterColorTexture;
			uniform fixed4 _WaterColorTexture_ST;
			  
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
			uniform float4 _LightDir;

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
                float2 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
				fixed3 normalDir : TEXCOORD4;
				fixed3 tangentDir : TEXCOORD5;
				fixed3 bitangentDir : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };

            VertexOutput vert (VertexInput v) 
			{
				VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex );
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0.xy, _WaterColorTexture);

				float2 UV = TRANSFORM_TEX(v.texcoord1.xy, _Normal_A_Texture);
				o.uv1.xy = UV;

				float NATU = fmod(_Time*_NormalA_Time_U, 1);
				float NATV = fmod(_Time*_NormalA_Time_V, 1);
				o.uv1.xy = (UV + float2(NATU, NATV)) * _Normal_A_Scale;

				float NBTU = fmod(_Time*_NormalB_Time_U, 1);
				float NBTV = fmod(_Time*_NormalB_Time_V, 1);
				o.uv2.xy = (UV + float2(NBTU, NBTV)) * _Normal_B_Scale;

                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, fixed4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }


			fixed4 frag(VertexOutput i) : COLOR 
			{ 
				fixed4 _MainTex = tex2D(_WaterColorTexture , i.uv0);
				fixed3 Color = _MainTex.rgb;
				fixed3 diffuseColor = Color;
				fixed3 specular = fixed3(0, 0, 0);
				
				i.normalDir = normalize(i.normalDir ) + normalize(_LightDir.xyz);
				fixed3x3 tangentTransform = fixed3x3( i.tangentDir, i.bitangentDir, i.normalDir);
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz );

				fixed3 _Normal_A = UnpackNormal(tex2D(_Normal_A_Texture, i.uv1)) ;
				fixed3 _Normal_B = UnpackNormal(tex2D(_Normal_B_Texture, i.uv2));

				fixed3 normal = normalize(mul(_Normal_A.rgb * _Normal_B.rgb, tangentTransform ));
				fixed3 lightdir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 floatDir = normalize(viewDir + lightdir);

				fixed gloss = _Roughness;
				fixed perceptualRoughness = 1.0 - _Roughness;
				fixed roughness = perceptualRoughness * perceptualRoughness;

				fixed3 specColor;
				fixed smoothness;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specColor, smoothness);

				smoothness = 1.0 - smoothness;
				fixed NL = saturate(dot(normal, lightdir));
				fixed LH = saturate(dot(lightdir, floatDir));
				fixed NV = abs(dot(normal, viewDir));
				fixed NH = saturate(dot(normal, floatDir));
				fixed V = SmithJointGGXVisibilityTerm(NL, NV, roughness );
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


				fixed4 _WaterAlphaTexture_var = tex2D(_WaterAlphaTexture, i.uv0);
				fixed WaterAlpha = _WaterAlphaTexture_var.r;
				fixed WAWP = WaterAlpha*_WaterAlphaPower;
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
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			uniform fixed4 _LightColor;
			uniform fixed _WaterAlphaPower;
			uniform sampler2D _WaterAlphaTexture;
			uniform fixed4 _WaterAlphaTexture_ST;
			uniform sampler2D _WaterColorTexture;
			uniform fixed4 _WaterColorTexture_ST;
			uniform fixed _CausticsPower;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord0 : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0.xy, _WaterColorTexture);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(VertexOutput i) : COLOR
			{
				fixed4 _MainTex = tex2D(_WaterColorTexture , i.uv0);
				fixed3 Color = _MainTex.rgb;
				fixed3 diffuseColor = Color;
				fixed3 specular = fixed3(0, 0, 0);

				fixed4 _WaterAlphaTexture_var = tex2D(_WaterAlphaTexture, i.uv0);
				fixed WaterAlpha = _WaterAlphaTexture_var.r;
				fixed WAWP = WaterAlpha * _WaterAlphaPower;
				fixed CTP = (1.0 - WAWP) * _CausticsPower;
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
