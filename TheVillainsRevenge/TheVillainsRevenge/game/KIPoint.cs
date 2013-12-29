using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class KIPoint
    {
        
        public Vector2 position; //Position
        public Rectangle cbox = new Rectangle(0,0,48,48); //Collisionsbox
        public int id;
        public KIPoint(Vector2 npos,int nid)
        {
            //Setze Position und Collisionsbox
            id = nid;
            position = npos;
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
        }

    }
}
