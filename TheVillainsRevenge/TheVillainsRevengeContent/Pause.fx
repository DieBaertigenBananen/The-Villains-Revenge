sampler textureSampler;
float gameTime;

float2 distortion(float2 coords)
{
	float2 uv = coords;
	uv -= float2(0.5, 0.5);
	uv *= 1.2 * (1. / 1.2 + 2. * uv.x * uv.x * uv.y * uv.y);
	uv += float2(.5, .5);
	return uv;
};

float vignette(float2 uv, float amount)
{
	return (1. - amount * (uv.y - .5) * (uv.y - .5)) * (1. - amount * (uv.x - .5) * (uv.x - .5));
};

float4 pause(float2 coords: TEXCOORD) : COLOR
{  
	float2 uv = distortion(coords); //Distortion
	float4 color = tex2D(textureSampler, uv); //Get Pixel
	color.rgb *= vignette(uv, 3.); //Vignette
	color *= (12. + fmod(uv.y * 20. - (gameTime * 0.003), 1.)) / 11.5; //Lines
	color.rgb = (color.r + color.g + color.b) / 3; //Gray
	
	float bar = 10. * sqrt(tan(uv.y - gameTime * 0.0005)); //Line
	bar /= vignette(uv, 2.); //Vignette
	if (bar < 1) //Mask unused info from bar
	{
		color = color * bar;
	}
	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 pause();
    }  
}  