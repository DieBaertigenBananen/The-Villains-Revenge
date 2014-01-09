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
using LuaInterface;

namespace TheVillainsRevenge
{
    class GameScreen
    {
        public static int test;
        Texture2D texture;
        Texture2D debug;
        Player spieler = new Player(40, 1000);
        Hero hero = new Hero(0, 0);
        Map karte = new Map();
        Camera camera = new Camera();
        GUI gui = new GUI();
        ParallaxPlane background_0 = new ParallaxPlane();
        ParallaxPlane background_1 = new ParallaxPlane();
        ParallaxPlane background_2 = new ParallaxPlane();
        ParallaxPlane background_3 = new ParallaxPlane();
        CloudPlane clouds_1 = new CloudPlane(1);
        CloudPlane clouds_2 = new CloudPlane(2);
        CloudPlane clouds_3 = new CloudPlane(3);
        Sound bgmusic = new Sound("sounds/Level_1/background");
        RenderTarget2D renderTarget;
        RenderTarget2D renderSpine;
        SpriteFont font;
        bool levelend = false;
        Effect coverEyes;
        public static int slow = 0;
        double slowTime;
        public static Lua LuaKI = new Lua();

        //KIDaten
        public int getPoints(string w)
        {
            if (w == "Spieler")
                return spieler.kicheck.Count()-1;
            else
                return hero.kicheck.Count()-1;
        }
        public int getPointID(int s,string w)
        {
            if (w == "Spieler")
                return spieler.kicheck.ElementAt(s).id;
            else
                return hero.kicheck.ElementAt(s).id;
        }
        public int getPointTime(int s,string w)
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
            if(geht)
                hero.kicheck.Add(new KICheck(t, s));
        }

        public GameScreen()
        {
            texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
            LuaKI.RegisterFunction("getPoints", this, this.GetType().GetMethod("getPoints"));
            LuaKI.RegisterFunction("getPointID", this, this.GetType().GetMethod("getPointID"));
            LuaKI.RegisterFunction("getPointTime", this, this.GetType().GetMethod("getPointTime"));
            LuaKI.RegisterFunction("addPoint", this, this.GetType().GetMethod("addPoint"));
            LuaKI.RegisterFunction("removePoint", this, this.GetType().GetMethod("removePoint")); 
        }

        public void Load(ContentManager Content)
        {
            renderTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            font = Content.Load<SpriteFont>("fonts/schrift");
            spieler.Load(Content, Game1.graphics);
            hero.Load(Content, Game1.graphics);
            karte.Load(Content);
            karte.Generate();
            background_0.Load(Content, "background_0", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground0"]));
            background_1.Load(Content, "background_1", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground1"]));
            background_2.Load(Content, "background_2", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground2"]));
            background_3.Load(Content, "background_3", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground3"]));
            clouds_1.Load(Content, "clouds_1", karte, camera);
            clouds_2.Load(Content, "clouds_2", karte, camera);
            clouds_3.Load(Content, "clouds_3", karte, camera);
            gui.Load(Content);
            coverEyes = Content.Load<Effect>("CoverEyes");
            if (Game1.sound)
            {
                bgmusic.Load(Content);
            }
            debug = Content.Load<Texture2D>("sprites/Level_1/Planes/background_0_debug");
        }
        public void Save(int x)
        {
            spieler.Save(x);
            hero.Save();
            foreach (Enemy enemy in karte.enemies)
            {
                enemy.Save();
            }
            foreach (Trigger trigger in karte.triggers)
            {
                trigger.Save();
            }
            foreach (MovingBlock mblock in karte.mblocks)
            {
                mblock.Save();
            }
        }
        public void Reset()
        {
            if (spieler.lifes != 0)
            {
                spieler.Reset();
                hero.Reset();
                foreach (Enemy enemy2 in karte.enemies)
                {
                    enemy2.Reset();
                }
                foreach (Trigger trigger in karte.triggers)
                {
                    trigger.Reset(karte.blocks);
                }
                foreach (MovingBlock mblock in karte.mblocks)
                {
                    mblock.Reset();
                }
            }
        }

        public int Update(GameTime gameTime)
        {
            if (!levelend)
            {
                karte.Update(gameTime, spieler.cbox.box);
                for (int i = 0; i < karte.objects.Count(); i++)
                {
                    Obj obj = karte.objects.ElementAt(i);
                    obj.Update(gameTime, karte);
                    if (obj.box.Intersects(hero.cbox.box))
                    {
                        hero.slowtime += 10;
                        karte.objects.Remove(obj);
                    }
                    else if (obj.type == 2)
                    {
                        foreach (Block block in karte.blocks)
                        {
                            if (obj.box.Intersects(block.cbox))
                            {
                                karte.objects.Remove(obj);
                                break;
                            }
                        }
                    }
                }
                foreach (Enemy enemy in karte.enemies)
                {
                    enemy.Update(gameTime, karte,hero.position);
                    if (enemy.position.X < -enemy.cbox.box.Width || enemy.position.Y < -enemy.cbox.box.Height || enemy.position.X > karte.size.X || enemy.position.Y > karte.size.Y)
                    {
                        karte.enemies.Remove(enemy);
                        break;
                    }
                    if (spieler.cbox.box.Intersects(enemy.cbox.box)&&enemy.type == 1)
                    {
                        spieler.getHit();
                        Reset();
                        break;
                    }
                    if(enemy.type == 2&&hero.cbox.box.Intersects(enemy.cbox.box))
                    {
                        karte.enemies.Remove(enemy);
                        break;
                    }
                }
                foreach (Item item in karte.items)
                {
                    if (spieler.cbox.box.Intersects(item.cbox))
                    {
                        if (item.type == "herz")
                        {
                            if (spieler.lifes != 4)
                                spieler.lifes++;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "zeit")
                        {
                            if (spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 1;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "banana")
                        {
                            if (spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 2;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "monkey")
                        {
                            if (spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 3;
                            karte.items.Remove(item);
                        }
                        break;
                    }
                }
                foreach (Checkpoint cpoint in karte.checkpoints)
                {
                    if (spieler.position.X > cpoint.x && spieler.checkpoint.X < cpoint.x)
                    {
                        //TODO: Speichern aller dynamischen Objekte in der Welt um diesen Zustand bei zurücksetzen an Checkpoint exakt zu rekonstruieren.
                        if (cpoint.end)
                        {
                            spieler.spine.anim("idle", 0,false);
                            levelend = true;
                        }
                        else
                        {
                            Save(cpoint.x);
                        }
                        break;
                    }
                }
                foreach (Trigger trigger in karte.triggers)
                {
                    if (spieler.cbox.box.Intersects(trigger.cbox) && spieler.fall)
                    {
                        trigger.Pushed(karte.blocks);
                        break;
                    }
                }
                hero.Update(gameTime, karte, spieler.cbox.box);    
                for (int i = 0; i <karte.kipoints.Count(); i++)
                {
                    KIPoint kipoint = karte.kipoints.ElementAt(i);
                    if (spieler.cbox.box.Intersects(kipoint.cbox))
                    {
                        bool geht = true;
                        if (spieler.kicheck.Count() > 0)
                        {
                            KICheck check = spieler.kicheck.ElementAt(spieler.kicheck.Count()-1);
                            if(check.id == kipoint.id)
                                geht = false;
                        }
                        if (geht)
                        {
                            if (spieler.kicheck.Count() >= 20)
                            {
                                spieler.kicheck.RemoveAt(0);
                            }
                            spieler.kicheck.Add(new KICheck((int)gameTime.TotalGameTime.TotalSeconds, kipoint.id));
                            LuaKI.DoFile("kiscript.txt");
                        }
                    }
                    if (hero.cbox.box.Intersects(kipoint.cbox)&&hero.kicheck.Count() != 0)
                    {
                        if (hero.kicheck.ElementAt(0).id == kipoint.id)
                        {
                            hero.kicheck.RemoveAt(0);
                        }
                    }
                }
                if(!levelend)
                    spieler.Update(gameTime, karte);
                if (spieler.position.Y >= (karte.size.Y))
                {
                    spieler.getHit();
                    Reset();
                }
                if (spieler.cbox.box.Intersects(hero.cbox.box))
                {
                    spieler.lifes = 0;
                }
                camera.Update(Game1.graphics, spieler, karte);
                background_0.Update(karte, camera);
                background_1.Update(karte, camera);
                background_2.Update(karte, camera);
                background_3.Update(karte, camera);
                clouds_1.Update(karte, gameTime, camera);
                clouds_2.Update(karte, gameTime, camera);
                clouds_3.Update(karte, gameTime, camera);
                if (slow != 0)
                {
                    slowTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (slowTime > slow)
                    {
                        slowTime = 0;
                        slow = 0;
                    }
                }
            }
            if (spieler.lifes != 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //--------------------Draw to Spine--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spieler.Draw(gameTime, camera);
            hero.Draw(gameTime, camera);


            //--------------------Draw to Texture--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);

            //-----Hintergrundebenen-----
            //if (!Game1.debug)
            //{
                background_3.Draw(spriteBatch); //Himmel
                clouds_3.Draw(spriteBatch);
                background_2.Draw(spriteBatch); //Berge
                clouds_2.Draw(spriteBatch);
                background_1.Draw(spriteBatch); //Wald
                clouds_1.Draw(spriteBatch);
                if (!Game1.debug)
                {
                    background_0.Draw(spriteBatch); //Bäume
                }
                else
                {
                    spriteBatch.Draw(debug, new Vector2(background_0.position.X, background_0.position.Y), Color.White);
                }
            //}

            //-----Spielebene-----
            karte.Draw(spriteBatch); //Enthält eine zusätzliche Backgroundebene
            hero.Draw(gameTime,camera); //Ashbrett
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
            if (Game1.debug) //Boundingbox Bonepuker
            {
                spriteBatch.Draw(texture, spieler.cbox.box, null, Color.White);
                spriteBatch.Draw(texture, hero.cbox.box, null, Color.White);
                spriteBatch.Draw(texture, hero.kicollide, null, Color.Red);
            }
            spriteBatch.End();


            //--------------------Draw to Screen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            
            //-----renderTarget-----
            coverEyes.Parameters["playerX"].SetValue((spieler.position.X - camera.viewport.X) / camera.viewport.Width);
            coverEyes.Parameters["playerY"].SetValue((spieler.position.Y - camera.viewport.Y) / camera.viewport.Height);
            coverEyes.Parameters["gameTime"].SetValue(gameTime.TotalGameTime.Milliseconds);
            coverEyes.Parameters["left"].SetValue(spieler.spine.skeleton.FlipX);
            if (spieler.coverEyes)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, coverEyes, camera.screenTransform);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            }
            spriteBatch.Draw(renderTarget, new Vector2(), Color.White);
            spriteBatch.End();

            //-----HUD-----
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            if (levelend)
            {
                spriteBatch.DrawString(font, "Finished level!", new Vector2((Game1.resolution.X /2)-200, (Game1.resolution.Y / 2) -200), Color.Black,0.0f,Vector2.Zero,4.0f,SpriteEffects.None,0f);
            }
            if (Game1.debug)
            {
                spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(Game1.resolution.X - 300, 90), Color.White);
                spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(Game1.resolution.X - 300, 110), Color.White);
                spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(Game1.resolution.X - 300, 130), Color.White);
                spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(Game1.resolution.X - 300, 150), Color.White);
                spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(Game1.resolution.X - 300, 170), Color.White);
                spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(Game1.resolution.X - 300, 190), Color.White);
                spriteBatch.DrawString(font, "Hero: " + (hero.position.X + " " + hero.position.Y), new Vector2(Game1.resolution.X - 300, 210), Color.White);
                spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y + " " + camera.viewport.Width + " " + camera.viewport.Height), new Vector2(Game1.resolution.X - 300, 230), Color.White);
                spriteBatch.DrawString(font, "Skeleton: " + (spieler.spine.skeleton.X + " " + spieler.spine.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                spriteBatch.DrawString(font, "Planes.Size.X: " + background_1.size.X + " " + background_2.size.X + " " + background_3.size.X + " " + clouds_1.size.X + " " + clouds_2.size.X + " " + clouds_3.size.X + " " + background_0.size.X, new Vector2(Game1.resolution.X - 700, 270), Color.White);
                spriteBatch.DrawString(font, "Skeleton: " + (spieler.spine.skeleton.X + " " + spieler.spine.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                spriteBatch.DrawString(font, "Planes.Size.X: " + background_0.size.X + " " + background_1.size.X + " " + background_2.size.X + " " + background_3.size.X, new Vector2(Game1.resolution.X - 700, 270), Color.White);
                //spriteBatch.DrawString(font, "Kollision: " + spieler.check, new Vector2(Game1.resolution.X - 300, 290), Color.White);
                Slot bb = spieler.spine.skeleton.FindSlot("bonepuker");
                spriteBatch.DrawString(font, "bb-bonepuker: " + spieler.spine.bounds.BoundingBoxes.FirstOrDefault(), new Vector2(Game1.resolution.X - 300, 310), Color.White);
                spriteBatch.DrawString(font, "SlowTime: " + slow + " Vergangen: " + slowTime, new Vector2(Game1.resolution.X - 300, 330), Color.White);
                spriteBatch.DrawString(font, "KIState: " + hero.kistate, new Vector2(Game1.resolution.X - 300, 350), Color.White);
                spriteBatch.DrawString(font, "TEST: " + hero.slowtime, new Vector2(Game1.resolution.X - 300, 370), Color.White);
                for (int i = 0; i < spieler.kicheck.Count(); i++)
                {
                    KICheck kicheck = spieler.kicheck.ElementAt(i);
                    spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: "+kicheck.time, new Vector2(Game1.resolution.X - 300, 390+i*20), Color.White);
                }
                for (int i = 0; i < hero.kicheck.Count(); i++)
                {
                    KICheck kicheck = hero.kicheck.ElementAt(i);
                    spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(Game1.resolution.X - 450, 390 + i * 20), Color.White);
                }
                //for (int i = 0; i < karte.background.Width; i++)
                //{
                //    for (int t = 15; t < karte.background.Height; t++)
                //    {
                //        spriteBatch.DrawString(font, karte.pixelRGBA[i, t, 0] + "," + karte.pixelRGBA[i, t, 1] + "," + karte.pixelRGBA[i, t, 2], new Vector2(i * 60, (t - 15) * 30), Color.Black);
                //    }
                //}*/
            }
            gui.Draw(spriteBatch, spieler.lifes, spieler.position, hero.position, karte.size, spieler.item1, spieler.item2);
            spriteBatch.End();
        }
    }
}
