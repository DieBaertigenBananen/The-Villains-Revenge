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

        static Video start;
        static Video intro;
        static Video final;
        static Video credits;
        public static VideoPlayer player;

        static public void Load(ContentManager Content)
        {
            start = Content.Load<Video>("scenes/start");
            intro = Content.Load<Video>("scenes/intro");
            final = Content.Load<Video>("scenes/final");
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
        }
    }
}
