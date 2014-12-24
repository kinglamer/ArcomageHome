Shader "Custom/CardShader" {
	Properties {
		_CardBack ("Card Back", 2D) = "white" {}
		_Picture("Card Picture",2D) = "white" {}
		_CardBackColor("2=R,1=G,0=B",Float)=0.0
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
		float _CardBackColor;
		float _Light;

		struct Input {
			float2 uv_CardBack;
			float2 uv_Picture;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float2 CardUV = IN.uv_CardBack;
			CardUV.x = (IN.uv_CardBack.x+_CardBackColor)*0.25;
			half4 cardback = tex2D (_CardBack, CardUV);
			float2 picUV = IN.uv_Picture;
			picUV.x=clamp(clamp(picUV.x-(11.0/174.0),0,1)*(174.0/150.0),0,1);
			picUV.y=clamp(clamp(picUV.y-(109.0/253.0),0,1)*(253.0/100.0),0,1);
//			picUV.x=clamp(clamp(picUV.x-(20.0/300.0),0,1)*(300.0/260.0),0,1);
//			picUV.y=clamp(clamp(picUV.y-(260.0/500.0),0,1)*(500.0/180.0),0,1);
			half4 picture = tex2D (_Picture, picUV);
			float2 BlendUV=IN.uv_CardBack;
			BlendUV.x = (IN.uv_CardBack.x+3)*0.25;
			half4 blendtex = tex2D (_CardBack, BlendUV);
//			o.Albedo = picture.rgb;
			o.Albedo = lerp(picture,cardback,blendtex.r)*_Light;
			o.Alpha = cardback.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}