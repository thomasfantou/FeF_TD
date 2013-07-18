using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;

namespace FeF_TD
{
    public class SGamesList : GameScreen
    {
        #region Variables


        Texture2D background;
        Texture2D title;
        Texture2D cadre;
        Texture2D header;

        Texture2D gamelistrow;
        Texture2D gamelistrowup;
        Texture2D gamelistrows;
        Texture2D gamelistrowups;


        SpriteFont font;
        String[] headerStrings;

        Bouton boutonBack;
        Bouton boutonJoin;
        List<Row> rows;
        Texture2D stringName;
        InputForm inputName;

        float timerDiscovery;

        public delegate void HostListenerEventHandler(String gameName, String hostName, String players, String date);
        public static event HostListenerEventHandler HostListenerEvent;

        #endregion

        public SGamesList(GraphicsDeviceManager gdm, SpriteBatch sb, GraphicsDevice gd)
            : base(gdm, sb, gd)
        {
            this.screen = Enumeration.Screen.GameList;
            timerDiscovery = 2000;
        }

        public override void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>(Config.IMG_GAME + "background");
            title = content.Load<Texture2D>(Config.IMG_STRING + "joinagametitle");
            cadre = content.Load<Texture2D>(Config.IMG_SYS + "cadre");
            header = content.Load<Texture2D>(Config.IMG_SYS + "gamelistheader");
            

            this.font = content.Load<SpriteFont>(Config.FONT + "GamesListFont");
            headerStrings = new String[4];
            headerStrings[0] = Config.GAME_LIST_GAME;
            headerStrings[1] = Config.GAME_LIST_HOST;
            headerStrings[2] = Config.GAME_LIST_PLAYERS;
            headerStrings[3] = Config.GAME_LIST_TIME;

            boutonBack = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutonback"), new Vector2(130, 530),
                                        content.Load<Texture2D>(Config.IMG_SYS + "boutonbackover"), new Vector2(-63, -66));
            boutonJoin = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutonjoin"), new Vector2(680, 530),
                                        content.Load<Texture2D>(Config.IMG_SYS + "boutonjoinover"), new Vector2(-59, -64));

            rows = new List<Row>();
            //for (int i = 0; i < 2; i++) //valeurs brutes
            //{
            //    Bouton agame = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "gamelistrow"), new Vector2(119, 230 + (i*45)),
            //                            content.Load<Texture2D>(Config.IMG_SYS + "gamelistrowup"), new Vector2(-10, -10),
            //                            content.Load<Texture2D>(Config.IMG_SYS + "gamelistrows"),
            //                            content.Load<Texture2D>(Config.IMG_SYS + "gamelistrowups"));
            //    String[] strings = { "Test " + i, "Pseudo " + ((i+1)*10), "1/" + ((i+1)*2), "12:00"};
            //    Row row = new Row(agame, strings);
            //    rows.Add(row);
            //}


            gamelistrow = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrow");
            gamelistrowup = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrowup");
            gamelistrows = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrows");
            gamelistrowups = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrowups");

            stringName = content.Load<Texture2D>(Config.IMG_STRING + "playername");
            inputName = new InputForm(content.Load<Texture2D>(Config.IMG_SYS + "champtexte"), new Vector2(370, 570),
                                        content.Load<Texture2D>(Config.IMG_SYS + "champtexteselected"), new Vector2(-69, -69),
                                        content);
            

            GameStateManagementGame.StartClient();
            HostListenerEvent +=new HostListenerEventHandler(SGamesList_HostListenerEvent);

            base.LoadContent(content);
        }


        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            CheckMouseClick();

            timerDiscovery -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timerDiscovery <= 0)
            {
                Client.DiscoverNetwork();
                timerDiscovery = 2000;
            }

            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] key;

            key = keyboardState.GetPressedKeys();

            if (key.Count() != 0 && (previousKeyboardState.IsKeyUp(key[0])))
            {
                if (inputName.Selected)
                {
                    inputName.AddChar(key[0]);
                }
            }
            previousKeyboardState = keyboardState;

            base.Update(gameTime);

                
        }

        protected override void CheckMouseClick()
        {
            base.CheckMouseClick();

            if (leftMouseClicked)
            {

                if (boutonBack.IsMouseOver(mousePosition))
                    GameStateManagementGame.CallScreen(screen, Enumeration.Screen.MainMenu);
                else if (boutonJoin.IsMouseOver(mousePosition))
                {
                    if (inputName.Text != "")
                    {
                        foreach (Row row in rows)
                        {
                            if (row.Bouton.Selected)
                            {
                                Client.ConnectTo(row.HeaderStrings[0], row.HeaderStrings[1], null, inputName.Text);
                                GlobalAppVariables.PlayerName = inputName.Text;
                                GameStateManagementGame.CallScreen(Enumeration.Screen.GameList, Enumeration.Screen.PlayerRoom);
                            }
                        }
                    }
                }

                foreach (Row row in rows)
                    row.Bouton.Selected = false;

                foreach (Row row in rows)
                {
                    if (row.Bouton.IsMouseOver(mousePosition))
                        row.Bouton.Selected = true;
                }

                inputName.Selected = false;
                if (inputName.IsMouseOver(mousePosition))
                    inputName.Selected = true;

                leftMouseClicked = false;
            }
        }


        void SGamesList_HostListenerEvent(string gameName, string hostName, string players, string date)
        {
            bool alreadyExist = false;
            foreach (Row r in rows)
            {
                if (r.HeaderStrings[0] == gameName && r.HeaderStrings[1] == hostName)
                {
                    alreadyExist = true;
                    if (r.HeaderStrings[2] != players && r.HeaderStrings[3] == date)
                        r.HeaderStrings[2] = players;
                }
            }
            if (!alreadyExist)
            {
                Bouton agame = new Bouton(gamelistrow, new Vector2(119, 230 + (45 * rows.Count)),
                                        gamelistrowup, new Vector2(-10, -10),
                                        gamelistrows,
                                        gamelistrowups);
                String[] strings = { gameName, hostName, players, date };
                Row row = new Row(agame, strings);
                rows.Add(row);
            }
        }

        public static void AddRow(string gameName, string hostName, string players, string date)
        {
            if(HostListenerEvent != null)
                HostListenerEvent(gameName, hostName, players, date);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.LightBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(title, new Vector2(25, 25), Color.White);
            spriteBatch.Draw(cadre, new Vector2(50, 150), Color.White);
            Vector2 headerPosition = new Vector2(119, 170);
            spriteBatch.Draw(header, headerPosition, Color.White);

            if (!boutonBack.IsMouseOver(mousePosition))
                spriteBatch.Draw(boutonBack.Texture, boutonBack.Position, Color.White);
            else
                spriteBatch.Draw(boutonBack.TextureOver, boutonBack.PositionOver, Color.White);
            if (!boutonJoin.IsMouseOver(mousePosition))
                spriteBatch.Draw(boutonJoin.Texture, boutonJoin.Position, Color.White);
            else
                spriteBatch.Draw(boutonJoin.TextureOver, boutonJoin.PositionOver, Color.White);

            foreach (Row row in rows)
            {
                if (!row.Bouton.IsMouseOver(mousePosition))
                {
                    if (!row.Bouton.Selected)
                        spriteBatch.Draw(row.Bouton.Texture, row.Bouton.Position, Color.White);
                    else
                        spriteBatch.Draw(row.Bouton.TextureSelected, row.Bouton.PositionOver, Color.White);
                }
                else
                {
                    if (!row.Bouton.Selected)
                        spriteBatch.Draw(row.Bouton.TextureOver, row.Bouton.Position, Color.White);
                    else
                        spriteBatch.Draw(row.Bouton.TextureSelectedOver, row.Bouton.PositionOver, Color.White);
                }
                for (int i = 0; i < row.HeaderStrings.Count(); i++)
                {
                    Vector2 position = row.Bouton.Position + new Vector2((row.Bouton.Texture.Width / row.HeaderStrings.Count() * i) + 10, 5);
                    spriteBatch.DrawString(font, row.HeaderStrings[i], position, Color.White);
                }
            }

            for (int i = 0; i < headerStrings.Count(); i++)
            {
                Vector2 position = headerPosition + new Vector2((header.Width / headerStrings.Count() * i) + 10, 7);
                spriteBatch.DrawString(font, headerStrings[i], position , Color.White);
            }

            spriteBatch.Draw(stringName, new Vector2(425, 530), Color.White);
            if (!inputName.Selected)
                spriteBatch.Draw(inputName.Texture, inputName.Position, Color.White);
            else
                spriteBatch.Draw(inputName.TextureOver, inputName.PositionOver, Color.White);
            spriteBatch.DrawString(inputName.Font, inputName.Text, inputName.TextPosition, Color.DarkCyan);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
