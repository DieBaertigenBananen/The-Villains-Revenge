sampler textureSampler;
float gameTime;
bool activated;
float4 outline(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	if (activated)
	{
		color.b = 1;
	}
	else
	{
		color.r = 1;
	}
	
	return color;
}
  
technique Technique1
{  
    pass Pass1
    {
        PixelShader = compile ps_2_0 outline();
    }  
}  