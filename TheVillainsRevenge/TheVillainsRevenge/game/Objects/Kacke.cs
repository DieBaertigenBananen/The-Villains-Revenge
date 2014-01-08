using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Kacke : Obj
    {
        public bool richtung;
        public bool fall = false;
        public int y = Convert.ToInt32((double)Game1.luaInstance["kackeStartY"]);
        public Kacke(Vector2 pos, int t,bool richtung)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
            this.richtung = richtung;
        }
        public override void Update(GameTime gameTime, Map map)
        {
            y++;
            int speedx = Convert.ToInt32((double)Game1.luaInstance["kackeSpeed"]);
            if (!richtung)
            {
                speedx = -speedx;
            }
            int speedy = y;
            //Formel: y = x*x
            position.X += speedx;
            position.Y += speedy;
            box.X = (int)position.X;
            box.Y = (int)position.Y;
        }
    }
}
