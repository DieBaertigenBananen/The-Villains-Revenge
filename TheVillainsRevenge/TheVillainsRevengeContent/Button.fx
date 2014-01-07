sampler textureSampler;
float gameTime;
bool activated;
bool nofx;

float4 outline(float2 coords: TEXCOORD) : COLOR
{
	float4 color = tex2D(textureSampler, coords);
	if (!nofx) 
	{
		if (activated)
		{
			color.r = 1;
			color.gb = 0;
		}
		else
		{
			color.b = 1;
			color.rg = 0;
		}
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