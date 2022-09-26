// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// MIT License
//
// Copyright (c) 2017 Noisecrime
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


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
				o.pos = UnityObjectToClipPos(v.vertex);
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
