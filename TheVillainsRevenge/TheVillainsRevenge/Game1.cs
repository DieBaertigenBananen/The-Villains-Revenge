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
        ParallaxPlane background_1 = new ParallaxPlane();
        ParallaxPlane background_2 = new ParallaxPlane();
        ParallaxPlane background_3 = new ParallaxPlane();
        ParallaxPlane clouds_1 = new ParallaxPlane();
        ParallaxPlane clouds_2 = new ParallaxPlane();
        ParallaxPlane clouds_3 = new ParallaxPlane();
        ParallaxPlane foreground_1 = new ParallaxPlane();
        Rectangle viewport = new Rectangle();
        Matrix viewportTransform;
        Matrix screenTransform;
        RenderTarget2D renderTarget;
        bool stretch;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth / 16 * 9;
            graphics.IsFullScreen = false;
            stretch = false;
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

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
            karte.Generate();
            background_1.Load(this.Content, "background_1");
            background_2.Load(this.Content, "background_2");
            background_3.Load(this.Content, "background_3");
            clouds_1.Load(this.Content, "clouds_1");
            clouds_2.Load(this.Content, "clouds_2");
            clouds_3.Load(this.Content, "clouds_3");
            foreground_1.Load(this.Content, "foreground_1");
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

                if (stretch) //Viewport screenfüllend
                {
                    viewport.X = 0;
                    viewport.Y = 0;
                    viewport.Width = GraphicsDevice.PresentationParameters.BackBufferWidth;
                    viewport.Height = GraphicsDevice.PresentationParameters.BackBufferHeight;
                }
                else //Viewport mit Offset auf Screen
                {
                    if (viewport.X < viewport.Y) //Balken oben/unten
                    {
                        viewport.Width = (int)GraphicsDevice.PresentationParameters.BackBufferWidth;
                        viewport.Height = (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / resolution.X * resolution.Y);
                    }
                    else //Balken links/rechts
                    { 
                        viewport.Height = (int)GraphicsDevice.PresentationParameters.BackBufferHeight;
                        viewport.Width = (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / resolution.Y * resolution.X);
                    }
                    viewport.X = (GraphicsDevice.PresentationParameters.BackBufferWidth - (int)viewport.Width) / 2;
                    viewport.Y = (GraphicsDevice.PresentationParameters.BackBufferHeight - (int)viewport.Height) / 2;
                    //= viewport.Width / resolution.X;
                    //= viewport.Height / resolution.Y;
                }
                Matrix screenScale = Matrix.CreateScale((float)viewport.Width / resolution.X, (float)viewport.Height / resolution.Y, 1);
                screenTransform = screenScale * Matrix.CreateTranslation(viewport.X, viewport.Y, 0);

                viewportTransform = Matrix.CreateTranslation(-camera.viewport.X, -camera.viewport.Y, 0);

                background_1.Update(karte, camera);
                background_2.Update(karte, camera);
                background_3.Update(karte, camera);
                clouds_1.Update(karte, camera);
                clouds_2.Update(karte, camera);
                clouds_3.Update(karte, camera);
                foreground_1.Update(karte, camera);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Draw to Texture
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportTransform);

            //Hintergrund und Wolken
            background_3.Draw(spriteBatch); //Himmel
            clouds_3.Draw(spriteBatch);
            background_2.Draw(spriteBatch); //Berge
            clouds_2.Draw(spriteBatch);
            background_1.Draw(spriteBatch); //Wald
            clouds_1.Draw(spriteBatch);

            //Spiel
            spieler.Draw(spriteBatch);
            karte.Draw(spriteBatch); //Enthält eine zusätzliche Backgroundebene

            //Vordergrund
            foreground_1.Draw(spriteBatch); //Bäume etc

            spriteBatch.End();

            //Draw Texture to Screen
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, screenTransform);

            spriteBatch.Draw(renderTarget, new Vector2(), Color.White);

            //HUD
            //spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(resolution.X - 300, 90), Color.Black);
            //spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(resolution.X - 300, 110), Color.Black);
            //spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(resolution.X - 300, 130), Color.Black);
            //spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(resolution.X - 300, 150), Color.Black);
            //spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(resolution.X - 300, 170), Color.Black);
            //spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(resolution.X - 300, 190), Color.Black);
            //spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y), new Vector2(resolution.X - 300, 210), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
