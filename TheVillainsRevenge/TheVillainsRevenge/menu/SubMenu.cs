using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class SubMenu
    {
        public bool visible;
        public List<Button> buttons = new List<Button>();
        int optionCount;
        public int option;
        string name;
        public bool exit;
        int optionSpace;
        Vector2 offset;
        Texture2D button_texture;

        public SubMenu(int options, string menuName, Vector2 menuOffset, int optSpace)
        {
            optionCount = options;
            name = menuName;
            optionSpace = optSpace;
            offset = menuOffset;
        }

        public void Load(ContentManager Content)
        {
            button_texture = Content.Load<Texture2D>("sprites/menu/buttons_" + name);
        }

        public void Update(GameTime gameTime, bool changeSprite)
        {
            exit = false;
            if (Game1.input.down) //Nach unten
            {
                option++;
            }
            if (Game1.input.up) //Nach oben
            {
                option--;
            }
            if (option > optionCount - 1) //Von unten nach oben springen
            {
                option = 0;
            }
            else if (option < 0) //Von oben nach unten springen
            {
                option = optionCount - 1;
            }
            if (Game1.input.back || Game1.input.escape) //Esc (Untere/Letzte Option muss immer Exit/Return sein)
            {
                //Setze auf Exit
                if (option == optionCount - 1)
                {
                    exit = true;
                }
                else
                {
                    option = optionCount - 1;
                }
            }
            if (Game1.input.leftM)
            {
                if (name == "main")
                {
                    option = optionCount - 1;
                }
                else
                {
                    exit = true;
                }
            }
            //Buttons updaten
            for (int i = 0; i < buttons.Count(); ++i)
            {
                Button button = buttons.ElementAt(i);
                if (option == i)
                {
                    button.Update(changeSprite, true);
                }
                else
                {
                    button.Update(changeSprite, false);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Camera camera)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewportTransform);
            for (int i = 0; i < buttons.Count(); ++i)
            {
                Button button = buttons.ElementAt(i);
                button.Draw(spriteBatch, new Vector2((Game1.resolution.X / 2) + offset.X, (Game1.resolution.Y / 2) - (((optionCount - 1) * optionSpace) / 2) + (i * optionSpace) + offset.Y), button_texture); 
            }
            spriteBatch.End();
        }
    }
}
