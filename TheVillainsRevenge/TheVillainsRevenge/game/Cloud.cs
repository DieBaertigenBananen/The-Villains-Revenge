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
        Rectangle cuttexture;
        public Vector2 position;
        float size;
        
        public Cloud(int t, Vector2 pos, float s)
        {
            type = t;
            position = pos;
            size = s;
            switch (type)
            {
                case 1:
                    cuttexture = new Rectangle(0, 0, 200, 100);
                    break;
                case 2:
                    cuttexture = new Rectangle(0, 100, 200, 100);
                    break;
            }
        }

        public void Update(int wind)
        {
            position.X -= wind;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D cloudTexture)
        {
            spriteBatch.Draw(cloudTexture, position, cuttexture, Color.White);
        }
    }
}
