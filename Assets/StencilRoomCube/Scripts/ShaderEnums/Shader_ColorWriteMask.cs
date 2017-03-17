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
