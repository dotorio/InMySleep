Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _Color3 ("Color 3", Color) = (0, 0, 1, 1)
        _Color4 ("Color 4", Color) = (1, 1, 0, 1)
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {

            // Stencil
            // {
            //     Ref 1    
            //     Comp Equal
            //     Pass Keep
            //     Fail Keep
            // }

            // ZWrite Off 
            // ZTest Always               
            // ColorMask RGB         

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UV 좌표를 기반으로 네 가지 색상을 적용
                float2 uv = i.uv;
                fixed4 color = lerp(lerp(_Color1, _Color2, uv.x), lerp(_Color3, _Color4, uv.x), uv.y);
                return color;
            }
            ENDCG
        }
    }
}