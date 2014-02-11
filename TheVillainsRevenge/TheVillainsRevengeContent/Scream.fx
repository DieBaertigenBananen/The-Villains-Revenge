sampler textureSampler;
int radius;
float gameTime;

float4 scream(float2 coords: TEXCOORD) : COLOR
{
	float2 position = float2(coords.x * 1920.0, coords.y * 1080.0);
	//float4 color = tex2D(textureSampler, coords);
	float4 color = float4(0.0,0.0,0.0,1.0);

	float thickness = 1.0;
	float d = distance(position,float2(900.0,400.0));
	float r = 200.0;
	if (d < (r + thickness) && d > (r - thickness))
	{
		color = 1;
	}

	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 scream();
    }  
}  