using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Welle : Obj
    {
        public Welle(Vector2 pos, int t, bool richtung)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
            this.richtung = richtung;
            box = new Rectangle((int)pos.X, (int)pos.Y, 144, 48);
        }
        public override void Update(GameTime gameTime, Map map)
        {
            int speedx = 20;
            if (!richtung)
            {
                speedx = -speedx;
            }
            //Formel: y = x*x
            position.X += speedx;
            box.X = (int)position.X;
            box.Y = (int)position.Y;
        }
    }
}
