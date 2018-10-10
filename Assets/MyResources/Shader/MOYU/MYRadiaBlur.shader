Shader "MOYU/radialBlur" 
{ 
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TimeX ("Time", Range(0.0, 1.0)) = 1.0
		_ScreenResolution ("_ScreenResolution", Vector) = (0.,0.,0.,0.)
	}

	SubShader
	{
		Tags{"Queue"="Geometry" "IgnoreProjector"="True"}
		Pass
		{
			ZTest Always

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#pragma glsl
			#include "UnityCG.cginc"
			uniform sampler2D _MainTex;
			uniform half _TimeX;
			uniform half _Value;
			uniform half _Value2;
			uniform half _Value3;
			uniform half4 _ScreenResolution;

			struct appdata_t
			{
				half4 vertex   : POSITION;
				fixed4 color    : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				half2 texcoord  : TEXCOORD0;
				half4 vertex   : POSITION;
				fixed4 color    : COLOR;
			};

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				return OUT;
			}

			fixed4 frag (v2f i) : COLOR
			{
				half2 center = half2(_Value2,_Value3);
				half2 uv = i.texcoord.xy;
				uv -= center;
				fixed4 color = fixed4(0.0,0.0,0.0,0.0);
				half scale;
				_Value *= 0.15;
				scale = 1 + (half(0*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(1*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(2*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(3*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(4*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(5*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(6*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				scale = 1 + (half(7*_Value));
				color += tex2D(_MainTex, uv * scale + center);
				color *= 0.125;
				return  color;
			}
			ENDCG
		}
	}
}
