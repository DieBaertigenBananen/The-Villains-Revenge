using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Boss : Hero
    {
        public double inactiveTime = 0;
        public double wellencooldown = 10;
        public bool welleladen = false;
        public Boss(int x, int y): base(x,y) //Konstruktor, setzt Anfangsposition
        {
            checkpoint = new Vector2(x, y);
            position.X = x;
            position.Y = y;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionHeight"]));
            heroStartTime = Convert.ToInt32((double)Game1.luaInstance["heroStartTime"]);
            spine = new Spine();
        }
        public override void attack()
        {
            attacktimer = 1;
            spine.anim("attack", 3, false);
        }
        public override void Update(GameTime gameTime, Map map, Rectangle spieler)
        {
            Rectangle Player = spieler;
            if (wellencooldown > 0)
                wellencooldown -= gameTime.ElapsedGameTime.TotalMilliseconds/1000;
            else if(!welleladen)
            {
                Console.WriteLine("Welle start!");
                welleladen = true;
                inactiveTime = 1;
            }

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
            if (Math.Abs(spielerdistanz) < 120 && cbox.box.Y >= spieler.Y && cbox.box.Y - 480 <= spieler.Y + spieler.Height && !jump && !fall)
            {
                bool geht = true;
                for (int i = 0; i < 10; i++)
                {
                    kicollide.Y = cbox.box.Y - i * 48;
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
                    kistate = 0;
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
                attacktimer -= gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                if (attacktimer <= 0)
                {
                    inactiveTime = 0.3f;
                    spine.anim("idle", 3, true);
                }

            }
            else if (inactiveTime > 0)
            {
                Console.WriteLine("Inactive"+inactiveTime);
                inactiveTime -= gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            }
            else if (welleladen)
            {
                Console.WriteLine("welle woaa");
                if (spieler.X < position.X)
                {
                    spine.anim("attack", 2, false);
                    map.objects.Add(new Welle(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 48), 4, false));
                }
                else
                {
                    spine.anim("attack", 1, false);
                    map.objects.Add(new Welle(new Vector2(cbox.box.X, cbox.box.Y + cbox.box.Height - 48), 4, true));
                }
                welleladen = false;
                wellencooldown = 10;
                inactiveTime = 1;
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
                            if (CollisionCheckedVector(actualspeed, 0, map.blocks, Player).X == actualspeed)
                            {
                                Move(actualspeed, 0, map);
                            }
                            else
                            {
                                if (!fall && !jump)
                                {
                                    spine.anim("jump", 3, false);
                                    Jump(gameTime, map); //Springen!
                                    kistate = 3;
                                }

                            }
                            if (kistate == 0 && CollisionCheckedVector(0, 1, map.blocks, Player).Y > 0)
                            {
                                //AAAAHHH WIR fallen T_T
                                if (!fall && !jump)
                                {
                                    Jump(gameTime, map); //Springen!
                                    kistate = 1;
                                }
                                else
                                {
                                    Move(-actualspeed, 0, map);
                                }

                            }
                        }
                        else
                        {
                            GameScreen.test = 2;
                            if (spieler.Y < position.Y - 20)
                            {
                                if (CollisionCheckedVector(realspeed, 0, map.blocks, Player).X == realspeed)
                                {
                                    kicollide.X = cbox.box.X;
                                    kicollide.Y += 1;
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
                                        if (CollisionCheckedVector(0, 1, map.blocks, Player).Y > 0)
                                        {
                                            Move(-actualspeed, 0, map);
                                            if (!fall && !jump)
                                            {
                                                spine.anim("jump", 3, false);
                                                Jump(gameTime, map); //Springen!
                                                kistate = 2;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!fall && !jump)
                                    {
                                        spine.anim("jump", 3, false);
                                        Jump(gameTime, map); //Springen!
                                        kistate = 3;
                                    }

                                }
                            }
                            else
                            {
                                if (CollisionCheckedVector(0, 1, map.blocks, Player).Y > 0)
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
                        if (CollisionCheckedVector(0, (int)((gravitation)), map.blocks, Player).Y == (int)((gravitation)))
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
                            if (CollisionCheckedVector(0, 1, map.blocks, Player).Y == 0)
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
                        if (CollisionCheckedVector(0, 1, map.blocks, Player).Y == 0)
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
                        if (CollisionCheckedVector(actualspeed, 0, map.blocks, Player).X == actualspeed)
                        {
                            //Rechts ist nichts mehr! Kann mit dem Springen aufhören und sich draufbewegen
                            Move(actualspeed, 0, map);
                            jumptimer = 0;
                            kistate = 0;
                        }
                    }
                    else if (kistate == 4)
                    {
                        if (CollisionCheckedVector(0, 1, map.blocks, Player).Y == 0)
                        {
                            //Grund!!!! Wir sind unten!!! Starte normalen Modus
                            kistate = 0;
                        }
                    }
                }
            }
            if (CollisionCheckedVector(0, 1, map.blocks, Player).Y > 0 && !jump)
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
        }

        public Vector2 CollisionCheckedVector(int x, int y, List<Block> list, Rectangle spieler)
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
                if (cboxnew.box.Intersects(spieler))
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
