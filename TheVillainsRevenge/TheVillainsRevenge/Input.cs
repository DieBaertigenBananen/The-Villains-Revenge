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
        public bool iteme;
        bool itemep;
        public bool itemq;
        bool itemqp;
        public bool links, rechts, sprung,fall;
        public void update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Tab) == true)
            {
                fall = true;
            }
            else
            {
                fall = false;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || keyState.IsKeyDown(Keys.Space) == true)
            {
                sprung = true;
            }
            else
            {
                sprung = false;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0f
                ||
                keyState.IsKeyDown(Keys.Right) == true
                ||
                keyState.IsKeyDown(Keys.D) == true) //Wenn Rechte Pfeiltaste
            {
                rechts = true;
            }
            else
            {
                rechts = false;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0f
                ||
                keyState.IsKeyDown(Keys.Left) == true
                ||
                keyState.IsKeyDown(Keys.A) == true) //Wenn Rechte Pfeiltaste
            {
                links = true;
            }
            else
            {
                links = false;
            }
            if (!itemep)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.E))
                {
                    iteme = true;
                    itemep = true;
                }
            }
            else
            {
                iteme = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Released
                    ||
                    keyState.IsKeyUp(Keys.E))
                {
                    itemep = false;
                }
            }
            if (!itemqp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.Q))
                {
                    itemq = true;
                    itemqp = true;
                }
            }
            else
            {
                itemq = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Released
                    || 
                    keyState.IsKeyUp(Keys.Q))
                {
                    itemqp = false;
                }
            }
            if (!enterp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
                    || 
                    keyState.IsKeyDown(Keys.Enter))
                {
                    enter = true;
                    enterp = true;
                }
            }
            else
            {
                enter = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released
                    || 
                    keyState.IsKeyUp(Keys.Enter))
                {
                    enterp = false;
                }
            }
            if (!downp)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0f
                    || 
                    keyState.IsKeyDown(Keys.Down) 
                    || 
                    keyState.IsKeyDown(Keys.S))
                {
                    down = true;
                    downp = true;
                }
            }
            else
            {
                down = false;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0f
                    &&
                    keyState.IsKeyUp(Keys.Down) && keyState.IsKeyUp(Keys.S))
                {
                    downp = false;
                }
            }
            if (!upp)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0f
                    ||
                    keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    up = true;
                    upp = true;
                }
            }
            else
            {
                up = false;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= 0f
                    &&
                    keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.W))
                {
                    upp = false;
                }
            }

        }

    }
}
