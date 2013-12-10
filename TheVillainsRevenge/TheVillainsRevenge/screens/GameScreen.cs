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

namespace TheVillainsRevenge
{
    class GameScreen
    {
        Texture2D texture;
        Player spieler = new Player(40, 100);
        Hero hero = new Hero(0, 0);
        Map karte = new Map();
        Camera camera = new Camera();
        GUI gui = new GUI();
        ParallaxPlane background_0 = new ParallaxPlane();
        ParallaxPlane background_1 = new ParallaxPlane();
        ParallaxPlane background_2 = new ParallaxPlane();
        ParallaxPlane background_3 = new ParallaxPlane();
        ParallaxPlane clouds_1 = new ParallaxPlane();
        ParallaxPlane clouds_2 = new ParallaxPlane();
        ParallaxPlane clouds_3 = new ParallaxPlane();
        RenderTarget2D renderTarget;
        RenderTarget2D renderSpine;
        SpriteFont font;
        List<Enemy> enemies = new List<Enemy>(); //Erstelle Blocks als List
        bool levelend = false;
        Effect coverEyes;

        public GameScreen()
        {
            enemies.Add(new Enemy(1200, 0, 1));
            enemies.Add(new Enemy(2300, 0, 1));
            texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
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
            background_0.Load(Content, "background_0", 7);
            background_1.Load(Content, "background_1", 4);
            background_2.Load(Content, "background_2", 2);
            background_3.Load(Content, "background_3", 1);
            clouds_1.Load(Content, "clouds_1", 6);
            clouds_2.Load(Content, "clouds_2", 5);
            clouds_3.Load(Content, "clouds_3", 3);
            gui.Load(Content);
            foreach (Enemy enemy in enemies)
            {
                enemy.Load(Content);

            }
            coverEyes = Content.Load<Effect>("CoverEyes");
        }

        public int Update(GameTime gameTime)
        {
            if (!levelend)
            {

                foreach (Enemy enemy in enemies)
                {
                    enemy.Update(gameTime, karte);
                    if (enemy.position.X < -enemy.cbox.Width || enemy.position.Y < -enemy.cbox.Height || enemy.position.X > karte.size.X || enemy.position.Y > karte.size.Y)
                    {
                        enemies.Remove(enemy);
                        break;
                    }
                    if (spieler.cbox.box.Intersects(enemy.cbox))
                     {
                         spieler.getHit();
                         enemies.Remove(enemy);
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
                            spieler.checkpoint.X = cpoint.x;
                            spieler.checkpoint.Y = spieler.position.Y;
                        }
                        break;
                    }
                }
                hero.Update(gameTime, karte, spieler.position);
                spieler.Update(gameTime, karte);
                camera.Update(Game1.graphics, spieler, karte);
                background_0.Update(karte, camera);
                background_1.Update(karte, camera);
                background_2.Update(karte, camera);
                background_3.Update(karte, camera);
                clouds_1.Update(karte, camera);
                clouds_2.Update(karte, camera);
                clouds_3.Update(karte, camera);
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
            //Draw to Spine
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderSpine);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spieler.Draw(gameTime, camera);

            //Draw to Texture
            Game1.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            //Hintergrund und Wolken
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
            //Spiel
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            spriteBatch.Draw(renderSpine, new Vector2(camera.viewport.X, camera.viewport.Y), Color.White);
            if (Game1.debug)
            {
                spriteBatch.Draw(texture, spieler.cbox.box, null, Color.White);
            }
            hero.Draw(spriteBatch);
            karte.Draw(spriteBatch); //Enthält eine zusätzliche Backgroundebene
            spriteBatch.End();

            //Shader


            //Draw Texture to Screen
            Game1.graphics.GraphicsDevice.SetRenderTarget(null);
            Game1.graphics.GraphicsDevice.Clear(Color.Black);
            
            //Shader
            coverEyes.Parameters["playerX"].SetValue((spieler.position.X - camera.viewport.X) / camera.viewport.Width);
            coverEyes.Parameters["playerY"].SetValue((spieler.position.Y - camera.viewport.Y) / camera.viewport.Height);
            coverEyes.Parameters["gameTime"].SetValue(gameTime.TotalGameTime.Milliseconds);
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
            if (levelend)
            {
                spriteBatch.DrawString(font, "Finished level!", new Vector2(Game1.resolution.X - 300, Game1.resolution.Y/2), Color.Black);
            }
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.screenTransform);
            gui.Draw(spriteBatch, spieler.lifes, spieler.position, hero.position, karte.size, spieler.item1, spieler.item2);
            if (Game1.debug)
            {
                spriteBatch.DrawString(font, "Speed: " + (spieler.speed), new Vector2(Game1.resolution.X - 300, 90), Color.White);
                spriteBatch.DrawString(font, "Falltimer: " + (spieler.falltimer), new Vector2(Game1.resolution.X - 300, 110), Color.White);
                spriteBatch.DrawString(font, "Fall: " + (spieler.fall), new Vector2(Game1.resolution.X - 300, 130), Color.White);
                spriteBatch.DrawString(font, "Jumptimer: " + (spieler.jumptimer), new Vector2(Game1.resolution.X - 300, 150), Color.White);
                spriteBatch.DrawString(font, "Jump: " + (spieler.jump), new Vector2(Game1.resolution.X - 300, 170), Color.White);
                spriteBatch.DrawString(font, "Player: " + (spieler.position.X + " " + spieler.position.Y), new Vector2(Game1.resolution.X - 300, 190), Color.White);
                spriteBatch.DrawString(font, "Hero: " + (hero.position.X + " " + hero.position.Y), new Vector2(Game1.resolution.X - 300, 210), Color.White);
                spriteBatch.DrawString(font, "Camera: " + (camera.viewport.X + " " + camera.viewport.Y), new Vector2(Game1.resolution.X - 300, 230), Color.White);
                spriteBatch.DrawString(font, "Skeleton: " + (spieler.skeleton.X + " " + spieler.skeleton.Y), new Vector2(Game1.resolution.X - 300, 250), Color.White);
                spriteBatch.DrawString(font, "Planes.Size.X: " + background_1.size.X + " " + background_2.size.X + " " + background_3.size.X + " " + clouds_1.size.X + " " + clouds_2.size.X + " " + clouds_3.size.X + " " + background_0.size.X, new Vector2(Game1.resolution.X - 700, 270), Color.White);
                spriteBatch.DrawString(font, "Kollision: " + spieler.check, new Vector2(Game1.resolution.X - 300, 290), Color.White);
                Slot bb = spieler.skeleton.FindSlot("bonepuker");
                spriteBatch.DrawString(font, "bb-bonepuker: " + spieler.bounds.BoundingBoxes.FirstOrDefault(), new Vector2(Game1.resolution.X - 300, 310), Color.White);
                //for (int i = 0; i < karte.background.Width; i++)
                //{
                //    for (int t = 15; t < karte.background.Height; t++)
                //    {
                //        spriteBatch.DrawString(font, karte.pixelRGBA[i, t, 0] + "," + karte.pixelRGBA[i, t, 1] + "," + karte.pixelRGBA[i, t, 2], new Vector2(i * 60, (t - 15) * 30), Color.Black);
                //    }
                //}*/
            }
            spriteBatch.End();
        }
    }
}
