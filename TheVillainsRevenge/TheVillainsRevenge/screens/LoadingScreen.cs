using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class LoadingScreen
    {
        RenderTarget2D renderScreen;
        RenderTarget2D renderBackground;
        RenderTarget2D renderTiles;
        Texture2D bg_texture;
        Texture2D logo_texture;
        Texture2D overlay_texture;
        public Texture2D[] tile;
        Vector2[] tilePosition;

        public LoadingScreen()
        {

        }

        public void Load(ContentManager Content)
        {
            renderScreen = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderBackground = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderTiles = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            bg_texture = Content.Load<Texture2D>("sprites/menu/background");
            logo_texture = Content.Load<Texture2D>("sprites/menu/logo");
            overlay_texture = Content.Load<Texture2D>("sprites/menu/overlay");
            tile = new Texture2D[7];
            tilePosition = new Vector2[7];
            for (int i = 0; i < 7; i++)
            {
                tile[i] = Content.Load<Texture2D>("sprites/loading/" + (i + 1) + "0");
            }
            tilePosition[0] = new Vector2(960 - (tile[1].Width / 2), 900);
            tilePosition[1] = new Vector2(960 - (tile[1].Width / 2), 850);
            tilePosition[2] = new Vector2(960 - (tile[1].Width / 2), 800);
            tilePosition[3] = new Vector2(960 - (tile[1].Width / 2), 750);
            tilePosition[4] = new Vector2(960 - (tile[1].Width / 2), 700);
            tilePosition[5] = new Vector2(960 - (tile[1].Width / 2), 650);
            tilePosition[6] = new Vector2(960 - (tile[1].Width / 2), 600);
        }

        public void UpdateDraw(int state, SpriteBatch spriteBatch, GameTime gameTime, Camera camera)
        {
            //----------------------------------------------------------------------
            //----------------------------------------Update
            //----------------------------------------------------------------------
            camera.UpdateTransformation(Game1.graphics);

            //----------------------------------------------------------------------
            //----------------------------------------Draw
            //----------------------------------------------------------------------
            //--------------------Draw to renderBackground--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderBackground);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
                spriteBatch.Draw(bg_texture, Vector2.Zero, Color.White);
                spriteBatch.Draw(logo_texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            //--------------------Draw to renderTiles--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTiles);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            if (state > 0)
            {
                for (int i = 0; i < state; i++)
                {
                    spriteBatch.Draw(tile[i], tilePosition[i], Color.White);
                }
            }
            spriteBatch.End();

            //--------------------Draw to renderScreen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderScreen);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(renderBackground, Vector2.Zero, Color.White);
                spriteBatch.Draw(renderTiles, Vector2.Zero, Color.White);
                spriteBatch.Draw(overlay_texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            //--------------------Draw to Screen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
                spriteBatch.Draw(renderScreen, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
