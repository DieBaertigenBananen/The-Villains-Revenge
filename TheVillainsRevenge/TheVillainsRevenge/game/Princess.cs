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
        public float rageMeter;
        int rageWarmup;
        int enrageSpeed;
        bool pressedLeft;
        bool sack;
        public bool coverEyes;
        double coverTimer;
        int coverTime;
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
            rageWarmup = Convert.ToInt32((double)Game1.luaInstance["princessRageWarmup"]) * 1000;
            rageChance = Convert.ToInt32((double)Game1.luaInstance["princessRageChance"]);
            enrageSpeed = Convert.ToInt32((double)Game1.luaInstance["princessEnrageSpeed"]);
            unrageSpeed = Convert.ToInt32((double)Game1.luaInstance["princessUnrageSpeed"]);
            rageLimit = Convert.ToInt32((double)Game1.luaInstance["princessRageLimit"]);
            coverTime = Convert.ToInt32((double)Game1.luaInstance["princessCoverTime"]) * 1000;
        }

        public void Save()
        {

        }

        public void Reset()
        {
        
        }

        public void Update(GameTime gameTime)
        {
            if (beating)
            {
                //Wenn Kloppwolke zu Ende
                beating = false;
                rageMode = false;
                rageMeter = 0;
                rageTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (coverEyes)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > (coverTimer + (float)coverTime)) //CoverEyes beenden?
                {
                    coverEyes = false;
                    rageMode = false;
                    rageMeter = 0;
                    rageTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (rageMode)
            {
                rageMeter += 100 / ((float)enrageSpeed * 60); //RageUp
                if (Game1.input.rechts)
                {
                    if (pressedLeft)
                    {
                        rageMeter -= unrageSpeed; //RageDown
                        pressedLeft = false;
                    }
                }
                else if (Game1.input.links)
                {
                    if (!pressedLeft)
                    {
                    rageMeter -= unrageSpeed; //RageDown
                    pressedLeft = true;
                    }
                }
                if (rageMeter > rageLimit) //Enrage?!?
                {
                    randomNumber = randomGen.Next(0, 100); //Augen zu halten vs hart aufs Maul!
                    if (randomNumber < 50)
                    {
                        beating = true;
                        //ENRAAAAAGGGGEEEE!!!!!!!!!!!!!!!!!
                    }
                    else
                    {
                        coverEyes = true;
                        coverTimer = gameTime.TotalGameTime.TotalMilliseconds;
                        //Deine Mudda enraged!
                    }
                }
                if (rageMeter <= 0)
                {
                    rageMode = false;
                    rageMeter = 0;
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
            beating = true;
            coverEyes = true;
        }
    }
}
