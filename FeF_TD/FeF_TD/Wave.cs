using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datas;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FeF_TD
{
    public sealed class Wave
    {
        #region Variables

        private List<Mob> _mobs;
        static Wave instance = null;
        static readonly object padlock = new object();
        private float _timer;
        private Bouton _bouton;
        private bool _isActive;
        private int _nbrMob;
        private int _nbrMobLeft;
        private int _mobIndex;
        private float _popTimer;
        private int _currentWave;
        
        #endregion


        #region Properties

        public int NbrMobLeft
        {
            get { return _nbrMobLeft; }
            set { _nbrMobLeft = value; }
        }

        public int CurrentWave
        {
            get { return _currentWave; }
            set { _currentWave = value; }
        }

        public int NbrMob
        {
            get { return _nbrMob; }
            set { _nbrMob = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Bouton Bouton
        {
            get { return _bouton; }
            set { _bouton = value; }
        }

        public float Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public List<Mob> Mobs
        {
            get { return _mobs; }
            set { _mobs = value; }
        }

        #endregion


        #region Methods

        private Wave()
        {
            _mobs = new List<Mob>();
            _popTimer = Config.WAVE_POP_TIMER;
            _currentWave = 0;
        }

        public static Wave GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Wave();
                }
                return instance;
            }

        }

        public void Set(DataMob model, Texture2D sprite)
        {
            _mobIndex = 0;
            _nbrMob = Config.WAVE_MOB_COUNT;
            _nbrMobLeft = Config.WAVE_MOB_COUNT;
            Mobs.Clear();
            for (int i = 0; i < _nbrMob; i++)
            {
                Mob mob = new Mob();
                mob.Set(model, sprite);
                mob.Id = i + 1;
                mob.MobDie += new Mob.MobDieHandler(mob_MobDie);
                Mobs.Add(mob);
            }

            _timer = Config.WAVE_TIMER;
            _isActive = false;
            _currentWave++;

        }

        void mob_MobDie()
        {
            _nbrMobLeft--;
        }

        public void Init()
        {

        }

        public void Start()
        {

        }

        public bool Update(GameTime gameTime, Player player)
        {
            if (!_isActive)
            {
                _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_timer <= 0)
                {
                    _isActive = true;
                    _bouton.Enabled = false;
                }
            }
            else
            {
                _popTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_mobIndex < Config.WAVE_MOB_COUNT)
                {
                    if (_popTimer <= 0)
                    {
                        _popTimer = Config.WAVE_POP_TIMER;
                        _mobs.ElementAt(_mobIndex).Enabled = true;

                        _mobIndex++;
                    }
                }

                for (int i = 0; i < _mobIndex; i++)
                {
                    _mobs.ElementAt(i).Update(gameTime, player);
                }
                return CheckEndWave(); //on retourne true si c'est finit
            }

            return false;
        }

        public bool CheckEndWave()
        {
            bool isFinish = true;

            for (int i = 0; i < Config.WAVE_MOB_COUNT; i++)
            {
                if (_mobs.ElementAt(i).Alive == true)
                {
                    isFinish = false;
                    break;
                }
            }

            return isFinish;
        }

        public override string ToString()
        {
            if (!_isActive)
            {
                String[] myTimer = _timer.ToString().Split(',');
                String secTimer;
                if (myTimer[0].Count() == 4)
                    secTimer = _timer.ToString().Substring(0, 1);
                else if (myTimer[0].Count() > 4)
                    secTimer = _timer.ToString().Substring(0, 2);
                else
                    secTimer = "";

                return Config.GAME_SCREEN_WAVE_MSG + secTimer + '\n' + "Wave #" + _currentWave;
            }
            else
            {
                return Config.Game_SCREEN_WAVE_MSG2 + _nbrMobLeft + '\n' + "Wave #" + _currentWave;
            }
        }

        #endregion

    }
}
