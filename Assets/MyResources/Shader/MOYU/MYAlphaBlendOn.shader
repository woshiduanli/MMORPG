Shader "MOYU/AlphaBlendOn" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0,5)) = 1
	}

	SubShader 
	{
		Tags {"Queue"="Transparent"  "IgnoreProjector"="false"}
		Blend SrcAlpha OneMinusSrcAlpha
		Pass 
		{ 
			Tags{ "LightMode"="ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"

			struct appdata_lightmap 
			{
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				half2 texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
		
			struct v2f_vertex 
			{
				half4 pos : POSITION;
				half4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};
		
			sampler2D _MainTex;
			half4 _MainTex_ST;
			uniform sampler2D _AlphaTex;
			uniform half4 _AlphaTex_ST;
			uniform	fixed4 _Color;
			uniform half _Brightness;
			v2f_vertex vert (appdata_lightmap v) 
			{
				v2f_vertex o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv.xy = v.texcoord.xy;
				o.uv.zw = half2(0,0);
				#ifdef LIGHTMAP_ON
					o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
		
			fixed4 frag (v2f_vertex i) : COLOR
			{ 
				fixed alpha = tex2D(_AlphaTex, i.uv.xy).r;
				fixed4 c = tex2D(_MainTex, i.uv.xy) * _Color * _Brightness;
				c.a *= alpha;
				#ifdef LIGHTMAP_ON
					c.rgb *= DecodeLightmap (UNITY_SAMPLE_TEX2D (unity_Lightmap, i.uv.zw));
				#endif
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}

			ENDCG
 		}
	}
}