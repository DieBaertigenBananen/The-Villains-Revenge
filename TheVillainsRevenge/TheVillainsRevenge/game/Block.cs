using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Block
    {
        public Vector2 position; //Position
        public Rectangle cbox = new Rectangle(0,0,48,48); //Collisionsbox
        public Rectangle cuttexture = new Rectangle(0,0,48,48);
        public bool block;
        public string type;
        public bool inlist;
        //Checkpoint//
        Vector2 checkpoint;

        public void Reset()
        {
            position.Y = checkpoint.Y;
            position.X = checkpoint.X;
        }
        public void Save()
        {
            checkpoint.X = position.X;
            checkpoint.Y = position.Y;
        }
        public Block(Vector2 npos, string type)
        {
            //Setze Position und Collisionsbox
            this.type = type;
            position = npos;
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
            checkpoint.X = position.X;
            checkpoint.Y = position.Y;
            block = true;
            //Je nach Blocktyp Ausschnitt aus Textur und größe der Kollisionsbox anpassen
            switch (type)
            {
                case "movingend":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    cuttexture.Width = 0;
                    cuttexture.Height = 0;
                    block = false;
                    break;
                case "moving":
                    cuttexture.X = 48;
                    cuttexture.Y = 0;
                    break;
                case "triggerend":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    cuttexture.Width = 0;
                    cuttexture.Height = 0;
                    block = false;
                    break;
                case "trigger": //Triggerblock, nur zum blocken
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    cuttexture.Width = 0;
                    cuttexture.Height = 0;
                    cbox.Height = 46;
                    cbox.Y = (int)position.Y + 2;
                    break;
                case "underground_earth":
                    cuttexture.X = 0;
                    cuttexture.Y = 0;
                    break;
                case "ground_grass":
                    cuttexture.X = 48;
                    cuttexture.Y = 0;
                    break;
                case "platform_grass":
                    cuttexture.X = 2 * 48;
                    cuttexture.Y = 0;
                    break;
                case "water":
                    cuttexture.X = 3 * 48;
                    cuttexture.Y = 0;
                    block = false;
                    break;
                case "underground_rock":
                    cuttexture.X = 0;
                    cuttexture.Y = 48;
                    break;
                case "ground_rock":
                    cuttexture.X = 48;
                    cuttexture.Y = 48;
                    break;
                case "platform_rock":
                    cuttexture.X = 2 * 48;
                    cuttexture.Y = 48;
                    break;
                case "lava":
                    cuttexture.X = 3 * 48;
                    cuttexture.Y = 48;
                    break;
            }

        }
    }
}
