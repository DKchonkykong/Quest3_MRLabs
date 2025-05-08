Shader "Custom/VertexPulsatingShader"
{
 Properties
 {
_BaseColor ("Color", Color) = (1,0,0,1) 
 _DisplacementAmount ("Displacement Amount", Float) = 0.1
 _DisplacementFrequency ("Displacement Amount", Float) = 2.0
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
 };
 struct v2f
 {
 float4 vertex : SV_POSITION;
 };
 float4 _BaseColor;
float _DisplacementAmount;
 float _DisplacementFrequency;

 v2f vert (appdata v)
 {
     v2f o;
    float4 inPos=v.vertex;
 float3 N=normalize(v.normal);
 inPos.xyz += _DisplacementAmount*(1.0+sin(_DisplacementFrequency*_Time.x))*N;
 o.vertex = mul(UNITY_MATRIX_MVP, inPos);

     return o;
 }
 fixed4 frag (v2f i) : SV_Target
 {
     return _BaseColor;
 }
 ENDHLSL
 }
 }
}