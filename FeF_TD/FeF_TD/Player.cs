using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 *  author tfantou
 */

namespace FeF_TD
{
    public class Player
    {
        #region Variables

        private int _gold;
        private int _kill;
        private int _live;
        private string _name;
        private int _baseLevel;
        private Tower _focusedTower;
        private List<Tower> _towers;
        private int _towerIndex;

        public delegate void GameLostHandler();
        public event GameLostHandler GameLost;

        #endregion

        #region Properties



        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        public int Kill
        {
            get { return _kill; }
            set { _kill = value; }
        }

        public int Live
        {
            get { return _live; }
            set { _live = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int BaseLevel
        {
            get { return _baseLevel; }
            set { _baseLevel = value; }
        }

        public Tower FocusedTower
        {
            get { return _focusedTower; }
            set { _focusedTower = value; }
        }

        public List<Tower> Towers
        {
            get { return _towers; }
            set { _towers = value; }
        }

        #endregion

        #region Methods

        public Player(String name)
        {
            _name = name;
            _towers = new List<Tower>();
            _gold = Config.GAME_PLAYER_INITIAL_GOLD;
            _kill = 0;
            _live = Config.GAME_PLAYER_INITIAL_LIVES;
            _baseLevel = 1;
            _towerIndex = 0;


        }

        public bool BaseUpgrade()
        {
            if (_gold >= Base.GetInstance().BasePrice[_baseLevel - 1])
            {
                _gold -= Base.GetInstance().BasePrice[_baseLevel - 1];
                _baseLevel++;
                return true;
            }
            else
                return false;
        }

        public bool Buy(Tower t)
        {
            if (_gold >= t.Price)
            {
                _towers.Add(t);
                _gold -= t.Price;
                t.Id = ++_towerIndex;
                return true;
            }
            else
                return false;
        }

        public bool Buy(int price)
        {
            if (_gold >= price)
            {
                _gold -= price;
                return true;
            }
            else
                return false;
        }

        public void Sell()
        {

        }

        public void Update()
        {
        }

        public bool CheckPlayer()
        {
            if (_live <= 0)
            {
                _gold = 0;
                _towers.Clear();
                return false;
            }
            return true;
        }

        public void AddTower(Tower t)
        {
            _towers.Add(t);
        }


        #endregion

    }
}
