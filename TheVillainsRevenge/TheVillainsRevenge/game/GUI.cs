﻿using System;
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
        public Texture2D MiniTexture,HUDTexture,Mini_A_Texture,Mini_B_Texture;
        Texture2D ItemTexture;

        public virtual void Load(ContentManager Content)
        {
            MiniTexture = Content.Load<Texture2D>("sprites/minimap");
            Mini_A_Texture = Content.Load<Texture2D>("sprites/mini_ashbrett");
            Mini_B_Texture = Content.Load<Texture2D>("sprites/mini_bonepuker");
            HUDTexture = Content.Load<Texture2D>("sprites/hud");
            ItemTexture = Content.Load<Texture2D>("sprites/items");
        }
        public virtual void Draw(SpriteBatch spriteBatch,int leben,Vector2 spielerpos,Vector2 heropos,Vector2 kartesize,int sitem1,int sitem2,int GUIFace)
        {
            if (leben != 0)
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
                // --------- Itemslot Links -------------
                spriteBatch.Draw(HUDTexture, new Vector2(10, 129), new Rectangle(0, 316, 130, 130), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                switch (sitem1)
                {
                    case 1:
                        spriteBatch.Draw(ItemTexture, new Vector2(20, 140), new Rectangle(384, 0, 96, 96), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        spriteBatch.Draw(ItemTexture, new Vector2(20, 140), new Rectangle(0, 0, 96, 96), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 3:
                        spriteBatch.Draw(ItemTexture, new Vector2(20, 140), new Rectangle(288, 0, 96, 96), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                }                
                // --------- Itemslot Rechts -------------
                spriteBatch.Draw(HUDTexture, new Vector2(200, 129), new Rectangle(130, 316, 133, 130), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                switch (sitem2)
                {
                    case 1: //zeit
                        spriteBatch.Draw(ItemTexture, new Vector2(220, 140), new Rectangle(384, 0, 96, 96), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 2: //Banana
                        spriteBatch.Draw(ItemTexture, new Vector2(220, 140), new Rectangle(0, 0, 96, 96), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 3: //Monkey
                        spriteBatch.Draw(ItemTexture, new Vector2(220, 140), new Rectangle(288, 0, 96, 96), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                }   
                
                // --------- BonePuker -------------
                switch (GUIFace)
                {
                    case 0:
                        spriteBatch.Draw(HUDTexture, new Vector2(80, 45), new Rectangle(0, 0, 182,192), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 1:
                        spriteBatch.Draw(HUDTexture, new Vector2(80, 45), new Rectangle(182, 0, 182, 192), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        spriteBatch.Draw(HUDTexture, new Vector2(80, 45), new Rectangle(182 * 2, 0, 182, 192), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        break;
                }  
                 //Itemslot hinten
                //Leben


                int spielerX = (int)((spielerpos.X / kartesize.X) * 1000);
                int heldX = (int)((heropos.X / kartesize.X) * 1000);
                //Minikarte
                spriteBatch.Draw(MiniTexture, new Vector2((Game1.resolution.X / 2) -550 , Game1.resolution.Y - 150), new Rectangle(0, 0, 1049, 127), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                //Ashbrett
                spriteBatch.Draw(Mini_A_Texture, new Vector2((Game1.resolution.X / 2) - 550 + heldX, Game1.resolution.Y - 159), new Rectangle(0, 0, 74, 109), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                //Bonepuker
                spriteBatch.Draw(Mini_B_Texture, new Vector2((Game1.resolution.X / 2) - 550 + spielerX, Game1.resolution.Y - 185), new Rectangle(0, 0, 79, 135), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
    }
}
