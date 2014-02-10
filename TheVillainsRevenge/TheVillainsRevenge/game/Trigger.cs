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
        public Vector2 wallposition = new Vector2(0, 0);
        public int wallY = 0;
        public int doorframe = 0;
        Block b;
        bool pushed;
        public int typ;
        //Checkpoint//
        double checkTime;
        int checktyp;
        bool checkpushed;
        bool checkactive;
        int checkactiveTime;
        bool checkblock;
        //Start//
        double startTime;
        int starttyp;
        bool startpushed;
        bool startactive;
        int startactiveTime;
        bool startblock;
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
                                Block block = list.ElementAt(j);
                                if(block.type == "triggerend"&&block.cbox.Intersects(cboxnew))
                                {
                                    wallposition = block.position;
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
                                Block block = list.ElementAt(j);
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
                                Block block = list.ElementAt(j);
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
        public void StartReset(List<Block> list, List<Enemy> elist)
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
            active = startactive;
            pushed = startpushed;
            time = startTime;
            activeTime = startactiveTime;
            typ = starttyp;
            b.block = startblock;
        }
        public void StartSave()
        {
            startpushed = pushed;
            startactive = active;
            startactiveTime = activeTime;
            startTime = time;
            starttyp = typ;
            startblock = b.block;
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
        public void Check(List<Block> list)
        {
            //Ermittle Typ
            if (typ == 0)
            {
                bool oben = false;
                //Schaue wo die Blöcke sind
                Rectangle cboxnew = new Rectangle((int)cbox.X, (int)cbox.Y, cbox.Width, cbox.Height);
                //Gehe nach links um die X-Distanz herauszufidnen
                bool hat = false;
                for (int i = 0; i < 20; i++)
                {
                    cboxnew.X = cboxnew.X - 48;
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
                if (!hat)
                {
                    cboxnew.X = cbox.X;
                    for (int i = 0; i < 20; i++)
                    {
                        cboxnew.X = cboxnew.X + 48;
                        for (int j = 0; j < list.Count(); ++j)
                        {
                            Block block = list.ElementAt(j);
                            if (cboxnew.Intersects(block.cbox) && block.type == "triggerdoor")
                            {
                                //Wir haben den Block, jetzt hole die Höhe
                                triggerend.X = block.cbox.X;
                                triggerend.Y = block.cbox.Y + 48;
                                doorstart.Y = block.cbox.Y;
                                doorstart.X = block.cbox.X;
                                wallposition.X = triggerend.X;
                                wallposition.Y = triggerend.Y;
                                hat = true;
                                break;
                            }
                        }
                        if (hat)
                            break;
                    }
                    typ = 2;
                }
                else
                {
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
                    {
                        cboxnew = triggerend;
                        doorstart.X = cboxnew.X;
                        wallposition.X = triggerend.X;
                        wallposition.Y = triggerend.Y;
                        for (int i = 0; i < 20; i++)
                        {
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = list.ElementAt(j);
                                if (block.cbox.Intersects(cboxnew) && block.type == "triggerdoor")
                                {
                                    doorstart.Y = block.cbox.Y;
                                }
                            }
                            cboxnew.Y = cboxnew.Y + 48;
                        }
                        typ = 2; //Tür
                    }
                }
            }
        }
        public void Pushed(List<Block> list,Rectangle hero)
        {
            //Triggerend und typ ermittelt
            if (!pushed&&!active)
            {
                Console.WriteLine(typ);
                Sound.Play("triggerwall");
                if (typ == 1) //Wenn es eine Wand ist
                {
                    //Gucke ob held da ist
                    bool heroda = false;
                    Rectangle cboxnew = triggerend;
                    wallposition.X = cboxnew.X;
                    wallposition.Y = cboxnew.Y;
                    wallY = 0;
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
                    doorframe = 0;
                }
            }
        }
        public void Update(GameTime gameTime, List<Block> list,List<Enemy> elist,Rectangle sbox,Rectangle hbox)
        {
            int speed = 4;
            if (pushed)
            {
                if (typ == 2)
                {
                    if (doorframe != 20)
                    {
                        doorframe += 1;
                    }
                    if (doorframe == 20)
                    {
                        Rectangle cboxnew = doorstart;
                        //Wenn es keine Blöcke existieren
                        for (int i = 0; i < 20; i++)
                        {
                            for (int j = 0; j < list.Count(); ++j)
                            {
                                Block block = list.ElementAt(j);
                                if (block.cbox.Intersects(cboxnew) && block.type == "triggerdoor")
                                {
                                    //Füge existierenden zur Liste hinzu
                                    list.Remove(block);
                                }
                            }
                            cboxnew.Y = cboxnew.Y - 48;
                        }
                        if (Game1.level == 4)
                        {
                            Rectangle dposition = doorstart;
                            dposition.X = (int)position.X;
                            dposition.Y = (int)position.Y+48;
                            bool collide = false;
                            for (int i = 0; i < 20; i++)
                            {
                                collide = false;
                                for (int j = 0; j < list.Count(); ++j)
                                {
                                    Block block = list.ElementAt(j);
                                    if (block.cbox.Intersects(dposition))
                                    {
                                        //Füge existierenden zur Liste hinzu
                                        collide = true;
                                    }
                                }
                                if (!collide)
                                {
                                    Block block2 = new Block(new Vector2(dposition.X, dposition.Y), "triggerdoor");
                                    list.Add(block2);
                                }
                                dposition.X = dposition.X + 48;
                            }
                        }
                        pushed = false;
                        time = 0;
                        activeTime = Convert.ToInt32((double)Game1.luaInstance["triggerTimeDoor"]);
                        cboxnew.Y = doorstart.Y - 48;
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
                                    Sound.Play("skullmonkey_freed");
                                }
                            }
                        }
                    }
                }
                else if (typ == 1)
                {
                    bool ende = false;
                    bool collide = false;
                    Rectangle cboxnew = triggerend;
                    cboxnew.Y = triggerend.Y + 48;
                    bool wallmoved = false;
                    for (int j = 0; j < blocks.Count(); ++j)
                    {
                        Block block = blocks.ElementAt(j);
                        block.cbox.Y -= speed;
                        block.position.Y -= speed;
                        if (!wallmoved)
                        {
                            wallposition = block.position;
                            wallY += speed;
                            if (wallY > 672)
                                wallY = 672;
                            wallmoved = true;
                        }
                        for (int a = 0; a < list.Count(); ++a)
                        {
                            Block blocka = list.ElementAt(a);
                            if (block.cbox.Intersects(blocka.cbox)&& !block.cbox.Intersects(triggerend)&&blocka.type == "triggerend")
                            {
                                ende = true;
                                block.cbox.Y += speed;
                                block.position.Y += speed;
                                break;
                            }
                            if (block.cbox.Intersects(cboxnew))
                            {
                                collide = true;
                                break;
                            }
                        }
                        if (ende || collide)
                            break;
                    }
                    if (!collide && !ende)
                    {
                        Block block = new Block(new Vector2(triggerend.X, triggerend.Y + 48), "underground_earth");
                        list.Add(block);
                        blocks.Add(block);
                    }
                    else if (ende)
                    {
                        pushed = false;
                        time = 0;
                        activeTime = Convert.ToInt32((double)Game1.luaInstance["triggerTimeWall"]);
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
                        Rectangle cboxnew = doorstart;
                        bool geht = true;
                        if (cboxnew.Intersects(sbox) || cboxnew.Intersects(hbox))
                        {
                            geht = false;
                        }
                        else
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                for (int a = 0; a < elist.Count(); ++a)
                                {
                                    Enemy enemy = elist.ElementAt(a);
                                    if (enemy.cbox.box.Intersects(cboxnew))
                                    {
                                        geht = false;
                                        break;
                                    }
                                }
                                if (!geht)
                                    break;
                                cboxnew.Y = cboxnew.Y - 48;
                            }
                            cboxnew.Y = doorstart.Y - 48;
                            for (int a = 0; a < 10; a++)
                            {
                                cboxnew.X = cboxnew.X + 48;
                                if (sbox.Intersects(cboxnew)||hbox.Intersects(cboxnew))
                                    geht = false;
                                for (int j = 0; j < elist.Count(); ++j)
                                {
                                    Enemy enemy = elist.ElementAt(j);
                                    if (cboxnew.Intersects(enemy.cbox.box))
                                    {
                                        geht = false;
                                    }
                                    if (!geht)
                                        break;
                                }
                                if (!geht)
                                    break;
                            }
                        }
                        if (geht)
                        {
                            if(doorframe == 0)
                            {
                                bool collide = false;

                                if (Game1.level == 4)
                                {
                                    Rectangle dposition = doorstart;
                                    dposition.X = (int)position.X;
                                    dposition.Y = (int)position.Y + 48;
                                    for (int i = 0; i < 20; i++)
                                    {
                                        for (int j = 0; j < list.Count(); ++j)
                                        {
                                            Block block = list.ElementAt(j);
                                            if (block.cbox.Intersects(dposition)&&block.type == "triggerdoor")
                                            {
                                                list.Remove(block);
                                            }
                                        }
                                        dposition.X = dposition.X + 48;
                                    }
                                }
                                cboxnew = doorstart;
                                cboxnew.Y = doorstart.Y + 48;
                                for (int j = 0; j < 10; j++)
                                {
                                    collide = false;
                                    for (int a = 0; a < list.Count(); ++a)
                                    {
                                        Block blocka = list.ElementAt(a);
                                        if (blocka.cbox.Intersects(cboxnew))
                                        {
                                            collide = true;
                                            break;
                                        }
                                    }
                                    if (!collide)
                                    {
                                        Block block = new Block(new Vector2(cboxnew.X, cboxnew.Y), "triggerdoor");
                                        list.Add(block);
                                    }
                                    cboxnew.Y -= 48;
                                }
                                b.block = true;
                                active = false;
                                time = 0;
                            }
                            else
                            {
                                doorframe -= 1;
                            }
                        }
                    }
                    if (typ == 1)
                    {
                        Rectangle cboxnew = triggerend;
                        cboxnew.Y = cboxnew.Y + 96;
                        bool wallmoved = false;
                        for (int i = 0; i < blocks.Count(); ++i)
                        {
                            //Bewege sie einfach nach oben
                            Block block = blocks.ElementAt(i);
                            block.cbox.Y += speed;
                            block.position.Y += speed;
                            if (!wallmoved)
                            {
                                wallposition = block.position;
                                wallY -= speed;
                                if (wallY < 0)
                                    wallY = 0;
                                wallmoved = true;
                            }
                            if (block.cbox.Intersects(cboxnew))
                            {
                                blocks.Remove(block);
                                list.Remove(block);
                            }
                        }
                        if (blocks.Count() == 0)
                        {
                            blocks.Clear();
                            b.block = true;
                            active = false;
                            time = 0;
                        }
                    }
                }
            }
        }
    }
}
