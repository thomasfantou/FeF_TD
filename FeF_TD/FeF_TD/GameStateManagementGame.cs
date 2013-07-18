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
using Lidgren.Network;

namespace FeF_TD
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameStateManagementGame : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public delegate void CallScreenEventHandler(Enumeration.Screen caller, Enumeration.Screen called);
        public static event CallScreenEventHandler CallScreenEvent;

        public delegate void StartServerEventHandler(String player, String game, int maxPlayer);
        public static event StartServerEventHandler StartServerEvent;
        public delegate void StopServerEventHandler();
        public static event StopServerEventHandler StopServerEvent;
        public delegate void StartClientEventHandler();
        public static event StartClientEventHandler StartClientEvent;
        //on passe par un event car dans la méthode CallScreen en static, on a pas accés aux variables non static


        GameScreen currentScreen;
        SMainMenu sMainMenu;
        SGameCreation sGameCreation;
        SGamesList sGamesList;
        SGame sGame;
        SPlayerRoom sPlayerRoom;

        static public Song menuSong;




        #endregion

        public GameStateManagementGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

            GameComponent1 gc = new GameComponent1(this);
            Components.Add(gc);
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            CallScreenEvent += new CallScreenEventHandler(GameStateManagementGame_CallScreenEvent);
            StartServerEvent += new StartServerEventHandler(GameStateManagementGame_StartServerEvent);
            StopServerEvent += new StopServerEventHandler(GameStateManagementGame_StopServerEvent);
            StartClientEvent += new StartClientEventHandler(GameStateManagementGame_StartClientEvent);
        }

        void GameStateManagementGame_CallScreenEvent(Enumeration.Screen caller, Enumeration.Screen called)
        {
            switch (called)
            {
                case Enumeration.Screen.MainMenu:
                    sMainMenu = new SMainMenu(graphics, spriteBatch, GraphicsDevice);
                    currentScreen = sMainMenu;
                    break;
                case Enumeration.Screen.GameCreation:
                    sGameCreation = new SGameCreation(graphics, spriteBatch, GraphicsDevice);
                    currentScreen = sGameCreation;
                    break;
                case Enumeration.Screen.GameList:
                    sGamesList = new SGamesList(graphics, spriteBatch, GraphicsDevice);
                    currentScreen = sGamesList;
                    break;
                case Enumeration.Screen.PlayerRoom:
                    sPlayerRoom = new SPlayerRoom(graphics, spriteBatch, GraphicsDevice);
                    currentScreen = sPlayerRoom;
                    break;
                case Enumeration.Screen.Game:
                    sGame = new SGame(graphics, spriteBatch, GraphicsDevice);
                    currentScreen = sGame;
                    break;
            }

            currentScreen.LoadContent(Content);
        }

        void GameStateManagementGame_StartServerEvent(string player, string game, int maxPlayer)
        {
            Server.Start();
            Server.HostName = player;
            Server.GameName = game;
            Server.MaxPlayer = maxPlayer;
            Server.Date = DateTime.Now;
        }


        void GameStateManagementGame_StopServerEvent()
        {
            Server.Stop();
        }

        void GameStateManagementGame_StartClientEvent()
        {
            Client.Start();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuSong = Content.Load<Song>(Config.MUSICS + "crystal_castles-1991");
            MediaPlayer.Play(menuSong);
            MediaPlayer.Pause();

            //*
            sMainMenu = new SMainMenu(graphics, spriteBatch, GraphicsDevice);
            currentScreen = sMainMenu;
            currentScreen.LoadContent(Content);
            //*/

            /*
            sGameCreation = new SGameCreation(graphics, spriteBatch, GraphicsDevice);
            currentScreen = sGameCreation;
            currentScreen.LoadContent(Content);
            //*/

            /*
            sGamesList = new SGamesList(graphics, spriteBatch, GraphicsDevice);
            currentScreen = sGamesList;
            currentScreen.LoadContent(Content);
            //*/

            /*
            sGame = new SGame(graphics, spriteBatch, GraphicsDevice);
            currentScreen = sGame;
            currentScreen.LoadContent(Content);
            //*/


        }



        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);

            currentScreen.MouseEnabled = this.IsActive;
            currentScreen.Update(gameTime);


            if (Server.IsStarted)
                Server.Update();

            if (Client.IsStarted)
                Client.Update();
            
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            currentScreen.Draw(gameTime);

        }

        public static void CallScreen(Enumeration.Screen caller, Enumeration.Screen called)
        {
            CallScreenEvent(caller, called);
        }

        public static void StartServer(String player, String game, int maxPlayer)
        {
            StartServerEvent(player, game, maxPlayer);
        }

        public static void StopServer()
        {
            StopServerEvent();
        }

        public static void StartClient()
        {
            StartClientEvent();
        }
    }
}
