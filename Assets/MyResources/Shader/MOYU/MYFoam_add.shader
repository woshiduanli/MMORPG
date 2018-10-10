Shader "MOYU/Foam_add" 
{
	Properties
	{
		_Offset ("Offset",Range (0.00,1.00)) = 1.00
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutout ("Mask (A)", 2D ) = "white" {}
	}

    SubShader
	{
		Tags { "Queue"= "Transparent" "IgnoreProjector"="True"}
        ZWrite Off
        Cull Off
        Offset -1, -1
        Blend SrcAlpha One
        ColorMask RGB

        Pass
		{
			Color[_Color]
	        SetTexture [_MainTex] {constantcolor[_Color]combine texture * primary double, texture * constant double}
	        SetTexture [_Cutout] { combine previous * texture }
	    } 
	}
}
