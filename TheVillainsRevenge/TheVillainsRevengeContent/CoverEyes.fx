sampler textureSampler;
//float playerX;
//float playerY;
float gameTime;
//bool left;

float4 blackMask(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	float blubb = 20.0f * sin(gameTime / 20);
	color = (color + tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y)) + tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y)) + tex2D(textureSampler, float2(coords.x, coords.y + blubb / 1080)) + tex2D(textureSampler, float2(coords.x, coords.y - blubb / 1080)) + tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y + blubb / 1080)) + tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y + blubb / 1080)) + tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y - blubb / 1080)) + tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y - blubb / 1080))) / 9; //Blur

	color = color * ((sin(gameTime * 0.01) + 0.5) * 0.5 + 0.5);

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