Shader "Hidden/VignetteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Intensity("Intensity",range(-1,1)) = 0
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
			float _Intensity;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 distToCentre = abs(fixed2(i.uv.x-.5,i.uv.y-.5));



                fixed4 col = lerp(float4(.9,.9,.9,1),tex2D(_MainTex, i.uv),min(1,1-(length(distToCentre)+_Intensity)));
                // just invert the colors
                return col;
            }
            ENDCG
        }
    }
}
