Shader "Custom/Ambient/Sprite/Water Advanced"
{
    Properties
    {
        [Header(Fog)]
        [PerRendererData] _MainTex("Fog Ramp", 2D) = "white" {}
        _FadeWidth("Fog Width", Range(0,10)) = 1.3
        _Color("Tint", Color) = (1,1,1,1)

        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)

        [Header(Reflection)]
        _ReflectionColor("Reflection Tint", Color) = (1,1,1,1)

        [Header(Foam)]
        _FoamTex("Foam Texture (R)", 2D) = "white" {}
        _FoamColor("Foam Tint", Color) = (1,1,1,1)
        [NoScaleOffset] _NoiseTex("Noise Texture (R)", 2D) = "white" {}

        [Header(Foam Movement)]
        _FoamMovementAmplitude("Movement Amplitude", Range(0, 0.3)) = 0.3
        _FoamMovementFrequency("Movement Frequency", Range(0, 0.1)) = 0.1
        _FoamMovementSpeed("Movement Speed", Range(0, 1)) = 1

        [Header(Foam Mask)]
        _FoamMaskIntensity("Mask Intensity", Range(0, 3)) = 1
        _FoamMaskScale("Mask Scale", Range(0, 0.02)) = 0.01
        _FoamMaskSpeed("Mask Speed", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite On
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #include "UnitySprites.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f_custom
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float4 screenPos : TEXCOORD0;
                float4 position : TEXCOORD1;
                half3 normal : TEXCOORD2;
                float2 uv : TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture); //Depth Texture

            float _FadeWidth;
            sampler2D _FoamTex;
            float4 _FoamTex_ST;

            v2f_custom vert(appdata IN)
            {
                v2f_custom OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.position = IN.vertex;
                OUT.normal = IN.normal;
                OUT.color = IN.color * _Color * _RendererColor;

                OUT.uv = TRANSFORM_TEX(IN.vertex.xy, _FoamTex);

                #ifdef PIXELSNAP_ON
			    OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                OUT.screenPos = ComputeScreenPos(OUT.vertex);
                const appdata v = IN;
                COMPUTE_EYEDEPTH(OUT.screenPos.z);

                return OUT;
            }

            fixed3 _ReflectionColor;
            fixed4 _FoamColor;
            sampler2D _NoiseTex;

            float _FoamMovementAmplitude;
            float _FoamMovementFrequency;
            float _FoamMovementSpeed;

            float _FoamMaskIntensity;
            float _FoamMaskScale;
            float _FoamMaskSpeed;

            fixed4 frag(v2f_custom IN) : SV_Target
            {
                fixed3 color = IN.color;
                fixed alpha = 1;

                // Fade
                const float depth = LinearEyeDepth(
                    SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
                const float fogIntensity = saturate((depth - IN.screenPos.z) / _FadeWidth);
                const fixed4 texColor = SampleSpriteTexture(fogIntensity);

                alpha *= texColor.a;
                color *= texColor.rgba;

                // Foam movement
                const float2 noiseUV = IN.position.xy * _FoamMovementFrequency + _Time.xx * _FoamMovementSpeed;
                const float2 foamUV = IN.uv + tex2D(_NoiseTex, noiseUV).r * _FoamMovementAmplitude;

                // Foam erase
                const float2 foamMaskUV = IN.position.xy * _FoamMaskScale + _Time.xx * _FoamMaskSpeed;
                const float2 foamMask = 1 - min(tex2D(_NoiseTex, foamMaskUV).r * _FoamMaskIntensity, 1);

                const fixed foam = tex2D(_FoamTex, foamUV).r * foamMask * texColor.a * _FoamColor.a;
                color = lerp(color.rgb, _FoamColor.rgb, foam);

                // Reflection
                const float fresnel = dot(normalize(ObjSpaceViewDir(IN.position)) /*V*/, IN.normal /*N*/);
                color = lerp(_ReflectionColor, color.rgb, fresnel).rgb;

                fixed4 c = fixed4(color, alpha) * UNITY_LIGHTMODEL_AMBIENT;

                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}