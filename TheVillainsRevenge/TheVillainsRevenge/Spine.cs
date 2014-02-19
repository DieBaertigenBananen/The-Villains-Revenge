using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class Spine
    {
        //----------Spine----------
        public string animation = "";
        public SkeletonRenderer skeletonRenderer;
        public Skeleton skeleton;
        public AnimationState animationState;
        public SkeletonBounds bounds = new SkeletonBounds();
        public bool flipSkel;
        public double animationTimer;
        string name = "";
        //------------ Save ---------
        float[] pausedTime = new float[3];

        public void anim(string newanim,int flip,bool loop)
        {
            if (flip == 1)
            {
                skeleton.flipX = true;
                flipSkel = true;
            }
            if (flip == 2)
            {
                skeleton.flipX = false;
                flipSkel = false;
            }
            if ((newanim == "die" || newanim == "die2") && (name == "bonepuker" || name == "sweetcheeks"))
            {
                animationState.ClearTracks();
            }
            if ((newanim == "attack" && (name == "bonepuker" || name == "sweetcheeks" || name == "ashbrett")) || newanim == "smash" || newanim == "cloud")
            {
                animationState.SetAnimation(1, newanim, loop);
            }
            else if (newanim == "sc_escape" || newanim == "sc_cover_eyes" || newanim == "sc_bag")
            {
                animationState.SetAnimation(2, newanim, loop);
            }
            else if (newanim != "")
            {
                if (animation != newanim || newanim == "die")
                {
                    animationTimer = Game1.time.TotalMilliseconds;
                    animationState.SetAnimation(0, newanim, loop);
                    animation = newanim;
                }
            }
        }
        public void Save()
        {
            for (int i = 0; i < 3; i++)
            {
                if (animationState.GetCurrent(i) != null)
                    pausedTime[i] = animationState.GetCurrent(i).time;
            }
        }
        public void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                if (animationState.GetCurrent(i) != null)
                    animationState.GetCurrent(i).time = pausedTime[i];
            }
        }

        public void Clear(int track)
        {
            animationState.ClearTrack(track);
            if (track == 0)
            {
                animation = "";
            }
        }

        public float CurrentAnimTime()
        {
            float time = (float)(Game1.time.TotalMilliseconds - animationTimer) / 1000;
            return time;
        }

        public void Draw(GameTime gameTime, Camera camera,Vector2 position)
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

        public void Load(Vector2 position,string name,float scale, float acceleration)
        {
            this.name = name;
            //----------Spine----------
            skeletonRenderer = new SkeletonRenderer(Game1.graphics.GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            Atlas atlas = new Atlas("spine/sprites/" + name + ".atlas", new XnaTextureLoader(Game1.graphics.GraphicsDevice));
            SkeletonJson json = new SkeletonJson(atlas);
            json.Scale = scale;
            skeleton = new Skeleton(json.ReadSkeletonData("spine/sprites/" + name + ".json"));
            skeleton.SetSlotsToSetupPose(); // Without this the skin attachments won't be attached. See SetSkin.

            // Define mixing between animations.
            AnimationStateData animationStateData = new AnimationStateData(skeleton.Data);
            float fading_standard = 0.2f;
            switch (name)
            {
                case "bonepuker":
                    animationStateData.SetMix("idle", "run", acceleration);
                    animationStateData.SetMix("run", "idle", acceleration);
                    animationStateData.SetMix("idle", "jump", 0.3f);
                    animationStateData.SetMix("run", "jump", 0.3f);
                    animationStateData.SetMix("jump", "run", 0.1f);
                    animationStateData.SetMix("jump", "idle", 0.3f);
                    animationStateData.SetMix("jump", "die", 0.3f);
                    animationStateData.SetMix("run", "die", 0.1f);
                    animationStateData.SetMix("idle", "die", 0.2f);
                    animationStateData.SetMix("smash", "idle", 0.2f);
                    animationStateData.SetMix("smash", "run", 0.2f);
                    animationStateData.SetMix("smash", "jump", 0.2f);
                    //Fading für Attack dürfte so nicht funktionieren, da Attack auf einem anderen Track ausgeführt wird
                    animationStateData.SetMix("attack", "idle", 0.1f);
                    animationStateData.SetMix("attack", "run", 0.1f);
                    animationStateData.SetMix("attack", "jump", 0.1f);
                    break;
                case "ashbrett":
                    fading_standard = 0.2f;
                    animationStateData.SetMix("idle", "walk", fading_standard);
                    animationStateData.SetMix("idle", "jump", fading_standard);
                    animationStateData.SetMix("idle", "attack", fading_standard);
                    animationStateData.SetMix("idle", "close", fading_standard);
                    animationStateData.SetMix("idle", "defend", fading_standard);
                    animationStateData.SetMix("idle", "die", fading_standard);
                    animationStateData.SetMix("idle", "hit", fading_standard);
                    animationStateData.SetMix("idle", "super_attack", fading_standard);

                    animationStateData.SetMix("walk", "idle", fading_standard);
                    animationStateData.SetMix("walk", "jump", fading_standard);
                    animationStateData.SetMix("walk", "attack", fading_standard);
                    animationStateData.SetMix("walk", "close", fading_standard);
                    animationStateData.SetMix("walk", "defend", fading_standard);
                    animationStateData.SetMix("walk", "die", fading_standard);
                    animationStateData.SetMix("walk", "hit", fading_standard);
                    animationStateData.SetMix("walk", "super_attack", fading_standard);

                    animationStateData.SetMix("jump", "idle", fading_standard);
                    animationStateData.SetMix("jump", "walk", fading_standard);
                    animationStateData.SetMix("jump", "attack", fading_standard);
                    animationStateData.SetMix("jump", "close", fading_standard);
                    animationStateData.SetMix("jump", "defend", fading_standard);
                    animationStateData.SetMix("jump", "die", fading_standard);
                    animationStateData.SetMix("jump", "hit", fading_standard);
                    animationStateData.SetMix("jump", "super_attack", fading_standard);

                    animationStateData.SetMix("attack", "idle", fading_standard);
                    animationStateData.SetMix("attack", "walk", fading_standard);
                    animationStateData.SetMix("attack", "jump", fading_standard);
                    animationStateData.SetMix("attack", "close", fading_standard);
                    animationStateData.SetMix("attack", "defend", fading_standard);
                    animationStateData.SetMix("attack", "die", fading_standard);
                    animationStateData.SetMix("attack", "hit", fading_standard);
                    animationStateData.SetMix("attack", "super_attack", fading_standard);

                    animationStateData.SetMix("close", "idle", fading_standard);
                    animationStateData.SetMix("close", "walk", fading_standard);
                    animationStateData.SetMix("close", "jump", fading_standard);
                    animationStateData.SetMix("close", "attack", fading_standard);
                    animationStateData.SetMix("close", "defend", fading_standard);
                    animationStateData.SetMix("close", "die", fading_standard);
                    animationStateData.SetMix("close", "hit", fading_standard);
                    animationStateData.SetMix("close", "super_attack", fading_standard);

                    animationStateData.SetMix("defend", "idle", fading_standard);
                    animationStateData.SetMix("defend", "walk", fading_standard);
                    animationStateData.SetMix("defend", "jump", fading_standard);
                    animationStateData.SetMix("defend", "attack", fading_standard);
                    animationStateData.SetMix("defend", "close", fading_standard);
                    animationStateData.SetMix("defend", "die", fading_standard);
                    animationStateData.SetMix("defend", "hit", fading_standard);
                    animationStateData.SetMix("defend", "super_attack", fading_standard);

                    animationStateData.SetMix("hit", "idle", fading_standard);
                    animationStateData.SetMix("hit", "walk", fading_standard);
                    animationStateData.SetMix("hit", "jump", fading_standard);
                    animationStateData.SetMix("hit", "attack", fading_standard);
                    animationStateData.SetMix("hit", "close", fading_standard);
                    animationStateData.SetMix("hit", "die", fading_standard);
                    animationStateData.SetMix("hit", "defend", fading_standard);
                    animationStateData.SetMix("hit", "super_attack", fading_standard);

                    animationStateData.SetMix("super_attack", "idle", fading_standard);
                    animationStateData.SetMix("super_attack", "walk", fading_standard);
                    animationStateData.SetMix("super_attack", "jump", fading_standard);
                    animationStateData.SetMix("super_attack", "attack", fading_standard);
                    animationStateData.SetMix("super_attack", "close", fading_standard);
                    animationStateData.SetMix("super_attack", "die", fading_standard);
                    animationStateData.SetMix("super_attack", "defend", fading_standard);
                    animationStateData.SetMix("super_attack", "hit", fading_standard);
                    break;
                case "fluffy":
                    fading_standard = 0.2f;
                    animationStateData.SetMix("attack", "die", fading_standard);
                    animationStateData.SetMix("attack", "smash_die", fading_standard);
                    animationStateData.SetMix("attack", "idle", fading_standard);
                    animationStateData.SetMix("attack", "walk", fading_standard);

                    animationStateData.SetMix("idle", "die", fading_standard);
                    animationStateData.SetMix("idle", "smash_die", fading_standard);
                    animationStateData.SetMix("idle", "attack", fading_standard);
                    animationStateData.SetMix("idle", "walk", fading_standard);

                    animationStateData.SetMix("walk", "die", fading_standard);
                    animationStateData.SetMix("walk", "smash_die", fading_standard);
                    animationStateData.SetMix("walk", "attack", fading_standard);
                    animationStateData.SetMix("walk", "idle", fading_standard);
                    break;
                case "skullmonkey":
                    fading_standard = 0.2f;
                    animationStateData.SetMix("attack", "dying", fading_standard);
                    animationStateData.SetMix("attack", "sitting", fading_standard);
                    animationStateData.SetMix("attack", "walking", fading_standard);

                    animationStateData.SetMix("sitting", "dying", fading_standard);
                    animationStateData.SetMix("sitting", "attack", fading_standard);
                    animationStateData.SetMix("sitting", "walking", fading_standard);

                    animationStateData.SetMix("walking", "dying", fading_standard);
                    animationStateData.SetMix("walking", "attack", fading_standard);
                    animationStateData.SetMix("walking", "sitting", fading_standard);
                    break;
                case "sweetcheeks":
                    fading_standard = 0.2f;
                    animationStateData.SetMix("attack", "die", fading_standard);
                    animationStateData.SetMix("attack", "hit", fading_standard);
                    animationStateData.SetMix("attack", "idle", fading_standard);
                    animationStateData.SetMix("attack", "jump", fading_standard);
                    animationStateData.SetMix("attack", "run", fading_standard);
                    animationStateData.SetMix("attack", "scream", fading_standard);

                    animationStateData.SetMix("hit", "die", fading_standard);
                    animationStateData.SetMix("hit", "attack", fading_standard);
                    animationStateData.SetMix("hit", "idle", fading_standard);
                    animationStateData.SetMix("hit", "jump", fading_standard);
                    animationStateData.SetMix("hit", "run", fading_standard);
                    animationStateData.SetMix("hit", "scream", fading_standard);

                    animationStateData.SetMix("idle", "die", fading_standard);
                    animationStateData.SetMix("idle", "attack", fading_standard);
                    animationStateData.SetMix("idle", "hit", fading_standard);
                    animationStateData.SetMix("idle", "jump", fading_standard);
                    animationStateData.SetMix("idle", "run", fading_standard);
                    animationStateData.SetMix("idle", "scream", fading_standard);

                    animationStateData.SetMix("jump", "die", fading_standard);
                    animationStateData.SetMix("jump", "attack", fading_standard);
                    animationStateData.SetMix("jump", "hit", fading_standard);
                    animationStateData.SetMix("jump", "idle", fading_standard);
                    animationStateData.SetMix("jump", "run", fading_standard);
                    animationStateData.SetMix("jump", "scream", fading_standard);

                    animationStateData.SetMix("run", "die", fading_standard);
                    animationStateData.SetMix("run", "attack", fading_standard);
                    animationStateData.SetMix("run", "hit", fading_standard);
                    animationStateData.SetMix("run", "idle", fading_standard);
                    animationStateData.SetMix("run", "jump", fading_standard);
                    animationStateData.SetMix("run", "scream", fading_standard);

                    animationStateData.SetMix("scream", "die", fading_standard);
                    animationStateData.SetMix("scream", "attack", fading_standard);
                    animationStateData.SetMix("scream", "hit", fading_standard);
                    animationStateData.SetMix("scream", "idle", fading_standard);
                    animationStateData.SetMix("scream", "jump", fading_standard);
                    animationStateData.SetMix("scream", "run", fading_standard);
                    break;
            }
            animationState = new AnimationState(animationStateData);

            // Event handling for all animations.
            /*
            animationState.Start += Start;
            animationState.End += End;
            animationState.Complete += Complete;
            animationState.Event += Event;
             * */

            skeleton.x = position.X;
            skeleton.y = position.Y;
        }

        //----------Spine----------
        /*
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
         * */

        public bool BoundingBoxCollision(Rectangle cbox) //Checken ob Rectangle mit Attachement (z.B. Keule) kollidiert
        {
            bounds.Update(skeleton, true);
            bool collision = false;
            if (bounds.AabbIntersectsSegment(cbox.X, cbox.Y, cbox.X, cbox.Y + cbox.Height)
                ||
                bounds.AabbIntersectsSegment(cbox.X + cbox.Width, cbox.Y, cbox.X + cbox.Width, cbox.Y + cbox.Height)
                ||
                bounds.AabbIntersectsSegment(cbox.X, cbox.Y, cbox.X + cbox.Width, cbox.Y)
                ||
                bounds.AabbIntersectsSegment(cbox.X, cbox.Y + cbox.Height, cbox.X + cbox.Width, cbox.Y + cbox.Height)
                )
            {
                if (bounds.IntersectsSegment(cbox.X, cbox.Y, cbox.X, cbox.Y + cbox.Height) != null
                    ||
                    bounds.IntersectsSegment(cbox.X + cbox.Width, cbox.Y, cbox.X + cbox.Width, cbox.Y + cbox.Height) != null
                    ||
                    bounds.IntersectsSegment(cbox.X, cbox.Y, cbox.X + cbox.Width, cbox.Y) != null
                    ||
                    bounds.IntersectsSegment(cbox.X, cbox.Y + cbox.Height, cbox.X + cbox.Width, cbox.Y + cbox.Height) != null
                    )
                {
                    collision = true;
                }
            }
            return collision;
        }
    }
}
