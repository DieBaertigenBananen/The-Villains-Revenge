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
        Texture2D enemyTexture; //Textur
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int jumppower; //Anfangsgeschwindigkeit in m/s _/60
        public double herotime;
        bool start = false;
        int heroStartTime;

        public Hero(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            position.X = x;
            position.Y = y;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionHeight"]));
            heroStartTime = Convert.ToInt32((double)Game1.luaInstance["heroStartTime"]);
        }
        public void Load(ContentManager Content)//Wird im Hauptgame ausgeführt und geladen
        {
                enemyTexture = Content.Load<Texture2D>("sprites/held");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Wird im Hauptgame ausgeführt und malt den Spieler mit der entsprechenden Animation
            if(start)
                spriteBatch.Draw(enemyTexture, position, new Rectangle(0, 0, 85, 85), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
        public void Update(GameTime gameTime, Map map,Vector2 sposition)
        {
            if (start)
            {
                speed = Convert.ToInt32((double)Game1.luaInstance["heroSpeed"]);
                if (GameScreen.slow != 0)
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

                if (sposition.X > position.X)
                {
                    Move(actualspeed, 0, map); //Bewege Rechts
                }
                else if (sposition.X < position.X)
                {
                    Move(-actualspeed, 0, map); //Bewege Rechts
                }
                if (sposition.Y + 50 < position.Y)
                {
                    if (!jump && !fall)
                    {
                        Jump(gameTime, map); //Springen!
                    }
                }
                if (CollisionCheckedVector(0, 1, map.blocks).Y > 0 && !jump)
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

                //Sprung fortführen
                if (jump)
                {
                    Jump(gameTime, map);
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
