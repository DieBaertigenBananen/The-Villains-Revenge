using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    public class Input
    {
        public bool up;
        bool upp;
        public bool down;
        bool downp;
        public bool enter;
        bool enterp;
        public bool itemu1;
        bool itemup1;
        public bool itemu2;
        bool itemup2;
        public bool back;
        bool backp;
        public bool debug;
        bool debugp;
        public bool hit;
        bool hitp;
        public bool shit;
        bool shitp;
        public bool fullscreen;
        bool fullscreenp;
        public bool sprung;
        public bool sprungp;
        public bool links, rechts, end;
        public float cameraDynR, cameraDynL;
        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            //Zurück
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed && GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) || keyState.IsKeyDown(Keys.F10) == true)
            {
                end = true;
            }
            else
            {
                end = false;
            }

            // --- GAME STUFF --- //
            //Sprung
            if (!sprungp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.Space))
                {
                    sprung = true;
                    sprungp = true;
                }
            }
            else
            {
                sprung = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released
                    &&
                    keyState.IsKeyUp(Keys.Space))
                {
                    sprungp = false;
                }
            }
            //Schlag
            if (!hitp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.C))
                {
                    hit = true;
                    hitp = true;
                }
            }
            else
            {
                hit = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Released
                    &&
                    keyState.IsKeyUp(Keys.C))
                {
                    hitp = false;
                }
            }
            //Smash
            if (!shitp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.V))
                {
                    shit = true;
                    shitp = true;
                }
            }
            else
            {
                shit = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Released
                    &&
                    keyState.IsKeyUp(Keys.V))
                {
                    shitp = false;
                }
            }

            //Dynamische Camera Rechtsbewegung
            cameraDynR = GamePad.GetState(PlayerIndex.One).Triggers.Right;
            //Dynamische Camera Linksbewegung
            cameraDynL = GamePad.GetState(PlayerIndex.One).Triggers.Left;


            //itembenutzung
            if (!itemup1)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.F))
                {
                    itemu1 = true;
                    itemup1 = true;
                }
            }
            else
            {
                itemu1 = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed
                    &&
                    keyState.IsKeyUp(Keys.F))
                {
                    itemup1 = false;
                }
            }
            if (!itemup2)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed
                    ||
                    keyState.IsKeyDown(Keys.G))
                {
                    itemu2 = true;
                    itemup2 = true;
                }
            }
            else
            {
                itemu2 = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed
                    &&
                    keyState.IsKeyUp(Keys.G))
                {
                    itemup2 = false;
                }
            }

            // --- Allgemeiner Stuff --- //

            //Rechts
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0f
                ||
                GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed
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
            //Links
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0f
                ||
                GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed
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
            if (!enterp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed
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
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Released
                    && 
                    keyState.IsKeyUp(Keys.Enter))
                {
                    enterp = false;
                }
            }
            if (!downp)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0f
                    || 
                    GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed
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
                    GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Released
                    &&
                    keyState.IsKeyUp(Keys.Down)
                    &&
                    keyState.IsKeyUp(Keys.S))
                {
                    downp = false;
                }
            }
            if (!upp)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0f
                    ||
                    GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed 
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
                    GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Released 
                    &&
                    keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.W))
                {
                    upp = false;
                }
            }
            if (!backp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    ||
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    back = true;
                    backp = true;
                }
            }
            else
            {
                back = false;
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released
                    &&
                    keyState.IsKeyUp(Keys.Escape))
                {
                    backp = false;
                }
            }
            if (!debugp)
            {
                if (keyState.IsKeyDown(Keys.Tab) == true
                    ||
                    GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Pressed)
                {
                    debug = true;
                    debugp = true;
                }
            }
            else
            {
                debug = false;
                if (keyState.IsKeyUp(Keys.Tab) == true
                    &&
                    GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Released)
                {
                    debugp = false;
                }
            }
            if (!fullscreenp)
            {
                if (keyState.IsKeyDown(Keys.F11) == true)
                {
                    fullscreen = true;
                    fullscreenp = true;
                }
            }
            else
            {
                fullscreen = false;
                if (keyState.IsKeyUp(Keys.F11) == true)
                {
                    fullscreenp = false;
                }
            }
        }

    }
}
