﻿using System;
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
        public Rectangle viewport;
        public Rectangle screenViewport = new Rectangle();
        public Matrix viewportTransform;
        public Matrix screenTransform;
        public bool stretchScreen;
        public Camera()
        {
            viewport = new Rectangle(0, 0, (int)Game1.resolution.X, (int)Game1.resolution.Y);
            stretchScreen = false;
        }

        public void Update(GraphicsDeviceManager graphics, Player spieler, Map karte)
        {
            //Kamera an Spieler anpassen
            int leftspace = 800;
            int rightspace = (int)Game1.resolution.X-leftspace;
            int bottomspace = 700;
            int topspace = (int)Game1.resolution.Y - bottomspace;
            

            if (viewport.X + leftspace > spieler.position.X) //Scrolling nach links
            {
                viewport.X = (int)spieler.position.X - leftspace;
            }
            else if (viewport.X + viewport.Width - rightspace < spieler.position.X) //Scrolling nach rechts
            {
                viewport.X = (int)spieler.position.X - (viewport.Width - rightspace);
            }
            if (viewport.X < 0) //Linker Maprand
            {
                viewport.X = 0;
            }
            else if (viewport.X > karte.size.X - viewport.Width) //Rechter Maprand
            {
                viewport.X = (int)karte.size.X - viewport.Width;
            }
            if (viewport.Y + topspace > spieler.position.Y) //Scrolling nach oben
            {
                viewport.Y = (int)spieler.position.Y - topspace;
            }
            else if (viewport.Y + viewport.Height - bottomspace < spieler.position.Y) //Scrolling nach unten
            {
                viewport.Y = (int)spieler.position.Y - (viewport.Height - bottomspace);
            }
            if (viewport.Y < 0) //Oberer Maprand
            {
                viewport.Y = 0;
            }
            else if (viewport.Y > karte.size.Y - viewport.Height) //Unterer Maprand
            {
                viewport.Y = (int)karte.size.Y - viewport.Height;
            }

            int width = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            if (stretchScreen) //Viewport screenfüllend
            {
                screenViewport.X = 0;
                screenViewport.Y = 0;
                screenViewport.Width = width;
                screenViewport.Height = height;
            }
            else //Viewport mit Offset auf Screen
            {
                if (screenViewport.X < screenViewport.Y) //Balken oben/unten
                {
                    screenViewport.Width = (int)width;
                    screenViewport.Height = (int)(width / Game1.resolution.X * Game1.resolution.Y);
                }
                else //Balken links/rechts
                {
                    screenViewport.Height = (int)height;
                    screenViewport.Width = (int)(height / Game1.resolution.Y * Game1.resolution.X);
                }
                screenViewport.X = (width - (int)screenViewport.Width) / 2;
                screenViewport.Y = (height - (int)screenViewport.Height) / 2;
                //= viewport.Width / resolution.X;
                //= viewport.Height / resolution.Y;
            }
            Matrix screenScale = Matrix.CreateScale((float)screenViewport.Width / Game1.resolution.X, (float)screenViewport.Height / Game1.resolution.Y, 1);
            screenTransform = screenScale * Matrix.CreateTranslation(screenViewport.X, screenViewport.Y, 0);

            viewportTransform = Matrix.CreateTranslation(-viewport.X, -viewport.Y, 0);
        }

    }
}