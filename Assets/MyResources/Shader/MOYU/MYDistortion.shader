Shader "MOYU/Distortion" 
{
	SubShader
	{
		Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" }
		Pass
		{
			Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "false" }
			ColorMask RGB

			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"


			fixed4 frag(v2f_img i) : COLOR
			{
				clip(0 - 0.5f);
				return fixed4(0,0,0,0);
			}
			ENDCG
		}
	}

}