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
        GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteFont font = this.Content.Load<SpriteFont>("fonts/schrift");
        RenderTarget2D renderTarget;
        Player spieler = new Player(10, 0);
        Map karte = new Map();
        Camera camera = new Camera(resolution);
        public Game1()
        {
            this.Window.AllowUserResizing = true;
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice,1920,1080); //the RenderTarget2D that we will draw to
            camera.changeresolution(graphics, 960, 576);
            karte.Generate();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
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

                camera.changeresolution(graphics, Window.ClientBounds.Width, Window.ClientBounds.Height);
                camera.togglefullscreen(graphics, false);
                camera.Update(spieler, karte);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            
            
            // change the scale of our rendered scene to the backbuffer
            float renderTargetScale = 1f;



            // Example of drawing to the screen. You can use multiple sprite batches if you need to
            Vector2 fullscreen = new Vector2(1920, 1080);
 

            // Draw to the renderTarget instead of the back buffer
            GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.cammatrix);
            GraphicsDevice.Clear(Color.Transparent);
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

            // Now switch back to the back buffer as our rendering target
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Transparent);
            // Draw our renderTarget to the back buffer
            spriteBatch.Draw(renderTarget, new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth / 2, GraphicsDevice.PresentationParameters.BackBufferHeight / 2), null, Color.White, 0f, new Vector2(renderTarget.Width / 2, renderTarget.Height / 2), renderTargetScale, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);  
        }
    }
}
