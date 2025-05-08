Shader "Custom/Per-Pixel_Lighting"
{
    Properties
    {
        LightPos ("Light Position", Vector) = (0,10,0,1)
        _Ambient ("Ambient Color", Color) = (0.2,0.2,0.2,0.2)
        _Diffuse ("Diffuse Color", Color) = (1,1,1,1)
        _Specular ("Specular Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex_ST ("Texture_ST", Vector) = (1,1,0,0) 
        _SpecularPower ("Specular Power", Range(0, 256)) = 30
    }

    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _LightPos;
            float4 _Ambient;
            float4 _Diffuse;
            float4 _Specular;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _SpecularPower;

            struct VS_INPUT
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct VS_OUTPUT
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWorld : TEXCOORD1;
                float3 ViewDirection : TEXCOORD2;
                float3 LightDirection : TEXCOORD3;
            };

            VS_OUTPUT vert (VS_INPUT v)
            {
                VS_OUTPUT o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalWorld = UnityObjectToWorldNormal(v.normal);
                o.LightDirection = _LightPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz;
                o.ViewDirection = _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            float4 frag (VS_OUTPUT i) : SV_Target
            {
                float4 textureColor = tex2D(_MainTex, i.uv);
                float3 normalWorld = normalize(i.normalWorld);
                float3 lightDirWorld = normalize(i.LightDirection);

                // Ambient lighting
                float4 fvTotalAmbient = _Ambient * textureColor;

                // Diffuse lighting
                float dotNL = max(0.0, dot(normalWorld, lightDirWorld));
                float4 fvTotalDiffuse = _Diffuse * dotNL * textureColor;

                // Specular lighting
                float3 fvReflection = normalize(reflect(-lightDirWorld, normalWorld));
                float3 fvViewDirection = normalize(i.ViewDirection);
                float fRDotV = max(0.0f, dot(fvReflection, fvViewDirection));
                float4 fvTotalSpecular = _Specular * pow(fRDotV, _SpecularPower);

                return fvTotalAmbient + fvTotalDiffuse + fvTotalSpecular;
            }
            ENDHLSL
        }
    }
}