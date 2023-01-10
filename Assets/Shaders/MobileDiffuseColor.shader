Shader "Mobile/Diffuse Color"
{
    Properties
    {
        _Color("Base (RGB)", COLOR) = (0.5, 0.5, 0.5, 1.0)
    }
 
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 150
        CGPROGRAM

        #pragma surface surf Lambert noforwardadd
 
        fixed4 _Color;
 
        struct Input
        {
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}