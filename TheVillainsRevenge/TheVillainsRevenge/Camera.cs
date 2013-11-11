using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
<<<<<<< HEAD
=======
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
>>>>>>> parent of 532732f... Cam Test

namespace TheVillainsRevenge
{
    class Camera
    {
<<<<<<< HEAD
=======
        public Vector2 camp = new Vector2(0, 0);
        bool check = false;

        public Camera()
        {

        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            //Steuerung
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.F))
            {
                if (!check)
                {
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    check = true;
                }
                else
                {

                    graphics.PreferredBackBufferHeight = (int)Game1.cams.Y;
                    graphics.PreferredBackBufferWidth = (int)Game1.cams.X;
                    check = false;
                }
                graphics.ApplyChanges();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) == true)
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D2) == true)
            {
                camp.X--;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D1) == true)
            {
                camp.X++;
            }
        }
>>>>>>> parent of 532732f... Cam Test
    }
}
