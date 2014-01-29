sampler textureSampler;
float playerX;
float playerY;
float gameTime;
float force;

float4 dust(float2 coords: TEXCOORD) : COLOR
{
	float2 playerPos = float2(playerX, playerY);
	float4 color = tex2D(textureSampler, coords);
	//color = noise();
	//float blubb = (float)force * (sin(gameTime / 10) * 0.5 + 0.5);
	return color;
}
  
technique Technique1
{  
    pass Pass1
    {
		PixelShader = compile ps_2_0 dust();
	}
}