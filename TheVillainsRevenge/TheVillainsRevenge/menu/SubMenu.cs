using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class SubMenu
    {
        public bool visible;
        int optionCount;
        public int option;
        string name;
        SpriteFont font;
        public SubMenu(int options, string menuName, SpriteFont menuFont)
        {
            optionCount = options;
            name = menuName;
            font = menuFont;
        }

        public void Update()
        {
            if (Game1.input.down)
            {
                option++;
            }
            if (Game1.input.up)
            {
                option--;
            }
            if (option > optionCount - 1)
            {
                option = 0;
            }
            else if (option < 0)
            {
                option = optionCount - 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (name == "main")
            {
                if (option == 0)
                {
                    spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 250, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Start Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 250, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Gray);
                }
                if (option == 1)
                {
                    spriteBatch.DrawString(font, "Options", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 250, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Options", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 250, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Gray);
                }
                if (option == 2)
                {
                    spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 250, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Exit Game", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 250, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Gray);
                }
            }
            else if (name == "option")
            {
                if (option == 0)
                {
                    if (Game1.graphics.IsFullScreen)
                        spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                    else
                        spriteBatch.DrawString(font, "Windowmode", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                }
                else
                {
                    if (Game1.graphics.IsFullScreen)
                        spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Gray);
                    else
                        spriteBatch.DrawString(font, "Windowmode", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Gray);
                }
                if (option == 1)
                {
                    if (Game1.stretch)
                        spriteBatch.DrawString(font, "Stretch on", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Black);
                    else
                        spriteBatch.DrawString(font, "Stretch off", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Black);
                }
                else
                {
                    if (Game1.stretch)
                        spriteBatch.DrawString(font, "Stretch on", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Gray);
                    else
                        spriteBatch.DrawString(font, "Stretch off", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Gray);
                }
                if (option == 2)
                {
                    if (Game1.sound)
                        spriteBatch.DrawString(font, "Sound on", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);
                    else
                        spriteBatch.DrawString(font, "Sound off", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);
                }
                else
                {
                    if (Game1.sound)
                        spriteBatch.DrawString(font, "Sound on", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Gray);
                    else
                        spriteBatch.DrawString(font, "Sound off", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Gray);
                }
                if (option == 3)
                {
                    spriteBatch.DrawString(font, "Return", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 100), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Return", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 100), Color.Gray);
                }
            }


        }
    }
}
