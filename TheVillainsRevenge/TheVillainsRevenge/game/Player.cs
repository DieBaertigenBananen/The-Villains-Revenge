using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    class Player
    {
        //Deine Mutter ist so fett, selbst die Sonne wird von ihr angezogen
        public Vector2 position; //Position
        Vector2 lastPosition; //Position vor vorherigem Update
        public CollisionBox cbox;
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        bool savejump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int jumppower; //Anfangsgeschwindigkeit in m/s _/60
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int lifes;
        public static int startLifes = Convert.ToInt32((double)Game1.luaInstance["playerStartLifes"]);
        public int item1;
        public int item2;
        public Spine spine;
        public List<KICheck> kicheck = new List<KICheck>(); //Erstelle Blocks als List
        float initAcceleration;
        public float acceleration;
        public bool hit = false;
        public bool richtung = false;
        public bool smash = false;
        public bool smashImpact = false;
        public float smashCooldown = 0;
        public int smashIntensity;
        int smashInitIntensity;
        public double hitTimer;
        public double smashTimer;
        public Rectangle hitCbox;
        bool allowSmash = false;
        //Checkpoint Speicherng//
        public List<KICheck> kicheckcp = new List<KICheck>(); //Erstelle Blocks als List
        public Vector2 checkpoint;
        float checkSmashCooldown = 0;
        bool checkjump;
        double checkjumpt;
        //Start Speicherng//
        public Vector2 startpoint;
        float startSmashCooldown = 0;
        bool startjump;
        double startjumpt;
        Random randomNumber = new Random();

        public Player(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            checkpoint = new Vector2(x, y);
            position.X = x;
            position.Y = y;
            lastPosition = position;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["playerCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionHeight"]));
            lifes = startLifes;
            spine = new Spine();
            initAcceleration = (float)Convert.ToInt32((double)Game1.luaInstance["playerAcceleration"]) / 100;
            smashInitIntensity = Convert.ToInt32((double)Game1.luaInstance["playerSmashIntensity"]);
            smashCooldown = (float)Convert.ToDouble(Game1.luaInstance["playerMegaSchlagCooldown"]) * 1000;
        }
        public void StartSave()
        {
            startSmashCooldown = smashCooldown;
            startpoint.X = position.X;
            startpoint.Y = position.Y;
            startjump = jump;
            startjumpt = jumptimer;
        }
        public void StartReset()
        {
            smashCooldown = startSmashCooldown;
            jump = startjump;
            jumptimer = startjumpt;
            spine.skeleton.x = startpoint.X;
            spine.skeleton.y = startpoint.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
            lastPosition = new Vector2(spine.skeleton.X, spine.skeleton.Y);
            kicheck.Clear();
        }
        public void Save(int x)
        {
            checkSmashCooldown = smashCooldown;
            checkpoint.X = x;
            checkpoint.Y = position.Y;
            checkjump = jump;
            checkjumpt = jumptimer;
            kicheckcp.Clear();
            foreach (KICheck kc in kicheck)
            {
                kicheckcp.Add(kc);
            }
        }
        public void Reset()
        {
            smashCooldown = checkSmashCooldown;
            jump = checkjump;
            jumptimer = checkjumpt;
            spine.skeleton.x = checkpoint.X;
            spine.skeleton.y = checkpoint.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
            lastPosition = new Vector2(spine.skeleton.X, spine.skeleton.Y);
            kicheck.Clear();
            foreach (KICheck kc in kicheckcp)
            {
                kicheck.Add(kc);
            }
        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics)//Wird im Hauptgame ausgeführt und geladen
        {
            spine.Load(position, "bonepuker", (float)Convert.ToDouble(Game1.luaInstance["playerScale"]), initAcceleration);
        }

        public void getHit(GameTime gameTime)
        {
            lifes--;
            spine.anim("die", 0, false, gameTime);
        }

        public void Update(GameTime gameTime, Map map, Princess princess)
        {
            speed = Convert.ToInt32((double)Game1.luaInstance["playerSpeed"]);
            airspeed = Convert.ToInt32((double)Game1.luaInstance["playerAirspeed"]);
            jumppower = Convert.ToInt32((double)Game1.luaInstance["playerJumppower"]);
            gravitation = Convert.ToInt32((double)Game1.luaInstance["playerGravitation"]);
            if (Game1.input.enter)
            {
                lifes = Convert.ToInt32((double)Game1.luaInstance["playerLifes"]);
                item1 = Convert.ToInt32((double)Game1.luaInstance["playerItem1"]);
                item2 = Convert.ToInt32((double)Game1.luaInstance["playerItem2"]);
            }

            //Geschwindigkeit festlegen
            int actualspeed = speed;
            if (jump || fall)
            {
                actualspeed = airspeed;
            }
            //Einfluss Gamepad
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0)
            {
                actualspeed = (int)((float)actualspeed * Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X));
            }
            if (princess.beating)
            {
                float randomAcceleration = ((float)randomNumber.Next(0, 100) / 50) - 1;
                acceleration += (initAcceleration * randomAcceleration);
                if (acceleration < -initAcceleration / 2)
                {
                    acceleration = -initAcceleration / 2;
                }
                if (acceleration > initAcceleration / 2)
                {
                    acceleration = initAcceleration / 2;
                }
            }
            //Deine Mudda stinkt nach Backfisch
            if (CollisionCheckedVector(0, 1, map.blocks).Y == 0) //AllowSmash am Boden zurück setzen
            {
                allowSmash = false;
            }
            //-----Sprung-----
            if ((Game1.input.sprung || savejump) && !princess.beating)
            {
                if (!jump && !fall && Game1.input.sprungp)
                {
                    Jump(gameTime, map); //Springen!
                    savejump = false;
                }
                else if (fall)
                {
                    savejump = true;
                }
                else
                {
                    savejump = false;
                }
            }
            else
            {
                if (jump && !Game1.input.sprungp)
                {
                    jumptimer -= GameScreen.slow + Convert.ToInt32((double)Game1.luaInstance["playerJumpCutoff"]);
                }
            }
            //-----Schlag / Smash starten-----
            if (Game1.input.hit && !princess.beating)
            {
                if (jump || fall)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > (smashTimer + smashCooldown) && allowSmash) //Smash beginnen
                    {
                        jump = false;
                        fall = true;
                        hit = false;
                        smash = true;
                        spine.anim("smash", 0, false, gameTime);
                        smashTimer = gameTime.TotalGameTime.TotalMilliseconds;
                        smashIntensity = smashInitIntensity;
                        falltimer = gameTime.TotalGameTime.TotalMilliseconds - Convert.ToInt32((double)Game1.luaInstance["playerMegaSchlagFall"]);
                    }
                }
                if (!smash && !hit) //Schlag beginnen
                {
                    if (jump || fall)
                    {
                        allowSmash = true; //Nächster Schlag in der Luft = smash
                    }
                    Sound.Play("schlag");
                    hit = true;
                    spine.anim("attack", 0, false, gameTime);
                    hitTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            //Schlag ggf beenden
            if (hit && gameTime.TotalGameTime.TotalMilliseconds > hitTimer + (spine.skeleton.Data.FindAnimation("attack").Duration * 1000))
            {
                hit = false;
                spine.animationState.ClearTrack(1);
            }
            //Smash fortführen
            if (smash)
            {
                smashIntensity--;
                //Smash ggf beenden
                if (smashIntensity <= 0)
                {
                    smash = false;
                    smashImpact = false;
                }
                else if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                {
                    smashImpact = true;
                }
            }
            if (!smash)
            {
                //-----Move-----
                if (Game1.input.rechts) //Wenn Rechte Pfeiltaste
                {
                    richtung = true;
                    acceleration += 1 / (60 * initAcceleration);
                    if (Math.Abs(acceleration) <= 2 / (60 * initAcceleration) || Math.Abs(acceleration) > initAcceleration) //Drehen bzw weiter laufen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("run", 2, true, gameTime);
                        }
                        else
                        {
                            spine.anim("jump", 2, false, gameTime);
                        }
                    }
                    else if (spine.flipSkel && Math.Abs(acceleration) <= 2 / (60 * initAcceleration)) //Bei direktem Richtungstastenwechsel trotzdem beim Abbremsen in idle übergehen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("idle", 0, true, gameTime); //In idle-Position übergehen
                        }
                    }
                    if (jump || fall) //Zusätzliche Beschleunigung in der Luft
                    {
                        acceleration += 2 / (60 * initAcceleration);
                    }
                }
                else if (Game1.input.links) //Wenn Rechte Pfeiltaste
                {
                    richtung = false;
                    acceleration -= 1 / (60 * initAcceleration);
                    if (Math.Abs(acceleration) <= 2 / (60 * initAcceleration) || Math.Abs(acceleration) > initAcceleration) //Drehen bzw weiter laufen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("run", 1, true, gameTime);
                        }
                        else
                        {
                            spine.anim("jump", 1, false, gameTime);
                        }
                    }
                    else if (!spine.flipSkel && Math.Abs(acceleration) <= 2 / (60 * initAcceleration)) //Bei direktem Richtungstastenwechsel trotzdem in idle übergehen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("idle", 0, true, gameTime); //In idle-Position übergehen
                        }
                    }
                    if (jump || fall) //Zusätzliche Beschleunigung in der Luft
                    {
                        acceleration -= 2 / (60 * initAcceleration);
                    }
                }
                else
                {
                    //Auslaufen_Abbremsen
                    if (acceleration < 0)
                    {
                        acceleration += 1 / (60 * initAcceleration);
                        if (acceleration > 0) //Nicht umdrehen
                        {
                            acceleration = 0;
                        }
                    }
                    else if (acceleration > 0)
                    {
                        acceleration -= 1 / (60 * initAcceleration);
                        if (acceleration < 0) //Nicht umdrehen
                        {
                            acceleration = 0;
                        }
                    }
                    if (!jump && !fall)
                    {
                        spine.anim("idle", 0, true, gameTime); //In idle-Position übergehen
                    }
                }
                //Keine Beschleunigungs"vermehrung", durch Beschleunigung wird nur MaxSpeed bei jedem Update absolut vermindert. Fake aber funzt...
                if (acceleration < -initAcceleration)
                {
                    acceleration = -initAcceleration;
                }
                if (acceleration > initAcceleration)
                {
                    acceleration = initAcceleration;
                }
                if (Math.Abs(CollisionCheckedVector((int)((acceleration / initAcceleration) * actualspeed), 0, map.blocks).X) < Math.Abs((int)((acceleration / initAcceleration) * actualspeed)))
                {
                    acceleration = -acceleration * 0.8f;
                }
                Move((int)((acceleration / initAcceleration) * actualspeed), 0, map);
                if (Game1.input.itemw)
                {
                    int i = item1;
                    item1 = item2;
                    item2 = i;
                }
                if (Game1.input.itemu)
                {
                    if (item1 == 1)
                    {
                        Sound.Play("time_shift");
                        GameScreen.slow = GameScreen.slow + Convert.ToInt32((double)Game1.luaInstance["itemSlowTime"]);
                        item1 = 0;
                    }
                    else if (item1 == 2 && !fall && !jump)
                    {
                        map.objects.Add(new Banana(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 48), 1));
                        item1 = 0;
                    }
                    else if (item1 == 3 && !fall && !jump)
                    {
                        map.enemies.Add(new Monkey(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 64), 2, false));
                        item1 = 0;
                    }
                    if (item1 == 0 && item2 != 0)
                    {
                        item1 = item2;
                        item2 = 0;
                    }
                }
            }

            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks).Y > 0 && !jump)
            {
                if (!fall)
                {
                    fall = true;
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - falltimer) / 1000);
                Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
                spine.anim("jump", 0, false, gameTime);
            }
            else
            {
                if (fall)
                {
                    fall = false;
                    Sound.Play("land");
                }
            }

            //Sprung fortführen
            if (jump)
            {
                Jump(gameTime, map);
            }
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
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
        }

        public void Draw(GameTime gameTime, Camera camera)
        {
            spine.Draw(gameTime, camera, position);
        }

        public void Jump(GameTime gameTime, Map map) //Deine Mudda springt bei Doodle Jump nach unten.
        {
            if (CollisionCheckedVector(0, -1, map.blocks).Y < 0)
            {

                if (!jump)
                {
                    spine.anim("jump", 0, false, gameTime);
                    jump = true;
                    jumptimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - jumptimer) / 1000);
                int deltay = (int)(-jumppower + (gravitation * t));
                if (deltay > 0)
                {
                    jump = false;
                    fall = true;
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
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
                    if (cboxnew.box.Intersects(block.cbox) && block.block)
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
        //Vector2 BoundingCheckedVector(int x, int y, List<Block> list)
        //{
        //    position.Y = spine.skeleton.Y;
        //    position.X = spine.skeleton.X;
        //    Vector2 move = new Vector2(0, 0);
        //    int icoll;
        //    bool stop;
        //    //Größere Koordinate als Iteration nehmen
        //    if (Math.Abs(x) > Math.Abs(y))
        //    {
        //        icoll = Math.Abs(x);
        //    }
        //    else
        //    {
        //        icoll = Math.Abs(y);
        //    }
        //    //Iteration
        //    for (int i = 1; i <= icoll; i++)
        //    {
        //        stop = false;
        //        //Box für nächsten Iterationsschritt berechnen
        //        spine.skeleton.X = position.X + ((x / icoll) * i);
        //        spine.skeleton.Y = position.Y + ((y / icoll) * i);
        //        spine.bounds.Update(spine.skeleton, true);
        //        //Gehe die Blöcke der Liste durch
        //        foreach (Block block in list)
        //        {
        //            Vector2 ol = new Vector2((float)block.cbox.X, (float)block.cbox.Y);
        //            Vector2 or = new Vector2((float)(block.cbox.X + block.cbox.Width), (float)block.cbox.Y);
        //            Vector2 ul = new Vector2((float)block.cbox.X, (float)(block.cbox.Y + block.cbox.Height));
        //            Vector2 ur = new Vector2((float)(block.cbox.X + block.cbox.Width), (float)(block.cbox.Y + block.cbox.Height));
        //            if (spine.bounds.AabbContainsPoint(ol.X, ol.Y) || spine.bounds.AabbContainsPoint(or.X, or.Y) || spine.bounds.AabbContainsPoint(ul.X, ul.Y) || spine.bounds.AabbContainsPoint(ur.X, ur.Y))
        //            {
        //                BoundingBoxAttachment colOL = spine.bounds.ContainsPoint(ol.X, ol.Y);
        //                BoundingBoxAttachment colOR = spine.bounds.ContainsPoint(or.X, or.Y);
        //                BoundingBoxAttachment colUL = spine.bounds.ContainsPoint(ul.X, ul.Y);
        //                BoundingBoxAttachment colUR = spine.bounds.ContainsPoint(ur.X, ur.Y);
        //                //BoundingBoxAttachment bb = bounds.BoundingBoxes.FirstOrDefault();
        //                //if (colOL == bb || colOR == bb || colUL == bb || colUR == bb) //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
        //                if (colOL != null || colOR != null || colUL != null || colUR != null) //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
        //                {
        //                    stop = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (stop == true) //Bei Kollision: Kollisionsabfrage mit letztem kollisionsfreien Zustand beenden
        //        {
        //            break;
        //        }
        //        else //Kollisionsfreien Fortschritt speichern
        //        {
        //            move.X = spine.skeleton.X - position.X;
        //            move.Y = spine.skeleton.Y - position.Y;
        //        }
        //    }
        //    spine.skeleton.Y = position.Y;
        //    spine.skeleton.X = position.X;
        //    return move;
        //}
    }
}
