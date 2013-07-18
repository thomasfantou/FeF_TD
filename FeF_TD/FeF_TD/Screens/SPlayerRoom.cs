using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FeF_TD
{
    public class SPlayerRoom : GameScreen
    {
        Texture2D background;
        Texture2D title;
        Texture2D cadre;
        Texture2D header;

        Texture2D gameName;
        Texture2D hostName;
        Texture2D players;

        Texture2D gamelistrow;
        Texture2D gamelistrowup;

        Bouton boutonBack;
        Bouton boutonJoin;

        SpriteFont font;

        float timerCheckoutClients;

        List<Row> rows;
        Texture2D boutonRow;
        Texture2D boutonRowUp;

        ClientPlayerRoom cpr;

        bool isHost;


        public SPlayerRoom(GraphicsDeviceManager gdm, SpriteBatch sb, GraphicsDevice gd)
            : base(gdm, sb, gd)
        {
            this.screen = Enumeration.Screen.PlayerRoom;
            timerCheckoutClients = 1000;
            cpr = ClientPlayerRoom.GetInstance();
            if (Server.IsStarted)
            {
                Server.StartPlayerRoomServer();
                isHost = true;
            }
            else
            {
                isHost = false;
            }

            cpr.LaunchGame += new ClientPlayerRoom.LaunchGameHandler(cpr_LaunchGame);
        }

        void cpr_LaunchGame()
        {
            GameStateManagementGame.CallScreen(screen, Enumeration.Screen.Game);
        }

        public override void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>(Config.IMG_GAME + "background");
            title = content.Load<Texture2D>(Config.IMG_STRING + "playerroom");
            cadre = content.Load<Texture2D>(Config.IMG_SYS + "cadre");
            header = content.Load<Texture2D>(Config.IMG_SYS + "gamelistheader");
            gameName = content.Load<Texture2D>(Config.IMG_STRING + "gamename");
            hostName = content.Load<Texture2D>(Config.IMG_STRING + "hostname");
            players = content.Load<Texture2D>(Config.IMG_STRING + "players");

            boutonBack = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutonback"), new Vector2(130, 550),
                                        content.Load<Texture2D>(Config.IMG_SYS + "boutonbackover"), new Vector2(-63, -66));
            boutonJoin = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutonjoin"), new Vector2(680, 550),
                                        content.Load<Texture2D>(Config.IMG_SYS + "boutonjoinover"), new Vector2(-59, -64));

            boutonJoin.Enabled = false;

            gamelistrow = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrow");
            gamelistrowup = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrowup");

            font = content.Load<SpriteFont>(Config.FONT + "GamesListFont");

            boutonRow = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrow");
            boutonRowUp = content.Load<Texture2D>(Config.IMG_SYS + "gamelistrowup");

            rows = new List<Row>();
            for (int i = 0; i < cpr.PlayerCount; i++)
            {
                Bouton agame = new Bouton(boutonRow, new Vector2(119, 280 + (i * 45)), boutonRowUp, new Vector2(-10, -10));
                String[] strings = { "" };
                Row row = new Row(agame, strings);
                rows.Add(row);
            }

            //rows.ElementAt(0).HeaderStrings[0] = Server.HostName;
            

            base.LoadContent(content);
        }


        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            CheckMouseClick();
            timerCheckoutClients -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (rows.Count != cpr.PlayerCount)
            {
                rows.Clear();
                for (int i = 0; i < cpr.PlayerCount; i++)
                {
                    Bouton agame = new Bouton(boutonRow, new Vector2(119, 280 + (i * 45)), boutonRowUp, new Vector2(-10, -10));
                    String[] strings = { "" };
                    Row row = new Row(agame, strings);
                    rows.Add(row);
                }
            }

            if (timerCheckoutClients <= 0)
            {
                //List<User> clients = Server.GetPlayers();
                int min = rows.Count;
                if (min > cpr.PlayersName.Count)
                    min = cpr.PlayersName.Count;

                for (int i = 0; i < min; i++)
                {
                    rows[i].HeaderStrings = new String[] { cpr.PlayersName[i] };


                }
                timerCheckoutClients = 1000;
            }
            base.Update(gameTime);

            if (cpr.PlayersName != null && cpr.PlayersName.Count > 0 && cpr.PlayersName.ElementAt(0) != "")
                if (isHost)
                    boutonJoin.Enabled = true;
        }

        protected override void CheckMouseClick()
        {
            base.CheckMouseClick();

            if (leftMouseClicked)
            {
                if (boutonBack.IsMouseOver(mousePosition))
                {
                    if (isHost)
                    {
                        GameStateManagementGame.StopServer();
                        GameStateManagementGame.CallScreen(screen, Enumeration.Screen.GameCreation);
                    }
                    else
                    {
                        GameStateManagementGame.CallScreen(screen, Enumeration.Screen.GameList);
                    }
                }
                else if (boutonJoin.IsMouseOver(mousePosition))
                {
                    if (isHost)
                    {
                        Server.LaunchGame();
                    }
                }
                leftMouseClicked = false;
            }
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

            spriteBatch.Draw(gameName, headerPosition + new Vector2(5, 6), Color.White);
            spriteBatch.Draw(hostName, headerPosition + new Vector2(410, 6), Color.White);
            spriteBatch.Draw(players, headerPosition + new Vector2(15, 60), Color.White);

            spriteBatch.DrawString(font, cpr.GameName, headerPosition + new Vector2(170, 8), Color.White);
            spriteBatch.DrawString(font, cpr.HostName, headerPosition + new Vector2(590, 8), Color.White);

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
                    if (row.HeaderStrings[i] != null)
                    {
                        Vector2 position = row.Bouton.Position + new Vector2((row.Bouton.Texture.Width / row.HeaderStrings.Count() * i) + 10, 5);
                        spriteBatch.DrawString(font, row.HeaderStrings[i], position, Color.White);
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
