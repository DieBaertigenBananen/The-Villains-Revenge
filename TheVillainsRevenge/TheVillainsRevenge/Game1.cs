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
        public static Vector2 cams; // public static von überall zugreifbar mit Game1.cams
        bool check;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            camp.X = 0;
            camp.Y = 0;
            check = false;
            cams.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //cams.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/16*9;
            cams.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else //Falls kein Escape
            {
                //Steuerung
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (!check)
                    {
                        graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                        graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                        check = true;
                    }
                    else
                    {
                        graphics.PreferredBackBufferHeight = (int)cams.Y;
                        graphics.PreferredBackBufferWidth = (int)cams.X;
                        check = false;
                    }
                    graphics.ApplyChanges();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    camp.X--;
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    camp.X++;
                }


                spieler.Update(karte);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = new Viewport(0,
                  (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height -
                  (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 16 * 9)) / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 16 * 9);

            GraphicsDevice.Clear(Color.Black);
            Vector3 screenScalingFactor = new Vector3((float)1, (float)1, 1);
            Matrix trans = Matrix.CreateScale(screenScalingFactor) * Matrix.CreateTranslation(camp.X, camp.Y, 0);
            //Beginne malen10	    
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, trans);
            spieler.Draw(spriteBatch); //Führe Spielermalen aus
            karte.Draw(spriteBatch);
            spriteBatch.DrawString(font, "X: " + spieler.cbox.X, new Vector2(500, 50), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Y: " + spieler.cbox.Y, new Vector2(500, 70), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "W: " + (spieler.cbox.Width + spieler.cbox.X), new Vector2(600, 50), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "H: " + (spieler.cbox.Height + spieler.cbox.Y), new Vector2(600, 70), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(500, 90), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Cam.X: " + (cams.X), new Vector2(500, 110), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Cam.Y: " + (cams.Y), new Vector2(500, 130), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
