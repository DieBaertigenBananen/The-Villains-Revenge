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
        //Lade Textur
        Texture2D mapTexture;
        //Erstelle Blocks als List
        public List<Block> blocks = new List<Block>();
        public void Load(ContentManager Content)
        {
            //Lade Textur, einmal ausgeführt
            mapTexture = Content.Load<Texture2D>("sprites/tiles");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Gehe alle Blöcke durch
            foreach (Block block in blocks)
            {
                //Zeichne die Blöcke anhand der Daten der Blöcke
                spriteBatch.Draw(mapTexture, block.pos, block.cuttexture, Color.White);
            }
        }

        public void Generate()
        {
            //generiere Das Level (erzeuge neue Objekte in der List)
            //Iteration
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i*48, 500-48), "ground"));
            }
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 10 * 48, 500 - 48), "solid"));
            }
            for (int i = 0; i <= 3; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 5 * 48, 500 - 4 * 48), "water"));
            }
            for (int i = 0; i <= 5; i++)
            {
                blocks.Add(new Block(new Vector2(i * 48 + 10 * 48, 500 - 7 * 48), "ladder"));
            }
        }
    }
}
