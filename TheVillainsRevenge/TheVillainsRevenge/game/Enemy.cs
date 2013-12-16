using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Enemy
    {
        public Vector2 position; //Position
        public CollisionBox cbox;
        public int type;
        public bool fall = false;
        public double falltimer;
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public bool mover = false;
        public Enemy(Vector2 pos, int t) //Konstruktor, setzt Anfangsposition
        {
            position = pos;
            type = t;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["enemyCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["enemyCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["enemyCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["enemyCollisionHeight"]));
        }
        public void Update(GameTime gameTime, Map map)
        {
            speed = Convert.ToInt32((double)Game1.luaInstance["enemySpeed"]);
            if (GameScreen.slow != 0)
            {
                speed = speed /Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
            }
            gravitation = Convert.ToInt32((double)Game1.luaInstance["enemyGravitation"]);
            if (mover)
            {
                if (CollisionCheckedVector(speed, 0, map.blocks).X != 0)
                {
                    Move(speed, 0, map);//Bewege Rechts
                    if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                    {
                        mover = false;
                        Move(-speed, 0, map);
                    }
                }
                else
                {
                    mover = false;
                }
            }
            else
            {
                if (CollisionCheckedVector(-speed, 0, map.blocks).X != 0)
                {
                    Move(-speed, 0, map);//Bewege Links
                    if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                    {
                        mover = true;
                        Move(speed, 0, map);
                    }
                }
                else
                {
                    mover = true;
                }
            }

            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
            {
                if (!fall)
                {
                    fall = true;
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - falltimer) / 1000);
                Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
            }
            else
            {
                fall = false;
            }


            foreach (Block block in map.blocks)
            {
                if (block.move != 0)
                {
                    Rectangle collide = new Rectangle(cbox.box.X, cbox.box.Y + 1, cbox.box.Width, cbox.box.Height);
                    //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                    if (collide.Intersects(block.cbox))
                    {
                        int movespeed = Convert.ToInt32((double)Game1.luaInstance["blockSpeed"]);
                        if (GameScreen.slow != 0)
                        {
                            movespeed = movespeed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
                        }
                        if (block.move == 1)
                            Move(movespeed, 0, map);
                        else
                            Move(-movespeed, 0, map);
                        break;
                    }
                }
            }
        }

        public void Move(int deltax, int deltay, Map map) //Falls Input, bewegt den Spieler
        {
            Vector2 domove = new Vector2(0, 0);
            domove = CollisionCheckedVector(deltax, deltay, map.blocks);
            position.X += domove.X;
            position.Y += domove.Y;
            cbox.Update(position);
        }


        Vector2 CollisionCheckedVector(int x, int y, List<Block> list)
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
                    if (cboxnew.box.Intersects(block.cbox))
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
