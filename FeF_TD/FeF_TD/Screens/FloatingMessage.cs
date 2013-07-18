using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FeF_TD
{
    public class FloatingMessage
    {
        private string _text;
        private Vector2 _position;
        private Color _color;
        private float _opacity;
        private float _ttl;
        private float _size;
        private Vector2 _direction;
        private float _speed;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Vector2 Direction1
        {
            get { return _direction; }
            set { _direction = value; }
        }


        public float Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public float Ttl
        {
            get { return _ttl; }
            set { _ttl = value; }
        }

        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        public Color Color
        {
            get { return _color * _opacity; }
            set { _color = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public FloatingMessage(String text, Vector2 position, Color color, Vector2 direction, float speed)
        {
            _text = text;
            _position = position;
            _color = color;
            _opacity = 1f;
            _size = 1f;
            _ttl = 1200f;
            _direction = direction;
            _speed = speed;
        }

        public bool Update()
        {
            _position += _direction * _speed;
            _opacity = _opacity - 0.015f;
            _size *=  1.02f;

            if (_opacity <= 0)
                return false;

            return true;
        }
    }
}
