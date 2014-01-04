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
        bool deadScreen;
        bool loadScreen;
        SubMenu mainMenu;
        SubMenu optionMenu;
        public MenuScreen(bool playerDied)
        {
            deadScreen = playerDied;
        }
        public void Load(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("fonts/schrift");
            mainMenu = new SubMenu(3, "main", font);
            optionMenu = new SubMenu(4, "option", font);
            mainMenu.visible = true;
        }
        public int Update()
        {
            if (optionMenu.visible)
            {
                optionMenu.Update();
            }
            else if (mainMenu.visible)
            {
                mainMenu.Update();
            }
            if (deadScreen)
            {
                if (Game1.input.enter || Game1.input.sprung)
                {
                    deadScreen = false;
                }
            }
            else if (loadScreen)
            {
                return 2;
            }
            else if (optionMenu.visible)
            {
                //Enter wählt Menüfelder
                if (Game1.input.enter)
                {
                    //Option == 2 ist Exit
                    if (optionMenu.option == 3)
                    {
                        optionMenu.visible = false;
                        mainMenu.option = 1;
                    }
                    //Option = 1 ist stretch
                    else if (optionMenu.option == 2)
                    {
                        if (Game1.sound)
                            Game1.sound = false;
                        else
                            Game1.sound = true;
                    }
                    else if (optionMenu.option == 1)
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
                if (Game1.input.back)
                {
                    //Setze auf Exit
                    if (optionMenu.option == 3)
                    {
                        optionMenu.visible = false;
                        mainMenu.option = 1;
                    }
                    else
                    {
                        optionMenu.option = 3;
                    }
                }
            }
            else if (mainMenu.visible)
            {
                if (Game1.input.enter || Game1.input.sprung)
                {
                    //Option == 2 ist Exit
                    if (mainMenu.option == 2)
                    {
                        return 0;
                    }
                        //Option = 1 ist Fullscreen
                    else if (mainMenu.option == 1)
                    {
                        optionMenu.visible = true;
                        optionMenu.option = 0;
                    }
                    else
                    {
                        //Game Start
                        loadScreen = true;
                    }
                }
                if (Game1.input.back)
                {
                    //Setze auf Exit
                    if (mainMenu.option == 2)
                    {
                        return 0;
                    }
                    else
                    {
                        mainMenu.option = 2;
                    }
                }
            }
            return 1;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (mainMenu.visible)
            {
                mainMenu.Draw(spriteBatch);
            }
            if (optionMenu.visible)
            {
                optionMenu.Draw(spriteBatch);
            }
            if (deadScreen)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);
                spriteBatch.DrawString(font, "Press Enter", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 60, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 50), Color.Black);

            }
            else if (loadScreen)
            {
                spriteBatch.DrawString(font, "Game loading ...", new Vector2((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 50, (Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 50), Color.Black);

            }
            spriteBatch.End();
        }
    }
}
