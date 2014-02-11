sampler textureSampler;
int radius;
float gameTime;
int playerX;
int playerY;

float4 scream(float2 coords: TEXCOORD) : COLOR
{
	float2 position = float2(coords.x * 1920.0, coords.y * 1080.0);
	float4 color = tex2D(textureSampler, coords);
	color = 0;

	float thickness = 1.0;
	float d = distance(position,float2(playerX, playerY));
	float r = (float)radius;
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