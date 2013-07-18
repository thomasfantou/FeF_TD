using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace FeF_TD
{
    class SMainMenu : GameScreen
    {
        #region Variables

        Bouton bouton1;
        Bouton bouton2;
        Bouton bouton3;


        Texture2D background;
        Texture2D icone;



        enum SelectedOption { Create, Join, Exit };
        SelectedOption current;

        #endregion

        public SMainMenu(GraphicsDeviceManager gdm, SpriteBatch sb, GraphicsDevice gd)
            : base(gdm, sb, gd)
        {
            this.screen = Enumeration.Screen.MainMenu;
        }

        public override void LoadContent(ContentManager content)
        {

            current = SelectedOption.Create;

            bouton1 = new Bouton(content.Load<Texture2D>(Config.IMG_STRING + "createnewgame"), new Vector2(400, 550),
                                    content.Load<Texture2D>(Config.IMG_STRING + "createnewgame2"), new Vector2(2, 2));
            bouton2 = new Bouton(content.Load<Texture2D>(Config.IMG_STRING + "joinagame"), new Vector2(400, 600),
                                    content.Load<Texture2D>(Config.IMG_STRING + "joinagame2"), new Vector2(2, 2));
            bouton3 = new Bouton(content.Load<Texture2D>(Config.IMG_STRING + "exit"), new Vector2(400, 650),
                                    content.Load<Texture2D>(Config.IMG_STRING + "exit2"), new Vector2(2, 2));

            background = content.Load<Texture2D>(Config.IMG_GAME + "FEF_TD_background");
            icone = content.Load<Texture2D>(Config.IMG_SYS + "Life");


            base.LoadContent(content);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            CheckMouseClick();

            CheckOnMouseOver();


            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (current != SelectedOption.Exit)
                    current++;
            }
            if (keyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (current != SelectedOption.Create)
                    current--;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                switch (current)
                {
                    case SelectedOption.Create:
                        GameStateManagementGame.CallScreen(screen, Enumeration.Screen.GameCreation);
                        break;
                    case SelectedOption.Join:
                        GameStateManagementGame.CallScreen(screen, Enumeration.Screen.GameList);
                        break;
                    case SelectedOption.Exit:
                        Program.ExitGame();
                        break;
                }
            }


            previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        void CheckOnMouseOver()
        {

            if (bouton1.IsMouseOver(mousePosition))
                current = SelectedOption.Create;
            else if (bouton2.IsMouseOver(mousePosition))
                current = SelectedOption.Join;
            else if (bouton3.IsMouseOver(mousePosition))
                current = SelectedOption.Exit;

        }

        protected override void CheckMouseClick()
        {
            base.CheckMouseClick();

            if (leftMouseClicked)
            {
                if (bouton1.IsMouseOver(mousePosition))
                    GameStateManagementGame.CallScreen(screen, Enumeration.Screen.GameCreation);
                else if (bouton2.IsMouseOver(mousePosition))
                    GameStateManagementGame.CallScreen(screen, Enumeration.Screen.GameList);
                else if (bouton3.IsMouseOver(mousePosition))
                    Program.ExitGame();
                leftMouseClicked = false;
            }
        }



        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.LightBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(bouton1.Texture, bouton1.Position, Color.White);
            spriteBatch.Draw(bouton2.Texture, bouton2.Position, Color.White);
            spriteBatch.Draw(bouton3.Texture, bouton3.Position, Color.White);

            switch (current)
            {
                case SelectedOption.Create:
                    spriteBatch.Draw(bouton1.TextureOver, bouton1.PositionOver, Color.White);
                    spriteBatch.Draw(icone, bouton1.Position - new Vector2(40, 2), Color.White);
                    break;
                case SelectedOption.Join:
                    spriteBatch.Draw(bouton2.TextureOver, bouton2.PositionOver, Color.White);
                    spriteBatch.Draw(icone, bouton2.Position - new Vector2(40, 2), Color.White);
                    break;
                case SelectedOption.Exit:
                    spriteBatch.Draw(bouton3.TextureOver, bouton3.PositionOver, Color.White);
                    spriteBatch.Draw(icone, bouton3.Position - new Vector2(40, 2), Color.White);
                    break;
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }


    }
}
