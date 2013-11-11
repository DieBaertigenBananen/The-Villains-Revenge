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
<<<<<<< HEAD
        public static Vector2 resolution = new Vector2(1920, 1080);
        Player spieler = new Player();
        Map karte = new Map();
        Cam cam = new Cam(resolution);
=======
        Player spieler = new Player(10, 10);
        Map karte = new Map();
        Camera camera = new Camera();

        public static Vector2 cams = new Vector2(1920, 1080); // public static von überall zugreifbar mit Game1.cams
>>>>>>> parent of 532732f... Cam Test

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
<<<<<<< HEAD
            this.Window.AllowUserResizing = true;
            cam.changeresolution(graphics, 960, 576, false);
=======
            graphics.IsFullScreen = false;
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferHeight = (int)cams.Y;
            graphics.PreferredBackBufferWidth = (int)cams.X;
>>>>>>> parent of 532732f... Cam Test
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
<<<<<<< HEAD
                //Steuerung
                if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    cam.position.X--;
=======
                spieler.Update(gameTime, karte);
                camera.Update(gameTime, graphics);
                //Kamera an Spieler anpassen
                camera.camp.X = spieler.pos.X - (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width*0.4); //Scrolling seitlich
                if (camera.camp.X < 0) //Linker Maprand
                {
                    camera.camp.X = 0;
                }
                else if (camera.camp.X > cams.X - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) //Rechter Maprand
                {
                    camera.camp.X = cams.X - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
>>>>>>> parent of 532732f... Cam Test
                }
                if (camera.camp.Y + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.2) > spieler.pos.Y) //Scrolling nach oben
                {
<<<<<<< HEAD
                    cam.position.X++;
=======
                    camera.camp.Y = spieler.pos.Y - (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.2);
                }
                else if (camera.camp.Y + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8) < spieler.pos.Y) //Scrolling nach unten
                {
                    camera.camp.Y = spieler.pos.Y - (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8);
                }
                if (camera.camp.Y < 0) //Oberer Maprand
                {
                    camera.camp.Y = 0;
                }
                else if (camera.camp.Y > cams.Y - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) //Unterer Maprand
                {
                    camera.camp.Y = cams.Y - GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
>>>>>>> parent of 532732f... Cam Test
                }
                cam.changeresolution(graphics, Window.ClientBounds.Width, Window.ClientBounds.Height,cam.full);
                GraphicsDevice.Viewport = new Viewport(0,
                      ((int)Window.ClientBounds.Height -
                    ((int)Window.ClientBounds.Width / 16 * 9)) / 2, (int)Window.ClientBounds.Width, (int)Window.ClientBounds.Width / 16 * 9);
                cam.update();

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
<<<<<<< HEAD

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.cammatrix);
=======
            GraphicsDevice.Clear(Color.White);
            Vector3 screenScalingFactor = new Vector3((float)(1), (float)(1), 1);
            Matrix trans = Matrix.CreateScale(screenScalingFactor) * Matrix.CreateTranslation(-camera.camp.X, -camera.camp.Y, 0);
            //Beginne malen10
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,trans);
>>>>>>> parent of 532732f... Cam Test
            spieler.Draw(spriteBatch); //Führe Spielermalen aus
            karte.Draw(spriteBatch);
            spriteBatch.DrawString(font, "X: " + spieler.cbox.X, new Vector2(500, 50), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Y: " + spieler.cbox.Y, new Vector2(500, 70), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "W: " + (spieler.cbox.Width + spieler.cbox.X), new Vector2(600, 50), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "H: " + (spieler.cbox.Height + spieler.cbox.Y), new Vector2(600, 70), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(500, 90), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
<<<<<<< HEAD
            spriteBatch.DrawString(font, "Cam.X: " + (cam.position.X), new Vector2(500, 110), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Cam.Y: " + (cam.position.Y), new Vector2(650, 110), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Res W: " + (Window.ClientBounds.Width), new Vector2(500, 130), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Res H: " + (Window.ClientBounds.Height), new Vector2(650, 130), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
=======
            spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(500, 110), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(500, 130), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(500, 150), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(500, 170), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
>>>>>>> parent of 532732f... Cam Test
            spriteBatch.End();
            //Beende malen
            base.Draw(gameTime);
        }
    }
}
