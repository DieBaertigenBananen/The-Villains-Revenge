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
         Texture2D mapTexture,itemTexture,enemyTexture;
         public Texture2D levelMap;
         public Vector2 size;
         public Color[] pixelColors;
         public int[, ,] pixelRGBA;
         public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
         public List<Item> items = new List<Item>(); //Erstelle Blocks als List
         public List<Checkpoint> checkpoints = new List<Checkpoint>(); //Erstelle Blocks als List
         public List<Enemy> enemies = new List<Enemy>(); //Erstelle Blocks als List

         public Map()
         {

         }

         public void Load(ContentManager Content)
         {
             //Lade Textur, einmal ausgeführt
             itemTexture = Content.Load<Texture2D>("sprites/items");
             mapTexture = Content.Load<Texture2D>("sprites/tiles");
             levelMap = Content.Load<Texture2D>("sprites/Level_1/map");
             enemyTexture = Content.Load<Texture2D>("sprites/bunny");
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

         public void Draw(SpriteBatch spriteBatch)
         {
             spriteBatch.Draw(levelMap, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
             foreach (Block block in blocks) //Gehe alle Blöcke durch
             {
                 //Zeichne die Blöcke anhand der Daten der Blöcke
                 spriteBatch.Draw(mapTexture, block.pos, block.cuttexture, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1.0f);
             }
             foreach (Item item in items) //Gehe alle Blöcke durch
             {
                 spriteBatch.Draw(itemTexture, item.position, item.cuttexture, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
             }
             foreach (Enemy enemy in enemies) //Gehe alle Blöcke durch
             {
                 spriteBatch.Draw(enemyTexture, enemy.position, new Rectangle(0, 0, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
             }
         }

         public void Generate()
         {
             //generiere Das Level (erzeuge neue Objekte in der List) anhand der Levelmap
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
                         switch (color)
                         {
                             case "104,60,17":
                                 type = "underground_earth";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "0,255,0":
                                 type = "ground_grass";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "02,02,02":
                                 type = "platform_grass";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "0,0,255":
                                 type = "water";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "01,01,01":
                                 type = "underground_rock";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                             case "0,0,0":
                                 enemies.Add(new Enemy(new Vector2(i * 48, (t * 48)-64), 1));
                                 break;
                             case "255,255,0":
                                 items.Add(new Item(new Vector2(i * 48, t * 48), "zeit"));
                                 break;
                             case "171,140,188":
                                 items.Add(new Item(new Vector2(i * 48, t * 48), "herz"));
                                 break;
                             case "255,0,0":
                                 type = "lava";
                                 blocks.Add(new Block(new Vector2(i * 48, t * 48), type));
                                 break;
                         }
                     }
                 }
             }
             checkpoints.Add(new Checkpoint(4000, false));
             checkpoints.Add(new Checkpoint((int)size.X-100, true));
         }
    }
}
