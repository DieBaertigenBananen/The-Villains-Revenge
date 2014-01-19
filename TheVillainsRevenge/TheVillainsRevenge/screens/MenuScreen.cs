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
        Character character = new Character(1250, 2100);
        RenderTarget2D renderScreen;
        RenderTarget2D renderSpine;
        RenderTarget2D renderMenu;
        RenderTarget2D renderTitle;
        RenderTarget2D renderLogo;
        RenderTarget2D renderOverlay;
        Texture2D bg_texture;
        Texture2D title_texture;
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
            character.Load(Content, Game1.graphics, "skeleton", 2.5f, 0.5f);
            renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderMenu = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderTitle = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderLogo = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderOverlay = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            bg_texture = Content.Load<Texture2D>("sprites/menu_bg");
            title_texture = Content.Load<Texture2D>("sprites/menu_titles");
            menuButtons = Content.Load<Texture2D>("sprites/menu_buttons");
            font = Content.Load<SpriteFont>("fonts/schrift");
            mainMenu = new SubMenu(3, "main", font, new Vector2(-850,100), 100, fontScale);
            mainMenu.Load(Content);
                mainMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 300, 100), new Rectangle(300, 0, 300, 100), false));
                mainMenu.buttons.Add(new Button("options", new Rectangle(0, 100, 300, 100), new Rectangle(300, 100, 300, 100), false));
                mainMenu.buttons.Add(new Button("exit", new Rectangle(0, 200, 300, 100), new Rectangle(300, 200, 300, 100), false));
            optionMenu = new SubMenu(4, "option", font, new Vector2(-500,100), 100, fontScale);
            optionMenu.Load(Content);
            optionMenu.buttons.Add(new Button("fullscreen", new Rectangle(0, 300, 300, 100), new Rectangle(300, 300, 300, 100), true));
            optionMenu.buttons.Add(new Button("stretch", new Rectangle(0, 400, 300, 100), new Rectangle(300, 400, 300, 100), true));
            optionMenu.buttons.Add(new Button("sound", new Rectangle(0, 500, 300, 100), new Rectangle(300, 500, 300, 100), true));
            optionMenu.buttons.Add(new Button("exit", new Rectangle(0, 600, 300, 100), new Rectangle(300, 600, 300, 100), false));
            mainMenu.visible = true;
        }
        public int Update(GameTime gameTime)
        {
            if (character.spine.animation != "idle")
            {
                character.spine.anim("idle", 0, true, gameTime);
            }

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
            //--------------------Draw to renderSpine--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            character.Draw(gameTime, camera);

            //--------------------Draw to renderMenu--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderMenu);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
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

            //--------------------Draw to renderTitle--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTitle);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                spriteBatch.Draw(title_texture, new Vector2(50, 100), new Rectangle(0,0,600,300), Color.White);
            spriteBatch.End();

            //--------------------Draw to renderLogo--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderLogo);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                spriteBatch.Draw(title_texture, new Vector2(1800, 950), new Rectangle(0, 300, 100, 100), Color.White);
            spriteBatch.End();

            //--------------------Draw to renderOverlay--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderOverlay);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                spriteBatch.Draw(renderTitle, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderLogo, Vector2.Zero, Color.White);
            spriteBatch.End();

            //--------------------Draw to renderScreen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
                if (!loadScreen && !deadScreen)
                {
                    spriteBatch.Draw(bg_texture, Vector2.Zero, Color.White);
                    spriteBatch.Draw(renderSpine, Vector2.Zero, Color.White);
                }
                spriteBatch.Draw(renderMenu, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderOverlay, Vector2.Zero, Color.White);
            spriteBatch.End();

            //--------------------Draw to Screen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
                spriteBatch.Draw(renderScreen, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
