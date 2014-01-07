using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVillainsRevenge
{
    class Button
    {
        public string name;
        Rectangle cuttexturePassive;
        Rectangle cuttextureActive;
        public bool activated;
        public bool selected;
        public bool blinkable;
        public bool blink;

        public Button(string buttonName, Rectangle cutPassive, Rectangle cutActive, bool blinkABLE)
        {
            name = buttonName;
            cuttexturePassive = cutPassive;
            cuttextureActive = cutActive;
            blinkable = blinkABLE;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D texture, Effect buttonShader)
        {
            Rectangle cut;
            if (!blinkable)
            {
                if (!activated)
                {
                    cut = cuttexturePassive;
                    buttonShader.Parameters["activated"].SetValue(false);
                }
                else //Wenn Button aktiviert ist entsprechend darstellen
                {
                    cut = cuttextureActive;
                    buttonShader.Parameters["activated"].SetValue(true);
                }
            }
            else
            {
                if (!activated)
                {
                    cut = cuttexturePassive;
                }
                else //Wenn Button aktiviert ist entsprechend darstellen
                {
                    cut = cuttextureActive;
                }
            }
            spriteBatch.Draw(texture, position, cut, Color.White);
        }
    }
}
