Shader "MagicLand/Rune/LightingChain/LightingChain"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Row ("Number of Rows", Float) = 4
		_Column ("Number of Columns", Float) = 1
		_AnimSpeed ("Animation Speed", Range(0,100)) = 20
		_MoveSpeed ("Move Speed", Range(-10,10)) = -1.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _Row;
			float _Column;
			float _AnimSpeed;
			float _MoveSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex) + frac(float2(_MoveSpeed, 0.0) * _Time.y);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float time = floor(_Time.y * _AnimSpeed);
				float row = floor(time / _Row);
				float column = time - row * _Row;
				half2 uv = i.uv + half2(column, -row);
				uv.x /= _Row;
				uv.y /= _Column;
				// sample the texture
				fixed4 col = tex2D(_MainTex, uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				if (col.a < 0.7) {
					col = fixed4(_Color.rgb, col.a);
				}
				return col;
			}
			ENDCG
		}
	}
	Fallback "Transparent/VertexLit"
}
