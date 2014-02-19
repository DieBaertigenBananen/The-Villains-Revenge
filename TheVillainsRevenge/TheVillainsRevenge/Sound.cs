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
        public static SoundEffectInstance bgMusicInstance;
        static SoundEffect menuMusic;
        public static SoundEffectInstance menuMusicInstance;
        static SoundEffect startMusic;
        public static SoundEffectInstance startMusicInstance;

        static SoundEffect[] fx = new SoundEffect[10];
        static SoundEffect[] ashbrett = new SoundEffect[7];
        static SoundEffect[] bonepuker = new SoundEffect[5];
        static SoundEffect[] sweetcheeks = new SoundEffect[6];
        static SoundEffectInstance[] fxInstance = new SoundEffectInstance[2];
        static SoundEffect[] enemy = new SoundEffect[5];

        static public void Load(ContentManager Content)
        {
            bgMusic = Content.Load<SoundEffect>("sounds/Level_" + Game1.level + "/background");
            startMusic = Content.Load<SoundEffect>("sounds/Menu/start");
            menuMusic = Content.Load<SoundEffect>("sounds/Menu/background");
            bgMusicInstance = bgMusic.CreateInstance();
            startMusicInstance = startMusic.CreateInstance();
            menuMusicInstance = menuMusic.CreateInstance();
            bgMusicInstance.Volume = 0.5f;
            menuMusicInstance.Volume = 0.5f;
            startMusicInstance.Volume = 0.5f;
            bgMusicInstance.IsLooped = true;
            menuMusicInstance.IsLooped = true;
            startMusicInstance.IsLooped = false;
            fx[0] = Content.Load<SoundEffect>("sounds/fx/supersmash");
            fx[1] = Content.Load<SoundEffect>("sounds/fx/schlag");
            fx[2] = Content.Load<SoundEffect>("sounds/fx/landing");
            fx[3] = Content.Load<SoundEffect>("sounds/fx/ashbrett_sword_hit");
            fx[4] = Content.Load<SoundEffect>("sounds/fx/ashbrett_sword_swoosh");
            fx[5] = Content.Load<SoundEffect>("sounds/fx/button_press");
            fx[6] = Content.Load<SoundEffect>("sounds/fx/punch_clubber_cloud");
            fx[7] = Content.Load<SoundEffect>("sounds/fx/ausrutscher"); //Bananenschale
            fx[8] = Content.Load<SoundEffect>("sounds/fx/time_shift");
            fx[9] = Content.Load<SoundEffect>("sounds/fx/triggerwall");
            ashbrett[0] = Content.Load<SoundEffect>("sounds/ashbrett/attack");
            ashbrett[1] = Content.Load<SoundEffect>("sounds/ashbrett/breathing");
            fxInstance[0] = ashbrett[1].CreateInstance();
            ashbrett[2] = Content.Load<SoundEffect>("sounds/ashbrett/dying");
            ashbrett[3] = Content.Load<SoundEffect>("sounds/ashbrett/hit");
            ashbrett[4] = Content.Load<SoundEffect>("sounds/ashbrett/jumping");
            ashbrett[5] = Content.Load<SoundEffect>("sounds/ashbrett/win");
            ashbrett[6] = Content.Load<SoundEffect>("sounds/ashbrett/super_attack");
            bonepuker[0] = Content.Load<SoundEffect>("sounds/bonepuker/attack");
            bonepuker[1] = Content.Load<SoundEffect>("sounds/bonepuker/dying");
            bonepuker[2] = Content.Load<SoundEffect>("sounds/bonepuker/jump");
            bonepuker[3] = Content.Load<SoundEffect>("sounds/bonepuker/smash");
            enemy[0] = Content.Load<SoundEffect>("sounds/fluffy/attack");
            enemy[1] = Content.Load<SoundEffect>("sounds/fluffy/dying");
            enemy[2] = Content.Load<SoundEffect>("sounds/skullmonkey/dying");
            enemy[3] = Content.Load<SoundEffect>("sounds/skullmonkey/freed");
            enemy[4] = Content.Load<SoundEffect>("sounds/skullmonkey/item");
            sweetcheeks[0] = Content.Load<SoundEffect>("sounds/sweetcheeks/attack");
            sweetcheeks[1] = Content.Load<SoundEffect>("sounds/sweetcheeks/dying");
            sweetcheeks[2] = Content.Load<SoundEffect>("sounds/sweetcheeks/enrage");
            fxInstance[1] = sweetcheeks[2].CreateInstance();
            sweetcheeks[3] = Content.Load<SoundEffect>("sounds/sweetcheeks/hit");
            sweetcheeks[4] = Content.Load<SoundEffect>("sounds/sweetcheeks/jump");
            sweetcheeks[5] = Content.Load<SoundEffect>("sounds/sweetcheeks/siren_scream");
        }
        static public void Stop(string sound)
        {
            if (Game1.sound)
            {
                switch (sound)
                {
                    case "sweetcheeks_enrage":
                        if(fxInstance[1].State == SoundState.Playing)
                            fxInstance[1].Stop(true);
                        break;
                    case "ashbrett_breath":
                        if (fxInstance[0].State == SoundState.Playing)
                            fxInstance[0].Stop(true);
                        break;
                }
            }
        }
        static public void Play(string sound)
        {
            if (Game1.sound)
            {
                switch (sound)
                {
                    // ---- Sweetcheeks ---- //
                    case "sweetcheeks_attack":
                        sweetcheeks[0].Play();
                        break;
                    case "sweetcheeks_dying":
                        sweetcheeks[1].Play();
                        break;
                    case "sweetcheeks_enrage":
                        fxInstance[1].Play();
                        break;
                    case "sweetcheeks_hit":
                        sweetcheeks[3].Play();
                        break;
                    case "sweetcheeks_jump":
                        sweetcheeks[4].Play();
                        break;
                    case "sweetcheeks_siren_scream":
                        sweetcheeks[5].Play();
                        break;

                    // ---- Fluffy ---- //
                    case "fluffy_attack":
                        enemy[0].Play();
                        break;
                    case "fluffy_dying":
                        enemy[1].Play();
                        break;
                    // ---- Skullmonkey ---- //
                    case "skullmonkey_dying":
                        enemy[2].Play();
                        break;
                    case "skullmonkey_freed":
                        enemy[3].Play();
                        break;
                    case "skullmonkey_item":
                        enemy[4].Play();
                        break;

                    // ---- Bonepuker ---- //
                    case "bonepuker_attack":
                        bonepuker[0].Play();
                        break;
                    case "bonepuker_dying":
                        bonepuker[1].Play();
                        break;
                    case "bonepuker_jump":
                        bonepuker[2].Play();
                        break;
                    case "bonepuker_smash":
                        bonepuker[3].Play();
                        break;

                    // ---- Ashbrett ---- //
                    case "ashbrett_attack":
                        ashbrett[0].Play();
                        break;
                    case "ashbrett_breath":
                        fxInstance[0].Play();
                        break;
                    case "ashbrett_dying":
                        ashbrett[2].Play();
                        break;
                    case "ashbrett_hit":
                        ashbrett[3].Play();
                        break;
                    case "ashbrett_jumping":
                        ashbrett[4].Play();
                        break;
                    case "ashbrett_win":
                        ashbrett[5].Play();
                        break;
                    case "ashbrett_superattack":
                        ashbrett[6].Play();
                        break;

                    // ---- FX ---- //
                    case "superSmash":
                        fx[0].Play();
                        break;
                    case "schlag":
                        fx[1].Play();
                        break;
                    case "land":
                        fx[2].Play();
                        break;
                    case "attack":
                        fx[3].Play();
                        break;
                    case "miss":
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
    }
}
