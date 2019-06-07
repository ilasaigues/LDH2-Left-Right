Shader "Hidden/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Distance ( "Distance", float) = 0
		_Intensity("Intensity",range(0,1)) = 0
		_Width ("Width", float) = 1600
		_Height ("Height", float) = 900

    }
    SubShader
    {
        // No culling or depth
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _Distance;
			float _Width;
			float _Height;
			float _Intensity;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 pixelSize = fixed2(1/_Width,1/_Height);

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col1 = tex2D(_MainTex, i.uv + fixed2(pixelSize.x,pixelSize.y)*_Distance);
                fixed4 col2 = tex2D(_MainTex, i.uv + fixed2(pixelSize.x,-pixelSize.y)*_Distance);
                fixed4 col3 = tex2D(_MainTex, i.uv + fixed2(-pixelSize.x,pixelSize.y)*_Distance);
                fixed4 col4 = tex2D(_MainTex, i.uv + fixed2(-pixelSize.x,-pixelSize.y)*_Distance);

				fixed4 blur = (col+col1+col2+col3+col4)/5;
				
                col.rgb = lerp(col.rgb, blur,_Intensity);
                return col;
            }
            ENDCG
        }
    }
}
