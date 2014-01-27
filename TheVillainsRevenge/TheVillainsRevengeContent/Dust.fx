sampler textureSampler;
float playerX;
float playerY;
float gameTime;
int intensity;

float4 dust(float2 coords: TEXCOORD) : COLOR
{
	float2 playerPos = float2(playerX, playerY);
	float4 color = tex2D(textureSampler, coords);
	
	float blubb = (float)intensity * (sin(gameTime / 10) * 0.5 + 0.5);
	int weight = 4;
	float3 c0 = (0.5, 0.5, 0.5);
	float4 c1 = tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y));
	float4 c2 = tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y));
	float4 c3 = tex2D(textureSampler, float2(coords.x, coords.y + blubb / 1080));
	float4 c4 = tex2D(textureSampler, float2(coords.x, coords.y - weight * blubb / 1080));
	float4 c5 = tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y + blubb / 1080));
	float4 c6 = tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y + blubb / 1080));
	float4 c7 = tex2D(textureSampler, float2(coords.x + blubb / 1920, coords.y - weight * blubb / 1080));
	float4 c8 = tex2D(textureSampler, float2(coords.x - blubb / 1920, coords.y - weight * blubb / 1080));
	color.rgb = (color.rgb + c1.rgb + c2.rgb + c3.rgb + c4.rgb + c5.rgb + c6.rgb + c7.rgb + c8.rgb) / 9;
	//color = (color + c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8) / 9;

	return color;
}  
  
technique Technique1
{  
    pass Pass1
    {
		PixelShader = compile ps_2_0 dust();
	}
}  