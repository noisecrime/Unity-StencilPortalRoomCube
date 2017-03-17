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

using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Specifies which color components will get written into the target framebuffer.</para>
	/// </summary>
	[Flags]
	public enum Shader_ColorWriteMask
	{
		None = 0,

		/// <summary>
		///   <para>Write alpha component.</para>
		/// </summary>
		Alpha = 1,
		/// <summary>
		///   <para>Write blue component.</para>
		/// </summary>
		Blue = 2,
		/// <summary>
		///   <para>Write green component.</para>
		/// </summary>
		Green = 4,
		/// <summary>
		///   <para>Write red component.</para>
		/// </summary>
		Red = 8,
		/// <summary>
		///   <para>Write all components (R, G, B and Alpha).</para>
		/// </summary>
		All = 15
	}
}
