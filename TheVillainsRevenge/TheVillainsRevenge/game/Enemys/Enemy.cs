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
        public double animeTime = 0;
        public bool dead = false;
        public Spine spine;
        public Vector2 position; //Position
        public CollisionBox cbox;
        public int type;
        public bool fall = false;
        public double falltimer;
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public bool mover = false;
        //Checkpoint//
        bool checkmover;
        public Vector2 checkpoint;

        public void Reset()
        {
            mover = checkmover;
            position.Y = checkpoint.Y;
            position.X = checkpoint.X;
            cbox.Update(position);
        }
        public void Save()
        {
            checkmover = mover;
            checkpoint.X = position.X;
            checkpoint.Y = position.Y;
        }
        public Enemy(Vector2 pos, int t) //Konstruktor, setzt Anfangsposition
        {
            spine = new Spine();
            checkmover = mover;
            checkpoint = new Vector2(pos.X, pos.Y);
            position = pos;
            type = t;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["enemyCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["enemyCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["enemyCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["enemyCollisionHeight"]));
       
        }
        public virtual void Update(GameTime gameTime, Map map, Vector2 heropos)
        {
        }
        public virtual void Load(ContentManager Content)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, Camera camera)
        {
            spine.Draw(gameTime, camera, position);
        }

        public void Move(int deltax, int deltay, Map map) //Falls Input, bewegt den Spieler
        {
            Vector2 domove = new Vector2(0, 0);
            domove = CollisionCheckedVector(deltax, deltay, map.blocks);
            position.X += domove.X;
            position.Y += domove.Y;
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
