#ifndef COMMON_EFFECT
#define COMMON_EFFECT

#define PI 3.1415926

CBUFFER_START(UnityPerMaterial)
half4 _diffuse_ST;
half4 _noise_rongjie_ST;
half _edge_width;
half _edge_power;
half _diffuse_power;

half4 _noise01_ST;
half _raodong_power;
half4 _edge_color;
half4 _diffuse_color;
half _opacity;
half4 _diff_speed_xu_yv;
half4 _noise1_speed;

half4 _noiseRongj_speed;
half4 _mask_ST;
half4 _mask2_ST;
half _MaskChannel;
half _MaskChannel2;
half _rongjie;
half _diff_raodong;
half _RimWidth;
half4 _RimColor;
half _RimPower;
float _ZTestAddValue;
half4 _MaskTilingSpeed;
half4 _MaskTilingSpeed2;

half _RotationSpeed;
half2 _RotationPivot;

half _PolarUV;

half _CustomDataDissolve;
half _CustomComponentDissolve;
half _CustomDataU;
half _CustomComponentU;
half _CustomDataV;
half _CustomComponentV;
half _CustomDataMaskU;
half _CustomComponentMaskU;
half _CustomDataMaskV;
half _CustomComponentMaskV;
half _CustomDataMask2U;
half _CustomComponentMask2U;
half _CustomDataMask2V;
half _CustomComponentMask2V;
CBUFFER_END

sampler2D _diffuse;
sampler2D _noise_rongjie;
sampler2D _noise01;
sampler2D _mask;
sampler2D _mask2;

struct VertexInput 
{
    half4 vertex : POSITION;
    half4 texcoord0 : TEXCOORD0;
    half3 normal : NORMAL;
    fixed4 color : COLOR;
    half4 texcoord1 : TEXCOORD1;
    half4 texcoord2 : TEXCOORD2;
};
struct VertexOutput 
{
    half4 pos : SV_POSITION;
    half4 uv : TEXCOORD0;
    half4 texcoord1 : TEXCOORD1;
    half4 texcoord2 : TEXCOORD2;
    fixed4 color : COLOR;
};
VertexOutput vert (VertexInput v) 
{
    VertexOutput o = (VertexOutput)0;
    o.uv.xy = TRANSFORM_TEX(v.texcoord0, _diffuse);
    o.uv.zw = TRANSFORM_TEX(v.texcoord0, _mask);
    o.pos = UnityObjectToClipPos(v.vertex);
    #if UNITY_REVERSED_Z
		o.pos.z += _ZTestAddValue;
	#else
		o.pos.z -= _ZTestAddValue;
	#endif
	o.color = v.color;
    // uv1和uv2用来接收粒子customdata
    o.texcoord1 = v.texcoord1;
    o.texcoord2 = v.texcoord2;
    return o;
}
            
void Rotate(inout half2 uv, half radian) {
    uv -= _RotationPivot.xy;
    half cosValue = cos(radian);
    half sinValue = sin(radian);
    half2x2 rotateMat = half2x2(cosValue, -sinValue, sinValue, cosValue);
    uv = mul(rotateMat, uv);
    uv += _RotationPivot.xy;
}

half2 PolarUV(half2 uv) {
    half2 delta = uv - half2(0.5, 0.5);
    half radius = length(delta) * 2;
    float angle = atan2(delta.x, delta.y) * 1.0/6.28;
    return half2(radius, angle);
}

half GetCustomData(VertexOutput i, float custom, float component)
{
    if (custom==0) { 
        return 0;
    }
    half4 data = i.texcoord1;
    if (custom==2) { 
        data = i.texcoord2;
    }
    half value = data.x;
    if (component==1) { 
        value = data.y;
    } else if (component==2) {
        value = data.z;
    } else if (component==3) {
        value = data.w;
    }
    return value;
}

fixed4 frag(VertexOutput i) : SV_TARGET 
{
    // 绕中心旋转uv
    Rotate(i.uv.xy, _RotationSpeed * (PI / 180.0) * _Time.y);
    // 极坐标
    float2 uv = i.uv.xy;
    if (_PolarUV>0) 
    { 
        uv = PolarUV(i.uv.xy);
    }
    // 扰动
    half2 noise_speed = float2(frac( _Time.y * _noise1_speed.xy) + uv);
    half2 noise_tiling = uv * _noise1_speed.zw ;
    half2 noise1_uv = noise_speed + noise_tiling;
    half4 noise1_dissolve = tex2D(_noise01,noise1_uv);
    half dissolve = noise1_dissolve.r;

    half diffuse_u = GetCustomData(i, _CustomDataU, _CustomComponentU);
    half diffuse_v = GetCustomData(i, _CustomDataV, _CustomComponentV);

	half2 diffuse_speed = float2(frac( _diff_speed_xu_yv.xy * _Time.y)+ uv);
    half2 diffuse_tiling = uv * _diff_speed_xu_yv.zw ;
    half2 diffuse_uv =  diffuse_speed +(dissolve*_diff_raodong) + diffuse_tiling;
    half4 diffusecolor = tex2D(_diffuse, diffuse_uv + float2(diffuse_u, diffuse_v));
                
    // 溶解
    half rongjie_value = GetCustomData(i, _CustomDataDissolve, _CustomComponentDissolve);

    half2 rongjie_speed = float2(uv + frac(_Time.y * _noiseRongj_speed.xy));
    half2 rongjie_tiling = uv * _noiseRongj_speed.zw ;
    half2 rongjie_uv = (dissolve *_raodong_power)+ rongjie_speed + rongjie_tiling;
    half4 noise_rongjie = tex2D(_noise_rongjie,rongjie_uv);
                
    half clampRouj = clamp(_rongjie + rongjie_value,0.0,5.0);
    half rongjie_dissloveA = step(noise_rongjie.r,clampRouj);
    half rongjie_dissloveB = step(clampRouj,noise_rongjie.r);

    half rongjie = lerp(rongjie_dissloveB,1,rongjie_dissloveA*rongjie_dissloveB);
    half eageA = step(noise_rongjie.r,(clampRouj+_edge_width));
    half eageB = step((clampRouj+_edge_width),noise_rongjie.r);
    half eageColor = _edge_power*(rongjie-lerp(eageB,1,eageA*eageB));
    half3 emissive = (_diffuse_color.rgb*(_diffuse_power*diffusecolor.rgb))+(eageColor*_edge_color.rgb);
    half3 finalColor = emissive;

    // 遮罩
    half mask_u = GetCustomData(i, _CustomDataMaskU, _CustomComponentMaskU);
    half mask_v = GetCustomData(i, _CustomDataMaskV, _CustomComponentMaskV);
    half4 maskColor = tex2D(_mask, i.uv.zw + _MaskTilingSpeed.xy * _Time.y + float2(mask_u, mask_v));

    half mask2_u = GetCustomData(i, _CustomDataMask2U, _CustomComponentMask2U);
    half mask2_v = GetCustomData(i, _CustomDataMask2V, _CustomComponentMask2V);
    half4 maskColor2 = tex2D(_mask2, i.uv + _MaskTilingSpeed2.xy * _Time.y + float2(mask2_u, mask2_v));

    half maskValue = _MaskChannel == 0 ? maskColor.r : maskColor.a;
    half maskValue2 = _MaskChannel2 == 0 ? maskColor2.r : maskColor2.a;
    half alpha = maskValue * maskValue2 * diffusecolor.a * (rongjie+eageColor) * _opacity;
    half4 finalRGBA = half4(finalColor, alpha);
    finalRGBA *= i.color;

    return finalRGBA;
}

#endif