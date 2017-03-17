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
		if ( !val )
			_warningTextComponent.gameObject.SetActive( false );
		else
		{
			if ( SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.Direct3D9 )
			{
				_warningTextComponent.text = "(Unity 5.2.2f1) Edge Detection: Only works in DX9 - Unity bug?";
				_warningTextComponent.gameObject.SetActive( true );
			}
		}
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
