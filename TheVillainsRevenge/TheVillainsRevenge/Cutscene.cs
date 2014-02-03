//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content; 
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Graphics;

//namespace TheVillainsRevenge
//{
//    static class Cutscene
//    {

//        static Video start;
//        public static VideoPlayer startInstance;
//        static Video intro;
//        public static VideoPlayer introInstance;
//        static Video final;
//        public static VideoPlayer finalInstance;
//        static Video credits;
//        public static VideoPlayer creditsInstance;

//        static public void Load(ContentManager Content)
//        {
//            start = Content.Load<Video>("scenes/start");
//            intro = Content.Load<Video>("scenes/intro");
//            final = Content.Load<Video>("scenes/final");
//            credits = Content.Load<Video>("scenes/credits");
//        }
//        static public void DrawStart(SpriteBatch spriteBatch)
//        {
//            if (startInstance.State == MediaState.Stopped)
//            {
//                startInstance.Play(start);
//            }
//            spriteBatch.Draw(startInstance.GetTexture(), Vector2.Zero, Color.White);
//        }
//        static public void PlayMenu()
//        {
//            if (Game1.sound)
//            {
//                menuMusicInstance = menuMusic.CreateInstance();
//                menuMusicInstance.Volume = 0.5f;
//                menuMusicInstance.IsLooped = true;
//                menuMusicInstance.Play();
//            }
//        }
//        static public void PlayStart()
//        {
//            if (Game1.sound)
//            {
//                startMusicInstance = startMusic.CreateInstance();
//                startMusicInstance.Volume = 0.5f;
//                startMusicInstance.IsLooped = false;
//                startMusicInstance.Play();
//            }
//        }
//        static public void PauseMenu()
//        {
//            menuMusicInstance.Stop();
//        }
//    }
//}
