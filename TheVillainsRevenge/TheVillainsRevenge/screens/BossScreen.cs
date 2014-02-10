﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LuaInterface;

namespace TheVillainsRevenge
{
    class BossScreen
    {
        Texture2D texture;
        public PrincessSpieler spieler = new PrincessSpieler(40, 1000);
        Map karte = new Map();
        Boss hero = new Boss(0, 0);
        public static Lua LuaKI = new Lua();
        Camera camera = new BossCam();
        Camera pauseCamera = new Camera();
        SubMenu pauseMenu;
        BossGUI GUI = new BossGUI();
        Texture2D bg_texture,fg_texture;
        public static int bossleben = 100;
        int bosslebenshow = 100;
        public bool paused = false;
        Effect pause,scream;
        public static double spriteTimer;
        public static int spriteDelay = 120;
        public static bool changeSprite = false;


        RenderTarget2D renderSpine;
        RenderTarget2D renderGame;
        RenderTarget2D renderScreen;

        public int getPoints(string w)
        {
            if (w == "Spieler")
                return spieler.kicheck.Count() - 1;
            else
                return hero.kicheck.Count() - 1;
        }
        public int getPointID(int s, string w)
        {
            if (w == "Spieler")
                return spieler.kicheck.ElementAt(s).id;
            else
                return hero.kicheck.ElementAt(s).id;
        }
        public int getPointTime(int s, string w)
        {
            if (w == "Spieler")
                return spieler.kicheck.ElementAt(s).time;
            else
                return hero.kicheck.ElementAt(s).time;
        }
        public void removePoint(int s)
        {
            if (hero.kicheck.Count() != 0)
            {
                hero.kicheck.RemoveAt(s);
            }
        }
        public void addPoint(int s, int t)
        {
            bool geht = true;
            if (hero.kicheck.Count() != 0)
            {
                if (hero.kicheck.ElementAt(hero.kicheck.Count() - 1).id == s)
                {
                    geht = false;
                }

            }
            if (geht)
                hero.kicheck.Add(new KICheck(t, s));
        }

        public BossScreen()
        {
            texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
            LuaKI.RegisterFunction("getPoints", this, this.GetType().GetMethod("getPoints"));
            LuaKI.RegisterFunction("getPointID", this, this.GetType().GetMethod("getPointID"));
            LuaKI.RegisterFunction("getPointTime", this, this.GetType().GetMethod("getPointTime"));
            LuaKI.RegisterFunction("addPoint", this, this.GetType().GetMethod("addPoint"));
            LuaKI.RegisterFunction("removePoint", this, this.GetType().GetMethod("removePoint"));
            bossleben = 100;
        }

        public void Load(ContentManager Content)
        {
            bg_texture = Content.Load<Texture2D>("sprites/level_5/planes/background");
            fg_texture = Content.Load<Texture2D>("sprites/level_5/planes/foreground");
            pause = Content.Load<Effect>("Pause");
            scream = Content.Load<Effect>("Scream");
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderGame = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            spieler.Load(Content, Game1.graphics);
            hero.Load(Content, Game1.graphics);
            hero.start = true;
            karte.Load(Content);
            GUI.Load(Content);
            karte.Generate(spieler, hero);
            Sound.Load(Content);
            if (Game1.sound)
            {
                Sound.bgMusicInstance.Play();
            }
            pauseMenu = new SubMenu(2, "pause", new Vector2(-30, -100), 120);
            pauseMenu.Load(Content);
            pauseMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 63, 100), 4));
            pauseMenu.buttons.Add(new Button("exit", new Rectangle(0, 200, 63, 100), 4));
        }

        public int Update(GameTime gameTime, ContentManager Content)
        {
            if (!paused)
            {
                Game1.time += gameTime.ElapsedGameTime;
                //--------------------Map--------------------
                karte.Update(gameTime, spieler.cbox.box, hero.cbox.box);
                //Objekte updaten
                for (int i = 0; i < karte.objects.Count(); i++)
                {
                    Obj obj = karte.objects.ElementAt(i);
                    obj.Update(gameTime, karte);
                    if (obj.type == 4)
                    {
                        if (obj.box.Intersects(spieler.cbox.box))
                        {
                            spieler.getHit("die2");
                            karte.objects.Remove(obj);
                        }
                        else if (obj.position.X < 0 || obj.position.X > karte.size.X)
                            karte.objects.Remove(obj);
                    }
                }
                //--------------------Spieler--------------------
                spieler.Update(gameTime, karte, hero.cbox.box);

                if (bosslebenshow != bossleben)
                    bosslebenshow--;
                else
                {
                    if (spieler.hit && hero.schlagbar)
                    {
                        if (spieler.spine.BoundingBoxCollision(hero.cbox.box))
                        {
                            bossleben -= 5;
                            hero.gethit();
                        }
                    }
                    else if (spieler.smash)
                    {            
                        int x = spieler.cbox.box.X + (spieler.cbox.box.Width/2);
                        int y = spieler.cbox.box.Y + (spieler.cbox.box.Height/2);
                        if (Circle.Intersects(new Vector2(x, y), spieler.screamradius, hero.cbox.box)&&hero.screamhit)
                        {
                            bossleben -= 20;
                            if (bossleben < 20)
                                bossleben = 0;
                            hero.gethit();
                            hero.screamhit = false;
                        }
                    }
                    else if(!hero.screamhit)
                        hero.screamhit = true;
                }

                //--------------------Hero--------------------
                hero.Update(gameTime, karte, spieler.cbox.box,spieler.smash);
                Rectangle herohit = hero.cbox.box;
                if (!hero.richtung) //Hero ist rechts
                    hero.cbox.box.X -= 48;
                else //Hero links
                    hero.cbox.box.X += 48;
                if (spieler.cbox.box.Intersects(hero.cbox.box) && hero.start && hero.attacktimer <= 0 && hero.animeTime <= 0 && !hero.welleladen)
                {
                    hero.attack();
                }
                else if (spieler.cbox.box.Intersects(hero.cbox.box) && hero.start && hero.hits && !spieler.ishit)
                {
                    Sound.Play("attack");
                    spieler.ishit = true;
                    spieler.getHit("");
                }
                else if(spieler.ishit&&!hero.hits)
                    spieler.ishit = false;
                if (!hero.richtung) //Hero ist rechtss
                    hero.cbox.box.X += 48;
                else //Hero links
                    hero.cbox.box.X -= 48;
                //KiPunkte
                for (int i = 0; i < karte.kipoints.Count(); i++)
                {
                    KIPoint kipoint = karte.kipoints.ElementAt(i);
                    //Wenn Spieler sie übertritt
                    if (spieler.cbox.box.Intersects(kipoint.cbox))
                    {
                        bool geht = true;
                        if (spieler.kicheck.Count() > 0)
                        {
                            //Falls es der selbe Punkt ist mache nicht weiter
                            KICheck check = spieler.kicheck.ElementAt(spieler.kicheck.Count() - 1);
                            if (check.id == kipoint.id)
                                geht = false;
                        }
                        if (geht) //Ist nicht der selbe Punkt wie vorher, speichere
                        {
                            if (spieler.kicheck.Count() >= 20) //Nicht mehr als 20 punkte
                            {
                                spieler.kicheck.RemoveAt(0);
                            }
                            //Adde die Punkte und führ Skript aus
                            spieler.kicheck.Add(new KICheck((int)Game1.time.TotalSeconds, kipoint.id));
                            LuaKI.DoFile("Level_" + Game1.level + "/kiscript.txt");
                        }
                    }
                    //Wenn held ein Kipoint übertritt
                    if (hero.cbox.box.Intersects(kipoint.cbox) && hero.kicheck.Count() != 0)
                    {
                        //Lösche diesen
                        if (hero.kicheck.ElementAt(0).id == kipoint.id)
                        {
                            if (hero.kistate == 2)
                            {
                                hero.kistate = 4;
                            }
                            hero.kicheck.RemoveAt(0);
                        }
                    }
                }
                //--------------------Camera--------------------
                camera.Update(Game1.graphics, spieler, karte);
                if (Game1.input.pause)
                {
                    spieler.spine.Save();
                    hero.spine.Save();
                    paused = true;
                }
                return 1;
            }
            else
            {
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
                pauseMenu.Update(gameTime, changeSprite);
                if (pauseMenu.exit)
                {
                    return 4;
                }
                if (Game1.input.sprung)
                {
                    //Option == 2 ist Exit
                    if (pauseMenu.option == 1)
                    {
                        if (Game1.input.sprung) //Safe dass man nicht mit nach links drücken Escaped
                        {
                            return 4;
                        }
                    }
                    else
                    {
                        //Resume Game
                        paused = false;
                    }
                }
                else if (Game1.input.pause)
                {
                    paused = false;
                }
                spieler.spine.Reset();
                hero.spine.Reset();
                camera.UpdateTransformation(Game1.graphics);
                pauseCamera.UpdateTransformation(Game1.graphics);
                return 1;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderSpine
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spieler.Draw(gameTime, camera);
            hero.Draw(gameTime, camera);

            //----------------------------------------------------------------------
            //----------------------------------------Draw to RenderGame
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderGame);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            karte.Draw(spriteBatch, gameTime, camera); //Plattformen & Co
            int x = spieler.cbox.box.X + (spieler.cbox.box.Width/2);
            int y = spieler.cbox.box.Y + (spieler.cbox.box.Height/2);
            if (spieler.smash)
            {
                if (Circle.Intersects(new Vector2(x, y), spieler.screamradius, hero.cbox.box))
                    //Zeichne Kreis weil Intersekt
                    spriteBatch.Draw(Circle.createCircle(Game1.graphics.GraphicsDevice, spieler.screamradius * 2), Circle.Middle(spieler.screamradius, x, y), Color.Green);
                else
                    //Zeichne Kreis nicht Intersekt
                    spriteBatch.Draw(Circle.createCircle(Game1.graphics.GraphicsDevice, spieler.screamradius * 2), Circle.Middle(spieler.screamradius, x, y), Color.Red);
            }
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
            if (Game1.debug) //Boundingboxen
            {
                spriteBatch.Draw(texture, spieler.cbox.box, null, Color.White);
                spriteBatch.Draw(texture, hero.cbox.box, null, Color.White);
            }
            GUI.Draw(spriteBatch, spieler.lifes, bosslebenshow);
            spriteBatch.End();


            if (paused)
            {
                pauseMenu.Draw(spriteBatch, gameTime, pauseCamera);
            }

            //----------------------------------------------------------------------
            //----------------------------------------Draw to Screen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            spriteBatch.Draw(renderScreen, new Vector2(), Color.White);
            spriteBatch.End();

            //----------------------------------------------------------------------
            //----------------------------------------Draw to Screen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            if (paused)
            {
                pause.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, pause, camera.screenTransform);
            }
            else if(spieler.smash)
            {
                scream.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                scream.Parameters["radius"].SetValue(spieler.screamradius);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, scream, camera.screenTransform);
             }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            }
            spriteBatch.Draw(bg_texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(fg_texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderGame, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
