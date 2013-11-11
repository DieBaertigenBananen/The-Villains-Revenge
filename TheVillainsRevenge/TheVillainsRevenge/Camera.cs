using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Camera
    {
        public Vector2 position;
        public Vector2 screenresolution;
        public Vector3 scaling;
        public Matrix cammatrix;
        public bool full;
        private int w, h;
        public Camera(Vector2 resolution)
        {
            screenresolution = resolution;
            position = new Vector2(0, 0);
        }
        public void changeresolution(GraphicsDeviceManager graphics,int width,int height,bool fullscreen)
        {
            if (w != width || h != height)
            {
                w = width;
                h = height;
                graphics.PreferredBackBufferHeight = height;
                graphics.PreferredBackBufferWidth = width;
                //scaling.Y = (float)(graphics.PreferredBackBufferWidth / 16 * 9) / screenresolution.Y;
                scaling.X = (float)graphics.PreferredBackBufferWidth / screenresolution.X;
                scaling.Y = (float)graphics.PreferredBackBufferHeight / screenresolution.Y;
                if (full != fullscreen)
                {
                    full = fullscreen;
                    graphics.IsFullScreen = full;
                }
                graphics.ApplyChanges();
            }
        }
        public void Update()
        {
            cammatrix = Matrix.CreateScale(scaling) * Matrix.CreateTranslation(-position.X, -position.Y, 0);

        }

    }
}
