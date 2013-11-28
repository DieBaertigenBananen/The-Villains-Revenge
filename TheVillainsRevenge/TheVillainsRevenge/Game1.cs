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
Deine Mudda nutzt den Telefonjoker, um zu fragen, welche Farbe das weiﬂe Haus hat.
 * 
 * */
namespace TheVillainsRevenge
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Vector2 resolution = new Vector2(1920, 1080);
        GameScreen game;
        MenuScreen menu;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth / 16 * 9;
            graphics.IsFullScreen = false;
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
            menu = new MenuScreen();
            //game = new GameScreen();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu.load(Content);
            //game.load(Content);
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
                int wert = 0;
                if (menu != null)
                {
                    wert = menu.update();
                    if (wert == 2)
                    {
                        menu = null;
                        game = new GameScreen();
                        game.load(Content);
                    }
                }
                else if (game != null)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        game = null;
                        menu = new MenuScreen();
                        menu.load(Content);
                        wert = 1;
                    }
                    else
                    {
                        wert = game.update(gameTime);
                    }
                }
                if (wert == 0)
                    this.Exit();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (menu != null)
                menu.draw(spriteBatch);
            else if (game != null)
                game.draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
