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
        public Collision cbox; //Collisionsbox
        public Block(Vector2 npos)
        {
            //Setze Position und Collisionsbox
            pos = npos;
            cbox = new Collision(pos, 64, 64);
        }
    }
}
