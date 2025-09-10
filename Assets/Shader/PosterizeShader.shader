Shader "Custom/PosterizeWithNoise" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _Levels("Color Levels", Range(2, 16)) = 4
        _NoiseStrength("Noise Strength", Range(0,1)) = 0.1
        _NoiseScale("Noise Scale", Range(1,100)) = 25
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Levels;
            float _NoiseStrength;
            float _NoiseScale;

            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float2 co) {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            float4 frag(v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);

                float n = rand(i.uv * _NoiseScale);
                col.rgb += (n - 0.5) * _NoiseStrength;

                col.rgb = floor(col.rgb * _Levels) / (_Levels - 1);

                return saturate(col);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
