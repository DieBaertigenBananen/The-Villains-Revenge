using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Obj
    {
        //Objekte wie banana/geworfenes womit der Ashbrett kollidiert
        public bool richtung;
        public bool fall = true;
        public Vector2 position; //Position
        public Rectangle box; //Kollisionsbox
        public int type;
        public Obj(Vector2 pos, int t) //Konstruktor, setzt Anfangsposition
        {
            position = pos;
            type = t;
            box = new Rectangle((int)pos.X,(int)pos.Y, 48, 48);
        }
        public virtual void Update(GameTime gameTime, Map map)
        {
        }
    }
}
