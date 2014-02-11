using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LuaInterface;
using System.Threading;

namespace TheVillainsRevenge
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Vector2 resolution = new Vector2(1920, 1080);
        BossScreen boss;
        GameScreen game;
        MenuScreen menu;
        public static bool sound = true;
        public static bool stretch;
        public static Input input;
        public static Lua luaInstance = new Lua();
        public static bool debug = false;
        public static int level = 1;
        public static int leben = 4;
        public static TimeSpan time = new TimeSpan();
        public struct SaveData
        {
            public bool sound;
            public bool fullscreen;
            public bool stretch;
            public int level;
            public int leben;
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth / 16 * 9;
            graphics.IsFullScreen = false;
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            this.IsMouseVisible = true;
            input = new Input();
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            menu = new MenuScreen(3);
            //game = new GameScreen();

            //Loading Stuffs
            luaInstance["playerStartLifes"] = "";
            luaInstance["playerScale"] = "";
            luaInstance["playerCollisionOffsetX"] = "";
            luaInstance["playerCollisionOffsetY"] = "";
            luaInstance["playerCollisionWidth"] = "";
            luaInstance["playerCollisionHeight"] = "";
            luaInstance["playerJumpCutoff"] = "";
            luaInstance["playerSmashIntensity"] = "";


            luaInstance["planeTilesBackground0"] = "";
            luaInstance["planeTilesBackground1"] = "";
            luaInstance["planeTilesBackground2"] = "";
            luaInstance["planeTilesBackground3"] = "";
            luaInstance["planeForegroundHeightOffset"] = "";
            
            luaInstance["blockSpeed"] = "";

            luaInstance["triggerTime"] = "";

            luaInstance["heroScale"] = "";
            luaInstance["heroStartTime"] = "";
            luaInstance["heroCollisionOffsetX"] = "";
            luaInstance["heroCollisionOffsetY"] = "";
            luaInstance["heroCollisionWidth"] = "";
            luaInstance["heroCollisionHeight"] = "";
            luaInstance["heroCloseWalkRange"] = "";

            luaInstance["enemyCollisionOffsetX"] = "";
            luaInstance["enemyCollisionOffsetY"] = "";
            luaInstance["enemyCollisionWidth"] = "";
            luaInstance["enemyCollisionHeight"] = "";

            //Update
            luaInstance["playerSpeed"] = "";
            luaInstance["playerAirspeed"] = "";
            luaInstance["playerJumppower"] = "";
            luaInstance["playerGravitation"] = "";
            luaInstance["playerAcceleration"] = "";
            luaInstance["playerSchlagRange"] = "";
            luaInstance["playerMegaSchlagRange"] = "";
            luaInstance["playerMegaSchlagCooldown"] = "";

            luaInstance["heroSpeed"] = "";
            luaInstance["heroKISpeed"] = "";
            luaInstance["heroAirspeed"] = "";
            luaInstance["heroJumppower"] = "";
            luaInstance["heroGravitation"] = "";

            luaInstance["bunnySize"] = "";
            luaInstance["bunnySpeed"] = "";
            luaInstance["bunnyGravitation"] = "";

            luaInstance["monkeySize"] = "";
            luaInstance["monkeySpeed"] = "";
            luaInstance["monkeyThrowRange"] = "";
            luaInstance["monkeyGravitation"] = "";

            luaInstance["kackeSpeed"] = "";
            luaInstance["kackeStartY"] = "";
            luaInstance["objectGravitation"] = "";

            luaInstance["cameraLeftspace"] = "";
            luaInstance["cameraRightspace"] = "";
            luaInstance["cameraBottomspace"] = "";
            luaInstance["cameraTopspace"] = "";
            luaInstance["cameraMaxBewegung"] = "";
            luaInstance["cameraBewegungSteps"] = "";

            luaInstance["minimapWidth"] = "";
            luaInstance["minimapOffsetX"] = "";

            luaInstance["itemSlowTime"] = "";
            luaInstance["itemSlowReduce"] = "";

            //Enter
            luaInstance["playerLifes"] = "";
            luaInstance["playerItem1"] = "";
            luaInstance["playerItem2"] = "";

            luaInstance["cloudPlane1Width"] = "";
            luaInstance["cloudPlane1Height"] = "";
            luaInstance["cloudPlane1Top"] = "";
            luaInstance["cloudPlane1Bottom"] = "";
            luaInstance["cloudPlane1Amount"] = "";
            luaInstance["cloudPlane1Chaos"] = "";
            luaInstance["cloudPlane1Type"] = "";
            luaInstance["cloudPlane1Wind"] = "";
            luaInstance["cloudPlane1SizeMin"] = "";
            luaInstance["cloudPlane1SizeMax"] = "";

            luaInstance["cloudPlane2Width"] = "";
            luaInstance["cloudPlane2Height"] = "";
            luaInstance["cloudPlane2Top"] = "";
            luaInstance["cloudPlane2Bottom"] = "";
            luaInstance["cloudPlane2Amount"] = "";
            luaInstance["cloudPlane2Chaos"] = "";
            luaInstance["cloudPlane2Type"] = "";
            luaInstance["cloudPlane2Wind"] = "";
            luaInstance["cloudPlane2SizeMin"] = "";
            luaInstance["cloudPlane2SizeMax"] = "";

            luaInstance["cloudPlane3Width"] = "";
            luaInstance["cloudPlane3Height"] = "";
            luaInstance["cloudPlane3Top"] = "";
            luaInstance["cloudPlane3Bottom"] = "";
            luaInstance["cloudPlane3Amount"] = "";
            luaInstance["cloudPlane3Chaos"] = "";
            luaInstance["cloudPlane3Type"] = "";
            luaInstance["cloudPlane3Wind"] = "";
            luaInstance["cloudPlane3SizeMin"] = "";
            luaInstance["cloudPlane3SizeMax"] = "";

            luaInstance["princessRageChance"] = "";
            luaInstance["princessRageWarmup"] = "";
            luaInstance["princessEnrageSpeed"] = "";
            luaInstance["princessUnrageSpeed"] = "";
            luaInstance["princessRageLimit"] = "";
            luaInstance["princessCoverTime"] = "";
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Load();
            menu.Load(Content);
            //game.load(Content);
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            if (this.IsMouseVisible == graphics.IsFullScreen)
            {
                this.IsMouseVisible = !graphics.IsFullScreen;
            }
            //if (input.debug)
            //{
            //    debug = !debug;
            //}
            luaInstance.DoFile("Level_"+level+"/luascript.txt");
            input.Update();
            int menuOption = 0;
            //Wenn Menü existiert
            //menuOption == 0 = game ende
            //menuOption == 1 = läuft weiter
            //menuOption == 2 = nächste szene
            if (menu != null)
            {
                //Update und hole Wert vom Menü
                menuOption = menu.Update(gameTime);
                if (menuOption == 2 || menuOption == 3)
                {
                    if (menuOption == 3)
                    {
                        level = 1;
                        leben = 4;
                    }
                    Save();
                    menu = null; //entlädt das menü
                    Content.Unload(); //entlädt den Content
                    if (level != 5)
                    {
                        game = new GameScreen(); //lädt das Game
                        for (int i = 0; i < 7; i++)
                        {
                            this.BeginDraw();                        
                            game.Load(Content, gameTime, spriteBatch, i); // lädt die Game Bilder 
                            this.EndDraw();
                            //Thread.Sleep(250);
                        }
                        Thread.Sleep(500);
                    }
                    else
                    {
                        boss = new BossScreen(); //lädt das Game
                        boss.Load(Content); // lädt die Game Bilder
                    }
                }
            }
            else if (game != null)
            {
                menuOption = game.Update(gameTime, Content);
                if (menuOption == 2) //GameScreen beendet (Spieler tot)
                {
                    game = null;
                    Content.Unload();
                    GC.Collect();
                    Load();
                    menu = new MenuScreen(1);
                    menu.Load(Content);
                }
                else if (menuOption == 3) //Level Ende
                {                        
                    game = null;
                    Content.Unload();
                    GC.Collect();
                    if(level < 5)
                        level += 1;
                    Save();
                    menu = new MenuScreen(2);
                    menu.Load(Content);
                }
                else if (menuOption == 4) //GameScreen beendet (Exit)
                {
                    game = null;
                    Content.Unload();
                    Load();
                    menu = new MenuScreen(0);
                    menu.Load(Content);
                    menuOption = 1;
                }
            }
            else if (boss != null)
            {
                if (input.escape && boss.paused)
                {
                    boss = null;
                    Content.Unload();
                    Load();
                    menu = new MenuScreen(0);
                    menu.Load(Content);
                    menuOption = 1;
                }
                else
                {
                    menuOption = boss.Update(gameTime, Content);
                    if (menuOption == 2) //GameScreen beendet (Spieler tot)
                    {
                        boss = null;
                        Content.Unload();
                        Load();
                        menu = new MenuScreen(1);
                        menu.Load(Content);
                    }
                    else if (menuOption == 3) //Level Ende
                    {
                        boss = null;
                        Content.Unload();
                        menu = new MenuScreen(2);
                        menu.Load(Content);
                    }
                    else if (menuOption == 4) //GameScreen beendet (Exit)
                    {
                        boss = null;
                        Content.Unload();
                        Load();
                        menu = new MenuScreen(0);
                        menu.Load(Content);
                        menuOption = 1;
                    }
                }
            }
            if (input.fullscreen) //F11 = Toggle Fullscreen
            {
                Game1.ToggleFullscreen();
            }
            if (menuOption == 0 || input.end)
            {
                Save();
                this.Exit();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (menu != null)
                menu.Draw(spriteBatch, gameTime);
            else if (game != null)
                game.Draw(gameTime, spriteBatch);
            else if (boss != null)
                boss.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }

        void Load()
        {
            SaveData data = new SaveData();
            XmlSerializer reader = new
               XmlSerializer(data.GetType());
            // Read the XML file.
            if (File.Exists("save"))
            {
                StreamReader file = new StreamReader("save");
                // Deserialize the content of the file into a Book object.
                data = (SaveData)reader.Deserialize(file);
                file.Close();
                stretch = data.stretch;
                sound = data.sound;
                if (data.fullscreen != graphics.IsFullScreen)
                {
                    ToggleFullscreen();
                }
                if (data.level <= 5)
                {
                    level = data.level;
                }
                else
                {
                    level = 1;
                }
                if (level != 1)
                {
                    if (leben <= 4 || leben > 0)
                    {
                        leben = data.leben;
                    }
                    else
                    {
                        leben = 4;
                    }
                }
                else
                    leben = 4;
            }
        }
        void Save()
        {
            SaveData data = new SaveData()
            {
                sound = Game1.sound,
                fullscreen = graphics.IsFullScreen,
                stretch = Game1.stretch,
                level = Game1.level,
                leben = Game1.leben,
            };
            XmlSerializer writer = new XmlSerializer(data.GetType());
            StreamWriter file = new StreamWriter("save");
            writer.Serialize(file, data);
            file.Close();
        }

        public static void ToggleFullscreen()
        {
            if (graphics.IsFullScreen)
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth / 16 * 9;
            }
            else
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            graphics.ApplyChanges();
        }
    }
}
