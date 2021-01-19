﻿Shader "Spine/SkeletonGraphic(Unity UI)" {
	Properties {
		_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
		_MainTex ("Texture to blend", 2D) = "black" {}
        [HideInInspector]StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255

		[HideInInspector]_ColorMask("Color Mask", Float) = 15
	}
	// 2 texture stage GPUs
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType" = "Plane"}
		LOD 100

		Fog { Mode Off }
		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off
        ZTest[unity_GUIZTestMode]

         Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

        ColorMask[_ColorMask]
         
        BindChannels
        {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		Pass {
			ColorMaterial AmbientAndDiffuse
			SetTexture [_MainTex] {
				Combine texture * primary
			}
		}

	//	Pass {
	//		Name "Caster"
	//		Tags { "LightMode"="ShadowCaster" }
	//		Offset 1, 1

	//		ZWrite On
	//		ZTest LEqual
	//		Cull Off
	//		Lighting Off

	//		CGPROGRAM
	//		#pragma vertex vert
	//		#pragma fragment frag
	//		#pragma multi_compile_shadowcaster
	//		#pragma fragmentoption ARB_precision_hint_fastest
	//		#include "UnityCG.cginc"
	//		struct v2f { 
	//			V2F_SHADOW_CASTER;
	//			float2  uv : TEXCOORD1;
	//		};

	//		uniform float4 _MainTex_ST;

	//		v2f vert (appdata_base v) {
	//			v2f o;
	//			TRANSFER_SHADOW_CASTER(o)
	//			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	//			return o;
	//		}

	//		uniform sampler2D _MainTex;
	//		uniform fixed _Cutoff;

	//		float4 frag (v2f i) : COLOR {
	//			fixed4 texcol = tex2D(_MainTex, i.uv);
	//			clip(texcol.a - _Cutoff);
	//			SHADOW_CASTER_FRAGMENT(i)
	//		}
	//		ENDCG
	//	}
	//}
	//// 1 texture stage GPUs
	//SubShader {
	//	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	//	LOD 100

	//	Cull Off
	//	ZWrite Off
	//	Blend One OneMinusSrcAlpha
	//	Lighting Off

	//	Pass {
	//		ColorMaterial AmbientAndDiffuse
	//		SetTexture [_MainTex] {
	//			Combine texture * primary DOUBLE, texture * primary
	//		}
	//	}
	}
}