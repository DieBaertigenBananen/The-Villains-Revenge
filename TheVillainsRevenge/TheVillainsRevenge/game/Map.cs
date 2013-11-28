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
        Texture2D mapTexture;
        Texture2D background;
        Texture2D itemTexture;
        public Vector2 size;
        public List<Block> blocks = new List<Block>(); //Erstelle Blocks als List
        public List<Item> items = new List<Item>(); //Erstelle Blocks als List

        public Map()
        {

        }

        public void Load(ContentManager Content)
        {
            //Lade Textur, einmal ausgeführt
            mapTexture = Content.Load<Texture2D>("sprites/tiles");
            itemTexture = Content.Load<Texture2D>("sprites/items");
            background = Content.Load<Texture2D>("sprites/background");
            size = new Vector2(background.Width * 2, background.Height * 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
            foreach (Block block in blocks) //Gehe alle Blöcke durch
            {
                //Zeichne die Blöcke anhand der Daten der Blöcke
                spriteBatch.Draw(mapTexture, block.pos, block.cuttexture, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            }
            foreach (Item item in items) //Gehe alle Blöcke durch
            {
                spriteBatch.Draw(itemTexture, item.pos, item.cuttexture, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            }
        }

        public void Generate()
        {
            //generiere Das Level (erzeuge neue Objekte in der List)
            //Iteration
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48, size.Y - 48), "ground_earth"));
            }
            for (int i = 0; i <= 80; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 5 * 48, size.Y - 4 * 48), "platform_grass"));
            }
            for (int i = 0; i <= 3; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 10 * 48, size.Y - 5 * 48), "ground_grass_30"));
            }
            for (int i = 0; i <= 3; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 60 * 48, size.Y - 5 * 48), "ground_grass_15"));
            }
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 15 * 48, size.Y - 7 * 48), "underground_earth"));
            }
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 20 * 48, size.Y - 10 * 48), "underground_earth"));
            }
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 25 * 48, size.Y - 13 * 48), "underground_earth"));
            }
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 30 * 48, size.Y - 16 * 48), "underground_earth"));
            }
            items.Add(new Item(2000, (int)size.Y - 240, "herz"));
        }
    }
}
