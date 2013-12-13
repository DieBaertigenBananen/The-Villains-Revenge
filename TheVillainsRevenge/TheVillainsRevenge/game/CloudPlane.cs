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
        public int number;

        public double spawnTimer;

        int luaTop;
        int luaBottom;
        int luaAmount;
        int luaChaos;
        int luaType;
        public int luaWind;
        int testSpawn;
        int testType;
        int testPosition;
        int testSize;
        Random randomSpawn = new Random();
        Random randomType = new Random();
        Random randomPosition = new Random();
        Random randomSize = new Random();

        public CloudPlane(int planeNumber)
        {
            number = planeNumber;
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
            
            luaTop = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Top"]);
            luaBottom = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Bottom"]);
            luaAmount = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Amount"]);
            luaChaos = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Chaos"]);
            luaType = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Type"]);
            luaWind = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Wind"]);
            //Spawntimer
            if (gameTime.TotalGameTime.TotalMilliseconds > spawnTimer + ((100000 - (float)luaAmount) * ((100 - (float)luaChaos) / 100)))
            {
                spawnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                //Entscheiden ob Wolke gespawned werden soll
                testSpawn = randomSpawn.Next(0, 100);
                if (testSpawn > luaChaos)
                {
                    //Wolkentyp bestimmen
                    testType = randomType.Next(0, 10);
                    int type = 1;
                    if (testType < luaType)
                    {
                        type = 2;
                    }
                    //Wolkenposition bestimmen
                    testPosition = randomPosition.Next(0, 100);
                    int spawnPosition = ((int)((luaBottom - luaTop) * ((float)testPosition / 100))) + luaTop;
                    //Wolke erstellen
                    testSize = randomSize.Next(0, 100);
                    clouds.Add(new Cloud(type, new Vector2(karte.size.X, spawnPosition), (float)testSize / 100));
                }
            }

            //Wolken updaten
            foreach (Cloud cloud in clouds)
            {
                cloud.Update(luaWind);
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
