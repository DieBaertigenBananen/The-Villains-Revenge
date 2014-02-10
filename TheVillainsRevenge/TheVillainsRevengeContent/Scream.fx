#define POLES 21
#define REFLECTIONS 10.0
sampler textureSampler;
int radius;
float gameTime;

float ripple(float dist, float shift)
{
	return cos(64.0 * dist + shift) / (1.0 + 1.0 * dist);
}

float4 scream(float2 coords: TEXCOORD) : COLOR
{
	float4 color = float4(0,0,0,0);
	//float2 iMouse = float2(100, 100);
	//float2 iResolution = float2(1920, 1080);  
	//float larger = max(iResolution.x, iResolution.y);
	//float2 uv = (coords.xy - .5*iResolution.xy) / larger;
	//float2 uvflip = float2(uv.x, -uv.y);
	//float2 cursor = (iMouse.xy - .5*iResolution.xy) / larger;
	//float2 blessr = float2(-cursor.x, cursor.y);
	
	////float on = float(abs(uv.x)<.25 && abs(uv.y)<.25);
	
	//float lum = .5 + .1 * ripple(length(uv), 0.0) + 0.0;

	//	//.1 * ripple(length(cursor - uv), -iGlobalTime) +
	//	//.1 * ripple(length(blessr - uv), -iGlobalTime) +
	//	//.1 * ripple(length(cursor - uvflip), -iGlobalTime) +
	//	//.1 * ripple(length(blessr - uvflip), -iGlobalTime) +*/
	//	//.1 * cos(64.0*uv.y - iGlobalTime) +
	//	//.1 * cos(64.0*(uv.x*uv.x) - iGlobalTime) +

	
	//float twopi = 2.0*3.141592654;
	//const int count = POLES;
	//float fcount = float(count);
	//float2 rot = float2(cos(twopi*.618), sin(twopi*.618));
	//float2 tor = float2(-sin(twopi*.618), cos(twopi*.618));
	//for (int i = 0; i < count; ++i)
	//{
	//	lum += .2 * ripple(length(cursor - uv), -gameTime);
	//	cursor = cursor.x*rot + cursor.y*tor;
	//}
	
	////float lumtemp1 = 3.*lum*lum;
	////float lumtemp2 = 2.*lum*lum*lum;
	////lum = lumtemp1 - lumtemp2;
	//color = float4(lum, lum, lum, 1.0);

	return color;
}
  
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 scream();
    }  
}  