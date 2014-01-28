#define RADIUS 7
#define KERNEL_SIZE (RADIUS * 2+1)
sampler textureSampler;

float kernel[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];


float4 Blur(float2 coords: TEXCOORD) : COLOR
{
	float4 color.rgb = float4(0.0f, 0.0f, 0.0f, 0.0f);
	for (int i = 0; i < KERNEL_SIZE; ++i)
	{
        color += tex2D(textureSampler, coords + offsets[i]) * kernel[i];
	}
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 Blur();
    }
}