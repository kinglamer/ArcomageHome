Shader "Custom/Done_CardShader" {
	Properties {
		_CardBack ("Card Back", 2D) = "white" {}
		_Picture("Card Picture",2D) = "white" {}
		_BlendTex("Blend Texture",2D) = "white" {}
		_Light("Lighter",Float)=0.7
	}
	SubShader {
		Tags { "RenderType"="Opaque"
			   "Queue"="Transparent"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _CardBack;
		sampler2D _Picture;
		sampler2D _BlendTex;
		float _Light;

		struct Input {
			float2 uv_CardBack;
			float2 uv_Picture;
			float2 uv_BlendTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 cardback = tex2D (_CardBack, IN.uv_CardBack);
			float2 picUV = IN.uv_Picture;
			picUV.x=clamp(clamp(picUV.x-(20.0/300.0),0,1)*(300.0/260.0),0,1);
			picUV.y=clamp(clamp(picUV.y-(260.0/500.0),0,1)*(500.0/180.0),0,1);
			half4 picture = tex2D (_Picture, picUV);
			half4 blendtex = tex2D (_BlendTex, IN.uv_BlendTex);
//			o.Albedo = picture.rgb;
			o.Albedo = lerp(picture,cardback,blendtex.r)*_Light;
			o.Alpha = cardback.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}