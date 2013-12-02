using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TheVillainsRevenge
{
    class Checkpoint
    {
        public Rectangle cbox;

        public Checkpoint(int x)
        {
            cbox = new Rectangle(x, 0, 128, 2160);
        }
    }
}
