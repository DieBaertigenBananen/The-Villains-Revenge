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

        static SoundEffect[] fx = new SoundEffect[10];

        static public void Load(ContentManager Content)
        {
            bgMusic = Content.Load<SoundEffect>("sounds/Level_" + Game1.level + "/background");
            fx[0] = Content.Load<SoundEffect>("sounds/supersmash");
            fx[1] = Content.Load<SoundEffect>("sounds/schlag");
            fx[2] = Content.Load<SoundEffect>("sounds/landing");
            fx[3] = Content.Load<SoundEffect>("sounds/ashbrett_sword_hit");
            fx[4] = Content.Load<SoundEffect>("sounds/ashbrett_sword_swoosh");
            fx[5] = Content.Load<SoundEffect>("sounds/button_press");
            fx[6] = Content.Load<SoundEffect>("sounds/punch_clubber_cloud");
            fx[7] = Content.Load<SoundEffect>("sounds/ausrutscher"); //Bananenschale
            fx[8] = Content.Load<SoundEffect>("sounds/time_shift");
            fx[9] = Content.Load<SoundEffect>("sounds/triggerwall");
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
                    case "ashbrett_attack":
                        fx[3].Play();
                        break;
                    case "ashbrett_miss":
                        fx[4].Play();
                        break;
                    case "button":
                        fx[5].Play();
                        break;
                    case "clubbercloud":
                        fx[6].Play();
                        break;
                    case "ausrutscher":
                        fx[7].Play();
                        break;
                    case "time_shift":
                        fx[8].Play();
                        break;
                    case "triggerwall":
                        fx[9].Play();
                        break;
                }
            }
        }
        static public void PlayBG()
        {
            if (Game1.sound)
            {
                bgMusicInstance = bgMusic.CreateInstance();
                bgMusicInstance.Volume = 0.5f;
                bgMusicInstance.IsLooped = true;
                bgMusicInstance.Play();
            }
        }
    }
}
