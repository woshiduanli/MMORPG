Shader "MOYU/DistortionEX" 
{
	Properties 
	{
		_TintColor ("Main Color", Color) = (1,1,1,1)
		_RimColor("Rim Color", Color) = (1,1,1,0.5)
		_NoiseTex("Noise Texture",2D) = "white"{}
		_Alpha("AlphaTexture", 2D) = "white" {}
		_HeatForce("Heat Force",range(0,2)) = 0.1
		_HeatTime("Heat Time", range(0,1.5)) = 1

		_FPOW("FPOW Fresnel", Float) = 5.0
		_R0("R0 Fresnel", Float) = -0.44
	}
	SubShader 
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Back
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DISTORT_ON DISTORT_OFF

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			uniform sampler2D _NoiseTex;
			uniform float4 _NoiseTex_ST;
			uniform sampler2D _GrabTextureMobile;

			uniform half  _GrabTextureMobileScale;
			uniform float4 _GrabTextureMobile_TexelSize;
			uniform half4 _TintColor;
			uniform half4 _RimColor;

			uniform sampler2D _Alpha;
			uniform half  _HeatForce;
			uniform half _HeatTime;

			uniform half _FPOW;
			uniform half _R0;
			
			struct appdata_t 
			{
				float4 vertex : POSITION;
				#if DISTORT_ON
				float3 normal : NORMAL;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				#endif
			};

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				#if DISTORT_ON
				half2 uv : TEXCOORD0;
				float4 uvgrab : TEXCOORD1;
				half fresnel : TEXCOORD2;
				half4 color : COLOR;
				#endif
			};

			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#if DISTORT_OFF
				return o;
				#else
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*_ProjectionParams.x) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.w;
				#if UNITY_SINGLE_PASS_STEREO
				o.uvgrab.xy = TransformStereoScreenSpaceTex(o.uvgrab.xy, o.uvgrab.w);
				#endif
				o.uvgrab.z /= distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex));

				o.uv = TRANSFORM_TEX(v.texcoord, _NoiseTex);
				o.color = v.color;
				o.color.rgb *= _LightColor0.rgb * _LightColor0.w;
				o.fresnel = (1 - dot(normalize(v.normal), normalize(ObjSpaceViewDir(v.vertex))));
				o.fresnel = pow(o.fresnel, _FPOW);
				o.fresnel = saturate(_R0 + (1.0 - _R0) * o.fresnel);

				return o;
				#endif
			}

			fixed4 frag (v2f i) : SV_Target
			{ 
				#if DISTORT_OFF
				discard;
				return 0;
				#else
				fixed4 offestcol1 = tex2D(_NoiseTex, i.uv + _Time.xz*_HeatTime);
				fixed4 offestcol2 = tex2D(_NoiseTex, i.uv - _Time.yx*_HeatTime);
				half2 offset;
				offset.x = ((offestcol1.r + offestcol2.r) - 1) * _HeatForce;
				offset.y = ((offestcol1.g + offestcol2.g) - 1) * _HeatForce;

				offset = offset.xy * 400 * _GrabTextureMobile_TexelSize.xy * _GrabTextureMobileScale;
				i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
				fixed4 col = tex2Dproj(_GrabTextureMobile, UNITY_PROJ_COORD(i.uvgrab));
				
				fixed3 alpha = tex2D(_Alpha, i.uv).rgb;
				fixed value = saturate(alpha);
				fixed3 emission = col.xyz * _TintColor.xyz + col.rgb * i.fresnel *_RimColor * _RimColor.a * 3;
				return fixed4 (emission, _TintColor.a *	value * i.color.a);
				#endif
			}
			ENDCG 
		}
	}	
}