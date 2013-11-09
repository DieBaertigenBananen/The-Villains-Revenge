using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TheVillainsRevenge
{
    class Cam
    {
        public Vector2 position;
        public Vector2 screenresolution;
        public Vector3 scaling;
        public Matrix cammatrix;
        public Cam(Vector2 resolution)
        {
            screenresolution = resolution;
            position = new Vector2(0, 0);
        }
        public void changeresolution(GraphicsDeviceManager graphics,int width,int height)
        {
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            scaling.Y = (float)graphics.PreferredBackBufferHeight / screenresolution.Y;
            scaling.X = (float)graphics.PreferredBackBufferWidth / screenresolution.X;
            graphics.ApplyChanges();
        }
        public void update()
        {
            cammatrix = Matrix.CreateScale(scaling) * Matrix.CreateTranslation(position.X, position.Y, 0);

        }

    }
}
