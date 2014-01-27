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
        public bool rageMode;
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
        double beatingTimer;
        Random randomGen = new Random();
        int randomNumber;
        int rageChance;
        public int rageLimit;
        float unrageSpeed;
        Spine spine;
        //Deine Mutter ist so fett, wenn sie aus dem Bett fällt, dann auf beiden Seiten

        public Princess()
        {
            spine = new Spine();
        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics)
        {
            spine.Load(Vector2.Zero, "clobbercloud", (float)Convert.ToDouble(Game1.luaInstance["playerScale"]), 0);
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

        public void StartSave()
        {

        }

        public void StartReset()
        {
            rageMode = false;
            rageMeter = 0;
            beating = false;
            coverEyes = false;
        }

        public void Reset()
        {
            rageMode = false;
            rageMeter = 0;
            beating = false;
            coverEyes = false;
        }

        public void Update(GameTime gameTime, Player player, Map map)
        {
            spine.skeleton.X = player.position.X;
            spine.skeleton.Y = player.position.Y;
            if (beating)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > beatingTimer + (spine.skeleton.Data.FindAnimation("cloud").Duration * 1000)) //Kloppwolke zu Ende?
                {
                    beating = false;
                    rageMode = false;
                    rageMeter = 0;
                    rageTimer = gameTime.TotalGameTime.TotalMilliseconds;
                    //spine.animationState.ClearTrack(0);
                }
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
                if (rageMeter > rageLimit && !coverEyes && !beating) //Enrage?!?
                {
                    randomNumber = randomGen.Next(0, 100); //Augen zu halten vs hart aufs Maul!
                    if (randomNumber <= 50)
                    {
                        beating = true;
                        beatingTimer = gameTime.TotalGameTime.TotalMilliseconds;
                        int tempFlipState;
                        if (spine.skeleton.FlipX)
                        {
                            tempFlipState = 1;
                        }
                        else
                        {
                            tempFlipState = 2;
                        }
                        spine.anim("cloud", tempFlipState, false, gameTime);
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
                if (player.CollisionCheckedVector(0, 1, map.blocks).X == 0) //Nur auf dem Boden enragen
                {
                    rageTimer = gameTime.TotalGameTime.TotalMilliseconds;
                    randomNumber = randomGen.Next(0, 100);
                    if (randomNumber <= rageChance)
                    {
                        //Teste Enrage-Bedingungen
                        rageMode = true;
                    }
                }
                else
                {
                    rageTimer = gameTime.TotalGameTime.TotalMilliseconds - ((float)rageWarmup * 0.2f); //Wenn Spieler wieder auf den Boden kommt nach kurzer Zeit enragen
                }
            }
            beating = true;
        }

        public void Draw(GameTime gameTime, Camera camera)
        {
            spine.Draw(gameTime, camera, new Vector2(spine.skeleton.x, spine.skeleton.y));
        }
    }
}
