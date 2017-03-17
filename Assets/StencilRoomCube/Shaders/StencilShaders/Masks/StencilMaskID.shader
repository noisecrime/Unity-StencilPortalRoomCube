Shader "Stencils/Masks/StencilID"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)

		_StencilReferenceID("Stencil ID Reference Reference", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)]		_StencilComp("Stencil Comparison", Float)	= 8
		[Enum(UnityEngine.Rendering.StencilOp)]				_StencilOp("Stencil Operation", Float)		= 2
		_StencilWriteMask("Stencil Write Mask", Float)				= 255
		_StencilReadMask("Stencil Read Mask", Float)				= 255

		[Enum(UnityEngine.Rendering.Shader_ColorWriteMask)]	_ColorMask("Color Mask", Float) = 0
		[MaterialToggle]									_ZWrite("ZWrite", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"		= "StencilMaskOpaque" 
			"Queue"				= "Geometry-100" 
			"IgnoreProjector"	= "True"
		}
	

		Pass
		{
			ZWrite [_ZWrite]
			ColorMask[_ColorMask]

			Stencil
			{
				Ref[_StencilReferenceID]
				Comp[_StencilComp]	// always
				Pass[_StencilOp]	// replace
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4		_Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return _Color;
			}
			ENDCG
		}
	}
}


/*
Pass {
Cull Front
ZTest Less

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
ENDCG
}

Pass {
Cull Back
ZTest Greater

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
ENDCG
}

*/
