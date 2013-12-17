using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class MovingBlock
    {
        public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
        public int move = 1;
        public int movespeed = 0;
        public Rectangle cbox =  new Rectangle(0, 0, 48, 48);
        public MovingBlock(List<Block> list)
        {
            int x = 0;
            for (int i = 0; i < list.Count(); ++i)
            {
                Block block = list.ElementAt(i);
                if (block.type == "moving"&&!block.inlist)
                {
                    x = block.cbox.X;
                    this.cbox.X = block.cbox.X;
                    this.cbox.Y = block.cbox.Y;
                    blocks.Add(block);
                    block.inlist = true;
                    cbox = block.cbox;
                    break;
                }
            }
            for (int j = 0; j < 10; ++j)
            {
                cbox.X = cbox.X + 48;
                for (int i = 0; i < list.Count(); ++i)
                {
                    Block block = list.ElementAt(i);
                    if (cbox.Intersects(block.cbox) &&block.type == "moving"&&!block.inlist)
                    {
                        blocks.Add(block);
                        block.inlist = true;
                    }
                }
            }
            this.cbox.Width = blocks.Count * 48;
            this.cbox.X = x;
        }
        public void Update(GameTime gameTime, List<Block> list)
        {
            movespeed = Convert.ToInt32((double)Game1.luaInstance["blockSpeed"]);
            if (GameScreen.slow != 0)
            {
                movespeed = movespeed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
            }
            if (move == 2)
                movespeed = -movespeed;
            bool collide = false;
            Rectangle cboxnew = new Rectangle((int)cbox.X + movespeed, (int)cbox.Y, cbox.Width, cbox.Height);
            for (int i = 0; i < list.Count(); ++i)
            {
                Block block = list.ElementAt(i);
                if (cboxnew.Intersects(block.cbox) &&block.type == "movingend")
                {
                    if (move == 1)
                        move = 2;
                    else
                        move = 1;
                    collide = true;
                    break;
                }
            }
            if (!collide)
            {
                cbox = cboxnew;
                for (int i = 0; i < blocks.Count(); ++i)
                {
                    Block block = blocks.ElementAt(i);
                    block.cbox.X += movespeed;
                    block.position.X += movespeed;
                }
            }
        }
    }
}
