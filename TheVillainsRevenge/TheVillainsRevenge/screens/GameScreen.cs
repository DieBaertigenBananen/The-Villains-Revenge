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
        Texture2D texture;
        public static Texture2D debug;
        Player spieler = new Player(40, 1000);
        Hero hero = new Hero(0, 0);
        Princess princess = new Princess();
        Map karte = new Map();
        Camera camera = new Camera();
        GUI gui = new GUI();
        ParallaxPlane foreground_1 = new ParallaxPlane("foreground_1");
        ParallaxPlane foreground_0 = new ParallaxPlane("foreground_0");
        ParallaxPlane background_0 = new ParallaxPlane("background_0");
        ParallaxPlane background_1 = new ParallaxPlane("background_1");
        ParallaxPlane background_2 = new ParallaxPlane("background_2");
        ParallaxPlane background_3 = new ParallaxPlane("background_3");
        CloudPlane clouds_1 = new CloudPlane(1);
        CloudPlane clouds_2 = new CloudPlane(2);
        CloudPlane clouds_3 = new CloudPlane(3);
        RenderTarget2D renderScreen;
        RenderTarget2D renderSpine;
        RenderTarget2D renderGame;
        RenderTarget2D renderForeground_0;
        RenderTarget2D renderForeground_1;
        RenderTarget2D renderBackground0;
        RenderTarget2D renderBackground1;
        RenderTarget2D renderBackground2;
        RenderTarget2D renderBackground3;
        RenderTarget2D renderHud;
        SpriteFont font;
        bool levelend = false;
        Effect coverEyes;
        Effect outline;
        Effect smash;
        public static int slow = 0;
        double slowTime;
        public static Lua LuaKI = new Lua();
        public static int test = 0;
        double dietime = 0;
        bool herohit = false;

        //KIDaten
        //Dies sind Luafunktionen für den netten GD
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
            renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderGame = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderForeground_1 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderForeground_0 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderBackground0 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderBackground1 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderBackground2 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderBackground3 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderHud = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            font = Content.Load<SpriteFont>("fonts/schrift");
            spieler.Load(Content, Game1.graphics);
            hero.Load(Content, Game1.graphics);
            princess.Load(Content, Game1.graphics);
            karte.Load(Content);
            karte.Generate(spieler,hero);
            foreground_1.Load(Content, 5, Convert.ToInt32((double)Game1.luaInstance["planeForeground1HeightOffset"]));
            foreground_0.Load(Content, 5, Convert.ToInt32((double)Game1.luaInstance["planeForeground0HeightOffset"]));
            background_0.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground0"]), 0);
            background_1.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground1"]), 0);
            background_2.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground2"]), 0);
            background_3.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground3"]), 0);
            clouds_1.Load(Content, "clouds_1", karte, camera);
            clouds_2.Load(Content, "clouds_2", karte, camera);
            clouds_3.Load(Content, "clouds_3", karte, camera);
            gui.Load(Content);
            coverEyes = Content.Load<Effect>("CoverEyes");
            outline = Content.Load<Effect>("Outline");
            smash = Content.Load<Effect>("Smash");
            if (Game1.sound)
            {
                Sound.Load(Content);
                Sound.PlayBG();
            }
            debug = Content.Load<Texture2D>("sprites/Level_"+Game1.level+"/Planes/background_0_debug");
            StartSave();
        }
        public void StartSave()
        {
            spieler.StartSave();
            hero.StartSave();
            princess.StartSave();
            foreach (Enemy enemy in karte.enemies)
            {
                enemy.StartSave();
            }
            karte.StartSave();
        }
        public void Save(int checkpointX)
        {
            spieler.Save(checkpointX);
            hero.Save();
            princess.Save();
            foreach (Enemy enemy in karte.enemies)
            {
                enemy.Save();
            }
            karte.Save();
        }
        public void StartReset()
        {
            if (spieler.lifes != 0)
            {
                spieler.StartReset();
                spieler.Save((int)spieler.position.X);
                hero.StartReset();
                hero.Save();
                princess.StartReset();
                princess.Save();
                foreach (Enemy enemy2 in karte.enemies)
                {
                    enemy2.StartReset();
                    enemy2.Save();
                }
                karte.StartReset();
                karte.Save();
            }
        }
        public void Reset()
        {
            if (spieler.lifes != 0)
            {
                spieler.Reset();
                hero.Reset();
                princess.Reset();
                foreach (Enemy enemy2 in karte.enemies)
                {
                    enemy2.Reset();
                }
                karte.Reset();
            }
        }

        public int Update(GameTime gameTime, ContentManager Content)
        {
            if (!levelend && dietime == 0)
            {
                //Update Spieler
                spieler.Update(gameTime, karte, princess);
                if(spieler.hit)
                {
                    //if (spieler.richtung)
                    //{
                    //    schlagRECT.X = schlagRECT.X + schlagRECT.Width;
                    //    schlagRECT.Width = Convert.ToInt32((double)Game1.luaInstance["playerSchlagRange"]);
                    //}
                    //else
                    //{
                    //    schlagRECT.Width = Convert.ToInt32((double)Game1.luaInstance["playerSchlagRange"]);
                    //    schlagRECT.X = schlagRECT.X - schlagRECT.Width;
                    //}
                    for (int i = 0; i < karte.blocks.Count(); i++)
                    {
                        Block block = karte.blocks.ElementAt(i);
                        if (block.type == "breakable_verticale" && spieler.spine.BoundingBoxCollision(block.cbox))
                        {
                            karte.objects.Add(new Debris(block.position, 3));
                            karte.blocks.Remove(block);
                        }
                    }
                }
                else if(spieler.smashImpact)
                {
                    Sound.Play("superSmash");
                    int Range = Convert.ToInt32((double)Game1.luaInstance["playerMegaSchlagRange"]);
                    //Definiere SchlagRectangle
                    spieler.hitCbox = new Rectangle(spieler.cbox.box.X - Range, spieler.cbox.box.Y, spieler.cbox.box.Width + (Range * 2), spieler.cbox.box.Height);
                    spieler.hitCbox.Y += 1;
                    for (int i = 0; i < karte.blocks.Count(); i++)
                    {
                        Block block = karte.blocks.ElementAt(i);
                        if (block.cbox.Intersects(spieler.hitCbox) && block.type == "breakable")
                        {
                            Rectangle nextblock = block.cbox;
                            nextblock.X -= 48;
                            nextblock.Width = nextblock.Width * 3;
                            for (int j = 0; j < karte.blocks.Count(); j++)
                            {
                                Block block2 = karte.blocks.ElementAt(j);
                                if (block2.cbox.Intersects(nextblock) && block2.type == "breakable")
                                {
                                    karte.objects.Add(new Debris(block2.position, 3));
                                    karte.blocks.Remove(block2);
                                }
                            }
                        }
                    }
                    spieler.hitCbox.Y -= 1;
                }

                //--------------------Map--------------------
                karte.Update(gameTime, spieler.cbox.box,hero.cbox.box);
                //Objekte updaten
                for (int i = 0; i < karte.objects.Count(); i++)
                {
                    Obj obj = karte.objects.ElementAt(i);
                    obj.Update(gameTime, karte);
                    if (obj.box.Intersects(hero.cbox.box))
                    {
                        if (obj.type != 3) //Obj
                        {
                            //Banane oder Kacke, verlangsame und entferne sich selber
                            if (obj.type == 1)
                                Sound.Play("ausrutscher");
                            hero.slowtime += 10;
                            karte.objects.Remove(obj);
                        }
                        else
                        { //obj ist geröll, verlangsame den Held ohne entfernt zu werden
                            hero.slowtime += 0.1;
                        }
                    }
                    else if (obj.type == 2) // Kacke
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
                    else if (obj.type == 3 && obj.fall) //Geröll/Debris
                    {
                        foreach (Block block in karte.blocks)
                        {
                            if (obj.box.Intersects(block.cbox))
                            {
                                obj.fall = false;
                                break;
                            }
                        }
                    }
                }
                //Update Enemies
                for (int i = 0; i < karte.enemies.Count(); i++)
                {
                    Enemy enemy = karte.enemies.ElementAt(i);
                    //Wenn Enemy tot ist update die Animation
                    if (enemy.animeTime > 0)
                    {
                        enemy.animeTime -= gameTime.ElapsedGameTime.TotalSeconds;
                        if (enemy.animeTime <= 0)
                        {
                            if(enemy.dead)
                                karte.enemies.Remove(enemy);
                        }
                    }
                    else //Enemy lebt
                    {
                        enemy.Update(gameTime, karte, hero.position);
                        //Wenn Spieler schlägt
                        if (spieler.hit)
                        {
                            if (enemy.type == 1 && spieler.spine.BoundingBoxCollision(enemy.cbox.box)) //Töte Kanninchen
                            {
                                enemy.anim(gameTime, "die",0);
                            }
                        }
                            //Megaschlag
                        else if (spieler.smash)
                        {
                            if (enemy.type == 1 && spieler.hitCbox.Intersects(enemy.cbox.box)) //Töte Kanninchen
                            {
                                enemy.anim(gameTime, "die",0);
                            }
                        }
                        //Wenn Enemy aus der Map läuft
                        if (enemy.position.X < -enemy.cbox.box.Width || enemy.position.Y < -enemy.cbox.box.Height || enemy.position.X > karte.size.X || enemy.position.Y > karte.size.Y)
                        {
                            karte.enemies.Remove(enemy);
                        }
                        //Wenn Spieler von enemy getroffen wird
                        if (enemy.type == 1 && spieler.cbox.box.Intersects(enemy.cbox.box))
                        {
                            if (spieler.smash && spieler.fall)
                            {
                                //Falls Megaschlag
                                enemy.anim(gameTime,"die",0);
                            }
                            else
                            {
                                //Kein Megaschlag, Spieler stirbt
                                if(spieler.position.X > enemy.position.X)
                                    enemy.anim(gameTime, "attack", 1);
                                else
                                    enemy.anim(gameTime, "attack", 2);
                                spieler.getHit(gameTime);
                                dietime = 2;
                                //Reset();
                            }
                        }
                        //Falls hero den Monkey erreicht
                        if (enemy.type == 2 && hero.cbox.box.Intersects(enemy.cbox.box))
                        {
                            enemy.anim(gameTime,"dying",0);
                            hero.attack(gameTime);
                        }
                    }
                }
                //Update Items
                foreach (Item item in karte.items)
                {
                    if (spieler.cbox.box.Intersects(item.cbox))
                    {
                        if (item.type == "herz") //Wird sofort ausgeführt
                        {
                            if (spieler.lifes != 4)
                                spieler.lifes++;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "zeit") //Setze in Slot
                        {
                            if (spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 1;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "banana")//Setze in Slot
                        {
                            if (spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 2;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "monkey")//Setze in Slot
                        {
                            if (spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 3;
                            karte.items.Remove(item);
                        }
                        break;
                    }
                }
                //Update Checkpoints
                foreach (Checkpoint cpoint in karte.checkpoints)
                {
                    if (spieler.position.X > cpoint.x && spieler.checkpoint.X < cpoint.x)
                    {
                        if (cpoint.end)
                        {
                            spieler.spine.anim("idle", 1, false, gameTime);
                            levelend = true;
                        }
                        else
                        {
                            Save(cpoint.x);
                        }
                        break;
                    }
                }
                //Update Trigger
                foreach (Trigger trigger in karte.triggers)
                {
                    if (spieler.cbox.box.Intersects(trigger.cbox) && spieler.fall)
                    {
                        Sound.Play("button");
                        trigger.Pushed(karte.blocks,hero.cbox.box);
                        break;
                    }
                }
                //--------------------Hero--------------------
                hero.Update(gameTime, karte, spieler.cbox.box);
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
                            hero.kicheck.RemoveAt(0);
                        }
                    }
                }
                //--------------------Princess--------------------
                princess.Update(gameTime, spieler, karte);
                //Wenn Spieler über den Maprand tritt (zu tief fällt)
                if (spieler.position.Y >= karte.size.Y)
                {
                    spieler.getHit(gameTime);
                    dietime = 1;
                    Reset();
                }
                else if (spieler.position.Y <= 0)
                {
                    spieler.jump = false;
                    spieler.fall = true;
                    spieler.falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                //Held hat den Spieler eingeholt
                if (spieler.cbox.box.Intersects(hero.cbox.box) && hero.start)
                {
                    spieler.getHit(gameTime);
                    dietime = 2;
                    hero.attack(gameTime);
                    herohit = true;
                }
                //--------------------Camera--------------------
                camera.Update(Game1.graphics, spieler, karte);
                //--------------------Backgrounds--------------------
                foreground_1.Update(karte, camera);
                foreground_0.Update(karte, camera);
                background_0.Update(karte, camera);
                background_1.Update(karte, camera);
                background_2.Update(karte, camera);
                background_3.Update(karte, camera);
                clouds_1.Update(karte, gameTime, camera);
                clouds_2.Update(karte, gameTime, camera);
                clouds_3.Update(karte, gameTime, camera);
                //--------------------SlowTimer--------------------
                if (slow != 0)
                {
                    slowTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (slowTime > slow)
                    {
                        slowTime = 0;
                        slow = 0;
                    }
                }
                //-----Update Shader-----
                coverEyes.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                outline.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                smash.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
            }
            else if(levelend)
            {
                return 3;
            }
            if (dietime != 0)
            {
                dietime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (dietime < 0 && spieler.lifes != 0)
                {
                    if (herohit)
                        StartReset();
                    else
                        Reset();
                    dietime = 0;
                    return 1;
                }
                else if (dietime < 0)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
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
            for (int i = 0; i < karte.enemies.Count(); ++i)
            {
                Enemy enemy = karte.enemies.ElementAt(i);
                enemy.Draw(spriteBatch, gameTime, camera);
            }
            if (princess.beating)
            {
                princess.Draw(gameTime, camera);
            }
            else
            {
                spieler.Draw(gameTime, camera);
            }
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
                karte.Draw(spriteBatch,gameTime,camera); //Plattformen & Co
                //hero.Draw(gameTime,camera); //Ashbrett
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
                    for (int i = 0; i < karte.triggers.Count(); i++)
                    {
                        Trigger trigger = karte.triggers.ElementAt(i);
                        Rectangle c = new Rectangle(trigger.doorstart.X, trigger.doorstart.Y + 46, trigger.doorstart.Width, trigger.doorstart.Height);
                        spriteBatch.Draw(texture, c, null, Color.Blue);
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
                    spriteBatch.DrawString(font, spieler.acceleration + " " + spieler.spine.animation, new Vector2(Game1.resolution.X - 300, 170), Color.White);
                    spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(Game1.resolution.X - 300, 190), Color.White);
                    spriteBatch.DrawString(font, "Hero: " + (hero.position.X + " " + hero.position.Y), new Vector2(Game1.resolution.X - 300, 210), Color.White);
                    spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y + " " + camera.viewport.Width + " " + camera.viewport.Height), new Vector2(Game1.resolution.X - 300, 230), Color.White);
                    spriteBatch.DrawString(font, "Skeleton: " + (spieler.spine.skeleton.X + " " + spieler.spine.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                    spriteBatch.DrawString(font, "RageMeter: " + princess.rageMeter, new Vector2(Game1.resolution.X - 300, 290), Color.White);
                    Slot bb = spieler.spine.skeleton.FindSlot("bonepuker");
                    spriteBatch.DrawString(font, "bb-bonepuker: " + spieler.spine.bounds.BoundingBoxes.FirstOrDefault(), new Vector2(Game1.resolution.X - 300, 310), Color.White);
                    spriteBatch.DrawString(font, "SlowTime: " + slow + " Vergangen: " + slowTime, new Vector2(Game1.resolution.X - 300, 330), Color.White);
                    spriteBatch.DrawString(font, "KIState: " + hero.kistate, new Vector2(Game1.resolution.X - 300, 350), Color.White);
                    spriteBatch.DrawString(font, "SmashCooldown: " + spieler.smashCooldown, new Vector2(Game1.resolution.X - 300, 370), Color.White);
                    spriteBatch.DrawString(font, "CEyes: " + princess.coverEyes + " Beat: " + princess.beating + " RageMode: " + princess.rageMode, new Vector2(Game1.resolution.X - 450, 390), Color.White);
                    if (spieler.hit)
                    {
                        spriteBatch.DrawString(font, "SCHLAG", new Vector2(Game1.resolution.X - 500, 350), Color.White);
                    } else if (spieler.smash)
                    {
                        spriteBatch.DrawString(font, "MEGAAA SMAAASH", new Vector2(Game1.resolution.X - 500, 350), Color.White);
                    }
                    for (int i = 0; i < spieler.kicheck.Count(); i++)
                    {
                        KICheck kicheck = spieler.kicheck.ElementAt(i);
                        spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(10, 100 + i * 20), Color.White);
                    }
                    for (int i = 0; i < hero.kicheck.Count(); i++)
                    {
                        KICheck kicheck = hero.kicheck.ElementAt(i);
                        spriteBatch.DrawString(font, "ID: " + kicheck.id + " Time: " + kicheck.time, new Vector2(100, 100 + i * 20), Color.White);
                    }
                    spriteBatch.DrawString(font, "Test: " + test, new Vector2(Game1.resolution.X - 300, 490), Color.White);
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
            if (princess.beating || spieler.smash || spieler.smashIntensity > 0) //-----[Shader]-----Smash
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, smash);
                if (spieler.smashIntensity >= 0)
                {
                    smash.Parameters["intensity"].SetValue(spieler.smashIntensity);
                }
                else
                {
                    smash.Parameters["intensity"].SetValue(20); //SmashIntensity für Princess.Beating
                }
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
            if (dietime < 1 && dietime > 0&&spieler.lifes != 0)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);

                spriteBatch.End();
            }
            else
            {
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
            }
            
        }
    }
}
