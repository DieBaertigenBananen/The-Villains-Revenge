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
    class GameScreen
    {
        Texture2D texture;
        public Player spieler = new Player(40, 1000);
        Hero hero = new Hero(0, 0);
        Princess princess = new Princess();
        Map karte = new Map();
        Camera camera = new Camera();
        Camera pauseCamera = new Camera();
        GUI gui = new GUI();
        LoadingScreen loadingScreen = new LoadingScreen();
        ParallaxPlane foreground_1 = new ParallaxPlane("foreground_1");
        ParallaxPlane foreground_0 = new ParallaxPlane("foreground_0");
        ParallaxPlane background_0 = new ParallaxPlane("background_0");
        ParallaxPlane background_1 = new ParallaxPlane("background_1");
        ParallaxPlane background_2 = new ParallaxPlane("background_2");
        ParallaxPlane background_3 = new ParallaxPlane("background_3");
        //CloudPlane clouds_1 = new CloudPlane(1);
        //CloudPlane clouds_2 = new CloudPlane(2);
        //CloudPlane clouds_3 = new CloudPlane(3);
        RenderTarget2D renderScreen;
        RenderTarget2D renderSpine;
        RenderTarget2D renderGame;
        RenderTarget2D renderForeground0;
        RenderTarget2D renderForeground1;
        RenderTarget2D renderBackground0;
        RenderTarget2D renderBackground1;
        RenderTarget2D renderBackground2;
        RenderTarget2D renderBackground3;
        RenderTarget2D renderHud;
        RenderTarget2D renderShader;
        GaussianBlur gaussScreen;
        GaussianBlur gaussShader;
        GaussianBlur gaussBackground0;
        GaussianBlur gaussBackground1;
        GaussianBlur gaussBackground2;
        GaussianBlur gaussBackground3;
        SpriteFont font;
        bool levelend = false;
        Effect coverEyes;
        Effect smash;
        Effect dust;
        Effect clear;
        Effect pause;
        SubMenu pauseMenu;
        public static int slow = 0;
        double slowTime;
        public static Lua LuaKI = new Lua();
        public static int test = 0;
        double dietime = 0;
        public int GUIFace = 0;
        bool herohit = false;
        public bool paused = false;
        public static double spriteTimer;
        public static int spriteDelay = 120;
        public static bool changeSprite = false;

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

        public void Load(ContentManager Content, GameTime gameTime, SpriteBatch spriteBatch, int loadingState)
        {
            if (loadingState == 0)
            {
                camera.UpdateTransformation(Game1.graphics);
                loadingScreen.Load(Content);
            }
            loadingScreen.UpdateDraw(loadingState, spriteBatch, gameTime, camera);
            switch (loadingState)
            {
                case 0:
                    renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderGame = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderForeground1 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderForeground0 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderBackground0 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderBackground1 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderBackground2 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderBackground3 = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderHud = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    renderShader = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
                    font = Content.Load<SpriteFont>("fonts/schrift");
                    foreground_1.Load(Content, 5, Convert.ToInt32((double)Game1.luaInstance["planeForeground1HeightOffset"]));
                    break;
                case 1:
                    spieler.Load(Content, Game1.graphics);
                    hero.Load(Content, Game1.graphics);
                    princess.Load(Content, Game1.graphics);
                    karte.Load(Content);
                    karte.Generate(spieler, hero);
                    foreground_0.Load(Content, 5, Convert.ToInt32((double)Game1.luaInstance["planeForeground0HeightOffset"]));
                    break;
                case 2:
                    background_0.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground0"]), 0);
                    break;
                case 3:
                    background_1.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground1"]), 0);
                    break;
                case 4:
                    background_2.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground2"]), 0);
                    break;
                case 5:
                    background_3.Load(Content, Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground3"]), 0);
                    break;
                case 6:
                    //clouds_1.Load(Content, "clouds_1", karte, camera);
                    //clouds_2.Load(Content, "clouds_2", karte, camera);
                    //clouds_3.Load(Content, "clouds_3", karte, camera);
                    gui.Load(Content);
                    coverEyes = Content.Load<Effect>("CoverEyes");
                    smash = Content.Load<Effect>("Smash");
                    dust = Content.Load<Effect>("Dust");
                    clear = Content.Load<Effect>("Clear");
                    pause = Content.Load<Effect>("Pause");
                    gaussShader = new GaussianBlur(Content, Game1.graphics, 1920, 1080, 20f);
                    gaussScreen = new GaussianBlur(Content, Game1.graphics, 1920, 1080, 20f);
                    gaussBackground0 = new GaussianBlur(Content, Game1.graphics, 1920, 1080, 1f);
                    gaussBackground1 = new GaussianBlur(Content, Game1.graphics, 1920, 1080, 5f);
                    gaussBackground2 = new GaussianBlur(Content, Game1.graphics, 1920, 1080, 10f);
                    gaussBackground3 = new GaussianBlur(Content, Game1.graphics, 1920, 1080, 20f);
                    princess.ResetRage(gameTime);
                    Sound.Load(Content);
                    if (Game1.sound)
                    {
                        Sound.bgMusicInstance.Play();
                    }
                    StartSave();
                    pauseMenu = new SubMenu(2, "pause", new Vector2(- 30, -100), 120);
                    pauseMenu.Load(Content);
                    pauseMenu.buttons.Add(new Button("start", new Rectangle(0, 0, 63, 100), 4));
                    pauseMenu.buttons.Add(new Button("exit", new Rectangle(0, 200, 63, 100), 4));
                    break;
            }   
        }
        public void StartSave()
        {
            foreach (Enemy enemy in karte.enemies)
            {
                enemy.StartSave();
            }
            karte.StartSave();
            spieler.StartSave();
            hero.StartSave();
            princess.StartSave();
        }
        public void Save(int checkpointX)
        {
            foreach (Enemy enemy in karte.enemies)
            {
                if (!enemy.dead)
                    enemy.Save();
            }
            karte.Save();
            spieler.Save(checkpointX);
            hero.Save();
            princess.Save();
        }
        public void StartReset(GameTime gameTime)
        {
            Sound.Stop("ashbrett_breath");
            Sound.Stop("sweetcheeks_enrage");
            if (spieler.lifes != 0)
            {
                foreach (Enemy enemy2 in karte.enemies)
                {
                    enemy2.StartReset();
                    enemy2.Save();
                }
                karte.StartReset();
                karte.Save();
                spieler.StartReset();
                spieler.Save((int)spieler.position.X);
                hero.StartReset();
                hero.Save();
                princess.StartReset();
                princess.Save();
            }
            princess.ResetRage(gameTime);
        }
        public void Reset(GameTime gameTime)
        {
            Sound.Stop("ashbrett_breath");
            Sound.Stop("sweetcheeks_enrage");
            if (spieler.lifes != 0)
            {
                foreach (Enemy enemy2 in karte.enemies)
                {
                    enemy2.Reset();
                }
                karte.Reset();
                spieler.Reset();
                hero.Reset();
                princess.Reset();
            }
            princess.ResetRage(gameTime);
        }

        public int Update(GameTime gameTime, ContentManager Content)
        {
            if (!levelend && dietime == 0 && !paused)
            {
                Game1.time += gameTime.ElapsedGameTime;
                if (princess.rageMode)
                    GUIFace = 2;
                else
                    GUIFace = 0;
                //Update Spieler
                spieler.Update(gameTime, karte, princess);

                if (spieler.hit)
                {
                    for (int i = 0; i < karte.breakblocks.Count(); i++)
                    {
                        Breakable bblock = karte.breakblocks.ElementAt(i);
                        if (spieler.spine.BoundingBoxCollision(bblock.cbox) && bblock.vertikal)
                        {
                            Vector2 pos = new Vector2(0, 0);
                            for (int j = 0; j < bblock.blocks.Count(); j++)
                            {
                                Block block = bblock.blocks.ElementAt(j);
                                if (pos.X == 0)
                                {
                                    pos = block.position;
                                }
                                karte.blocks.Remove(block);
                                bblock.blocks.Remove(block);
                            }

                            for (int j = 0; j < karte.blocks.Count(); j++)
                            {
                                Block block = karte.blocks.ElementAt(j);
                                if (block.cbox.Intersects(bblock.cbox) && block.type == "breakable_vertikale")
                                {
                                    karte.blocks.Remove(block);
                                    bblock.blocks.Remove(block);
                                }
                            }
                            karte.objects.Add(new Debris(pos, 3));
                            karte.breakblocks.RemoveAt(i);
                        }
                    }
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
                else if (spieler.smashImpact)
                {
                    Sound.Play("superSmash");
                    int Range = Convert.ToInt32((double)Game1.luaInstance["playerMegaSchlagRange"]);
                    //Definiere SchlagRectangle
                    spieler.hitCbox = new Rectangle(spieler.cbox.box.X - Range, spieler.cbox.box.Y, spieler.cbox.box.Width + (Range * 2), spieler.cbox.box.Height);
                    spieler.hitCbox.Y += 1;
                    for (int i = 0; i < karte.breakblocks.Count(); i++)
                    {
                        Breakable bblock = karte.breakblocks.ElementAt(i);
                        if (bblock.cbox.Intersects(spieler.hitCbox) && !bblock.vertikal)
                        {
                            for (int j = 0; j < bblock.blocks.Count(); j++)
                            {
                                Block block = bblock.blocks.ElementAt(j);
                                karte.objects.Add(new Debris(block.position, 3));
                                karte.blocks.Remove(block);
                                bblock.blocks.Remove(block);
                            }

                            for (int j = 0; j < karte.blocks.Count(); j++)
                            {
                                Block block = karte.blocks.ElementAt(j);
                                if (block.cbox.Intersects(bblock.cbox)&&block.type == "breakable")
                                {
                                    karte.objects.Add(new Debris(block.position, 3));
                                    karte.blocks.Remove(block);
                                    bblock.blocks.Remove(block);
                                }
                            }
                            karte.breakblocks.RemoveAt(i);
                        }
                    }

                    for (int j = 0; j < karte.blocks.Count(); j++)
                    {
                        Block block = karte.blocks.ElementAt(j);
                        if (block.cbox.Intersects(spieler.hitCbox) && block.type == "breakable")
                        {
                            karte.objects.Add(new Debris(block.position, 3));
                            karte.blocks.Remove(block);
                        }
                    }
                    spieler.hitCbox.Y -= 1;
                }

                //--------------------Map--------------------
                karte.Update(gameTime, spieler.cbox.box, hero.cbox.box);
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
                            if (hero.slowtime < 30)
                            {
                                hero.slowtime += 10;
                            }
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
                    if (enemy.dead)
                    {
                        enemy.animeTime -= gameTime.ElapsedGameTime.TotalSeconds;
                        //if (enemy.animeTime <= 0)
                        //{
                        //    if (enemy.dead)
                        //    {
                        //        karte.enemies.Remove(enemy);
                        //    }
                        //}
                    }
                    else //Enemy lebt
                    {
                        enemy.Update(gameTime, karte, hero.position);
                        //Wenn Spieler schlägt
                        if (spieler.hit)
                        {
                            if (enemy.type == 1 && spieler.spine.BoundingBoxCollision(enemy.cbox.box)) //Töte Kanninchen
                            {
                                enemy.anim("die", 0);
                            }
                        }
                            //Megaschlag
                        else if (spieler.smash)
                        {
                            if (enemy.type == 1 && spieler.hitCbox.Intersects(enemy.cbox.box)) //Töte Kanninchen
                            {
                                enemy.anim("smash_die", 0);
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
                            if ((spieler.smash && spieler.fall) || spieler.hit)
                            {
                                //Falls Megaschlag
                                if(spieler.smash)
                                    enemy.anim("smash_die", 0);
                                else
                                    enemy.anim("die", 0);

                            }
                            else
                            {
                                //Kein Megaschlag, Spieler stirbt
                                if(spieler.position.X > enemy.position.X)
                                    enemy.anim("attack", 1);
                                else
                                    enemy.anim("attack", 2);
                                Sound.Play("fluffy_attack");
                                if (princess.bag)
                                {
                                    enemy.anim("die", 0);
                                    princess.bag = false;
                                }
                                else
                                {
                                    spieler.getHit("die");
                                    dietime = 2;
                                    GUIFace = 1;
                                    //Reset();
                                }
                            }
                        }
                        //Falls hero den Monkey erreicht
                        if (enemy.type == 2 && hero.cbox.box.Intersects(enemy.cbox.box))
                        {
                            Sound.Play("skullmonkey_dying");
                            enemy.anim("dying", 0);
                            hero.attack();
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
                        else if (item.type == "bag")//Aktiviere Sack
                        {
                            if (!princess.bag)
                            {
                                princess.bag = true;
                                spieler.spine.animationState.ClearTrack(2);
                                spieler.spine.anim("sc_bag", 0, true);
                            }
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
                            spieler.spine.anim("idle", 1, false);
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
                                hero.kistate = 9;
                            }
                            hero.kicheck.RemoveAt(0);
                        }
                    }
                }
                //--------------------Princess--------------------
                princess.Update(gameTime, spieler, karte);
                //Wenn Spieler ins nichts fällt
                if (spieler.position.Y >= karte.size.Y)
                {
                    spieler.getHit("die");
                    GUIFace = 1;
                    dietime = 1;
                    Reset(gameTime);
                }
                //Held hat den Spieler eingeholt
                if (spieler.cbox.box.Intersects(hero.cbox.box) && hero.start)
                {
                    spieler.getHit("die2");
                    GUIFace = 1;
                    dietime = 2;
                    hero.attack();
                    herohit = true;
                    Sound.Play("ashbrett_win");
                }
                //--------------------Camera--------------------
                camera.Update(Game1.graphics, spieler, karte);
                //--------------------Backgrounds--------------------
                foreground_1.Update(Content, karte, camera);
                foreground_0.Update(Content, karte, camera);
                background_0.Update(Content, karte, camera);
                background_1.Update(Content, karte, camera);
                background_2.Update(Content, karte, camera);
                background_3.Update(Content, karte, camera);
                //clouds_1.Update(karte, time, camera);
                //clouds_2.Update(karte, time, camera);
                //clouds_3.Update(karte, time, camera);
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
                coverEyes.Parameters["gameTime"].SetValue((float)Game1.time.TotalMilliseconds);
                smash.Parameters["gameTime"].SetValue((float)Game1.time.TotalMilliseconds);
                dust.Parameters["gameTime"].SetValue((float)Game1.time.TotalMilliseconds);
                dust.Parameters["playerX"].SetValue(spieler.position.X - camera.viewport.X);
                dust.Parameters["playerY"].SetValue(spieler.position.Y - camera.viewport.Y);
                dust.Parameters["force"].SetValue(spieler.acceleration / spieler.initAcceleration);
                if (Game1.input.pause)
                {
                    spieler.spine.Save();
                    princess.spine.Save();
                    hero.spine.Save();
                    for (int i = 0; i < karte.enemies.Count(); i++)
                    {
                        Enemy enemy = karte.enemies.ElementAt(i);
                        enemy.spine.Save();
                    }
                    paused = true;
                    pauseMenu.option = 0;
                }
            }
            else if (paused)
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
                princess.spine.Reset();
                hero.spine.Reset();
                for (int i = 0; i < karte.enemies.Count(); i++)
                {
                    Enemy enemy = karte.enemies.ElementAt(i);
                    enemy.spine.Reset();
                }
                camera.UpdateTransformation(Game1.graphics);
                pauseCamera.UpdateTransformation(Game1.graphics);
                return 1;
            }
            else if(levelend)
            {
                return 3;
            }
            if (dietime != 0)
            {
                Game1.time += gameTime.ElapsedGameTime;
                dietime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (dietime < 0 && spieler.lifes != 0)
                {
                    if (herohit)
                        StartReset(gameTime);
                    else
                        Reset(gameTime);
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

        void ClearRenderTarget(SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            //Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, clear); //-----[Shader]-----Clear
            spriteBatch.Draw(renderScreen, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //----------------------------------------------------------------------
            //----------------------------------------Clear renderTargets
            //----------------------------------------------------------------------
            //ClearRenderTarget(spriteBatch, renderBackground0);
            //ClearRenderTarget(spriteBatch, renderBackground1);
            //ClearRenderTarget(spriteBatch, renderBackground2);
            //ClearRenderTarget(spriteBatch, renderBackground3);
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
                //clouds_3.Draw(spriteBatch);
            spriteBatch.End();
            //Background2
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground2);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                background_2.Draw(spriteBatch, spieler); //Berge
                //clouds_2.Draw(spriteBatch);
            spriteBatch.End();
            //Background1
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground1);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                background_1.Draw(spriteBatch, spieler); //Wald
                //clouds_1.Draw(spriteBatch);
            spriteBatch.End();
            //Background0
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground0);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                background_0.Draw(spriteBatch, spieler); //Bäume
            spriteBatch.End();
            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderForeground
            //----------------------------------------------------------------------
            //Foreground0
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderForeground0);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            foreground_0.Draw(spriteBatch, spieler); //Ebene
            spriteBatch.End();
            //Foreground1
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderForeground1);
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
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, dust, camera.viewportTransform); //-----[Shader]-----Dust
                spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
            spriteBatch.End();
            if (Game1.debug) //Boundingboxen
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
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
                spriteBatch.End(); 
            }

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
                    spriteBatch.DrawString(font, "Tex" + background_1.texture[0].IsDisposed + background_1.texture[1].IsDisposed, new Vector2(Game1.resolution.X - 300, 170), Color.White);
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
                    spriteBatch.DrawString(font, "Test: " + gameTime.TotalGameTime.TotalSeconds, new Vector2(Game1.resolution.X - 300, 490), Color.White);
                    spriteBatch.DrawString(font, "Test2: " + Game1.time.TotalSeconds, new Vector2(Game1.resolution.X - 300, 520), Color.White);
                }
                gui.Draw(spriteBatch, spieler.lifes, spieler.position, hero.position, karte.size, spieler.item1, spieler.item2,GUIFace);
            spriteBatch.End();

            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderShader
            //----------------------------------------------------------------------
            //-----Apply Shaders-----
            if (Game1.debug) //-----[Shader]-----GaussianBlur
            {
                renderBackground3 = gaussBackground3.PerformGaussianBlur(Game1.graphics, spriteBatch, renderBackground3, BlendState.AlphaBlend);
                renderBackground2 = gaussBackground2.PerformGaussianBlur(Game1.graphics, spriteBatch, renderBackground2, BlendState.AlphaBlend);
                renderBackground1 = gaussBackground1.PerformGaussianBlur(Game1.graphics, spriteBatch, renderBackground1, BlendState.AlphaBlend);
                renderBackground0 = gaussBackground0.PerformGaussianBlur(Game1.graphics, spriteBatch, renderBackground0, BlendState.AlphaBlend);
            }
            //-----Background-----
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderShader);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); //-----[Shader]-----Outline
            //Background3
            spriteBatch.Draw(renderBackground3, Vector2.Zero, Color.White);
            //Background2
            spriteBatch.Draw(renderBackground2, Vector2.Zero, Color.White);
            //Background1
            spriteBatch.Draw(renderBackground1, Vector2.Zero, Color.White);
            //Background0
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
            spriteBatch.Draw(renderForeground0, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderGame, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderForeground1, Vector2.Zero, Color.White);
            spriteBatch.End();

            //-----[Shader]-----GaussianBlur
            if (princess.coverEyes)
            {
                for (int i = 0; i < 2; i++)
                {
                    renderShader = gaussShader.PerformGaussianBlur(Game1.graphics, spriteBatch, renderShader, BlendState.AlphaBlend);
                }
            }
            if (paused)
            {
                pauseMenu.Draw(spriteBatch, gameTime, pauseCamera);
            }

            //----------------------------------------------------------------------
            //----------------------------------------Draw to renderScreen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            if (princess.coverEyes && !paused) //-----[Shader]-----CoverEyes
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, coverEyes);
            }
            else if (paused)
            {
                pause.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, pause);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
            }
                spriteBatch.Draw(renderShader, new Vector2(), Color.White);
            spriteBatch.End();
            //-----HUD-----
            if (paused)
            {
                pause.Parameters["gameTime"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, pause);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
            }
                spriteBatch.Draw(renderHud, new Vector2(), Color.White);
            spriteBatch.End();

            //-----[Shader]-----GaussianBlur
            if (princess.coverEyes) //|| paused)
            {
                for (int i = 0; i < 1; i++)
                {
                    renderScreen = gaussScreen.PerformGaussianBlur(Game1.graphics, spriteBatch, renderScreen, BlendState.AlphaBlend);
                }
            }

            //----------------------------------------------------------------------
            //----------------------------------------Draw to Screen
            //----------------------------------------------------------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            if(dietime <= 0||dietime > 1)
                spriteBatch.Draw(renderScreen, new Vector2(), Color.White);
            spriteBatch.End();
        }
    }
}
