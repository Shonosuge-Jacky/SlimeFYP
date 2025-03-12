Shader "Custom/ScreenspaceObject"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _PatternTex("Pattern", 2D) = "white" {}
        _Scaling("Scaling", Float) = 0.5
        _PatternColor("Pattern Color", Color) = (0,0,0,1)

        _OutlineColor("Outline Color",Color) = (0.1,0.1,0.1,1)
		_OutlineWidth("Outline Width",Range(0,0.0001)) = 0
        _TexOffset("Pattern Texture Offset", Range(0,1)) = 1
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
				return _OutlineColor;
			}

			ENDCG
		}
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Unlit fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _PatternTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        fixed4 _Color;
        float _Scaling;
        float4 _PatternColor;
        float _TexOffset;

        fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;

            c.rgb = s.Albedo;
            c.a = s.Alpha;

            //atten = step(.1,atten) * .5;
            // c.rgb *= atten;

            return c * _LightColor0;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float2 coords = IN.screenPos.xy / IN.screenPos.w;
            coords.y *= _ScreenParams.y / _ScreenParams.x;

            fixed4 patternColor = tex2D(_PatternTex, coords * _Scaling);
            if(patternColor.r > _TexOffset){
                o.Albedo = _PatternColor;
            }else{
                o.Albedo = c.rgb;
            }

            // c *= patternColor ;

            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
