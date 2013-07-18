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


namespace FeF_TD
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameComponent1 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public static SpriteBatch spriteBatch;


        public GameComponent1(Game game)
            : base(game)
        {
        }



        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            SpriteFont font = Game.Content.Load<SpriteFont>(Config.FONT + "font3");


            spriteBatch.DrawString(font, "salut", new Vector2(20, 20), Color.White);
            

            spriteBatch.End();
        }
    }
}
