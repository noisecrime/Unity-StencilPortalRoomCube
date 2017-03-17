// MaterialPropertyBlocks cannot be used to set shader state properties such as stencil reference due to it not being support or a bug in Unity!
// Tested from Unity 4.7.1 up to 5.6.0.b10 - fails in every version.

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
		m_MaterialPropertyBlock.AddFloat("_StencilReferenceID", (float)m_StencilReferenceID);

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
