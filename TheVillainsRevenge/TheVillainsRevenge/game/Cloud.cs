using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Cloud
    {
        public int type;
        public Rectangle cuttexture;
        public Vector2 position;
        public float size;
        public Cloud(int t, Vector2 pos, float s)
        {
            type = t;
            position = pos;
            size = s;
            switch (type)
            {
                case 1:
                    cuttexture = new Rectangle(0, 0, 500, 500);
                    break;
                case 2:
                    cuttexture = new Rectangle(0, 500, 600, 600);
                    break;
            }
        }

        public void Update(int wind)
        {
            if (GameScreen.slow != 0)
            {
                wind = wind / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
            }
            position.X -= wind;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D cloudTexture, Vector2 parallaxOffset)
        {
            spriteBatch.Draw(cloudTexture, position + parallaxOffset, cuttexture, Color.White, 0.0f, Vector2.Zero, size, SpriteEffects.None, 1.0f);
        }
    }
}
