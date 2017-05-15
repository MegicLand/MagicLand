Shader "MagicLand/Rune/MageShield/MageShield"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap ("Bump Map", 2D) = "white" {}
		_LightColor ("Light Color", Color) = (1,1,1,1)
		_NoiseMap ("Noise Map", 2D) = "white" {}
		_MaskMap ("Mask Map", 2D) = "white" {}
		_CubeMap ("Environment Texture", Cube) = "_Skybox" {}
		_Distortion("Distortion", Range(0, 500)) = 10
		_Refraction("Refraction", Range(0, 1)) = 1
		_Outline ("Outline Width", Range(0, 0.1)) = 0.05
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
		LOD 100

		GrabPass { "_RefractionTex" }

		Pass
		{
			Cull Front
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _Outline;
			fixed4 _OutlineColor;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert(a2v v) {
				v2f o;

				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				normal.z = -0.5;
				float depth = -pos.z / pos.w * 0.01;
				_Outline = -clamp(depth, 0, _Outline);
				pos = pos + float4(normalize(normal), 0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, pos);

				return o;
			}

			float4 frag(v2f i) : SV_Target{
				return float4(_OutlineColor.rgb, 1);
			}

			ENDCG
		}

		Pass
		{
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			fixed4 _LightColor;
			sampler2D _NoiseMap;
			float4 _NoiseMap_ST;
			sampler2D _MaskMap;
			float4 _MaskMap_ST;
			samplerCUBE _CubeMap;
			float _Distortion;
			float _Refraction;
			sampler2D _RefractionTex;
			float4 _RefractionTex_TexelSize;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD1;
				float4 TtoW0 : TEXCOORD2;
				float4 TtoW1 : TEXCOORD3;
				float4 TtoW2 : TEXCOORD4;
				float4 uvLight : TEXCOORD5;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _BumpMap);
				o.scrPos = ComputeGrabScreenPos(o.vertex);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

				o.uvLight.xy = TRANSFORM_TEX(v.uv, _NoiseMap);
				o.uvLight.zw = TRANSFORM_TEX(v.uv, _MaskMap);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 worldPos = float3(i.TtoW0.x, i.TtoW1.x, i.TtoW2.x);
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				fixed3 bump = UnpackNormal(tex2D(_BumpMap, i.uv.zw));

				float2 offset = bump.xy * _Distortion * _RefractionTex_TexelSize.xy;
				i.scrPos.xy = offset * i.scrPos.z + i.scrPos.xy;
				fixed3 refractionColor = tex2D(_RefractionTex, i.scrPos.xy / i.scrPos.w).rgb;

				bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));
				fixed3 reflectionDir = reflect(-worldViewDir, bump);
				fixed4 texColor = tex2D(_MainTex, i.uv.xy);

				fixed3 reflectionColor = texCUBE(_CubeMap, reflectionDir).rgb * texColor.rgb;
				fixed3 col = reflectionColor * (1 - _Refraction) + refractionColor * _Refraction;

				float noise1 = tex2D(_NoiseMap, i.uvLight.xy).r;
				float noise2 = tex2D(_NoiseMap, i.uvLight.xy + frac(float2(0.1, 0.0) * _Time.y)).r;
				float mask = tex2D(_MaskMap, i.uvLight.zw).r;
				float argv = saturate(noise1 - noise2 / 1.2);
				fixed3 lightColor = col * (1 - argv) + argv * _LightColor.rgb;

				col = lerp(lightColor, col, mask);

				return fixed4(col, 1) * _Color;
			}
			ENDCG
		}
	}
	Fallback "Transparent/VertexLit"
}
