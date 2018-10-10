Shader "MOYU/FresnelGlow" 
{
    Properties 
	{
        _color ("color", Color) = (1,0.7655172,0,1)
        _exp ("exp", Float ) = 0.5
    }
    SubShader 
	{
        Tags { "IgnoreProjector"="True" "Queue"="Transparent+100" "RenderType"="Transparent" }
		ColorMask RGB
        Pass 
		{
            Blend One One
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform fixed4 _color;
            uniform fixed _exp;
            struct VertexInput 
			{
                half4 vertex : POSITION;
                half3 normal : NORMAL;
            };
            struct VertexOutput 
			{
                half4 pos : SV_POSITION;
                half4 posWorld : TEXCOORD0;
                half3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) 
			{
                VertexOutput o;
                o.normalDir = mul(half4(v.normal,0), unity_WorldToObject).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR 
			{
                i.normalDir = normalize(i.normalDir);
                half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                half3 normalDirection =  i.normalDir;
				fixed3 finalColor = _color.rgb * pow(1.0-max(0,dot(i.normalDir, viewDirection)) , _exp);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
}
