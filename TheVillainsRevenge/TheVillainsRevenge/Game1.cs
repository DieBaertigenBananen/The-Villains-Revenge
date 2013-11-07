using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
/*
 * 
 * Was geht???!!???
 * 
 * 
Deine Mudda nutzt den Telefonjoker, um zu fragen, welche Farbe das weiße Haus hat.
 * 
 * */
namespace TheVillainsRevenge
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Player spieler = new Player();
        Map karte = new Map();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            karte.Generate();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = this.Content.Load<SpriteFont>("fonts/schrift");
            spieler.Load(this.Content);
            karte.Load(this.Content);
        }

        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            else //Falls kein Escape
            {
                //Lade Keyboard-Daten
                KeyboardState currentKeyboardState = Keyboard.GetState();
                    if (currentKeyboardState.IsKeyDown(Keys.Right) == true) //Wenn Rechte Pfeiltaste
                    {
                        spieler.Move(1, karte.blocks); //Bewege Rechts
                    }
                    else if (currentKeyboardState.IsKeyDown(Keys.Left) == true)//Wenn Linke Pfeiltaste
                    {
                        spieler.Move(2, karte.blocks);//Bewege Links
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Up) == true)//Wenn Oben Pfeiltaste
                    {
                        spieler.Move(3, karte.blocks);//Bewege Oben
                    }
                    else if (currentKeyboardState.IsKeyDown(Keys.Down) == true)//Wenn Unten Pfeiltaste
                    {
                        spieler.Move(4, karte.blocks);//Bewege Unten
                    }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Beginne malen
            spriteBatch.Begin();
            spieler.Draw(spriteBatch); //Führe Spielermalen aus
            karte.Draw(spriteBatch);
            foreach (Block block in karte.blocks)
            {
                spriteBatch.DrawString(font, "X: " + block.cbox.X, new Vector2(500, 10), Color.Black);
                spriteBatch.DrawString(font, "Y: " + block.cbox.Y, new Vector2(500, 30), Color.Black);
                spriteBatch.DrawString(font, "W: " + (block.cbox.Width + block.cbox.X), new Vector2(600, 10), Color.Black);
                spriteBatch.DrawString(font, "H: " + (block.cbox.Height + block.cbox.Y), new Vector2(600, 30), Color.Black);
            }
            spriteBatch.DrawString(font, "X: " + spieler.cbox.X, new Vector2(500, 50), Color.Black);
            spriteBatch.DrawString(font, "Y: " + spieler.cbox.Y, new Vector2(500, 70), Color.Black);
            spriteBatch.DrawString(font, "W: " + (spieler.cbox.Width+spieler.cbox.X), new Vector2(600, 50), Color.Black);
            spriteBatch.DrawString(font, "H: " + (spieler.cbox.Height + spieler.cbox.Y), new Vector2(600, 70), Color.Black);
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
