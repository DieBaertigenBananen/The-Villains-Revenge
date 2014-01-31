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
        Texture2D BossBar;
        public override void Load(ContentManager Content)
        {
            HUDTexture = Content.Load<Texture2D>("sprites/hud");
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
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(232, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 2:
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(232 * 2, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 1:
                    spriteBatch.Draw(HUDTexture, new Vector2(50, 10), new Rectangle(232 * 3, 192, 232, 124), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
            }
            spriteBatch.Draw(BossBar, new Vector2((Game1.resolution.X / 2) - 500, Game1.resolution.Y - 64), new Rectangle(0, 0, bossleben*10, 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
    }
}
