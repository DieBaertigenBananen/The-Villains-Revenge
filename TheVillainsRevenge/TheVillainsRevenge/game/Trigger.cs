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
        public Rectangle cbox = new Rectangle(0, 0, 48, 48); //Collisionsbox
        public Rectangle triggerend = new Rectangle(0, 0, 48, 48); //Collisionsbox
        public Rectangle doorstart = new Rectangle(0, 0, 48, 48); //Collisionsbox
        public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
        Block b;
        bool pushed;
        int typ;
        double spawntime;
        //Checkpoint//
        double checkTime;
        int checktyp;
        bool checkpushed;
        bool checkactive;
        int checkactiveTime;
        bool checkblock;
        public Trigger(Vector2 npos,Block b)
        {
            //Setze Position und Collisionsbox
            this.b = b;
            position = npos;
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
            active = false;
            activeTime = 0;
            checkpushed = pushed;
            checkactive = active;
            checktyp = typ;
            checkblock = b.block;
        }
        public void Reset(List<Block> list, List<Enemy> elist)
        {
            if (checktyp != 0)
            {
                if (checktyp == 1)
                {
                    if (checkactive)
                    {
                        //Baue Wand komplett auf
                        Rectangle cboxnew = triggerend;
                        for (int i = 0; i < 20; i++)
                        {
                            cboxnew.Y -= 48;
                            bool end = false;
                            bool collide = false;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = blocks.ElementAt(j);
                                if(block.type == "triggerend"&&block.cbox.Intersects(cboxnew))
                                {
                                    end = true;
                                    break;
                                }
                                else if (block.cbox.Intersects(cboxnew) && block.block)
                                {
                                    collide = true;
                                }
                            }
                            if (end)
                                break;
                            else if(!collide)
                            {
                                Block block = new Block(new Vector2(cboxnew.X, cboxnew.Y), "underground_earth");
                                list.Add(block);
                                blocks.Add(block);
                            }
                        }
                    }
                    else
                    {
                        //Zerstöre alle Blöcke
                        for (int i = 0; i < blocks.Count(); ++i)
                        {
                            Block block = blocks.ElementAt(i);
                            list.Remove(block);
                        }
                        blocks.Clear();
                    }
                }
                else if (checktyp == 2)
                {
                    if (checkactive)
                    {
                        //Zerstöre alle Blöcke
                        Rectangle cboxnew = triggerend;
                        for (int i = 0; i < 20; i++)
                        {
                            cboxnew.Y += 48;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = blocks.ElementAt(j);
                                if (block.cbox.Intersects(cboxnew) && block.type == "triggerdoor")
                                {
                                    list.Remove(block);
                                }

                            }
                        }

                    }
                    else
                    {
                        //Baue die Wand komplett auf
                        Rectangle cboxnew = triggerend;
                        for (int i = 0; i < 20; i++)
                        {
                            cboxnew.Y += 48;
                            bool collide = false;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = blocks.ElementAt(j);
                                if (block.cbox.Intersects(cboxnew) && block.block)
                                {
                                    collide = true;
                                }

                            }
                            if (!collide)
                            {
                                Block blockn = new Block(new Vector2(cboxnew.X, cboxnew.Y), "triggerdoor");
                                list.Add(blockn);
                            }
                        }
                    }
                }

            }
            else
            {
                //Wurden schonmal gedrückt aber nicht gesavet
                if (typ == 1) 
                {
                    //Zerstöre Wand
                    for (int i = 0; i < blocks.Count(); ++i)
                    {
                        Block block = blocks.ElementAt(i);
                        list.Remove(block);
                    }
                    blocks.Clear();
                }
                //Typ 2 wird automatisch aufgebaut
            }
            active = checkactive;
            pushed = checkpushed;
            time = checkTime;
            activeTime = checkactiveTime;
            typ = checktyp;
            b.block = checkblock;
        }
        public void Save()
        {
            checkpushed = pushed;
            checkactive = active;
            checkactiveTime = activeTime;
            checkTime = time;
            checktyp = typ;
            checkblock = b.block;
        }
        public void Pushed(List<Block> list,Rectangle hero)
        {
            //Ermittle Typ
            if (typ == 0)
            {
                bool oben = false;
                //Schaue wo die Blöcke sind
                Rectangle cboxnew = new Rectangle((int)cbox.X, (int)cbox.Y, cbox.Width, cbox.Height);
                //Gehe nach links um die X-Distanz herauszufidnen
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
                            triggerend.Y = block.cbox.Y;
                            hat = true;
                            break;
                        }
                    }
                    if (hat)
                        break;
                }
                //X-Distanz gefunden, in triggerend gespeichert
                //Gehe nach Oben
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
                    typ = 1; //Wand
                else
                    typ = 2; //Tür
            }
            //Triggerend und typ ermittelt
            if (!pushed&&!active)
            {
                if (typ == 1) //Wenn es eine Wand ist
                {
                    //Gucke ob held da ist
                    bool heroda = false;
                    Rectangle cboxnew = triggerend;
                    for (int i = 0; i < 20; i++)
                    {
                        cboxnew.Y = cboxnew.Y - 48;
                        if (cboxnew.Intersects(hero))
                        {
                            //Wir haben alle Daten
                            heroda = true;
                            break;
                        }
                    }
                    if (!heroda)
                    {
                        pushed = true;
                        active = true;
                        b.block = false;
                    }
                }
                else if (typ == 2) //Wenn es eine Tür ist
                {
                    pushed = true;
                    active = true;
                    b.block = false;
                }
            }
            /*
            if (oben&&!heroblock)
            {
                //Oben ist ein Block und auf den Weg ist der Held nicht, daher erzeuge Blöcke
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
            else if(!oben)
            {
                heroblock = false;
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
            if (!heroblock)
            {
                time = 0;
                active = true;
                b.block = false;
            }
             * */
        }
        public void Update(GameTime gameTime, List<Block> list,List<Enemy> elist,Rectangle sbox,Rectangle hbox)
        {
            if (pushed)
            {
                if (typ == 2)
                {
                    spawntime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (spawntime > 0.1)
                    {
                        //Entferne die Blöcke unten
                        Rectangle cboxnew = triggerend;
                        doorstart.X = cboxnew.X;
                        for (int i = 0; i < 20; i++)
                        {
                            bool hat = false;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = list.ElementAt(j);
                                if(block.cbox.Intersects(cboxnew)&&block.type == "triggerdoor")
                                {
                                    doorstart.Y = block.cbox.Y;
                                    list.Remove(block);
                                    hat = true;
                                    break;

                                }
                            }
                            if (hat)
                                break;
                            else if (i == 19)
                            {
                                Console.WriteLine("geht");
                                pushed = false;
                                activeTime = Convert.ToInt32((double)Game1.luaInstance["triggerTimeDoor"]);
                                cboxnew.Y = doorstart.Y;
                                cboxnew.X = doorstart.X;
                                for (int a = 0; a < 10; a++)
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
                            cboxnew.Y = cboxnew.Y + 48;
                        }
                        spawntime = 0;

                    }
                }
                else if (typ == 1)
                {
                    spawntime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (spawntime > 0.1)
                    {
                        Rectangle cboxnew = triggerend;
                        for (int i = 0; i < 20; i++)
                        {
                            bool triggerblock = false;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = list.ElementAt(j);
                                for (int a = 0; a < blocks.Count(); ++a)
                                {
                                    Block blocka = blocks.ElementAt(a);
                                    if (blocka == block && block.cbox.Intersects(cboxnew))
                                    {
                                        triggerblock = true;
                                        break;
                                    }
                                }
                                if (cboxnew.Intersects(block.cbox) && block.block && !triggerblock || i == 20 || cboxnew.Intersects(block.cbox) && block.type == "triggerend" && !block.cbox.Intersects(triggerend))
                                {
                                    //Wir haben alle Daten
                                    pushed = false;
                                    activeTime = Convert.ToInt32((double)Game1.luaInstance["triggerTimeWall"]);
                                    break;
                                }
                            }
                            if (!pushed)
                                break;
                            else if (!triggerblock)
                            {
                                Block block = new Block(new Vector2(cboxnew.X, cboxnew.Y), "underground_earth");
                                list.Add(block);
                                blocks.Add(block);
                                spawntime = 0;
                                break;
                            }
                            cboxnew.Y = cboxnew.Y - 48;
                        }
                    }
                }
            }
            else if(active&&!sbox.Intersects(b.cbox))
            {
                time += gameTime.ElapsedGameTime.TotalSeconds;
                if (time > activeTime)
                {
                    if (typ == 2)
                    {
                        spawntime += gameTime.ElapsedGameTime.TotalSeconds;
                        if (spawntime > 0.1)
                        {
                            Block blockn = new Block(new Vector2(doorstart.X, doorstart.Y), "triggerdoor");
                            list.Add(blockn);
                            doorstart.Y -= 48;
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = list.ElementAt(j);
                                if (block.cbox.Intersects(doorstart) && block.block)
                                {
                                    b.block = true;
                                    active = false;
                                    time = 0;
                                    break;
                                }
                            }
                            spawntime = 0;
                        }
                    }
                    if (typ == 1)
                    {
                        for (int i = 0; i < blocks.Count(); ++i)
                        {
                            Block block = blocks.ElementAt(i);
                            list.Remove(block);
                        }
                        blocks.Clear();
                        b.block = true;
                        active = false;
                        time = 0;
                    }
                }
            }
            /*
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
                    bool blocker = false;
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
                            if (cboxnew.Intersects(hbox) || cbox.Intersects(sbox))
                            {
                                blocker = true;
                                break;
                            }
                        }
                        if (!blocker)
                        {
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
                    }
                    if (!blocker)
                    {
                        active = false;
                        b.block = true;
                    }
                }
            }*/
        }
    }
}
