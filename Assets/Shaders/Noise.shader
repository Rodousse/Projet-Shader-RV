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
				
				v2f vert (vinput v)
				{
					v2f o;
			
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	

					o.uv_screen = float2(v.vertex.x,  v.vertex.y);
					o.uv_noise = v.texcoord.xyxy;

					return o; 
				}

				float4 frag ( v2f i ) : SV_Target
				{
					float3 oldColor = tex2D(_ScreenTex, i.uv_screen);
					float3 newColor = tex2D(_NoiseTex, i.uv_noise);

					newColor = lerp(float3(0.5,0.5,0.5), newColor, _NoiseAmount);
					float3 result = oldColor + newColor - float3(0.5,0.5,0.5);

					return float4(result,1);
				}

			ENDCG
		}	
	}
}
