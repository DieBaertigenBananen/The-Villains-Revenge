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
        Vector2 camp;
        Vector2 cams;
        bool check;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            camp.X = 0;
            camp.Y = 0;
            check = false;
            cams.X = 840;
            cams.Y = 500;
            graphics.PreferredBackBufferHeight = (int)cams.Y;
            graphics.PreferredBackBufferWidth = (int)cams.X;
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
                //Steuerung
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    if (!check)
                    {
                        graphics.PreferredBackBufferHeight = 600;
                        graphics.PreferredBackBufferWidth = 1008;
                        check = true;
                    }
                    else
                    {
                        graphics.PreferredBackBufferHeight =(int) cams.Y;
                        graphics.PreferredBackBufferWidth = (int) cams.X;
                        check = false;
                    }
                    graphics.ApplyChanges();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    camp.X--;
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                {
                    camp.X++;
                }


                spieler.Update(karte);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector3 screenScalingFactor = new Vector3((float)(graphics.PreferredBackBufferWidth / cams.X), (float)(graphics.PreferredBackBufferHeight / cams.Y), 1);
            Matrix trans = Matrix.CreateScale(screenScalingFactor) * Matrix.CreateTranslation(camp.X, camp.Y, 0);
            //Beginne malen10	    
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,trans);
            spieler.Draw(spriteBatch); //Führe Spielermalen aus
            karte.Draw(spriteBatch);
            spriteBatch.DrawString(font, "X: " + spieler.cbox.X, new Vector2(500, 50), Color.Black);
            spriteBatch.DrawString(font, "Y: " + spieler.cbox.Y, new Vector2(500, 70), Color.Black);
            spriteBatch.DrawString(font, "W: " + (spieler.cbox.Width+spieler.cbox.X), new Vector2(600, 50), Color.Black);
            spriteBatch.DrawString(font, "H: " + (spieler.cbox.Height + spieler.cbox.Y), new Vector2(600, 70), Color.Black);
            spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(500, 90), Color.Black);
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
