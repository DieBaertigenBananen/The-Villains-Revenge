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
    class Button
    {
        public string name;
        Rectangle cuttexture;
        public bool active;
        bool activated = false;
        int spriteStateCount;
        int spriteState = 1;

        public Button(string buttonName, Rectangle cuttex, int spriteStates)
        {
            name = buttonName;
            cuttexture = cuttex;
            spriteStateCount = spriteStates;
        }

        public void Update(bool isButtonActive)
        {
            active = isButtonActive;
            if (MenuScreen.changeSprite) //Sprite wechseln
            {
                spriteState++;
                if (spriteState > spriteStateCount)
                {
                    spriteState = 1;
                }
            }
            if (name == "sound") //Soundbutton
            {
                if (Game1.sound)
                {
                    activated = true;
                }
                else
                {
                    activated = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D texture)
        {
            Rectangle cut = cuttexture; //Von erster CutTexture ausgehen
            if (active)
            {
                cut.X += cut.Width * spriteState;
            }
            spriteBatch.Draw(texture, position, cut, Color.White); //Button zeichnen

            //Display ButtonState
            if (name == "sound") //Soundbutton
            {
                cut = cuttexture; //CutTexture zurücksetzen
                if (activated) //Sound an
                {
                    if (!active) //Nicht aktiv
                    {
                        cut.X += cut.Width * (spriteStateCount + 1);
                    }
                    else //Aktiv
                    {
                        cut.X += cut.Width * (spriteStateCount + 2);
                    }
                    spriteBatch.Draw(texture, new Vector2(position.X + 60, position.Y), cut, Color.White); //StateOverlay zeichnen
                }
            }
        }
    }
}
