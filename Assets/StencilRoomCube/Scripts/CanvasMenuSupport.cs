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

using UnityEngine;
using UnityEngine.UI;

public class CanvasMenuSupport : MonoBehaviour
{
	public  Material				_glassWallMaterial;
	public	Text					_stencilSupportTextComponent;
	public	Text					_warningTextComponent;
	
	private PortalContentCulling[]	_portalContentCulling;
	private	GameObject[]			_glassWalls;
	private	GameObject[]			_depthMaskCubes;


	void Start ()
	{
		_glassWalls				= GameObject.FindGameObjectsWithTag("GlassWalls");
		_depthMaskCubes			= GameObject.FindGameObjectsWithTag("DepthMaskCubes");
		_portalContentCulling	= FindObjectsOfType<PortalContentCulling>();

		_stencilSupportTextComponent.text = string.Format("Stencil Support: {0}\n3D API: {1}", SystemInfo.supportsStencil.ToString(), SystemInfo.graphicsDeviceType.ToString() ) ;
	}

	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Escape ) ) Application.Quit();
	}

	public void Toggle_ImageEffect( bool val)
	{
		_warningTextComponent.gameObject.SetActive( false );
	}

	public void Toggle_PortalContentCulling()
	{
		foreach ( PortalContentCulling pcc in _portalContentCulling )
		{
			pcc.enabled = !pcc.enabled;
		}
	}

	public void Toggle_DepthMaskCubes()
	{
		foreach ( GameObject go in _depthMaskCubes )
		{
			go.SetActive( !go.activeSelf );
		}
	}

	public void Toggle_Glass()
	{
		foreach ( GameObject go in _glassWalls )
		{
			go.SetActive( !go.activeSelf );
		}
	}

	public void Set_GlassWallStrength( float val )
	{
		_glassWallMaterial.SetFloat("_BumpAmt", val );
	}

	

}
