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

/// <remarks>
/// MaterialPropertyBlocks cannot be used to set shader state properties such as stencil reference due to it not being support or a bug in Unity!
/// Tested from Unity 4.7.1 up to 5.6.0.b10 - fails in every version.
/// </remarks>

using UnityEngine;
using System.Collections;

// [ExecuteInEditMode]
public class PortalMaterialControl : MonoBehaviour
{
	[Range(0, 255)]
	public		int							m_StencilReferenceID = 1;

	void SetUpPropertyBlocks()
	{
		MaterialPropertyBlock m_MaterialPropertyBlock = new MaterialPropertyBlock();
		
		m_MaterialPropertyBlock.Clear();
		m_MaterialPropertyBlock.SetFloat("_StencilReferenceID", (float)m_StencilReferenceID);

		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		
		foreach(Renderer r in renderers)
		{
			r.SetPropertyBlock(m_MaterialPropertyBlock);
		}
	}

	void OnEnable()
	{
		SetUpPropertyBlocks();
	}

	void OnValidate()
	{
		SetUpPropertyBlocks();
	}
}
