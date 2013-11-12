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
        public Rectangle viewport;
        public Camera()
        {
            viewport = new Rectangle(0, 0, (int)Game1.resolution.X, (int)Game1.resolution.Y);
        }

        public void Update(GraphicsDeviceManager graphics, Player spieler, Map karte)
        {
            //Kamera an Spieler anpassen
            int leftspace = 800;
            int rightspace = 800;
            int bottomspace = 700;
            int topspace = (int)Game1.resolution.Y - bottomspace;
            

            if (viewport.X + leftspace > spieler.pos.X) //Scrolling nach links
            {
                viewport.X = (int)spieler.pos.X - leftspace;
            }
            else if (viewport.X + viewport.Width - rightspace < spieler.pos.X) //Scrolling nach rechts
            {
                viewport.X = (int)spieler.pos.X - (viewport.Width - rightspace);
            }
            if (viewport.X < 0) //Linker Maprand
            {
                viewport.X = 0;
            }
            else if (viewport.X > karte.size.X - viewport.Width) //Rechter Maprand
            {
                viewport.X = (int)karte.size.X - viewport.Width;
            }
            if (viewport.Y + topspace > spieler.pos.Y) //Scrolling nach oben
            {
                viewport.Y = (int)spieler.pos.Y - topspace;
            }
            else if (viewport.Y + viewport.Height - bottomspace < spieler.pos.Y) //Scrolling nach unten
            {
                viewport.Y = (int)spieler.pos.Y - (viewport.Height - bottomspace);
            }
            if (viewport.Y < 0) //Oberer Maprand
            {
                viewport.Y = 0;
            }
            else if (viewport.Y > karte.size.Y - viewport.Height) //Unterer Maprand
            {
                viewport.Y = (int)karte.size.Y - viewport.Height;
            }
        }

    }
}
