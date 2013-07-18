using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FeF_TD
{
    class SGameCreation : GameScreen
    {
        #region Variables

        Texture2D background;
        Texture2D title;
        Texture2D cadre;
        Texture2D stringName;
        Texture2D stringGame;
        Texture2D stringPlayers;
        Dictionary<int,Texture2D> playerNumber;
        
        Bouton boutonBack;
        Bouton boutonCreate;
        Bouton boutonPlayer;

        List<InputForm> inputs;

        int playerNumberOption;
        

        #endregion

        public SGameCreation(GraphicsDeviceManager gdm, SpriteBatch sb, GraphicsDevice gd)
            : base(gdm, sb, gd)
        {
            this.screen = Enumeration.Screen.GameCreation;
        }

        public override void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>(Config.IMG_GAME + "background");
            title = content.Load<Texture2D>(Config.IMG_STRING + "createnewgametitle");
            cadre = content.Load<Texture2D>(Config.IMG_SYS + "cadre");
            stringName = content.Load<Texture2D>(Config.IMG_STRING + "playername");
            stringGame = content.Load<Texture2D>(Config.IMG_STRING + "gamename");
            stringPlayers = content.Load<Texture2D>(Config.IMG_STRING + "maximumplayer");

            boutonBack = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutonback"), new Vector2(130, 530),
                                        content.Load<Texture2D>(Config.IMG_SYS + "boutonbackover"), new Vector2(-63, -66));
            boutonCreate = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutoncreate"), new Vector2(680, 530),
                                        content.Load<Texture2D>(Config.IMG_SYS + "boutoncreateover"), new Vector2(-63, -65));

            inputs = new List<InputForm>();
            inputs.Add(new InputForm(content.Load<Texture2D>(Config.IMG_SYS + "champtexte"), new Vector2(585, 210),
                                        content.Load<Texture2D>(Config.IMG_SYS + "champtexteselected"), new Vector2(-69, -69),
                                        content));
            inputs.Add(new InputForm(content.Load<Texture2D>(Config.IMG_SYS + "champtexte"), new Vector2(585, 325),
                                        content.Load<Texture2D>(Config.IMG_SYS + "champtexteselected"), new Vector2(-69, -69),
                                        content));


            playerNumber = new Dictionary<int, Texture2D>();
            playerNumber.Add(2, content.Load<Texture2D>(Config.IMG_STRING + "2"));
            playerNumber.Add(4, content.Load<Texture2D>(Config.IMG_STRING + "4"));
            playerNumber.Add(6, content.Load<Texture2D>(Config.IMG_STRING + "6"));
            playerNumberOption = 2;
            boutonPlayer = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "lvlup"), new Vector2(750, 444),
                                        content.Load<Texture2D>(Config.IMG_SYS + "lvlupover"), Vector2.Zero);

            base.LoadContent(content);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            CheckMouseClick();
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] key;

            key = keyboardState.GetPressedKeys();

            if (key.Count() != 0 && (previousKeyboardState.IsKeyUp(key[0])))
            {
                foreach (InputForm input in inputs)
                {
                    if (input.Selected)
                    {
                        input.AddChar(key[0]);
                    }
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
                foreach (InputForm input in inputs)
                    input.Selected = false;

                if (boutonBack.IsMouseOver(mousePosition))
                    GameStateManagementGame.CallScreen(screen, Enumeration.Screen.MainMenu);
                else if (boutonCreate.IsMouseOver(mousePosition))
                {
                    if (inputs[0].Text.Equals(""))
                        inputs[0].Text = "NO NAME";
                    if (inputs[1].Text.Equals(""))
                        inputs[1].Text = "NO NAME";
                    GameStateManagementGame.StartServer(inputs[0].Text, inputs[1].Text, playerNumberOption);
                    GameStateManagementGame.CallScreen(screen, Enumeration.Screen.PlayerRoom);
                    GlobalAppVariables.PlayerName = inputs[0].Text;
                    Client.ConnectAsHost(inputs[0].Text, inputs[1].Text);
                }
                else if (boutonPlayer.IsMouseOver(mousePosition))
                {
                    if (playerNumberOption == 6)
                        playerNumberOption = 2;
                    else
                        playerNumberOption += 2;
                }

                foreach (InputForm input in inputs)
                {
                    if (input.IsMouseOver(mousePosition))
                        input.Selected = true;
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
            spriteBatch.Draw(cadre, new Vector2(50,150), Color.White);
            spriteBatch.Draw(stringName, new Vector2(235, 220), Color.White);
            spriteBatch.Draw(stringGame, new Vector2(245, 335), Color.White);
            spriteBatch.Draw(stringPlayers, new Vector2(210, 450), Color.White);
            if(!boutonBack.IsMouseOver(mousePosition))
                spriteBatch.Draw(boutonBack.Texture, boutonBack.Position, Color.White);
            else
                spriteBatch.Draw(boutonBack.TextureOver, boutonBack.PositionOver, Color.White);
            if (!boutonCreate.IsMouseOver(mousePosition))
                spriteBatch.Draw(boutonCreate.Texture, boutonCreate.Position, Color.White);
            else
                spriteBatch.Draw(boutonCreate.TextureOver, boutonCreate.PositionOver, Color.White);

            foreach (InputForm input in inputs)
            {
                if (!input.Selected)
                    spriteBatch.Draw(input.Texture, input.Position, Color.White);
                else
                    spriteBatch.Draw(input.TextureOver, input.PositionOver, Color.White);

                spriteBatch.DrawString(input.Font, input.Text, input.TextPosition, Color.DarkCyan);

            }

            foreach (KeyValuePair<int, Texture2D> kvp in playerNumber)
            {
                if (kvp.Key == playerNumberOption)
                    spriteBatch.Draw(kvp.Value, new Vector2(680, 440), Color.White);
            }

            if (!boutonPlayer.IsMouseOver(mousePosition))
                spriteBatch.Draw(boutonPlayer.Texture, boutonPlayer.Position, Color.White);
            else
                spriteBatch.Draw(boutonPlayer.TextureOver, boutonPlayer.PositionOver, Color.White);
            


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
