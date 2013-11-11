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
        Player spieler = new Player(10, 10);
        Map karte = new Map();
        Cam cam = new Cam(resolution);
        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            cam.changeresolution(graphics, 960, 576, false);
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
                cam.position.X = spieler.pos.X - (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width*0.4); //Scrolling seitlich
                if (cam.position.X < 0) //Linker Maprand
                {
                    cam.position.X = 0;
                }
                else if (cam.position.X > resolution.X - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) //Rechter Maprand
                {
                    cam.position.X = resolution.X - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                }
                if (cam.position.Y + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.2) > spieler.pos.Y) //Scrolling nach oben
                {
                    cam.position.Y = spieler.pos.Y - (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.2);
                }
                else if (cam.position.Y + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8) < spieler.pos.Y) //Scrolling nach unten
                {
                    cam.position.Y = spieler.pos.Y - (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8);
                }
                if (cam.position.Y < 0) //Oberer Maprand
                {
                    cam.position.Y = 0;
                }
                else if (cam.position.Y > resolution.Y - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) //Unterer Maprand
                {
                    cam.position.Y = resolution.Y - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
                cam.changeresolution(graphics, Window.ClientBounds.Width, Window.ClientBounds.Height,cam.full);
                /*GraphicsDevice.Viewport = new Viewport(0,
                      ((int)Window.ClientBounds.Height -
                    ((int)Window.ClientBounds.Width / 16 * 9)) / 2, (int)Window.ClientBounds.Width, (int)Window.ClientBounds.Width / 16 * 9);
                */cam.update();

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.cammatrix);
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
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
