Shader "CustomPassthrough"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Opacity("Opacity", Range(0.0,1.0)) = 1
    }
        SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        ZWrite Off
        Blend OneMinusSrcAlpha SrcAlpha
        Cull off
        LOD 100


        Pass
        {
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
            float4 _MainTex_ST;
            float _Opacity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //return fixed4(0, 0, 0, 0);
                return lerp(fixed4(0, 0, 0, 0), fixed4(0, 0, 0, 1),_Opacity);
                //return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}