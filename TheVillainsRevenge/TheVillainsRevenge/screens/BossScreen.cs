using System;
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
        SpriteFont font;
        Texture2D texture;
        public PrincessSpieler spieler = new PrincessSpieler(40, 1000);
        Map karte = new Map();
        Boss hero = new Boss(0, 0);
        public static Lua LuaKI = new Lua();
        Camera camera = new BossCam();
        Camera pauseCamera = new Camera();
        SubMenu pauseMenu;
        BossGUI GUI = new BossGUI();
        Texture2D fg_texture;
        public static int bossleben = 100;
        int bosslebenshow = 100;
        public bool paused = false;
        Effect pause,scream, attack;
        public static double spriteTimer;
        public static int spriteDelay = 120;
        public static bool changeSprite = false;
        Texture2D circle;
        bool cutscene = false;

        Spine background = new Spine();
        RenderTarget2D renderSpine;
        RenderTarget2D renderGame;
        RenderTarget2D renderScreen;
        RenderTarget2D renderBG;
        RenderTarget2D renderShader;

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
            cutscene = true;
        }

        public void Load(ContentManager Content)
        {
            circle = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            circle.SetData<Color>(new Color[]{Color.White});
            background.Load(new Vector2(0,0), "background", 1.0f, 1.0f);
            background.anim("background", 0, true);
            fg_texture = Content.Load<Texture2D>("sprites/level_5/planes/foreground");
            pause = Content.Load<Effect>("Pause");
            scream = Content.Load<Effect>("Scream");
            attack = Content.Load<Effect>("Attack");
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderGame = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderBG = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderShader = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            spieler.Load(Content, Game1.graphics);
            spieler.lifes = 4;
            hero.Load(Content, Game1.graphics);
            hero.start = true;
            hero.spine.anim("idle", 0, false);
            hero.animeTime = 1.0f;
            karte.Load(Content);
            GUI.Load(Content);
            karte.Generate(spieler, hero);
            font = Content.Load<SpriteFont>("fonts/schrift");
            pauseMenu = new SubMenu(2, "pause", new Vector2(-60, -100), 200);
            pauseMenu.Load(Content);
            pauseMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 150, 175), 4));
            pauseMenu.buttons.Add(new Button("exit", new Rectangle(0, 390, 150, 250), 4));
            Sound.Load(Content);
            Cutscene.Load(Content);
            if (cutscene)
            {
                Cutscene.Play("final");
            }
        }

        public int Update(GameTime gameTime, ContentManager Content)
        {
            if (cutscene)
            {
                if (Cutscene.player.State == MediaState.Stopped)
                {
                    cutscene = false;
                }
                else if (Game1.input.skip)
                {
                    cutscene = false;
                    if (Game1.sound)
                    {
                        Cutscene.player.Stop();
                        Sound.bgMusicInstance.Play();
                    }
                }
                if (!cutscene && Game1.sound)
                {
                    Sound.bgMusicInstance.Play();
                }
            }
            else
            {
                if (!paused)
                {
                    Game1.time += gameTime.ElapsedGameTime;
                    //--------------------Map--------------------
                    karte.Update(gameTime, spieler.cbox.box, hero.cbox.box);
                    //Objekte updaten
                    //for (int i = 0; i < karte.objects.Count(); i++)
                    //{
                    //    Obj obj = karte.objects.ElementAt(i);
                    //    obj.Update(gameTime, karte);
                    //    if (obj.type == 4)
                    //    {
                    //        if (obj.box.Intersects(spieler.cbox.box))
                    //        {
                    //            spieler.getHit(gameTime, karte, hero.position, "");
                    //            karte.objects.Remove(obj);
                    //        }
                    //        else if (obj.position.X < 0 || obj.position.X > karte.size.X)
                    //            karte.objects.Remove(obj);
                    //    }
                    //}
                    if (hero.waveRolling)
                    {
                        if (hero.wavefront.Intersects(spieler.cbox.box))
                        {
                            spieler.getHit(gameTime, karte, hero.position, "");
                            hero.waveRolling = false;
                        }
                        else if (hero.wavefront.X < 0 || hero.wavefront.X > karte.size.X)
                            hero.waveRolling = false;
                        if (!hero.waveRolling)
                        {
                            hero.waveRolling = false;
                            hero.waveCooldown = 10;
                            hero.animeTime = 1.0f;
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
                            int x = spieler.cbox.box.X + (spieler.cbox.box.Width / 2);
                            int y = spieler.cbox.box.Y + (spieler.cbox.box.Height / 2);
                            if (Circle.Intersects(new Vector2(x, y), spieler.screamradius, hero.cbox.box) && hero.screamhit)
                            {
                                bossleben -= 20;
                                if (bossleben < 20)
                                    bossleben = 0;
                                hero.gethit();
                                hero.screamhit = false;
                            }
                        }
                        else if (!hero.screamhit)
                            hero.screamhit = true;
                        if (!hero.schlagbar && spieler.hit)
                        {
                            if (spieler.spine.BoundingBoxCollision(hero.cbox.box))
                            {
                                hero.defend();
                            }
                        }
                    }

                    //--------------------Hero--------------------
                    hero.Update(gameTime, karte, spieler.cbox.box, spieler.smash);
                    Rectangle herohit = hero.cbox.box;
                    if (!hero.notFlipped) //Hero ist rechts
                        herohit.X -= 64;
                    else //Hero links
                        herohit.X += 64;
                    herohit.Y = herohit.Y + herohit.Height - 64;
                    herohit.Height = 64;
                    if (spieler.cbox.box.Intersects(herohit) && hero.start && hero.attacktimer <= 0 && hero.animeTime <= 0 && !hero.waveRolling)
                    {
                        hero.attack();
                    }
                    else if (spieler.cbox.box.Intersects(herohit) && hero.start && hero.hits && !spieler.ishit)
                    {
                        Sound.Play("attack");
                        spieler.getHit(gameTime, karte, hero.position, "");
                    }
                    if (spieler.lifes == 0)
                        GUI.GFace = 2;
                    else if (spieler.ishit)
                        GUI.GFace = 1;
                    else
                        GUI.GFace = 0;
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
                        background.Save();
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
                    background.Reset();
                    spieler.spine.Reset();
                    hero.spine.Reset();
                    camera.UpdateTransformation(Game1.graphics);
                    pauseCamera.UpdateTransformation(Game1.graphics);
                    return 1;
                }
            }
            return 1;
        }

        void drawPartCircel(float radius, float startAngel, float endAngel, Vector2 pos, SpriteBatch batch, Color color)
        {

            for (float i = startAngel; i <= endAngel; i += 0.1f)
            {
                float x = (float)(pos.X + Math.Cos(i * Math.PI / 180) * radius);
                float y = (float)(pos.Y - Math.Sin(i * Math.PI / 180) * radius);

                Vector2 pixelpos = new Vector2(x, y);
                batch.Draw(circle, pixelpos, color);
            }
        }

        void DrawScreamCircles(SpriteBatch spriteBatch, int x, int y)
        {
            if (spieler.smash)
            {
                if (Circle.Intersects(new Vector2(x, y), spieler.screamradius, hero.cbox.box)) //Zeichne Kreis weil Intersekt
                {
                    for (int i = 0; i < 5; i++)
                    {
                        drawPartCircel(spieler.screamradius  - i * 5, 0f, 360f, new Vector2((float)x, (float)y), spriteBatch, Color.Green);
                    }
                }
                else //Zeichne Kreis nicht Intersekt
                {
                    for (int i = 0; i < 5; i++)
                    {
                        drawPartCircel(spieler.screamradius - i * 5, 0f, 360f, new Vector2((float)x, (float)y), spriteBatch, Color.Red);
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (cutscene)
            {
                //--------------------Draw to renderScreen--------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
                Cutscene.Draw(spriteBatch);
                spriteBatch.End();
                camera.UpdateTransformation(Game1.graphics);
            }
            else
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
                int x = spieler.cbox.box.X + (spieler.cbox.box.Width / 2);
                int y = spieler.cbox.box.Y + (spieler.cbox.box.Height / 2);
                int herox = hero.cbox.box.X + (hero.cbox.box.Width / 2);
                int heroy = hero.cbox.box.Y + (hero.cbox.box.Height / 2);
                //Ursprünglich hier ScreamCircles gedrawed
                spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
                if (Game1.debug) //Boundingboxen
                {
                    spriteBatch.Draw(texture, spieler.cbox.box, null, Color.White);
                    spriteBatch.Draw(texture, hero.cbox.box, null, Color.White);
                    spriteBatch.Draw(texture, hero.wavefront, null, Color.White);
                }
                GUI.Draw(spriteBatch, spieler.lifes, bosslebenshow);
                spriteBatch.End();


                if (paused)
                {
                    pauseMenu.Draw(spriteBatch, gameTime, pauseCamera);
                }
                //----------------------------------------------------------------------
                //----------------------------------------Draw to BG
                //----------------------------------------------------------------------

                Game1.graphics.GraphicsDevice.SetRenderTarget(renderBG);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                background.Draw(gameTime, camera, new Vector2(karte.size.X / 2, karte.size.Y));
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                spriteBatch.Draw(fg_texture, Vector2.Zero, Color.White);
                spriteBatch.End();

                //----------------------------------------------------------------------
                //----------------------------------------Draw to renderShader
                //----------------------------------------------------------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderShader);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                if (spieler.position.X > 0)
                {
                    scream.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                    scream.Parameters["radius"].SetValue(spieler.screamradius);
                    scream.Parameters["playerX"].SetValue(x - camera.viewport.X);
                    scream.Parameters["playerY"].SetValue(y - camera.viewport.Y);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, scream, camera.viewportTransform);
                }
                else
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                }
                spriteBatch.Draw(renderBG, Vector2.Zero, Color.White);
                for (int i = 0; i < spieler.kicheck.Count(); i++)
                {
                    KICheck kicheck = spieler.kicheck.ElementAt(i);
                    spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(10, 400 + i * 20), Color.White);
                }
                for (int i = 0; i < hero.kicheck.Count(); i++)
                {
                    KICheck kicheck = hero.kicheck.ElementAt(i);
                    spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(100, 400 + i * 20), Color.White);
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                DrawScreamCircles(spriteBatch, x, y);
                spriteBatch.Draw(renderGame, Vector2.Zero, Color.White);
                spriteBatch.End();

                //----------------------------------------------------------------------
                //----------------------------------------Draw to renderScreen
                //----------------------------------------------------------------------
                Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
                Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
                if (hero.waveRolling)
                {
                    attack.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                    attack.Parameters["radius"].SetValue(hero.wavefront.X - herox);
                    attack.Parameters["heroX"].SetValue(herox - camera.viewport.X);
                    attack.Parameters["heroY"].SetValue(heroy - camera.viewport.Y);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, attack, camera.viewportTransform);
                }
                else
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                }
                spriteBatch.Draw(renderShader, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            //----------------------------------------------------------------------
            //----------------------------------------Draw to Screen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            if (paused && !cutscene)
            {
                pause.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, pause, camera.screenTransform);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            }
                spriteBatch.Draw(renderScreen, new Vector2(), Color.White);
            spriteBatch.End();
        }
    }
}
