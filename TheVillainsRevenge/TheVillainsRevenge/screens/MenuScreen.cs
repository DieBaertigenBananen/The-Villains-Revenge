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
        bool loadScreen;
        bool optionScreen;
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
            //Wenn ladet
            if (loadScreen)
            {
                return 2;
            }
            //Wenn nicht laden
            else
            {
                if (deadScreen)
                {
                    if (Game1.input.enter || Game1.input.sprung)
                    {
                        deadScreen = false;
                    }
                }
                else if (optionScreen)
                {
                    //Enter wählt Menüfelder
                    if (Game1.input.enter)
                    {
                        //Option == 2 ist Exit
                        if (option == 2)
                        {
                            optionScreen = false;
                            option = 1;
                        }
                        //Option = 1 ist stretch
                        else if (option == 1)
                        {
                            if(Game1.stretch)
                                Game1.stretch = false;
                            else
                                Game1.stretch = true;
                        }
                        else //Fullscreentoogle
                        {
                            return 3;
                        }
                    }
                    if (Game1.input.down)
                    {
                        //Wechsel
                        if (option == 0)
                            option = 1;
                        else if (option == 1)
                            option = 2;
                        else
                            option = 0;
                    }
                    if (Game1.input.up)
                    {
                        //Wechsel
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
                        //Setze auf Exit
                        if (option == 2)
                        {
                            optionScreen = false;
                            option = 1;
                        }
                        else
                        {
                            option = 2;
                        }
                    }
                }
                else
                {
                    if (Game1.input.enter || Game1.input.sprung)
                    {
                        //Option == 2 ist Exit
                        if (option == 2)
                        {
                            return 0;
                        }
                            //Option = 1 ist Fullscreen
                        else if (option == 1)
                        {
                            optionScreen = true;
                            option = 0;
                        }
                        else
                        {
                            //Game Start
                            loadScreen = true;
                        }
                    }
                    if (Game1.input.down)
                    {
                        //Wechsel
                        if (option == 0)
                            option = 1;
                        else if (option == 1)
                            option = 2;
                        else
                            option = 0;
                    }
                    if (Game1.input.up)
                    {
                        //Wechsel
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
                        //Setze auf Exit
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
        }
        public void draw(SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (!deadScreen&&!loadScreen&&!optionScreen)
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
                    spriteBatch.DrawString(font, "Options", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Options", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)), Color.Gray);
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
            else if(deadScreen)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                spriteBatch.DrawString(font, "Press Enter", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 60, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);

            }
            else if (loadScreen)
            {
                spriteBatch.DrawString(font, "Game loading ...", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);

            }
            else
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
                    if(Game1.stretch)
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
                    spriteBatch.DrawString(font, "Return", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Return", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Gray);
                }
            }
            spriteBatch.End();
        }
    }
}
