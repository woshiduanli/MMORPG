Shader "MOYU/Shadow" 
{
	Properties {
		_ShadowTex ("Shadow", 2D) = "gray" {}
		_FadeTex ("FadeTex", 2D) = "white" {}
		_Alpha("Alpha", Range(0,1)) = 0.6
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		Pass {
			ZWrite Off
			Fog { Mode Off }
			ColorMask RGB
			Blend DstColor Zero

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos:POSITION;
				float4 sproj:TEXCOORD0;
				float4 fproj:TEXCOORD1;
			};

			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			fixed _Alpha;
			sampler2D _ShadowTex;
			sampler2D _FadeTex;

			v2f vert(float4 vertex:POSITION){
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.sproj = mul(unity_Projector, vertex);
				o.fproj = mul(unity_ProjectorClip, vertex);
				return o;
			}

			fixed4 frag(v2f i):COLOR{
				fixed4 c = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.sproj));
				fixed fade = tex2Dproj(_FadeTex, UNITY_PROJ_COORD(i.sproj)).g;
				fixed4 result;
				result.rgb = lerp(fixed3(1, 1, 1), 1 - c.a, _Alpha);
				result.rgb += 1 - fade;
				result.a = 1;
				return result;
			}

			ENDCG
		}
	} 
}
