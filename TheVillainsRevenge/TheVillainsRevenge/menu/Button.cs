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

        public Button(string buttonName, Rectangle cutPassive, Rectangle cutActive)
        {
            name = buttonName;
            cuttexturePassive = cutPassive;
            cuttextureActive = cutActive;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D texture, Effect buttonShader)
        {
            Rectangle cut;
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
            spriteBatch.Draw(texture, position, cut, Color.White);
        }
    }
}
