using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class ParallaxPlane
    {
        public Texture2D[] texture;
        int tilesCount;
        public Vector2 size;
        public Vector2 position;
        string textureName;
        int additionalHeight;
        

        public ParallaxPlane(string texName)
        {
            textureName = texName;
            position = new Vector2(0, 0);
            size = new Vector2(0, 0);
        }

        public void Load(ContentManager Content, int tiles, int addHeight)
        {
            additionalHeight = addHeight;
            tilesCount = tiles;
            //Lade Textur, einmal ausgeführt
            texture = new Texture2D[tilesCount];
            for (int i = 0; i < tilesCount; i++)
            {
                texture[i] = Content.Load<Texture2D>("sprites/level_"+Game1.level+"/planes/" + textureName + "_" + (i + 1));
                size.X += texture[i].Width;
            }
            size.Y = texture[0].Height + additionalHeight;
        }

        public void Draw(SpriteBatch spriteBatch, Player spieler)
        {
            for (int i = 0; i < tilesCount; i++)
            {
                if (!texture[i].IsDisposed)
                {
                    spriteBatch.Draw(texture[i], new Vector2(position.X + (i * 3840), position.Y), Color.White);
                }
            }
        }

        public void Update(ContentManager Content, Map map, Camera camera)
        {
            //Position = Viewportposition - (Position wenn am Ende am Maprand * Positionsfaktor abhängig von Viewportposition/letzte Mapposition)
            position.X = camera.viewport.X - ((size.X - camera.viewport.Width) * (camera.viewport.X / (map.size.X - camera.viewport.Width)));
            position.Y = camera.viewport.Y - ((size.Y - camera.viewport.Height) * (camera.viewport.Y / (map.size.Y - camera.viewport.Height))) + additionalHeight;
            //TextureManager(Content, camera);
        }
        public void TextureManager(ContentManager Content, Camera camera)
        {
            //Lade Textur, einmal ausgeführt
            for (int i = 0; i < tilesCount; i++)
            {
                if (camera.viewport.X + camera.viewport.Width >= position.X + (i * 3840) && camera.viewport.X <= position.X + (i * 3840) + texture[i].Width)
                {
                    if (texture[i].IsDisposed)
                    {
                        texture[i] = Content.Load<Texture2D>("sprites/Level_" + Game1.level + "/Planes/" + textureName + "_" + (i + 1));
                    }
                }
                else if (!texture[i].IsDisposed)
                {
                    texture[i].Dispose();
                }
            }
        }
    }
    
}
