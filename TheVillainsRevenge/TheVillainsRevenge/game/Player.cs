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
        public Vector2 lastPosition; //Position vor vorherigem Update
        public CollisionBox cbox;
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        public bool savejump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int jumppower; //Anfangsgeschwindigkeit in m/s _/60
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int lifes;
        public int item1;
        public int item2;
        public Spine spine;
        public List<KICheck> kicheck = new List<KICheck>(); //Erstelle Blocks als List
        public float initAcceleration;
        public float acceleration;
        public bool hit = false;
        public bool richtung = false;
        public bool smash = false;
        public bool smashImpact = false;
        public float smashCooldown = 0;
        public int smashIntensity;
        public int smashInitIntensity;
        public double hitTimer;
        public double smashTimer;
        public Rectangle hitCbox;
        //Checkpoint Speicherng//
        public List<KICheck> kicheckcp = new List<KICheck>(); //Erstelle Blocks als List
        public Vector2 checkpoint;
        double checkSmashTimer = 0;
        bool checkjump;
        double checkjumpt;
        float checkSmashCooldown;
        //Start Speicherng//
        public Vector2 startpoint;
        public double startSmashTimer = 0;
        public bool startjump;
        public double startjumpt;
        public float startSmashCooldown;
        public Random randomNumber = new Random();

        public Player(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            checkpoint = new Vector2(x, y);
            position.X = x;
            position.Y = y;
            lastPosition = position;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["playerCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionHeight"]));
            lifes = Game1.leben;
            spine = new Spine();
            initAcceleration = (float)Convert.ToInt32((double)Game1.luaInstance["playerAcceleration"]) / 100;
            smashInitIntensity = Convert.ToInt32((double)Game1.luaInstance["playerSmashIntensity"]);
            smashCooldown = (float)Convert.ToDouble(Game1.luaInstance["playerMegaSchlagCooldown"]) * 1000;
        }
        public void StartSave()
        {
            startSmashTimer = smashTimer;
            startpoint.X = position.X;
            startpoint.Y = position.Y;
            startjump = jump;
            startjumpt = jumptimer;
            startSmashCooldown = smashCooldown;
        }
        public void StartReset()
        {
            smashTimer = startSmashTimer;
            jump = startjump;
            jumptimer = startjumpt;
            spine.skeleton.x = startpoint.X;
            spine.skeleton.y = startpoint.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
            lastPosition = new Vector2(spine.skeleton.X, spine.skeleton.Y);
            kicheck.Clear();
            smashCooldown = startSmashCooldown;
        }
        public void Save(int x)
        {
            checkSmashCooldown = smashCooldown;
            checkSmashTimer = smashTimer;
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
            smashTimer = checkSmashTimer;
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

        public virtual void Load(ContentManager Content, GraphicsDeviceManager graphics)//Wird im Hauptgame ausgeführt und geladen
        {
            spine.Load(position, "bonepuker", (float)Convert.ToDouble(Game1.luaInstance["playerScale"]), initAcceleration);
        }

        public virtual void getHit(string animation)
        {
            Sound.Play("bonepuker_dying");
            lifes--;
            Game1.leben = lifes;
            spine.anim(animation, 0, false);
        }

        public void Update(GameTime gameTime, Map map, Princess princess)
        {
            speed = Convert.ToInt32((double)Game1.luaInstance["playerSpeed"]);
            airspeed = Convert.ToInt32((double)Game1.luaInstance["playerAirspeed"]);
            jumppower = Convert.ToInt32((double)Game1.luaInstance["playerJumppower"]);
            gravitation = Convert.ToInt32((double)Game1.luaInstance["playerGravitation"]);

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
            //Einfluss princess.beating
            if (princess.beating)
            {
                actualspeed = actualspeed / 2;
                float randomAcceleration = (float)randomNumber.Next(-50, 50) / 80;
                acceleration += initAcceleration * randomAcceleration;
                if (acceleration < -initAcceleration)
                {
                    acceleration = -initAcceleration;
                }
                if (acceleration > initAcceleration)
                {
                    acceleration = initAcceleration;
                }
            }
            //-----Sprung-----
            if ((Game1.input.sprung || savejump) && !princess.beating)
            {
                if (!jump && !fall && Game1.input.sprungp)
                {
                    Sound.Play("bonepuker_jump");
                    spine.animationTimer = Game1.time.TotalMilliseconds;
                    spine.animationState.SetAnimation(0, "jump", false);
                    spine.animation = "jump";
                    //spine.Clear(0);
                    //spine.anim("jump", 0, false, gameTime);
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
            if (Game1.input.smash && !princess.beating && (jump || fall))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > (smashTimer + smashCooldown)) //Smash beginnen
                {
                    if (Game1.time.TotalMilliseconds > (smashTimer + smashCooldown)) //Smash beginnen
                    {
                        Sound.Play("bonepuker_smash");
                        jump = false;
                        fall = true;
                        hit = false;
                        smash = true;
                        spine.anim("smash", 0, false);
                        smashTimer = Game1.time.TotalMilliseconds;
                        smashIntensity = smashInitIntensity;
                        falltimer = Game1.time.TotalMilliseconds - Convert.ToInt32((double)Game1.luaInstance["playerMegaSchlagFall"]);
                    }
                }
            }
            else if (Game1.input.hit && !princess.beating)
            {
                if (!smash && (!hit || Game1.time.TotalMilliseconds > hitTimer + (spine.skeleton.Data.FindAnimation("attack").Duration * 1000) * 0.4)) //Schlag beginnen
                {
                    Sound.Play("bonepuker_attack");
                    Sound.Play("schlag");
                    hit = true;
                    spine.anim("attack", 0, false);
                    hitTimer = Game1.time.TotalMilliseconds;
                }
            }
            //Schlag ggf beenden
            if (hit && Game1.time.TotalMilliseconds > hitTimer + (spine.skeleton.Data.FindAnimation("attack").Duration * 1000))
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
                else if (CollisionCheckedVector(0, 1, map.blocks, map).Y == 0)
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
                            spine.anim("run", 2, true);
                        }
                        else
                        {
                            spine.anim("jump", 2, false);
                        }
                    }
                    else if (spine.flipSkel && Math.Abs(acceleration) <= 2 / (60 * initAcceleration)) //Bei direktem Richtungstastenwechsel trotzdem beim Abbremsen in idle übergehen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("idle", 0, true); //In idle-Position übergehen
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
                            spine.anim("run", 1, true);
                        }
                        else
                        {
                            spine.anim("jump", 1, false);
                        }
                    }
                    else if (!spine.flipSkel && Math.Abs(acceleration) <= 2 / (60 * initAcceleration)) //Bei direktem Richtungstastenwechsel trotzdem in idle übergehen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("idle", 0, true); //In idle-Position übergehen
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
                        spine.anim("idle", 0, true); //In idle-Position übergehen
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
                if (Math.Abs(CollisionCheckedVector((int)((acceleration / initAcceleration) * actualspeed), 0, map.blocks, map).X) < Math.Abs((int)((acceleration / initAcceleration) * actualspeed)))
                {
                    acceleration = -acceleration * 0.8f;
                }
                Move((int)((acceleration / initAcceleration) * actualspeed), 0, map);
                if (Game1.input.itemu1)
                {
                    if (item1 == 1)
                    {
                        Sound.Play("time_shift");
                        GameScreen.slow = GameScreen.slow + Convert.ToInt32((double)Game1.luaInstance["itemSlowTime"]);
                        item1 = 0;
                    }
                    else if (item1 == 2 && !fall && !jump)
                    {
                        map.objects.Add(new Banana(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 96), 1));
                        item1 = 0;
                    }
                    else if (item1 == 3 && !fall && !jump)
                    {
                        Sound.Play("skullmonkey_item");
                        map.enemies.Add(new Monkey(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 64), 2, false));
                        item1 = 0;
                    }
                }
                if (Game1.input.itemu2)
                {
                    if (item2 == 1)
                    {
                        Sound.Play("time_shift");
                        GameScreen.slow = GameScreen.slow + Convert.ToInt32((double)Game1.luaInstance["itemSlowTime"]);
                        item2 = 0;
                    }
                    else if (item2 == 2 && !fall && !jump)
                    {
                        map.objects.Add(new Banana(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 48), 1));
                        item2 = 0;
                    }
                    else if (item2 == 3 && !fall && !jump)
                    {
                        Sound.Play("skullmonkey_item");
                        map.enemies.Add(new Monkey(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 64), 2, false));
                        item2 = 0;
                    }
                }
            }

            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks, map).Y > 0 && !jump)
            {
                if (!fall)
                {
                    fall = true;
                    falltimer = Game1.time.TotalMilliseconds;
                }
                float t = (float)((Game1.time.TotalMilliseconds - falltimer) / 1000);
                Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
                if(!smash)
                    spine.anim("jump", 0, false);
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
                    if (CollisionCheckedVector(movespeed, 0, map.blocks, map).X == movespeed)
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
            if (jump || fall) //SchlagStaub ggf hiden
            {
                //StaubSkinAttachment hiden
            }
        }

        public void Draw(GameTime gameTime, Camera camera)
        {
            spine.Draw(gameTime, camera, position);
        }

        public void Jump(GameTime gameTime, Map map) //Deine Mudda springt bei Doodle Jump nach unten.
        {
            if (CollisionCheckedVector(0, -1, map.blocks, map).Y < 0)
            {

                if (!jump)
                {
                    spine.anim("jump", 0, false);
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
            domove = CollisionCheckedVector(deltax, deltay, map.blocks, map);
            spine.skeleton.X += domove.X;
            spine.skeleton.Y += domove.Y;
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
            cbox.Update(position);
        }


        public Vector2 CollisionCheckedVector(int x, int y, List<Block> list, Map map)
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
                    if ((cboxnew.box.Intersects(block.cbox) && block.block) || cboxnew.box.X < 0 || (cboxnew.box.X + cboxnew.box.Width) > map.size.X || cboxnew.box.Y <= 0)
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
