sampler textureSampler;
//float playerX;
//float playerY;
float gameTime;
//bool left;

float4 blackMask(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	float x = gameTime * 0.04f;
	float offsetVertical = 10.0f * (tan(x) / sin(x));
	float offsetHorizontal = offsetVertical * 3;
	color = (color + tex2D(textureSampler, float2(coords.x + offsetHorizontal / 1920, coords.y)) + tex2D(textureSampler, float2(coords.x - offsetHorizontal / 1920, coords.y)) + tex2D(textureSampler, float2(coords.x, coords.y + offsetVertical / 1080)) + tex2D(textureSampler, float2(coords.x, coords.y - offsetVertical / 1080)) + tex2D(textureSampler, float2(coords.x + offsetHorizontal / 1920, coords.y + offsetVertical / 1080)) + tex2D(textureSampler, float2(coords.x - offsetHorizontal / 1920, coords.y + offsetVertical / 1080)) + tex2D(textureSampler, float2(coords.x + offsetHorizontal / 1920, coords.y - offsetVertical / 1080)) + tex2D(textureSampler, float2(coords.x - offsetHorizontal / 1920, coords.y - offsetVertical / 1080))) / 9; //Blur

	x = gameTime * 0.01f;
	float i = (tan(sin(x)) * 0.2f) + 0.6f;
	if (i > 1.0f)
	{
		i = 1.0f;
	}
	color.rgb = color.rgb * i;

	return color;

	//float2 playerPos = float2(playerX, playerY);
	//playerPos.y = playerPos.y - 0.05;
	//if (left)
	//{
	//	playerPos.x = -playerPos.x;
	//	coords.x = -coords.x;
	//}
	//float deltaViewX = coords.x - playerPos.x;
	//float deltaY = deltaViewX * 1;
	//
	//if (coords.x > playerPos.x && coords.y > playerPos.y - deltaY && coords.y < playerPos.y + deltaY && distance(playerPos, coords) < 0.5) //Kegel
	//{
	//
	//}
	//else
	//{
	//	color.rgb = 0;
	//}
}
  
technique Technique1
{  
    pass Pass1
    {
        PixelShader = compile ps_2_0 blackMask();
    }  
}  