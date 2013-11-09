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
                case "block":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
            }

        }
    }
}
