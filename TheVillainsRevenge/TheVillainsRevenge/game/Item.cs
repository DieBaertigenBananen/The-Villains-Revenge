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
        public Rectangle cuttexture = new Rectangle(0, 0, 64, 64);
        public Item(Vector2 pos, string t) //Konstruktor, setzt Anfangsposition
        {
            position = pos;
            type = t;
            cbox = new Rectangle((int)position.X, (int)position.Y, 64, 64);
            switch (type)
            {
                case "zeit":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
                case "sack":
                    cuttexture.X = 64;
                    cuttexture.Y = 0;
                    break;
                case "herz":
                    cuttexture.X = 128;
                    cuttexture.Y = 0;
                    break;
                case "banana":
                    cuttexture.X = 192;
                    cuttexture.Y = 0;
                    break;
                case "monkey":
                    cuttexture.X = 256;
                    cuttexture.Y = 0;
                    break;
            }
        }
    }
}
