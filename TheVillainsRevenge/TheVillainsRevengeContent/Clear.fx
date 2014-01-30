sampler textureSampler;

float4 clear(float2 coords: TEXCOORD) : COLOR
{
	return 0.0;
}
  
technique Technique1
{  
    pass Pass1
    {
        PixelShader = compile ps_2_0 clear();
    }  
}  