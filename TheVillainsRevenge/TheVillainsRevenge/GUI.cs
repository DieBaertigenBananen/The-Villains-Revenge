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
        Texture2D Texture;

        public void Load(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("sprites/gui");
        }
        public void Draw(SpriteBatch spriteBatch,int leben,Vector2 spielerpos,Vector2 heropos,Vector2 kartesize)
        {
            for (int i = 0; i != leben; i++)
            {
                spriteBatch.Draw(Texture, new Vector2(10+i*50, 0), new Rectangle(0, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
            //Breite = 450
            int spielerX = (int)((spielerpos.X / kartesize.X) * 450);
            int heldX = (int)((heropos.X / kartesize.X) * 450);
            //Start
            spriteBatch.Draw(Texture, new Vector2((Game1.resolution.X/2)-300, Game1.resolution.Y-50), new Rectangle(144, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            //Ende 
            spriteBatch.Draw(Texture, new Vector2((Game1.resolution.X / 2) + 150, Game1.resolution.Y - 50), new Rectangle(144, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            //Spieler 
            spriteBatch.Draw(Texture, new Vector2((Game1.resolution.X / 2) - 300 + spielerX, Game1.resolution.Y - 50), new Rectangle(48, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            //Held 
            spriteBatch.Draw(Texture, new Vector2((Game1.resolution.X / 2) - 300 + heldX, Game1.resolution.Y - 50), new Rectangle(96, 0, 48, 48), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
           
        }
    }
}
