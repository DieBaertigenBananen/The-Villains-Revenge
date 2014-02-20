using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
     class Map
     {
         Texture2D mapTexture,itemTexture,triggerTexture,objectTexture,bewegendTexture,breakTexture,wallTexture,doorTexture;
         public Texture2D levelMap;
         public Vector2 size;
         public Color[] pixelColors;
         public int[, ,] pixelRGBA;
         public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
         public List<Item> items = new List<Item>(); //Erstelle Blocks als List
         public List<Checkpoint> checkpoints = new List<Checkpoint>(); //Erstelle Blocks als List
         public List<Enemy> enemies = new List<Enemy>(); //Erstelle Blocks als List
         public List<Trigger> triggers = new List<Trigger>(); //Erstelle Blocks als List
         public List<MovingBlock> mblocks = new List<MovingBlock>(); //Erstelle Blocks als List
         public List<Breakable> breakblocks = new List<Breakable>(); //Erstelle Blocks als List
         public List<KIPoint> kipoints = new List<KIPoint>(); //Erstelle Blocks als List
         public List<Obj> objects = new List<Obj>(); //Erstelle Blocks als List

         public List<Breakable> savebreakable = new List<Breakable>(); //Erstelle Blocks als List
         public List<Block> saveblocks = new List<Block>(); //Erstelle Blocks als List
         public List<Enemy> saveenemies = new List<Enemy>(); //Erstelle Blocks als List
         public List<Obj> saveobjects = new List<Obj>(); //Erstelle Blocks als List
         public List<Item> saveitems = new List<Item>(); //Erstelle Blocks als List

         public List<Breakable> startbreakable = new List<Breakable>(); //Erstelle Blocks als List
         public List<Block> startblocks = new List<Block>(); //Erstelle Blocks als List
         public List<Enemy> startenemies = new List<Enemy>(); //Erstelle Blocks als List
         public List<Obj> startobjects = new List<Obj>(); //Erstelle Blocks als List
         public List<Item> startitems = new List<Item>(); //Erstelle Blocks als List


         public Map()
         {
         }
         public void StartSave()
         {
             startitems.Clear();
             for (int i = 0; i < items.Count(); ++i)
             {
                 Item item = items.ElementAt(i);
                 startitems.Add(new Item(item.position,item.type));
             }
             startenemies.Clear();
             for (int i = 0; i < enemies.Count(); ++i)
             {
                 Enemy enemy = enemies.ElementAt(i);
                 startenemies.Add(new Enemy(enemy.position, enemy.type, enemy.mover));
             }
             startblocks.Clear();
             for (int i = 0; i < blocks.Count(); ++i)
             {
                 Block block = blocks.ElementAt(i);
                 if (block.type == "breakable" || block.type == "triggerdoor" || block.type == "breakable_verticale")
                     startblocks.Add(new Block(block.position, block.type));
             }
             startobjects.Clear();
             for (int i = 0; i < objects.Count(); ++i)
             {
                 Obj obj = objects.ElementAt(i);
                 if (obj.type == 1)
                     startobjects.Add(new Banana(obj.position, 1));
                 else if (obj.type == 2)
                     startobjects.Add(new Kacke(obj.position, 2, obj.richtung));
                 else if (obj.type == 3)
                     startobjects.Add(new Debris(obj.position, 3, obj.stone));
             }
             foreach (Trigger trigger in triggers)
             {
                 trigger.StartSave();
             }
             foreach (MovingBlock mblock in mblocks)
             {
                 mblock.StartSave();
             }
             startbreakable.Clear();
             for (int i = 0; i < breakblocks.Count(); ++i)
             {
                 Breakable breakblock = breakblocks.ElementAt(i);
                 startbreakable.Add(new Breakable(blocks, breakblock.vertikal, breakblock.id));
             }
         }

         public void StartReset()
         {
             items.Clear();
             for (int i = 0; i < startitems.Count(); ++i)
             {
                 Item item = startitems.ElementAt(i);
                 items.Add(new Item(item.position, item.type));
             }
             enemies.Clear();
             for (int i = 0; i < startenemies.Count(); ++i)
             {
                 Enemy enemy = startenemies.ElementAt(i);
                 if (enemy.type == 1)
                     enemies.Add(new Bunny(enemy.position, enemy.type, enemy.mover));
                 if (enemy.type == 2)
                     enemies.Add(new Monkey(enemy.position, enemy.type, enemy.mover));
             }
             for (int i = 0; i < blocks.Count(); ++i)
             {
                 Block block = blocks.ElementAt(i);
                 if (block.type == "breakable" || block.type == "triggerdoor" || block.type == "breakable_verticale")
                     blocks.RemoveAt(i);
             }
             for (int i = 0; i < startblocks.Count(); ++i)
             {
                 Block block = startblocks.ElementAt(i);
                 blocks.Add(new Block(block.position, block.type));
             }
             breakblocks.Clear();
             for (int i = 0; i < startbreakable.Count(); ++i)
             {
                 Breakable breakblock = startbreakable.ElementAt(i);
                 breakblocks.Add(new Breakable(blocks, breakblock.vertikal, breakblock.id));
             }
             objects.Clear();
             for (int i = 0; i < startobjects.Count(); ++i)
             {
                 Obj obj = startobjects.ElementAt(i);
                 if (obj.type == 1)
                     objects.Add(new Banana(obj.position, 1));
                 else if (obj.type == 2)
                     objects.Add(new Kacke(obj.position, 2, obj.richtung));
                 else if (obj.type == 3)
                     objects.Add(new Debris(obj.position, 3, obj.stone));
             }
             foreach (Trigger trigger in triggers)
             {
                 trigger.StartReset(blocks, enemies);
             }
             foreach (MovingBlock mblock in mblocks)
             {
                 mblock.StartReset();
             }
         }
         public void Save()
         {
             saveitems.Clear();
             for (int i = 0; i < items.Count(); ++i)
             {
                 Item item = items.ElementAt(i);
                 saveitems.Add(new Item(item.position, item.type));
             }
             saveenemies.Clear();
             for (int i = 0; i < enemies.Count(); ++i)
             {
                 Enemy enemy = enemies.ElementAt(i);
                 saveenemies.Add(new Enemy(enemy.position, enemy.type, enemy.mover));
             }
             saveblocks.Clear();
             for (int i = 0; i < blocks.Count(); ++i)
             {
                 Block block = blocks.ElementAt(i);
                 if (block.type == "breakable"||block.type == "triggerdoor"||block.type == "breakable_verticale")
                     saveblocks.Add(new Block(block.position, block.type));
             }
             saveobjects.Clear();
             for (int i = 0; i < objects.Count(); ++i)
             {
                 Obj obj = objects.ElementAt(i);
                 if(obj.type == 1)
                     saveobjects.Add(new Banana(obj.position, 1));
                 else if (obj.type == 2)
                     saveobjects.Add(new Kacke(obj.position, 2,obj.richtung));
                 else if (obj.type == 3)
                     saveobjects.Add(new Debris(obj.position, 3, obj.stone));
             }
             foreach (Trigger trigger in triggers)
             {
                 trigger.Save();
             }
             foreach (MovingBlock mblock in mblocks)
             {
                 mblock.Save();
             }
             savebreakable.Clear();
             for (int i = 0; i < breakblocks.Count(); ++i)
             {
                 Breakable breakblock = breakblocks.ElementAt(i);
                 savebreakable.Add(new Breakable(blocks, breakblock.vertikal, breakblock.id));
             }
         }
         public void Reset()
         {
             items.Clear();
             for (int i = 0; i < saveitems.Count(); ++i)
             {
                 Item item = saveitems.ElementAt(i);
                 items.Add(new Item(item.position, item.type));
             }
             enemies.Clear();
             for (int i = 0; i < saveenemies.Count(); ++i)
             {
                 Enemy enemy = saveenemies.ElementAt(i);
                 if (enemy.type == 1)
                     enemies.Add(new Bunny(enemy.position, enemy.type, enemy.mover));
                 if (enemy.type == 2)
                     enemies.Add(new Monkey(enemy.position, enemy.type,enemy.mover));
             }
             for (int i = 0; i < blocks.Count(); ++i)
             {
                 Block block = blocks.ElementAt(i);
                 if (block.type == "breakable" || block.type == "triggerdoor" || block.type == "breakable_verticale")
                     blocks.RemoveAt(i);
             }
             for (int i = 0; i < saveblocks.Count(); ++i)
             {
                 Block block = saveblocks.ElementAt(i);
                 blocks.Add(new Block(block.position, block.type));
             }
             breakblocks.Clear();
             for (int i = 0; i < savebreakable.Count(); ++i)
             {
                 Breakable breakblock = savebreakable.ElementAt(i);
                 breakblocks.Add(new Breakable(blocks, breakblock.vertikal, breakblock.id));
             }
             objects.Clear();
             for (int i = 0; i < saveobjects.Count(); ++i)
             {
                 Obj obj = saveobjects.ElementAt(i);
                 if (obj.type == 1)
                     objects.Add(new Banana(obj.position, 1));
                 else if (obj.type == 2)
                     objects.Add(new Kacke(obj.position, 2, obj.richtung));
                 else if (obj.type == 3)
                     objects.Add(new Debris(obj.position, 3,obj.stone));
             }
             foreach (Trigger trigger in triggers)
             {
                 trigger.Reset(blocks,enemies);
             }
             foreach (MovingBlock mblock in mblocks)
             {
                 mblock.Reset();
             }
         }

         public void Load(ContentManager Content)
         {
             //Lade Textur, einmal ausgeführt
             wallTexture = Content.Load<Texture2D>("sprites/wall");
             objectTexture = Content.Load<Texture2D>("sprites/objects");
             itemTexture = Content.Load<Texture2D>("sprites/items");
             mapTexture = Content.Load<Texture2D>("sprites/tiles");
             levelMap = Content.Load<Texture2D>("sprites/Level_" + Game1.level + "/map");
             bewegendTexture = Content.Load<Texture2D>("sprites/Level_" + Game1.level + "/bewegend");
             breakTexture = Content.Load<Texture2D>("sprites/Level_" + Game1.level + "/destruction");
             triggerTexture = Content.Load<Texture2D>("sprites/Level_" + Game1.level + "/buttons");
             doorTexture = Content.Load<Texture2D>("sprites/Level_" + Game1.level + "/door");
             pixelColors = new Color[levelMap.Width * levelMap.Height];
             levelMap.GetData<Color>(pixelColors);
             pixelRGBA = new int[levelMap.Width, levelMap.Height, 4];
             for (int i = 0; i < levelMap.Height; i++)
             {
                 for (int t = 0; t < levelMap.Width; t++)
                 {
                     pixelRGBA[t, i, 0] = pixelColors[i * levelMap.Width + t].R;
                     pixelRGBA[t, i, 1] = pixelColors[i * levelMap.Width + t].G;
                     pixelRGBA[t, i, 2] = pixelColors[i * levelMap.Width + t].B;
                     pixelRGBA[t, i, 3] = pixelColors[i * levelMap.Width + t].A;
                 }
             }
             size = new Vector2(levelMap.Width * 48, levelMap.Height * 48);
         }
         public void Update(GameTime gameTime,Rectangle sbox,Rectangle hbox)
         {
             for (int i = 0; i < triggers.Count(); ++i)
             {
                 Trigger trigger = triggers.ElementAt(i);
                 trigger.Update(gameTime, blocks,enemies,sbox,hbox);
             }
             for (int i = 0; i < mblocks.Count(); ++i)
             {
                 MovingBlock mblock = mblocks.ElementAt(i);
                 mblock.Update(gameTime, blocks);
             }
         }

         public void Draw(SpriteBatch spriteBatch,GameTime gameTime,Camera camera, Color color)
         {
             for (int i = 0; i < blocks.Count(); ++i)
             { 
                 Block block = blocks.ElementAt(i);
                 if (Game1.debug)
                     spriteBatch.Draw(mapTexture, block.position, block.cuttexture, color, 0, Vector2.Zero, 1, SpriteEffects.None, 1.0f);
             }
             for (int i = 0; i < items.Count(); ++i)
             { 
                 Item item = items.ElementAt(i);
                 spriteBatch.Draw(itemTexture, item.position, item.cuttexture, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
             }
             for (int i = 0; i < triggers.Count(); ++i)
             {
                 Trigger trigger = triggers.ElementAt(i);
                 Vector2 tvector = trigger.position;
                 tvector.X -= 40;
                 tvector.Y += 20;
                 if (trigger.typ == 2)
                 {
                     Vector2 dvector = trigger.wallposition;
                     if (Game1.level == 3)
                     {
                         if (trigger.doorframe < 10)
                         {
                             dvector.Y += 100;
                             dvector.X -= 50;
                             spriteBatch.Draw(doorTexture, dvector, new Rectangle(0, 0, 106, 409), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                         }
                         else if (trigger.doorframe >= 10&&trigger.doorframe < 20)
                         {
                             dvector.Y += 90;
                             dvector.X -= 320;
                             spriteBatch.Draw(doorTexture, dvector, new Rectangle(106, 0, 376, 409), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                         }
                         else
                         {
                             dvector.Y += 120;
                             dvector.X -= 370;
                             spriteBatch.Draw(doorTexture, dvector, new Rectangle(484, 0, 416, 409), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                         }
                     }
                     else if (Game1.level == 4)
                     {
                         if (i == 0)
                         {
                             if (trigger.doorframe < 10)
                             {
                                 dvector.Y -= 500;
                                 dvector.X -= 50;
                                 spriteBatch.Draw(doorTexture, dvector, new Rectangle(0, 0, 106, 540), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                             }
                             else if (trigger.doorframe >= 10 && trigger.doorframe < 20)
                             {
                                 dvector.Y -= 500;
                                 dvector.X -= 320;
                                 spriteBatch.Draw(doorTexture, dvector, new Rectangle(106, 0, 376, 540), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                             }
                             else
                             {
                                 dvector.Y -= 500;
                                 dvector.X -= 370;
                                 spriteBatch.Draw(doorTexture, dvector, new Rectangle(484, 0, 416, 540), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                             }
                         }
                         else if (i == 1)
                         {
                             if (trigger.doorframe < 10)
                             {
                                 dvector.Y -= 500;
                                 dvector.X -= 100;
                                 spriteBatch.Draw(doorTexture, dvector, new Rectangle(896, 0, 896 + 251, 540), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                             }
                             else if (trigger.doorframe >= 10 && trigger.doorframe < 20)
                             {
                                 dvector.Y -= 490;
                                 dvector.X -= 480;
                                 spriteBatch.Draw(doorTexture, dvector, new Rectangle(896 + 251, 0, 896 + 580, 540), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                             }
                             else
                             {
                                 dvector.Y -= 470;
                                 dvector.X -= 570;
                                 spriteBatch.Draw(doorTexture, dvector, new Rectangle(896 + 831, 0, 896 + 647, 540), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                             }
                         }
                     }
                 }
                 if (trigger.active)
                 {
                     spriteBatch.Draw(triggerTexture, tvector, new Rectangle(147, 0, 147, 60), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     if (trigger.typ == 1)
                         spriteBatch.Draw(wallTexture, trigger.wallposition, new Rectangle(0, 0, 48, trigger.wallY), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 }
                 else
                     spriteBatch.Draw(triggerTexture, tvector, new Rectangle(0, 0, 147, 60), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
             }
             for (int i = 0; i < objects.Count(); ++i)
             {
                 Obj obj = objects.ElementAt(i);
                 if (obj.type == 1)
                     spriteBatch.Draw(objectTexture, obj.position, new Rectangle(0, 0, 96, 96), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 else if (obj.type == 2)
                     spriteBatch.Draw(objectTexture, obj.position, new Rectangle(96, 0, 96, 96), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 else if (obj.type == 3)
                 {  
                     if(obj.stone)
                         spriteBatch.Draw(objectTexture, obj.position, new Rectangle(96 * 2, 0, 96, 96), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     else
                         spriteBatch.Draw(objectTexture, obj.position, new Rectangle(96 * 3, 0, 96, 96), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 }
                 else if (obj.type == 4)
                     spriteBatch.Draw(objectTexture, obj.position, new Rectangle(93*3, 0, 144, 96), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
             }
             for (int i = 0; i < mblocks.Count(); ++i)
             {
                 MovingBlock mblock = mblocks.ElementAt(i);
                 spriteBatch.Draw(bewegendTexture, new Vector2(mblock.cbox.X,mblock.cbox.Y-24),color);
             }
             for (int i = 0; i < breakblocks.Count(); ++i)
             {
                 Breakable breakblock = breakblocks.ElementAt(i);
                 if (Game1.level == 3)
                 {
                     if (breakblock.id == 0)
                     {
                         spriteBatch.Draw(breakTexture, new Vector2(breakblock.cbox.X - 127, breakblock.cbox.Y - 20), new Rectangle(0, 0, 332, 372), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     }
                     else if (breakblock.id == 1 )
                     {
                         spriteBatch.Draw(breakTexture, new Vector2(breakblock.cbox.X - 70, breakblock.cbox.Y-27), new Rectangle(332 * 1, 0, 332, 372), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     }
                     else if (breakblock.id == 2)
                     {
                         spriteBatch.Draw(breakTexture, new Vector2(breakblock.cbox.X - 10, breakblock.cbox.Y-24), new Rectangle(332 * 2, 0, 332, 372), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     }
                     else if (breakblock.id == 3)
                     {
                         spriteBatch.Draw(breakTexture, new Vector2(breakblock.cbox.X - 20, breakblock.cbox.Y-65), new Rectangle(332 * 3, 0, 332, 372), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     }
                 }
                 else if (Game1.level == 4)
                 {
                     if (breakblock.id == 0)
                     {
                         spriteBatch.Draw(breakTexture, new Vector2(breakblock.cbox.X - 60, breakblock.cbox.Y - 35), new Rectangle(0, 0, 389, 377), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     }
                     else if (breakblock.id == 1)
                     {
                         spriteBatch.Draw(breakTexture, new Vector2(breakblock.cbox.X - 315, breakblock.cbox.Y - 35), new Rectangle(389, 0, 389, 377), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     }
                 }
             }
         }

         public void Generate(Player spieler,Hero hero)
         {
             //generiere Das Level (erzeuge neue Objekte in der List) anhand der Levelmap
             int moving_last = 0;
             int break_x_last = 0;
             int break_x_anzahl = 0;
             int break_y_last = 0;
             int break_y_anzahl = 0;
             int moving_anzahl = 0;
             for (int i = 0; i < levelMap.Width; i++)
             {
                 for (int t = 0; t < levelMap.Height; t++)
                 {
                     string type = "";
                     if (pixelRGBA[i, t, 3] == 0)
                     {
                     }
                     else
                     {
                         String color = pixelRGBA[i, t, 0] + "," + pixelRGBA[i, t, 1] + "," + pixelRGBA[i, t, 2];
                         if (pixelRGBA[i, t, 1] == 255 && pixelRGBA[i, t, 2] == 255)
                         {
                             kipoints.Add(new KIPoint(new Vector2(i * 48, t * 48), pixelRGBA[i, t, 0]));
                         }
                         switch (color)
                         {
                             case "255,100,255":
                                 spieler.spine.skeleton.X = i*48;
                                 spieler.spine.skeleton.Y = t * 48;
                                 spieler.position.Y = spieler.spine.skeleton.Y;
                                 spieler.position.X = spieler.spine.skeleton.X;
                                 spieler.cbox.Update(spieler.position);
                                 spieler.checkpoint = spieler.position;
                                 break;
                             case "255,115,255":
                                 hero.spine.skeleton.X = i * 48;
                                 hero.spine.skeleton.Y = t * 48;
                                 hero.position.Y = hero.spine.skeleton.Y;
                                 hero.position.X = hero.spine.skeleton.X;
                                 hero.cbox.Update(hero.position);
                                 hero.checkpoint = hero.position;
                                 break;
                             case "147,17,126":
                                 type = "checkpoint";
                                 checkpoints.Add(new Checkpoint(i * 48, false));
                                 break;
                             case "39,118,33":
                                 type = "moving";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 if (moving_last + 1 != i)
                                 {
                                     moving_anzahl++;
                                 }
                                 moving_last = i;
                                 break;
                             case "119,0,255":
                                 type = "breakable";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 if (break_x_last + 1 != i)
                                 {
                                     break_x_anzahl++;
                                 }
                                 break_x_last = i;
                                 break;
                             case "119,20,255":
                                 type = "breakable_verticale";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 if (break_y_last + 1 != t)
                                 {
                                     break_y_anzahl++;
                                 }
                                 break_y_last = t;
                                 break;
                             case "180,165,0":
                                 type = "movingend";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "104,60,17":
                                 type = "underground_earth";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "0,255,0":
                                 type = "ground_grass";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "0,0,255":
                                 type = "water";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "0,0,0":
                                 type = "hase";
                                 enemies.Add(new Bunny(new Vector2(i * 48, t * 48), 1,true));
                                 break;
                             case "255,255,0":
                                 type = "zeit";
                                 items.Add(new Item(new Vector2(i * 48, t * 48 -96 +48), type));
                                 break;
                             case "180,150,140":
                                 type = "bag";
                                 items.Add(new Item(new Vector2(i * 48, t * 48 -96+48), type));
                                 break;
                             case "171,140,188":
                                 type = "herz";
                                 items.Add(new Item(new Vector2(i * 48, t * 48 - 96 + 48), type));
                                 break;
                             case "255,200,200":
                                 type = "monkey";
                                 items.Add(new Item(new Vector2(i * 48, t * 48 - 96 + 48), type));
                                 break;
                             case "50,50,50":
                                 type = "monkey";
                                 enemies.Add(new Monkey(new Vector2(i * 48, t * 48), 2, true));
                                 break;
                             case "255,255,100":
                                 type = "banana";
                                 items.Add(new Item(new Vector2(i * 48, t * 48 - 96 + 48), type));
                                 break;
                             case "255,0,0":
                                 type = "trigger";
                                 Block b = new Block(new Vector2(i * 48, t * 48), type);
                                 blocks.Add(b);
                                 triggers.Add(new Trigger(new Vector2(i * 48, t * 48),b));
                                 break;
                             case "153,0,0":
                                 type = "triggerend";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "155,0,0":
                                 type = "triggerdoor";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                         }
                     }
                 }
             }
             for (int i = 0; i < moving_anzahl; i++)
             {
                 mblocks.Add(new MovingBlock(blocks));
             }
             int id = 0;
             for (int i = 0; i < break_x_anzahl; i++)
             {
                 breakblocks.Add(new Breakable(blocks,false,id));
                 id++;
             }
             for (int i = 0; i < break_y_anzahl; i++)
             {
                 breakblocks.Add(new Breakable(blocks, true, id));
                 id++;
             }
             checkpoints.Add(new Checkpoint((int)size.X - 100, true)); //Ende
             for (int i = 0; i < triggers.Count(); ++i)
             {
                 Trigger trigger = triggers.ElementAt(i);
                 trigger.Check(blocks);
             }
             Save();
         }
    }
}
