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

        public void anim(string newanim,int flip,bool loop, GameTime gameTime)
        {
            if(flip == 1)
                skeleton.flipX = true;
                flipSkel = true;
            if (flip == 2)
                skeleton.flipX = false;
                flipSkel = false;
            if (animation != newanim)
            {
                animationTimer = gameTime.TotalGameTime.TotalMilliseconds;
                animationState.SetAnimation(0, newanim, loop);
                animation = newanim;
            }
        }

        public float CurrentAnimTime(GameTime gameTime)
        {
            float time = (float)(gameTime.TotalGameTime.TotalMilliseconds - animationTimer) / 1000;
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
            switch (name)
            {
                case "skeleton":
<<<<<<< HEAD
                    animationStateData.SetMix("idle", "run", 0.2f);
                    animationStateData.SetMix("run", "idle", 0.4f);
=======
                    animationStateData.SetMix("idle", "run", acceleration);
                    animationStateData.SetMix("run", "idle", acceleration);
>>>>>>> origin/Animation
                    break;
                case "ashbrett":
                    break;
            }
            animationState = new AnimationState(animationStateData);

            // Event handling for all animations.
            animationState.Start += Start;
            animationState.End += End;
            animationState.Complete += Complete;
            animationState.Event += Event;

            skeleton.x = position.X;
            skeleton.y = position.Y;
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
