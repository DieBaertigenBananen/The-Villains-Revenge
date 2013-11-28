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
        int wo = 0;
        public void load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("fonts/schrift");
        }
        public int update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Enter))
            {
                if (wo == 1)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            if (keyState.IsKeyDown(Keys.Down)||keyState.IsKeyDown(Keys.S))
            {
                wo = 1;
            }
            if (keyState.IsKeyDown(Keys.Up) | keyState.IsKeyDown(Keys.W))
            {
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
                spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2) - 100), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2) - 100), Color.Black);
            }
            if (wo == 1)
            {
                spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2)), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.PreferredBackBufferWidth / 2) - 50, (Game1.graphics.PreferredBackBufferHeight / 2)), Color.Black);
            }
            spriteBatch.End();
        }
    }
}
