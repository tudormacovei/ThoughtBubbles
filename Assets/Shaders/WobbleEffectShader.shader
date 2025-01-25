Shader "WobbleEffectShader"
{
    Properties
    {
        _frequency("Frequency",float) = 1
        _shift("Shift",float) = 0
        _amplitude("Amplitude",float) = 0
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
        ZWrite Off Cull Off
        Pass
        {
            Name "WobbleBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            float _frequency;
            float _shift;
            float _amplitude;

            float _Intensity;

            float4 Frag(Varyings input) : SV_Target
            {
                // This function handles the different ways XR platforms handle texture arrays.
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            // Sample the texture using the SAMPLE_TEXTURE2D_X_LOD function
            float2 uv = input.texcoord.xy;
            half4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);


            const float pi = 3.14159;

            float x_old = uv.x;

            float x_new = uv.x - sin(_frequency * 2 * pi * uv.y + _shift) * _amplitude;

            uv.x = x_new;

            // sample the texture
            float4 col = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);

            return col;
        }

        ENDHLSL
    }
    }
}