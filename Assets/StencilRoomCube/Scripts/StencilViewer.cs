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

/// <summary>
/// Simple script that renders the four different stencils at the end of each frame, over-drawing anything on screen.
/// Providing visualisation of the actual stencil portals.
/// StencilTable mode is work in progress - want an easy way to assign colours and to add numerical values to the display.
/// </summary>
	
using UnityEngine;

namespace NoiseCrime.StencilPortals
{
	[ExecuteInEditMode]
	public class StencilViewer : MonoBehaviour
	{
		public enum StencilViewerMode { StencilColors, StencilTable };

		public  StencilViewerMode   _stencilViewerMode;
		public  int                 _maxStencilID = 4;

		public  Color[]             _colours;
		public  Texture2D           _stencilTable;
		public  Shader              _stencilViewerColor;
		public  Shader              _stencilViewerTable;

		void Start() { }

		void OnPostRender()
		{
			if ( _stencilViewerMode == StencilViewerMode.StencilColors )
				RenderViaStencilColors();
			else
				RenderViaStencilTable();
		}


		void RenderViaStencilColors()
		{
			int maxIDs = Mathf.Min(_maxStencilID, _colours.Length);

			Material stencilViewerMat = new Material(_stencilViewerColor);

			GL.PushMatrix();
			GL.LoadOrtho();

			for ( int i = 0; i < maxIDs; i++ )
			{
				stencilViewerMat.SetColor( "_Color", _colours[ i ] );
				stencilViewerMat.SetFloat( "_StencilReferenceID", i );
				stencilViewerMat.SetPass( 0 );

				GL.Begin( GL.QUADS );
				GL.TexCoord2( 0, 0 );
				GL.Vertex3( 0.0F, 0.0F, 0 );
				GL.TexCoord2( 0, 1 );
				GL.Vertex3( 0.0F, 1.0F, 0 );
				GL.TexCoord2( 1, 1 );
				GL.Vertex3( 1.0F, 1.0F, 0 );
				GL.TexCoord2( 1, 0 );
				GL.Vertex3( 1.0F, 0.0F, 0 );
				GL.End();
			}
			GL.PopMatrix();

			if ( null != stencilViewerMat ) DestroyImmediate( stencilViewerMat );
		}

		void RenderViaStencilTable()
		{
			//	float offset = 1f/16f;

			Material stencilViewerMat = new Material(_stencilViewerTable);

			stencilViewerMat.SetTexture( "_StencilTable", _stencilTable );

			GL.PushMatrix();
			GL.LoadOrtho();

			for ( int i = 0; i < _maxStencilID; i++ )
			{
				// float y = (float)(i % 16) / 16f; // * offset;
				// float x = (float)(i / 16) / 16f; // * offset;

				stencilViewerMat.SetFloat( "_StencilReferenceID", i );
				stencilViewerMat.SetPass( 0 );


				GL.Begin( GL.QUADS );
				GL.TexCoord2( 0, 0 );
				GL.Vertex3( 0.0F, 0.0F, 0 );
				GL.TexCoord2( 0, 1 );
				GL.Vertex3( 0.0F, 1.0F, 0 );
				GL.TexCoord2( 1, 1 );
				GL.Vertex3( 1.0F, 1.0F, 0 );
				GL.TexCoord2( 1, 0 );
				GL.Vertex3( 1.0F, 0.0F, 0 );
				GL.End();

				/*						 
			GL.Begin(GL.QUADS);
			GL.TexCoord2(x, y);
			GL.Vertex3(0.0F, 0.0F, 0);
			GL.TexCoord2(x, y+offset);
			GL.Vertex3(0.0F, 1.0F, 0);
			GL.TexCoord2(x+offset,  y+offset);
			GL.Vertex3(1.0F, 1.0F, 0);
			GL.TexCoord2( x+offset, y);
			GL.Vertex3(1.0F, 0.0F, 0);
			GL.End();*/
			}
			GL.PopMatrix();

			if ( null != stencilViewerMat ) DestroyImmediate( stencilViewerMat );
		}
	}
}
