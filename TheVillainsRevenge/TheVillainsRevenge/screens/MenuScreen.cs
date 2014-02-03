using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class MenuScreen
    {
        Camera camera;
        Character character = new Character(1250, 1000);
        RenderTarget2D renderScreen;
        RenderTarget2D renderSpine;
        RenderTarget2D renderMenu;
        RenderTarget2D renderTitle;
        Texture2D bg_texture;
        Texture2D logo_texture;
        Texture2D credit_texture;
        Texture2D overlay_texture;
        SpriteFont font;
        float fontScale;
        bool levelendScreen;
        bool deadScreen;
        bool startScreen;
        SubMenu mainMenu;
        SubMenu optionMenu;
        public static Color textColor;
        public static Color activeColor;
        public static double spriteTimer;
        public static int spriteDelay = 120;
        public static bool changeSprite = false;


        public MenuScreen(int screen)
        {
            textColor = Color.Red;
            activeColor = Color.DarkRed;
            fontScale = 2.0f;
            if (screen == 1)
            {
                deadScreen = true;
            }
            else if (screen == 2)
            {
                levelendScreen = true;
            }
            else
            {
                startScreen = true;
            }
        }
        public void Load(ContentManager Content)
        {
            camera = new Camera();
            character.Load(Content, Game1.graphics, "bonepuker", 1.5f, 0.5f);
            renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderMenu = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderTitle = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            bg_texture = Content.Load<Texture2D>("sprites/menu/background");
            logo_texture = Content.Load<Texture2D>("sprites/menu/logo");
            credit_texture = Content.Load<Texture2D>("sprites/menu/credits");
            overlay_texture = Content.Load<Texture2D>("sprites/menu/overlay");
            font = Content.Load<SpriteFont>("fonts/schrift");
            mainMenu = new SubMenu(3, "main", font, new Vector2(-750,200), 120, fontScale);
            mainMenu.Load(Content);
                mainMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 63, 100), 4));
                mainMenu.buttons.Add(new Button("options", new Rectangle(0, 100, 63, 100), 4));
                mainMenu.buttons.Add(new Button("exit", new Rectangle(0, 200, 63, 100), 4));
            optionMenu = new SubMenu(4, "option", font, new Vector2(-500,200), 140, fontScale);
            optionMenu.Load(Content);
            optionMenu.buttons.Add(new Button("fullscreen", new Rectangle(0, 0, 122, 75), 3));
            optionMenu.buttons.Add(new Button("stretch", new Rectangle(0, 75, 122, 105), 3));
            optionMenu.buttons.Add(new Button("sound", new Rectangle(0, 185, 122, 132), 3));
            optionMenu.buttons.Add(new Button("exit", new Rectangle(0, 316, 122, 85), 3));
            mainMenu.visible = true;
            Sound.Load(Content);
            if (Game1.sound)
            {
                Sound.PlayStart();
            }
            Cutscene.Load(Content);
            if (startScreen)
            {
                Cutscene.Play("start");
            }
        }
        public int Update(GameTime gameTime)
        {
            if (Game1.sound && Sound.startMusicInstance.State == SoundState.Stopped && Sound.menuMusicInstance == null)
            {
                Sound.PlayMenu();
            }
            if (startScreen && Cutscene.player.State == MediaState.Stopped)
            {
                startScreen = false;
            }
            else
            {
                if (character.spine.animation != "idle")
                {
                    character.spine.anim("idle", 0, true, gameTime);
                }
                //Update SpriteTimer
                if (gameTime.TotalGameTime.TotalMilliseconds > (spriteTimer + (float)spriteDelay))
                {
                    spriteTimer = gameTime.TotalGameTime.TotalMilliseconds;
                    changeSprite = true;
                }
                else
                {
                    changeSprite = false;
                }
                //Update SubMenu
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
                //Auf Menu / Screen reagieren
                if (deadScreen)
                {
                    if (Game1.input.enter || Game1.input.sprung)
                    {
                        deadScreen = false;
                    }
                }
                else if (levelendScreen)
                {
                    if (Game1.input.enter || Game1.input.sprung)
                    {
                        levelendScreen = false;
                        return 2;
                    }
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
                            {
                                Sound.PauseMenu();
                                Game1.sound = false;
                            }
                            else
                            {
                                Game1.sound = true;
                                Sound.PlayMenu();
                            }
                        }
                        else if (optionMenu.option == 1) //Wird noch zu ShowControls umgebaut
                        {
                            if (Game1.stretch)
                                Game1.stretch = false;
                            else
                                Game1.stretch = true;
                        }
                        else //Fullscreentoogle
                        {
                            Game1.ToggleFullscreen();
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
                            return 2;
                        }
                    }
                }
            }
            camera.UpdateTransformation(Game1.graphics);
            return 1;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (startScreen)
            {
                //--------------------Draw to renderScreen--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
                spriteBatch.Draw(Cutscene.player.GetTexture(), Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            else
            {
                //--------------------Draw to renderSpine--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                character.Draw(gameTime, camera);

                //--------------------Draw to renderMenu--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderMenu);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                if (mainMenu.visible && !deadScreen && !levelendScreen)
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
                else if (levelendScreen)
                {
                    spriteBatch.DrawString(font, "Level completed!", new Vector2((Game1.resolution.X / 2) - 50, (Game1.resolution.Y / 2) - 50), textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);
                    spriteBatch.DrawString(font, "Press Enter", new Vector2((Game1.resolution.X / 2) - 60, (Game1.resolution.Y / 2) + 50), textColor, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 1.0f);

                }
                spriteBatch.End();

                //--------------------Draw to renderTitle--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderTitle);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                spriteBatch.Draw(logo_texture, Vector2.Zero, Color.White);
                spriteBatch.Draw(credit_texture, Vector2.Zero, Color.White);
                spriteBatch.End();

                //--------------------Draw to renderScreen--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
                if (!deadScreen && !levelendScreen)
                {
                    spriteBatch.Draw(bg_texture, Vector2.Zero, Color.White);
                    spriteBatch.Draw(renderSpine, Vector2.Zero, Color.White);
                }
                spriteBatch.Draw(renderMenu, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderTitle, Vector2.Zero, Color.White);
                spriteBatch.Draw(overlay_texture, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            //--------------------Draw to Screen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
                spriteBatch.Draw(renderScreen, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
