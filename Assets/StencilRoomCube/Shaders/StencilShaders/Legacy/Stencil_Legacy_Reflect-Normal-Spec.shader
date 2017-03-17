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


Shader "Stencils/Legacy Shaders/Reflective/Bumped Specular" 
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5,0.5,0.5,1)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
		_MainTex("Base (RGB) RefStrGloss (A)", 2D) = "white" {}
		_Cube("Reflection Cubemap", Cube) = "" {}
		_BumpMap("Normalmap", 2D) = "bump" {}

		_StencilReferenceID("Stencil ID Reference", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 3
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
	}

	SubShader
	{
		Tags
		{
			"Queue"				= "Geometry"
			"RenderType"		= "StencilOpaque"
		}

		LOD 400

		Stencil
		{
			Ref[_StencilReferenceID]
			Comp[_StencilComp]	// equal
			Pass[_StencilOp]	// keep
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0

		sampler2D	_MainTex;
		sampler2D	_BumpMap;
		samplerCUBE _Cube;

		fixed4		_Color;
		fixed4		_ReflectColor;
		half		_Shininess;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			INTERNAL_DATA
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			o.Albedo = c.rgb;

			o.Gloss = tex.a;
			o.Specular = _Shininess;

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			float3 worldRefl = WorldReflectionVector(IN, o.Normal);
			fixed4 reflcol = texCUBE(_Cube, worldRefl);
			reflcol *= tex.a;
			o.Emission = reflcol.rgb * _ReflectColor.rgb;
			o.Alpha = reflcol.a * _ReflectColor.a;
		}
		ENDCG
	}

	FallBack "Legacy Shaders/Reflective/Bumped Diffuse"
}
