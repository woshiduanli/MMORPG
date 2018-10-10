Shader "MOYU/Player" 
{
	Properties 
	{
	    _Color ("Main Color", Color) = (0,0,0,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_UVColor("UVColor", Color) = (0.5,0.5,0.5,0)
		_UVMainTex("UVMainTex", 2D) = "white" {}
		_UVMask("UVMask", 2D) = "white" {}

		_UVXSpeed("UVXSpeed", Range(0, 8)) = 2
		_UVYSpeed("UVYSpeed", Range(0, 8)) = 0
		_UVDir("UVDir", Range(-1, 1)) = -1
	}

	 SubShader
	 {
		 Tags{ "Queue" = "Geometry+20" "IgnoreProjector" = "True" }
		 Pass
		 {
			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_fwdbase

			#include "HLSLSupport.cginc"
			#include "UnityCG.cginc"

			 uniform sampler2D _MainTex;
			 uniform sampler2D _MatCap;
			 uniform half _MatCapPower;

			 uniform half4 _Color;
			 uniform half4 _MainTex_ST;


			 uniform sampler2D _UVMainTex;
			 uniform half4 _UVMainTex_ST;
			 uniform sampler2D _UVMask;
			 uniform half4 _UVMask_ST;
			 uniform half4 _UVColor;

			 uniform half _UVXSpeed;
			 uniform half _UVYSpeed;
			 uniform half _UVDir;
			 uniform half3 _RimColor;

			 struct v2f_surf
			 {
				 half4 pos : SV_POSITION;
				 half4 uv : TEXCOORD0;
				 half3 normal : TEXCOORD1;
				 half3 viewDir : TEXCOORD2;
				 half4 uv1 : TEXCOORD3;
			 };

			 v2f_surf vert_surf(appdata_full v)
			 {
				 v2f_surf o = (v2f_surf)0;

				 o.pos = UnityObjectToClipPos(v.vertex);
				 o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				 half3 worldN = UnityObjectToWorldNormal(v.normal);
				 o.normal = worldN;
				 o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex).xyz));
				 o.uv1.xy = TRANSFORM_TEX(v.texcoord, _UVMainTex);
				 o.uv1.zw = half2(_UVXSpeed, _UVYSpeed)* _Time.y * _UVDir + o.pos.xy * _UVMask_ST.xy;
				 return o;
			 }

			 fixed4 frag_surf(v2f_surf IN) : SV_Target
			 {
				 fixed4 c = tex2D(_MainTex, IN.uv.xy)*1.15f;

				 c.rgb *= _Color.rgb;
				 c.rgb += 2.0f * tex2D(_UVMainTex, IN.uv1.xy).rgb * tex2D(_UVMask, IN.uv1.zw).rgb * _UVColor * _UVColor.a;
				 c.rgb += _RimColor * saturate(1 - saturate(dot(IN.normal, IN.viewDir)) * 1.8) * _MatCapPower * 2;
				 return c;
			 }
			 ENDCG
		}
	 }
}
