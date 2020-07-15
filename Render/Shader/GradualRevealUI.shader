Shader "Unlit/GradualRevealUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white"{}
		_AlphaRX( "RangeAlphaRX",Float ) = 1
		_AlphaPower( "Power",Float ) = 0
		_Color( "Text Color", Color ) = (1,1,1,1)
    }
    SubShader
    {
		Tags {
			"RenderType" = "Transparent"
			 "Queue" = "Transparent"
			"IgnoreProjector" = "True"
			 "PreviewType" = "Plane"
		}
		Blend SrcAlpha OneMinusSrcAlpha
        // No culling or depth
		Lighting Off Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _AlphaTex;
			float _AlphaRX;
			float _AlphaPower;
			uniform fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.color = v.color*_Color;
                return o;
            }

			fixed4 SampleSpriteTexture( float2 uv )
			{
				fixed4 color = tex2D( _MainTex, uv );

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D( _AlphaTex, uv ).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag( v2f i ) : SV_Target
			{
				fixed4 col = tex2D( _MainTex, i.uv );
				col.r = col.r == 0 ? 1 : col.r;
				col.g = col.g == 0 ? 1 : col.g;
				col.b = col.b == 0 ? 1 : col.b;
                // just invert the colors

				fixed alpharx = col.a * lerp( 1, _AlphaPower, (i.uv.x - _AlphaRX) );
				col.a = saturate( lerp( col.a, alpharx, step( _AlphaRX, i.uv.x ) ) );

                return col;
            }
            ENDCG
        }
    }
}
