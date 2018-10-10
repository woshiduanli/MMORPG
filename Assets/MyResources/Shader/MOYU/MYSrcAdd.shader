Shader "MOYU/Particles/SrcAdd" 
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Value ("Value", float) = 2.0
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True"}

		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		Lighting Off
		Fog { Mode Off }
		ZWrite Off
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
			    fixed _Cutoff;

				struct appdata_t 
				{
					half4 vertex : POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
				};

				struct v2f 
				{
					half4 vertex : POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
				};
			
				v2f vert (appdata_t v)
				{
					v2f o;

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
					clip( col.a - _Cutoff );
					return col;
				}

				ENDCG 
			}
		}	
	}
}
