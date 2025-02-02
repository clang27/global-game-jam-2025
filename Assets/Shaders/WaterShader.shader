Shader "Sprites/WaterShader" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend DstColor SrcColor
        
        Pass {
            CGPROGRAM
                #pragma vertex SpriteVert
                #pragma fragment SpriteWaterFrag
                #pragma target 2.0
                #pragma multi_compile_instancing
                #pragma multi_compile_local _ PIXELSNAP_ON
                #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
                #include "UnitySprites.cginc"

                float3 HUEtoRGB(in float H) {
                    float R = abs(H * 6 - 3) - 1;
                    float G = 2 - abs(H * 6 - 2);
                    float B = 2 - abs(H * 6 - 4);
                    return saturate(float3(R,G,B));
                }
                
                float Epsilon = 1e-10;
                 
                float3 RGBtoHCV(in float3 RGB) {
                    // Based on work by Sam Hocevar and Emil Persson
                    float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0/3.0) : float4(RGB.gb, 0.0, -1.0/3.0);
                    float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
                    float C = Q.x - min(Q.w, Q.y);
                    float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
                    return float3(H, C, Q.x);
                }

                float3 HSVtoRGB(in float3 HSV) {
                    float3 RGB = HUEtoRGB(HSV.x);
                    return ((RGB - 1) * HSV.y + 1) * HSV.z;
                }
                
                fixed4 SpriteWaterFrag(v2f IN) : SV_Target {
                    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                    float3 hsv = RGBtoHCV(float3(c.r, c.g, c.b));
                    
                    c.rgb = HSVtoRGB(float3(hsv.x, 1, hsv.z));
                    return c;
                }
            ENDCG
        }
    }
}
