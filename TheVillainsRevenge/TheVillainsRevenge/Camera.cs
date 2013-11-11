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
        public void Update(Player spieler, Map karte)
        {
            //Kamera an Spieler anpassen
            int walkingspace = 200;
            int topspace = 200;
            int bottomspace = 100;

            position.X = spieler.pos.X - walkingspace; //Scrolling seitlich
            if (position.X < 0) //Linker Maprand
            {
                position.X = 0;
            }
            else if (position.X > karte.size.X - screenresolution.X) //Rechter Maprand
            {
                position.X = karte.size.X - screenresolution.X;
            }
            if (position.Y + topspace > spieler.pos.Y) //Scrolling nach oben
            {
                position.Y = spieler.pos.Y - topspace;
            }
            else if (position.Y + screenresolution.Y - bottomspace < spieler.pos.Y) //Scrolling nach unten
            {
                position.Y = spieler.pos.Y - (int)(screenresolution.Y - bottomspace);
            }
            if (position.Y < 0) //Oberer Maprand
            {
                position.Y = 0;
            }
            else if (position.Y > karte.size.Y - screenresolution.Y) //Unterer Maprand
            {
                position.Y = karte.size.Y - screenresolution.Y;
            }

            cammatrix = Matrix.CreateScale(scaling) * Matrix.CreateTranslation(-position.X, -position.Y, 0);

        }

    }
}
