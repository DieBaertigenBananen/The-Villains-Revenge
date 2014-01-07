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
        public List<Button> buttons = new List<Button>();
        int optionCount;
        public int option;
        string name;
        SpriteFont font;
        float fontScale;
        public bool exit;
        int optionSpace;
        Vector2 offset;
        Effect buttonShader;

        public SubMenu(int options, string menuName, SpriteFont menuFont, Vector2 menuOffset, int optSpace, float textScale)
        {
            optionCount = options;
            name = menuName;
            font = menuFont;
            optionSpace = optSpace;
            offset = menuOffset;
            fontScale = textScale;
        }

        public void Load(ContentManager Content)
        {
            buttonShader = Content.Load<Effect>("Button");
        }

        public void Update(GameTime gameTime)
        {
            exit = false;
            if (Game1.input.down) //Nach unten
            {
                option++;
                MenuScreen.UpdateBlinkingTimer(gameTime, true);
            }
            if (Game1.input.up) //Nach oben
            {
                option--;
                MenuScreen.UpdateBlinkingTimer(gameTime, true);
            }
            if (option > optionCount - 1) //Von unten nach oben springen
            {
                option = 0;
            }
            else if (option < 0) //Von oben nach unten springen
            {
                option = optionCount - 1;
            }
            if (Game1.input.back) //Esc (Untere/Letzte Option muss immer Exit/Return sein)
            {
                //Setze auf Exit
                if (option == optionCount - 1)
                {
                    exit = true;
                }
                else
                {
                    option = optionCount - 1;
                }
            }
            //ButtonStates an aktuelle Option anpassen
            for (int i = 0; i < buttons.Count(); ++i)
            {
                Button button = buttons.ElementAt(i);
                if (!button.blinkable) //Standardbutton
                {
                    button.ChangeState(false);
                    if (i == option)
                    {
                        button.ChangeState(true);
                    }
                }
                else //Blinkable Button
                {
                    switch (button.name)
                    {
                        case "fullscreen":
                            if (Game1.graphics.IsFullScreen)
                            {
                                button.ChangeState(true);
                            }
                            else
                            {
                                button.ChangeState(false);
                            }
                            break;
                        case "stretch":
                            if (Game1.stretch)
                            {
                                button.ChangeState(true);
                            }
                            else
                            {
                                button.ChangeState(false);
                            }
                            break;
                        case "sound":
                            if (Game1.sound)
                            {
                                button.ChangeState(true);
                            }
                            else
                            {
                                button.ChangeState(false);
                            }
                            break;
                    }
                    if (i == option)
                    {
                        if (button.previousState == button.activated)
                        {
                            button.blinking = true;
                            
                        }
                        else
                        {
                            button.blinking = false;
                        }
                    }
                    else
                    {
                        button.blinking = false;
                        button.previousState = button.activated;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Camera camera)
        {
            buttonShader.Parameters["gameTime"].SetValue(gameTime.TotalGameTime.Milliseconds);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, buttonShader, camera.viewportTransform);
            for (int i = 0; i < buttons.Count(); ++i)
            {
                Button button = buttons.ElementAt(i);
                button.Draw(spriteBatch, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (i * optionSpace) + offset.Y), MenuScreen.menuButtons, buttonShader); 
            }
            spriteBatch.End();


            //if (name == "main")
            //{
            //    if (option == 0)
            //    {
            //        spriteBatch.Draw(MenuScreen.menuButtons, , new Rectangle(300,0,300,100), Color.White);
            //    }
            //    else
            //    {
            //        spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (0 * optionSpace) + offset.Y), new Rectangle(0, 0, 300, 100), Color.White);
            //    }
            //    if (option == 1)
            //    {
            //        spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (1 * optionSpace) + offset.Y), new Rectangle(300, 100, 300, 100), Color.White);
            //    }
            //    else
            //    {
            //        spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (1 * optionSpace) + offset.Y), new Rectangle(0, 100, 300, 100), Color.White);
            //    }
            //    if (option == 2)
            //    {
            //        spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (2 * optionSpace) + offset.Y), new Rectangle(300, 200, 300, 100), Color.White);
            //    }
            //    else
            //    {
            //        spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (2 * optionSpace) + offset.Y), new Rectangle(0, 200, 300, 100), Color.White);
            //    }
            //}
            //else if (name == "option")
            //{
            //    if (option == 0)
            //    {
            //        if (Game1.graphics.IsFullScreen)
            //            spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (0 * optionSpace) + offset.Y), new Rectangle(300, 300, 300, 100), Color.White);
            //        else
            //            spriteBatch.Draw(MenuScreen.menuButtons, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (0 * optionSpace) + offset.Y), new Rectangle(0, 300, 300, 100), Color.White);
            //    }
            //    else
            //    {
            //        if (Game1.graphics.IsFullScreen)
            //            spriteBatch.DrawString(font, "Fullscreen", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (0 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //        else
            //            spriteBatch.DrawString(font, "Windowmode", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (0 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //    if (option == 1)
            //    {
            //        if (Game1.stretch)
            //            spriteBatch.DrawString(font, "Stretch on", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (1 * optionSpace) + offset.Y), MenuScreen.textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //        else
            //            spriteBatch.DrawString(font, "Stretch off", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (1 * optionSpace) + offset.Y), MenuScreen.textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //    else
            //    {
            //        if (Game1.stretch)
            //            spriteBatch.DrawString(font, "Stretch on", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (1 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //        else
            //            spriteBatch.DrawString(font, "Stretch off", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (1 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //    if (option == 2)
            //    {
            //        if (Game1.sound)
            //            spriteBatch.DrawString(font, "Sound on", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (2 * optionSpace) + offset.Y), MenuScreen.textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //        else
            //            spriteBatch.DrawString(font, "Sound off", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (2 * optionSpace) + offset.Y), MenuScreen.textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //    else
            //    {
            //        if (Game1.sound)
            //            spriteBatch.DrawString(font, "Sound on", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (2 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //        else
            //            spriteBatch.DrawString(font, "Sound off", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (2 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //    if (option == 3)
            //    {
            //        spriteBatch.DrawString(font, "Return", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (3 * optionSpace) + offset.Y), MenuScreen.textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //    else
            //    {
            //        spriteBatch.DrawString(font, "Return", new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (3 * optionSpace) + offset.Y), MenuScreen.activeColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
            //    }
            //}


        }
    }
}
