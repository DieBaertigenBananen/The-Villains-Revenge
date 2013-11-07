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
        public void Move(int deltax, int deltay, List<Block> list) //Falls Input, bewegt den Spieler
        {
            if (!Collision(deltax, deltay, list))
                {
                    pos.X += deltax;
                    pos.Y += deltay;
                }
            cbox.X = (int)pos.X;
            cbox.Y = (int)pos.Y;
        }

        public bool Collision(int x, int y, List<Block> list)
        {
            Rectangle cboxnew = this.cbox;
            cboxnew.X += x;
            cboxnew.Y += y;
            bool check = false;
            //Gehe die Blöcke der Liste durch
            foreach (Block block in list)
            {
                if (cboxnew.Intersects(block.cbox))
                {
                    check = true;
                }
            }
            return check;
        }
    }
}
