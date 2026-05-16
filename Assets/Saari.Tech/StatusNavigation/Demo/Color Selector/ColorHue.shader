Shader "SaariTech/ColorHue"
{
	Properties
	{
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_4_0_g50 = 0.2;
			float ifLocalVar11_g50 = 0;
			if( i.uv_texcoord.x <= temp_output_4_0_g50 )
				ifLocalVar11_g50 = (float)1;
			float4 color31 = IsGammaSpace() ? float4(1,0,0,1) : float4(1,0,0,1);
			float4 color32 = IsGammaSpace() ? float4(1,1,0,1) : float4(1,1,0,1);
			float temp_output_5_0_g50 = 0.0;
			float temp_output_1_0_g51 = temp_output_5_0_g50;
			float4 lerpResult10_g50 = lerp( color31 , color32 , ( ( i.uv_texcoord.x - temp_output_1_0_g51 ) / ( temp_output_4_0_g50 - temp_output_1_0_g51 ) ));
			float ifLocalVar16_g50 = 0;
			if( temp_output_5_0_g50 <= i.uv_texcoord.x )
				ifLocalVar16_g50 = (float)1;
			float temp_output_4_0_g52 = 0.4;
			float ifLocalVar11_g52 = 0;
			if( i.uv_texcoord.x <= temp_output_4_0_g52 )
				ifLocalVar11_g52 = (float)1;
			float4 color42 = IsGammaSpace() ? float4(0,1,0,1) : float4(0,1,0,1);
			float temp_output_5_0_g52 = 0.2;
			float temp_output_1_0_g53 = temp_output_5_0_g52;
			float4 lerpResult10_g52 = lerp( color32 , color42 , ( ( i.uv_texcoord.x - temp_output_1_0_g53 ) / ( temp_output_4_0_g52 - temp_output_1_0_g53 ) ));
			float ifLocalVar16_g52 = 0;
			if( temp_output_5_0_g52 <= i.uv_texcoord.x )
				ifLocalVar16_g52 = (float)1;
			float temp_output_4_0_g54 = 0.6;
			float ifLocalVar11_g54 = 0;
			if( i.uv_texcoord.x <= temp_output_4_0_g54 )
				ifLocalVar11_g54 = (float)1;
			float4 color52 = IsGammaSpace() ? float4(0,1,1,1) : float4(0,1,1,1);
			float temp_output_5_0_g54 = 0.4;
			float temp_output_1_0_g55 = temp_output_5_0_g54;
			float4 lerpResult10_g54 = lerp( color42 , color52 , ( ( i.uv_texcoord.x - temp_output_1_0_g55 ) / ( temp_output_4_0_g54 - temp_output_1_0_g55 ) ));
			float ifLocalVar16_g54 = 0;
			if( temp_output_5_0_g54 <= i.uv_texcoord.x )
				ifLocalVar16_g54 = (float)1;
			float temp_output_4_0_g46 = 0.8;
			float ifLocalVar11_g46 = 0;
			if( i.uv_texcoord.x <= temp_output_4_0_g46 )
				ifLocalVar11_g46 = (float)1;
			float4 color54 = IsGammaSpace() ? float4(0,0,1,1) : float4(0,0,1,1);
			float temp_output_5_0_g46 = 0.6;
			float temp_output_1_0_g47 = temp_output_5_0_g46;
			float4 lerpResult10_g46 = lerp( color52 , color54 , ( ( i.uv_texcoord.x - temp_output_1_0_g47 ) / ( temp_output_4_0_g46 - temp_output_1_0_g47 ) ));
			float ifLocalVar16_g46 = 0;
			if( temp_output_5_0_g46 <= i.uv_texcoord.x )
				ifLocalVar16_g46 = (float)1;
			float temp_output_4_0_g48 = 1.0;
			float ifLocalVar11_g48 = 0;
			if( i.uv_texcoord.x <= temp_output_4_0_g48 )
				ifLocalVar11_g48 = (float)1;
			float4 color56 = IsGammaSpace() ? float4(1,0,1,1) : float4(1,0,1,1);
			float temp_output_5_0_g48 = 0.8;
			float temp_output_1_0_g49 = temp_output_5_0_g48;
			float4 lerpResult10_g48 = lerp( color54 , color56 , ( ( i.uv_texcoord.x - temp_output_1_0_g49 ) / ( temp_output_4_0_g48 - temp_output_1_0_g49 ) ));
			float ifLocalVar16_g48 = 0;
			if( temp_output_5_0_g48 <= i.uv_texcoord.x )
				ifLocalVar16_g48 = (float)1;
			o.Emission = ( ( ifLocalVar11_g50 * lerpResult10_g50 * ifLocalVar16_g50 ) + ( ifLocalVar11_g52 * lerpResult10_g52 * ifLocalVar16_g52 ) + ( ifLocalVar11_g54 * lerpResult10_g54 * ifLocalVar16_g54 ) + ( ifLocalVar11_g46 * lerpResult10_g46 * ifLocalVar16_g46 ) + ( ifLocalVar11_g48 * lerpResult10_g48 * ifLocalVar16_g48 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}