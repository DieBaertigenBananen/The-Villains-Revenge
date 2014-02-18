sampler textureSampler;
int radius;
float gameTime;
int heroX;
int heroY;
bool flipped;

float4 attack(float2 coords: TEXCOORD) : COLOR
{
	float2 position = float2(coords.x * 1920.0, coords.y * 1080.0);
	float4 color = tex2D(textureSampler, coords);
	float dist = distance(position,float2(heroX, heroY));
	float space = 100;
	float waves = fmod(dist - fmod(radius, space) + space, space) / space;

	
	//Kegel
	float2 playerPos = float2(heroX, heroY);
	float deltaViewX = position.x - playerPos.x;
	float deltaY = (abs(radius) - deltaViewX) * 0.3;
	if (position.x > playerPos.x && position.y > playerPos.y - deltaY && position.y < playerPos.y + deltaY && dist < radius)
	{
		color.rgb = waves;
	}


	
	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 attack();
    }  
}  