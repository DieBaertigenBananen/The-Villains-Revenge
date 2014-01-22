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
        //Checkpoint//
        bool checkactive;
        double checkactiveTime;
        public Trigger(Vector2 npos,Block b)
        {
            //Setze Position und Collisionsbox
            this.b = b;
            position = npos;
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
            cuttexture.X = 3 * 48;
            cuttexture.Y = 48;
            active = false;
            checkactive = false;
            activeTime = 0;
            checkactiveTime = 0;
        }
        public void Reset(List<Block> list, List<Enemy> elist)
        {
            if (!active && checkactive)
            {
                Pushed(list,elist);
                time = checkactiveTime;
            }
            else if (active && !checkactive)
            {
                activeTime = 0;
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
        public void Save()
        {
            active = checkactive;
            time = checkactiveTime;
        }
        public void Pushed(List<Block> list,List<Enemy> elist)
        {
            //Schaue wo die Blöcke sind
            Rectangle cboxnew = new Rectangle((int)cbox.X, (int)cbox.Y, cbox.Width, cbox.Height);
            Rectangle triggerend = new Rectangle((int)cbox.X, (int)cbox.Y, cbox.Width, cbox.Height);
            for (int i = 0; i < 20; i++)
            {
                cboxnew.X = cboxnew.X - 48;
                bool hat = false;
                for (int j = 0; j < list.Count(); ++j)
                {
                    Block block = list.ElementAt(j);
                    if (cboxnew.Intersects(block.cbox) && block.type == "triggerend")
                    {
                        //Wir haben den Block, jetzt hole die Höhe
                        triggerend.X = block.cbox.X;
                        hat = true;
                        break;
                    }
                }
                if (hat)
                    break;
            }
            //Gehe nach Oben
            bool oben = false;
            for (int i = 0; i < 20; i++)
            {
                cboxnew.Y = cboxnew.Y - 48;
                for (int j = 0; j < list.Count(); ++j)
                {
                    Block block = list.ElementAt(j);
                    if (cboxnew.Intersects(block.cbox) && block.type == "triggerend")
                    {
                        //Wir haben alle Daten
                        oben = true;
                        break;
                    }
                }
                if (oben)
                    break;
            }
            if (oben)
            {
                activeTime = Convert.ToInt32((double)Game1.luaInstance["triggerTimeWall"]);
                //Haben nun alle Daten, nun platziere Blöcke bis unten ne Kollision ist
                for (int i = 0; i < 20; i++)
                {
                    cboxnew.Y = cboxnew.Y + 48;
                    bool collide = false;
                    for (int j = 0; j < list.Count(); ++j)
                    {
                        Block block = list.ElementAt(j);
                        if (cboxnew.Intersects(block.cbox) && block.block)
                        {
                            //Wir haben alle Daten
                            collide = true;
                            break;
                        }
                    }
                    if (collide)
                        break;
                    else
                    {
                        Block block = new Block(new Vector2(cboxnew.X, cboxnew.Y), "underground_earth");
                        list.Add(block);
                        blocks.Add(block);
                        b.block = false;
                    }
                }
            }
            else
            {
                activeTime = Convert.ToInt32((double)Game1.luaInstance["triggerTimeDoor"]);
                //Es ist kein Block oben, daher ist es eine Tür unten
                cboxnew.Y = triggerend.Y;
                for (int i = 0; i < 20; i++)
                {
                    cboxnew.Y = cboxnew.Y + 48;
                    for (int j = 0; j < list.Count(); ++j)
                    {
                        Block block = list.ElementAt(j);
                        if (cboxnew.Intersects(block.cbox) && block.type == "triggerdoor")
                        {
                            //Wir haben alle Daten
                            list.Remove(block);
                            triggerend.Y = block.cbox.Y;
                        }
                    }
                }
                cboxnew.Y = triggerend.Y;
                for (int i = 0; i < 10; i++)
                {
                    cboxnew.X = cboxnew.X + 48;
                    for (int j = 0; j < elist.Count(); ++j)
                    {
                        Enemy enemy = elist.ElementAt(j);
                        if (cboxnew.Intersects(enemy.cbox.box) && enemy.type == 2)
                        {
                            enemy.moving = true;
                            enemy.mover = false;
                        }
                    }
                }
            }
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

                    //Schaue wo die Blöcke sind
                    Rectangle cboxnew = new Rectangle((int)cbox.X, (int)cbox.Y, cbox.Width, cbox.Height);
                    Rectangle triggerend = new Rectangle((int)cbox.X, (int)cbox.Y, cbox.Width, cbox.Height);
                    for (int i = 0; i < 20; i++)
                    {
                        cboxnew.X = cboxnew.X - 48;
                        bool hat = false;
                        for (int j = 0; j < list.Count(); ++j)
                        {
                            Block block = list.ElementAt(j);
                            if (cboxnew.Intersects(block.cbox) && block.type == "triggerend")
                            {
                                //Wir haben den Block, jetzt hole die Höhe
                                triggerend.X = block.cbox.X;
                                hat = true;
                                break;
                            }
                        }
                        if (hat)
                            break;
                    }
                    //Gehe nach Oben
                    bool oben = false;
                    for (int i = 0; i < 20; i++)
                    {
                        cboxnew.Y = cboxnew.Y - 48;
                        for (int j = 0; j < list.Count(); ++j)
                        {
                            Block block = list.ElementAt(j);
                            if (cboxnew.Intersects(block.cbox) && block.type == "triggerend")
                            {
                                //Wir haben alle Daten
                                oben = true;
                                break;
                            }
                        }
                        if (oben)
                            break;
                    }
                    if (oben)
                    {
                        for (int i = 0; i < blocks.Count(); ++i)
                        {
                            Block block = blocks.ElementAt(i);
                            list.Remove(block);
                        }
                        blocks.Clear();
                    }
                    else
                    {
                        bool start = false;
                        cboxnew.Y = triggerend.Y;
                        for (int i = 0; i < 20; i++)
                        {
                            cboxnew.Y = cboxnew.Y + 48;
                            bool istda = false;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = list.ElementAt(j);
                                if (cboxnew.Intersects(block.cbox))
                                {
                                    //Wir haben alle Daten
                                    istda = true;
                                }
                            }
                            if (!istda)
                            {
                                start = true;
                                Block block = new Block(new Vector2(cboxnew.X, cboxnew.Y), "triggerdoor");
                                list.Add(block);
                            }
                            else if (start)
                            {
                                break;
                            }
                        }
                    }
                    active = false;
                    b.block = true;
                }
            }
        }
    }
}
