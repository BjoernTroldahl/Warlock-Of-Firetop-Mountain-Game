Shader "Output"
{
Properties
{
_UV("UV", Vector) = (0, 0, 0, 0)
_DissolveNoiseScale("DissolveNoiseScale", Float) = 0
_DissolveProgress("DissolveProgress", Float) = 0
_DissolveDepth("DissolveDepth", Float) = 0
_Dissolve3("Dissolve3", Vector) = (0, 0, 0, 0)
_Input_Color("Input Color", Vector) = (0, 0, 0, 0)
}
SubShader
{
Tags
{
// RenderPipeline: <None>
"RenderType"="Opaque"
"Queue"="Geometry"
"ShaderGraphShader"="true"
}
Pass
{
    // Name: <None>
    Tags
    {
        // LightMode: <None>
    }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass

    HLSLPROGRAM

    // Pragmas
    #pragma vertex vert
#pragma fragment frag

    // DotsInstancingOptions: <None>
    // HybridV1InjectedBuiltinProperties: <None>

    // Keywords
    // PassKeywords: <None>
    // GraphKeywords: <None>

    // Defines
    #define VARYINGS_NEED_POSITION_WS
    /* WARNING: $splice Could not find named fragment 'PassInstancing' */
    #define SHADERPASS SHADERPASS_PREVIEW
#define SHADERGRAPH_PREVIEW 1
    /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

    // Includes
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

    // --------------------------------------------------
    // Structs and Packing

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

    struct Attributes
{
 float3 positionOS : POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
 uint instanceID : INSTANCEID_SEMANTIC;
#endif
};
struct Varyings
{
 float4 positionCS : SV_POSITION;
 float3 positionWS;
#if UNITY_ANY_INSTANCING_ENABLED
 uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
};
struct SurfaceDescriptionInputs
{
 float3 ObjectSpacePosition;
};
struct VertexDescriptionInputs
{
};
struct PackedVaryings
{
 float4 positionCS : SV_POSITION;
 float3 interp0 : INTERP0;
#if UNITY_ANY_INSTANCING_ENABLED
 uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
};

    PackedVaryings PackVaryings (Varyings input)
{
PackedVaryings output;
ZERO_INITIALIZE(PackedVaryings, output);
output.positionCS = input.positionCS;
output.interp0.xyz =  input.positionWS;
#if UNITY_ANY_INSTANCING_ENABLED
output.instanceID = input.instanceID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
output.cullFace = input.cullFace;
#endif
return output;
}

Varyings UnpackVaryings (PackedVaryings input)
{
Varyings output;
output.positionCS = input.positionCS;
output.positionWS = input.interp0.xyz;
#if UNITY_ANY_INSTANCING_ENABLED
output.instanceID = input.instanceID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
output.cullFace = input.cullFace;
#endif
return output;
}


    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
half2 _UV;
half _DissolveNoiseScale;
half _DissolveProgress;
half _DissolveDepth;
half4 _Dissolve3;
half4 _Input_Color;
CBUFFER_END

// Object and Global properties

    // Graph Includes
    // GraphIncludes: <None>

    // -- Property used by ScenePickingPass
    #ifdef SCENEPICKINGPASS
    float4 _SelectionID;
    #endif

    // -- Properties used by SceneSelectionPass
    #ifdef SCENESELECTIONPASS
    int _ObjectId;
    int _PassValue;
    #endif

    // Graph Functions
    

inline float Unity_SimpleNoise_RandomValue_float (float2 uv)
{
    float angle = dot(uv, float2(12.9898, 78.233));
    #if defined(SHADER_API_MOBILE) && (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3) || defined(SHADER_API_VULKAN))
        // 'sin()' has bad precision on Mali GPUs for inputs > 10000
        angle = fmod(angle, TWO_PI); // Avoid large inputs to sin()
    #endif
    return frac(sin(angle)*43758.5453);
}

inline float Unity_SimpleNnoise_Interpolate_float (float a, float b, float t)
{
    return (1.0-t)*a + (t*b);
}


inline float Unity_SimpleNoise_ValueNoise_float (float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = Unity_SimpleNoise_RandomValue_float(c0);
    float r1 = Unity_SimpleNoise_RandomValue_float(c1);
    float r2 = Unity_SimpleNoise_RandomValue_float(c2);
    float r3 = Unity_SimpleNoise_RandomValue_float(c3);

    float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
    float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
    float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
    return t;
}
void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
{
    float t = 0.0;

    float freq = pow(2.0, float(0));
    float amp = pow(0.5, float(3-0));
    t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

    freq = pow(2.0, float(1));
    amp = pow(0.5, float(3-1));
    t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

    freq = pow(2.0, float(2));
    amp = pow(0.5, float(3-2));
    t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

    Out = t;
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Step_float(float Edge, float In, out float Out)
{
    Out = step(Edge, In);
}

void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
{
    Out = lerp(A, B, T);
}

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

    // Graph Vertex
    // GraphVertex: <None>

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */

    // Graph Pixel
    struct SurfaceDescription
{
float Alpha_1;
float4 Out;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
SurfaceDescription surface = (SurfaceDescription)0;
half _Property_eea64631abc74295b9f38b517267933e_Out_0 = _DissolveNoiseScale;
float _SimpleNoise_547fcc7f3d294d9ab942c299ab27917f_Out_2;
Unity_SimpleNoise_float((IN.ObjectSpacePosition.xy), _Property_eea64631abc74295b9f38b517267933e_Out_0, _SimpleNoise_547fcc7f3d294d9ab942c299ab27917f_Out_2);
half _Property_3eb39b9f3241445380ec64d873490d71_Out_0 = _DissolveProgress;
float _Subtract_e8f12579379f4d8f84efe92815c3b200_Out_2;
Unity_Subtract_float(_SimpleNoise_547fcc7f3d294d9ab942c299ab27917f_Out_2, _Property_3eb39b9f3241445380ec64d873490d71_Out_0, _Subtract_e8f12579379f4d8f84efe92815c3b200_Out_2);
float _Step_062d34b049e84a0eb97f1fa9482ae6bb_Out_2;
Unity_Step_float(0, _Subtract_e8f12579379f4d8f84efe92815c3b200_Out_2, _Step_062d34b049e84a0eb97f1fa9482ae6bb_Out_2);
half4 _Property_aeb846b75da7488eb995773eb981db2a_Out_0 = _Dissolve3;
half4 _Property_77eaab5a890d4bb798fef063f29e22a1_Out_0 = _Input_Color;
half _Property_add4bb4f135840c585394dfd68353c8f_Out_0 = _DissolveDepth;
float _Step_c751563a08e242f582c3fd380d6df46a_Out_2;
Unity_Step_float(_Property_add4bb4f135840c585394dfd68353c8f_Out_0, _Subtract_e8f12579379f4d8f84efe92815c3b200_Out_2, _Step_c751563a08e242f582c3fd380d6df46a_Out_2);
float4 _Lerp_12e00be9aa2642b197c4168949b9ce48_Out_3;
Unity_Lerp_float4(_Property_aeb846b75da7488eb995773eb981db2a_Out_0, _Property_77eaab5a890d4bb798fef063f29e22a1_Out_0, (_Step_c751563a08e242f582c3fd380d6df46a_Out_2.xxxx), _Lerp_12e00be9aa2642b197c4168949b9ce48_Out_3);
float4 _Lerp_6782341c4e334d7f9846022d9b9e913e_Out_3;
Unity_Lerp_float4(_Property_aeb846b75da7488eb995773eb981db2a_Out_0, float4(0, 0, 0, 0), (_Step_c751563a08e242f582c3fd380d6df46a_Out_2.xxxx), _Lerp_6782341c4e334d7f9846022d9b9e913e_Out_3);
surface.Alpha_1 = _Step_062d34b049e84a0eb97f1fa9482ae6bb_Out_2;
surface.Out = all(isfinite(surface.Alpha_1)) ? half4(surface.Alpha_1, surface.Alpha_1, surface.Alpha_1, 1.0) : float4(1.0f, 0.0f, 1.0f, 1.0f);
return surface;
}

    // --------------------------------------------------
    // Build Graph Inputs

    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */





    output.ObjectSpacePosition =                        TransformWorldToObject(input.positionWS);
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/PreviewVaryings.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/PreviewPass.hlsl"

    ENDHLSL
}
}
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
FallBack "Hidden/Shader Graph/FallbackError"
}