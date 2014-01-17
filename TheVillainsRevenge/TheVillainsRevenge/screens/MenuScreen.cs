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
            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderSpine
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spieler.Draw(gameTime, camera);
            hero.Draw(gameTime, camera);

            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderBackground
            //----------------------------------------------------------------------
            //Background3
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground3);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            background_3.Draw(spriteBatch, spieler); //Himmel
            clouds_3.Draw(spriteBatch);
            spriteBatch.End();
            //Background2
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground2);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            background_2.Draw(spriteBatch, spieler); //Berge
            clouds_2.Draw(spriteBatch);
            spriteBatch.End();
            //Background1
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground1);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            background_1.Draw(spriteBatch, spieler); //Wald
            clouds_1.Draw(spriteBatch);
            spriteBatch.End();
            //Background0
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground0);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            if (!Game1.debug)
            {
                background_0.Draw(spriteBatch, spieler); //Bäume
            }
            else
            {
                spriteBatch.Draw(debug, new Vector2(background_0.position.X, background_0.position.Y), Color.White);
            }
            spriteBatch.End();
            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderForeground
            //----------------------------------------------------------------------
            //Foreground0
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderForeground_0);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            foreground_0.Draw(spriteBatch, spieler); //Ebene
            spriteBatch.End();
            //Foreground1
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderForeground_1);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            foreground_1.Draw(spriteBatch, spieler); //Ebene
            spriteBatch.End();
            //----------------------------------------------------------------------
            //----------------------------------------Draw to RenderGame
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderGame);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            karte.Draw(spriteBatch, gameTime, camera); //Plattformen & Co
            hero.Draw(gameTime, camera); //Ashbrett
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
            if (Game1.debug) //Boundingboxen
            {
                spriteBatch.Draw(texture, spieler.cbox.box, null, Color.White);
                spriteBatch.Draw(texture, hero.cbox.box, null, Color.White);
                for (int i = 0; i < karte.enemies.Count(); i++)
                {
                    Enemy enemy = karte.enemies.ElementAt(i);
                    spriteBatch.Draw(texture, enemy.cbox.box, null, Color.White);
                }
                spriteBatch.Draw(texture, hero.kicollide, null, Color.Red);

            }
            spriteBatch.End();

            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderHud
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderHud);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (levelend)
            {
                spriteBatch.DrawString(font, "Finished level!", new Vector2((Game1.resolution.X / 2) - 200, (Game1.resolution.Y / 2) - 200), Color.Black, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);
            }
            if (Game1.debug)
            {
                //for (int i = 0; i <= 62; i++)
                //{
                spriteBatch.DrawString(font, spieler.acceleration + " " + spieler.spine.animation, new Vector2(Game1.resolution.X - 300, 150), Color.White);
                //}
                spriteBatch.DrawString(font, "bg0.tex[0]: " + (background_0.texture[0].Name), new Vector2(Game1.resolution.X - 300, 170), Color.White);
                spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(Game1.resolution.X - 300, 190), Color.White);
                spriteBatch.DrawString(font, "Hero: " + (hero.position.X + " " + hero.position.Y), new Vector2(Game1.resolution.X - 300, 210), Color.White);
                spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y + " " + camera.viewport.Width + " " + camera.viewport.Height), new Vector2(Game1.resolution.X - 300, 230), Color.White);
                spriteBatch.DrawString(font, "Skeleton: " + (spieler.spine.skeleton.X + " " + spieler.spine.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                spriteBatch.DrawString(font, "RageMeter: " + princess.rageMeter, new Vector2(Game1.resolution.X - 300, 290), Color.White);
                Slot bb = spieler.spine.skeleton.FindSlot("bonepuker");
                spriteBatch.DrawString(font, "bb-bonepuker: " + spieler.spine.bounds.BoundingBoxes.FirstOrDefault(), new Vector2(Game1.resolution.X - 300, 310), Color.White);
                spriteBatch.DrawString(font, "SlowTime: " + slow + " Vergangen: " + slowTime, new Vector2(Game1.resolution.X - 300, 330), Color.White);
                spriteBatch.DrawString(font, "KIState: " + hero.kistate, new Vector2(Game1.resolution.X - 300, 350), Color.White);
                spriteBatch.DrawString(font, "SmashCooldown: " + spieler.megacooldown, new Vector2(Game1.resolution.X - 300, 370), Color.White);
                if (spieler.schlag)
                {
                    spriteBatch.DrawString(font, "SCHLAG", new Vector2(Game1.resolution.X - 500, 350), Color.White);
                }
                else if (spieler.megaschlag)
                {
                    spriteBatch.DrawString(font, "MEGAAA SMAAASH", new Vector2(Game1.resolution.X - 500, 350), Color.White);
                }
                for (int i = 0; i < spieler.kicheck.Count(); i++)
                {
                    KICheck kicheck = spieler.kicheck.ElementAt(i);
                    spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(Game1.resolution.X - 300, 390 + i * 20), Color.White);
                }
                for (int i = 0; i < hero.kicheck.Count(); i++)
                {
                    KICheck kicheck = hero.kicheck.ElementAt(i);
                    spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(Game1.resolution.X - 400, 390 + i * 20), Color.White);
                }
            }
            gui.Draw(spriteBatch, spieler.lifes, spieler.position, hero.position, karte.size, spieler.item1, spieler.item2);
            spriteBatch.End();

            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderScreen
            //----------------------------------------------------------------------
            //-----Background-----
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, outline); //-----[Shader]-----Outline
            //Background3
            outline.Parameters["lineSize"].SetValue(10);
            outline.Parameters["lineBrightness"].SetValue(4);
            spriteBatch.Draw(renderBackground3, Vector2.Zero, Color.White);
            //Background2
            outline.Parameters["lineSize"].SetValue(15);
            outline.Parameters["lineBrightness"].SetValue(3);
            spriteBatch.Draw(renderBackground2, Vector2.Zero, Color.White);
            //Background1
            outline.Parameters["lineSize"].SetValue(20);
            outline.Parameters["lineBrightness"].SetValue(2);
            spriteBatch.Draw(renderBackground1, Vector2.Zero, Color.White);
            //Background0
            outline.Parameters["lineSize"].SetValue(20);
            outline.Parameters["lineBrightness"].SetValue(0);
            spriteBatch.Draw(renderBackground0, Vector2.Zero, Color.White);
            spriteBatch.End();
            //-----Spielebene-----
            if (princess.beating || spieler.megaschlag) //-----[Shader]-----Smash
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, smash);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            }
            spriteBatch.Draw(renderForeground_0, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderGame, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderForeground_1, Vector2.Zero, Color.White);
            spriteBatch.End();

            //----------------------------------------------------------------------
            //----------------------------------------Draw to Screen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);

            //-----renderTarget-----
            if (princess.coverEyes) //-----[Shader]-----CoverEyes
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, coverEyes, camera.screenTransform);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            }
            spriteBatch.Draw(renderScreen, new Vector2(), Color.White);
            spriteBatch.End();

            //-----HUD-----
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            spriteBatch.Draw(renderHud, new Vector2(), Color.White);
            spriteBatch.End();





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
