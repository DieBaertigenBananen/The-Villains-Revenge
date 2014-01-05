sampler textureSampler;
float playerX;
float playerY;
float gameTime;
int lineSize;
int lineBrightness;
int aura;

float4 outline(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	
	float2 playerPos = float2(playerX, playerY);
	playerPos.y = playerPos.y - 0.05;
	float size = ((float)lineSize / 10);
	float2 thick = float2(size / 1920, size / 1080);
	
	int count = 0;
	float c1 = tex2D(textureSampler, float2(coords.x + thick.x, coords.y)).a;
	float c2 = tex2D(textureSampler, float2(coords.x - thick.x, coords.y)).a;
	float c3 = tex2D(textureSampler, float2(coords.x, coords.y + thick.y)).a;
	float c4 = tex2D(textureSampler, float2(coords.x, coords.y - thick.y)).a;
	if (c1 != 0)
	{
		count++;
	}
	if (c2 != 0)
	{
		count++;
	}
	if (c3 != 0)
	{
		count++;
	}
	if (c4 != 0)
	{
		count++;
	}
	if (distance(playerPos, coords) < (float)aura / 1080)
	{
		color.rgb = 1 * (distance(playerPos, coords) / ((float)aura / 1080));
	}
	if (count < 4 && count > 0)
	{
		color.rgb = ((float)lineBrightness / 10);
		color.a = 1;
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