using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FeF_TD
{
    public class CursorSelection
    {
        private Vector2 _position;
        private Texture2D _sprite;
        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public Texture2D Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public CursorSelection()
        {
            _selected = false;
        }

        public void Set(Vector2 p)
        {
            _position.X = p.X - ((_sprite.Width-Config.APP_CELLS_SIZE) / 2);
            _position.Y = p.Y - ((_sprite.Height-Config.APP_CELLS_SIZE) / 2);
        }
    }
}
