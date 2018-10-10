Shader "MOYU/UVMove" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (0,0,0,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_offsetx ("Scrollx Speed", float) = 0.99
		_offsety ("Scrolly Speed", float) = 0.99
		_Value ("Value", float) = 2.0
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True"}
		Blend SrcAlpha One
		AlphaTest Greater .01
		Cull Off 
		Lighting Off
		ZWrite Off

		SubShader 
		{
			Pass 
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#pragma multi_compile_particles
				#include "UnityCG.cginc"

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

				uniform sampler2D _MainTex;
				uniform fixed4 _Color;
				uniform half4 _MainTex_ST;
				uniform half _offsetx,_offsety;
				uniform half _Value;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = half4(v.texcoord.x + _offsetx * _Time.x , v.texcoord.y + _offsety * _Time.x, 0, 1);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
			
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = _Value * i.color * _Color * tex2D(_MainTex, i.texcoord);
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		} 
	}
}