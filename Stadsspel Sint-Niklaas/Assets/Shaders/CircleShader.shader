Shader "Custom/Circle" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_Opacity("Opacity", Range(0.0, 1)) = 0.2
		_Thickness("Thickness", Range(0.0, 1)) = 0.1
		_Smoothing("Smoothing", Range(0.0, 1)) = 0.1
	}

	SubShader {
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"}
		Pass{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			fixed4 _Color; // low precision type is usually enough for colors
			float _Opacity;
			float _Thickness;
			float _Smoothing;

			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXTCOORD0;
			};

			fragmentInput vert(appdata_base v)
			{
				fragmentInput o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy - fixed2(.5,.5);

				return o;
			}

			// r = radius
			// o = opacity
			// d = distance
			// t = thickness
			// s = % thickness used for smoothing
			float antialias(float r, float o, float d, float t, float sr)
			{
				if(d < (r - t))
				{
					float temp = 1 - pow(d - r + t, 2) / sr;
					if(temp < o)
						return o;
					else
						return temp;
				}
				else if(d > (r + t))
					return 1 - pow(d - r - t,2) / sr;
				else
					return 1.0;
			}

			fixed4 frag(fragmentInput i) : SV_Target{
				float distance = sqrt(pow(i.uv.x, 2) + pow(i.uv.y,2));
				_Thickness *= .2; //Divide by four
				float radius = .5 - _Thickness;
				float smoothingRadius = _Smoothing*radius;
				radius -= smoothingRadius;
				smoothingRadius = pow(smoothingRadius, 2);
				return fixed4(_Color.r, _Color.g, _Color.b, _Color.a*antialias(radius, _Opacity, distance, _Thickness, smoothingRadius));
			}

			ENDCG
		}
	}
}