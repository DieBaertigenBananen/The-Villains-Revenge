using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Debris : Obj
    {
        public double falltimer = 0;
        public Debris(Vector2 pos, int t,int bid)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
            if (Game1.level == 3)
            {
                if (bid == 0 || bid == 1)
                {
                    stone = true;
                }
                else
                {
                    stone = false;
                }
            }
            else if (Game1.level == 4)
            {
                stone = true;
            }
        }
        public Debris(Vector2 pos, int t, bool stone)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
            this.stone = stone;
        }
        public override void Update(GameTime gameTime, Map map)
        {
            if (fall)
            {
                if (falltimer == 0)
                    falltimer = Game1.time.TotalMilliseconds;
                float t = (float)((Game1.time.TotalMilliseconds - falltimer) / 1000);
                int gravitation = Convert.ToInt32((double)Game1.luaInstance["objectGravitation"]);
                position.Y += (gravitation * t);
                box.Y = (int)position.Y;
                for (int i = 0; i < map.objects.Count(); i++)
                {
                    Obj obj = map.objects.ElementAt(i);
                    if(obj.box.Intersects(box)&&obj != this&&obj.type == 3)
                    {
                        map.objects.Remove(obj);
                    }
                }
            }
        }
    }
}
