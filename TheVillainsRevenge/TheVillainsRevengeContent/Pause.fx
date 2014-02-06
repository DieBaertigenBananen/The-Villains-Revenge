sampler textureSampler;
float gameTime;

float4 pause(float2 coords: TEXCOORD) : COLOR
{  
	//Distortion
	float2 uv = coords;
	uv -= float2(0.5, 0.5);
	uv *= 1.2 * (1. / 1.2 + 2. * uv.x * uv.x * uv.y * uv.y);
	uv += float2(.5, .5);
	//Get Pixel
	float4 color = tex2D(textureSampler, uv);
	//Vignette
	float amount = 3.;
	float vignette = (1. - amount * (uv.y - .5) * (uv.y - .5)) * (1. - amount * (uv.x - .5) * (uv.x - .5));
	color.rgb *= vignette;
	//Lines
	color *= (12. + fmod(uv.y * 20. - (gameTime * 0.003), 1.)) / 11.5;
	//Gray
	color.rgb = (color.r + color.g + color.b) / 3;
	
	float amount = 2.;
	float vignette = (1. - amount * (uv.y - .5) * (uv.y - .5)) * (1. - amount * (uv.x - .5) * (uv.x - .5));
	float bar = 10. * sqrt(tan(uv.y - gameTime * 0.0005));
	bar /= vignette;
	if (bar < 1)
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