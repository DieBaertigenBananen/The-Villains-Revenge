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
        public int speed = 1;

        public Player() //Konstruktor, setzt Anfangsposition
        {
            pos.X = 0;
            pos.Y = 0;
            lastpos = pos;
            cbox = new Rectangle((int)pos.X, (int)pos.Y, 64, 64);

        }
        public void Load(ContentManager Content)//Wird im Hauptgame ausgeführt und geladen
        {
            playerTexture = Content.Load<Texture2D>("sprites/player"); 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Wird im Hauptgame ausgeführt und malt den Spieler mit der entsprechenden Animation
            spriteBatch.Draw(playerTexture, pos, new Rectangle(0, 0, 64, 64), Color.White);
        }
        public void Update(Map map)
        {
            //Lade Keyboard-Daten
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Right) == true) //Wenn Rechte Pfeiltaste
            {
                Move(speed, 0, map); //Bewege Rechts
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Left) == true)//Wenn Linke Pfeiltaste
            {
                Move(-speed, 0, map);//Bewege Links
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) == true)//Wenn Oben Pfeiltaste
            {
                Move(0, -speed, map);//Bewege Oben
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Down) == true)//Wenn Unten Pfeiltaste
            {
                Move(0, speed, map);//Bewege Unten
            }

            if (currentKeyboardState.IsKeyDown(Keys.A) == true) //Wenn Rechte Pfeiltaste
            {
                speed++;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Y) == true)//Wenn Linke Pfeiltaste
            {
                speed--;
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
                cboxnew.X += ((x / icoll) * i);
                cboxnew.Y += ((y / icoll) * i);
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
