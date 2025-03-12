// // This shader fills the mesh shape with a color predefined in the code.
// Shader "Custom/URP/URPUnlitShaderBasic"
// {
//     // The properties block of the Unity shader. In this example this block is empty
//     // because the output color is predefined in the fragment shader code.
//     Properties
//     {
//         _MainTex ("Texture", 2D) = "white" {}
//         _Color ("Color", Color) = (1, 1, 1, 1)
		
// 		_SpecularColor("Specular Color", Range(0,2)) = 0.9
//         _DiffuseColor("Diffuse Color", Range(0,2)) = 0.6
//         _AmbientColor("Ambient Color", Range(0,2)) = 0.4
//         _ExternalColor("External Light Color", Range(0, 1)) = 0.8

//         _SpecularLightRange("Specular Light Range", Range(0, 1)) = 0.5
//         _DiffuseLightRange("Diffuse Light Range", Range(0, 1)) = 0
//         _ExternalLightRange("External Light Range", Range(0, 20)) = 10

//         _RimThreshold("Rim Threshold", Range(0,1)) = 0.1
//         _RimAmount("Rim Amount", Range(0, 1)) = 0.7
//         _RimColor("Rim Color", Range(0, 2)) = 1.5

// 		_Smoothness("Smoothness", Range(0,1)) = 0.5
//         _FadeRange("Fade Range", float) = 1
//         _FadeDensity("Fade Density", Range(0, 0.5)) = 0.25
//         _FadeRatio("Fade Ratio", Range(0,1)) = 0.5

//         _OutlineColor("Outline Color",Color) = (0.1,0.1,0.1,1)
// 		_OutlineWidth("Outline Width",Range(0,0.001)) = 0.001
//     }

//     // The SubShader block containing the Shader code. 
//     SubShader
//     {
//         // SubShader Tags define when and under which conditions a SubShader block or
//         // a pass is executed.
//         Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        
//         Pass        //Outline Pass
//         {
//             Tags { "LightMode" = "SRPDefault"}

//             ZWrite Off

//             HLSLPROGRAM

//             #pragma vertex MyVertexProgram
//             #pragma fragment MyFragmentProgram

//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//             struct VertexData
//             {
//                 float4 position : POSITION;
//                 float3 normal : NORMAL;  
//             };

//             struct Interpolators
//             {
//                 float4 pos : SV_POSITION;
//                 float4 worldPos : TEXCOORD0;
//             };

//             float4 _OutlineColor;
// 			float _OutlineWidth;
//             float _FadeRange;
//             sampler3D _DitherMaskLOD;
//             float _FadeDensity;	
//             float _FadeRatio;

//             Interpolators MyVertexProgram(VertexData v) {
// 				Interpolators i;
// 				float camDist = distance(TransformObjectToWorldDir(v.position), _WorldSpaceCameraPos);
//                 // v.vertex.xyz += normalize(v.normal.xyz) * _OutlineWidth;
// 				v.position.xyz += normalize(v.normal) * camDist * _OutlineWidth;
// 				i.pos = TransformObjectToHClip(v.position);
//                 i.worldPos = mul(unity_ObjectToWorld, v.position);
// 				return i;
// 			}

// 			half4 MyFragmentProgram(Interpolators i) : SV_TARGET {
//                 float _Distance = distance(_WorldSpaceCameraPos, i.worldPos);
//                 if(_Distance < _FadeRange){
//                     float dither = 
//                         tex3D(_DitherMaskLOD, float3(i.pos.xy * _FadeDensity, _FadeRatio * 0.9348)).a;
//                     clip(dither - 1);
//                 }
// 				return _OutlineColor;
// 			}

//             ENDHLSL
//         }

//         Pass
//         {
//             // The HLSL code block. Unity SRP uses the HLSL language.
//             HLSLPROGRAM
//             // This line defines the name of the vertex shader. 
//             #pragma vertex vert
//             // This line defines the name of the fragment shader. 
//             #pragma fragment frag

//             // The Core.hlsl file contains definitions of frequently used HLSL
//             // macros and functions, and also contains #include references to other
//             // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

//             // The structure definition defines which variables it contains.
//             // This example uses the Attributes structure as an input structure in
//             // the vertex shader.
//             struct Attributes
//             {
//                 // The positionOS variable contains the vertex positions in object
//                 // space.
//                 float4 positionOS   : POSITION;                 
//             };

//             struct Varyings
//             {
//                 // The positions in this struct must have the SV_POSITION semantic.
//                 float4 positionHCS  : SV_POSITION;
//             };            

//             // The vertex shader definition with properties defined in the Varyings 
//             // structure. The type of the vert function must match the type (struct)
//             // that it returns.
//             Varyings vert(Attributes IN)
//             {
//                 // Declaring the output object (OUT) with the Varyings struct.
//                 Varyings OUT;
//                 // The TransformObjectToHClip function transforms vertex positions
//                 // from object space to homogenous space
//                 OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
//                 // Returning the output.
//                 return OUT;
//             }

//             // The fragment shader definition.            
//             half4 frag() : SV_Target
//             {
//                 // Defining the color variable and returning it.
//                 half4 customColor;
//                 customColor = half4(0.5, 0, 0, 1);
//                 return customColor;
//             }
//             ENDHLSL
//         }
//     }
// }

// Shader "ActionCode/Outline_Unlit"
// {
//     Properties
//     {
//         _MainTex ("Texture", 2D) = "white" {}
//         _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        
//         [Space]
//         _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
//         _OutlineThickness ("Outline Thickness", Range(0.0, 0.2)) = 0.1
//     }
//     SubShader
//     {
//         LOD 100
//         Tags { "Queue" = "Transparent" }
 
//         Pass // Outline
//         {
//             Tags { "LightMode" = "SRPDefaultUnlit" }
 
//             Blend SrcAlpha OneMinusSrcAlpha
//             ZWrite Off
 
//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
 
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };
 
//             struct v2f
//             {
//                 float2 uv : TEXCOORD0;
//                 float4 vertex : SV_POSITION;
//             };
 
//             sampler2D _MainTex;
//             float4 _MainTex_ST;
//             float4 _OutlineColor;
//             float _OutlineThickness;
 
//             float4 outline(float4 vertexPos, float thickness)
//             {
//                 float thicknessUnit = 1 + thickness;
//                 float4x4 scale = float4x4
//                 (
//                     thicknessUnit, 0, 0, 0,
//                     0, thicknessUnit, 0, 0,
//                     0, 0, thicknessUnit, 0,
//                     0, 0, 0, thicknessUnit
//                 );
 
//                 return mul(scale, vertexPos);
//             }
 
//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 float4 vertexPos = outline(v.vertex, _OutlineThickness);
 
//                 o.vertex = TransformObjectToHClip(vertexPos);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
//                 return o;
//             }
 
//             fixed4 frag () : SV_Target
//             {
//                 return _OutlineColor;
//             }
//             ENDHLSL
//         }
 
//         Pass // Texture
//         {
//             Tags { "LightMode" = "UniversalForward" }
 
//             Blend SrcAlpha OneMinusSrcAlpha
 
//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
 
//             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };
 
//             struct v2f
//             {
//                 float2 uv : TEXCOORD0;
//                 float4 vertex : SV_POSITION;
//             };
 
//             sampler2D _MainTex;
//             float4 _MainTex_ST;
//             float4 _MainColor;
 
//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.vertex = TransformObjectToHClip(v.vertex);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                 return o;
//             }
 
//             fixed4 frag (v2f i) : SV_Target
//             {
//                 fixed4 col = tex2D(_MainTex, i.uv);
//                 return col * _MainColor;
//             }
//             ENDHLSL
//         }
//     }
// }


// Shader "Custom/URP/UnlitColor"
// {
//     Properties
//     {
//         _Color ("Color", Color) = (1, 1, 1, 1)
//     }

//     SubShader
//     {
//         Tags
//         {
//             "RenderType"="Opaque"
//         }
//         LOD 100

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #pragma multi_compile_instancing
//             #include "UnityCG.cginc"

//             #pragma target 4.5
//             #pragma instancing_options renderinglayer
//             #pragma multi_compile _ DOTS_INSTANCING_ON

//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 UNITY_VERTEX_INPUT_INSTANCE_ID
//             };

//             struct v2f
//             {
//                 float4 vertex : SV_POSITION;
//                 UNITY_VERTEX_INPUT_INSTANCE_ID // use this to access instanced properties in the fragment shader.
//             };


//             // For SRP Batcher to work you need to put things in the right Cbuffer
//             half4 _Color;

//             UNITY_DOTS_INSTANCING_BUFFER_START(Props)
//             UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
//             UNITY_DOTS_INSTANCING_BUFFER_END(Props)

//             v2f vert(appdata v)
//             {
//                 v2f o;

//                 UNITY_SETUP_INSTANCE_ID(v);
//                 UNITY_TRANSFER_INSTANCE_ID(v, o);
//                 o.vertex = UnityObjectToClipPos(v.vertex);
//                 return o;
//             }

//             fixed4 frag(v2f i) : SV_Target
//             {
//                 UNITY_SETUP_INSTANCE_ID(i);
//                 return UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
//             }
//             ENDCG
//         }
//     }
// }