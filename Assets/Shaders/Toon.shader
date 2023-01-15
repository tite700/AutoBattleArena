Shader "Custom/Toon/Toon"
{
    Properties
    {
        _Tint("Tint (RGB)", Color) = (1,1,1,1)
        _OverlayColor("Overlay Color", Color) = (0,0,0,0)

        // _DirectionalAngle("Directional Angle", Vector) = (-0.35,1,-0.37,1)
        // _RampThreshold1("Ramp Threshold 1", Range(0, 1)) = 0.434
        // _RampThreshold2("Ramp Threshold 2", Range(0, 1)) = 0.908
        // _RampSmoothness("Ramp Smoothness", Range(0, 0.5)) = 0.196
    }

    SubShader
    {

        Tags
        {
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
        }
        LOD 200

        // Toon
        Pass
        {
            Tags
            {
                "LightMode"="ForwardBase"
            }

            Cull Back
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            // No lightmap support
            #pragma multi_compile_fwdbase/* nolightmap nodirlightmap nodynlightmap novertexlight*/

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed3 ambient : COLOR0;
                float3 normal : TEXCOORD0;
                UNITY_FOG_COORDS(1) // put fog data into TEXCOORD1
                SHADOW_COORDS(2) // put shadows data into TEXCOORD2
            };

            fixed4 _Tint;
            fixed4 _OverlayColor;

            static float _RampThreshold1 = 0.434;
            static float _RampThreshold2 = 0.89;
            static float _RampSmoothness = 0.2;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.ambient = ShadeSH9(half4(o.normal, 1));

                UNITY_TRANSFER_FOG(o, o.vertex);
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Tint;
                col.a = 1;

                // compute toon lighting
                fixed lightRamp = dot(i.normal, _WorldSpaceLightPos0.xyz);
                fixed lightPower = smoothstep(_RampThreshold1, _RampThreshold1 + _RampSmoothness, lightRamp);
                lightPower += smoothstep(_RampThreshold2, _RampThreshold2 + _RampSmoothness, lightRamp);
                lightPower = lightPower / 2 + 0.2;

                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);

                col.rgb = _Tint.rgb * lightPower * _LightColor0.rgb * shadow + _Tint.rgb * i.ambient;

                col.rgb = lerp(col.rgb, _OverlayColor.rgb, _OverlayColor.a);

                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }

        // pull in shadow caster from VertexLit built-in shader
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}