Shader "Star/Effect/Tint Particle HDR" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        [HDR]_diffuse_color ("diffuse_color", Color) = (0.5,0.5,0.5,1)
        _opacity ("opacity", Float ) = 1
        _diff_speed_xu_yv ("diff_speed_x", Vector) = (0,0,0,0)

        _MaskTilingSpeed ("MaskTilingSpeed", Vector) = (0,0,0,0)
        _mask ("mask", 2D) = "white" {}
        [Enum(R,0,A,1)] _MaskChannel ("MaskChannel", Float) = 0

        _MaskTilingSpeed2 ("MaskTilingSpeed", Vector) = (0,0,0,0)
        _mask2 ("mask", 2D) = "white" {}
        [Enum(R,0,A,1)] _MaskChannel2 ("MaskChannel2", Float) = 0

        _noise_rongjie ("noise_rongjie", 2D) = "white" {}
        _edge_width ("edge_width", Float ) = 0.02
        [HDR]_edge_color ("edge_color", Color) = (0.5,0.5,0.5,1)
        _edge_power ("edge_power", Float ) = 2
        _diffuse_power ("diffuse_power", Float ) = 1
        _rongjie ("rongjie", Float ) = 0
        _diff_raodong ("diff_raodong", Float ) = 0

        _noise01 ("noise01", 2D) = "white" {}
        _raodong_power ("raodong_power", Float ) = 0.1
        _noise1_speed ("noise1_speed",Vector) = (0,0,0,0)

        _noiseRongj_speed("noiseRongj_speed", Vector) = (0,0,0,0)

        _RotationSpeed ("RotationSpeed", Float) = 0
        _RotationPivot ("RotationPivot", Vector) = (0.5,0.5,1,1)

        [HideInInspector] _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [HideInInspector] _QueueOffset ("__offset", Float) = 0.0
		[HideInInspector] _PZWrite ("__zw", Float) = 0.0
		[HideInInspector] _CullMode ("__cull", Float) = 0.0
		[HideInInspector] _BlendMode ("__cull", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 5.0
		[HideInInspector] _DstBlend ("__dst", Float) = 10.0
        [HideInInspector] _ZTestAddValue ("ZTest Add Value", Float) = 0
        [HideInInspector] _PolarUV ("PolarUV", Float) = 0

        [Enum(Custom1,1,Custom2,2)] _CustomDataDissolve ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentDissolve ("CustomComponent", Float) = 0
        [Enum(Custom1,1,Custom2,2)] _CustomDataU ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentU ("CustomComponent", Float) = 0
        [Enum(Custom1,1,Custom2,2)] _CustomDataV ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentV ("CustomComponent", Float) = 0
        [Enum(Custom1,1,Custom2,2)] _CustomDataMaskU ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentMaskU ("CustomComponent", Float) = 0
        [Enum(Custom1,1,Custom2,2)] _CustomDataMaskV ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentMaskV ("CustomComponent", Float) = 0
        [Enum(Custom1,1,Custom2,2)] _CustomDataMask2U ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentMask2U ("CustomComponent", Float) = 0
        [Enum(Custom1,1,Custom2,2)] _CustomDataMask2V ("CustomData", Float) = 0
        [Enum(X,0,Y,1,Z,2,W,3)] _CustomComponentMask2V ("CustomComponent", Float) = 0
    }
    SubShader {
        Tags { "IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass {
            Blend [_SrcBlend] [_DstBlend]
            Cull [_CullMode]
            ZWrite [_PZWrite]
            Lighting Off
            
            CGPROGRAM

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "CommonEffect.cginc"

            ENDCG
        }
    }
	CustomEditor "ParticleHDRShaderGUI"
}