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
                bool cr = false;
                bool cl = false;
                bool ct = false;
                bool cb = false;
                //Gehe die Blöcke der Karte durch
                foreach (Block block in karte.blocks)
                {
                    //Eigene Collision , wenn Kollision nicht gleich 0 ,eine Seite also kollidiert
                    cl = spieler.cbox.Intersect(block.cbox, 1);
                    cr = spieler.cbox.Intersect(block.cbox, 2);
                    ct = spieler.cbox.Intersect(block.cbox, 3);
                    cb = spieler.cbox.Intersect(block.cbox, 4);
                }
                KeyboardState currentKeyboardState = Keyboard.GetState();
                    if (currentKeyboardState.IsKeyDown(Keys.Right) == true&&!cr) //Wenn Rechte Pfeiltaste
                    {
                        spieler.Move(1); //Bewege Rechts
                    }
                    else if (currentKeyboardState.IsKeyDown(Keys.Left) == true && !cl)//Wenn Linke Pfeiltaste
                    {
                        spieler.Move(2);//Bewege Links
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Up) == true && !ct)//Wenn Oben Pfeiltaste
                    {
                        spieler.Move(3);//Bewege Oben
                    }
                    else if (currentKeyboardState.IsKeyDown(Keys.Down) == true && !cb)//Wenn Unten Pfeiltaste
                    {
                        spieler.Move(4);//Bewege Unten
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
                spriteBatch.DrawString(font, "X: " + block.cbox.pos.X, new Vector2(500, 10), Color.Black);
                spriteBatch.DrawString(font, "Y: " + block.cbox.pos.Y, new Vector2(500, 30), Color.Black);
                spriteBatch.DrawString(font, "W: " + (block.cbox.width + block.cbox.pos.X), new Vector2(600, 10), Color.Black);
                spriteBatch.DrawString(font, "H: " + (block.cbox.height + block.cbox.pos.Y), new Vector2(600, 30), Color.Black);
            }
            spriteBatch.DrawString(font, "X: " + spieler.cbox.pos.X, new Vector2(500, 50), Color.Black);
            spriteBatch.DrawString(font, "Y: " + spieler.cbox.pos.Y, new Vector2(500, 70), Color.Black);
            spriteBatch.DrawString(font, "W: " + (spieler.cbox.width+spieler.cbox.pos.X), new Vector2(600, 50), Color.Black);
            spriteBatch.DrawString(font, "H: " + (spieler.cbox.height + spieler.cbox.pos.Y), new Vector2(600, 70), Color.Black);
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
