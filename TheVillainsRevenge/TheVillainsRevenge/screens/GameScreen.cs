﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class GameScreen
    {
        Player spieler = new Player(40, 100);
        Hero hero = new Hero(0, 0);
        Map karte = new Map();
        Camera camera = new Camera();
        GUI gui = new GUI();
        ParallaxPlane background_1 = new ParallaxPlane();
        ParallaxPlane background_2 = new ParallaxPlane();
        ParallaxPlane background_3 = new ParallaxPlane();
        ParallaxPlane clouds_1 = new ParallaxPlane();
        ParallaxPlane clouds_2 = new ParallaxPlane();
        ParallaxPlane clouds_3 = new ParallaxPlane();
        ParallaxPlane foreground_1 = new ParallaxPlane();
        RenderTarget2D renderTarget;
        RenderTarget2D renderSpine;
        SpriteFont font;
        List<Enemy> enemies = new List<Enemy>(); //Erstelle Blocks als List

        public GameScreen()
        {
            enemies.Add(new Enemy(1200, 0, 1));
            enemies.Add(new Enemy(2300, 0, 1));
        }

        public void Load(ContentManager Content)
        {
            renderTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            font = Content.Load<SpriteFont>("fonts/schrift");
            spieler.Load(Content, Game1.graphics);
            hero.Load(Content);
            karte.Load(Content);
            karte.Generate();
            background_1.Load(Content, "background_1", 4);
            background_2.Load(Content, "background_2", 2);
            background_3.Load(Content, "background_3", 1);
            clouds_1.Load(Content, "clouds_1", 6);
            clouds_2.Load(Content, "clouds_2", 5);
            clouds_3.Load(Content, "clouds_3", 3);
            foreground_1.Load(Content, "foreground_1", 7);
            gui.Load(Content);
            foreach (Enemy enemy in enemies)
            {
                enemy.Load(Content);

            }
        }

        public int Update(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime, karte);
                if (spieler.cbox.Intersects(enemy.cbox))
                {
                    spieler.getHit();
                    enemies.Remove(enemy);
                    break;
                }
            }
            foreach (Item item in karte.items)
            {
                if (spieler.cbox.Intersects(item.cbox))
                {
                    if (item.type == "herz")
                    {
                        if(spieler.lifes != 4)
                            spieler.lifes++;
                        karte.items.Remove(item);
                    }
                    break;
                }
            }
            foreach (Checkpoint cpoint in karte.checkpoints)
            {
                if (spieler.cbox.Intersects(cpoint.cbox))
                {
                    spieler.checkpoint.X = cpoint.cbox.X;
                    spieler.checkpoint.Y = spieler.position.Y;
                    break;
                }
            }
            hero.Update(gameTime, karte, spieler.position);
            spieler.Update(gameTime, karte);
            camera.Update(Game1.graphics, spieler, karte);
            background_1.Update(karte, camera);
            background_2.Update(karte, camera);
            background_3.Update(karte, camera);
            clouds_1.Update(karte, camera);
            clouds_2.Update(karte, camera);
            clouds_3.Update(karte, camera);
            foreground_1.Update(karte, camera);
            if (spieler.lifes != 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw to Spine
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spieler.Draw(gameTime, camera);

            //Draw to Texture
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            //Hintergrund und Wolken
            background_3.Draw(spriteBatch); //Himmel
            clouds_3.Draw(spriteBatch);
            background_2.Draw(spriteBatch); //Berge
            clouds_2.Draw(spriteBatch);
            background_1.Draw(spriteBatch); //Wald
            clouds_1.Draw(spriteBatch);
            //Spiel
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White);
            hero.Draw(spriteBatch);
            karte.Draw(spriteBatch); //Enthält eine zusätzliche Backgroundebene
            //Vordergrund
            foreground_1.Draw(spriteBatch); //Bäume etc
            spriteBatch.End();

            //Draw Texture to Screen
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);

            spriteBatch.Draw(renderTarget, new Vector2(), Color.White);
            gui.Draw(spriteBatch, spieler.lifes, spieler.position, hero.position, karte.size, spieler.item1, spieler.item2);

            

            spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(Game1.resolution.X - 300, 90), Color.Black);
            spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(Game1.resolution.X - 300, 110), Color.Black);
            spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(Game1.resolution.X - 300, 130), Color.Black);
            spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(Game1.resolution.X - 300, 150), Color.Black);
            spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(Game1.resolution.X - 300, 170), Color.Black);
            spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(Game1.resolution.X - 300, 190), Color.Black);
            spriteBatch.DrawString(font, "Hero: " + (hero.position.X + " " + hero.position.Y), new Vector2(Game1.resolution.X - 300, 210), Color.Black);
            spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y), new Vector2(Game1.resolution.X - 300, 230), Color.Black);
            spriteBatch.DrawString(font, "Skeleton: " + (spieler.skeleton.X + " " + spieler.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.Black);
            spriteBatch.DrawString(font, "Planes.Size.X: " + background_1.size.X + " " + background_2.size.X + " " + background_3.size.X + " " + clouds_1.size.X + " " + clouds_2.size.X + " " + clouds_3.size.X + " " + foreground_1.size.X, new Vector2(Game1.resolution.X - 700, 270), Color.Black);
            spriteBatch.DrawString(font, "Kollision: " + spieler.check, new Vector2(Game1.resolution.X - 300, 290), Color.Black);
            Slot bb = spieler.skeleton.FindSlot("bb-head");
            spriteBatch.DrawString(font, "bb-head: " + spieler.bounds.BoundingBoxes.FirstOrDefault(), new Vector2(Game1.resolution.X - 300, 310), Color.Black);
            //for (int i = 0; i < karte.background.Width; i++)
            //{
            //    for (int t = 15; t < karte.background.Height; t++)
            //    {
            //        spriteBatch.DrawString(font, karte.pixelRGBA[i, t, 0] + "," + karte.pixelRGBA[i, t, 1] + "," + karte.pixelRGBA[i, t, 2], new Vector2(i * 60, (t - 15) * 30), Color.Black);
            //    }
            //}*/
            
            spriteBatch.End();
        }
    }
}