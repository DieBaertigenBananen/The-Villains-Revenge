sampler textureSampler;
float gameTime;

float4 pause(float2 coords: TEXCOORD) : COLOR
{  
	float4 color = tex2D(textureSampler, coords);
	color.rgb = (color.r + color.g + color.b) / 3;

	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 pause();
    }  
}  