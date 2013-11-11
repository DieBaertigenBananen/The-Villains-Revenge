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
        Camera camera = new Camera();
        Rectangle viewport = new Rectangle();
        Matrix viewportTransform;
        float screenScale;
        RenderTarget2D renderTarget;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800 / 16 * 9;
            karte.Generate();
            graphics.IsFullScreen = false;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
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

                camera.Update(graphics, spieler, karte);


                //Kamera an Fenster anpassen
                //Breite, Höhe des Fensters
                //Viewport anpassen
                int width = GraphicsDevice.PresentationParameters.BackBufferWidth;
                int height = GraphicsDevice.PresentationParameters.BackBufferHeight;
                viewport.Width = (int)(height / resolution.Y * resolution.X);
                viewport.Height = (int)(width / resolution.X * resolution.Y);
                viewport.X = ((int)viewport.Width - width) / 2;
                viewport.Y = ((int)viewport.Height - height) / 2;
                if (viewport.X < viewport.Y)
                {
                    //Balken oben/unten
                    viewport.Width = (int)width;
                    screenScale = viewport.Width / resolution.X;
                    viewport.X = 0;
                }
                else
                {
                    //Balken links/rechts
                    viewport.Height = (int)height;
                    screenScale = viewport.Height / resolution.Y;
                    viewport.Y = 0;
                }

                //Render/Scaling anpassen
                Vector2 scaling = new Vector2();
                scaling.X = (float)viewport.Width / resolution.X;
                scaling.Y = (float)viewport.Height / resolution.Y;
                graphics.ApplyChanges();

                viewportTransform = Matrix.CreateTranslation(-camera.viewport.X, -camera.viewport.Y, 0);
                viewportTransform = Matrix.CreateScale(1.0f);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Draw to Texture
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportTransform);
            spieler.Draw(spriteBatch);
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
            spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y), new Vector2(500, 210), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            //Draw Texture to Screen
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(screenScale));
            //spriteBatch.Draw(renderTarget, new Vector2(viewport.X/2, viewport.Y/2), Color.White);
            spriteBatch.Draw(renderTarget, new Vector2(0,0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
