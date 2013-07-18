using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Datas;

namespace FeF_TD
{
    public class Tower
    {
        #region Variables

        private int _id;
        //private Texture2D _sprite;
        private Vector2 _position;
        private float _attackSpeed;
        private int _damage;
        private float _fearChance;
        private List<Missile> _missiles;
        private float _missileSpeed;
        private string _name;
        private int _price;
        private float _range;
        private float _slowPercentage;
        private float _slowTime;
        private bool _splashEffect;
        private const float _splashArea = 150;
        private List<Mob> _mobArea; //les mob qui seront affecté par l'AoE
        private float _stunChance;
        private float _stunTime;
        private Mob _targetedMob;
        private Mob _manualTargetedMob; //si le joueur cible un enemie spécifique
        private string _comment;
        private int _level;
        private float _rotate;
        private Rectangle _rect;
        private float _timerMissile;
        private List<Mob> _mobs;
        private Enumeration.BuildingState _towerState;
        private bool _readyToFire;
        

        #endregion

        #region Properties



        public bool ReadyToFire
        {
            get { return _readyToFire; }
            set { _readyToFire = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public float TimerMissile
        {
            get { return _timerMissile; }
            set { _timerMissile = value; }
        }

        public float MissileSpeed
        {
            get { return _missileSpeed; }
            set { _missileSpeed = value; }
        }

        public Enumeration.BuildingState TowerState
        {
            get { return _towerState; }
            set { _towerState = value; }
        }
        
        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X + (Config.APP_CELLS_SIZE / 2), Position.Y + (Config.APP_CELLS_SIZE / 2));
            }
        }

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public float Rotate
        {
            get { return _rotate; }
            set { _rotate = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        //public Texture2D Sprite
        //{
        //    get { return _sprite; }
        //    set { _sprite = value; }
        //}

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float AttackSpeed
        {
            get { return _attackSpeed; }
            set { _attackSpeed = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public float FearChance
        {
            get { return _fearChance; }
            set { _fearChance = value; }
        }

        public List<Missile> Missiles
        {
            get { return _missiles; }
            set { _missiles = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public float Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public float SlowPercentage
        {
            get { return _slowPercentage; }
            set { _slowPercentage = value; }
        }

        public float SlowTime
        {
            get { return _slowTime; }
            set { _slowTime = value; }
        }

        public bool SplashEffect
        {
            get { return _splashEffect; }
            set { _splashEffect = value; }
        }

        private List<Mob> MobArea
        {
            get { return _mobArea; }
            set { _mobArea = value; }
        }

        public float StunChance
        {
            get { return _stunChance; }
            set { _stunChance = value; }
        }

        public float StunTime
        {
            get { return _stunTime; }
            set { _stunTime = value; }
        }

        public Mob TargetedMob
        {
            get { return _targetedMob; }
            set { _targetedMob = value; }
        }

        public Mob ManualTargetedMob
        {
            get { return _manualTargetedMob; }
            set { _manualTargetedMob = value; }
        }

        #endregion

        #region Methods

        public Tower()
        {
            _rotate = 0;
            _rect = new Rectangle(0, 0, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE);
            _mobArea = new List<Mob>();
            _missiles = new List<Missile>();
            _readyToFire = false;
        }

        public void Create()
        {

        }

        public void Delete()
        {

        }

        public void Upgrade()
        {

        }

        public void Set(DataTower dt, Enumeration.BuildingState bs)
        {
            _attackSpeed = dt.attaqueSpeed;
            _damage = dt.damage;
            _fearChance = dt.fearChance;
            _missileSpeed = dt.missileSpeed;
            _name = dt.name;
            _price = dt.price;
            _range = dt.range;
            _slowPercentage = dt.slowPercentage;
            _slowTime = dt.slowTime;
            _splashEffect = dt.splachEffect;
            _stunChance = dt.stunChance;
            _stunTime = dt.stunTime;
            _comment = dt.comment;
            _level = int.Parse(dt.name.Substring(dt.name.Length - 1, 1));
            _timerMissile = _attackSpeed * 1000;

            _towerState = bs;

        }

        public void LevelUp()
        {
            switch (_name.Substring(0, _name.Length - 1))
            {
                case "aoe":
                    _attackSpeed = SGame.aoeTowers[_level].attaqueSpeed;
                    _damage = SGame.aoeTowers[_level].damage;
                    _fearChance = SGame.aoeTowers[_level].fearChance;
                    _missileSpeed = SGame.aoeTowers[_level].missileSpeed;
                    _name = SGame.aoeTowers[_level].name;
                    _price = SGame.aoeTowers[_level].price;
                    _range = SGame.aoeTowers[_level].range;
                    _slowPercentage = SGame.aoeTowers[_level].slowPercentage;
                    _slowTime = SGame.aoeTowers[_level].slowTime;
                    _splashEffect = SGame.aoeTowers[_level].splachEffect;
                    _stunChance = SGame.aoeTowers[_level].stunChance;
                    _stunTime = SGame.aoeTowers[_level].stunTime;
                    _comment = SGame.aoeTowers[_level].comment;
                    _towerState = Enumeration.BuildingState.Aoe;
                    break;
                case "attaque":
                    _attackSpeed = SGame.attaqueTowers[_level].attaqueSpeed;
                    _damage = SGame.attaqueTowers[_level].damage;
                    _fearChance = SGame.attaqueTowers[_level].fearChance;
                    _missileSpeed = SGame.attaqueTowers[_level].missileSpeed;
                    _name = SGame.attaqueTowers[_level].name;
                    _price = SGame.attaqueTowers[_level].price;
                    _range = SGame.attaqueTowers[_level].range;
                    _slowPercentage = SGame.attaqueTowers[_level].slowPercentage;
                    _slowTime = SGame.attaqueTowers[_level].slowTime;
                    _splashEffect = SGame.attaqueTowers[_level].splachEffect;
                    _stunChance = SGame.attaqueTowers[_level].stunChance;
                    _stunTime = SGame.attaqueTowers[_level].stunTime;
                    _comment = SGame.attaqueTowers[_level].comment;
                    _towerState = Enumeration.BuildingState.Attaque;
                    break;
                case "freeze":
                    _attackSpeed = SGame.freezeTowers[_level].attaqueSpeed;
                    _damage = SGame.freezeTowers[_level].damage;
                    _fearChance = SGame.freezeTowers[_level].fearChance;
                    _missileSpeed = SGame.freezeTowers[_level].missileSpeed;
                    _name = SGame.freezeTowers[_level].name;
                    _price = SGame.freezeTowers[_level].price;
                    _range = SGame.freezeTowers[_level].range;
                    _slowPercentage = SGame.freezeTowers[_level].slowPercentage;
                    _slowTime = SGame.freezeTowers[_level].slowTime;
                    _splashEffect = SGame.freezeTowers[_level].splachEffect;
                    _stunChance = SGame.freezeTowers[_level].stunChance;
                    _stunTime = SGame.freezeTowers[_level].stunTime;
                    _comment = SGame.freezeTowers[_level].comment;
                    _towerState = Enumeration.BuildingState.Freeze;
                    break;
                case "poison":
                    _attackSpeed = SGame.poisonTowers[_level].attaqueSpeed;
                    _damage = SGame.poisonTowers[_level].damage;
                    _fearChance = SGame.poisonTowers[_level].fearChance;
                    _missileSpeed = SGame.poisonTowers[_level].missileSpeed;
                    _name = SGame.poisonTowers[_level].name;
                    _price = SGame.poisonTowers[_level].price;
                    _range = SGame.poisonTowers[_level].range;
                    _slowPercentage = SGame.poisonTowers[_level].slowPercentage;
                    _slowTime = SGame.poisonTowers[_level].slowTime;
                    _splashEffect = SGame.poisonTowers[_level].splachEffect;
                    _stunChance = SGame.poisonTowers[_level].stunChance;
                    _stunTime = SGame.poisonTowers[_level].stunTime;
                    _comment = SGame.poisonTowers[_level].comment;
                    _towerState = Enumeration.BuildingState.Poison;
                    break;
                case "speed":
                    _attackSpeed = SGame.speedTowers[_level].attaqueSpeed;
                    _damage = SGame.speedTowers[_level].damage;
                    _fearChance = SGame.speedTowers[_level].fearChance;
                    _missileSpeed = SGame.speedTowers[_level].missileSpeed;
                    _name = SGame.speedTowers[_level].name;
                    _price = SGame.speedTowers[_level].price;
                    _range = SGame.speedTowers[_level].range;
                    _slowPercentage = SGame.speedTowers[_level].slowPercentage;
                    _slowTime = SGame.speedTowers[_level].slowTime;
                    _splashEffect = SGame.speedTowers[_level].splachEffect;
                    _stunChance = SGame.speedTowers[_level].stunChance;
                    _stunTime = SGame.speedTowers[_level].stunTime;
                    _comment = SGame.speedTowers[_level].comment;
                    _towerState = Enumeration.BuildingState.Speed;
                    break;
            }
            _level++;



        }

        public void FindTarget(List<Mob> mobs)
        {
            _mobs = mobs;
            if (this.ManualTargetedMob == null || !this.ManualTargetedMob.Alive)
            {
                if (this.TargetedMob == null || !this.TargetedMob.Alive)
                {
                    this.TargetedMob = null;
                    this.ManualTargetedMob = null;

                    Vector2 towerCenter = new Vector2(Position.X + (Config.APP_CELLS_SIZE / 2), Position.Y + (Config.APP_CELLS_SIZE / 2));

                    foreach (Mob mob in mobs)
                    {
                        if (mob.Enabled && mob.Alive)
                        {
                            Vector2 mobCenter = new Vector2(mob.Position.X + ((mob.Sprite.Width / 3) / 2), mob.Position.Y + (mob.Sprite.Height / 2));
                            double hypothenuse = Math.Pow(Math.Abs(towerCenter.X - mobCenter.X), 2) + Math.Pow(Math.Abs(towerCenter.Y - mobCenter.Y), 2); //Th pythagore
                            if (this.Range >= Math.Sqrt(hypothenuse))
                            {
                                this.TargetedMob = mob;
                                break;
                            }
                            else
                            {
                                this.TargetedMob = null; //Lorsqu'on change de cible, c'est que le mob est mort, si on en trouve pas d'autre a target, c qu'il n'y en a plus
                            }
                        }
                    }
                }
                else
                {
                    Vector2 towerCenter = new Vector2(Position.X + (Config.APP_CELLS_SIZE / 2), Position.Y + (Config.APP_CELLS_SIZE / 2));
                    Vector2 mobCenter = new Vector2(this.TargetedMob.Position.X + ((this.TargetedMob.Sprite.Width / 3) / 2), this.TargetedMob.Position.Y + (this.TargetedMob.Sprite.Height / 2));
                    double hypothenuse = Math.Pow(Math.Abs(towerCenter.X - mobCenter.X), 2) + Math.Pow(Math.Abs(towerCenter.Y - mobCenter.Y), 2); //Th pythagore
                    if (this.Range <= Math.Sqrt(hypothenuse) || this.TargetedMob.Alive == false) //si le mob sort du range
                    {
                        this.TargetedMob = null;
                    }
                }
            }
            else
            {
                Vector2 towerCenter = new Vector2(Position.X + (Config.APP_CELLS_SIZE / 2), Position.Y + (Config.APP_CELLS_SIZE / 2));
                Vector2 mobCenter = new Vector2(this.ManualTargetedMob.Position.X + ((this.ManualTargetedMob.Sprite.Width / 3) / 2), this.ManualTargetedMob.Position.Y + (this.ManualTargetedMob.Sprite.Height / 2));
                double hypothenuse = Math.Pow(Math.Abs(towerCenter.X - mobCenter.X), 2) + Math.Pow(Math.Abs(towerCenter.Y - mobCenter.Y), 2); //Th pythagore
                if (this.Range <= Math.Sqrt(hypothenuse)) //si le mob sort du range
                {
                    this.ManualTargetedMob = null;
                }
            }

        }

        private void FindSplashAreaMob()
        {
            _mobArea.Clear();

            if (ManualTargetedMob != null)
            {
                foreach (Mob mob in _mobs)
                {
                    Vector2 targetedMobCenter = new Vector2(this.ManualTargetedMob.Position.X + ((this.ManualTargetedMob.Sprite.Width / 3) / 2),
                        this.ManualTargetedMob.Position.Y + (this.ManualTargetedMob.Sprite.Height / 2));
                    Vector2 mobCenter = new Vector2(mob.Position.X + ((mob.Sprite.Width / 3) / 2), mob.Position.Y + (mob.Sprite.Height / 2));
                    double hypothenuse = Math.Pow(Math.Abs(targetedMobCenter.X - mobCenter.X), 2) + Math.Pow(Math.Abs(targetedMobCenter.Y - mobCenter.Y), 2); //Th pythagore
                    if (_splashArea >= Math.Sqrt(hypothenuse)) //si le mob est dans le range du splash
                    {
                        _mobArea.Add(mob);
                    }
                }
            }
            else if (TargetedMob != null)
            {
                foreach (Mob mob in _mobs)
                {
                    Vector2 targetedMobCenter = new Vector2(this.TargetedMob.Position.X + ((this.TargetedMob.Sprite.Width / 3) / 2),
                        this.TargetedMob.Position.Y + (this.TargetedMob.Sprite.Height / 2));
                    Vector2 mobCenter = new Vector2(mob.Position.X + ((mob.Sprite.Width / 3) / 2), mob.Position.Y + (mob.Sprite.Height / 2));
                    double hypothenuse = Math.Pow(Math.Abs(targetedMobCenter.X - mobCenter.X), 2) + Math.Pow(Math.Abs(targetedMobCenter.Y - mobCenter.Y), 2); //Th pythagore
                    if (_splashArea >= Math.Sqrt(hypothenuse)) //si le mob est dans le range du splash
                    {
                        _mobArea.Add(mob);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _timerMissile -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            UpdateRotate();


            if (_timerMissile <= 0)
            {
                _readyToFire = true;
            }

            if (_readyToFire)
            {
                if (_targetedMob != null || _manualTargetedMob != null)
                {
                    if (_splashEffect)
                        FindSplashAreaMob();
                    FireMissile();
                    _timerMissile = 1000 / _attackSpeed;

                    _readyToFire = false;
                }
            }

            UpdateMissiles();
        }

        private void UpdateRotate()
        {
            Mob mob;
            if (ManualTargetedMob != null)
                mob = ManualTargetedMob;
            else if (TargetedMob != null)
                mob = TargetedMob;
            else
                return;


            float cote1 = Math.Abs(Center.Y - mob.Center.Y);
            float cote2 = Math.Abs(Center.X - mob.Center.X);
            float oppose = 0;
            float adjacent = 0;

            double angle = 0;


            if (cote1 <= cote2)
            {
                oppose = cote1;
                adjacent = cote2;
                if (mob.Center.X < this.Center.X)
                {
                    if (mob.Center.Y < this.Center.Y)
                        angle = MathHelper.PiOver2 - Math.Tanh(oppose / adjacent);
                    else if (mob.Center.Y > this.Center.Y)
                        angle = MathHelper.PiOver2 + Math.Tanh(oppose / adjacent);

                }
                else if (mob.Center.X > this.Center.X)
                {
                    if (mob.Center.Y < this.Center.Y)
                        angle = MathHelper.Pi + MathHelper.PiOver2 + Math.Tanh(oppose / adjacent);
                    else if (mob.Center.Y > this.Center.Y)
                        angle = MathHelper.Pi + MathHelper.PiOver2 - Math.Tanh(oppose / adjacent);
                }
            }
            else
            {
                oppose = cote2;
                adjacent = cote1;
                if (mob.Center.X < this.Center.X)
                {
                    if (mob.Center.Y < this.Center.Y)
                        angle = Math.Tanh(oppose / adjacent);
                    else if (mob.Center.Y > this.Center.Y)
                        angle = MathHelper.Pi - Math.Tanh(oppose / adjacent);

                }
                else if (mob.Center.X > this.Center.X)
                {
                    if (mob.Center.Y < this.Center.Y)
                        angle = 2 * MathHelper.Pi - Math.Tanh(oppose / adjacent);
                    else if (mob.Center.Y > this.Center.Y)
                        angle = MathHelper.Pi + Math.Tanh(oppose / adjacent);
                }
            }

            _rotate = (float) -angle;
        }

        public void FireMissile()
        {
            if (_manualTargetedMob != null)
            {
                Missile missile = new Missile(_missileSpeed, Center, _name.Substring(0, _name.Length - 1), _manualTargetedMob);
                _missiles.Add(missile);
            }
            else if (_targetedMob != null)
            {
                Missile missile = new Missile(_missileSpeed, Center, _name.Substring(0, _name.Length - 1), _targetedMob);
                _missiles.Add(missile);
            }
            
            
        }

        public void UpdateMissiles()
        {
            if (_missiles != null)
            {
                foreach (Missile missile in _missiles)
                {
                    missile.Update();
                }
                for (int i = 0; i < _missiles.Count; i++)
                {
                    if (_missiles[i].Alive == false)
                        _missiles.RemoveAt(i);
                }
            }
        }

        public void ManageCollision(Player player)
        {
            if (_missiles != null)
            {
                foreach (Missile missile in _missiles)
                {
                    if (Intersections.intersectPixel(missile.Sprite, missile.TargetedMob.Sprite, missile.Rectangle, missile.TargetedMob.Rectangle))
                    {
                        missile.Explode();

                        if (_splashEffect)
                        {
                            foreach (Mob mob in _mobArea)
                            {
                                if (mob.GetHit(this))
                                {
                                    player.Gold += mob.Gold;
                                    player.Kill++;
                                }
                            }

                        }



                        bool isDead = missile.TargetedMob.GetHit(this);

                        if (isDead)
                        {
                            player.Gold += missile.TargetedMob.Gold;
                            player.Kill++;
                        }
                        else
                        {
                            if (_stunChance != 0 || _fearChance != 0)
                            {
                                Random r = new Random();
                                int nbr = r.Next(1, 100);
                                int nbr2 = r.Next(1, 100);
                                if (nbr <= (_stunChance * 100))
                                {
                                //    missile.TargetedMob.Stun = true;
                                //    missile.TargetedMob.StunTimer = _stunTime * 1000;
                                    ClientGame.GetInstance().OutStunMob(missile.TargetedMob.Id);
                                }
                                if (nbr2 <= (_fearChance * 100))
                                {
                                    missile.TargetedMob.Fear();
                                    ClientGame.GetInstance().OutFearMob(missile.TargetedMob.Id);
                                }
                            }

                            if (_slowPercentage != 0)
                            {
                                missile.TargetedMob.SlowPercentage = _slowPercentage;
                                missile.TargetedMob.SlowTimer = _slowTime * 1000;

                                foreach (Mob mob in _mobArea)
                                {
                                    mob.SlowPercentage = _slowPercentage;
                                    mob.SlowTimer = _slowTime * 1000;
                                }
                            }
                        }

                    }
                }
            }
        }

        public void ManageCollisionOtherPlayer(Player player)
        {
            if (_missiles != null)
            {
                foreach (Missile missile in _missiles)
                {
                    if (Intersections.intersectPixel(missile.Sprite, missile.TargetedMob.Sprite, missile.Rectangle, missile.TargetedMob.Rectangle))
                    {
                        missile.Explode();

                        if (_splashEffect)
                        {
                            foreach (Mob mob in _mobArea)
                            {
                                if (mob.GetHit(this))
                                {
                                    player.Gold += mob.Gold;
                                    player.Kill++;
                                }
                            }

                        }



                        bool isDead = missile.TargetedMob.GetHit(this);

                        if (isDead)
                        {
                            player.Gold += missile.TargetedMob.Gold;
                            player.Kill++;
                        }
                        else
                        {
                            

                            if (_slowPercentage != 0)
                            {
                                missile.TargetedMob.SlowPercentage = _slowPercentage;
                                missile.TargetedMob.SlowTimer = _slowTime * 1000;

                                foreach (Mob mob in _mobArea)
                                {
                                    mob.SlowPercentage = _slowPercentage;
                                    mob.SlowTimer = _slowTime * 1000;
                                }
                            }
                        }

                    }
                }
            }
        }

        #endregion

        public String GetInfos()
        {
            String infos = "";
            infos += _id;
            infos += ";";
            infos += _position.X;
            infos += ";";
            infos += _position.Y;
            infos += ";";
            infos += _attackSpeed;
            infos += ";";
            infos += _damage;
            infos += ";";
            infos += _fearChance;
            infos += ";";
            infos += _missileSpeed;
            infos += ";";
            infos += _name;
            infos += ";";
            infos += _price;
            infos += ";";
            infos += _range;
            infos += ";";
            infos += _slowPercentage;
            infos += ";";
            infos += _slowTime;
            infos += ";";
            infos += _splashEffect;
            infos += ";";
            infos += _stunChance;
            infos += ";";
            infos += _stunTime;
            infos += ";";
            infos += _level;
            infos += ";";
            infos += _rotate;
            infos += ";";
            infos += _rect.X;
            infos += ";";
            infos += _rect.Y;
            infos += ";";
            infos += _rect.Width;
            infos += ";";
            infos += _rect.Height;
            infos += ";";
            //infos += _timerMissile;
            //infos += ";";
            infos += _towerState.ToString();
            infos += ";";
            infos += _comment;
            infos += ";";
            if (_targetedMob != null)
                infos += _targetedMob.Id;
            else
                infos += "-1";
            infos += ";";
            if (_manualTargetedMob != null)
                infos += _manualTargetedMob.Id;
            else
                infos += "-1";
            infos += ";";
            infos += _mobArea.Count + ":";
            foreach (Mob mob in _mobArea)
                infos += mob.Id + ":";


            return infos;
        }

        public static Tower SetInfos(String str)
        {
            Tower tower = new Tower();

            String[] infos = str.Split(';');
            int index = 0;
            tower.Id = int.Parse(infos[index++]);
            tower.Position = new Vector2(int.Parse(infos[index++]), int.Parse(infos[index++]));
            tower.AttackSpeed = float.Parse(infos[index++]);
            tower.Damage = int.Parse(infos[index++]);
            tower.FearChance = float.Parse(infos[index++]);
            tower.MissileSpeed = float.Parse(infos[index++]);
            tower.Name = infos[index++];
            tower.Price = int.Parse(infos[index++]);
            tower.Range = float.Parse(infos[index++]);
            tower.SlowPercentage = float.Parse(infos[index++]);
            tower.SlowTime = float.Parse(infos[index++]);
            tower.SplashEffect = bool.Parse(infos[index++]);
            tower.StunChance = float.Parse(infos[index++]);
            tower.StunTime = float.Parse(infos[index++]);
            tower.Level = int.Parse(infos[index++]);
            tower.Rotate = float.Parse(infos[index++]);
            tower.Rect = new Rectangle(int.Parse(infos[index++]), int.Parse(infos[index++]), int.Parse(infos[index++]), int.Parse(infos[index++]));
            //tower.TimerMissile  = float.Parse(infos[index++]);
            tower.TowerState = (Enumeration.BuildingState)Enum.Parse(typeof(Enumeration.BuildingState), infos[index++], true);
            tower.Comment = infos[index++];
            
            int idTargetedMob = int.Parse(infos[index++]);
            if(idTargetedMob != -1)
                tower.TargetedMob = Wave.GetInstance().Mobs.Find(o => o.Id == idTargetedMob);
            idTargetedMob = int.Parse(infos[index++]);
            if (idTargetedMob != -1)
                tower.ManualTargetedMob = Wave.GetInstance().Mobs.Find(o => o.Id == idTargetedMob);

            String sequence = infos[index++];
            String[] sequences = sequence.Split(':');
            int mobAreaCount = int.Parse(sequences[0]);
            List<Mob> tmpMobArea = new List<Mob>();
            for (int i = 0; i < mobAreaCount; i++)
            {
                tmpMobArea.Add(Wave.GetInstance().Mobs.Find(o => o.Id == int.Parse(sequences[i + 1])));
            }
            tower.MobArea = tmpMobArea;


            return tower;
        }

        public void ToNewVersion(Tower tower)
        {
            this._attackSpeed = tower.AttackSpeed;
            this._damage = tower.Damage;
            this._fearChance = tower.FearChance;
            this._missileSpeed = tower.MissileSpeed;
            this._price = tower.Price;
            this._range = tower.Range;
            this._slowPercentage = tower.SlowPercentage;
            this._slowTime = tower.SlowTime;
            this._splashEffect = tower.SplashEffect;
            this._stunChance = tower.StunChance;
            this._stunTime = tower.StunTime;
            this._manualTargetedMob = tower.ManualTargetedMob; //si le joueur cible un enemie spécifique
            this._comment = tower.Comment;
            this._level = tower.Level;
            this._rotate = tower.Rotate;
            this._rect = tower.Rect;
        }
    }
}
