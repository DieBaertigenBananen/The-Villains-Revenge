﻿using System;
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
         Texture2D mapTexture,itemTexture,triggerTexture,objectTexture;
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
         public List<KIPoint> kipoints = new List<KIPoint>(); //Erstelle Blocks als List
         public List<Obj> objects = new List<Obj>(); //Erstelle Blocks als List

         public List<Block> saveblocks = new List<Block>(); //Erstelle Blocks als List
         public List<Enemy> saveenemies = new List<Enemy>(); //Erstelle Blocks als List
         public List<Obj> saveobjects = new List<Obj>(); //Erstelle Blocks als List


         public Map()
         {
         }
         public void Save()
         {
             saveenemies.Clear();
             for (int i = 0; i < enemies.Count(); ++i)
             {
                 Enemy enemy = enemies.ElementAt(i);
                 saveenemies.Add(enemy);
             }
             saveblocks.Clear();
             for (int i = 0; i < blocks.Count(); ++i)
             {
                 Block block = blocks.ElementAt(i);
                 saveblocks.Add(block);
             }
             saveobjects.Clear();
             for (int i = 0; i < objects.Count(); ++i)
             {
                 Obj obj = objects.ElementAt(i);
                 saveobjects.Add(obj);
             }
             foreach (Trigger trigger in triggers)
             {
                 trigger.Save();
             }
             foreach (MovingBlock mblock in mblocks)
             {
                 mblock.Save();
             }
         }
         public void Reset()
         {
             enemies.Clear();
             for (int i = 0; i < saveenemies.Count(); ++i)
             {
                 Enemy enemy = saveenemies.ElementAt(i);
                 enemies.Add(enemy);
             }
             blocks.Clear();
             for (int i = 0; i < saveblocks.Count(); ++i)
             {
                 Block block = saveblocks.ElementAt(i);
                 blocks.Add(block);
             }
             objects.Clear();
             for (int i = 0; i < saveobjects.Count(); ++i)
             {
                 Obj obj = saveobjects.ElementAt(i);
                 objects.Add(obj);
             }
             foreach (Trigger trigger in triggers)
             {
                 trigger.Reset(blocks);
             }
             foreach (MovingBlock mblock in mblocks)
             {
                 mblock.Reset();
             }
         }

         public void Load(ContentManager Content)
         {
             //Lade Textur, einmal ausgeführt
             objectTexture = Content.Load<Texture2D>("sprites/objects");
             itemTexture = Content.Load<Texture2D>("sprites/items");
             mapTexture = Content.Load<Texture2D>("sprites/tiles");
             levelMap = Content.Load<Texture2D>("sprites/Level_1/map");
             triggerTexture = Content.Load<Texture2D>("sprites/trigger");
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
         public void Update(GameTime gameTime,Rectangle sbox)
         {
             for (int i = 0; i < triggers.Count(); ++i)
             {
                 Trigger trigger = triggers.ElementAt(i);
                 trigger.Update(gameTime, blocks,sbox);
             }
             for (int i = 0; i < mblocks.Count(); ++i)
             {
                 MovingBlock mblock = mblocks.ElementAt(i);
                 mblock.Update(gameTime, blocks);
             }
         }

         public void Draw(SpriteBatch spriteBatch,GameTime gameTime,Camera camera)
         {
             spriteBatch.Draw(levelMap, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
             for (int i = 0; i < blocks.Count(); ++i)
             { 
                 Block block = blocks.ElementAt(i);
                 //Zeichne die Blöcke anhand der Daten der Blöcke
                 spriteBatch.Draw(mapTexture, block.position, block.cuttexture, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1.0f);
             }
             for (int i = 0; i < items.Count(); ++i)
             { 
                 Item item = items.ElementAt(i);
                 spriteBatch.Draw(itemTexture, item.position, item.cuttexture, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
             }
             for (int i = 0; i < enemies.Count(); ++i)
             { 
                 Enemy enemy = enemies.ElementAt(i);
                 enemy.Draw(spriteBatch,gameTime,camera);
             }
             for (int i = 0; i < triggers.Count(); ++i)
             {
                 Trigger trigger = triggers.ElementAt(i);
                 if(trigger.active)
                     spriteBatch.Draw(triggerTexture, trigger.position, new Rectangle(48, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 else
                     spriteBatch.Draw(triggerTexture, trigger.position, new Rectangle(0, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
             }
             for (int i = 0; i < objects.Count(); ++i)
             {
                 Obj obj = objects.ElementAt(i);
                 if (obj.type == 1)
                     spriteBatch.Draw(objectTexture, obj.position, new Rectangle(0, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 else if (obj.type == 2)
                     spriteBatch.Draw(objectTexture, obj.position, new Rectangle(48, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 else if (obj.type == 3)
                 {  
                     if(obj.fall)
                        spriteBatch.Draw(objectTexture, obj.position, new Rectangle(96, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                     else
                         spriteBatch.Draw(objectTexture, obj.position, new Rectangle(144, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                 }
             }
         }

         public void Generate()
         {
             //generiere Das Level (erzeuge neue Objekte in der List) anhand der Levelmap
             int moving_last = 0;
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
                                 enemies.Add(new Bunny(new Vector2(i * 48, (t * 48)-64), 1));
                                 break;
                             case "255,255,0":
                                 type = "zeit";
                                 items.Add(new Item(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "171,140,188":
                                 type = "herz";
                                 items.Add(new Item(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "50,50,50":
                                 type = "monkey";
                                 items.Add(new Item(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "255,255,100":
                                 type = "banana";
                                 items.Add(new Item(new Vector2(i * 48, t * 48), type));
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
                         }
                     }
                 }
             }
             for (int i = 0; i < moving_anzahl; i++)
             {
                 mblocks.Add(new MovingBlock(blocks));
             }
             checkpoints.Add(new Checkpoint((int)size.X - 100, true)); //Ende
             Save();
         }
    }
}
