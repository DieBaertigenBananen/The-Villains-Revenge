using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TheVillainsRevenge
{
    class Collision
    {
        //Position
        public Vector2 pos;
        //X und Y
        public int width, height;
        public Collision(Vector2 npos,int nWidth,int nHeight)
        {
            //Erstelle Daten
            pos = npos;
            width = nWidth;
            height = nHeight;
        }

        public bool Intersect(Collision box,int seite)
        {
            bool nseite = false;
            //seite = 0 = keine Kollision
            //Seite = 1 = Kollision Links
            //Seite = 2 = Kollision Rechts
            //Seite = 3 = Kollision Oben
            //Seite = 4 = Kollision Unten

            //pos = (10,10) width,height = (64,64)
            //box.pos = (10,30) width,height = (64,64)


            if (seite == 1)
            {
                if (pos.X <= (box.width + box.pos.X) && pos.X >= (box.width + box.pos.X - 2))
                {
                    if (
                        pos.Y >= box.pos.Y && pos.Y <= (box.pos.Y + box.height)
                        ||
                        (pos.Y + height) >= box.pos.Y && (pos.Y + height) <= (box.pos.Y + box.height)
                       )
                    {
                        nseite = true;
                    }
                }
            }
            else if (seite == 2)
            {
                if ((pos.X + width) <= box.pos.X && (pos.X + width) >= box.pos.X - 2)
                {
                    if (
                        pos.Y >= box.pos.Y && pos.Y <= (box.pos.Y + box.height)
                        ||
                        (pos.Y + height) >= box.pos.Y && (pos.Y + height) <= (box.pos.Y + box.height)
                       )
                    {
                        nseite = true;
                    }
                }
            }
            else if (seite == 3)
            {
                if (pos.Y <= (box.height + box.pos.Y) && pos.Y >= (box.height + box.pos.Y - 2))
                {
                    if (
                        pos.X >= box.pos.X && pos.X <= (box.pos.X + box.width)
                        ||
                        (pos.X + width) >= box.pos.X && (pos.X + width) <= (box.pos.X + box.width)
                       )
                    {
                        nseite = true;
                    }
                }
            }
            else if (seite == 4)
            {
                if ((pos.Y + height) <= box.pos.Y && (pos.Y + height) >= box.pos.Y - 2)
                {
                    if (
                        pos.X >= box.pos.X && pos.X <= (box.pos.X + box.width)
                        ||
                        (pos.X + width) >= box.pos.X && (pos.X + width) <= (box.pos.X + box.width)
                       )
                    {
                        nseite = true;
                    }
                }
            }


            return nseite;
        }

                //bool cr = false;
                //bool cl = false;
                //bool ct = false;
                //bool cb = false;
                ////Gehe die Blöcke der Karte durch
                //foreach (Block block in karte.blocks)
                //{
                //    //Eigene Collision , wenn Kollision nicht gleich 0 ,eine Seite also kollidiert
                //    cl = spieler.cbox.Intersect(block.cbox, 1);
                //    cr = spieler.cbox.Intersect(block.cbox, 2);
                //    ct = spieler.cbox.Intersect(block.cbox, 3);
                //    cb = spieler.cbox.Intersect(block.cbox, 4);
                //}

    }
}
