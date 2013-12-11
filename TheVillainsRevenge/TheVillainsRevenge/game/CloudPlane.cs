using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    class CloudPlane
    {
        Texture2D cloudTexture;
        List<Cloud> clouds = new List<Cloud>(); //Erstelle Blocks als List
        public int wind;
        public int number;

        public CloudPlane(int planeNumber)
        {
            number = planeNumber;
            wind = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Wind"]);
        }

        public void Load(ContentManager Content, string textureName)
        {
            cloudTexture = Content.Load<Texture2D>("sprites/Level_1/Planes/" + textureName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cloud cloud in clouds)
            {
                cloud.Draw(spriteBatch, cloudTexture);
            }
        }

        public void Update(Map karte, GameTime gameTime)
        {
            //Wolken random erzeugen
            float randomFactor = (float)Math.Sin((double)gameTime.TotalGameTime.Milliseconds);
            randomFactor = (randomFactor + 1.0f) / 2; //Normalisieren
            //Entscheiden ob Wolke gespawned werden soll
            if (randomFactor > (0.05 - (float)Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Amount"]) / 200) && randomFactor < (0.05 + (float)Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Amount"]) / 200))
            {
                Console.WriteLine("YAY");
                //Wolkentyp bestimmen
                int type = 1;
                if (randomFactor < Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Type"]) / 10)
                {
                    type = 2;
                }
                //Wolkenposition bestimmen
                int top = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Top"]);
                int bottom = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Bottom"]);
                int spawnPosition = ((int)((bottom - top) * randomFactor)) + top;
                //Wolke erstellen
                clouds.Add(new Cloud(type, new Vector2(karte.size.X, spawnPosition), randomFactor));
            }

            //Wolken updaten
            foreach (Cloud cloud in clouds)
            {
                cloud.Update(wind);
            }
            foreach (Cloud cloud in clouds)
            {
                if (cloud.position.X < 0)
                {
                    clouds.Remove(cloud);
                    break;
                }
            }
        }
    }
}
