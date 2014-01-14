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
        

        public ParallaxPlane(string texName)
        {
            textureName = texName;
            position = new Vector2(0, 0);
            size = new Vector2(0, 0);
        }

        public void Load(ContentManager Content, int tiles)
        {
            tilesCount = tiles;
            //Lade Textur, einmal ausgeführt
            texture = new Texture2D[tilesCount];
            for (int i = 0; i < tilesCount; i++)
            {
                texture[i] = Content.Load<Texture2D>("sprites/level_1/planes/" + textureName + "_" + (i + 1));
                size.X += texture[i].Width;
            }
            size.Y = texture[0].Height;
        }

        public void Draw(SpriteBatch spriteBatch, Player spieler)
        {
            for (int i = 0; i < tilesCount; i++)
            {
                spriteBatch.Draw(texture[i], new Vector2(position.X + (i * 3840), position.Y), Color.White);
            }
        }

        public void Update(Map karte, Camera camera,bool fest)
        {
            //Position = Viewportposition - (Position wenn am Ende am Maprand * Positionsfaktor abhängig von Viewportposition/letzte Mapposition)
            if (fest)
            {
                position.X = 0;
                position.Y = karte.size.Y-size.Y;
            }
            else
            {
                position.X = camera.viewport.X - ((size.X - camera.viewport.Width) * (camera.viewport.X / (karte.size.X - camera.viewport.Width)));
                position.Y = camera.viewport.Y - ((size.Y - camera.viewport.Height) * (camera.viewport.Y / (karte.size.Y - camera.viewport.Height)));
            }
                //TextureManager(camera);
        }
        //public void TextureManager(Camera camera)
        //{
        //    //Lade Textur, einmal ausgeführt
        //    for (int i = 0; i < tilesCount; i++)
        //    {
        //        if (camera.viewport.X > position.X + (i * 3840) && camera.viewport.X < position.X + (i * 3840) + texture[i].Width)
        //        {
        //            if (texture[i].IsDisposed)
        //            {
        //                texture[i] = Content.Load<Texture2D>("sprites/Level_1/Planes/" + textureName + "_" + (i + 1));
        //            }
        //        }
        //        else
        //        {
        //            if (!texture[i].IsDisposed)
        //            {
        //                texture[i].Dispose();
        //            }
        //        }
        //    }
        //}
    }
    
}
