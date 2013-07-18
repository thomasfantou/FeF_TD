using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FeF_TD
{
    public abstract class GameScreen
    {

        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicsDevice;
        protected Enumeration.Screen screen;
        protected Vector2 mousePosition = new Vector2(-100, -100); //on ne veut pas voir le curseur en haut a gauche sur chaque premiere frame
        protected bool leftMouseClicked = false;
        protected bool rightMouseClicked = false;
        protected bool leftMouseHolding = false;
        public bool MouseEnabled { get; set; }

        private Texture2D supinfo;

        protected KeyboardState previousKeyboardState;
        protected MouseState previousMouseState;

        protected bool initDone = false;


        protected ClientGame cgame;


        Texture2D cursor;

        public GameScreen(GraphicsDeviceManager gdm, SpriteBatch sb, GraphicsDevice gd)
        {
            this.graphics = gdm;
            this.spriteBatch = sb;
            this.graphicsDevice = gd;
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            MouseEnabled = true;
        }

        #region Initialization


        public virtual void LoadContent(ContentManager content)
        {
            cursor = content.Load<Texture2D>(Config.IMG_SYS + "curseur");
            if(screen == Enumeration.Screen.Game)
                cgame.InitDone();

            supinfo = content.Load<Texture2D>(Config.IMG_SYS + "supinfo");
            
        }


        public virtual void UnloadContent()
        {

        }


        #endregion

        #region Update and Draw


        public virtual void Update(GameTime gameTime)
        {
            if (MouseEnabled)
                UpdateMouse();
        }


        protected void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

        }

        protected virtual void CheckMouseClick()
        {

            MouseState mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    leftMouseClicked = true;
                    leftMouseHolding = true;
                }
                else
                {
                    leftMouseClicked = false;
                    leftMouseHolding = false;
                }
            }
            previousMouseState = mouseState;
        }

        protected virtual void CheckMouseRightClick()
        {
            MouseState mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    rightMouseClicked = true;
                }
                else
                {
                    rightMouseClicked = false;
                }
            }
            previousMouseState = mouseState;
        }

        public virtual void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(cursor, new Vector2(mousePosition.X, mousePosition.Y), Color.White);
            spriteBatch.Draw(supinfo, new Vector2(480, 670), Color.White);
            spriteBatch.End();
        }



        #endregion


    }
}
