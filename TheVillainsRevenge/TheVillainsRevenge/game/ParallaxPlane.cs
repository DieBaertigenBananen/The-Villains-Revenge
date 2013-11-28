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
        Texture2D texture;
        public Vector2 size;
        public Vector2 position;

        public ParallaxPlane()
        {
            position = new Vector2(0, 0);
        }

        public void Load(ContentManager Content, string textureName)
        {
            //Lade Textur, einmal ausgeführt
            texture = Content.Load<Texture2D>("sprites/" + textureName);
            size = new Vector2(texture.Width * 2, texture.Height * 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
        }

        public void Update(Map karte, Camera camera)
        {
            //Position = Viewportposition - (Position wenn am Ende am Maprand * Positionsfaktor abhängig von Viewportposition/letzte Mapposition)
            position.X = camera.viewport.X - ((size.X - camera.viewport.Width) * (camera.viewport.X / (karte.size.X - camera.viewport.Width)));
            position.Y = camera.viewport.Y - ((size.Y - camera.viewport.Height) * (camera.viewport.Y / (karte.size.Y - camera.viewport.Height)));
        }
    }
    
}
