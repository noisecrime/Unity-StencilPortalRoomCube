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


// Basic shader that simpy renders the specified colour for the area of the stencil .

Shader "Stencils/ViewerTextured"
{
	Properties
	{
		_StencilTable("StencilTable", 2D) = "white" {}
		_StencilReferenceID("Stencil ID Reference", Float) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry+10"}		
		ZWrite off
		ZTest Always

		Stencil 
		{
			Ref[_StencilReferenceID]
			Comp Equal
			Pass Keep
		}
		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D	_StencilTable;
			float		_StencilReferenceID;

			struct appdata 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			v2f vert(appdata v) 
			{
				v2f o;
				o.pos		= UnityObjectToClipPos(v.vertex);
				o.texcoord	= v.texcoord.xy;
				return o;
			}
			
			half4 frag(v2f i) : COLOR 
			{
				float2 coord;

				coord.x = (_StencilReferenceID % 16) / 16; // * offset;
				coord.y = 1 - (_StencilReferenceID / 16) / 16; // * offset;

				return tex2D(_StencilTable, coord + frac(i.texcoord * 32) / 16 );

				return tex2D(_StencilTable, (i.texcoord*16) + coord);
			}
		ENDCG
		}
	}
}
