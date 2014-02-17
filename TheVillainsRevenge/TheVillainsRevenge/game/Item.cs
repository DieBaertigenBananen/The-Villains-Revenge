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
        public Rectangle cuttexture = new Rectangle(0, 0, 96, 96);
        public Item(Vector2 pos, string t) //Konstruktor, setzt Anfangsposition
        {
            position = pos;
            type = t;
            cbox = new Rectangle((int)position.X, (int)position.Y, 96, 96);
            switch (type)
            {
                case "banana":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
                case "bag":
                    cuttexture.X = 96;
                    cuttexture.Y = 0;
                    break;
                case "herz":
                    cuttexture.X = 192;
                    cuttexture.Y = 0;
                    break;
                case "monkey":
                    cuttexture.X = 288;
                    cuttexture.Y = 0;
                    break;
                case "zeit":
                    cuttexture.X = 384;
                    cuttexture.Y = 0;
                    break;
            }
        }
    }
}
