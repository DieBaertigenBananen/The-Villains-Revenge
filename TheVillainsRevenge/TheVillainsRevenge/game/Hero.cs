using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TheVillainsRevenge
{
    class Hero
    {
        public Vector2 position; //Position
        public CollisionBox cbox;
        public int speed; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int gravitation; //Erdbeschleunigung in (m/s)*(m/s) _/60
        public int jumppower; //Anfangsgeschwindigkeit in m/s _/60
        public double herotime;
        bool start = false;
        int heroStartTime;
        public float test;

        //----------Spine----------
        string animation = "";
        public SkeletonRenderer skeletonRenderer;
        public Skeleton skeleton;
        public AnimationState animationState;
        public SkeletonBounds bounds = new SkeletonBounds();

        public Hero(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            position.X = x;
            position.Y = y;
            cbox = new CollisionBox(Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetX"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionOffsetY"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionWidth"]), Convert.ToInt32((double)Game1.luaInstance["heroCollisionHeight"]));
            heroStartTime = Convert.ToInt32((double)Game1.luaInstance["heroStartTime"]);
        }
        public void Load(ContentManager Content, GraphicsDeviceManager graphics)//Wird im Hauptgame ausgeführt und geladen
        {
            //----------Spine----------
            skeletonRenderer = new SkeletonRenderer(graphics.GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            String name = "ashbrett";

            Atlas atlas = new Atlas("spine/sprites/" + name + ".atlas", new XnaTextureLoader(graphics.GraphicsDevice));
            SkeletonJson json = new SkeletonJson(atlas);
            json.Scale = (float)Convert.ToInt32((double)Game1.luaInstance["heroScale"]) / 20; //Für den Fall dass die aktuelle Textur in der Größe von der in Spine verwendeten Textur abweicht.
            skeleton = new Skeleton(json.ReadSkeletonData("spine/sprites/" + name + ".json"));
            skeleton.SetSlotsToSetupPose(); // Without this the skin attachments won't be attached. See SetSkin.

            // Define mixing between animations.
            AnimationStateData animationStateData = new AnimationStateData(skeleton.Data);
            //animationStateData.SetMix("walk", "jump", 0.2f);
            //animationStateData.SetMix("jump", "walk", 0.4f);
            animationState = new AnimationState(animationStateData);

            // Event handling for all animations.
            animationState.Start += Start;
            animationState.End += End;
            animationState.Complete += Complete;
            animationState.Event += Event;

            skeleton.x = position.X;
            skeleton.y = position.Y;
        }
        public void Draw(GameTime gameTime, Camera camera)
        {
            //Wird im Hauptgame ausgeführt und malt den Spieler mit der entsprechenden Animation
            if (start)
            {
                //Player -> Drawposition
                skeleton.X = position.X - camera.viewport.X;
                skeleton.Y = position.Y - camera.viewport.Y;
                //----------Spine----------
                animationState.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
                animationState.Apply(skeleton);
                skeleton.UpdateWorldTransform();
                skeletonRenderer.Begin();
                skeletonRenderer.Draw(skeleton);
                skeletonRenderer.End();
                //Player -> Worldposition
                skeleton.X = position.X;
                skeleton.Y = position.Y;
            }
        }
        public void Update(GameTime gameTime, Map map,Vector2 sposition)
        {
            if (start)
            {
                speed = Convert.ToInt32((double)Game1.luaInstance["heroSpeed"]);
                if (GameScreen.slow != 0)
                {
                    speed = speed / Convert.ToInt32((double)Game1.luaInstance["itemSlowReduce"]);
                }
                airspeed = Convert.ToInt32((double)Game1.luaInstance["heroAirspeed"]);
                jumppower = Convert.ToInt32((double)Game1.luaInstance["heroJumppower"]);
                gravitation = Convert.ToInt32((double)Game1.luaInstance["heroGravitation"]);
                //Geschwindigkeit festlegen
                int actualspeed = speed;
                if (jump || fall)
                {
                    actualspeed = airspeed;
                }
                sposition.X = map.size.X;
                //Wenn Spieler ist hinten bewege zurück
                if (sposition.X < position.X)
                {
                    actualspeed = -actualspeed;
                }
                //Schaue ob an nächster Position ein Block ist
                if (CollisionCheckedVector(actualspeed, 0, map.blocks).X != 0)
                {
                    //kein Block, also bewege
                    Move(actualspeed, 0, map);
                    //Schau ob unten ein Block ist
                    if (CollisionCheckedVector(0, 1, map.blocks).Y > 0)
                    {
                        //Schau ob er tief fällt, wenn ja, beweg mal zurück
                        if (fall || jump)
                        {
                            //Schaue ob an nächster Position ein Block wär wo er drauf könnte
                            if (CollisionCheckedVector(actualspeed, 0, map.blocks).X != 0)
                            {
                                Move(actualspeed, 0, map);
                                //Schau ob er direkt auf nen Block steht, wenn ja bewege
                                if (CollisionCheckedVector(0, gravitation, map.blocks).Y != gravitation) 
                                {
                                    Move(-actualspeed, 0, map);
                                }
                                else
                                {
                                    Move(-actualspeed, 0, map);
                                    //Schaue ob er wenn er fällt auf einen block stehen würd, wenn ja, bewege garnicht
                                    test = CollisionCheckedVector(0, gravitation, map.blocks).Y;
                                    if (CollisionCheckedVector(0, gravitation, map.blocks).Y != gravitation)
                                    {
                                        Move(-actualspeed, 0, map);
                                    }
                                }
                            }
                        }
                        //Kein Block unten, kann also fallen, daher spring mal lieber
                        if (!jump && !fall)
                        {
                            Jump(gameTime, map); //Springen!
                        }
                    }
                    //Schau ob oben ein Block ist
                    if (CollisionCheckedVector(actualspeed, 0, map.blocks).X != 0)
                    {
                        Move(actualspeed, 0, map);
                        if (CollisionCheckedVector(0, -jumppower, map.blocks).Y != -jumppower)
                        {
                            Move(-actualspeed, 0, map);
                            //Kein Block unten, kann also fallen, daher spring mal lieber
                            if (!jump && !fall)
                            {
                                Jump(gameTime, map); //Springen!
                            }
                        }
                        else
                        {
                            Move(-actualspeed, 0, map);

                        }
                    }

                }
                else
                {
                    //Doch ein Block, also spring
                    if (!jump && !fall)
                    {
                        Jump(gameTime, map); //Springen!
                    }
                }
                if (CollisionCheckedVector(0, 1, map.blocks).Y > 0 && !jump)
                {
                    if (!fall)
                    {
                        fall = true;
                        falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - falltimer) / 1000);
                    Move(0, (int)((gravitation * t)), map); //v(t)=-g*t
                }
                else
                {
                    fall = false;
                }

                //Sprung fortführen
                if (jump)
                {
                    Jump(gameTime, map);
                }
            }
            else
            {
                herotime += gameTime.ElapsedGameTime.TotalSeconds;
                if (herotime > heroStartTime)
                    start = true;
            }
        }

        public void Jump(GameTime gameTime, Map map) //Deine Mudda springt bei Doodle Jump nach unten.
        {
            if (CollisionCheckedVector(0, -1, map.blocks).Y < 0)
            {
                if (!jump)
                {
                    jump = true;
                    jumptimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                float t = (float)((gameTime.TotalGameTime.TotalMilliseconds - jumptimer) / 1000);
                int deltay = (int)(-jumppower + (gravitation * t));
                if (deltay > 0)
                {
                    jump = false;
                    fall = true;
                    falltimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else
                {
                    Move(0, deltay, map); //v(t)=-g*t
                }
            }
            else
            {
                jump = false;
            }
        }

        public void Move(int deltax, int deltay, Map map) //Falls Input, bewegt den Spieler
        {
            Vector2 domove = new Vector2(0, 0);
            domove = CollisionCheckedVector(deltax, deltay, map.blocks);
            skeleton.X += domove.X;
            skeleton.Y += domove.Y;
            position.Y = skeleton.Y;
            position.X = skeleton.X;
            cbox.Update(position);
        }


        Vector2 CollisionCheckedVector(int x, int y, List<Block> list)
        {
            CollisionBox cboxnew = new CollisionBox((int)cbox.offset.X, (int)cbox.offset.Y, cbox.box.Width, cbox.box.Height);
            cboxnew.Update(cbox.position);
            Vector2 move = new Vector2(0, 0);
            int icoll;
            bool stop;
            //Größere Koordinate als Iteration nehmen
            if (Math.Abs(x) > Math.Abs(y))
            {
                icoll = Math.Abs(x);
            }
            else
            {
                icoll = Math.Abs(y);
            }
            //Iteration
            for (int i = 1; i <= icoll; i++)
            {
                stop = false;
                //Box für nächsten Iterationsschritt berechnen
                cboxnew.box.X = this.cbox.box.X + ((x / icoll) * i);
                cboxnew.box.Y = this.cbox.box.Y + ((y / icoll) * i);
                //Gehe die Blöcke der Liste durch
                foreach (Block block in list)
                {
                    //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                    if (cboxnew.box.Intersects(block.cbox))
                    {
                        stop = true;
                        break;
                    }
                }
                if (stop == true) //Bei Kollision: Kollisionsabfrage mit letztem kollisionsfreien Zustand beenden
                {
                    break;
                }
                else //Kollisionsfreien Fortschritt speichern
                {
                    move.X = cboxnew.box.X - cbox.box.X;
                    move.Y = cboxnew.box.Y - cbox.box.Y;
                }
            }
            return move;
        }

        //----------Spine----------
        public void Start(object sender, StartEndArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + animationState.GetCurrent(e.TrackIndex) + ": start");
        }

        public void End(object sender, StartEndArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + animationState.GetCurrent(e.TrackIndex) + ": end");
        }

        public void Complete(object sender, CompleteArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + animationState.GetCurrent(e.TrackIndex) + ": complete " + e.LoopCount);
        }

        public void Event(object sender, EventTriggeredArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + animationState.GetCurrent(e.TrackIndex) + ": event " + e.Event);
        }
    }
}
