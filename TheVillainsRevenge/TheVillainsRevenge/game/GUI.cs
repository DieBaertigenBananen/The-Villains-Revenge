using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class GUI
    {
        Texture2D GuiTexture;
        Texture2D ItemTexture;
        int mm_width,mm_offsetX;

        public void Load(ContentManager Content)
        {
            GuiTexture = Content.Load<Texture2D>("sprites/gui");
            ItemTexture = Content.Load<Texture2D>("sprites/items");
        }
        public void Draw(SpriteBatch spriteBatch,int leben,Vector2 spielerpos,Vector2 heropos,Vector2 kartesize,int sitem1,int sitem2)
        {
            if (leben != 0)
            {
                //Itemslot hinten
                spriteBatch.Draw(GuiTexture, new Vector2(45, 10), new Rectangle(0, 112, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                switch (sitem2)
                {
                    case 1:
                        spriteBatch.Draw(ItemTexture, new Vector2(45 + 9, 18), new Rectangle(48, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        spriteBatch.Draw(ItemTexture, new Vector2(45 + 9, 18), new Rectangle(96, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 3:
                        spriteBatch.Draw(ItemTexture, new Vector2(45 + 9, 18), new Rectangle(144, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                }
                //Itemslot vorne
                spriteBatch.Draw(GuiTexture, new Vector2(30, 10), new Rectangle(0, 112, 64, 64), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                switch (sitem1)
                {
                    case 1:
                        spriteBatch.Draw(ItemTexture, new Vector2(30 + 9, 18), new Rectangle(48, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        spriteBatch.Draw(ItemTexture, new Vector2(30 + 9, 18), new Rectangle(96, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 3:
                        spriteBatch.Draw(ItemTexture, new Vector2(30 + 9, 18), new Rectangle(144, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                }
                //Leben
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


                //Breite = 450
                mm_width = Convert.ToInt32((double)Game1.luaInstance["minimapWidth"]);
                mm_offsetX = Convert.ToInt32((double)Game1.luaInstance["minimapOffsetX"]);
                int spielerX = (int)((spielerpos.X / kartesize.X) * mm_width);
                int heldX = (int)((heropos.X / kartesize.X) * mm_width);
                //Start
                spriteBatch.Draw(GuiTexture, new Vector2((Game1.resolution.X / 2) - mm_offsetX, Game1.resolution.Y - 50), new Rectangle(0, 64, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                //Ende 
                spriteBatch.Draw(GuiTexture, new Vector2((Game1.resolution.X / 2) - mm_offsetX + mm_width, Game1.resolution.Y - 50), new Rectangle(144, 64, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                //Spieler 
                spriteBatch.Draw(GuiTexture, new Vector2((Game1.resolution.X / 2) - mm_offsetX + spielerX, Game1.resolution.Y - 50), new Rectangle(48, 64, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                //Held 
                spriteBatch.Draw(GuiTexture, new Vector2((Game1.resolution.X / 2) - mm_offsetX + heldX, Game1.resolution.Y - 50), new Rectangle(96, 64, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
    }
}
