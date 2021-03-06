﻿Shader "Hidden/ChromAber"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_tearingSpeed("Tearing speed", Range(0,30)) = 1
		_tearingIntensity("Tearing speed", Range(0,30)) = 1
		//_blurDistance("Blur Distance", Range(0,200)) = 50;
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _tearingSpeed;
			float _tearingIntensity;
			float4 _MainTex_TexelSize;

			fixed4 frag (v2f i) : SV_Target
			{
				float amount = sin(_Time.y*  _tearingSpeed)* (_tearingIntensity/10);
	
				fixed4 col;
				col.r = tex2D( _MainTex, fixed2(i.uv.x+amount,i.uv.y) ).r;
				col.g = tex2D( _MainTex, i.uv ).g;
				col.b = tex2D( _MainTex, fixed2(i.uv.x-amount,i.uv.y ) ).b;

				col *= (1.0 - amount * 0.5);
				col.a = 1.0;

#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)  //this is returning false even when the image is upside down!
				{
					i.vertex.y = 1 - i.vertex.y;
				}
#endif
				return col;
	
				//fragColor = vec4(col,1.0);
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// just invert the colors
				//col = 1 - col;
				//return col;
			}
			ENDCG
		}
	}
}
