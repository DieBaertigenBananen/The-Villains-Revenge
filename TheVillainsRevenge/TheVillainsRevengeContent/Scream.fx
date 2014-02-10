sampler textureSampler;
int radius;




float4 scream(float2 coords: TEXCOORD) : COLOR
{  
	return 1.0f;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 scream();
    }  
}  