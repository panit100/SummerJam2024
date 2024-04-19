Shader "Unlit/RotatingSquareCustomColor" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float aspect_ratio = _ScreenParams.y / _ScreenParams.x;
                float2 uv = i.uv - float2(0.5, 0.5 * aspect_ratio);
                float rot = radians(-30.0 );
                float2x2 rotation_matrix = float2x2(cos(rot), -sin(rot), sin(rot), cos(rot));
                uv = mul(rotation_matrix, uv);
                float2 scaled_uv = 20.0 * uv;
                float2 tile = frac(scaled_uv);
                float tile_dist = min(min(tile.x, 1.0 - tile.x), min(tile.y, 1.0 - tile.y));
                float square_dist = length(floor(scaled_uv));
                
                float edge = sin(_Time.y - square_dist * 20.0);
                edge = fmod(edge * edge, edge / edge);

                float value = lerp(tile_dist, 1.0 - tile_dist, step(1.0, edge));
                edge = pow(abs(1.0 - edge), 2.2) * 0.5;
                
                value = smoothstep(edge - 0.05, edge, 0.95 * value);
                
                value += square_dist * 0.1;
                value *= 0.8 - 0.2;
                
                return pow(_Color * fixed4(value * value, value * 1.5, value * 1.2, 1.0), 2.0);
            }
            ENDCG
        }
    }
}