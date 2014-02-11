using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    class PrincessSpieler : Player
    {
        public bool ishit = false;
        public int screamradius = 0;
        int maxscreamradius = 400;
        double screamtime = 0;
        double screamend = 3;
        double screamstart = 1;
        public double flytime = 0;
        public double flymaxtime = 1.0;
        public bool hitrichtung;
        public PrincessSpieler(int x, int y) : base(x,y) //Konstruktor, setzt Anfangsposition
        {
            checkpoint = new Vector2(x, y);
            position.X = x;
            position.Y = y;
            lastPosition = position;
            cbox = new CollisionBox(-50, -150, 100, 140);
            lifes = Game1.leben;
            spine = new Spine();
            initAcceleration = (float)Convert.ToInt32((double)Game1.luaInstance["playerAcceleration"]) / 100;
            smashInitIntensity = Convert.ToInt32((double)Game1.luaInstance["playerSmashIntensity"]);
            smashCooldown = 10 * 1000;
        }

        public void getHit(GameTime gameTime,Map map,Vector2 hposition,string animation)
        {
            lifes--;
            Game1.leben = lifes;
            if (lifes != 0)
            {
                Sound.Play("sweetcheeks_hit");
                ishit = true;
                if (hposition.X > position.X)
                    hitrichtung = false;
                else
                    hitrichtung = true;
                ishit = true;
                flytime = 0;
                spine.Clear(0);
                spine.anim("jump", 0, false);
                Jump(gameTime, map); //Springen!
                savejump = false;
            }
            else
            {
                ishit = true;
                spine.Clear(0);
                Sound.Play("sweetcheeks_dying");
                spine.anim("die", 3, false);
            }
        }


        public override void Load(ContentManager Content, GraphicsDeviceManager graphics)//Wird im Hauptgame ausgeführt und geladen
        {
            spine.Load(position, "sweetcheeks", 0.3f, initAcceleration);
        }
        public void Update(GameTime gameTime, Map map,Rectangle hero)
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
            if (ishit)
            {
                if (lifes != 0)
                {
                    if (!hitrichtung) //rechts fliegend
                    {
                        actualspeed = -actualspeed;
                    }
                    Move(actualspeed, 0, map);
                    flytime += gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    if (flytime > flymaxtime)
                    {
                        ishit = false;
                    }
                }

            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0)
                {
                    actualspeed = (int)((float)actualspeed * Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X));
                }
                //-----Sprung-----
                if ((Game1.input.sprung || savejump))
                {
                    if (!jump && !fall && Game1.input.sprungp)
                    {
                        Sound.Play("sweetcheeks_jump");
                        spine.Clear(0);
                        spine.anim("jump", 0, false);
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
                if (Game1.input.smash)
                {
                    if (jump || fall)
                    {
                        if (Game1.time.TotalMilliseconds > (smashTimer + smashCooldown)) //Smash beginnen
                        {
                            Sound.Play("sweetcheeks_siren_scream");
                            jump = false;
                            fall = true;
                            hit = false;
                            smash = true;
                            spine.anim("scream", 3, false);
                            smashTimer = Game1.time.TotalMilliseconds;
                            falltimer = Game1.time.TotalMilliseconds;
                            screamtime = 0;
                        }
                    }
                }
                else if (Game1.input.hit)
                {
                    if (!smash && !hit) //Schlag beginnen
                    {
                        Sound.Play("sweetcheeks_attack");
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
                    screamtime += gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    if (screamtime > screamend)
                    {
                        screamradius = 0;
                        smash = false;
                    }
                    else
                    {
                        falltimer = Game1.time.TotalMilliseconds;
                        if (screamradius < maxscreamradius && screamtime > screamstart)
                            screamradius += maxscreamradius / 40;
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
                                spine.anim("run", 1, true);
                            }
                            else
                            {
                                spine.anim("jump", 1, false);
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
                                spine.anim("run", 2, true);
                            }
                            else
                            {
                                spine.anim("jump", 2, false);
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
                    if (Math.Abs(CollisionCheckedVector((int)((acceleration / initAcceleration) * actualspeed), 0, map.blocks, map, hero).X) < Math.Abs((int)((acceleration / initAcceleration) * actualspeed)))
                    {
                        acceleration = -acceleration * 0.8f;
                    }
                    Move((int)((acceleration / initAcceleration) * actualspeed), 0, map);
                }
            }

            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks, map).Y > 0 && !jump)
            {
                if (CollisionCheckedVector(0, 1, map.blocks, map, hero).Y == 0)
                {
                    if(richtung)
                        Move(actualspeed, 0, map);
                    else
                        Move(-actualspeed, 0, map);
                }
                else
                {
                    if (!fall)
                    {
                        fall = true;
                        falltimer = Game1.time.TotalMilliseconds;
                    }
                    float t = (float)((Game1.time.TotalMilliseconds - falltimer) / 1000);
                    Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
                    if (!smash)
                        spine.anim("jump", 0, false);
                }
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
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
        }



        public Vector2 CollisionCheckedVector(int x, int y, List<Block> list, Map map,Rectangle hero)
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
                if (cboxnew.box.Intersects(hero))
                {
                    stop = true;
                    break;
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
