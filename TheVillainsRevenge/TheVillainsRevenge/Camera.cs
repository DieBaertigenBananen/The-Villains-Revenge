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
        public Vector2 resolution;
        public Vector3 scaling;
        public Matrix cammatrix;
        public bool full;
        private int w, h;
        public Viewport viewport;
        public Camera(Vector2 res)
        {
            resolution = res;
            position = new Vector2(0, 0);
            viewport = new Viewport(0, 0, 0, 0);
        }
        public void changeresolution(GraphicsDeviceManager graphics,int width,int height)
        {
            if (w != width || h != height)
            {
                //Breite, Höhe des Fensters
                w = width;
                h = height;
                //Viewport anpassen
                int viewportwidth = (int)height / 9 * 16;
                int viewportheight = (int)width / 16 * 9;
                int viewportdeltax = ((int)viewportwidth -  width) / 2;
                int viewportdeltay = ((int)viewportheight - height) / 2;
                if (viewportdeltax < viewportdeltay)
                {
                    //Balken oben/unten
                    viewportwidth = (int)width;
                    viewportdeltax = 0;
                }
                else
                {
                    //Balken links/rechts
                    viewportheight = (int)height;
                    viewportdeltay = 0;
                }
                viewport.X = viewportdeltax;
                viewport.Y = viewportdeltay;
                viewport.Width = viewportwidth;
                viewport.Height = viewportheight;

                //Render/Scaling anpassen
                graphics.PreferredBackBufferHeight = height;
                graphics.PreferredBackBufferWidth = width;
                scaling.X = (float)viewportwidth / resolution.X;
                scaling.Y = (float)viewportheight / resolution.Y;
                graphics.ApplyChanges();
            }
        }
        public void togglefullscreen(GraphicsDeviceManager graphics, bool fullscreen)
        {
            if (full != fullscreen)
            {
                full = fullscreen;
                graphics.IsFullScreen = full;
            }
            graphics.ApplyChanges();
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
            else if (position.X > karte.size.X - resolution.X) //Rechter Maprand
            {
                position.X = karte.size.X - resolution.X;
            }
            if (position.Y + topspace > spieler.pos.Y) //Scrolling nach oben
            {
                position.Y = spieler.pos.Y - topspace;
            }
            else if (position.Y + resolution.Y - bottomspace < spieler.pos.Y) //Scrolling nach unten
            {
                position.Y = spieler.pos.Y - (int)(resolution.Y - bottomspace);
            }
            if (position.Y < 0) //Oberer Maprand
            {
                position.Y = 0;
            }
            else if (position.Y > karte.size.Y - resolution.Y) //Unterer Maprand
            {
                position.Y = karte.size.Y - resolution.Y;
            }

            cammatrix = Matrix.CreateScale(scaling) * Matrix.CreateTranslation(-position.X, -position.Y, 1);

        }

    }
}
