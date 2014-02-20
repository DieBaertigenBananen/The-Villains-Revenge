using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheVillainsRevenge
{
    class Blood
    {
        Random bloodrand;
        public Vector2 bloodpos = new Vector2(-2000, 0);
        Spine blood = new Spine();
        public void Load()
        {
            blood.Load(Vector2.Zero, "blood", 1.0f, 1.0f);
            bloodrand = new Random();
        }
        public void Draw(GameTime gameTime,Camera camera)
        {
            Vector2 bp = new Vector2(bloodpos.X + camera.viewport.X, bloodpos.Y + camera.viewport.Y);
            blood.Draw(gameTime, camera, bp);
        }
        public void voll()
        {
            bloodpos.X = bloodrand.Next(192, 1920 - 192);
            bloodpos.Y = bloodrand.Next(108, 1080 - 108);
            blood.Clear(0);
            blood.anim("clear", 0, false);
            blood.anim("splat_full", 0, false);
        }
        public void Splash()
        {
            int anim = bloodrand.Next(1, 3);
            bloodpos.X = bloodrand.Next(192, 1920 - 192);
            bloodpos.Y = bloodrand.Next(108, 1080 - 108);
            blood.Clear(0);
            blood.anim("clear", 0, false);
            blood.anim("splat" + anim, 0, false);
        }
    }
}
