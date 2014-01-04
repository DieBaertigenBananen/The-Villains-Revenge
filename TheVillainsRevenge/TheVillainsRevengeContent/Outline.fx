sampler textureSampler;
float playerX;
float playerY;
float gameTime;

float4 outline(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	float size = 1.0;
	float2 thick = float2(size / 1920, size / 1080);
	float c1 = tex2D(textureSampler, float2(coords.x + thick.x, coords.y)).a;
	float c2 = tex2D(textureSampler, float2(coords.x - thick.x, coords.y)).a;
	float c3 = tex2D(textureSampler, float2(coords.x, coords.y + thick.y)).a;
	float c4 = tex2D(textureSampler, float2(coords.x, coords.y - thick.y)).a;
	if ((c1 == 0 || c2 == 0 || c3 == 0 || c4 == 0) && (c1 != 0 || c2 != 0 || c3 != 0 || c4 != 0))
	{
		color.rgb = 0.2;
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