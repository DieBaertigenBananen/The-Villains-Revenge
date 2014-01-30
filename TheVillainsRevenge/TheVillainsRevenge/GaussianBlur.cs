using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TheVillainsRevenge
{
    class GaussianBlur
    {

        public int radius = 7;
        float[] kernel;
        Vector2[] offsetsHori;
        Vector2[] offsetsVert;
        RenderTarget2D renderTargetBlur;
        public RenderTarget2D blurredRenderTarget;
        public Effect gauss;

        public GaussianBlur()
        {
        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics, int width, int height, float sigma)
        {
            gauss = Content.Load<Effect>("Gauss");
            CalcKernel(sigma);
            CalcOffsets(width, height);
            renderTargetBlur = new RenderTarget2D(graphics.GraphicsDevice, width, height);
            blurredRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, width, height);
        }

        void CalcOffsets(int width, int height)
        {
            offsetsHori = null;
            offsetsVert = null;

            offsetsHori = new Vector2[radius * 2 + 1];
            offsetsVert = new Vector2[radius * 2 + 1];

            float oneOffsetX = 1f / width;
            float oneOffsetY = 1f / height;
            int index = 0;

            for (int i = -radius; i <= radius; i++)
            {
                index = i + radius;
                offsetsHori[index] = new Vector2(i * oneOffsetX, 0.0f);
                offsetsVert[index] = new Vector2(0.0f, i * oneOffsetX);

            }
        }

        void CalcKernel(float sigma)
        {
            kernel = null;
            kernel = new float[radius * 2 + 1];

            int index = 0;
            float total = 0f;

            for (int i = -radius; i <= radius; i++)
            {
                index = i + radius;
 
                kernel[index] = (float) 1.0f/(sigma*(float)Math.Sqrt((2.0f*Math.PI))) * (float)Math.Exp(-((i)*(i))/(2.0f*(sigma*sigma)));
                total += kernel[index];
            }


            for (int i = 0; i < kernel.Length; i++)
            {
                kernel[i] /= total;
            }
        }

        public void ChangeSigma(float sigma)
        {
            CalcKernel(sigma);
        }

        public void PerformGaussianBlur(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, RenderTarget2D srcRenderTarget, BlendState blending)
        {
            //Do Horizontal Blur
            gauss.Parameters["kernel"].SetValue(kernel);
            gauss.Parameters["offsets"].SetValue(offsetsHori);
            graphics.GraphicsDevice.SetRenderTarget(renderTargetBlur);
            graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, blending, null, null, null, gauss);
            spriteBatch.Draw(srcRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            //Do Vertikal Blur
            gauss.Parameters["offsets"].SetValue(offsetsVert);
            graphics.GraphicsDevice.SetRenderTarget(blurredRenderTarget);
            graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, blending, null, null, null, gauss);
            spriteBatch.Draw(renderTargetBlur, Vector2.Zero, Color.White);
            spriteBatch.End();

            //graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
