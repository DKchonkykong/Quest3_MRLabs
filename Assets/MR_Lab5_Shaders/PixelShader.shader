Shader "Custom/PixelShader"
{
 Properties
 {
 _BaseColor ("Color", Color) = (1,0,0,1) 
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
 };
 struct v2f
 {
 float4 vertex : SV_POSITION;
 };
 float4 _BaseColor;
 v2f vert (appdata v)
 {
 v2f o;
 o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);;
 return o;
 }
fixed4 frag (v2f i) : SV_Target
 {
 float3 invertedColor = float3(1, 1, 1) -_BaseColor.rgb;
 return float4(invertedColor, 1.0);
 }
 ENDHLSL
 }
 }
}