Shader "MOYU/Dissolution_Add" {
    Properties {
        _TintColor ("Diffuse Color", Color) = (0.6985294,0.6985294,0.6985294,1)
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _N_mask ("N_mask", Float ) = 0.3
        _MaskTexture ("Mask Texture", 2D) = "white" {}
        _C_BYcolor ("C_BYcolor", Color) = (1,0,0,1)
        _N_BY_QD ("N_BY_QD", Float ) = 3
        _N_BY_KD ("N_BY_KD", Float ) = 0.01
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            uniform sampler2D _MaskTexture; 
			uniform half4 _MaskTexture_ST;
            uniform sampler2D _MainTex; 
			uniform half4 _MainTex_ST;
            uniform half4 _TintColor;
            uniform half _N_mask;
            uniform half _N_BY_KD;
            uniform half4 _C_BYcolor;
            uniform half _N_BY_QD;
            struct VertexInput 
			{
                half4 vertex : POSITION;
                half2 texcoord0 : TEXCOORD0;
                half4 vertexColor : COLOR;
            };
            struct VertexOutput 
			{
                half4 pos : SV_POSITION;
                half2 uv0 : TEXCOORD0;
                half4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v)
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            half4 frag(VertexOutput i) : COLOR 
			{
				half4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				half mask = (i.vertexColor.a*_N_mask);
				half4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(i.uv0, _MaskTexture));
				half leA = step(mask,_MaskTexture_var.r);
				half leB = step(_MaskTexture_var.r, mask);
				half blend = lerp(leB,1,leA*leB);
				half leA1 = step(mask,(_MaskTexture_var.r + _N_BY_KD));
				half leB2 = step((_MaskTexture_var.r + _N_BY_KD), mask);
				half value = (blend - lerp(leB2,1, leA1*leB2));
				half value2 = (blend + value);
				half3 value3 = ((value*_C_BYcolor.rgb)*_N_BY_QD);
				half3 emissive = (_TintColor.a*(((_TintColor.rgb*_MainTex_var.rgb)*value2) + value3));
				half3 finalColor = emissive + (_TintColor.a*value3);
				return fixed4(finalColor, _TintColor.a*(_MainTex_var.a*value2));
            }
            ENDCG
        }
    }
}
