sampler textureSampler;
int radius;
float gameTime;
int playerX;
int playerY;

float4 scream(float2 coords: TEXCOORD) : COLOR
{
	float2 position = float2(coords.x * 1920.0, coords.y * 1080.0);
	float4 color = tex2D(textureSampler, coords);

	float space = 200.0; //100.0;
	float thickness = 10.0;

	if (fmod(distance(position,float2(playerX, playerY)) - radius  - fmod(gameTime * 0.1, space) + space, space) < thickness)
	{
		color.rgb = 1;
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