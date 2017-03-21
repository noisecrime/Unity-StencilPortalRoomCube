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
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NoiseCrimeStudios.Utilities.ColorSwatchPalette
{
	public class PaletteTextureToColorSwatchPalette : MonoBehaviour
	{
		public class ColorData
		{
			public Color    color;
			public Vector3	positionRGB;
			public Vector3	positionHSV;
		}

		public	enum SwatchSortMode		{ None, Ascending, Descending, MaxDifferenceRGB, MaxDifferenceHSV };

		public  Texture2D               m_SwatchColorTexture;
		public	int						m_SwatchesInTexture = 256;
		public	int						m_SkipColorCount	= 0;

		#region CONTEXT MENU METHODS
		[ContextMenu( "Build SwatchPalette None" )]
		public void BuildColorSwatch_None()					{ BuildSwatchPalette( SwatchSortMode.None ); }

		[ContextMenu( "Build SwatchPalette Ascending" )]
		public void BuildColorSwatch_Ascending()			{ BuildSwatchPalette( SwatchSortMode.Ascending ); }

		[ContextMenu( "Build SwatchPalette Descending" )]
		public void BuildColorSwatch_Descending()			{ BuildSwatchPalette( SwatchSortMode.Descending ); }

		[ContextMenu( "Build SwatchPalette RGB MaxDifference" )]
		public void BuildColorSwatch_MaxDifferenceRGB()		{ BuildSwatchPalette( SwatchSortMode.MaxDifferenceRGB ); }
		
		[ContextMenu( "Build SwatchPalette HSV MaxDifference" )]
		public void BuildColorSwatch_MaxDifferenceHSV()		{ BuildSwatchPalette( SwatchSortMode.MaxDifferenceHSV ); }
		#endregion



#if UNITY_EDITOR
		public void BuildSwatchPalette( SwatchSortMode swatchSortMode)
		{
			ColorSwatchPalette colorSwatchPalette = CreateColorSwatchAsset();

			if ( null == colorSwatchPalette )
			{
				Debug.LogError( "Unable to create ColorSwatchPalette asset" );
				return;
			}			

			// Get Colors from texture
			Color[] srcSwatches = GetColorsFromTextureSwatch();

			// Apply sort mode
			switch ( swatchSortMode )
			{
				case SwatchSortMode.Ascending:			SortByLinq( srcSwatches, swatchSortMode ); break;
				case SwatchSortMode.Descending:			SortByLinq( srcSwatches, swatchSortMode ); break;
				case SwatchSortMode.MaxDifferenceRGB:	SortByMaxDiffRGB( srcSwatches ); break;
				case SwatchSortMode.MaxDifferenceHSV:	SortByMaxDiffHSV( srcSwatches ); break;
			}			

			// Put colors into asset
			colorSwatchPalette.m_SwatchColors = srcSwatches;

			// Save Asset
			SaveUpdatedAssets( colorSwatchPalette );
		}

		void SaveUpdatedAssets( ColorSwatchPalette colorSwatchPalette)
		{
			EditorUtility.SetDirty(colorSwatchPalette);
			AssetDatabase.SaveAssets ();
		}

		ColorSwatchPalette CreateColorSwatchAsset()
		{
			string lastpath = EditorPrefs.GetString("NoiseCrimeStudiosLastPath", "");
			if( string.IsNullOrEmpty(lastpath) ) lastpath = Application.dataPath + "/assets";

			string filePath = EditorUtility.SaveFilePanelInProject("Save ColorSwatchPalette", "ColorSwatchPalette", "asset", "", lastpath);
			if ( string.IsNullOrEmpty (filePath ) ) return null;
			EditorPrefs.SetString("NoiseCrimeStudiosLastPath", System.IO.Path.GetDirectoryName( filePath));

			ColorSwatchPalette colorSwatchPalette = (ColorSwatchPalette) ScriptableObject.CreateInstance( typeof( ColorSwatchPalette ) );
			AssetDatabase.CreateAsset( colorSwatchPalette, filePath );
			AssetDatabase.SaveAssets ();

			return colorSwatchPalette;
		}
#endif

		Color[] GetColorsFromTextureSwatch()
		{
			Color[] swatches	= new Color[m_SwatchesInTexture];

			int SwatchesPerRow	= (int) Mathf.Sqrt( m_SwatchesInTexture );

			int offset = (m_SwatchColorTexture.width/SwatchesPerRow);

			for ( int i = 0; i < m_SwatchesInTexture; i++)
			{
				int x = (i % SwatchesPerRow) * offset;
				int y = (SwatchesPerRow - 1 - (i / SwatchesPerRow)) * offset;
				swatches[i] = m_SwatchColorTexture.GetPixel( x, y);				
			}

			return swatches;
		}

		List<ColorData> PreCalculateColorData( Color[] srcSwatches )
		{
			List<ColorData> colorDataList = new List<ColorData>(srcSwatches.Length - m_SkipColorCount);

			for ( int i = m_SkipColorCount; i < srcSwatches.Length; i++ )
			{
				Vector3 colRGB = new Vector3( srcSwatches[i].r, srcSwatches[i].g, srcSwatches[i].b );
				Vector3 colHSV = Vector3.zero;
				Color.RGBToHSV( srcSwatches[ i ], out colHSV.x, out colHSV.y, out colHSV.z );

				ColorData cd = new ColorData() { color = srcSwatches[i], positionRGB = colRGB, positionHSV = colHSV } ;
				colorDataList.Add( cd );
			}

			return colorDataList;
		}

		// Can probably be done better with custom sorting method comparrision test in linq?
		void SortByLinq( Color[] srcSwatches, SwatchSortMode swatchSortMode )
		{
			List<ColorData> colorDataList = PreCalculateColorData( srcSwatches );
		//	List<ColorData> sortedList;

			if ( swatchSortMode == SwatchSortMode.Ascending )
				colorDataList  = colorDataList.OrderBy(o=>o.positionRGB.magnitude).ToList();
			else
				colorDataList = colorDataList.OrderByDescending(o=>o.positionRGB.magnitude).ToList();


			for ( int i = m_SkipColorCount; i < srcSwatches.Length; i++ )
			{
				srcSwatches[ i ] = colorDataList[ i - m_SkipColorCount ].color;
			}

			// srcSwatches = sortedList.Select( o => o.color ).ToArray();
		}

		void SortByMaxDiffRGB( Color[] srcSwatches )
		{
			List<ColorData> colorDataList = PreCalculateColorData( srcSwatches );

			int i = m_SkipColorCount;

			srcSwatches[ i++ ] = colorDataList[ 0 ].color;
			Vector3 lastPos  = colorDataList[0].positionRGB;
			colorDataList.RemoveAt( 0 );

			while ( colorDataList.Count > 0 )
			{
				float   bestDistance = -1f;
				int     bestIndex    =  0;

				for ( int b = 1; b < colorDataList.Count; b++ )
				{
					if ( Vector3.Distance( lastPos, colorDataList[ b ].positionRGB ) > bestDistance )
					{
						bestDistance = Vector3.Distance( lastPos, colorDataList[ b ].positionRGB );
						bestIndex = b;
					}
				}

				lastPos = colorDataList[ bestIndex ].positionRGB;
				srcSwatches[ i++ ] = colorDataList[ bestIndex ].color;
				colorDataList.RemoveAt( bestIndex );
			}
		}


		void SortByMaxDiffHSV( Color[] srcSwatches )
		{
			List<ColorData> colorDataList = PreCalculateColorData( srcSwatches );

			int i = m_SkipColorCount;
			bool toggle = true;
			float rotateHue = 0;

			srcSwatches[ i++ ] = colorDataList[ 0 ].color;
			Vector3 lastPos  = colorDataList[0].positionHSV;
			colorDataList.RemoveAt( 0 );

			while ( colorDataList.Count > 0 )
			{
				//	int bestIndex = toggle ? FindFurthestDistance( colorDataList, lastPos ) : FindClosestDistance( colorDataList, lastPos );							
				//	toggle = !toggle;
				int bestIndex = FindOppositeColor( colorDataList, lastPos, rotateHue );

				rotateHue = rotateHue + 1 / 16f;
				if ( rotateHue > 1f ) rotateHue -= 1f;

				lastPos = colorDataList[ bestIndex ].positionHSV;
				srcSwatches[ i++ ] = colorDataList[ bestIndex ].color;
				colorDataList.RemoveAt( bestIndex );
			}
		}		
		

		int FindOppositeColor( List<ColorData> colorDataList, Vector3 lastPos, float rotateHue )
		{
			float   bestDistance = -1f;
			int     bestIndex    =  0;
			float	bestHue		 =  0f;

			for ( int b = 1; b < colorDataList.Count; b++ )
			{
				Vector3 newPos = colorDataList[ b ].positionHSV;

				// Find most opposite color
				float huediff  = Mathf.Abs( ( lastPos.x - (rotateHue * 0.5f)) - newPos.x);
				huediff = huediff * Mathf.Sin( huediff * Mathf.PI);

				if ( huediff > bestHue )
					{
						bestHue = huediff;
						bestIndex = b;
					}

				/*
				if ( huediff > bestHue )
				{
					bestHue = huediff;
					float dist =  Vector2.Distance( new Vector2(lastPos.y, lastPos.z ), new Vector2(newPos.y, newPos.z ) );
				
					if ( dist > bestDistance )
					{
						bestDistance = dist;
						bestIndex = b;
					}
				}*/
			}

			return bestIndex;
		}


		int FindFurthestDistance( List<ColorData> colorDataList, Vector3 lastPos )
		{
			float   bestDistance = -1f;
			int     bestIndex    =  0;

			for ( int b = 1; b < colorDataList.Count; b++ )
			{
				Vector3 newPos = colorDataList[ b ].positionHSV;
				float dist =  Vector2.Distance( new Vector2(lastPos.y, lastPos.z ), new Vector2(newPos.y, newPos.z ) );
				dist = dist + (Mathf.Abs( lastPos.x - newPos.x) * 1.4f);


				if ( dist > bestDistance )
				{
					bestDistance = dist;
					bestIndex = b;
				}
			}

			return bestIndex;
		}

		int FindClosestDistance( List<ColorData> colorDataList, Vector3 lastPos )
		{
			float   bestDistance = float.MaxValue;
			int     bestIndex    =  0;

			for ( int b = 1; b < colorDataList.Count; b++ )
			{
				Vector3 newPos = colorDataList[ b ].positionHSV;
				float dist = Vector2.Distance( new Vector2(lastPos.y, lastPos.z ), new Vector2(newPos.y, newPos.z ) );
				dist = dist + (Mathf.Abs( lastPos.x - newPos.x) * 1.4f);

				if ( dist < bestDistance )
				{
					bestDistance = dist;
					bestIndex = b;
				}
			}

			return bestIndex;
		}
	}
}
