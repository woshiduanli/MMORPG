Shader "MOYU/Wind" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_Wind("Wind params",Vector) = (1,1,1,1)
	_WindEdgeFlutter("Wind edge fultter factor", float) = 0.5
	_WindEdgeFlutterFreqScale("Wind edge fultter freq scale",float) = 0.5
}

SubShader {
	Tags {"Queue"="Transparent" "RenderType"="Transparent" "LightMode"="ForwardBase"}
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off ZWrite Off
	
	
	CGINCLUDE
	#include "UnityCG.cginc"
	#include "TerrainEngine.cginc"
	sampler2D _MainTex;
	float4 _MainTex_ST;
	samplerCUBE _ReflTex;
	
	#ifndef LIGHTMAP_OFF
	// half4 unity_LightmapST;
	// sampler2D unity_Lightmap;
	#endif
	
	half _WindEdgeFlutter;
	half _WindEdgeFlutterFreqScale;

	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		#ifndef LIGHTMAP_OFF
		half2 lmap : TEXCOORD1;
		#endif
		fixed3 spec : TEXCOORD2;
	};

inline half4 AnimateVertex2(half4 pos, half3 normal, half4 animParams,half4 wind,half2 time)
{	
	// animParams stored in color
	// animParams.x = branch phase
	// animParams.y = edge flutter factor
	// animParams.z = primary factor
	// animParams.w = secondary factor

	half fDetailAmp = 0.1f;
	half fBranchAmp = 0.3f;
	
	// Phases (object, vertex, branch)
	half fObjPhase = dot(unity_ObjectToWorld[3].xyz, 1);
	half fBranchPhase = fObjPhase + animParams.x;
	
	half fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
	
	// x is used for edges; y is used for branches
	half2 vWavesIn = time  + half2(fVtxPhase, fBranchPhase );
	
	// 1.975, 0.793, 0.375, 0.193 are good frequencies
	half4 vWaves = (frac( vWavesIn.xxyy * half4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);
	
	vWaves = SmoothTriangleWave( vWaves );
	half2 vWavesSum = vWaves.xz + vWaves.yw;

	// Edge (xz) and branch bending (y)
	half3 bend = animParams.y * fDetailAmp * normal.xyz;
	bend.y = animParams.w * fBranchAmp;
	pos.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * animParams.w)) * wind.w; 

	// Primary bending
	// Displace position
	pos.xyz += animParams.z * wind.xyz;
	
	return pos;
}


	
	v2f vert (appdata_full v)
	{
		v2f o;
		
		half4	wind;
		
		half			bendingFact	= v.color.a;
		
		wind.xyz	= mul((half3x3)unity_WorldToObject,_Wind.xyz);
		wind.w		= _Wind.w  * bendingFact;
		
		
		half4	windParams	= half4(0,_WindEdgeFlutter,bendingFact.xx);
		half 		windTime 		= _Time.y * half2(_WindEdgeFlutterFreqScale,1);
		half4	mdlPos			= AnimateVertex2(v.vertex,v.normal,windParams,wind,windTime);
		
		o.pos = UnityObjectToClipPos(mdlPos);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		
		o.spec = v.color;
		
		#ifndef LIGHTMAP_OFF
		o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
		
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma debug
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 tex = tex2D (_MainTex, i.uv);
			
			fixed4 c;
			c.rgb = tex.rgb;
			c.a = tex.a;
			
			#ifndef LIGHTMAP_OFF
			fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
			c.rgb *= lm;
			#endif
			
			return c;
		}
		ENDCG 
	}	
}
}


