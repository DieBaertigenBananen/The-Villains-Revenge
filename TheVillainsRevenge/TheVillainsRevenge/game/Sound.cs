using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework.Audio; 

namespace TheVillainsRevenge
{
    static class Sound
    {
        static SoundEffect bgMusic;
        static SoundEffectInstance bgMusicInstance;

        static SoundEffect[] fx = new SoundEffect[3];

        static public void Load(ContentManager Content)
        {
            bgMusic = Content.Load<SoundEffect>("sounds/Level_" + Game1.level + "/background");
            fx[0] = Content.Load<SoundEffect>("sounds/supersmash");
            fx[1] = Content.Load<SoundEffect>("sounds/schlag");
            fx[2] = Content.Load<SoundEffect>("sounds/landing");
        }
        static public void Play(string sound)
        {
            if (Game1.sound)
            {
                switch (sound)
                {
                    case "superSmash":
                        fx[0].Play();
                        break;
                    case "schlag":
                        fx[1].Play();
                        break;
                    case "land":
                        fx[2].Play();
                        break;
                }
            }
        }
        static public void PlayBG()
        {
            if (Game1.sound)
            {
                bgMusicInstance = bgMusic.CreateInstance();
                bgMusicInstance.IsLooped = true;
                bgMusicInstance.Play();
            }
        }
    }
}
