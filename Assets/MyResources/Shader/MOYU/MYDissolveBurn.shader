Shader "MOYU/DissolveBurn" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (0.7,0.7,0.7,1)
		_Amount ("Amount", Range (0, 1)) = 0.5
		_StartAmount("StartAmount", float) = 0.1
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_AlphaTex("AlphaTex",2D) = "white"{}
		_DissolveSrc ("DissolveSrc", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}

	SubShader 
	{ 
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="true"}
		CGPROGRAM
		
		
		#pragma target 3.0
		#pragma surface surf BasicDiffuse nolightmap nodirlightmap noforwardadd alphatest:_Cutoff

		sampler2D _MainTex;
		sampler2D _AlphaTex;
		sampler2D _DissolveSrc;

		fixed4 _Color;
		half _Amount;
		static fixed3 Color = fixed3(1,1,1);
		half _StartAmount;
		
		struct Input 
		{
			half2 uv_MainTex;
			half2 uvDissolveSrc;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb*0.5f;
	
			half ClipTex = tex2D (_DissolveSrc, IN.uv_MainTex).r ;
			half ClipAmount = ClipTex - _Amount;
			half Clip = 0;
			if (_Amount > 0)
			{
				if (ClipAmount <0)
				{
					Clip = 1;
				}
				else
				{
					if (ClipAmount < _StartAmount)
					{
						Color.x = ClipAmount/_StartAmount;
						Color.y = ClipAmount/_StartAmount;
						Color.z = ClipAmount/_StartAmount;
						o.Albedo  = (o.Albedo *((Color.x+Color.y+Color.z))* Color*((Color.x+Color.y+Color.z)));
					}
				}
			 }
			fixed a = tex2D(_AlphaTex, IN.uv_MainTex).r * tex.a * _Color.a; 
			o.Alpha = a ;
			if (Clip == 1)
				clip(-0.1);
		}
		
		inline fixed4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 col;
			col.rgb = s.Albedo;
			col.a = s.Alpha;
			return col;
		}

		ENDCG
	}
}
