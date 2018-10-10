Shader "MOYU/Blinking GodRays" 
{
	Properties 
	{
		_MainTex ("Base texture", 2D) = "white" {}
		_FadeOutDistNear ("Near fadeout dist", float) = 10	
		_FadeOutDistFar ("Far fadeout dist", float) = 10000	
		_Multiplier("Color multiplier", float) = 1
		_Bias("Bias",float) = 0
		_TimeOnDuration("ON duration",float) = 0.5
		_TimeOffDuration("OFF duration",float) = 0.5
		_BlinkingTimeOffsScale("Blinking time offset scale (seconds)",float) = 5
		_SizeGrowStartDist("Size grow start dist",float) = 5
		_SizeGrowEndDist("Size grow end dist",float) = 50
		_MaxGrowSize("Max grow size",float) = 2.5
		_NoiseAmount("Noise amount (when zero, pulse wave is used)", Range(0,0.5)) = 0
		_Color("Color", Color) = (1,1,1,1)
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
		half	_Bias;
		half _TimeOnDuration;
		half	_TimeOffDuration;
		half _BlinkingTimeOffsScale;
		half _SizeGrowStartDist;
		half _SizeGrowEndDist;
		half _MaxGrowSize;
		half _NoiseAmount;
		half4 _Color;
	
	
		struct v2f 
		{
			half4	pos	: SV_POSITION;
			half2	uv		: TEXCOORD0;
			fixed4	color	: TEXCOORD1;
		};

	
		v2f vert (appdata_full v)
		{
			v2f 		o;
		
			half		time 			= _Time.y + _BlinkingTimeOffsScale * v.color.b;		
			half3	viewPos		= UnityObjectToViewPos(v.vertex);
			half		dist			= length(viewPos);
			half		nfadeout	= saturate(dist / _FadeOutDistNear);
			half		ffadeout	= 1 - saturate(max(dist - _FadeOutDistFar,0) * 0.2);
			half		fracTime	= fmod(time,_TimeOnDuration + _TimeOffDuration);
			half		wave			= smoothstep(0,_TimeOnDuration * 0.25,fracTime)  * (1 - smoothstep(_TimeOnDuration * 0.75,_TimeOnDuration,fracTime));
			half		noiseTime	= time *  (6.2831853f / _TimeOnDuration);
			half		noise			= sin(noiseTime) * (0.5f * cos(noiseTime * 0.6366f + 56.7272f) + 0.5f);
			half		noiseWave	= _NoiseAmount * noise + (1 - _NoiseAmount);
			half		distScale	= min(max(dist - _SizeGrowStartDist,0) / _SizeGrowEndDist,1);
		
			
			wave = _NoiseAmount < 0.01f ? wave : noiseWave;
		
			distScale = distScale * distScale * _MaxGrowSize * v.color.a;
		
			wave += _Bias;
		
			ffadeout *= ffadeout;
		
			nfadeout *= nfadeout;
			nfadeout *= nfadeout;
		
			nfadeout *= ffadeout;
		
			half4	mdlPos = v.vertex;
		
			mdlPos.xyz += distScale * v.normal;
				
			o.uv		= v.texcoord.xy;
			o.pos	= UnityObjectToClipPos(mdlPos);
			o.color	= nfadeout * _Color * _Multiplier * wave;
						
			return o;
		}
		ENDCG


		Pass 
		{
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

