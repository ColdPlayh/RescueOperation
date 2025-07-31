Shader "My/WaveDiffusion"
{
    Properties
    {
        //shader放一张图片,图片需要与要显示模型上材质球上图片一致,不放没有效果,
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Cull Off ZWrite Off ZTest Always

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4x4 _VPMatrix4x4_inverse;

            float4 _WaveColor;
            float4 _WaveCenter;
            float _WaveSpeed;
            float    _WaveInterval;
            float    _WavePower;
            float    _WaveColorPower;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed depth = tex2D(_CameraDepthTexture, i.uv).r;
                fixed4 ndc = fixed4(i.uv.x * 2 - 1, i.uv.y * 2 - 1, -depth * 2 + 1, 1);
                fixed4 worldPos = mul(_VPMatrix4x4_inverse, ndc);
                worldPos /= worldPos.w;
                float mask = 1 - Linear01Depth(depth);
                float dis = length(worldPos.xyz - _WaveCenter.xyz);
                dis -= _Time.y * _WaveSpeed;
                dis /= _WaveInterval;
                dis = dis - floor(dis);
                dis = (pow(dis, _WavePower) + pow(1 - dis, _WavePower * 4)) * 0.5;
                dis *= _WaveColorPower;
                return  dis * mask * _WaveColor + col;
            }
            ENDCG
        }
    }
}