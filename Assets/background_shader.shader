Shader "Custom/CylinderSideOnly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SideColor ("Side Color", Color) = (1,1,1,1)
        _TopBottomColor ("Top/Bottom Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _SideColor;
            float4 _TopBottomColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normalDir = normalize(v.normal);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Only apply texture to sides
                float3 n = normalize(i.normalDir);

                if (abs(n.y) < 0.5) // mostly horizontal normal => side
                {
                    fixed4 tex = tex2D(_MainTex, i.uv);
                    return tex * _SideColor;
                }
                else // top or bottom
                {
                    return _TopBottomColor;
                }
            }
            ENDCG
        }
    }
}
