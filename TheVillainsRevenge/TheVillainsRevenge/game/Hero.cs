using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TheVillainsRevenge
{
    class Hero
    {
        public Vector2 position; //Position
        public CollisionBox cbox;
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        public bool fall = true;
        public double falltimer;
        public double jumptimer;
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int jumppower; //Anfangsgeschwindigkeit in m/s _/60
        public double herotime;
        public bool start = false;
        public int heroStartTime;
        public Spine spine;
        public int kistate; //State der KI Berechnung
        public Rectangle kicollide;
        public List<KICheck> kicheck = new List<KICheck>(); //Erstelle Blocks als List
        public double slowtime;
        float acceleration = 0.0f;
        public double attacktimer = 0;
        double breathtimer = 0;
        //Checkpoint//
        public List<KICheck> kicheckcp = new List<KICheck>(); //Erstelle Blocks als List
        int checkkistate;
        public Vector2 checkpoint;
        bool checkjump;
        double checkjumpt;
        double checktime;
        bool checkstart;
        //CheckpointStart//
        int startkistate;
        public Vector2 startpoint;
        bool startjump;
        double startjumpt;
        double starttime;
        bool startstart;

        public Hero(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            checkpoint = new Vector2(x, y);
            position.X = x;
            position.Y = y;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionHeight"]));
            heroStartTime = Convert.ToInt32((double)Game1.luaInstance["heroStartTime"]);
            spine = new Spine();
        }
        public virtual void attack()
        {
            Sound.Play("ashbrett_attack");
            Sound.Play("attack");
            attacktimer = 1;
            spine.anim("attack", 3, false);
        }
        public void Load(ContentManager Content, GraphicsDeviceManager graphics)//Wird im Hauptgame ausgeführt und geladen
        {
            spine.Load(position, "ashbrett", (float)Convert.ToDouble(Game1.luaInstance["heroScale"]), acceleration);
        }
        public void Draw(GameTime gameTime, Camera camera)
        {
            //Wird im Hauptgame ausgeführt und malt den Spieler mit der entsprechenden Animation
            if (start)
            {
                spine.Draw(gameTime, camera, position);
            }
        }
        public void Reset()
        {
            kistate = checkkistate;
            start = checkstart;
            herotime = checktime;
            jump = checkjump;
            jumptimer = checkjumpt;
            spine.skeleton.x = checkpoint.X;
            spine.skeleton.y = checkpoint.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
            kicheck.Clear();
            foreach (KICheck kc in kicheckcp)
            {
                kicheck.Add(kc);
            }
        }
        public void StartReset()
        {
            kistate = startkistate;
            start = startstart;
            herotime = starttime;
            jump = startjump;
            jumptimer = startjumpt;
            spine.skeleton.x = startpoint.X;
            spine.skeleton.y = startpoint.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
            kicheck.Clear();
        }
        public void StartSave()
        {
            startkistate = kistate;
            startstart = start;
            starttime = herotime;
            startpoint.X = position.X;
            startpoint.Y = position.Y;
            startjump = jump;
            startjumpt = jumptimer;
        }
        public void Save()
        {
            checkkistate = kistate;
            checkstart = start;
            checktime = herotime;
            checkpoint.X = position.X;
            checkpoint.Y = position.Y;
            checkjump = jump;
            checkjumpt = jumptimer;
            kicheckcp.Clear();
            foreach (KICheck kc in kicheck)
            {
                kicheckcp.Add(kc);
            }
        }
        public virtual void Update(GameTime gameTime, Map map,Rectangle spieler)
        {
            if (start)
            {
                if (slowtime != 0)
                {
                    slowtime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (slowtime < 0)
                    {
                        slowtime = 0;
                    }
                }
                speed = Convert.ToInt32((double)Game1.luaInstance["heroSpeed"]);
                int realspeed = Convert.ToInt32((double)Game1.luaInstance["heroKISpeed"]);
                if (GameScreen.slow != 0)
                {
                    speed = speed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
                }
                if (slowtime != 0)
                {
                    speed = speed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
                }
                airspeed = Convert.ToInt32((double)Game1.luaInstance["heroAirspeed"]);
                jumppower = Convert.ToInt32((double)Game1.luaInstance["heroJumppower"]);
                gravitation = Convert.ToInt32((double)Game1.luaInstance["heroGravitation"]);
                //Geschwindigkeit festlegen
                int actualspeed = speed;
                if (jump || fall)
                {
                    actualspeed = airspeed;
                }
                float spielerdistanz = spieler.X - position.X;
                if (Math.Abs(spielerdistanz) < 240 && cbox.box.Y >= spieler.Y && cbox.box.Y - 480 <= spieler.Y + spieler.Height && !jump && !fall)
                {
                    if (breathtimer < 0)
                    {
                        Sound.Play("ashbrett_breath");
                        breathtimer = 20;
                    }
                    else
                    {
                        breathtimer -= gameTime.ElapsedGameTime.TotalMilliseconds/1000;
                    }
                    bool geht = true;
                    for (int i = 0; i < 10; i++)
                    {
                        kicollide.Y = cbox.box.Y - i *48;
                        foreach (Block block in map.blocks)
                        {
                            if (kicollide.Intersects(block.cbox) && block.block)
                            {
                                geht = false;
                                break;
                            }

                        }
                    }
                    if (geht)
                    {
                        if (kistate != 4 && kistate != 5)
                        {
                            kistate = 0;
                        }
                    }
                    else
                    {
                        if (kicheck.Count() != 0)
                        {
                            foreach (KIPoint kipoint in map.kipoints)
                            {
                                if (kipoint.id == kicheck.ElementAt(0).id)
                                    spieler = kipoint.cbox;
                            }
                        }
                    }
                }
                else
                {
                    Sound.Stop("ashbrett_breath");
                        if (kicheck.Count() != 0)
                        {
                            foreach (KIPoint kipoint in map.kipoints)
                            {
                                if (kipoint.id == kicheck.ElementAt(0).id)
                                    spieler = kipoint.cbox;
                            }
                        }
                }
                float punktdistanz = spieler.X - position.X;
                //sposition.X = map.size.X;
                //Vector2 Punkt = map.kipoints.ElementAt(kipoint).position;
               // sposition.X = Punkt.X;
                if (attacktimer > 0)
                {
                    attacktimer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    //Wenn Spieler ist hinten bewege zurück
                    if (spieler.X < position.X)
                    {
                        actualspeed = -actualspeed;
                        realspeed = -realspeed;
                    }
                    if (!cbox.box.Intersects(spieler))
                    {
                        GameScreen.test = 0;
                        bool geht = false;
                        kicollide.X = cbox.box.X;
                        kicollide.Y = cbox.box.Y;
                        kicollide.Width = cbox.box.Width;
                        kicollide.Height = cbox.box.Height;
                        if (kistate == 0)
                        {
                            if (Math.Abs(spielerdistanz) < Convert.ToInt32((double)Game1.luaInstance["heroCloseWalkRange"]))
                            {
                                if (actualspeed > 0)
                                    spine.anim("close", 1, true);
                                else
                                    spine.anim("close", 2, true);
                            }
                            else
                            {
                                if (actualspeed > 0)
                                    spine.anim("walk", 1, true);
                                else
                                    spine.anim("walk", 2, true);
                            }
                            //KI ist auf den Boden und alles ist gut
                            //Schaue ob der Block rechts ist
                            if (kicheck.Count() != 0)
                            {
                                for (int i = 0; i < 50; i++)
                                {
                                    kicollide.X = cbox.box.X + i * realspeed;
                                    kicollide.Y = cbox.box.Y;
                                    foreach (KIPoint kipoint in map.kipoints)
                                    {
                                        if (kicollide.Intersects(kipoint.cbox) && kipoint.id == kicheck.ElementAt(0).id)
                                        {
                                            geht = true;
                                            break;
                                        }
                                    }
                                    if (geht)
                                        break;
                                }
                            }
                            //Block ist auf gleicher Höhe, bewege nur drauf zu
                            if (geht)
                            {
                                if (CollisionCheckedVector(actualspeed, 0, map.blocks).X == actualspeed)
                                {
                                    Move(actualspeed, 0, map);
                                }
                                else
                                {
                                    if (!fall && !jump)
                                    {
                                        if (Math.Abs(spielerdistanz) < 240)
                                            Sound.Play("ashbrett_jumpinging");
                                        spine.anim("jump", 3, false);
                                        Jump(gameTime, map); //Springen!
                                        kistate = 3;
                                    }

                                }
                                if (kistate == 0&&CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                                {
                                    //AAAAHHH WIR fallen T_T
                                    //Hmm vielleicht ist da ja ein Block der für die bewegenden Plattformen zuständig ist?
                                    bool bewegblock = false;
                                    kicollide.Y += 1;
                                    foreach (Block block in map.blocks)
                                    {
                                        if (kicollide.Intersects(block.cbox) && block.type == "movingend")
                                        {
                                            bewegblock = true;
                                            //TATSACHE!!
                                            //Schaue ob rechts ein Block ist
                                        }
                                    }
                                    if (bewegblock && !fall && !jump)
                                    {
                                        //Warten wir einfach mal ...
                                        spine.anim("idle", 3, true);
                                        Move(-actualspeed, 0, map);
                                        kistate = 4;
                                    }
                                    //Beginne den Drüberspringmodus für Abgründe!!!HA!
                                    else
                                    {
                                        if (!fall && !jump)
                                        {
                                            if (Math.Abs(spielerdistanz) < 240)
                                                Sound.Play("ashbrett_jumping");
                                            Jump(gameTime, map); //Springen!
                                            kistate = 1;
                                        }
                                        else
                                        {
                                            Move(-actualspeed, 0, map);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                GameScreen.test = 2;
                                if (spieler.Y < position.Y - 20)
                                {
                                    if (CollisionCheckedVector(realspeed, 0, map.blocks).X == realspeed)
                                    {
                                        bool bewegblock = false;
                                        kicollide.X = cbox.box.X;
                                        kicollide.Y += 1;
                                        foreach (Block block in map.blocks)
                                        {
                                            if (kicollide.Intersects(block.cbox) && block.type == "movingend")
                                            {
                                                if (spieler.X > position.X && block.cbox.X > position.X || spieler.X < position.X && block.cbox.X < position.X)
                                                    bewegblock = true;
                                                //TATSACHE!!
                                                //Schaue ob rechts ein Block ist
                                            }
                                        }
                                        if (bewegblock && !fall && !jump)
                                        {
                                            //Warten wir einfach mal ...
                                            spine.anim("idle", 3, true);
                                            Move(-actualspeed, 0, map);
                                            kistate = 4;
                                        }
                                        else
                                        {
                                            //Block ist über den Hero
                                            bool b = false;
                                            int deltay = 0;
                                            Rectangle kicollide2 = cbox.box;
                                            for (int i = 0; i < 60; i++)
                                            {
                                                float t = (float)(i / 22);
                                                deltay = deltay + (int)(-jumppower + (gravitation * t));
                                                kicollide.X = cbox.box.X + (i * realspeed);
                                                kicollide.Y = cbox.box.Y + deltay;
                                                kicollide2.X = cbox.box.X + (i * realspeed);
                                                if (kicollide.Intersects(spieler) && !kicollide2.Intersects(spieler))
                                                {
                                                    b = true;
                                                    break;
                                                }
                                            }
                                            if (b)
                                            {
                                                if (!fall && !jump)
                                                {
                                                    if (Math.Abs(spielerdistanz) < 240)
                                                        Sound.Play("ashbrett_jumping");
                                                    spine.anim("jump", 3, false);
                                                    Jump(gameTime, map); //Springen!
                                                    kistate = 2;
                                                }
                                                else
                                                {
                                                    Move(-actualspeed, 0, map);
                                                }
                                            }
                                            else
                                            {
                                                Move(actualspeed, 0, map);
                                                if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                                                {
                                                    Move(-actualspeed, 0, map);
                                                    if (!fall && !jump)
                                                    {
                                                        if (Math.Abs(spielerdistanz) < 240)
                                                            Sound.Play("ashbrett_jumping");
                                                        spine.anim("jump", 3, false);
                                                        Jump(gameTime, map); //Springen!
                                                        kistate = 2;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!fall && !jump)
                                        {
                                            if (Math.Abs(spielerdistanz) < 240)
                                                Sound.Play("ashbrett_jumping");
                                            spine.anim("jump", 3, false);
                                            Jump(gameTime, map); //Springen!
                                            kistate = 3;
                                        }

                                    }
                                }
                                else
                                {
                                    if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                                    {
                                        for (int i = 0; i < 10; i++)
                                        {
                                            kicollide.X = cbox.box.X;
                                            kicollide.Y = cbox.box.Y + i * gravitation;
                                            if (kicollide.Intersects(spieler))
                                                geht = true;
                                        }
                                    }
                                    if (!geht)
                                        Move(actualspeed, 0, map);
                                }

                            }
                        }
                        else if (kistate == 1)
                        {
                            //KI befindet sich im Drüberspringmodus bei Abgründen!!
                            //Gucke ob er Grund haben könnte
                            if (CollisionCheckedVector(0, (int)((gravitation)), map.blocks).Y == (int)((gravitation)))
                            {
                                if (fall)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        kicollide.Y = cbox.box.Y + i * gravitation;
                                        foreach (Block block in map.blocks)
                                        {
                                            if (kicollide.Intersects(block.cbox) && block.block)
                                            {
                                                geht = true;
                                                break;
                                            }

                                        }
                                    }
                                }
                                //Kein Grund T_T Beweg mich mal
                                for (int i = 0; i < 10; i++)
                                {
                                    kicollide.Y = cbox.box.Y + i * gravitation;
                                    if (kicollide.Intersects(spieler))
                                        geht = true;
                                }
                                if (!geht)
                                    Move(actualspeed, 0, map);
                            }
                            else
                            {
                                jumptimer = 0;
                                if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                                {
                                    //Grund!!!! Wir sind unten!!! Starte normalen Modus
                                    kistate = 0;
                                }
                            }
                        }
                        else if (kistate == 2)
                        {
                            if (jump)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    kicollide.Y = cbox.box.Y + i * gravitation;
                                    if (kicollide.Intersects(spieler))
                                        geht = true;
                                }
                            }
                            if (!geht)
                                Move(actualspeed, 0, map);
                            //KI befindet sich im Drüberspringmodus!!
                            //Es scheint etwas rechts gegeben zu haben wo er drüberspringen muss
                            //Überprüfe ob rechts immernoch etwas ist
                            if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                            {
                                jumptimer = 0;
                                kistate = 0;
                            }
                        }

                        else if (kistate == 3)
                        {
                            //KI befindet sich im Drüberspringmodus!!
                            //Es scheint etwas rechts gegeben zu haben wo er drüberspringen muss
                            //Überprüfe ob rechts immernoch etwas ist
                            if (CollisionCheckedVector(actualspeed, 0, map.blocks).X == actualspeed)
                            {
                                //Rechts ist nichts mehr! Kann mit dem Springen aufhören und sich draufbewegen
                                Move(actualspeed, 0, map);
                                jumptimer = 0;
                                kistate = 0;
                            }
                        }

                        else if (kistate == 4)
                        {
                            spine.anim("idle", 3, true);
                            //KI befindet sich im Wartemodus!!
                            //Überprüfe ob die Plattform bald da ist
                            bool bewegblock = false;
                            kicollide.X = cbox.box.X + 64;
                            kicollide.Y = cbox.box.Y + 1;
                            foreach (MovingBlock mblock in map.mblocks)
                            {
                                if (kicollide.Intersects(mblock.cbox))
                                {
                                    bewegblock = true;
                                    //TATSACHE!! Er ist da!!
                                }
                            }
                            if (bewegblock)
                            {
                                if (Math.Abs(spielerdistanz) < 240)
                                    Sound.Play("ashbrett_jumping");
                                spine.anim("jump", 3, false);
                                Move(actualspeed, 0, map);
                                Jump(gameTime, map); //Springen!
                                kistate = 5;
                            }
                        }
                        else if (kistate == 5)
                        {
                            //Er springt auf die Plattform! YEA
                            float t = (float)((Game1.time.TotalMilliseconds - falltimer) / 1000);
                            if (CollisionCheckedVector(0, (int)((gravitation * t)), map.blocks).Y == (int)((gravitation * t)))
                            {
                                //Kein Grund T_T Beweg mich mal
                                Move(actualspeed, 0, map);
                            }
                            else
                            {
                                jumptimer = 0;
                                if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                                {
                                    //Grund!!!! Wir sind unten!!! Starte nächsten Modus
                                    kistate = 6;
                                }
                            }
                        }
                        else if (kistate == 6)
                        {
                            //Befindet sich auf der Plattform
                            //Beweg mich mal nach Forme und schau ob ich da falle T_T
                            spine.anim("walk", 3, true);
                            Move(actualspeed, 0, map);
                            if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                            {
                                //Ah wir fallen! Schnell zurück!
                                Move(-actualspeed, 0, map);
                                kistate = 7;
                            }
                        }
                        else if (kistate == 7)
                        {
                            //Ki Wartet nun am Ende und wartet auf einen movingend
                            spine.anim("idle", 3, true);
                            bool bewegblock = false;
                            kicollide.X = cbox.box.X + 48;
                            kicollide.Y = cbox.box.Y + 1;
                            foreach (Block block in map.blocks)
                            {
                                if (kicollide.Intersects(block.cbox) && block.type == "movingend")
                                {
                                    bewegblock = true;
                                    //TATSACHE!! Er ist da!!
                                }
                            }
                            if (bewegblock)
                            {
                                if (Math.Abs(spielerdistanz) < 240)
                                    Sound.Play("ashbrett_jumping");
                                spine.anim("jump", 3, false);
                                Jump(gameTime, map); //Springen!
                                kistate = 8;
                            }
                        }
                        else if (kistate == 8)
                        {
                            //Er springt auf die Plattform! YEA
                            float t = (float)((Game1.time.TotalMilliseconds - falltimer) / 1000);
                            if (CollisionCheckedVector(0, (int)((gravitation * t)), map.blocks).Y == (int)((gravitation * t)))
                            {
                                //Kein Grund T_T Beweg mich mal
                                Move(actualspeed, 0, map);
                            }
                            else
                            {
                                if (CollisionCheckedVector(actualspeed+96, 0, map.blocks).X == actualspeed+96)
                                {
                                    jumptimer = 0;
                                    if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                                    {
                                        //Grund!!!! Wir sind unten!!! Starte nächsten Modus
                                        kistate = 0;
                                    }
                                }
                            }
                        }
                        else if (kistate == 9)
                        {
                            if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                            {
                                //Grund!!!! Wir sind unten!!! Starte normalen Modus
                                kistate = 0;
                            }
                        }
                    }
                }
                if (CollisionCheckedVector(0, 1, map.blocks).Y > 0 && !jump)
                {
                    if (!fall)
                    {
                        fall = true;
                        falltimer = Game1.time.TotalMilliseconds;
                    }
                    float t = (float)((Game1.time.TotalMilliseconds - falltimer) / 1000);
                    Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
                }
                else
                {
                    fall = false;
                }

                //Sprung fortführen
                if (jump)
                {
                    Jump(gameTime, map);
                }
                if (position.Y > map.size.Y) Reset();
                foreach (MovingBlock block in map.mblocks)
                {
                    Rectangle collide = new Rectangle(cbox.box.X, cbox.box.Y + 1, cbox.box.Width, cbox.box.Height);
                    //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                    int movespeed = Convert.ToInt32((double)Game1.luaInstance["blockSpeed"]);
                    if (block.move == 2)
                        movespeed = -movespeed;
                    if (collide.Intersects(block.cbox))
                    {
                        if (GameScreen.slow != 0)
                        {
                            movespeed = movespeed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
                        }
                        Move(movespeed, 0, map);
                        break;
                    }

                    collide.Y = cbox.box.Y;
                    collide.X = cbox.box.X - movespeed;
                    if (collide.Intersects(block.cbox))
                    {
                        if (CollisionCheckedVector(movespeed, 0, map.blocks).X == movespeed)
                        {
                            Move(movespeed, 0, map);
                            break;
                        }
                        else
                        {
                            if (block.move == 1)
                                block.move = 2;
                            else
                                block.move = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                herotime += gameTime.ElapsedGameTime.TotalSeconds;
                if (herotime > heroStartTime)
                    start = true;
            }
        }

        public void Jump(GameTime gameTime, Map map) //Deine Mudda springt bei Doodle Jump nach unten.
        {
            if (CollisionCheckedVector(0, -1, map.blocks).Y < 0)
            {
                if (!jump)
                {
                    jump = true;
                    jumptimer = Game1.time.TotalMilliseconds;
                }
                float t = (float)((Game1.time.TotalMilliseconds - jumptimer) / 1000);
                int deltay = (int)(-jumppower + (gravitation * t));
                if (deltay > 0)
                {
                    jump = false;
                    fall = true;
                    falltimer = Game1.time.TotalMilliseconds;
                }
                else
                {
                    Move(0, deltay, map); //v(t)=-g*t
                }
            }
            else
            {
                jump = false;
            }
        }

        public void Move(int deltax, int deltay, Map map) //Falls Input, bewegt den Spieler
        {
            Vector2 domove = new Vector2(0, 0);
            domove = CollisionCheckedVector(deltax, deltay, map.blocks);
            spine.skeleton.X += domove.X;
            spine.skeleton.Y += domove.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
        }


        public Vector2 CollisionCheckedVector(int x, int y, List<Block> list)
        {
            CollisionBox cboxnew = new CollisionBox((int)cbox.offset.X, (int)cbox.offset.Y, cbox.box.Width, cbox.box.Height);
            cboxnew.Update(cbox.position);
            Vector2 move = new Vector2(0, 0);
            int icoll;
            bool stop;
            //Größere Koordinate als Iteration nehmen
            if (Math.Abs(x) > Math.Abs(y))
            {
                icoll = Math.Abs(x);
            }
            else
            {
                icoll = Math.Abs(y);
            }
            //Iteration
            for (int i = 1; i <= icoll; i++)
            {
                stop = false;
                //Box für nächsten Iterationsschritt berechnen
                cboxnew.box.X = this.cbox.box.X + ((x / icoll) * i);
                cboxnew.box.Y = this.cbox.box.Y + ((y / icoll) * i);
                //Gehe die Blöcke der Liste durch
                foreach (Block block in list)
                {
                    //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                    if (cboxnew.box.Intersects(block.cbox)&&block.block)
                    {
                        stop = true;
                        break;
                    }
                }
                if (stop == true) //Bei Kollision: Kollisionsabfrage mit letztem kollisionsfreien Zustand beenden
                {
                    break;
                }
                else //Kollisionsfreien Fortschritt speichern
                {
                    move.X = cboxnew.box.X - cbox.box.X;
                    move.Y = cboxnew.box.Y - cbox.box.Y;
                }
            }
            return move;
        }
    }
}
