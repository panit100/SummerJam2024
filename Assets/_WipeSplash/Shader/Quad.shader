Shader "Custom/SpriteJitterShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 0.1
        _TimeMultiplier ("Time Multiplier", Float) = 1.0
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
          

           

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _NoiseScale;
            float _TimeMultiplier;

            float hash( float n )
			{
			    return frac(sin(n)*43758.5453);
			}
            float snoise( float3 x )
			{
			    // The noise function returns a value in the range -1.0f -> 1.0f

			    float3 p = floor(x);
			    float3 f = frac(x);

			    f       = f*f*(3.0-2.0*f);
			    float n = p.x + p.y*57.0 + 113.0*p.z;

			    return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
			                   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
			               lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
			                   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
			}

            v2f vert (appdata_full v) {
                v2f o;
                // Add noise to vertex position
                float noise = (snoise(float3(v.vertex.xy * _NoiseScale, _TimeMultiplier * _Time.y)) - 0.5) * 0.1;
                o.vertex = UnityObjectToClipPos(v.vertex + float4(noise, noise, 0, 0));
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Sample main texture
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                return col;
            }
            ENDCG
        }
    }
}