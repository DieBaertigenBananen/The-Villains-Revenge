using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheVillainsRevenge
{
    class Player
    {
        //Deine Mutter ist so fett, selbst die Sonne wird von ihr angezogen
        public Vector2 position; //Position
        Vector2 lastpos; //Position vor vorherigem Update
        public Rectangle cbox; //Collisionsbox
        public int speed = 10; //Bewegungsgeschwindigkeit in m/s _/60
        public int airspeed = 8; //Geschwindigkeit bei Sprung & Fall in m/s _/60
        public bool jump = false;
        public bool fall = false;
        public double falltimer;
        public double jumptimer;
        public int jumppower = 20; //Anfangsgeschwindigkeit in m/s _/60
        public int gravitation = 60; //Erdbeschleunigung in (m/s)*(m/s) _/60

        //----------Spine----------
        public SkeletonRenderer skeletonRenderer;
        public Skeleton skeleton;
        public AnimationState animationState;
        public SkeletonBounds bounds = new SkeletonBounds();

        public Player(int x, int y) //Konstruktor, setzt Anfangsposition
        {
            position.X = x;
            position.Y = y;
            lastpos = position;
            cbox = new Rectangle((int)position.X, (int)position.Y, 128, 128);

        }

        public void Load(ContentManager Content, GraphicsDeviceManager graphics)//Wird im Hauptgame ausgeführt und geladen
        {
            //----------Spine----------
            skeletonRenderer = new SkeletonRenderer(graphics.GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            String name = "spineboy"; // "goblins";

            Atlas atlas = new Atlas("spine/" + name + ".atlas", new XnaTextureLoader(graphics.GraphicsDevice));
            SkeletonJson json = new SkeletonJson(atlas);
            //json.Scale = 1; //Für den Fall dass die aktuelle Textur in der Größe von der in Spine verwendeten Textur abweicht.
            skeleton = new Skeleton(json.ReadSkeletonData("spine/" + name + ".json"));
            if (name == "goblins") skeleton.SetSkin("goblingirl");
            skeleton.SetSlotsToSetupPose(); // Without this the skin attachments won't be attached. See SetSkin.

            // Define mixing between animations.
            AnimationStateData animationStateData = new AnimationStateData(skeleton.Data);
            animationStateData.SetMix("walk", "jump", 0.2f);
            animationStateData.SetMix("jump", "walk", 0.4f);

            animationState = new AnimationState(animationStateData);

            if (true)
            {
                // Event handling for all animations.
                animationState.Start += Start;
                animationState.End += End;
                animationState.Complete += Complete;
                animationState.Event += Event;

                animationState.SetAnimation(0, "drawOrder", true);
            }
            else
            {
                animationState.SetAnimation(0, "walk", false);
                TrackEntry entry = animationState.AddAnimation(0, "jump", false, 0);
                entry.End += new EventHandler<StartEndArgs>(End); // Event handling for queued animations.
                animationState.AddAnimation(0, "walk", true, 0);
            }

            skeleton.X = 320;
            skeleton.Y = 440;
            skeleton.UpdateWorldTransform();
        }

        public void Update(GameTime gameTime, Map map)
        {
            //Geschwindigkeit festlegen
            int actualspeed = speed; ;
            if (jump || fall)
            {
                actualspeed = airspeed;
            }

            //Lade Keyboard-Daten
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0f
                ||
                currentKeyboardState.IsKeyDown(Keys.Right) == true
                ||
                currentKeyboardState.IsKeyDown(Keys.D) == true
                ) //Wenn Rechte Pfeiltaste
            {
                Move(actualspeed, 0, map); //Bewege Rechts
            }
            if (
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0f
                ||
                currentKeyboardState.IsKeyDown(Keys.Left) == true
                ||
                currentKeyboardState.IsKeyDown(Keys.A) == true
                ) //Wenn Rechte Pfeiltaste
            {
                Move(-actualspeed, 0, map);//Bewege Links
            }
            if (
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0f
                ||
                currentKeyboardState.IsKeyDown(Keys.Space) == true
                )
            {
                if (!jump && !fall)
                {
                    Jump(gameTime, map); //Springen!
                }
            }

            //Speed verändern
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) == true || GamePad.GetState(PlayerIndex.One).Triggers.Right == 1.0f) //Wenn Rechte Pfeiltaste
            {
                speed++;
            }
            if (currentKeyboardState.IsKeyDown(Keys.LeftControl) == true || GamePad.GetState(PlayerIndex.One).Triggers.Left == 1.0f)//Wenn Linke Pfeiltaste
            {
                speed--;
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
            skeleton.X = position.X;
            skeleton.Y = position.Y;
            //skeleton.UpdateWorldTransform();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Wird im Hauptgame ausgeführt und malt den Spieler mit der entsprechenden Animation
            //----------Spine----------
            animationState.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
            animationState.Apply(skeleton);
            skeleton.UpdateWorldTransform();
            skeletonRenderer.Begin();
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();
            bounds.Update(skeleton, true);
        }

        public void Jump(GameTime gameTime, Map map) //Deine Mudda springt bei Doodle Jump nach unten.
        {
            if (CollisionCheckedVector(0, -1, map.blocks).Y < 0)
            {
                if (!jump)
                {
                    animationState.SetAnimation(0, "jump", true);
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
            position.X += domove.X;
            position.Y += domove.Y;
            cbox.X = (int)position.X;
            cbox.Y = (int)position.Y;
        }

        Vector2 CollisionCheckedVector(int x, int y, List<Block> list)
        {
            Rectangle cboxnew = this.cbox;
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
                cboxnew.X = this.cbox.X + ((x / icoll) * i);
                cboxnew.Y = this.cbox.Y + ((y / icoll) * i);
                //Gehe die Blöcke der Liste durch
                foreach (Block block in list)
                {
                    //Wenn Kollision vorliegt: Keinen weiteren Block abfragen
                    if (cboxnew.Intersects(block.cbox))
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
                    move.X = cboxnew.X - cbox.X;
                    move.Y = cboxnew.Y - cbox.Y;
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
