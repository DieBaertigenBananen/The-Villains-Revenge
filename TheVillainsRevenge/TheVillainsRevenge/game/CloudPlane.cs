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
        public Vector2 size;
        public Vector2 position;

        public double spawnTimer;

        int luaTop;
        int luaBottom;
        int luaAmount;
        int luaChaos;
        int luaType;
        public float luaWind;
        int luaSizeMin;
        int luaSizeMax;

        int testSpawn;
        int testType;
        int testPosition;
        int testSize;

        Random randomNumber = new Random();

        public CloudPlane(int planeNumber)
        {
            number = planeNumber;

            luaTop = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Top"]);
            luaBottom = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Bottom"]);
            luaAmount = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Amount"]);
            luaChaos = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Chaos"]);
            luaType = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Type"]);
            luaWind = (float)Game1.luaInstance["cloudPlane" + number.ToString() + "Wind"];
            luaSizeMin = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "SizeMin"]);
            luaSizeMax = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "SizeMax"]);
            size.X = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Width"]);
            size.Y = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Height"]);
        }

        public void Load(ContentManager Content, string textureName, Map karte, Camera camera)
        {
            cloudTexture = Content.Load<Texture2D>("sprites/Level_1/Planes/" + textureName);
            spawnTimer = 0;
            for (int i = 0; i <= size.X / luaWind; i++)
            {
                if (spawnTimer > (((100000 - (float)luaAmount) * ((100 - (float)luaChaos) / 100)) / 30))
                {
                    spawnTimer = 0;
                    SpawnCloud(karte, camera);
                }
                //Wolken updaten
                for (int t = 0; t < clouds.Count(); t++)
                {
                    Cloud cloud = clouds.ElementAt(t);
                    cloud.Update(luaWind);
                    if (cloud.position.X < -cloud.cuttexture.Width)
                    {
                        clouds.Remove(cloud);
                    }
                }
                spawnTimer++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cloud cloud in clouds)
            {
                cloud.Draw(spriteBatch, cloudTexture, position);
            }
        }

        public void Update(Map karte, GameTime gameTime, Camera camera)
        {
            luaTop = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Top"]);
            luaBottom = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Bottom"]);
            luaAmount = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Amount"]);
            luaChaos = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Chaos"]);
            luaType = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Type"]);
            luaWind = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Wind"]);
            luaSizeMin = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "SizeMin"]);
            luaSizeMax = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "SizeMax"]);
            size.X = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Width"]);
            size.Y = Convert.ToInt32((double)Game1.luaInstance["cloudPlane" + number.ToString() + "Height"]);

            position.X = camera.viewport.X - ((size.X - camera.viewport.Width) * (camera.viewport.X / (karte.size.X - camera.viewport.Width)));
            position.Y = camera.viewport.Y - ((size.Y - camera.viewport.Height) * (camera.viewport.Y / (karte.size.Y - camera.viewport.Height)));

            //Spawntimer
            if (gameTime.TotalGameTime.TotalMilliseconds > spawnTimer + ((100000 - (float)luaAmount) * ((100 - (float)luaChaos) / 100)))
            {
                spawnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                SpawnCloud(karte, camera);
            }

            //Wolken updaten
            for (int i = 0; i < clouds.Count(); i++)
            {
                Cloud cloud = clouds.ElementAt(i);
                cloud.Update(luaWind);
                if (cloud.position.X < -cloud.cuttexture.Width)
                {
                    clouds.Remove(cloud);
                }
            }
        }

        public void SpawnCloud(Map karte, Camera camera)
        {
            //Entscheiden ob Wolke gespawned werden soll
            testSpawn = randomNumber.Next(0, 100);
            if (testSpawn >= luaChaos)
            {
                //Wolkentyp bestimmen
                testType = randomNumber.Next(0, 10);
                int type = 1;
                if (testType < luaType)
                {
                    type = 2;
                }
                //Wolkenposition bestimmen
                testPosition = randomNumber.Next(0, 100);
                int spawnPosition = ((int)((luaBottom - luaTop) * ((float)testPosition / 100))) + luaTop;
                //Wolke erstellen
                testSize = randomNumber.Next(luaSizeMin, luaSizeMax);
                clouds.Add(new Cloud(type, new Vector2(size.X, spawnPosition), (float)testSize / 100));
            }
        }
    }
}
