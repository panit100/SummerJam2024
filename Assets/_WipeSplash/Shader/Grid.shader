Shader "Custom/GridWithTextures" {
    Properties {
        _MainTexArray ("Textures", 2DArray) = "white" {} 
        _CellScale ("Scale", Vector) = (1,1,0,0) 
        _CellRotation ("Rotation", Float) = 0.0 // Uniform rotation for simplicity
        _GridSize ("Grid Size", Vector) = (10,10,0,0) // 10x10 grid
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
                uint id : TEXCOORD1;
            };

            UNITY_DECLARE_TEX2DARRAY(_MainTexArray);
            float4 _CellScale;
            float _CellRotation;
            float2 _GridSize;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.id = floor(v.uv.x * _GridSize.x) + floor(v.uv.y * _GridSize.y) * _GridSize.x;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Calculate UVs based on grid position, scale, and rotation
                float2 uv = i.uv * _GridSize - floor(i.uv * _GridSize);
                uv = (uv - 0.5) * _CellScale.xy + 0.5;
                float cosTheta = cos(_CellRotation);
                float sinTheta = sin(_CellRotation);
                uv = float2(
                    cosTheta * (uv.x - 0.5) - sinTheta * (uv.y - 0.5) + 0.5,
                    sinTheta * (uv.x - 0.5) + cosTheta * (uv.y - 0.5) + 0.5
                );

                // Select the correct texture based on grid position
                fixed4 col = UNITY_SAMPLE_TEX2DARRAY(_MainTexArray, float3(uv, i.id));
                return col;
            }
            ENDCG
        }
    }
}