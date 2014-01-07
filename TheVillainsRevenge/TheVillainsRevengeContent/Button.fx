sampler textureSampler;
float gameTime;
bool activated;
float4 outline(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	if (activated)
	{
		color.rgb = 0.5;
	}
	else
	{
		color.rgb = color.rgb;
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