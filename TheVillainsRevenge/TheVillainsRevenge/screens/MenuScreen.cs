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
        int option = 0;
        bool deadScreen;
        public MenuScreen(bool playerDied)
        {
            deadScreen = playerDied;
        }
        public void load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("fonts/schrift");
        }
        public int update()
        {
            if (deadScreen)
            {
                if (Game1.input.enter)
                {
                    deadScreen = false;
                }
            }
            else
            {
                if (Game1.input.enter)
                {
                    if (option == 2)
                    {
                        return 0;
                    }
                    else if (option == 1)
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
                if (Game1.input.down)
                {
                    if (option == 0)
                        option = 1;
                    else if (option == 1)
                        option = 2;
                    else
                        option = 0;
                }
                if (Game1.input.up)
                {
                    if (option == 0)
                    {
                        option = 2;
                    }
                    else if (option == 2)
                    {
                        option = 1;
                    }
                    else
                    {
                        option = 0;
                    }
                }
                if (Game1.input.back)
                {
                    if (option == 2)
                    {
                        return 0;
                    }
                    else
                    {
                        option = 2;
                    }
                }
            }
            return 1;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (!deadScreen)
            {
                if (option == 0)
                {
                    spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Gray);
                }
                if (option == 1)
                {
                    spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Gray);
                }
                if (option == 2)
                {
                    spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Gray);
                }
            }
            else
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                spriteBatch.DrawString(font, "Press Enter", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 60, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);
                  
            }
            spriteBatch.End();
        }
    }
}
