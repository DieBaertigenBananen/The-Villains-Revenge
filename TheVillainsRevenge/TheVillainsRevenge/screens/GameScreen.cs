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
        public static int test;
        Texture2D texture;
        Player spieler = new Player(40, 1000);
        Hero hero = new Hero(0, 0);
        Map karte = new Map();
        Camera camera = new Camera();
        GUI gui = new GUI();
        ParallaxPlane background_0 = new ParallaxPlane();
        ParallaxPlane background_1 = new ParallaxPlane();
        ParallaxPlane background_2 = new ParallaxPlane();
        ParallaxPlane background_3 = new ParallaxPlane();
        CloudPlane clouds_1 = new CloudPlane(1);
        CloudPlane clouds_2 = new CloudPlane(2);
        CloudPlane clouds_3 = new CloudPlane(3);
        Sound bgmusic = new Sound("sounds/Level_1/background");
        RenderTarget2D renderTarget;
        RenderTarget2D renderSpine;
        SpriteFont font;
        bool levelend = false;
        Effect coverEyes;
        public static int slow = 0;
        double slowTime;

        public GameScreen()
        {
            texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
        }

        public void Load(ContentManager Content)
        {
            renderTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            renderSpine = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1920, 1080);
            font = Content.Load<SpriteFont>("fonts/schrift");
            spieler.Load(Content, Game1.graphics);
            hero.Load(Content, Game1.graphics);
            karte.Load(Content);
            karte.Generate();
            background_0.Load(Content, "background_0", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground0"]));
            background_1.Load(Content, "background_1", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground1"]));
            background_2.Load(Content, "background_2", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground2"]));
            background_3.Load(Content, "background_3", Convert.ToInt32((double)Game1.luaInstance["planeTilesBackground3"]));
            clouds_1.Load(Content, "clouds_1", karte, camera);
            clouds_2.Load(Content, "clouds_2", karte, camera);
            clouds_3.Load(Content, "clouds_3", karte, camera);
            gui.Load(Content);
            coverEyes = Content.Load<Effect>("CoverEyes");
            if(Game1.sound)
                bgmusic.Load(Content);
        }
        public void Save(int x)
        {
            spieler.Save(x);
            hero.Save();
            foreach (Enemy enemy in karte.enemies)
            {
                enemy.Save();
            }
            foreach (Trigger trigger in karte.triggers)
            {
                trigger.Save();
            }
            foreach (MovingBlock mblock in karte.mblocks)
            {
                mblock.Save();
            }
        }
        public void Reset()
        {
            if (spieler.lifes != 0)
            {
                spieler.Reset();
                hero.Reset();
                foreach (Enemy enemy2 in karte.enemies)
                {
                    enemy2.Reset();
                }
                foreach (Trigger trigger in karte.triggers)
                {
                    trigger.Reset(karte.blocks);
                }
                foreach (MovingBlock mblock in karte.mblocks)
                {
                    mblock.Reset();
                }
            }
        }

        public int Update(GameTime gameTime)
        {
            if (!levelend)
            {
                karte.Update(gameTime,spieler.cbox.box);
                foreach (Enemy enemy in karte.enemies)
                {
                    enemy.Update(gameTime, karte);
                    if (enemy.position.X < -enemy.cbox.box.Width || enemy.position.Y < -enemy.cbox.box.Height || enemy.position.X > karte.size.X || enemy.position.Y > karte.size.Y)
                    {
                        karte.enemies.Remove(enemy);
                        break;
                    }
                    if (spieler.cbox.box.Intersects(enemy.cbox.box))
                     {
                         spieler.getHit();
                         Reset();
                         break;
                     }
                }
                foreach (Item item in karte.items)
                {
                    if (spieler.cbox.box.Intersects(item.cbox))
                    {
                        if (item.type == "herz")
                        {
                            if(spieler.lifes != 4)
                                spieler.lifes++;
                            karte.items.Remove(item);
                        }
                        else if (item.type == "zeit")
                        {
                            if(spieler.item1 != 0)
                                spieler.item2 = spieler.item1;
                            spieler.item1 = 1;
                            karte.items.Remove(item);
                        }
                        break;
                    }
                }
                foreach (Checkpoint cpoint in karte.checkpoints)
                {
                    if (spieler.position.X > cpoint.x && spieler.checkpoint.X < cpoint.x)
                    {
                        //TODO: Speichern aller dynamischen Objekte in der Welt um diesen Zustand bei zurücksetzen an Checkpoint exakt zu rekonstruieren.
                        if (cpoint.end)
                            levelend = true;
                        else
                        {
                            Save(cpoint.x);
                        }
                        break;
                    }
                }
                foreach (Trigger trigger in karte.triggers)
                {
                    if (spieler.cbox.box.Intersects(trigger.cbox)&&spieler.fall)
                    {
                        trigger.Pushed(karte.blocks);
                        break;
                    }
                }
                hero.Update(gameTime, karte, spieler.position);
                spieler.Update(gameTime, karte);
                if (spieler.position.Y >= (karte.size.Y))
                {
                    spieler.getHit();
                    Reset();
                }
                if (spieler.cbox.box.Intersects(hero.cbox.box))
                {
                    spieler.lifes = 0;
                }
                camera.Update(Game1.graphics, spieler, karte);
                background_0.Update(karte, camera);
                background_1.Update(karte, camera);
                background_2.Update(karte, camera);
                background_3.Update(karte, camera);
                clouds_1.Update(karte, gameTime, camera);
                clouds_2.Update(karte, gameTime, camera);
                clouds_3.Update(karte, gameTime, camera);
                if (slow != 0)
                {
                    slowTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (slowTime > slow)
                    {
                        slowTime = 0;
                        slow = 0;
                    }
                }
            }
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
            //--------------------Draw to Spine--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spieler.Draw(gameTime, camera);
            hero.Draw(gameTime, camera);


            //--------------------Draw to Texture--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);

            //-----Hintergrundebenen-----
            if (!Game1.debug)
            {
                background_3.Draw(spriteBatch); //Himmel
                clouds_3.Draw(spriteBatch);
                background_2.Draw(spriteBatch); //Berge
                clouds_2.Draw(spriteBatch);
                background_1.Draw(spriteBatch); //Wald
                clouds_1.Draw(spriteBatch);
                background_0.Draw(spriteBatch); //Bäume
            }

            //-----Spielebene-----
            karte.Draw(spriteBatch); //Enthält eine zusätzliche Backgroundebene
            hero.Draw(gameTime,camera); //Ashbrett
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White); //Bonepuker
            if (Game1.debug) //Boundingbox Bonepuker
            {
                spriteBatch.Draw(texture, spieler.cbox.box, null, Color.White);
                spriteBatch.Draw(texture, hero.cbox.box, null, Color.White);
            }
            spriteBatch.End();


            //--------------------Draw to Screen--------------------
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            
            //-----renderTarget-----
            coverEyes.Parameters["playerX"].SetValue((spieler.position.X - camera.viewport.X) / camera.viewport.Width);
            coverEyes.Parameters["playerY"].SetValue((spieler.position.Y - camera.viewport.Y) / camera.viewport.Height);
            coverEyes.Parameters["gameTime"].SetValue(gameTime.TotalGameTime.Milliseconds);
            coverEyes.Parameters["left"].SetValue(spieler.spine.skeleton.FlipX);
            if (spieler.coverEyes)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, coverEyes, camera.screenTransform);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            }
            spriteBatch.Draw(renderTarget, new Vector2(), Color.White);
            spriteBatch.End();

            //-----HUD-----
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            if (levelend)
            {
                spriteBatch.DrawString(font, "Finished level!", new Vector2(Game1.resolution.X - 300, Game1.resolution.Y / 2), Color.Black);
            }
            if (Game1.debug)
            {
                spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(Game1.resolution.X - 300, 90), Color.White);
                spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(Game1.resolution.X - 300, 110), Color.White);
                spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(Game1.resolution.X - 300, 130), Color.White);
                spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(Game1.resolution.X - 300, 150), Color.White);
                spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(Game1.resolution.X - 300, 170), Color.White);
                spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(Game1.resolution.X - 300, 190), Color.White);
                spriteBatch.DrawString(font, "Hero: " + (hero.position.X + " " + hero.position.Y), new Vector2(Game1.resolution.X - 300, 210), Color.White);
                spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y + " " + camera.viewport.Width + " " + camera.viewport.Height), new Vector2(Game1.resolution.X - 300, 230), Color.White);
                spriteBatch.DrawString(font, "Skeleton: " + (spieler.spine.skeleton.X + " " + spieler.spine.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                spriteBatch.DrawString(font, "Planes.Size.X: " + background_1.size.X + " " + background_2.size.X + " " + background_3.size.X + " " + clouds_1.size.X + " " + clouds_2.size.X + " " + clouds_3.size.X + " " + background_0.size.X, new Vector2(Game1.resolution.X - 700, 270), Color.White);
                spriteBatch.DrawString(font, "Skeleton: " + (spieler.spine.skeleton.X + " " + spieler.spine.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                spriteBatch.DrawString(font, "Planes.Size.X: " + background_0.size.X + " " + background_1.size.X + " " + background_2.size.X + " " + background_3.size.X, new Vector2(Game1.resolution.X - 700, 270), Color.White);
                //spriteBatch.DrawString(font, "Kollision: " + spieler.check, new Vector2(Game1.resolution.X - 300, 290), Color.White);
                Slot bb = spieler.spine.skeleton.FindSlot("bonepuker");
                spriteBatch.DrawString(font, "bb-bonepuker: " + spieler.spine.bounds.BoundingBoxes.FirstOrDefault(), new Vector2(Game1.resolution.X - 300, 310), Color.White);
                spriteBatch.DrawString(font, "SlowTime: " + slow + " Vergangen: " + slowTime, new Vector2(Game1.resolution.X - 300, 330), Color.White);
                spriteBatch.DrawString(font, "Test: " + test, new Vector2(Game1.resolution.X - 300, 350), Color.White);
                //for (int i = 0; i < karte.background.Width; i++)
                //{
                //    for (int t = 15; t < karte.background.Height; t++)
                //    {
                //        spriteBatch.DrawString(font, karte.pixelRGBA[i, t, 0] + "," + karte.pixelRGBA[i, t, 1] + "," + karte.pixelRGBA[i, t, 2], new Vector2(i * 60, (t - 15) * 30), Color.Black);
                //    }
                //}*/
            }
            else
            {
                gui.Draw(spriteBatch, spieler.lifes, spieler.position, hero.position, karte.size, spieler.item1, spieler.item2);
            }
            spriteBatch.End();
        }
    }
}
