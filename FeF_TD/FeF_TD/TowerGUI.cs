using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datas;
using Microsoft.Xna.Framework;

namespace FeF_TD
{
    public class TowerGUI
    {
        private Tower _tower;
        private string _name;
        private int _damage;
        private int _price;
        private bool _enabled;
        private float _attaqueSpeed;
        private string _comment;
        private int _damageNext;
        private float _attaqueSpeedNext;
        private int _level;
        private Bouton _towerLvlUp;
        private int _priceNext;
        private float _range;
        private Vector2 _target;

        public Tower Tower
        {
            get { return _tower; }
            set { _tower = value; }
        }

        public Vector2 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public float Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public int PriceNext
        {
            get { return _priceNext; }
            set { _priceNext = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public Bouton TowerLvlUp
        {
            get { return _towerLvlUp; }
            set { _towerLvlUp = value; }
        }

        public int Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public float AttaqueSpeedNext
        {
            get { return _attaqueSpeedNext; }
            set { _attaqueSpeedNext = value; }
        }

        public int DamageNext
        {
            get { return _damageNext; }
            set { _damageNext = value; }
        }

        public string Comment
        {
            get { return _comment.Replace("\\n", ""); }
            set { _comment = value; }
        }

        public float AttaqueSpeed
        {
            get { return _attaqueSpeed; }
            set { _attaqueSpeed = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public string Name
        {
            get { 
                
                return _name.Substring(0,1).ToUpper() + _name.Substring(1, _name.Length - 2); 
            }
            set { _name = value; }
        }

        public TowerGUI()
        {
            _enabled = false;

        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void Set(Tower tower)
        {
            _tower = tower;
            _name = tower.Name;
            _damage = tower.Damage;
            _attaqueSpeed = tower.AttackSpeed;
            _comment = tower.Comment;
            _level = int.Parse(_name.Substring(_name.Length - 1, 1));
            _range = tower.Range;

            if (_level < 6)
            {

                switch (tower.Name.Substring(0, tower.Name.Length - 1))
                {
                    case "aoe":
                        _damageNext = SGame.aoeTowers[_level].damage;
                        _attaqueSpeedNext = SGame.aoeTowers[_level].attaqueSpeed;
                        _priceNext = SGame.aoeTowers[_level].price;
                        break;
                    case "attaque":
                        _damageNext = SGame.attaqueTowers[_level].damage;
                        _attaqueSpeedNext = SGame.attaqueTowers[_level].attaqueSpeed;
                        _priceNext = SGame.attaqueTowers[_level].price;
                        break;
                    case "freeze":
                        _damageNext = SGame.freezeTowers[_level].damage;
                        _attaqueSpeedNext = SGame.freezeTowers[_level].attaqueSpeed;
                        _priceNext = SGame.freezeTowers[_level].price;
                        break;
                    case "poison":
                        _damageNext = SGame.poisonTowers[_level].damage;
                        _attaqueSpeedNext = SGame.poisonTowers[_level].attaqueSpeed;
                        _priceNext = SGame.poisonTowers[_level].price;
                        break;
                    case "speed":
                        _damageNext = SGame.speedTowers[_level].damage;
                        _attaqueSpeedNext = SGame.speedTowers[_level].attaqueSpeed;
                        _priceNext = SGame.speedTowers[_level].price;
                        break;
                }
            }

            

        }

        public void Update()
        {
            if(_tower.TargetedMob != null)
                _target = new Vector2(_tower.TargetedMob.Position.X + Config.APP_CELLS_SIZE, _tower.TargetedMob.Position.Y + Config.APP_CELLS_SIZE);
        }
    }
}
