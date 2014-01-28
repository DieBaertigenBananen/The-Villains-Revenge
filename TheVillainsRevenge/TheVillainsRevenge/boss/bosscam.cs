using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class BossCam : Camera
    {
        public override void Update(GraphicsDeviceManager graphics, Player spieler, Map karte)
        {
            viewport.Y = 0;
            viewport.X = 0;
            UpdateTransformation(graphics); //Abgekapselt damit Camera für Menü ohne Spieler verwendbar ist.
        }
    }
}
