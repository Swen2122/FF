Shader "Custom/Shockwave2D"
{
    Properties
    {
        [MainTexture] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color     ("Color", Color)     = (1,1,1,1)
        _Center    ("Center", Vector)   = (0.5,0.5,0,0)
        _Thickness ("Thickness", Range(0.001,0.1)) = 0.02
        _Speed     ("Speed", Float)     = 1
        _ShockTime ("Shock Time", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            // ось ця штука змушує юніті юзати full-rect quad
            "CanUseSpriteAtlas"="True"
        }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4   _MainTex_ST;
            float4   _Color;
            float4   _Center;
            float    _Thickness;
            float    _Speed;
            float    _ShockTime;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos    : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.uv    = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Саме це вирізає ідеальне кільце
                float2 p = i.uv;
                float dist = distance(p, _Center.xy);
                float radius = _ShockTime * _Speed;
                float ring = smoothstep(radius - _Thickness, radius, dist)
                           * (1 - smoothstep(radius, radius + _Thickness, dist));

                // Беремо білий тектекс, щоб Unity думала, що ми юзаєм текстуру
                fixed4 tex = tex2D(_MainTex, i.uv);

                // Фінальний колір: наша α = ring, кольори з _Color
                tex.rgb = i.color.rgb;
                tex.a   = ring * i.color.a;
                return tex;
            }
            ENDHLSL
        }
    }
}
