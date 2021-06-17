Shader "Unlit/Blending"
{
    Properties
    {
        _SrcOne ("Texture", 2D) = "white" {}
        _SrcTwo ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _SrcOne;
            sampler2D _SrcTwo;

            float _Blending;

            float4 _SrcOne_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _SrcOne);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col1 = tex2D(_SrcOne, i.uv);
                fixed4 col2 = tex2D(_SrcTwo, i.uv);

                fixed4 col = lerp(col1, col2, _Blending);

                return col;
            }
            ENDCG
        }
    }
}