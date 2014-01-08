using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LuaInterface;

namespace TheVillainsRevenge
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Vector2 resolution = new Vector2(1920, 1080);
        GameScreen game;
        MenuScreen menu;
        public static bool sound = false;
        public static bool stretch;
        public static Input input;
        public static Lua luaInstance = new Lua();
        public static bool debug = false;

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
            menu = new MenuScreen(false);
            //game = new GameScreen();

            //Loading Stuffs
            luaInstance["playerStartLifes"] = "";
            luaInstance["playerScale"] = "";
            luaInstance["playerCollisionOffsetX"] = "";
            luaInstance["playerCollisionOffsetY"] = "";
            luaInstance["playerCollisionWidth"] = "";
            luaInstance["playerCollisionHeight"] = "";
            luaInstance["playerJumpCutoff"] = "";

            luaInstance["planeTilesBackground0"] = "";
            luaInstance["planeTilesBackground1"] = "";
            luaInstance["planeTilesBackground2"] = "";
            luaInstance["planeTilesBackground3"] = "";

            luaInstance["blockSpeed"] = "";

            luaInstance["triggerTime"] = "";

            luaInstance["heroScale"] = "";
            luaInstance["heroStartTime"] = "";
            luaInstance["heroCollisionOffsetX"] = "";
            luaInstance["heroCollisionOffsetY"] = "";
            luaInstance["heroCollisionWidth"] = "";
            luaInstance["heroCollisionHeight"] = "";

            luaInstance["enemyCollisionOffsetX"] = "";
            luaInstance["enemyCollisionOffsetY"] = "";
            luaInstance["enemyCollisionWidth"] = "";
            luaInstance["enemyCollisionHeight"] = "";

            //Update
            luaInstance["playerSpeed"] = "";
            luaInstance["playerAirspeed"] = "";
            luaInstance["playerJumppower"] = "";
            luaInstance["playerGravitation"] = "";

            luaInstance["heroSpeed"] = "";
            luaInstance["heroAirspeed"] = "";
            luaInstance["heroJumppower"] = "";
            luaInstance["heroGravitation"] = "";

            luaInstance["enemySpeed"] = "";
            luaInstance["enemyGravitation"] = "";

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
            menu.Load(Content);
            //game.load(Content);
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            if (input.debug)
            {
                debug = !debug;
            }
            luaInstance.DoFile("luascript.txt");
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
                if (menuOption == 2)
                {
                    menu = null; //entlädt das menü
                    Content.Unload(); //entlädt den Content
                    game = new GameScreen(); //lädt das Game
                    game.Load(Content); // lädt die Game Bilder
                }
            }
            else if (game != null)
            {
                if (input.back)
                {
                    game = null;
                    Content.Unload();
                    menu = new MenuScreen(false);
                    menu.Load(Content);
                    menuOption = 1;
                }
                else
                {
                    menuOption = game.Update(gameTime);
                    if (menuOption == 2) //GameScreen beendet (Spieler tot)
                    {
                        game = null;
                        Content.Unload();
                        menu = new MenuScreen(true);
                        menu.Load(Content);
                    }
                }
            }
            if (input.fullscreen) //F11 = Toggle Fullscreen
            {
                Game1.toggleFullscreen();
            }
            if (menuOption == 0 || input.end)
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (menu != null)
                menu.Draw(spriteBatch, gameTime);
            else if (game != null)
                game.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }

        public static void toggleFullscreen()
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
