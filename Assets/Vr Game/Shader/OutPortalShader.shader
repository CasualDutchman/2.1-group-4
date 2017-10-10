Shader "Custom/Outside Portal" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_ID("Mask ID", Int) = 1

	}

		SubShader{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+2" }
		LOD 200

		Stencil
		{
			Ref[_ID]
			Comp notequal
		}


		CGPROGRAM
#pragma surface surf Standard fullforwardshadows

#pragma target 3.0

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	fixed4 _Color;
	UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutputStandard o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}