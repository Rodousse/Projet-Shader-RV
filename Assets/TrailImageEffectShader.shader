Shader "TrailImageEffectShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

		SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog{ Mode off }

		Pass
	{
		CGPROGRAM

#include "UnityCG.cginc"

#pragma vertex vert_img
#pragma fragment frag

	uniform float samples;
	uniform sampler2D _MainTex;
	uniform sampler2D _CameraDepthTexture;
	uniform float4x4 _UNITY_VP_INV;
	uniform float4x4 _UNITY_VP_prev;

	float4 frag(v2f_img i) : SV_Target
	{
		float zOverW = tex2D(_CameraDepthTexture,i.uv);
		float4 H = float4(i.uv.x * 2 - 1, (1 - i.uv.y) * 2 - 1, zOverW, 1);
		float4 D = mul(H, _UNITY_VP_INV);
		float4 worldPos = D/D.w;

		float4 currentPos = H;
		float4 previousPos = mul(worldPos, _UNITY_VP_prev);
		previousPos /= previousPos.w;
		float2 velocity = (currentPos - previousPos) / 2.0f;

		float4 color = tex2D(_MainTex, i.uv);
		float2 tc = i.uv;
		tc += velocity;
		for (int j = 0; j < samples; ++j)
		{
			tc += velocity;
			float4 currentCol = tex2D(_MainTex, tc);
			color += currentCol;
		}
		return (color / samples);
	}

		ENDCG
	}
	}
}
