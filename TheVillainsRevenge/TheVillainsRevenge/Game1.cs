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
        public static Vector2 resolution = new Vector2(1920, 1080);
        Player spieler = new Player(10, 0);
        Map karte = new Map();
        Camera camera = new Camera(resolution);
        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            camera.changeresolution(graphics, 960, 576, false);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else //Falls kein Escape
            {
                spieler.Update(gameTime, karte);

                camera.changeresolution(graphics, Window.ClientBounds.Width, Window.ClientBounds.Height, camera.full);
                /*GraphicsDevice.Viewport = new Viewport(0,
                      ((int)Window.ClientBounds.Height -
                    ((int)Window.ClientBounds.Width / 16 * 9)) / 2, (int)Window.ClientBounds.Width, (int)Window.ClientBounds.Width / 16 * 9);
                */
                camera.Update(spieler, karte);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.cammatrix);
            spieler.Draw(spriteBatch); //Führe Spielermalen aus
            karte.Draw(spriteBatch);
            spriteBatch.DrawString(font, "X: " + spieler.cbox.X, new Vector2(500, 50), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Y: " + spieler.cbox.Y, new Vector2(500, 70), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "W: " + (spieler.cbox.Width + spieler.cbox.X), new Vector2(600, 50), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "H: " + (spieler.cbox.Height + spieler.cbox.Y), new Vector2(600, 70), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(500, 90), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(500, 110), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(500, 130), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(500, 150), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(500, 170), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Player: " + (spieler.pos.X + " " + spieler.pos.Y), new Vector2(500, 190), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Camera: " + (camera.position.X + " " + camera.position.Y), new Vector2(500, 210), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
