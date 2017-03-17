// Basic shader that simpy renders the specified colour for the area of the stencil .

Shader "Stencils/ViewerTextured"
{
	Properties
	{
		_StencilTable("StencilTable", 2D) = "white" {}
		_StencilReferenceID("Stencil ID Reference", Float) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry+10"}		
		ZWrite off
		ZTest Always

		Stencil 
		{
			Ref[_StencilReferenceID]
			Comp Equal
			Pass Keep
		}
		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D	_StencilTable;
			float		_StencilReferenceID;

			struct appdata 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			v2f vert(appdata v) 
			{
				v2f o;
				o.pos		= mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord	= v.texcoord.xy;
				return o;
			}
			
			half4 frag(v2f i) : COLOR 
			{
				float2 coord;

				coord.x = (_StencilReferenceID % 16) / 16; // * offset;
				coord.y = 1 - (_StencilReferenceID / 16) / 16; // * offset;

				return tex2D(_StencilTable, coord + frac(i.texcoord * 32) / 16 );

				return tex2D(_StencilTable, (i.texcoord*16) + coord);
			}
		ENDCG
		}
	}
}
