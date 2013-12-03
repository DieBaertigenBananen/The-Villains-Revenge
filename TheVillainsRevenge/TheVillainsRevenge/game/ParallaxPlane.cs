﻿using System;
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
        Texture2D[] texture;
        int tilesCount;
        public Vector2 size;
        public Vector2 position;

        public ParallaxPlane()
        {
            position = new Vector2(0, 0);
            size = new Vector2(0, 0);
        }

        public void Load(ContentManager Content, string textureName, int tiles)
        {
            tilesCount = tiles;
            //Lade Textur, einmal ausgeführt
            texture = new Texture2D[tilesCount];
            for (int i = 0; i < tilesCount; i++)
            {
                texture[i] = Content.Load<Texture2D>("sprites/Level_1/Planes/" + textureName + "_" + (i + 1));
                size.X += texture[i].Width;
            }
            size.Y = texture[0].Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tilesCount; i++)
            {
                spriteBatch.Draw(texture[i], new Vector2(position.X + (i * 3840), position.Y), Color.White);
            }
        }

        public void Update(Map karte, Camera camera)
        {
            //Position = Viewportposition - (Position wenn am Ende am Maprand * Positionsfaktor abhängig von Viewportposition/letzte Mapposition)
            position.X = camera.viewport.X - ((size.X - camera.viewport.Width) * (camera.viewport.X / (karte.size.X - camera.viewport.Width)));
            position.Y = camera.viewport.Y - ((size.Y - camera.viewport.Height) * (camera.viewport.Y / (karte.size.Y - camera.viewport.Height)));
        }
    }
    
}