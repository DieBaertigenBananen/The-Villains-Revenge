using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    static class Cutscene
    {

        static Video start = null;
        static Video intro = null;
        static Video final = null;
        static Video credits = null;
        public static VideoPlayer player;

        static public void Load(ContentManager Content)
        {
            start = Content.Load<Video>("scenes/start");
            intro = Content.Load<Video>("scenes/intro");
            final = Content.Load<Video>("scenes/bossfight");
            credits = Content.Load<Video>("scenes/credits");
            player = new VideoPlayer();
        }

        static public void Play(string scene)
        {
            Video tempVideo = null;
            switch (scene)
            {
                case "start":
                    tempVideo = start;
                    break;
                case "intro":
                    tempVideo = intro;
                    break;
                case "final":
                    tempVideo = final;
                    break;
                case "credits":
                    tempVideo = credits;
                    break;
            }
            player.Play(tempVideo);
            player.IsMuted = !Game1.sound;
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(player.GetTexture(), Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Game1.resolution.Y / player.Video.Height, SpriteEffects.None, 0f);
        }
    }
}
