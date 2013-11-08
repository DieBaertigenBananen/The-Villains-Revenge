using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Block
    {
        public Vector2 pos; //Position
        public Rectangle cbox = new Rectangle(0,0,48,48); //Collisionsbox
        public Rectangle cuttexture = new Rectangle(0,0,48,48);
        public Block(Vector2 npos, string type)
        {
            //Setze Position und Collisionsbox
            pos = npos;
            cbox.X = (int) pos.X;
            cbox.Y = (int) pos.Y;
            //Je nach Blocktyp Ausschnitt aus Textur und größe der Kollisionsbox anpassen
            switch (type)
            {
                case "underground_earth":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
                case "ground_grass":
                    cuttexture.X = 48;
                    cuttexture.Y = 0;
                    break;
                case "platform_grass":
                    cuttexture.X = 2 * 48;
                    cuttexture.Y = 0;
                    break;
                case "ground_grass_30":
                    cuttexture.X = 3 * 48;
                    cuttexture.Y = 0;
                    break;
                case "ground_grass_15":
                    cuttexture.X = 4 * 48;
                    cuttexture.Y = 0;
                    break;
                case "water":
                    cuttexture.X = 5 * 48;
                    cuttexture.Y = 0;
                    break;
                case "underground_rock":
                    cuttexture.X = 0;
                    cuttexture.Y = 48;
                    break;
                case "ground_rock":
                    cuttexture.X = 48;
                    cuttexture.Y = 48;
                    break;
                case "platform_rock":
                    cuttexture.X = 2 * 48;
                    cuttexture.Y = 48;
                    break;
                case "ground_rock_30":
                    cuttexture.X = 3 * 48;
                    cuttexture.Y = 48;
                    break;
                case "ground_rock_15":
                    cuttexture.X = 4 * 48;
                    cuttexture.Y = 48;
                    break;
                case "lava":
                    cuttexture.X = 5 * 48;
                    cuttexture.Y = 48;
                    break;
            }

        }
    }
}
