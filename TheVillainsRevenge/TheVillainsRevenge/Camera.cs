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
        public Rectangle virtualViewport;
        public Rectangle viewport = new Rectangle();
        public Matrix viewportTransform;
        public Matrix screenTransform;
        public bool stretch;
        public Camera()
        {
            virtualViewport = new Rectangle(0, 0, 1920, 1080);
        }

        public void Update(GraphicsDeviceManager graphics, Player spieler, Map karte)
        {
            //Kamera an Spieler anpassen
            int walkingspace = 600;
            int bottomspace = 700;
            int topspace = virtualViewport.Height - bottomspace;


            virtualViewport.X = (int)spieler.pos.X - walkingspace; //Scrolling seitlich
            if (virtualViewport.X < 0) //Linker Maprand
            {
                virtualViewport.X = 0;
            }
            else if (virtualViewport.X > karte.size.X - virtualViewport.Width) //Rechter Maprand
            {
                virtualViewport.X = (int)karte.size.X - virtualViewport.Width;
            }
            if (virtualViewport.Y + topspace > spieler.pos.Y) //Scrolling nach oben
            {
                virtualViewport.Y = (int)spieler.pos.Y - topspace;
            }
            else if (virtualViewport.Y + virtualViewport.Height - bottomspace < spieler.pos.Y) //Scrolling nach unten
            {
                virtualViewport.Y = (int)spieler.pos.Y - (virtualViewport.Height - bottomspace);
            }
            if (virtualViewport.Y < 0) //Oberer Maprand
            {
                virtualViewport.Y = 0;
            }
            else if (virtualViewport.Y > karte.size.Y - virtualViewport.Height) //Unterer Maprand
            {
                virtualViewport.Y = (int)karte.size.Y - virtualViewport.Height;
            }


            //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
            //ScreenViewport anpassen
            if (stretch) //Viewport screenfüllend
            {
                viewport.X = 0;
                viewport.Y = 0;
                viewport.Width = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
                viewport.Height = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            }
            else //Viewport mit Offset auf Screen
            {
                if (viewport.X < viewport.Y) //Balken oben/unten
                {
                    viewport.Width = (int)graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
                    viewport.Height = (int)(graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / virtualViewport.Width * virtualViewport.Height);
                }
                else //Balken links/rechts
                {
                    viewport.Height = (int)graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
                    viewport.Width = (int)(graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / virtualViewport.Height * virtualViewport.Width);
                }
                viewport.X = (graphics.GraphicsDevice.PresentationParameters.BackBufferWidth - (int)viewport.Width) / 2;
                viewport.Y = (graphics.GraphicsDevice.PresentationParameters.BackBufferHeight - (int)viewport.Height) / 2;
            }
            viewportTransform = Matrix.CreateTranslation(-virtualViewport.X, -virtualViewport.Y, 0);
            screenTransform = Matrix.CreateScale(viewport.Width / virtualViewport.Width, viewport.Height / virtualViewport.Height, 1) * Matrix.CreateTranslation(viewport.X, viewport.Y, 0);
        }

    }
}
