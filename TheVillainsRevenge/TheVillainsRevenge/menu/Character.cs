using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    class Character
    {
        public Vector2 position; //Position
        public Spine spine;

        public Character(int x, int y)
        {
            position.X = x;
            position.Y = y;
            spine = new Spine();
        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics, string skeletonName, float scale, float fading)//Wird im Hauptgame ausgeführt und geladen
        {
            spine.Load(position, skeletonName, scale, fading);
        }

        public void Draw(GameTime gameTime, Camera camera)
        {
            spine.Draw(gameTime, camera, position);
        }
    }
}
