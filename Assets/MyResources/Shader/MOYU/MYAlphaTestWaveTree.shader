Shader "MOYU/WaveTree/AlphaTest" 
{
	Properties 
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Cutoff ("Cutoff", float) = 0.5
		_Brightness("Brightness", Range(0,5)) = 1
		_Wind ("Wind", Vector) = (0, 0, 0, 0.5)
		_TimeScale("TimeScale",float) = 0.3
	}

	SubShader
	{
		Lod 300
		Tags{ "RenderType" = "Geometry" "Queue" = "AlphaTest" "IgnoreProjector" = "false" }
		ColorMask RGB
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			ColorMask RGB
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_vertex
			{
				float4 pos : POSITION;
				float4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			fixed4 _Color;
			fixed _Cutoff;
			half _Brightness;
			half4 _Wind;
			half _TimeScale;

			float4 SmoothCurve(float4 x)
			{
				return x * x *(3.0 - 2.0 * x);
			}

			float4 TriangleWave(float4 x)
			{
				return abs(frac(x + 0.5) * 2.0 - 1.0);
			}

			float4 SmoothTriangleWave(float4 x)
			{
				return SmoothCurve(TriangleWave(x));
			}

			inline float4 AnimateVertex(float4 pos, float3 normal, float4 animParams)
			{
				float fDetailAmp = 0.1f;
				float fBranchAmp = 0.3f;

				float fObjPhase = dot(unity_ObjectToWorld[3].xyz, 1);
				float fBranchPhase = fObjPhase + animParams.x;

				float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
				float time = _Time.y  * _TimeScale;

				float2 vWavesIn = float2(time, time) + float2(fVtxPhase, fBranchPhase);

				float4 vWaves = (frac(vWavesIn.xxyy * float4(4, 1.6, 0.75, 0.39)) - 1.0);

				vWaves = SmoothTriangleWave(vWaves);
				float2 vWavesSum = vWaves.xz + vWaves.yw;

				float3 bend = animParams.y * fDetailAmp * normal.xyz;
				bend.y = animParams.w * fBranchAmp;
				pos.xyz += ((vWavesSum.xyx * bend) + (_Wind.xyz * vWavesSum.y * animParams.w)) * _Wind.w;
				pos.xyz += animParams.z * _Wind.xyz;

				return pos;
			}

			void TreeVertLeaf(inout a2v v)
			{
				float y = v.vertex.y;
				v.vertex = AnimateVertex(v.vertex, v.normal, float4(v.color.xy, v.texcoord.xy)*v.color.a);
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

				clip(c.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}

			ENDCG
		}
	}

	SubShader
	{
		Lod 100
		Tags{ "RenderType" = "Geometry" "IgnoreProjector" = "False" }
		ColorMask RGB
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			ColorMask RGB
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_vertex
			{
				float4 pos : POSITION;
				float4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform	sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform fixed4 _Color;
			uniform fixed _Cutoff;
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

				clip(c.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
}