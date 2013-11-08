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
        public Rectangle cbox = new Rectangle(); //Collisionsbox
        Rectangle cuttexture = new Rectangle();
        public Block(Vector2 npos, string type)
        {
            //Setze Position und Collisionsbox
            pos = npos;
            cbox.X = (int) pos.X;
            cbox.Y = (int) pos.Y;
            //Je nach Blocktyp Ausschnitt aus Textur und größe der Kollisionsbox anpassen
            switch (type)
            {
                case "ground":
                    cbox.Height = 48;
                    cbox.Width = 48;
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
                case "solid":

                    break;
                case "water":

                    break;
                case "ladder":

                    break;
            }

        }
    }
}
