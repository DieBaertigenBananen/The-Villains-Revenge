sampler textureSampler;
float gameTime;

float4 pause(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	color.rgb = (color.r + color.g + color.b) / 3;
	float bar = 8 * sqrt(tan(coords.y - gameTime * 0.05 + coords.x * (1080.0f/1920.0f)));
	if (bar < 1)
	{
		color = color * bar;
	}
	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 pause();
    }  
}  