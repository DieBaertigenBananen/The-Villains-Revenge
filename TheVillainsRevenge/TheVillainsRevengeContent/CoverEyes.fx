sampler textureSampler;
float playerX;
float playerY;

float4 Shader(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
    if (coords.x > playerX && coords.y > playerY)
	{
		color.rgb = 0;
	}
	return color;
}  
  
technique Technique1  
{  
    pass Pass1  
    {  
        PixelShader = compile ps_2_0 Shader();  
    }  
}  