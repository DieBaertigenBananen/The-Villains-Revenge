using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class Princess
    {
        bool rageMode;
        double rageTimer;
        int rageMeter;
        bool sack;
        public bool coverEyes;
        public bool beating;
        Random randomGen = new Random();
        int randomNumber;
        public Princess()
        {

        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics)
        {

        }

        public void Save()
        {

        }

        public void Update()
        {
            randomNumber = randomGen.Next(0, 100);
        }
    }
}
