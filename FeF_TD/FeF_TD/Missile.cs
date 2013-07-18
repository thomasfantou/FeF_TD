using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FeF_TD
{
    public class Missile
    {
        #region Variables

        private Vector2 _position;
        private Texture2D _sprite;
        private Vector2 _velocity;
        private float _missileSpeed;
        private Mob _targetedMob;
        private Rectangle _rectangle;
        private bool _alive;


        #endregion

        #region Properties



        public bool Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }

        public Rectangle Rectangle
        {
            get 
            { 
                return new Rectangle((int)_position.X, (int)_position.Y, _sprite.Width, _sprite.Height); 
            }
        }

        public Mob TargetedMob
        {
            get { return _targetedMob; }
            set { _targetedMob = value; }
        }

        public Vector2 Center
        {
            get
            {
                if (Sprite != null)
                    return new Vector2(Position.X + (Sprite.Width / 2), Position.Y + (Sprite.Height / 2));
                else
                    return Vector2.Zero;
            }
        }

        public float MissileSpeed
        {
            get { return _missileSpeed; }
            set { _missileSpeed = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Texture2D Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        #endregion

        #region Methods

        public Missile(float speed, Vector2 position,string towerName, Mob mob)
        {
            _alive = true;
            _missileSpeed = speed;
            _position = position;
            _targetedMob = mob;

            switch (towerName)
            {
                case "aoe":
                    _sprite = Sprites.towerAoeBullet;
                    break;
                case "attaque":
                    _sprite = Sprites.towerAttaqueBullet;
                    break;
                case "freeze":
                    _sprite = Sprites.towerFreezeBullet;
                    break;
                case "poison":
                    _sprite = Sprites.towerPoisonBullet;
                    break;
                case "speed":
                    _sprite = Sprites.towerSpeedBullet;
                    break;
            }
        }

        public void Explode()
        {
            _alive = false;
        }

        public void Update()
        {
            if (_alive)
            {
                Vector2 v = new Vector2(_targetedMob.Center.X - Center.X, _targetedMob.Center.Y - Center.Y);
                float angle = (float)Math.Atan2(v.X, -v.Y);
                _velocity = new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
                _position += _velocity * _missileSpeed;
            }
        }

        #endregion
    }
}
