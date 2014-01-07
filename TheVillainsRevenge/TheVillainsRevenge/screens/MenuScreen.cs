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
        Camera camera;
        RenderTarget2D renderTarget;
        SpriteFont font;
        float fontScale;
        bool deadScreen;
        bool loadScreen;
        SubMenu mainMenu;
        SubMenu optionMenu;
        public static Color textColor;
        public static Color activeColor;
        public static Texture2D menuButtons;
        public static double blinkingTimer;
        public static int blinkingDelay = 500;
        public static bool blinkingState = false;

        public MenuScreen(bool playerDied)
        {
            deadScreen = playerDied;
            textColor = Color.Red;
            activeColor = Color.DarkRed;
            fontScale = 2.0f;
        }
        public void Load(ContentManager Content)
        {
            camera = new Camera();
            renderTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            menuButtons = Content.Load<Texture2D>("sprites/menu_buttons");
            font = Content.Load<SpriteFont>("fonts/schrift");
            mainMenu = new SubMenu(3, "main", font, new Vector2(-600,0), 200, fontScale);
            mainMenu.Load(Content);
                mainMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 300, 100), new Rectangle(300, 0, 300, 100), false));
                mainMenu.buttons.Add(new Button("options", new Rectangle(0, 100, 300, 100), new Rectangle(300, 100, 300, 100), false));
                mainMenu.buttons.Add(new Button("exit", new Rectangle(0, 200, 300, 100), new Rectangle(300, 200, 300, 100), false));
            optionMenu = new SubMenu(4, "option", font, new Vector2(-100,0), 200, fontScale);
            optionMenu.Load(Content);
            optionMenu.buttons.Add(new Button("fullscreen", new Rectangle(0, 300, 300, 100), new Rectangle(300, 300, 300, 100), true));
            optionMenu.buttons.Add(new Button("stretch", new Rectangle(0, 400, 300, 100), new Rectangle(300, 400, 300, 100), true));
            optionMenu.buttons.Add(new Button("sound", new Rectangle(0, 500, 300, 100), new Rectangle(300, 500, 300, 100), true));
            optionMenu.buttons.Add(new Button("exit", new Rectangle(0, 600, 300, 100), new Rectangle(300, 600, 300, 100), false));
            mainMenu.visible = true;
        }
        public int Update(GameTime gameTime)
        {
            UpdateBlinkingTimer(gameTime, false);
            if (optionMenu.visible)
            {
                optionMenu.Update(gameTime);
                if (optionMenu.exit)
                {
                    optionMenu.visible = false;
                }
            }
            else if (mainMenu.visible)
            {
                mainMenu.Update(gameTime);
                if (mainMenu.exit)
                {
                    return 0;
                }
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
                        Game1.toggleFullscreen();
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
            }
            camera.UpdateTransformation(Game1.graphics);
            return 1;
        }

        public static void UpdateBlinkingTimer(GameTime gameTime, bool reset)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > (blinkingTimer + (float)blinkingDelay) || reset)
            {
                blinkingTimer = gameTime.TotalGameTime.TotalMilliseconds;
                blinkingState = !blinkingState;
                if (reset)
                {
                    blinkingState = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //--------------------Draw to Texture--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Game1.graphics.GraphicsDevice.Clear(Color.White);
                if (mainMenu.visible && !deadScreen && !loadScreen)
                {
                    mainMenu.Draw(spriteBatch, gameTime, camera);
                }
                if (optionMenu.visible)
                {
                    optionMenu.Draw(spriteBatch, gameTime, camera);
                }
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                if (deadScreen)
                {
                    spriteBatch.DrawString(font, "Game Over", new Vector2((Game1.resolution.X / 2) - 50, (Game1.resolution.Y / 2) - 50), textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
                    spriteBatch.DrawString(font, "Press Enter", new Vector2((Game1.resolution.X / 2) - 60, (Game1.resolution.Y / 2) + 50), textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);

                }
                else if (loadScreen)
                {
                    spriteBatch.DrawString(font, "Game loading ...", new Vector2((Game1.resolution.X / 2) - 50, (Game1.resolution.Y / 2) - 50), textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);

                }
            spriteBatch.End();
            //--------------------Draw to Screen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
                spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
