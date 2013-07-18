using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Datas;


/*
 *  Author Kighten
 */


namespace FeF_TD
{
    public class Mob
    {

        #region variables

        private int _id;
        private int _health;
        private int _baseHealth;
        private string _name;
        private Vector2 _position;
        private float _resistance;
        private float _slowPercentage;
        private float _slowRatio;
        private float _slowTimer;
        private float _speed;
        private Texture2D _sprite;
        private bool _stun;
        private float _stunTimer;
        private Vector2 _velocity;
        private int _gold;
        private bool _enabled;
        private Vector2 _ending;
        private Rectangle _spriteRect;
        private Direction _direction;
        private float _timerAnim;
        private bool _alive;
        private bool _selected;
        private Rectangle _rectangle;
        private const float _timerHitConst = 800;
        private float _timerHit;
        private bool _isHit;
        private float _timerFear;
        private bool _fearing;

        public delegate void MobDieHandler();
        public event MobDieHandler MobDie;


        public enum Direction
        {
            Up,
            Right,
            Left,
            Down
        }
        


        #endregion


        #region properties


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public float TimerFear
        {
            get { return _timerFear; }
            set { _timerFear = value; }
        }

        public int BaseHealth
        {
            get { return _baseHealth; }
            set { _baseHealth = value; }
        }

        public bool IsHit
        {
            get { return _isHit; }
            set { _isHit = value; }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _sprite.Width, _sprite.Height);
            }
        }

        public Vector2 Center
        {
            get 
            {
                if (Sprite != null)
                    return new Vector2(Position.X + (Sprite.Width / 3 / 2), Position.Y + (Sprite.Height / 2));
                else
                    return Vector2.Zero;
            }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public bool Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }

        public Direction Direction1
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Rectangle SpriteRect
        {
            get { return _spriteRect; }
            set { _spriteRect = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        public float SlowRatio
        {
            get { return _slowRatio; }
            set { _slowRatio = value; }
        }
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public float Resistance
        {
            get
            {
                return _resistance;
            }
            set
            {
                _resistance = value;
            }
        }

        public float SlowPercentage
        {
            get
            {
                return _slowPercentage;
            }
            set
            {
                _slowPercentage = value;
            }
        }

        public float SlowTimer
        {
            get
            {
                return _slowTimer;
            }
            set
            {
                _slowTimer = value;
            }
        }

        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public Texture2D Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                _sprite = value;
            }
        }

        public bool Stun
        {
            get
            {
                return _stun;
            }
            set
            {
                _stun = value;
            }
        }

        public float StunTimer
        {
            get
            {
                return _stunTimer;
            }
            set
            {
                _stunTimer = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }



        #endregion

        #region method

        public Mob()
        {
            _enabled = false;
            _spriteRect = new Rectangle(0, 0, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE);
            _timerAnim = Config.MOB_TIMER_ANIM;
            _alive = true;
            _selected = false;
            _timerHit = _timerHitConst;
            _isHit = false;
            _stun = false;
            _fearing = false;
        }

        public void Set(DataMob dm, Texture2D sprite)
        {
            _name = dm.name;
            _health = dm.health;
            _baseHealth = dm.health;
            _resistance = dm.resistance;
            _slowRatio = dm.slowRatio;
            _speed = dm.speed;
            _gold = dm.gold;

            _sprite = sprite;
        }

        public void HealthLost()
        {

        }

        public void Update(GameTime gameTime, Player player)
        {
            if (_enabled && _alive)
            {
                if (!_stun)
                {
                    _timerAnim -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_timerAnim <= 0)
                    {
                        _timerAnim = Config.MOB_TIMER_ANIM;
                        _spriteRect = new Rectangle(0, 0, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE);
                    }
                    else if (_timerAnim <= 200)
                        _spriteRect = new Rectangle(128, 0, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE);
                    else if (_timerAnim <= 400)
                        _spriteRect = new Rectangle(0, 0, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE);
                    else if (_timerAnim <= 600)
                        _spriteRect = new Rectangle(Config.APP_CELLS_SIZE, 0, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE);
                }

                if (_slowTimer >= 0)
                {
                    _slowTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (_position == Vector2.Zero)
                {
                    _position = FindBegining();
                    _ending = FindEnding();
                    _velocity = new Vector2(0, 1); //on va vers le bas par default
                    _direction = Direction.Down;
                }
                if (!Continue()) //Si on arrive a une intersection
                    RenewVelocity(); //on detecte ou le mob doit partir et on lui affecte sa velocity

                UpdatePosition();

                
                


                if (_position == _ending)
                {
                    _alive = false;
                    Wave.GetInstance().NbrMobLeft--;
                    player.Live--;
                    
                }

                if (_isHit)
                    _timerHit -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timerHit <= 0)
                {
                    _timerHit = _timerHitConst;
                    _isHit = false;
                }

                if (_slowTimer > 0)
                    _slowTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                else
                    _slowPercentage = 0;

                if (_stunTimer > 0)
                    _stunTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                else
                    _stun = false;

                if (_fearing)
                {
                    if (_timerFear > 0)
                        _timerFear -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    else
                        ChangeDirection();
                }
            }
        }

        private void UpdatePosition()
        {
            if (!_stun)
            {
                float slow = 1f;
                if (_slowPercentage != 0)
                    slow = (_slowPercentage / _slowRatio);

                if (_velocity.X > 0)
                {
                    if (!((_position.X % Config.APP_CELLS_SIZE) > (Config.APP_CELLS_SIZE - _speed * slow)))
                        _position += _velocity * _speed * slow;
                    else
                    {
                        _position.X += Config.APP_CELLS_SIZE - _position.X % Config.APP_CELLS_SIZE;
                    }
                }
                else if (_velocity.X < 0)
                {
                    if ((_position.X % Config.APP_CELLS_SIZE == 0.0f) || !(Config.APP_CELLS_SIZE - (_position.X % Config.APP_CELLS_SIZE) > (Config.APP_CELLS_SIZE - _speed * slow)))
                        _position += _velocity * _speed * slow;
                    else
                    {
                        _position.X -= _position.X % Config.APP_CELLS_SIZE;
                    }
                }
                else if (_velocity.Y > 0)
                {
                    if (!((_position.Y % Config.APP_CELLS_SIZE) > (Config.APP_CELLS_SIZE - _speed * slow)))
                        _position += _velocity * _speed * slow;
                    else
                    {
                        _position.Y += Config.APP_CELLS_SIZE - _position.Y % Config.APP_CELLS_SIZE;
                    }
                }
                else if (_velocity.Y < 0)
                {
                    if ((_position.Y % Config.APP_CELLS_SIZE == 0.0f) || !(Config.APP_CELLS_SIZE - (_position.Y % Config.APP_CELLS_SIZE) > (Config.APP_CELLS_SIZE - _speed * slow)))
                        _position += _velocity * _speed * slow;
                    else
                    {
                        _position.Y -= _position.Y % Config.APP_CELLS_SIZE;
                    }
                }

                //if (!((_position.X % 64) > (64 - _speed * slow)) && !((_position.Y % 64) > (64 - _speed * slow)))
                //    _position += _velocity * _speed * slow;
                //else
                //{
                //    if (_position.Y % 64 == 0)
                //        _position.X += 64 - _position.X % 64;
                //    else if (_position.X % 64 == 0)
                //        _position.Y += 64 - _position.Y % 64;
                //}
            }
        }

        private Vector2 FindBegining()
        {
            for (int i = 0; i < World.lenght; i++)
            {
                for (int j = 0; j < World.width; j++)
                {
                    if (World.map[j, i] == 1)
                    {
                        //si on a un chemin sur la premiere ligne de gauche ou du haut, c'est le point d'entré
                        if (i == 0 || j == 0)
                            return new Vector2(j * Config.APP_CELLS_SIZE, i * Config.APP_CELLS_SIZE);
                    }
                }
            }
            return Vector2.Zero;
        }

        private bool Continue()
        {
            if (_position.X % Config.APP_CELLS_SIZE == 0 && _position.Y % Config.APP_CELLS_SIZE == 0) //lorsqu'on on arrive pile sur une case
            {
                int x = 0;
                int y = 0;

                if (_velocity.X > 0)
                {
                    x = (int)(_position.X / Config.APP_CELLS_SIZE) + 1;
                    y = (int)(_position.Y / Config.APP_CELLS_SIZE);
                }
                else if (_velocity.X < 0)
                {
                    x = (int)(_position.X / Config.APP_CELLS_SIZE) - 1;
                    y = (int)(_position.Y / Config.APP_CELLS_SIZE);
                }
                else if (_velocity.Y > 0)
                {
                    x = (int)(_position.X / Config.APP_CELLS_SIZE);
                    y = (int)(_position.Y / Config.APP_CELLS_SIZE) + 1;
                }
                else if (_velocity.Y < 0)
                {
                    x = (int)(_position.X / Config.APP_CELLS_SIZE);
                    y = (int)(_position.Y / Config.APP_CELLS_SIZE) - 1;
                }

                try
                {
                    if (World.map[x, y] == 0)
                        return false;
                    else
                        return true;
                }
                catch (Exception e)
                {
                    return true;
                }

            }
            return true;
        }

        private void RenewVelocity()
        {
            bool changed = false;
            int x = (int) _position.X / Config.APP_CELLS_SIZE;
            int y = (int) _position.Y / Config.APP_CELLS_SIZE;

            if (World.map[x + 1, y] == 1 && !changed)
            {
                if (_velocity.X >= 0)
                {
                    _velocity = new Vector2(1, 0);
                    _direction = Direction.Right;
                    changed = true;
                }
            }
            if (World.map[x, y + 1] == 1 && !changed)
            {
                if (_velocity.Y >= 0)
                {
                    _velocity = new Vector2(0, 1);
                    _direction = Direction.Down;
                    changed = true;
                }
            }
            if (World.map[x - 1, y] == 1 && !changed)
            {
                if (_velocity.X <= 0)
                {
                    _velocity = new Vector2(-1, 0);
                    _direction = Direction.Left;
                    changed = true;
                }
            }
            if (World.map[x, y - 1] == 1 && !changed)
            {
                if (_velocity.Y <= 0)
                {
                    _velocity = new Vector2(0, -1);
                    _direction = Direction.Up;
                    changed = true;
                }
            }
        }

        private Vector2 FindEnding()
        {
            for (int i = 0; i < World.lenght; i++)
            {
                for (int j = 0; j < World.width; j++)
                {
                    if (World.map[j, i] == 1)
                    {
                        if (i == World.lenght - 1 || j == World.width - 1)
                            return new Vector2(j * Config.APP_CELLS_SIZE, i * Config.APP_CELLS_SIZE);
                    }
                }
            }
            return Vector2.Zero;
        }

        public bool GetHit(Tower tower)
        {
            if (this.Alive)
            {
                _isHit = true;
                _health -= tower.Damage;


                if (_health <= 0)
                {
                    Die();
                    return true;
                }
            }
            return false;
        }

        private void Die()
        {
            _alive = false;
            MobDie();
        }

        public void Fear()
        {
            _timerFear = 2000.0f;
            ChangeDirection();
        }

        private void ChangeDirection()
        {
            _velocity *= -1;
            if (_fearing)
                _fearing = false;
            else
                _fearing = true;

            switch (_direction)
            {
                case Direction.Right:
                    _direction = Direction.Left;
                    break;
                case Direction.Left:
                    _direction = Direction.Right;
                    break;
                case Direction.Up:
                    _direction = Direction.Down;
                    break;
                case Direction.Down:
                    _direction = Direction.Up;
                    break;
            }
        }

        #endregion
    }

}
