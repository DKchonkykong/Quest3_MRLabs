Shader "Custom/NormalsShader"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,0,0,1)
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _HeightMap ("Height Map", 2D) = "white" {}
        _BumpScale ("Bump Scale", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            float4 _BaseColor;
            sampler2D _NormalMap;
            sampler2D _HeightMap;
            float _BumpScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                o.worldNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the normal map
                float3 normalTex = tex2D(_NormalMap, i.uv).rgb * 2.0 - 1.0;

                // Apply the height map as a bump map
                float height = tex2D(_HeightMap, i.uv).r;
                float3 modifiedNormal = normalize(i.worldNormal + normalTex * _BumpScale * height);

                // Calculate lighting (basic example)
                float3 lightDir = normalize(float3(0.0, 1.0, 1.0)); // Example light direction
                float diff = max(dot(modifiedNormal, lightDir), 0.0);

                // Combine base color with lighting
                float3 invertedColor = float3(1, 1, 1) - _BaseColor.rgb;
                float3 finalColor = invertedColor * diff;

                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}