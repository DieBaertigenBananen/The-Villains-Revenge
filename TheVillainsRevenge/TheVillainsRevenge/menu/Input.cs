using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    class Input
    {
        public bool up;
        bool upp;
        public bool down;
        bool downp;
        public bool enter;
        bool enterp;
        public void update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (!enterp)
            {
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    enter = true;
                    enterp = true;
                }
            }
            else
            {
                enter = false;
                if (keyState.IsKeyUp(Keys.Enter))
                {
                    enterp = false;
                }
            }
            if (!downp)
            {
                if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                {
                    down = true;
                    downp = true;
                }
            }
            else
            {
                down = false;
                if (keyState.IsKeyUp(Keys.Down) && keyState.IsKeyUp(Keys.S))
                {
                    downp = false;
                }
            }
            if (!upp)
            {
                if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    up = true;
                    upp = true;
                }
            }
            else
            {
                up = false;
                if (keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.W))
                {
                    upp = false;
                }
            }

        }

    }
}
