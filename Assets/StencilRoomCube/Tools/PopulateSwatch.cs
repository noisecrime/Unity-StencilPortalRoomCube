using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PopulateSwatch : MonoBehaviour
{
	public	GameObject		m_SwatchTextPrefab;
	public	GameObject		m_InstructionsPanel;

	void Start ()
	{
		if ( Screen.width != 512 || Screen.height != 512 )
		{
			Debug.LogWarning( "Game Resolution is not 512x512" );
			return;
		}

		StartCoroutine( PopulateSwatchExport() );
	}

	IEnumerator PopulateSwatchExport()
	{
		for ( int i = 0; i < 256; i++ )
		{
			GameObject g = Instantiate(m_SwatchTextPrefab);
			g.GetComponent<Text>().text = i.ToString();
			g.transform.SetParent(transform);
		}

		m_InstructionsPanel.SetActive( false );

		yield return null;

		Application.CaptureScreenshot("Swatch_" + DateTimeOffset.Now.ToString("yyyy_MM_dd_HHmmss") + "_F" + Time.frameCount.ToString() + ".png" ); // UTC Date + frame index

		yield return null;

		m_InstructionsPanel.SetActive( true );

		Debug.Log("PopulateSwatch: Stencil Swatch Texture created and saved in Project Root");
	}

		
}
