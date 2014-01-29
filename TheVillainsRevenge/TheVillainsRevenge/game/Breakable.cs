using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class Breakable
    {
        public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
        public Rectangle cbox = new Rectangle(0, 0, 48, 48);
        public bool vertikal;
        public int id;
        public Breakable(List<Block> list,bool vertikal,int id)
        {
            this.vertikal = vertikal;
            this.id = id;
            int x = 0;
            int y = 0;
            for (int i = 0; i < list.Count(); ++i)
            {
                Block block = list.ElementAt(i);
                if (block.type == "breakable" && !block.inlist && !vertikal || block.type == "breakable_verticale" && !block.inlist && vertikal)
                {
                    x = block.cbox.X;
                    y = block.cbox.Y;
                    this.cbox.X = block.cbox.X;
                    this.cbox.Y = block.cbox.Y;
                    blocks.Add(block);
                    block.inlist = true;
                    cbox = block.cbox;
                    break;
                }
            }
            if (!vertikal)
            {
                for (int j = 0; j < 10; ++j)
                {
                    cbox.X = cbox.X + 48;
                    for (int i = 0; i < list.Count(); ++i)
                    {
                        Block block = list.ElementAt(i);
                        if (cbox.Intersects(block.cbox) && block.type == "breakable" && !block.inlist)
                        {
                            blocks.Add(block);
                            block.inlist = true;
                            Console.WriteLine("Add Block: " + block.position.X + " " + block.position.Y);
                        }
                    }
                }
                this.cbox.Width = blocks.Count * 48;
                this.cbox.X = x;
            }
            else
            {
                for (int j = 0; j < 10; ++j)
                {
                    cbox.Y = cbox.Y + 48;
                    for (int i = 0; i < list.Count(); ++i)
                    {
                        Block block = list.ElementAt(i);
                        if (cbox.Intersects(block.cbox) && block.type == "breakable_verticale" && !block.inlist)
                        {
                            blocks.Add(block);
                            block.inlist = true;
                        }
                    }
                }
                this.cbox.Height = blocks.Count * 48;
                this.cbox.Y = y;
            }
        }
    }
}
