sampler textureSampler;
int radius;
float gameTime;
int playerX;
int playerY;

float4 scream(float2 coords: TEXCOORD) : COLOR
{
	float2 position = float2(coords.x * 1920.0, coords.y * 1080.0);
	float4 color = tex2D(textureSampler, coords);
	float dist = distance(position,float2(playerX, playerY));
	float space = 200.0 + sin(dist * 0.5); //100.0;
	float thickness = 5.0;
	float speed = 0.5;
	float time = (gameTime + 30000.0);
	float screamPattern = fmod(dist - (radius * 0.1) - fmod(time * speed, space) + space, space) * 0.01;
	float3 newColor = lerp(color.rgb + 0.6, color.rgb + 0.1, abs(screamPattern));

	float fallOff = 1. - dist/radius;
	if (dist > radius)
	{
		fallOff = 0.;
	}
	float3 bwColor = lerp(color.rgb, newColor.rgb, abs(fallOff));
	color = float4(bwColor,tex2D(textureSampler, coords).a);
	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 scream();
    }  
}  