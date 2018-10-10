Shader "MOYU/GodRays" 
{
	Properties 
	{
		_MainTex ("Base texture", 2D) = "white" {}
		_FadeOutDistNear ("Near fadeout dist", float) = 10	
		_FadeOutDistFar ("Far fadeout dist", float) = 10000	
		_Multiplier("Multiplier", float) = 1
		_ContractionAmount("Near contraction amount", float) = 5
	}

	
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
		Blend One One
	//	Blend One OneMinusSrcColor
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
		CGINCLUDE	
		#include "UnityCG.cginc"
		sampler2D _MainTex;
	
		half _FadeOutDistNear;
		half _FadeOutDistFar;
		half _Multiplier;
		half _ContractionAmount;
	
	
		struct v2f 
		{
			half4	pos	: SV_POSITION;
			half2	uv		: TEXCOORD0;
			fixed4	color	: TEXCOORD1;
		};

		half4 MyLerp(half4 from,half4 to,half t)
		{
			return from + t * (to - from);
		}
	
		v2f vert (appdata_full v)
		{
			v2f 		o;
			half3	viewPos	        = UnityObjectToViewPos(v.vertex);
			half		dist		= length(viewPos);
			half		nfadeout	= saturate(dist / _FadeOutDistNear);
			half		ffadeout	= 1 - saturate(max(dist - _FadeOutDistFar,0) * 0.2);
		
			ffadeout *= ffadeout;
		
			nfadeout *= nfadeout;
			nfadeout *= nfadeout;
		
			nfadeout *= ffadeout;
		
			half4 vpos = v.vertex;
		
			vpos.xyz -=   v.normal * saturate(1 - nfadeout) * v.color.a * _ContractionAmount;
						
			o.uv		= v.texcoord.xy;
			o.pos	= UnityObjectToClipPos(vpos);
			o.color	= nfadeout * v.color * _Multiplier;
						
			return o;
		}
		ENDCG


		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest		
			fixed4 frag (v2f i) : COLOR
			{			
				return tex2D (_MainTex, i.uv.xy) * i.color;
			}
			ENDCG 
		}	
	}


}

