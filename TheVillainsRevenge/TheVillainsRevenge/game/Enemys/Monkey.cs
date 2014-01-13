using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Monkey : Enemy
    {
<<<<<<< HEAD
        public double throwtime = 0;
        float acceleration;

=======
>>>>>>> origin/KI-Stuff
        public Monkey(Vector2 pos, int t)
            : base(pos, t)//Konstruktor, setzt Anfangsposition
        {
            spine.Load(position, "skullmonkey", 0.1f, acceleration);
        }
        public override void Update(GameTime gameTime, Map map,Vector2 heropos)
        {
            speed = Convert.ToInt32((double)Game1.luaInstance["monkeySpeed"]);
            if (GameScreen.slow != 0)
            {
                speed = speed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
            }
            gravitation = Convert.ToInt32((double)Game1.luaInstance["monkeyGravitation"]);
            bool move = false;
            if (Math.Abs(position.X - heropos.X) <= Convert.ToInt32((double)Game1.luaInstance["monkeyThrowRange"]))
            {
                if (position.X > heropos.X)
                    mover = true;
                else
                    mover = false;
                if (animeTime <= 0)
                {
                    if (position.X < heropos.X)
                    {
                        map.objects.Add(new Kacke(new Vector2(cbox.box.X, cbox.box.Y), 2, true));
                    }
                    else
                    {
                        map.objects.Add(new Kacke(new Vector2(cbox.box.X, cbox.box.Y), 2, false));
                    }
                    animeTime = 1;
                    if (mover)
                        spine.anim("attack", 2, true, gameTime);
                    else
                        spine.anim("attack", 1, true, gameTime);
                }
                else
                {
                    animeTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (animeTime < 0.5f)
                    {
                        if (mover)
                            spine.anim("walking", 1, true, gameTime);
                        else
                            spine.anim("walking", 2, true, gameTime);
                        move = true;
                    }
                }
                if (move)
                {
                    if (!mover)
                    {
                        speed = -speed;
                    }
                    if (CollisionCheckedVector(speed, 0, map.blocks).X != 0)
                    {
                        Move(speed, 0, map);//Bewege Rechts
                        if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                        {
                            Move(-speed, 0, map);
                        }
                    }
                }
            }
            else
            {
                if(mover)
                    spine.anim("sitting", 1, true, gameTime);
                else
                    spine.anim("sitting", 2, true, gameTime);

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

    }
}
