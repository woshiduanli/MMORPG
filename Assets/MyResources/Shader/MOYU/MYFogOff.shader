Shader "MOYU/Fogoff" 
{
	Properties
	{
		_MainTex("Particle Texture", 2D) = "white" { }
		_Color("Color", Color) = (1,1,1,1)
		_Alpha("Alpha",	float) = 0.0
	}
	SubShader
	{
		Tags{ "QUEUE" = "Transparent" "IGNOREPROJECTOR" = "true" }
		Pass
		{
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float4 _MainTex_ST;
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform fixed _Alpha;

			struct appdata 
			{
				float3 pos : POSITION;
				float3 uv0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f 
			{
				float2 uv0 : TEXCOORD0;
				float4 pos : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata IN) 
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uv0 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.pos = UnityObjectToClipPos(IN.pos);
				return o;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, IN.uv0.xy)* _Color * _Alpha;
				return col;
			}
			ENDCG
		}
	}
}