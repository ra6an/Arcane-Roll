Shader "Custom/UIBlurBackgroundShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _BlurSize ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            float _BlurSize;
            sampler2D _MainTex;
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                // Jednostavan blur sa sample-ovanjem više piksela u okolini
                col += tex2D(_MainTex, i.uv + float2(_BlurSize, 0));
                col += tex2D(_MainTex, i.uv + float2(-_BlurSize, 0));
                col += tex2D(_MainTex, i.uv + float2(0, _BlurSize));
                col += tex2D(_MainTex, i.uv + float2(0, -_BlurSize));
                col /= 5.0;
                return col;
            }
            ENDCG
        }
    }
}
