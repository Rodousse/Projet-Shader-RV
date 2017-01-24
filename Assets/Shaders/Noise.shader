Shader "Hidden/Noise"
{	
	SubShader
	{
		ZTest Always Cull Off ZWrite Off Blend Off
	  
		Pass
		{
			CGPROGRAM
		
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _ScreenTex;
				sampler2D _NoiseTex;

				uniform float _NoiseAmount;
				float4 _ScreenTex_TexelSize;
				struct vinput 
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0; // Offset
				};

				struct v2f 
				{
					float4 pos : SV_POSITION;
					float2 uv_screen : TEXCOORD0;
					float4 uv_noise : TEXCOORD1;
				};	



				inline float3 Overlay(float3 m, float3 color) {
					color = saturate(color);
					float3 check = step(float3(0.5,0.5,0.5), color.rgb);
					float3 result = check * (float3(1,1,1) - ((float3(1,1,1) - 2*(color.rgb-0.5)) * (1-m.rgb))); 
					result += (1-check) * (2*color.rgb) * m.rgb;
					return result;
				}
				
				v2f vert (vinput v)
				{

					v2f o;
			
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	

					// Background Pixel
					o.uv_screen = float2(v.vertex.x,  v.vertex.y);

					// Noise Tiling
					o.uv_noise = v.texcoord.xyxy;

					return o; 
				}

				float4 frag ( v2f i ) : SV_Target
				{

					float4 oldColor = (tex2D (_ScreenTex, i.uv_screen));
			
					float3 newColor = (tex2D(_NoiseTex, i.uv_noise.xy) * float4(1,1,1,0)).rgb;
					newColor = lerp(float3(0.5,0.5,0.5), newColor, _NoiseAmount);
#if UNITY_UV_STARTS_AT_TOP
					if (_ScreenTex_TexelSize.y < 0)  //this is returning false even when the image is upside down!
					{
						i.uv_screen.y = 1 - i.uv_screen.y;
					}
#endif
					return float4(Overlay(newColor, oldColor.rgb), oldColor.a);
				}

			ENDCG
		}	
	}
}
