// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnlitShadows/UnlitShadowReceiveColor" {
	Properties{
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader{
		Cull Off

		Pass{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag 
			#include "UnityCG.cginc" 
			#pragma multi_compile_fwdbase 
			#include "AutoLight.cginc" 
			float4 _Color;
			struct v2f {
				float4 pos : SV_POSITION;
				LIGHTING_COORDS(0, 1)
				float2 uv : TEXCOORD2;
			};

			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float attenuation = LIGHT_ATTENUATION(i);
				return _Color;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}