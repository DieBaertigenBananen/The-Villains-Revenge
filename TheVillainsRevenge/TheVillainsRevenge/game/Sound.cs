using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework.Audio; 

namespace TheVillainsRevenge
{
    class Sound
    {
        SoundEffect soundEffect;
        SoundEffectInstance soundEffectInstance;
        string name;
        public Sound(string name)
        {
            this.name = name;
        }

        public void Load(ContentManager Content)
        {
            soundEffect = Content.Load<SoundEffect>(name);
            soundEffectInstance = soundEffect.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();
        }
    }
}
