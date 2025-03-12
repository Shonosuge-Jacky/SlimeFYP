Shader "Custom/Outline Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
		
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
    }
}
