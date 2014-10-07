Shader "Custom/QuadCardShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" 
			   "Queue"="Geometry+1"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf CardLight alpha

		sampler2D _MainTex;

		half4 LightingCardLight (SurfaceOutput s, half3 lightDir, half atten) {
              half NdotL = dot (normalize(s.Normal), normalize(lightDir));
              half4 c;
              c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
              c.a = 1;
              return c;
         }
         
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
