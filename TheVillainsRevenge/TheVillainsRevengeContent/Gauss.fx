sampler textureSampler;
float gameTime;
int intensity;

float4 gaussX(float2 coords: TEXCOORD) : COLOR
{
	float4 color = tex2D(textureSampler, coords);

	return color;
}

float4 gaussY(float2 coords: TEXCOORD) : COLOR
{
	float4 color = tex2D(textureSampler, coords);

	return color;
} 
  
technique Technique1
{  
    pass Pass1
    {
		PixelShader = compile ps_2_0 gaussX();
		PixelShader = compile ps_2_0 gaussY();
	}
}  