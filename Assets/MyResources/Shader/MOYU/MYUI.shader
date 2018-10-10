Shader "MOYU/UI"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_AlphaTex("AlphaTex",2D) = "white"{}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader
	{
		Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True"}
		Lighting Off
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest Greater [_Cutoff]
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				
			#include "UnityCG.cginc"
	
			struct appdata_t
			{
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				half4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				fixed gray : TEXCOORD1; 
			};
	
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _AlphaTex;
			float4 _AlphaTex_ST;
			fixed _Cutoff;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				o.gray = dot(v.color, fixed4(1,1,1,0));
				return o;
			}
				
			fixed4 frag (v2f i) : COLOR
			{
			    fixed4 col;
				col = tex2D(_MainTex, i.texcoord);
				col.a *= tex2D(_AlphaTex, i.texcoord).r; 
				if(i.gray == 0)
				{
					fixed grey = dot(col.rgb, fixed3(0.299, 0.587, 0.114));
					col.rgb = fixed3(grey, grey, grey);
					col.a *= i.color.a;
				}
				else
				{
				  col = col * i.color;
				}
				clip( col.a - _Cutoff );
				return col;
			}
			ENDCG
		}
	}


	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
