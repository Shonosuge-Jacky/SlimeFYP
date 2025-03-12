Shader "Custom/Slime Toon Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
		
		_SpecularColor("Specular Color", Range(0,2)) = 0.9
        _DiffuseColor("Diffuse Color", Range(0,2)) = 0.6
        _AmbientColor("Ambient Color", Range(0,2)) = 0.4
        _ExternalColor("External Light Color", Range(0, 1)) = 0.8

        _SpecularLightRange("Specular Light Range", Range(0, 1)) = 0.5
        _DiffuseLightRange("Diffuse Light Range", Range(0, 1)) = 0
        _ExternalLightRange("External Light Range", Range(0, 20)) = 10

        _RimThreshold("Rim Threshold", Range(0,1)) = 0.1
        _RimAmount("Rim Amount", Range(0, 1)) = 0.7
        _RimColor("Rim Color", Range(0, 2)) = 1.5

		_Smoothness("Smoothness", Range(0,1)) = 0.5
        _FadeRange("Fade Range", float) = 1
        _FadeDensity("Fade Density", Range(0, 0.5)) = 0.25
        _FadeRatio("Fade Ratio", Range(0,1)) = 0.5

        _OutlineColor("Outline Color",Color) = (0.1,0.1,0.1,1)
		_OutlineWidth("Outline Width",Range(0,0.001)) = 0.001
    }
    SubShader
    {
        Pass        //Outline Pass
		{
            ZWrite Off

			CGPROGRAM
			
			#pragma vertex MyVertexProgam
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"
			
			struct VertexData
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
			};

			struct Interpolators
			{
				float4 pos : SV_POSITION;
                float4 worldPos : TEXCOORD0;
			};

			float4 _OutlineColor;
			float _OutlineWidth;
            float _FadeRange;
            sampler3D _DitherMaskLOD;
            float _FadeDensity;	
            float _FadeRatio;
            

			Interpolators MyVertexProgam(VertexData v) {
				Interpolators i;
				float camDist = distance(UnityObjectToWorldDir(v.position), _WorldSpaceCameraPos);
                // v.vertex.xyz += normalize(v.normal.xyz) * _OutlineWidth;
				v.position.xyz += normalize(v.normal) * camDist * _OutlineWidth;
				i.pos = UnityObjectToClipPos(v.position);
                i.worldPos = mul(unity_ObjectToWorld, v.position);
				return i;
			}

			fixed4 MyFragmentProgram(Interpolators i) : SV_TARGET {
                float _Distance = distance(_WorldSpaceCameraPos, i.worldPos);
                if(_Distance < _FadeRange){
                    float dither = 
                        tex3D(_DitherMaskLOD, float3(i.pos.xy * _FadeDensity, _FadeRatio * 0.9348)).a;
                    clip(dither - 1);
                }
				return _OutlineColor;
			}

			ENDCG
		}

        Pass    //Toon Shader Basic
        {
            Tags { 
                "LightMode" = "ForwardBase"
                // "PassFlags" = "OnlyDirectional"
            }
            CGPROGRAM
            #pragma vertex MyVertexProgam
            #pragma fragment MyFragmentProgram
            #pragma multi_compile DIRECTIONAL

            #include "UnityStandardBRDF.cginc"
            #include "Lighting.cginc"
			#include "AutoLight.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

			float _SpecularColor;
            float _DiffuseColor;
            float _AmbientColor;

            float _SpecularLightRange;
            float _DiffuseLightRange;

            float _RimThreshold;
            float _RimAmount;
            float _RimColor;

			float _Smoothness;	

            float _FadeRange;
            sampler3D _DitherMaskLOD;
            float _FadeDensity;	
            float _FadeRatio;
            

            struct VertexData {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Interpolators {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            Interpolators MyVertexProgam (VertexData v) {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.viewDir = WorldSpaceViewDir(v.position);
                i.worldPos = mul(unity_ObjectToWorld, v.position);
                return i;
            }

            fixed4 MyFragmentProgram (Interpolators i) : SV_Target {

                float _Distance = distance(_WorldSpaceCameraPos, i.worldPos);
                if(_Distance < _FadeRange){
                    float dither = 
                        tex3D(_DitherMaskLOD, float3(i.position.xy * _FadeDensity, _FadeRatio * 0.9348)).a;
                    clip(dither - 1);
                }
                
                
                i.normal = normalize(i.normal);
				i.viewDir = normalize(i.viewDir);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + i.viewDir);
				float specular =  pow( DotClamped(halfVector, i.normal), _Smoothness * 500);
                float specularLight = specular > _SpecularLightRange? ( _SpecularColor * (_LightColor0 - (0.43, 0.43, 0.43)) * 2 ) : 0;

				float diffuse = DotClamped(_WorldSpaceLightPos0, i.normal);
                float diffuseLight = diffuse > _DiffuseLightRange? _DiffuseColor * _LightColor0 : 0;

                float ambientLight = specularLight == 0 && diffuseLight == 0 ?_AmbientColor : 0;

                float rimDot = 1 - dot(i.viewDir, i.normal);
				float rimIntensity = rimDot * pow(diffuse, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float rimLight = rimIntensity * _RimColor;

                float totalLight = specularLight + diffuseLight + ambientLight + rimLight;

                float4 sample = tex2D(_MainTex, i.uv);

				return float4(totalLight,totalLight,totalLight, 1) * _Color * sample ;
            }
            ENDCG
        }

        Pass {      //Spot Light
			Tags {
				"LightMode" = "ForwardAdd"
			}

            Blend One One
            ZWrite Off
            

			CGPROGRAM

			#pragma target 3.0
            #pragma multi_compile
            #pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
            
            
			#include "UnityStandardBRDF.cginc"
            #include "UnityStandardUtils.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ExternalLightRange;
            float _ExternalColor;

            float _FadeOffset;
            sampler3D _DitherMaskLOD;
            float _FadeDensity;	

            struct VertexData {
                float4 position : POSITION;
                float3 normal : NORMAL;
            };

            struct Interpolators {
                float4 position : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPosition: TEXCOORD0;
            };

            Interpolators MyVertexProgram (VertexData v) {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.worldPosition = mul(unity_ObjectToWorld, v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                return i;
            }

            float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
                
                float _Distance = distance(_WorldSpaceCameraPos, i.worldPosition);
                if(_Distance < _FadeOffset){
                    float dither = 
                        tex3D(_DitherMaskLOD, float3(i.position.xy * _FadeDensity, lerp(0, _FadeOffset, _Distance)  * 0.9375)).a;
                    clip(dither - 1);
                }

                i.normal = normalize(i.normal);
                float3 lightDir = _WorldSpaceLightPos0 - i.worldPosition;
                float lightDistance = 1 / distance(_WorldSpaceLightPos0.xz, i.worldPosition.xz);
                float3 diffuse = _LightColor0 * DotClamped(lightDir, i.normal);
                float3 diffuseLight = diffuse > _ExternalLightRange ? _ExternalColor * lightDistance : 0;
                return float4(diffuseLight , 1);
            }

			ENDCG
		}

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}




//Outline Shader from online

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
 
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
 
//             #include "UnityCG.cginc"
 
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
 
//                 o.vertex = UnityObjectToClipPos(vertexPos);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
//                 return o;
//             }
 
//             fixed4 frag () : SV_Target
//             {
//                 return _OutlineColor;
//             }
//             ENDCG
//         }
 
//         Pass // Texture
//         {
//             Tags { "LightMode" = "UniversalForward" }
 
//             Blend SrcAlpha OneMinusSrcAlpha
 
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
 
//             #include "UnityCG.cginc"
 
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
//                 o.vertex = UnityObjectToClipPos(v.vertex);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                 return o;
//             }
 
//             fixed4 frag (v2f i) : SV_Target
//             {
//                 fixed4 col = tex2D(_MainTex, i.uv);
//                 return col * _MainColor;
//             }
//             ENDCG
//         }
//     }
// }