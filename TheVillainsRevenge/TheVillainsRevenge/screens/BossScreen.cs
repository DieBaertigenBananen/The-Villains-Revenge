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
        int bossleben = 100;
        int bosslebenshow = 100;
        bool bosshit = false;


        RenderTarget2D renderSpine;
        RenderTarget2D renderGame;

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
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderGame = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            spieler.Load(Content, Game1.graphics);
            hero.Load(Content, Game1.graphics);
            hero.start = true;
            karte.Load(Content);
            GUI.Load(Content);
            karte.Generate(spieler, hero);
            Sound.Load(Content);
            if (Game1.sound)
            {
                Sound.PlayBG();
            }
        }

        public int Update(GameTime gameTime, ContentManager Content)
        {

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
                        spieler.getHit(gameTime, "die2");
                        karte.objects.Remove(obj);
                    }
                    else if (obj.position.X < 0 || obj.position.X > karte.size.X)
                        karte.objects.Remove(obj);
                }
            }
            //--------------------Spieler--------------------
            spieler.Update(gameTime, karte, hero.cbox.box);
            //if (spieler.position.Y <= 0)
            //{
            //    spieler.jump = false;
            //    spieler.fall = true;
            //    spieler.falltimer = gameTime.TotalGameTime.TotalMilliseconds;
            //}
            //if (spieler.position.X < 0)
            //{
            //    spieler.Move((int)(-spieler.position.X), 0, karte);
            //}
            //if (spieler.position.X > karte.size.X)
            //{
            //    spieler.Move((int)-(spieler.position.X-karte.size.X), 0, karte);
            //} wurde alles in collisioncheckedvector integriert.

            if (bosslebenshow != bossleben)
                bosslebenshow--;
            else
            {
                if (!bosshit&&spieler.hit&&hero.inactiveTime > 0)
                {
                    if (spieler.spine.BoundingBoxCollision(hero.cbox.box))
                    {
                        bossleben -= 10;
                        bosshit = true;
                    }
                }
                else if (!spieler.hit&&bosshit)
                {
                    bosshit = false;
                }
            }

            //--------------------Hero--------------------
            hero.Update(gameTime, karte, spieler.cbox.box);

            if (spieler.cbox.box.Intersects(hero.cbox.box) && hero.start&&hero.attacktimer <= 0&&hero.inactiveTime <= 0)
            {
                hero.attack(gameTime);
            }
            else if (spieler.cbox.box.Intersects(hero.cbox.box) && hero.start&&hero.attacktimer <= 0 && hero.inactiveTime >= 0.3f)
            {
                spieler.getHit(gameTime, "die2");
            }
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
                        spieler.kicheck.Add(new KICheck((int)gameTime.TotalGameTime.TotalSeconds, kipoint.id));
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
            return 1;
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
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
            GUI.Draw(spriteBatch, spieler.lifes, bosslebenshow);
            spriteBatch.End();

            //----------------------------------------------------------------------
            //----------------------------------------Draw to Screen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            spriteBatch.Draw(bg_texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(fg_texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderGame, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
