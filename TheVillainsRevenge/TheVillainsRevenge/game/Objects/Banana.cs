using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Banana : Obj
    {
        public Banana(Vector2 pos, int t)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
        }
        public override void Update(GameTime gameTime, Map map)
        {
            foreach (MovingBlock block in map.mblocks)
            {
                Rectangle collide = new Rectangle(box.X, box.Y + 1, box.Width, box.Height);
                //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                int movespeed = Convert.ToInt32((double)Game1.luaInstance["blockSpeed"]);
                if (block.move == 2)
                    movespeed = -movespeed;
                if (collide.Intersects(block.cbox))
                {
                    if (GameScreen.slow != 0)
                    {
                        movespeed = movespeed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
                    }
                    position.X += movespeed;
                    box.X += movespeed;
                    break;
                }
            }
        }
    }
}
