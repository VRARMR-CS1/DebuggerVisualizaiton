Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,0.5) // 초기 투명도 설정
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline width", Range (.002, 0.03)) = .005
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _OutlineColor;
        float _Outline;

        struct Input
        {
            float4 color : Color;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
            o.Alpha = _Color.a; // 투명도 설정
        }
        ENDCG

        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ZTest LEqual

            Blend SrcAlpha OneMinusSrcAlpha

            Offset 7, 7

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            uniform float _Outline;
            uniform float4 _OutlineColor;

            v2f vert(appdata v)
            {
                // just make a copy of incoming vertex data but scaled according to normal direction
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 norm = mul((float3x3) unity_WorldToObject, v.normal);
                float3 pos = v.vertex.xyz + norm * _Outline;
                o.pos = UnityObjectToClipPos(float4(pos, 1));
                o.color = _OutlineColor;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
