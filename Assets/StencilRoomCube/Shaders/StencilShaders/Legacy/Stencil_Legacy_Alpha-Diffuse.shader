Shader "Stencils/Legacy Shaders/Transparent/Diffuse"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}

		_StencilReferenceID("Stencil ID Reference", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)]	_StencilComp("Stencil Comparison", Float) = 3	// equal
		[Enum(UnityEngine.Rendering.StencilOp)]			_StencilOp("Stencil Operation", Float) = 0		// keep
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
	}

	SubShader
	{
		Tags
		{
			"Queue"				= "Transparent"			
			"RenderType"		= "StencilTransparent"
			"IgnoreProjector"	= "True"
		}

		LOD 200

		Stencil
		{
			Ref[_StencilReferenceID]
			Comp[_StencilComp]	
			Pass[_StencilOp]	
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Transparent/VertexLit"
}

