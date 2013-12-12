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
        public Vector2 position; //Position
        public Rectangle cbox; //Collisionsbox
        public string type;
        public Rectangle cuttexture = new Rectangle(0, 0, 48, 48);
        public Item(Vector2 pos, string t) //Konstruktor, setzt Anfangsposition
        {
            position = pos;
            type = t;
            cbox = new Rectangle((int)position.X, (int)position.Y, 48, 48);
            switch (type)
            {
                case "herz":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
                case "zeit":
                    cuttexture.X = 48;
                    cuttexture.Y = 0;
                    break;
            }
        }
    }
}
