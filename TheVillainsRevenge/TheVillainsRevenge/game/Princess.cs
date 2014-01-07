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
        float rageMeter;
        int rageWarmup;
        int enrageSpeed;
        bool pressedLeft;
        bool sack;
        public bool coverEyes;
        public bool beating;
        Random randomGen = new Random();
        int randomNumber;
        int rageChance;
        int rageLimit;
        float unrageSpeed;

        public Princess()
        {

        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics)
        {
            rageWarmup = Convert.ToInt32((double)Game1.luaInstance["princessRageWarmup"]);
            rageChance = Convert.ToInt32((double)Game1.luaInstance["princessRageChance"]);
            enrageSpeed = Convert.ToInt32((double)Game1.luaInstance["princessEnrageSpeed"]);
            unrageSpeed = Convert.ToInt32((double)Game1.luaInstance["princessUnrageSpeed"]);
            rageLimit = Convert.ToInt32((double)Game1.luaInstance["princessRageLimit"]);
        }

        public void Save()
        {

        }

        public void Reset()
        {
        
        }

        public void Update(GameTime gameTime)
        {
            if (rageMode)
            {
                rageMeter += ((float)enrageSpeed / 60); //RageUp
                if (Game1.input.rechts && pressedLeft)
                {
                    rageMeter -= unrageSpeed; //RageDown
                    pressedLeft = false;
                }
                else if (Game1.input.links && !pressedLeft)
                {
                    rageMeter -= unrageSpeed; //RageDown
                    pressedLeft = true;
                }
                if (rageMeter > rageLimit) //Enrage?!?
                {
                    rageMeter = 0;
                    rageMode = false;
                    randomNumber = randomGen.Next(1, 2); //Augen zu halten vs hart aufs Maul!
                    if (randomNumber == 1)
                    {
                        //ENRAAAAAGGGGEEEE!!!!!!!!!!!!!!!!!
                    }
                    else
                    {
                        //Deine Mudda enraged!
                    }
                }

            }
            else if (gameTime.TotalGameTime.TotalMilliseconds > (rageTimer + (float)rageWarmup)) //RageWarmup
            {
                rageTimer = gameTime.TotalGameTime.TotalMilliseconds;
                randomNumber = randomGen.Next(0, 100);
                if (randomNumber <= rageChance)
                {
                    //Teste Enrage-Bedingungen
                    rageMode = true;
                }
            }
        }
    }
}
