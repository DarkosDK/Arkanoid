Shader "Unlit/BrickEffect"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        
        [HDR] _Color ("Effect Color", Color) = (1, 1, 1, 1)
        _Mask ("Mask", 2D) = "white" {}
		_MovementDirection ("Movement Direction", float) = (0, -1, 0, 1)
    }
    SubShader
    {
        Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
			"CanUseSpriteAtlas"="True"
		}
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite off
		Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
			sampler2D _Mask;
            
			float4 _MainTex_ST;
			float4 _Mask_ST;

            half2 _MovementDirection;
			fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
				fixed4 color : COLOR;
            };

            /*
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv1 : TEXCOORD1;
				fixed4 color : COLOR;
            };
            */

            struct v2f
            {
				float4 position : SV_POSITION;
            	UNITY_FOG_COORDS(1)
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				fixed4 color : COLOR;
            	
			};

            v2f vert (appdata v)
            {
                v2f o;
            	UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _Mask);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv1 += _MovementDirection * _Time.y / 2.0;

				fixed4 mainTexture = tex2D(_MainTex, i.uv);
				fixed4 overlayTexture = tex2D(_Mask, i.uv1);

				fixed4 returnTexture = mainTexture;


				overlayTexture.rgb *= _Color.rgb;
            	_Color.a = (sin(_Time.y * 1.5) + 1)/2;
				//returnTexture.rgb = overlayTexture.r * _Color.a * (overlayTexture.rgb-returnTexture.rgb) + returnTexture.rgb;
            	
            	returnTexture.rgb = lerp(mainTexture.rgb, _Color.rgb, (overlayTexture.r * _Color.a));

            	returnTexture.a = mainTexture.a;
            	
            	// apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

				return returnTexture;
                
            }
            ENDCG
        }
    }
}
