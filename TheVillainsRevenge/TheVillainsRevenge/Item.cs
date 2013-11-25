using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Item
    {
        public Vector2 pos; //Position
        public Rectangle cbox; //Collisionsbox
        public string type;
        public Rectangle cuttexture = new Rectangle(0, 0, 48, 48);
        public Item(int x, int y, string t) //Konstruktor, setzt Anfangsposition
        {
            pos.X = x;
            pos.Y = y;
            type = t;
            cbox = new Rectangle((int)pos.X, (int)pos.Y, 48, 48);
            switch (type)
            {
                case "herz":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
            }
        }
    }
}
