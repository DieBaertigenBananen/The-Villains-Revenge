using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class MenuScreen
    {
        SpriteFont font;
        Input input = new Input();
        int wo = 0;
        public void load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("fonts/schrift");
        }
        public int update()
        {
            input.update();
            if (input.enter)
            {
                if (wo == 2)
                {
                    return 0;
                }
                else if (wo == 1)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
            }
            if (input.down)
            {
                if (wo == 0)
                    wo = 1;
                else if (wo == 1)
                    wo = 2;
                else
                    wo = 0;
            }
            if (input.up)
            {
                if (wo == 0)
                    wo = 2;
                else if (wo == 2)
                    wo = 1;
                else
                    wo = 0;
            }
            return 1;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (wo == 0)
            {
                spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2) - 50), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2) - 50), Color.Black);
            }
            if (wo == 1)
            {
                spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2)), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2)), Color.Black);
            }
            if (wo == 2)
            {
                spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2) + 50), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2) + 50), Color.Black);
            }
            spriteBatch.End();
        }
    }
}
