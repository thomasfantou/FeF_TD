using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace FeF_TD
{
    class InputForm : Bouton
    {
        private String text;

        public String Text
        {
            get { return text; }
            set { text = value; }
        }
        private SpriteFont font;

        private Vector2 textPosition;

        public Vector2 TextPosition
        {
            get { return textPosition; }
            set { textPosition = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public InputForm(Texture2D t, Vector2 p, Texture2D to, Vector2 po, ContentManager content) : base (t, p, to, po)
        {
            selected = false;
            this.font = content.Load<SpriteFont>(Config.FONT + "InputFont");
            this.textPosition = this.Position + new Vector2(10, 10);
            this.text = "";
        }

        public void AddChar(Keys key)
        {
            int ascii = key.GetHashCode();
            if (text.Count() <= 19)
            {
                if (key == Keys.Back)
                {
                    if (text.Length > 0)
                        text = text.Remove(text.Length - 1, 1);
                }
                else if (key == Keys.Space)
                    text += " ";
                if (ascii >= 65 && ascii <= 90)
                    text += key.ToString();
                else if (ascii >= 48 && ascii <= 57) //numbers
                    text += key.ToString().Substring(1, 1);

                
            }
        }
    }
}
