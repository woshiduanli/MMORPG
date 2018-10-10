Shader "MOYU/Dissolution_Blend" {
    Properties {
        _TintColor ("Color&Alpha", Color) = (1,1,1,1)
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _N_mask ("N_mask", Float ) = 0.3
        _T_mask ("T_mask", 2D) = "white" {}
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            uniform sampler2D _T_mask; 
			uniform half4 _T_mask_ST;
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

            fixed4 frag(VertexOutput i) : COLOR
			{
				half4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				half3 emissive = ((_TintColor.rgb*_MainTex_var.rgb)*i.vertexColor.rgb);
				half mask = (i.vertexColor.a*_N_mask);
				half4 _T_mask_var = tex2D(_T_mask,TRANSFORM_TEX(i.uv0, _T_mask));
				half mask_r = step(mask,_T_mask_var.r);
				half T_mask_r = step(_T_mask_var.r, mask);
				half T_mask = lerp(T_mask_r,1, mask_r*T_mask_r);
				half leA = step(mask,(_T_mask_var.r + _N_BY_KD));
				half leB = step((_T_mask_var.r + _N_BY_KD), mask);
				half blend = (T_mask - lerp(leB,1, leA*leB));
				half3 finalColor = emissive + ((blend*_C_BYcolor.rgb)*_N_BY_QD);
				return fixed4(finalColor,_TintColor.a*(_MainTex_var.a*(T_mask + blend)));
            }
            ENDCG
        }
    }
}
