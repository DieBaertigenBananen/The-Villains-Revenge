using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class Trigger
    {
        public Vector2 position; //Position
        public bool active;
        public int activeTime;
        public double time;
        public Rectangle cuttexture = new Rectangle(0, 0, 48, 48);
        public Rectangle cbox = new Rectangle(0, 0, 48, 48); //Collisionsbox
        public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
        Block b;
        int blocka;
        int blocky;
        int blockx;
        public Trigger(Vector2 npos,Block b)
        {
            //Setze Position und Collisionsbox
            this.b = b;
            position = npos;
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
            cuttexture.X = 3 * 48;
            cuttexture.Y = 48;
            blocka = 11;
            blocky = 5;
            blockx = 6;
            active = false;
        }
        public void Pushed(List<Block> list)
        {
             //generiere Das Level (erzeuge neue Objekte in der List) anhand der Levelmap
            for (int i = 0; i < blocka; i++)
            {
                Block block = new Block(new Vector2(position.X - (blockx * 48), position.Y - (blocky * 48) + i * 48), "underground_earth");
                list.Add(block);
                blocks.Add(block);
                b.block = false;
            }
            activeTime = 10;
            time = 0;
            active = true;
        }
        public void Update(GameTime gameTime, List<Block> list,Rectangle sbox)
        {
            if (active&&!sbox.Intersects(b.cbox))
            {
                time += gameTime.ElapsedGameTime.TotalSeconds;
                if (time > activeTime)
                {
                    for (int i = 0; i < blocks.Count(); ++i)
                    {
                        Block block = blocks.ElementAt(i);
                        list.Remove(block);
                    }
                    blocks.Clear();
                    active = false;
                    b.block = true;
                }
            }
        }
    }
}
