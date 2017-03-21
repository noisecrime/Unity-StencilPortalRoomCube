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
using System;
using System.Collections;

namespace NoiseCrimeStudios.Utilities.ColorSwatchPalette
{
	/// <summary>
	/// Uses uGUI to add numbers to a texturePalette and exports result via CaptureScreenshot to Project Root.
	/// </summary>
	public class PaletteTextureBuilder : MonoBehaviour
	{
		public  GameObject				m_SwatchTextPrefab;
		public  GameObject				m_InstructionsPanel;
		public	Transform				m_SwatchNumberParent;

		public	ColorSwatchPalette		m_ColorSwatchPalette;

		void OnEnable()
		{
			if ( Screen.width != 512 || Screen.height != 512 )
			{
				Debug.LogWarning( "Game Resolution is not 512x512" );
				return;
			}

			StartCoroutine( PopulatePaletteExport() );
		}

		IEnumerator PopulatePaletteExport()
		{
			for ( int i = 0; i < 256; i++ )
			{
				GameObject g = Instantiate(m_SwatchTextPrefab);
				g.GetComponent<Text>().text = i.ToString();
				g.transform.SetParent( m_SwatchNumberParent );
			}

			m_InstructionsPanel.SetActive( false );

			yield return null;

			Application.CaptureScreenshot( "Swatch_" + DateTimeOffset.Now.ToString( "yyyy_MM_dd_HHmmss" ) + "_F" + Time.frameCount.ToString() + ".png" ); // UTC Date + frame index

			yield return null;

			m_InstructionsPanel.SetActive( true );

			Debug.Log( "PopulateSwatchPalette: Stencil Swatch Texture created and saved in Project Root" );
		}
	}
}
