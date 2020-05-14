Shader "Sprites/Time Wave"{
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
        _Amplitude ("Amplitude", Float) = 1.0
        _Wavelength ("Wave Length", Float) = 1.0
	}

	SubShader{
		Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		ZWrite off
		Cull off

		Pass{

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float _Speed;
            float _Amplitude;
            float _Wavelength;

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                //o.position.x += _Amplitude * sin(_Time * _Speed + o.uv.y * _Wavelength);
				return o;
			}

			// fixed4 frag(v2f i) : SV_TARGET{
			// 	float offset = sin(i.uv.y * _Wavelength) * _Amplitude;
			// 	fixed4 col = tex2D(_MainTex, float2(i.uv.x + offset, i.uv.y));
			// 	col *= i.color;
			// 	return col;
			// }

			fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float offset = sin((i.uv.y * _Wavelength) + (_Time * _Speed)) * _Amplitude;

                fixed4 col = tex2D(_MainTex, float2(i.uv.x + offset,i.uv.y));

                return col;
            }

			ENDCG
		}
	}
}