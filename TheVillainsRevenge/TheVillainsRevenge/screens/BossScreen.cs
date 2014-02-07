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
        public PrincessSpieler spieler = new PrincessSpieler(40, 1000);
        Map karte = new Map();
        Boss hero = new Boss(0, 0);
        public static Lua LuaKI = new Lua();
        Camera camera = new BossCam();
        BossGUI GUI = new BossGUI();
        Texture2D bg_texture,fg_texture;
        public static int bossleben = 100;
        int bosslebenshow = 100;
        public bool paused = false;
        Effect pause;


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



        Texture2D createCircle(int radius)
        {
            Texture2D texture = new Texture2D(Game1.graphics.GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public BossScreen()
        {
            LuaKI.RegisterFunction("getPoints", this, this.GetType().GetMethod("getPoints"));
            LuaKI.RegisterFunction("getPointID", this, this.GetType().GetMethod("getPointID"));
            LuaKI.RegisterFunction("getPointTime", this, this.GetType().GetMethod("getPointTime"));
            LuaKI.RegisterFunction("addPoint", this, this.GetType().GetMethod("addPoint"));
            LuaKI.RegisterFunction("removePoint", this, this.GetType().GetMethod("removePoint")); 
        }

        public void Load(ContentManager Content)
        {
            bg_texture = Content.Load<Texture2D>("sprites/level_5/planes/background");
            fg_texture = Content.Load<Texture2D>("sprites/level_5/planes/foreground");
            pause = Content.Load<Effect>("Pause");
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
                            bossleben -= 10;
                            hero.gethit();
                        }
                    }
                    else if (spieler.smash)
                    {            
                        int x = spieler.cbox.box.X + (spieler.cbox.box.Width/2);
                        int y = spieler.cbox.box.Y + (spieler.cbox.box.Height/2);
                        if (Circle.Intersects(new Vector2(x, y), spieler.screamradius, hero.cbox.box)&&hero.screamhit)
                        {
                            bossleben -= 30;
                            hero.gethit();
                            hero.screamhit = false;
                        }
                    }
                    else if(!hero.screamhit)
                        hero.screamhit = true;
                }

                //--------------------Hero--------------------
                hero.Update(gameTime, karte, spieler.cbox.box);
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
                spieler.spine.Reset();
                hero.spine.Reset();
                if (Game1.input.pause)
                {
                    paused = false;
                }
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
            GUI.Draw(spriteBatch, spieler.lifes, bosslebenshow);
            spriteBatch.End();

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
