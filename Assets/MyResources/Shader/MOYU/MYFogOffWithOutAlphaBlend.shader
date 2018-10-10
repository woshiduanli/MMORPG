Shader "MOYU/FogOffWithOutAlphaBlend"
{
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Color ("Color", Color) = (1,1,1,1)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Cull Off Lighting Off ZWrite OFF Fog { Mode Off }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
			SetTexture [_MainTex] {
			    constantColor[_Color]
				combine texture * constant
			}
		}
	}
}
}
