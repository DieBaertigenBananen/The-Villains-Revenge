using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Player
    {
        Vector2 pos; //Position
        Vector2 lastpos; //Position vor vorherigem Update
        Texture2D playerTexture; //Textur
        public Collision cbox; //Collisionsbox

        public Player() //Konstruktor, setzt Anfangsposition
        {
            pos.X = 0;
            pos.Y = 0;
            lastpos = pos;
            cbox = new Collision(pos, 64, 64);

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
        public void Move(int richtung) //Falls Input, bewegt den Spieler
        {
            //Testweise in alle Richtungen um Kollision zu testen
            if (richtung == 1) // rechts
            {
                pos.X++;
            }
            else if (richtung == 2) //links
            {
                pos.X--;
            }
            if (richtung == 3) //oben
            {
                pos.Y--;
            }
            else if (richtung == 4) //unten
            {
                pos.Y++;
            }
            cbox.pos.X = (int)pos.X;
            cbox.pos.Y = (int)pos.Y;
        }
    }
}
