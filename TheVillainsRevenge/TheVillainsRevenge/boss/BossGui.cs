using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class BossGUI : GUI
    {
        Texture2D BossBar,PrincessHUD;
        public int GFace = 0;
        public override void Load(ContentManager Content)
        {
            HUDTexture = Content.Load<Texture2D>("sprites/hud");
            PrincessHUD = Content.Load<Texture2D>("sprites/hud_sweetcheeks");
            BossBar = Content.Load<Texture2D>("sprites/bossbar");
        }
        public void Draw(SpriteBatch spriteBatch, int leben, int bossleben)
        {
            // --------- Leben -------------
            switch (leben)
            {
                case 4:
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(0, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 3:
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(234, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 2:
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(234 * 2, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 1:
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(234 * 3, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
            }
            // --------- Face -------------
            switch (GFace)
            {
                case 0:
                    spriteBatch.Draw(PrincessHUD, new Vector2(80, 45), new Rectangle(0, 0, 182, 192), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 1:
                    spriteBatch.Draw(PrincessHUD, new Vector2(80, 45), new Rectangle(182, 0, 182, 192), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 2:
                    spriteBatch.Draw(PrincessHUD, new Vector2(80, 45), new Rectangle(182 * 2, 0, 182, 192), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
            }  
            spriteBatch.Draw(BossBar, new Vector2((Game1.resolution.X / 2) - 500, Game1.resolution.Y - 64), new Rectangle(0, 0, bossleben*10, 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
    }
}
