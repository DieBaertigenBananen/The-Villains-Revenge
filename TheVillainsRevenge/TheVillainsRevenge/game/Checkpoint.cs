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
        public int x;
        public bool end;

        public Checkpoint(int x,bool end)
        {
            this.end = end;
            this.x = x;
        }
    }
}
