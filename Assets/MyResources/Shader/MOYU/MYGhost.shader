Shader "MOYU/Ghost"
{
	Properties
	{
		_fresnel_1("fresnel_1", Range(0, 1)) = 0.4686216
		_fresnel("fresnel", Float) = 20
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		_RimColor("RimColor", Color) = (1,1,1,1)
		_mask("mask", 2D) = "white" {}
		_alpha("alpha", Range(0, 8)) = 1.372233
		_subtact("subtact", Float) = 0.35
		_light("light", Float) = 5
	}
	SubShader
	{
		Tags{"Queue" = "AlphaTest" "RenderType" = "TransparentCutout"}
		Pass
		{
			Name "ForwardBase"
			Tags{"LightMode" = "ForwardBase"}
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			uniform fixed4 _Color;
			uniform fixed4 _RimColor;

			uniform half4 _mask_ST;
			uniform half _fresnel_1;
			uniform half _fresnel;
			uniform half _alpha;
			uniform half _subtact;
			uniform half _light;
			uniform sampler2D _mask;

			struct VertexInput
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half2 texcoord0 : TEXCOORD0;
			};

			struct VertexOutput
			{
				half4 pos : SV_POSITION;
				half4 posWorld : TEXCOORD1;
				half3 normalDir : TEXCOORD2;
				half2 uv0 : TEXCOORD0;
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o;
				o.uv0 = v.texcoord0;
				o.normalDir = mul(half4(v.normal,0), unity_WorldToObject).xyz;
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(VertexOutput i) : COLOR
			{
				i.normalDir = normalize(i.normalDir);
				fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				half3 normalDirection = i.normalDir;
				fixed maskr = (tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask)).rgb*_alpha).r;
				fixed maskr_a = step(maskr,0.3);
				fixed maskr_b = step(0.3, maskr);
				fixed blend = lerp(maskr_b,1, maskr_a*maskr_b);
				clip(blend - 0.5);
				fixed value_a = step(maskr,_subtact);
				fixed value_b = step(_subtact, maskr);
				fixed3 emissive = ((pow((_fresnel_1 + (1.0 - max(0,dot(normalDirection, viewDirection)))),_fresnel)*_RimColor.rgb) + ((_Color.rgb*(1.0*(blend - lerp(value_b,1, value_a*value_b))))*_light));
				return fixed4(emissive,1);
			}
			ENDCG
		}
	}
}
