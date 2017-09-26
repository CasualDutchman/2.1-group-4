// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders101/Simple Texture"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader
	{
		Tags
	{
		"PreviewType" = "Plane"
	}
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
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 screenuv : TEXCOORD1;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		//o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) * 0.5;
		o.screenuv = ((float4(o.vertex.x, -o.vertex.y, o.vertex.z, o.vertex.w).xy / o.vertex.w) + 1) * 0.5;
		return o;
	}

	sampler2D _MainTex;

	float4 frag(v2f i) : SV_Target
	{
		float4 tex = tex2D(_MainTex, i.screenuv);
		return tex;
	}
		ENDCG
	}
	}
}