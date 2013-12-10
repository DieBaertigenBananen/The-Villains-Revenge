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
            luaInstance["playerSpeed"] = "";
            luaInstance["playerAirspeed"] = "";
            luaInstance["playerJumppower"] = "";
            luaInstance["playerGravitation"] = "";
            luaInstance["playerLifes"] = "";
            luaInstance["playerStartLifes"] = "";
            luaInstance["playerItem1"] = "";
            luaInstance["playerItem2"] = "";
            luaInstance["cameraLeftspace"] = "";
            luaInstance["cameraRightspace"] = "";
            luaInstance["cameraBottomspace"] = "";
            luaInstance["cameraTopspace"] = "";
            luaInstance["playerScale"] = "";
            luaInstance["playerCollisionOffsetX"] = "";
            luaInstance["playerCollisionOffsetY"] = "";
            luaInstance["playerCollisionWidth"] = "";
            luaInstance["playerCollisionHeight"] = "";
            luaInstance["planeTilesBackground0"] = "";
            luaInstance["planeTilesBackground1"] = "";
            luaInstance["planeTilesBackground2"] = "";
            luaInstance["planeTilesBackground3"] = "";

            /*
            luaInstance.RegisterFunction("getPlayerHitpoints", this, this.GetType().GetMethod("getPlayerHitpoints"));
            public int getPlayerHitpoints()
                {
                    return (int)
                }
            */
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu.load(Content);
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
                menuOption = menu.update();
                if (menuOption == 3)
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
                else if (menuOption == 2)
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
                    menu.load(Content);
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
                        menu.load(Content);
                    }
                }
            }
            if (menuOption == 0 || input.end)
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (menu != null)
                menu.draw(spriteBatch);
            else if (game != null)
                game.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
