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
        Vector2 pos; //Position
        Vector2 lastpos; //Position vor vorherigem Update
        Texture2D playerTexture; //Textur
        public Rectangle cbox; //Collisionsbox
        public int speed = 6; //Bewegungsgeschwindigkeit in m/s _/60
        public bool jump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int jumppower = 20; //Anfangsgeschwindigkeit in m/s _/60
        public int gravitation = 60; //Erdbeschleunigung in (m/s)*(m/s) _/60

        public Player() //Konstruktor, setzt Anfangsposition
        {
            pos.X = 0;
            pos.Y = 0;
            lastpos = pos;
            cbox = new Rectangle((int)pos.X, (int)pos.Y, 128, 128);

        }
        public void Load(ContentManager Content)//Wird im Hauptgame ausgeführt und geladen
        {
            playerTexture = Content.Load<Texture2D>("sprites/player"); 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Wird im Hauptgame ausgeführt und malt den Spieler mit der entsprechenden Animation
            spriteBatch.Draw(playerTexture, pos, new Rectangle(0, 0, 128, 128), Color.White);
        }
        public void Update(GameTime gameTime, Map map)
        {
            //Lade Keyboard-Daten
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0f
                ||
                currentKeyboardState.IsKeyDown(Keys.Right) == true 
                || 
                currentKeyboardState.IsKeyDown(Keys.D) == true
                ) //Wenn Rechte Pfeiltaste
            {
                Move(speed, 0, map); //Bewege Rechts
            }
            if (
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0f
                ||
                currentKeyboardState.IsKeyDown(Keys.Left) == true 
                || 
                currentKeyboardState.IsKeyDown(Keys.A) == true
                ) //Wenn Rechte Pfeiltaste
            {
                Move(-speed, 0, map);//Bewege Links
            }
            if (
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0f
                ||
                currentKeyboardState.IsKeyDown(Keys.Space) == true
                )
            {
                if (!jump && !fall)
                {
                    Jump(gameTime, map); //Springen! Deine Mudda springt bei Doodle Jump nach unten.
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) == true || GamePad.GetState(PlayerIndex.One).Triggers.Right == 1.0f) //Wenn Rechte Pfeiltaste
            {
                speed++;
            }
            if (currentKeyboardState.IsKeyDown(Keys.LeftControl) == true || GamePad.GetState(PlayerIndex.One).Triggers.Left == 1.0f)//Wenn Linke Pfeiltaste
            {
                speed--;
            }  

            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks).Y > 0 && !jump)
            {
                if (!fall)
                {
                    fall = true;
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - falltimer)/1000);
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

        public void Jump(GameTime gameTime, Map map)
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
            pos.X += domove.X;
            pos.Y += domove.Y;
            cbox.X = (int)pos.X;
            cbox.Y = (int)pos.Y;
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
                cboxnew.X = this.cbox.X + ((x / icoll)*i);
                cboxnew.Y = this.cbox.Y + ((y / icoll)*i);
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
