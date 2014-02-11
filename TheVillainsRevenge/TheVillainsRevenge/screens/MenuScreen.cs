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
        Character character = new Character(1400, 1000);
        RenderTarget2D renderScreen;
        RenderTarget2D renderSpine;
        RenderTarget2D renderMenu;
        RenderTarget2D renderTitle;
        Texture2D bg_texture;
        Texture2D logo_texture;
        Texture2D credit_texture;
        Texture2D overlay_texture;
        Texture2D controls_texture;
        Texture2D levelend_texture;
        Texture2D gameover_texture;
        bool levelendScreen;
        bool deadScreen;
        bool startScreen;
        bool controlScreen;
        SubMenu mainMenu;
        SubMenu optionMenu;
        SubMenu startMenu;
        public static Color textColor;
        public static Color activeColor;
        public static double spriteTimer;
        public static int spriteDelay = 120;
        public static bool changeSprite = false;


        public MenuScreen(int screen)
        {
            textColor = Color.Red;
            activeColor = Color.DarkRed;
            if (screen == 1)
            {
                deadScreen = true;
            }
            else if (screen == 2)
            {
                levelendScreen = true;
            }
            else if (screen == 3)
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
            controls_texture = Content.Load<Texture2D>("sprites/menu/controls");
            gameover_texture = Content.Load<Texture2D>("sprites/menu/gameover");
            levelend_texture = Content.Load<Texture2D>("sprites/menu/levelend");
            mainMenu = new SubMenu(3, "main", new Vector2(-650,200), 120);
            mainMenu.Load(Content);
                mainMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 63, 100), 4));
                mainMenu.buttons.Add(new Button("options", new Rectangle(0, 100, 63, 100), 4));
                mainMenu.buttons.Add(new Button("exit", new Rectangle(0, 200, 63, 100), 4));
            startMenu = new SubMenu(3, "start", new Vector2(-400, 200), 140);
            startMenu.Load(Content);
                startMenu.buttons.Add(new Button("continue", new Rectangle(0, 0, 122, 75), 3));
                startMenu.buttons.Add(new Button("newgame", new Rectangle(0, 75, 122, 105), 3));
                startMenu.buttons.Add(new Button("newgame", new Rectangle(0, 180, 122, 85), 3));
            optionMenu = new SubMenu(4, "option", new Vector2(-400,200), 140);
            optionMenu.Load(Content);
            optionMenu.buttons.Add(new Button("fullscreen", new Rectangle(0, 0, 122, 75), 3));
            optionMenu.buttons.Add(new Button("stretch", new Rectangle(0, 75, 122, 105), 3));
            optionMenu.buttons.Add(new Button("sound", new Rectangle(0, 185, 122, 132), 3));
            optionMenu.buttons.Add(new Button("exit", new Rectangle(0, 316, 122, 85), 3));
            mainMenu.visible = true;
            Sound.Load(Content);
            if (Game1.sound && startScreen)
            {
                Sound.startMusicInstance.Play();
            }
            Cutscene.Load(Content);
            if (startScreen)
            {
                Cutscene.Play("start");
            }
        }
        public int Update(GameTime gameTime)
        {
            if (Game1.sound && Sound.startMusicInstance.State == SoundState.Stopped && Sound.menuMusicInstance.State == SoundState.Stopped)
            {
                Sound.menuMusicInstance.Play();
            }
            if (startScreen)
            {
                if (Cutscene.player.State == MediaState.Stopped)
                {
                    startScreen = false;
                }
                else if (Game1.input.sprung||Game1.input.skip)
                {
                    startScreen = false;
                    Sound.startMusicInstance.Stop();
                    Sound.menuMusicInstance.Play();
                }
            }
            else
            {
                if (character.spine.animation != "menu")
                {
                    character.spine.anim("menu", 1, true);
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
                if (!deadScreen && !controlScreen && !levelendScreen)
                {
                    //Update SubMenu
                    if (optionMenu.visible)
                    {
                        optionMenu.Update(gameTime, changeSprite);
                        if (optionMenu.exit)
                        {
                            optionMenu.visible = false;
                        }
                    }
                    else if (startMenu.visible)
                    {
                        startMenu.Update(gameTime, changeSprite);
                        if (startMenu.exit)
                        {
                            startMenu.visible = false;
                        }
                    }
                    else if (mainMenu.visible)
                    {
                        mainMenu.Update(gameTime, changeSprite);
                        if (mainMenu.exit)
                        {
                            return 0;
                        }
                    }
                }
                //Auf Menu / Screen reagieren
                if (deadScreen)
                {
                    deadScreen = false;
                    if (Game1.input.sprung)
                    {
                        deadScreen = false;
                    }
                }
                else if (levelendScreen)
                {
                    levelendScreen = false;
                    if (Game1.input.sprung)
                    {
                        levelendScreen = false;
                        return 2;
                    }
                }
                else if (controlScreen)
                {
                    if (Game1.input.back || Game1.input.leftM)
                    {
                        controlScreen = false;
                        optionMenu.option = 1;
                    }
                }
                else if (optionMenu.visible)
                {
                    //Enter wählt Menüfelder
                    if (Game1.input.sprung || Game1.input.rightM)
                    {
                        //Option == 2 ist Exit
                        if (optionMenu.option == 3)
                        {
                            if (Game1.input.sprung) //Safe dass man nicht mit nach rechts drücken Escaped
                            {
                                optionMenu.visible = false;
                                mainMenu.option = 1;
                            }
                        }
                        //Option = 1 ist stretch
                        else if (optionMenu.option == 2)
                        {
                            if (Game1.sound)
                            {
                                Sound.menuMusicInstance.Stop();
                                Game1.sound = false;
                            }
                            else
                            {
                                Game1.sound = true;
                                Sound.menuMusicInstance.Play();
                            }
                        }
                        else if (optionMenu.option == 1) //ShowControls
                        {
                            controlScreen = true;
                        }
                        else //Fullscreentoogle
                        {
                            Game1.ToggleFullscreen();
                        }
                    }
                }
                else if (startMenu.visible)
                {
                    if (Game1.input.sprung || Game1.input.rightM)
                    {
                        //Option == 2 ist Exit
                        if (startMenu.option == 2)
                        {
                            if (Game1.input.sprung) //Safe dass man nicht mit nach rechts drücken Escaped
                            {
                                startMenu.visible = false;
                                mainMenu.option = 0;
                            }
                        }
                        //Option = 1 ist New Game
                        else if (startMenu.option == 1)
                        {
                            return 3;
                        }
                        else
                        {
                            //Game Continue
                            return 2;
                        }
                    }
                }
                else if (mainMenu.visible)
                {
                    if (Game1.input.sprung || Game1.input.rightM)
                    {
                        //Option == 2 ist Exit
                        if (mainMenu.option == 2)
                        {
                            if (Game1.input.sprung) //Safe dass man nicht mit nach rechts drücken Escaped
                            {
                                return 0;
                            }
                        }
                        //Option = 1 ist OptionMenu
                        else if (mainMenu.option == 1)
                        {
                            optionMenu.visible = true;
                            optionMenu.option = 0;
                        }
                        else
                        {
                            //Game Start
                            startMenu.visible = true;
                            startMenu.option = 0;
                        }
                    }
                }
            }
            camera.UpdateTransformation(Game1.graphics);
            return 1;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (startScreen || controlScreen || deadScreen || levelendScreen)
            {
                //--------------------Draw to renderScreen--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
                if (startScreen)
                {
                    Cutscene.Draw(spriteBatch);
                }
                else if (controlScreen)
                {
                    spriteBatch.Draw(controls_texture, Vector2.Zero, new Rectangle(0,0,0,0), Color.White, 0f, Vector2.Zero, 1920f/controls_texture.Width, SpriteEffects.None, 0f);
                }
                else if (deadScreen)
                {
                    spriteBatch.Draw(gameover_texture, Vector2.Zero, Color.White);
                }
                else if (levelendScreen)
                {
                    spriteBatch.Draw(levelend_texture, Vector2.Zero, Color.White);
                }
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
                //Draw SubMenus
                if (mainMenu.visible)
                {
                    mainMenu.Draw(spriteBatch, gameTime, camera);
                }
                if (optionMenu.visible)
                {
                    optionMenu.Draw(spriteBatch, gameTime, camera);
                }
                if (startMenu.visible)
                {
                    startMenu.Draw(spriteBatch, gameTime, camera);
                }

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
                spriteBatch.Draw(bg_texture, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderMenu, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderTitle, Vector2.Zero, Color.White);
                spriteBatch.Draw(overlay_texture, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderSpine, Vector2.Zero, Color.White);
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
