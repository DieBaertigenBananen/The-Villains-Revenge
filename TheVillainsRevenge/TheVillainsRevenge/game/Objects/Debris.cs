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
        public Debris(Vector2 pos, int t)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
        }
        public override void Update(GameTime gameTime, Map map)
        {
            if (fall)
            {
                if (falltimer == 0)
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - falltimer) / 1000);
                int gravitation = Convert.ToInt32((double)Game1.luaInstance["objectGravitation"]);
                position.Y += (gravitation * t);
                box.Y = (int)position.Y;
                foreach (Debris debri in map.objects)
                {
                    if(debri.box.Intersects(box)&&debri != this)
                    {
                        map.objects.Remove(debri);
                        break;
                    }
                }
            }
        }
    }
}
