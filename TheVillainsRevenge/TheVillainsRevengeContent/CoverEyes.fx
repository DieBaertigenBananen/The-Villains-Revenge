sampler textureSampler;
float playerX;
float playerY;
float gameTime;

float4 blackMask(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);

	float2 playerPos = float2(playerX, playerY);
	playerPos.y = playerPos.y - 0.03;

	float deltaViewX = coords.x - playerPos.x;
	float deltaY = deltaViewX * 0.8;

    if (coords.x > playerPos.x && coords.y > playerPos.y - deltaY && coords.y < playerPos.y + deltaY && distance(playerPos, coords) < 0.2)
	{
		color = color;
	}
	else
	{
		color.rgb = 0;
	}
	return color;
}  

float4 blur(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	float blubb = 100.0f * sin(gameTime / 10);
	color = (tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y)) + tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y)) + tex2D(textureSampler, float2(coords.x, coords.y + blubb / 1080)) + tex2D(textureSampler, float2(coords.x, coords.y - blubb / 1080))) / 4;

	return color;
}  
  
technique Technique1
{  
    pass Pass1
    {  
        //PixelShader = compile ps_2_0 blackMask();  
    }  
	pass Pass2
	{
		PixelShader = compile ps_2_0 blur();
	}
}  