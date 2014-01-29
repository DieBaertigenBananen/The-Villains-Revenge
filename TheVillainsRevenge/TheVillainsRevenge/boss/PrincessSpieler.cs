using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    class PrincessSpieler : Player
    {

        public PrincessSpieler(int x, int y) : base(x,y) //Konstruktor, setzt Anfangsposition
        {
            checkpoint = new Vector2(x, y);
            position.X = x;
            position.Y = y;
            lastPosition = position;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["playerCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["playerCollisionHeight"]));
            lifes = Game1.leben;
            spine = new Spine();
            initAcceleration = (float)Convert.ToInt32((double)Game1.luaInstance["playerAcceleration"]) / 100;
            smashInitIntensity = Convert.ToInt32((double)Game1.luaInstance["playerSmashIntensity"]);
            smashCooldown = (float)Convert.ToDouble(Game1.luaInstance["playerMegaSchlagCooldown"]) * 1000;
        }
        public void Update(GameTime gameTime, Map map)
        {
            speed = Convert.ToInt32((double)Game1.luaInstance["playerSpeed"]);
            airspeed = Convert.ToInt32((double)Game1.luaInstance["playerAirspeed"]);
            jumppower = Convert.ToInt32((double)Game1.luaInstance["playerJumppower"]);
            gravitation = Convert.ToInt32((double)Game1.luaInstance["playerGravitation"]);

            //Geschwindigkeit festlegen
            int actualspeed = speed;
            if (jump || fall)
            {
                actualspeed = airspeed;
            }
            //Einfluss Gamepad
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0)
            {
                actualspeed = (int)((float)actualspeed * Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X));
            }
            //Deine Mudda stinkt nach Backfisch
            if (CollisionCheckedVector(0, 1, map.blocks).Y == 0) //AllowSmash am Boden zurück setzen
            {
                allowSmash = false;
            }
            //-----Sprung-----
            if ((Game1.input.sprung || savejump))
            {
                if (!jump && !fall && Game1.input.sprungp)
                {
                    spine.Clear(0);
                    spine.anim("jump", 0, false, gameTime);
                    Jump(gameTime, map); //Springen!
                    savejump = false;
                }
                else if (fall)
                {
                    savejump = true;
                }
                else
                {
                    savejump = false;
                }
            }
            else
            {
                if (jump && !Game1.input.sprungp)
                {
                    jumptimer -= GameScreen.slow + Convert.ToInt32((double)Game1.luaInstance["playerJumpCutoff"]);
                }
            }
            //-----Schlag / Smash starten-----
            if (Game1.input.hit)
            {
                if (jump || fall)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > (smashTimer + smashCooldown) && allowSmash) //Smash beginnen
                    {
                        jump = false;
                        fall = true;
                        hit = false;
                        smash = true;
                        spine.anim("smash", 0, false, gameTime);
                        smashTimer = gameTime.TotalGameTime.TotalMilliseconds;
                        smashIntensity = smashInitIntensity;
                        falltimer = gameTime.TotalGameTime.TotalMilliseconds - Convert.ToInt32((double)Game1.luaInstance["playerMegaSchlagFall"]);
                    }
                }
                if (!smash && !hit) //Schlag beginnen
                {
                    if (jump || fall)
                    {
                        allowSmash = true; //Nächster Schlag in der Luft = smash
                    }
                    hit = true;
                    spine.anim("attack", 0, false, gameTime);
                    hitTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            //Schlag ggf beenden
            if (hit && gameTime.TotalGameTime.TotalMilliseconds > hitTimer + (spine.skeleton.Data.FindAnimation("attack").Duration * 1000))
            {
                hit = false;
                spine.animationState.ClearTrack(1);
            }
            //Smash fortführen
            if (smash)
            {
                smashIntensity--;
                //Smash ggf beenden
                if (smashIntensity <= 0)
                {
                    smash = false;
                    smashImpact = false;
                }
                else if (CollisionCheckedVector(0, 1, map.blocks).Y == 0)
                {
                    smashImpact = true;
                }
            }
            if (!smash)
            {
                //-----Move-----
                if (Game1.input.rechts) //Wenn Rechte Pfeiltaste
                {
                    richtung = true;
                    acceleration += 1 / (60 * initAcceleration);
                    if (Math.Abs(acceleration) <= 2 / (60 * initAcceleration) || Math.Abs(acceleration) > initAcceleration) //Drehen bzw weiter laufen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("run", 2, true, gameTime);
                        }
                        else
                        {
                            spine.anim("jump", 2, false, gameTime);
                        }
                    }
                    else if (spine.flipSkel && Math.Abs(acceleration) <= 2 / (60 * initAcceleration)) //Bei direktem Richtungstastenwechsel trotzdem beim Abbremsen in idle übergehen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("idle", 0, true, gameTime); //In idle-Position übergehen
                        }
                    }
                    if (jump || fall) //Zusätzliche Beschleunigung in der Luft
                    {
                        acceleration += 2 / (60 * initAcceleration);
                    }
                }
                else if (Game1.input.links) //Wenn Rechte Pfeiltaste
                {
                    richtung = false;
                    acceleration -= 1 / (60 * initAcceleration);
                    if (Math.Abs(acceleration) <= 2 / (60 * initAcceleration) || Math.Abs(acceleration) > initAcceleration) //Drehen bzw weiter laufen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("run", 1, true, gameTime);
                        }
                        else
                        {
                            spine.anim("jump", 1, false, gameTime);
                        }
                    }
                    else if (!spine.flipSkel && Math.Abs(acceleration) <= 2 / (60 * initAcceleration)) //Bei direktem Richtungstastenwechsel trotzdem in idle übergehen
                    {
                        if (!jump && !fall)
                        {
                            spine.anim("idle", 0, true, gameTime); //In idle-Position übergehen
                        }
                    }
                    if (jump || fall) //Zusätzliche Beschleunigung in der Luft
                    {
                        acceleration -= 2 / (60 * initAcceleration);
                    }
                }
                else
                {
                    //Auslaufen_Abbremsen
                    if (acceleration < 0)
                    {
                        acceleration += 1 / (60 * initAcceleration);
                        if (acceleration > 0) //Nicht umdrehen
                        {
                            acceleration = 0;
                        }
                    }
                    else if (acceleration > 0)
                    {
                        acceleration -= 1 / (60 * initAcceleration);
                        if (acceleration < 0) //Nicht umdrehen
                        {
                            acceleration = 0;
                        }
                    }
                    if (!jump && !fall)
                    {
                        spine.anim("idle", 0, true, gameTime); //In idle-Position übergehen
                    }
                }
                //Keine Beschleunigungs"vermehrung", durch Beschleunigung wird nur MaxSpeed bei jedem Update absolut vermindert. Fake aber funzt...
                if (acceleration < -initAcceleration)
                {
                    acceleration = -initAcceleration;
                }
                if (acceleration > initAcceleration)
                {
                    acceleration = initAcceleration;
                }
                if (Math.Abs(CollisionCheckedVector((int)((acceleration / initAcceleration) * actualspeed), 0, map.blocks).X) < Math.Abs((int)((acceleration / initAcceleration) * actualspeed)))
                {
                    acceleration = -acceleration * 0.8f;
                }
                Move((int)((acceleration / initAcceleration) * actualspeed), 0, map);
            }

            //Gravitation
            if (CollisionCheckedVector(0, 1, map.blocks).Y > 0 && !jump)
            {
                if (!fall)
                {
                    fall = true;
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - falltimer) / 1000);
                Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
                spine.anim("jump", 0, false, gameTime);
            }
            else
            {
                if (fall)
                {
                    fall = false;
                }
            }

            //Sprung fortführen
            if (jump)
            {
                Jump(gameTime, map);
            }
            position.Y = spine.skeleton.Y;
            position.X = spine.skeleton.X;
        }
    }
}
