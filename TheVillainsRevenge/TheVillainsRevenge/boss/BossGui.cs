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
            GuiTexture = Content.Load<Texture2D>("sprites/gui");
            BossBar = Content.Load<Texture2D>("sprites/bossbar");
        }
        public void Draw(SpriteBatch spriteBatch, int leben, int bossleben)
        {
            switch (leben)
            {
                case 4:
                    spriteBatch.Draw(GuiTexture, new Vector2(10, 10), new Rectangle(0, 0, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 3:
                    spriteBatch.Draw(GuiTexture, new Vector2(10, 10), new Rectangle(64, 0, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 2:
                    spriteBatch.Draw(GuiTexture, new Vector2(10, 10), new Rectangle(128, 0, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 1:
                    spriteBatch.Draw(GuiTexture, new Vector2(10, 10), new Rectangle(192, 0, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
            }
            spriteBatch.Draw(BossBar, new Vector2((Game1.resolution.X / 2) - Convert.ToInt32((double)Game1.luaInstance["minimapOffsetX"]), Game1.resolution.Y - 64), new Rectangle(0, 0, bossleben*10, 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
    }
}
