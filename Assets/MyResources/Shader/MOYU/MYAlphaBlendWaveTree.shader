Shader "MOYU/WaveTree/AlphaBlend" 
{
	Properties 
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Brightness("Brightness", Range(0,5)) = 1
		_Wind ("Wind", Vector) = (0, 0, 0, 0.5)
		_TimeScale("TimeScale",float) = 0.3
	}

	SubShader
	{
		Lod 300
		Tags{ "RenderType" = "Geometry" "Queue" = "AlphaTest" "IgnoreProjector" = "false" }
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"

			struct a2v
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 texcoord : TEXCOORD0;
				half4 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_vertex
			{
				half4 pos : POSITION;
				half4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform half4 _AlphaTex_ST;
			fixed4 _Color;
			half _Brightness;
			half4 _Wind;
			half _TimeScale;

			half4 SmoothCurve(half4 x)
			{
				return x * x *(3.0 - 2.0 * x);
			}

			half4 TriangleWave(fixed4 x)
			{
				return abs(frac(x + 0.5) * 2.0 - 1.0);
			}

			half4 SmoothTriangleWave(fixed4 x)
			{
				return SmoothCurve(TriangleWave(x));
			}

			inline half4 AnimateVertex(half4 pos, half3 normal, half4 animParams)
			{
				fixed fDetailAmp = 0.1f;
				fixed fBranchAmp = 0.3f;

				half fObjPhase = dot(unity_ObjectToWorld[3].xyz, 1);
				half fBranchPhase = fObjPhase + animParams.x;

				half fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
				half time = _Time.y  * _TimeScale;

				half2 vWavesIn = half2(time, time) + half2(fVtxPhase, fBranchPhase);

				half4 vWaves = (frac(vWavesIn.xxyy * fixed4(4, 1.6, 0.75, 0.39)) - 1.0);

				vWaves = SmoothTriangleWave(vWaves);
				half2 vWavesSum = vWaves.xz + vWaves.yw;

				half3 bend = animParams.y * fDetailAmp * normal.xyz;
				bend.y = animParams.w * fBranchAmp;
				pos.xyz += ((vWavesSum.xyx * bend) + (_Wind.xyz * vWavesSum.y * animParams.w)) * _Wind.w;
				pos.xyz += animParams.z * _Wind.xyz;

				return pos;
			}

			void TreeVertLeaf(inout a2v v)
			{
				half y = v.vertex.y;
				v.vertex = AnimateVertex(v.vertex, v.normal, half4(v.color.xy, v.texcoord.xy)*v.color.a);
				v.vertex.y = y;
			}

			v2f_vertex vert(a2v v)
			{
				TreeVertLeaf(v);

				v2f_vertex o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.texcoord.xy;
				o.uv.zw = half2(0,0);
				#ifdef LIGHTMAP_ON
				o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(v2f_vertex i) : COLOR
			{
				fixed alpha = tex2D(_AlphaTex, i.uv.xy).r;
				fixed4 c = tex2D(_MainTex, i.uv.xy) * _Color * _Brightness;
				c.a *= alpha;
				#ifdef LIGHTMAP_ON
				c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv.zw));
				#endif
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}

			ENDCG
		}
	}

	SubShader
	{
		Lod 100
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "false" }
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"

			struct a2v
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 texcoord : TEXCOORD0;
				half4 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_vertex
			{
				half4 pos : POSITION;
				half4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform fixed4 _Color;
			uniform half _Brightness;

			v2f_vertex vert(a2v v)
			{
				v2f_vertex o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.texcoord.xy;
				o.uv.zw = half2(0,0);
				#ifdef LIGHTMAP_ON
				o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(v2f_vertex i) : COLOR
			{ 
				fixed alpha = tex2D(_AlphaTex, i.uv.xy).r;
				fixed4 c = tex2D(_MainTex, i.uv.xy) * _Color * _Brightness;
				c.a *= alpha;
				#ifdef LIGHTMAP_ON
				c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv.zw));
				#endif
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
}