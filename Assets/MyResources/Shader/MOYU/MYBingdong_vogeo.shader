Shader "MOYU/dongjie_vogeo" {
    Properties {
		_TintColor("Tint Color", Color) = (1,1,1,1)
        _BenTi ("BenTi", 2D) = "white" {}
        _BenTiColor ("BenTiColor", Color) = (1,1,1,1)
        _BenTi_normal ("BenTi_normal", 2D) = "bump" {}
        _normal02 ("normal02", Range(0, 2)) = 2
        _xiaosan_texture ("xiaosan_texture", 2D) = "white" {}
        _xiosan ("xiosan", Range(0, 1.5)) = 0.3076923
        _xiaosan_color ("xiaosan_color", Color) = (0.4078432,0.4627451,0.5019608,1)
        _xiaosan_bian ("xiaosan_bian", Range(0, 0.2)) = 0.03658742
        _bian_color ("bian_color", Color) = (0.7490196,0.7490196,0.7490196,1)
        _Bing_wenli ("Bing_wenli", 2D) = "white" {}
        _waifaguang_bian ("waifaguang_bian", Range(0, 2)) = 2
        _waifaguang ("waifaguang", Color) = (0.4196079,0.6862745,1,1)
        _Normal_bingwenli ("Normal_bingwenli", 2D) = "bump" {}
        _Normal_bingwenli_qiangdu ("Normal_bingwenli_qiangdu", Range(0, 2)) = 2
        _specular ("specular", Range(0, 2)) = 1.579953
        _gloss ("gloss", Range(0, 1)) = 0.4330766
        _tuqi ("tuqi", Range(0, 1)) = 0.1335529
    }
    SubShader 
	{
        Tags {"RenderType"="Transparent" "IgnoreProjector" = "True" }
        Pass 
		{
            Name "FORWARD"
            Tags {"LightMode"="ForwardBase"}
			Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
			ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fog
            #pragma target 3.0
            #pragma glsl
			uniform fixed4 _TintColor;
            uniform half4 _LightColor0;
            uniform sampler2D _BenTi; 
			uniform half4 _BenTi_ST;
            uniform half4 _BenTiColor;
            uniform sampler2D _xiaosan_texture; 
			uniform half4 _xiaosan_texture_ST;
            uniform half _xiosan;
            uniform sampler2D _Bing_wenli; 
			uniform half4 _Bing_wenli_ST;
            uniform half4 _bian_color;
            uniform half _xiaosan_bian;
            uniform half4 _xiaosan_color;
            uniform half _waifaguang_bian;
            uniform half4 _waifaguang;
            uniform sampler2D _Normal_bingwenli; 
			uniform half4 _Normal_bingwenli_ST;
            uniform half _Normal_bingwenli_qiangdu;
            uniform sampler2D _BenTi_normal; 
			uniform half4 _BenTi_normal_ST;
            uniform half _normal02;
            uniform half _specular;
            uniform half _gloss;
            uniform half _tuqi;
            struct VertexInput {
                half4 vertex : POSITION;
                half3 normal : NORMAL;
                half4 tangent : TANGENT;
                half2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                half4 pos : SV_POSITION;
                half2 uv0 : TEXCOORD0;
                half4 posWorld : TEXCOORD1;
                half3 normalDir : TEXCOORD2;
                half3 tangentDir : TEXCOORD3;
                half3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, half4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                half4 _xiaosan_texture_var = tex2Dlod(_xiaosan_texture,half4(TRANSFORM_TEX(o.uv0, _xiaosan_texture),0.0,0));
                half node_7507 = step((_xiosan-_xiaosan_bian),_xiaosan_texture_var.r);
                half4 _Bing_wenli_var = tex2Dlod(_Bing_wenli,half4(TRANSFORM_TEX(o.uv0, _Bing_wenli),0.0,0));
                half3 node_2312 = (node_7507*_Bing_wenli_var.rgb);
                v.vertex.xyz += ((node_2312*v.normal)*_tuqi);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                half3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            half4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                half3x3 tangentTransform = half3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                half4 _BenTi_var = tex2D(_BenTi,TRANSFORM_TEX(i.uv0, _BenTi));
                half3 node_1198 = (_BenTiColor.rgb*_BenTi_var.rgb);
                half4 _xiaosan_texture_var = tex2D(_xiaosan_texture,TRANSFORM_TEX(i.uv0, _xiaosan_texture));
                half node_7507 = step((_xiosan-_xiaosan_bian),_xiaosan_texture_var.r);
                half3 _Normal_bingwenli_var = UnpackNormal(tex2D(_Normal_bingwenli,TRANSFORM_TEX(i.uv0, _Normal_bingwenli)));
                half3 _BenTi_normal_var = UnpackNormal(tex2D(_BenTi_normal,TRANSFORM_TEX(i.uv0, _BenTi_normal)));
                half3 normalLocal = (node_1198+(node_7507*(lerp(half3(0,0,1),_Normal_bingwenli_var.rgb,_Normal_bingwenli_qiangdu)+lerp(half3(0,0,1),_BenTi_normal_var.rgb,_normal02))));
                half3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                half3 lightColor = _LightColor0.rgb;
                half3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                half attenuation = LIGHT_ATTENUATION(i);
                half3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                half gloss = _gloss;
                half specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                half NdotL = max(0, dot( normalDirection, lightDirection ));
                half3 specularColor = half3(_specular,_specular,_specular);
                half3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                half3 specular = directSpecular;
////// Emissive:
                half4 _Bing_wenli_var = tex2D(_Bing_wenli,TRANSFORM_TEX(i.uv0, _Bing_wenli));
                half3 node_2312 = (node_7507*_Bing_wenli_var.rgb);
                half3 emissive = (((_waifaguang.rgb*lerp(0,1,pow(1.0-max(0,dot(normalDirection, viewDirection)),_waifaguang_bian)))*node_7507)+(lerp(node_1198,lerp(_bian_color.rgb,node_1198,step(_xiosan,_xiaosan_texture_var.r)),node_7507)+(node_2312*_xiaosan_color.rgb)));
/// Final Color:
                half3 finalColor = (specular + emissive) * _TintColor.rgb;
                fixed4 finalRGBA = fixed4(finalColor, _TintColor.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
