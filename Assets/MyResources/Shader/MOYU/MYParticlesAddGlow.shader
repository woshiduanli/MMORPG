Shader "MOYU/Particles/AdditiveGlow" 
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_AtmoColor("Atmosphere Color", Color) = (0, 0.4, 1.0, 1)
		_Size("Size", Float) = 0.1
		_OutLightPow("Falloff",Float) = 5
		_OutLightStrength("Transparency", Float) = 15
		_Value ("Value", float) = 2.0
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True"}
		ColorMask RGB
		Blend SrcAlpha One
		Cull Off 
		Lighting Off
		ZWrite Off
		Fog { Mode Off }

		SubShader 
		{
			Pass 
			{
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half4 _MainTex_ST;
				fixed4 _TintColor;
				half _Value;
			
				struct appdata_t 
				{
					half4 vertex : POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f 
				{
					half4 vertex : POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					UNITY_VERTEX_OUTPUT_STEREO
				};
			
				v2f vert (appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX (v.texcoord, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
			
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = _Value * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
					UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
					return col;
				}

				ENDCG 
			}

			Pass
			{
				Name "AtmosphereBase"
				Tags{ "LightMode" = "Always" }
				Cull Front
				Blend SrcAlpha One

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				uniform fixed4 _Color;
				uniform fixed4 _AtmoColor;
				uniform half _Size;
				uniform half _OutLightPow;
				uniform half _OutLightStrength;

				struct vertexOutput
				{
					half4 pos:SV_POSITION;
					half3 normal:TEXCOORD0;
					half3 worldvertpos:TEXCOORD1;
				};

				vertexOutput vert(appdata_base v)
				{
					vertexOutput o;
					v.vertex.xyz += v.normal*_Size;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.normal = v.normal;
					o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				fixed4 frag(vertexOutput i) :COLOR
				{
					i.normal = normalize(i.normal);
					half3 viewdir = normalize(i.worldvertpos.xyz - _WorldSpaceCameraPos.xyz);
					fixed4 color = _AtmoColor;
					color.a = pow(saturate(dot(viewdir, i.normal)), _OutLightPow);
					color.a *= _OutLightStrength*dot(viewdir, i.normal);
					return color;
				}
				ENDCG
			}
		}	
	}
}
