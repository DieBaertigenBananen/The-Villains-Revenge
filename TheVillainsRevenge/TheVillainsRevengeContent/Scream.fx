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
	float dist = distance(position,float2(playerX, playerY));
	float space = 200.0 + sin(dist * 0.5); //100.0;
	float thickness = 5.0;
	float speed = 0.5;

	if (fmod(dist - radius  - fmod(gameTime * speed, space) + space, space) < thickness)
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