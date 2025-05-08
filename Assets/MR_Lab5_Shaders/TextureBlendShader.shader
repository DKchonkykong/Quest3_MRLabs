Shader "Custom/TextureBlendShader"
{
Properties
 {
 _MainTex ("Main Texture", 2D) = "white" {}
 _SecondaryTex ("Secondary Texture", 2D) = "white" {}
 _BlendFactor ("Blend Factor", Range(0, 1)) = 0.5
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
  float2 uv : TEXCOORD0;
 };
 struct v2f
 {
 float4 vertex : SV_POSITION;
  float2 uv : TEXCOORD0;
 };
 sampler2D _MainTex;
 sampler2D _SecondaryTex;
 float4 _MainTex_ST; 
 
 float4 _SecondaryTex_ST;
 float _BlendFactor;
 float4 _BaseColor;
 v2f vert (appdata v)
 {
 v2f o;
o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
 return o;
 }
fixed4 frag (v2f i) : SV_Target
 {
 fixed4 mainColor = tex2D(_MainTex, i.uv);
 
 fixed4 secondaryColor = tex2D(_SecondaryTex, i.uv);
 fixed4 blendedColor = lerp(mainColor, secondaryColor, _BlendFactor);
 return blendedColor;
 }
 ENDHLSL
 }
 }
}