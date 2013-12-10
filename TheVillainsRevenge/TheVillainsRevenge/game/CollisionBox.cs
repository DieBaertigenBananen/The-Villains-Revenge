using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class CollisionBox
    {
        public Rectangle box;
        public Vector2 position;
        public Vector2 offset;

        public CollisionBox(int offsetX, int offsetY, int width, int height)
        {
            offset = new Vector2(offsetX, offsetY);
            position = offset;
            box = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public void Update(Vector2 position)
        {
            box.X = (int)(position.X + offset.X);
            box.Y = (int)(position.Y + offset.Y);
        }
    }
}
