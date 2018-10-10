Shader "MOYU/Specular"
{
	Properties 
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_FallOff("FallOff", 2D) = "white" {}
		_FallOffPower("FallOffPower", Range(0,1) ) = 1
		_specTex("Specular", 2D) = "black" {}
		_specPower("specPower", Range(0,1) ) = 1

		_specMultiple("Specular Multiple", Float) = 1
		_shininess("Shininess", Range(0,1) ) = 0.044
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
		_CubePower("CubePower", Range(0,10) ) = 1
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)

		_FalloffSampler ("Falloff Control", 2D) = "white" {}
		_Falloff ("FallOff Power", Range(0,30)) = 1
		_RimLightSampler ("RimLight Control", 2D) = "white" {}
		_lightPower("light Power", Range(0,30)) = 0.5
	}
	
	SubShader 
	{
		Tags{"Queue"="Geometry" "IgnoreProjector"="true" "RenderType"="Opaque"}
		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGBA

		CGPROGRAM
        #pragma surface surf BlinnPhongEditor
        #pragma target 3.0

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _FallOff;
		sampler2D _specTex;
		half _specPower;
		half _specMultiple;
		half _shininess;
		half _FallOffPower;
		half _CubePower;
		samplerCUBE _Cube;
		fixed4 _ReflectColor;

		uniform fixed _Falloff;
		uniform fixed _lightPower;

		uniform sampler2D _FalloffSampler;
		uniform sampler2D _RimLightSampler;

		struct Input 
		{
			half2 uv_MainTex;
			half2 uv_specTex;
			half3 worldRefl;
		};

		struct EditorSurfaceOutput 
		{
			fixed3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
			half SAlpha;
		};
			
		inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize (lightDir + viewDir);
			half diff = max (0, dot ( lightDir, s.Normal ));
			half nh = max (0, dot (s.Normal, h));
			half spec = pow (nh, s.Specular * 256);
				
			half4 res;
			res.rgb = _LightColor0.rgb * diff ;
			res.a = saturate( spec * Luminance (_LightColor0.rgb));
			res *= atten;

			half4 c;
			c.rgb = (res.rgb * s.Albedo  + res.rgb * s.Specular * s.Gloss * res.a * s.SAlpha) ;

			half normalDotEye = dot( s.Normal, viewDir );
			half falloffU = clamp( 1 - abs( normalDotEye ), 0.02, 0.98 );
			half4 falloffSamplerColor = _Falloff * tex2D( _FalloffSampler, half2( falloffU, 0.25f ) );
			half3 combinedColor = lerp( c.rgb, falloffSamplerColor.rgb * c.rgb, falloffSamplerColor.a );
			half rimlightDot = saturate( 0.5 * ( dot( s.Normal, lightDir ) + 1.0 ) );
			falloffU = saturate( rimlightDot * falloffU );
			falloffU = tex2D( _RimLightSampler, half2( falloffU, 0.25f ) ).r;
			half3 lightColor = c.rgb * _lightPower;
			combinedColor += falloffU * lightColor;
			c.rgb = combinedColor;

			c.a = s.Alpha;
			return c ;
		}

		void surf (Input IN, inout EditorSurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex,IN.uv_MainTex);
			fixed4 spec = tex2D(_specTex,IN.uv_specTex);
			fixed4 falloff = tex2D(_FallOff,IN.uv_MainTex);
			fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
			half ralpha= spec.a;
			half alpha = lerp(0, spec.a , _specPower);
			o.SAlpha = alpha;
			o.Albedo = _Color * c;
			o.Albedo = lerp(o.Albedo,o.Albedo*falloff,_FallOffPower);

			o.Specular = _shininess * alpha;
			o.Gloss = _specMultiple * alpha;

			o.Emission = reflcol.rgb * _ReflectColor.rgb  * _CubePower;
	        o.Alpha = c.a;
			clip(c.a-0.5f);
		}
		ENDCG
	}
	Fallback "Diffuse"
}