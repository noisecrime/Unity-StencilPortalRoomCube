// Basic shader that simpy renders the specified colour for the area of the stencil .

Shader "Stencils/Viewer"
{
	Properties
	{
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
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
			
			fixed4 _Color;
			
			struct appdata 
			{
				float4 vertex : POSITION;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
			};
			
			v2f vert(appdata v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			half4 frag(v2f i) : COLOR 
			{
				return _Color;
			}
		ENDCG
		}
	}
}
