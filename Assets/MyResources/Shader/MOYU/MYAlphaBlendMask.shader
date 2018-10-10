Shader "MOYU/Alpha Blended Mask" 
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Mask ("Mask ( R Channel )", 2D) = "white" {}
		[HideInInspector]_Center ("Center",Vector) = (0,0,0,1)
		[HideInInspector]_Scale ("Scale",Vector) = (1,1,1,1)
		[HideInInspector]_Normal ("Normal",Vector) = (0,0,1,0)
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off

		SubShader 
		{
			Pass 
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile SCALE_OFF SCALE_ON
				#pragma multi_compile MIRROR_OFF MIRROR_ON
				#pragma multi_compile MESH BILLBOARD
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _Mask;
				fixed4 _TintColor;
			
				struct appdata_t 
				{
					half4 vertex : POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f 
				{
					half4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					half2 texcoordMask : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};
			
				half4 _MainTex_ST;
				half4 _Mask_ST;

				half4 _Center;
				half4 _Scale;
				half4 _Normal;

				uniform half4x4 _Camera2World;

				v2f vert (appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					#if SCALE_ON
					half4 worldpos;
					#if BILLBOARD
					worldpos = mul(_Camera2World,v.vertex);
					#else //BILLBOARD
					worldpos = mul(unity_ObjectToWorld,v.vertex);		
					#endif //BILLBOARD
					#if MIRROR_ON
					half3 srcDir = _Center.xyz - worldpos.xyz;
					half3 refDir = reflect(srcDir,_Normal.xyz);
					refDir.y = -srcDir.y;
					worldpos.xyz = refDir *_Scale.xyz + _Center.xyz;
					#else //MIRROR_ON 
					worldpos.xyz = (worldpos.xyz-_Center.xyz)*_Scale.xyz + _Center.xyz;
					#endif //MIRROR_ON 
					o.vertex = mul(UNITY_MATRIX_VP, worldpos);
					#else //SCALE_ON
					 o.vertex = UnityObjectToClipPos(v.vertex);
					#endif //SCALE_ON

					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					o.texcoordMask = TRANSFORM_TEX(v.texcoord,_Mask);

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 c = tex2D(_MainTex, i.texcoord);
					c.a *= tex2D(_Mask, i.texcoordMask).r;
					return 2.0f * _TintColor * c;
				}
				ENDCG 
			}
		}
	}
}
