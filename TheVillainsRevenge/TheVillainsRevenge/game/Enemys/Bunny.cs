﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Bunny : Enemy
    {
        float acceleration = 0.0f;
        
        public Bunny(Vector2 pos, int t,bool mover) : base(pos, t,mover)//Konstruktor, setzt Anfangsposition
        {
            this.mover = mover;
            spine.Load(position, "fluffy", (float)Convert.ToDouble(Game1.luaInstance["bunnySize"]), acceleration);
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["bunnyCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["bunnyCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["bunnyCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["bunnyCollisionHeight"]));
        }
        public override void anim(string anim,int richtung)
        {
            if (anim == "die"||anim == "smash_die")
            {
                Sound.Play("fluffy_dying");
                spine.anim(anim, richtung, false);
                dead = true;
            }
            else
            {
                spine.anim(anim, richtung, true);
            }
            animeTime = 1;
        }
        public override void Update(GameTime gameTime, Map map, Vector2 heropos)
        {
            speed = Convert.ToInt32((double)Game1.luaInstance["bunnySpeed"]);
            if (GameScreen.slow != 0)
            {
                speed = speed /Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
            }
            gravitation = Convert.ToInt32((double)Game1.luaInstance["bunnyGravitation"]);
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
            if(mover)
                spine.anim("walk", 1, true);
            else
                spine.anim("walk", 2, true);
            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
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
