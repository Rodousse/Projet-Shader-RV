// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "Custom/Wave"
{
	Properties
	{
		_MainTex ("Tint Color (RGB)", 2D) = "white" {}
		_TintIntensity ("Tint Intensity", range(0,1)) = 1
		_BumpMap ("Normalmap", 2D) = "bump" {}

		_BumpAmt  ("Distortion", range (0,128)) = 10
		_Speed ("Speed", range (0,10)) = 1
		_MaxRange ("MaxRange", range (0,2)) = 1
	}

	CGINCLUDE
	#pragma fragmentoption ARB_precision_hint_fastest
	#pragma fragmentoption ARB_fog_exp2
	#include "UnityCG.cginc"

	sampler2D _BackgroundTexture : register(s0);
	float4 _BackgroundTexture_TexelSize;
	sampler2D _BumpMap : register(s1);
	sampler2D _MainTex : register(s2);
	float _T;
	float _Speed;
	float _Acceleration;
	float _MaxRange;
	float _TintIntensity;

	struct v2f
	{
		float4 vertex : POSITION;
		float4 uvgrab : TEXCOORD0;
		float2 uvbump : TEXCOORD1;
		float2 uvmain : TEXCOORD2;
	};

	uniform float _BumpAmt;
	uniform float _TimeStart;

	half4 frag( v2f i ) : COLOR
	{
		_T = ((_Time[1] - _TimeStart) *_Speed) % _MaxRange;

		if(_T < 0.02)
			_T = 0.02;
		
		// calculate perturbed coordinates
		half2 bump = UnpackNormal(tex2D(_BumpMap, (i.uvbump-(1-_T)/2)/_T )).xy; // we could optimize this by just reading the x & y without reconstructing the Z
		float2 offset = bump * (_BumpAmt * (_MaxRange-_T)) * _BackgroundTexture_TexelSize.xy;
		i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

		half4 col = tex2Dproj( _BackgroundTexture, i.uvgrab );

		half4 tint = tex2D( _MainTex, (i.uvmain-(1-_T)/2)/_T );

		return lerp(col, col * tint,  _T/_MaxRange * _TintIntensity);
	}
	ENDCG

	Category
	{
		// We must be transparent, so other objects are drawn before this one.
		Tags { "Queue"="Transparent+100" "RenderType"="Transparent" }

		SubShader 
		{
			// This pass grabs the screen behind the object into a texture.
			// We can access the result in the next pass as _GrabTexture
			GrabPass
			{
				"_BackgroundTexture"
				Name "BASE"
				Tags { "LightMode" = "Always" }
 			}
 		
 			// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
 			// on to the screen
			Pass
			{
				Name "BASE"
				Tags { "LightMode" = "Always" }
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					o.uvbump = MultiplyUV( UNITY_MATRIX_TEXTURE1, v.texcoord );
					o.uvmain = MultiplyUV( UNITY_MATRIX_TEXTURE2, v.texcoord );
					return o;
				}
				ENDCG
			}
		}

		// ------------------------------------------------------------------
		// Fallback for older cards and Unity non-Pro
	
		SubShader 
		{
			Blend DstColor Zero
			Pass 
			{
				Name "BASE"
				SetTexture [_MainTex] {	combine texture }
			}
		}
	}
}
