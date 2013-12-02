using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class SpineTest
    {
        SkeletonRenderer skeletonRenderer;
        Skeleton skeleton;
        Slot headSlot;
        AnimationState state;
        SkeletonBounds bounds = new SkeletonBounds();

        public SpineTest()
        {
        }

        public void Load()
        {
            skeletonRenderer = new SkeletonRenderer(Game1.graphics.GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            String name = "spineboy"; // "goblins";

            Atlas atlas = new Atlas("spine/sprites/" + name + ".atlas", new XnaTextureLoader(Game1.graphics.GraphicsDevice));
            SkeletonJson json = new SkeletonJson(atlas);
            skeleton = new Skeleton(json.ReadSkeletonData("spine/sprites/" + name + ".json"));
            if (name == "goblins") skeleton.SetSkin("goblingirl");
            skeleton.SetSlotsToSetupPose(); // Without this the skin attachments won't be attached. See SetSkin.

            // Define mixing between animations.
            AnimationStateData stateData = new AnimationStateData(skeleton.Data);
            if (name == "spineboy")
            {
                stateData.SetMix("walk", "jump", 0.2f);
                stateData.SetMix("jump", "walk", 0.4f);
            }

            state = new AnimationState(stateData);

            if (true)
            {
                // Event handling for all animations.
                state.Start += Start;
                state.End += End;
                state.Complete += Complete;
                state.Event += Event;

                //state.SetAnimation(0, "drawOrder", true);
            }
            else
            {
                state.SetAnimation(0, "walk", false);
                TrackEntry entry = state.AddAnimation(0, "jump", false, 0);
                entry.End += new EventHandler<StartEndArgs>(End); // Event handling for queued animations.
                state.AddAnimation(0, "walk", true, 0);
            }

            skeleton.X = 320;
            skeleton.Y = 440;
            skeleton.UpdateWorldTransform();

            headSlot = skeleton.FindSlot("head");
        }

        public void Unload()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void Draw(GameTime gameTime)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.Black);

            state.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
            state.Apply(skeleton);
            skeleton.UpdateWorldTransform();
            skeletonRenderer.Begin();
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();

            bounds.Update(skeleton, true);
            MouseState mouse = Mouse.GetState();
            headSlot.G = 1;
            headSlot.B = 1;
            if (bounds.AabbContainsPoint(mouse.X, mouse.Y))
            {
                BoundingBoxAttachment hit = bounds.ContainsPoint(mouse.X, mouse.Y);
                if (hit != null)
                {
                    headSlot.G = 0;
                    headSlot.B = 0;
                }
            }
        }

        public void Start(object sender, StartEndArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + state.GetCurrent(e.TrackIndex) + ": start");
        }

        public void End(object sender, StartEndArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + state.GetCurrent(e.TrackIndex) + ": end");
        }

        public void Complete(object sender, CompleteArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + state.GetCurrent(e.TrackIndex) + ": complete " + e.LoopCount);
        }

        public void Event(object sender, EventTriggeredArgs e)
        {
            Console.WriteLine(e.TrackIndex + " " + state.GetCurrent(e.TrackIndex) + ": event " + e.Event);
        }
    }
}
