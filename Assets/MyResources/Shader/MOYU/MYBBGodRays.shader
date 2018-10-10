Shader "MOYU/BBGodRays" 
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
		_VerticalBillboarding("Vertical billboarding amount", Range(0,1)) = 1
		_ViewerOffset("Viewer offset", float) = 0
		_Color("Color", Color) = (1,1,1,1)
	}

	
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
		Blend One One
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
		half _VerticalBillboarding;
		half _ViewerOffset;
		half4 _Color;
	
	
		struct v2f 
		{
			half4	pos	: SV_POSITION;
			half2	uv		: TEXCOORD0;
			fixed4	color	: TEXCOORD1;
		};

		void CalcOrthonormalBasis(half3 dir,out half3 right,out half3 up)
		{
			up 	= abs(dir.y) > 0.999f ? half3(0,0,1) : half3(0,1,0);		
			right = normalize(cross(up,dir));		
			up	= cross(dir,right);	
		}

		half CalcFadeOutFactor(half dist)
		{
			half		nfadeout	= saturate(dist / _FadeOutDistNear);
			half		ffadeout	= 1 - saturate(max(dist - _FadeOutDistFar,0) * 0.2);

			ffadeout *= ffadeout;
		
			nfadeout *= nfadeout;
			nfadeout *= nfadeout;
		
			nfadeout *= ffadeout;

			return nfadeout;
		}
	
		half CalcDistScale(half dist)
		{
			half		distScale	= min(max(dist - _SizeGrowStartDist,0) / _SizeGrowEndDist,1);
		
			return distScale * distScale * _MaxGrowSize;
		}
	
	
		v2f vert (appdata_full v)
		{
			v2f o;
			#if 0
			// cheap view space billboarding
			half3	centerOffs		= half3(half(0.5).xx - v.color.rg,0) * v.texcoord1.xyy;
			half3	BBCenter		= v.vertex + centerOffs.xyz;	
			half3	viewPos			= UnityObjectToViewPos(half4(BBCenter,1)) - centerOffs;
			#else
		
			half3	centerOffs		= half3(half(0.5).xx - v.color.rg,0) * v.texcoord1.xyy;
			half3	centerLocal	= v.vertex.xyz + centerOffs.xyz;
			half3	viewerLocal	= mul(unity_WorldToObject,half4(_WorldSpaceCameraPos,1));			
			half3	localDir			= viewerLocal - centerLocal;
				
			localDir[1] = lerp(0,localDir[1],_VerticalBillboarding);
		
			half		localDirLength=length(localDir);
			half3	rightLocal;
			half3	upLocal;
		
			CalcOrthonormalBasis(localDir / localDirLength,rightLocal,upLocal);

			half		distScale		= CalcDistScale(localDirLength) * v.color.a;		
			half3	BBNormal		= rightLocal * v.normal.x + upLocal * v.normal.y;
			half3	BBLocalPos	= centerLocal - (rightLocal * centerOffs.x + upLocal * centerOffs.y) + BBNormal * distScale;

			BBLocalPos += _ViewerOffset * localDir;

			#endif
		
			half		time 			= _Time.y + _BlinkingTimeOffsScale * v.color.b;				
			half		fracTime	= fmod(time,_TimeOnDuration + _TimeOffDuration);
			half		wave			= smoothstep(0,_TimeOnDuration * 0.25,fracTime)  * (1 - smoothstep(_TimeOnDuration * 0.75,_TimeOnDuration,fracTime));
			half		noiseTime	= time *  (6.2831853f / _TimeOnDuration);
			half		noise			= sin(noiseTime) * (0.5f * cos(noiseTime * 0.6366f + 56.7272f) + 0.5f);
			half		noiseWave	= _NoiseAmount * noise + (1 - _NoiseAmount);
	
			wave = _NoiseAmount < 0.01f ? wave : noiseWave;
			wave += _Bias;
		
			o.uv		= v.texcoord.xy;
			o.pos	= UnityObjectToClipPos(half4(BBLocalPos,1));
			o.color	= CalcFadeOutFactor(localDirLength) * _Color * _Multiplier * wave;
						
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

