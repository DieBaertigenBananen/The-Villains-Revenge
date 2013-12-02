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
        public Rectangle cbox; //Collisionsbox
        Texture2D enemyTexture; //Textur
        public int speed = 8; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed = 6; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int gravitation = 60; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int jumppower = 20; //Anfangsgeschwindigkeit in m/s _/60
        public double herotime;
        bool start = false;

        public Hero(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            position.X = x;
            position.Y = y;
            cbox = new Rectangle((int)position.X, (int)position.Y, 85, 85);
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
                if (herotime > 10)
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
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
        }

        Vector2 CollisionCheckedVector(int x, int y, List<Block> list)
        {
            Rectangle cboxnew = this.cbox;
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
                cboxnew.X = this.cbox.X + ((x / icoll) * i);
                cboxnew.Y = this.cbox.Y + ((y / icoll) * i);
                //Gehe die Blöcke der Liste durch
                foreach (Block block in list)
                {
                    //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                    if (cboxnew.Intersects(block.cbox))
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
                    move.X = cboxnew.X - cbox.X;
                    move.Y = cboxnew.Y - cbox.Y;
                }
            }
            return move;
        }
    }
}
