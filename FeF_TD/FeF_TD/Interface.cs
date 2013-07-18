using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FeF_TD
{
    public class Interface
    {
        #region Variables

        private Texture2D _backgroundSprite;
        private Bouton _baseLvlUp;
        private Texture2D _baseSprite;
        private Vector2 _baseStringPosition;
        private Vector2 _basePosition;
        private string _baseString;
        private string _baseLvl;
        private string _infoPlayer;
        private Vector2 _infoPlayerPosition;
        private Texture2D _goldSprite;
        private Vector2 goldPosition;
        private string _goldString;
        private Vector2 _box1Position;
        private Vector2 _box2Position;
        private Vector2 _box3Position;
        private Texture2D _killSprite;
        private Vector2 _killPosition;
        private string _killString;
        private Texture2D _liveSprite;
        private Vector2 _livePosition;
        private string _liveString;
        private Vector2 _position;
        private Dictionary<string, int> _playersKillInfo;
        private List<Bouton> _towersBoutons;
        private List<String> _towersInfo;
        private Vector2 _towersInfoPosition;
        private SpriteFont _font;
        private SpriteFont _font2;
        private SpriteFont _font3;

        #endregion

        #region Properties

        public SpriteFont Font2
        {
            get { return _font2; }
            set { _font2 = value; }
        }

        public SpriteFont Font3
        {
            get { return _font3; }
            set { _font3 = value; }
        }

        public Vector2 InfoPlayerPosition
        {
            get { return _infoPlayerPosition; }
            set { _infoPlayerPosition = value + _box2Position; }
        }

        public Vector2 BaseStringPosition
        {
            get { return _baseStringPosition; }
            set { _baseStringPosition = value += _box1Position; }
        }

        public Vector2 TowersInfoPosition
        {
            get { return _towersInfoPosition; }
            set { _towersInfoPosition = value + _box3Position; }
        }

        public List<Bouton> TowersBoutons
        {
            get { return _towersBoutons; }
            set 
            {
                List<Bouton> bs = value;
                foreach (Bouton b in bs)
                {
                    b.Position += _box3Position;
                    b.PositionOver += _box3Position;
                    b.Rect = new Rectangle((int)b.Position.X, (int)b.Position.Y, b.Texture.Width, b.Texture.Height);
                }
                
                _towersBoutons = bs; 
            }
        }

        public string InfoPlayer
        {
            get { return _infoPlayer; }
            set { _infoPlayer = value; }
        }

        public string BaseLvl
        {
            get { return _baseLvl; }
            set { _baseLvl = value; }
        }

        public Vector2 LivePosition
        {
            get { return _livePosition; }
            set { _livePosition = value + _box2Position; }
        }

        public Vector2 KillPosition
        {
            get { return _killPosition; }
            set { _killPosition = value + _box2Position; }
        }

        public Vector2 GoldPosition
        {
            get { return goldPosition; }
            set { goldPosition = value + _box2Position; }
        }

        public Vector2 BasePosition
        {
            get { return _basePosition; }
            set { _basePosition = value + _box1Position; }
        }

        public Texture2D BackgroundSprite
        {
            get { return _backgroundSprite; }
            set { _backgroundSprite = value; }
        }

        public Bouton BaseLvlUp
        {
            get { return _baseLvlUp; }
            set 
            {
                Bouton b = value;
                b.Position += _box1Position;
                b.PositionOver += _box1Position;
                b.Rect = new Rectangle((int)b.Position.X, (int)b.Position.Y, b.Texture.Width, b.Texture.Height);
                _baseLvlUp = b;
            }
        }

        public Texture2D BaseSprite
        {
            get { return _baseSprite; }
            set { _baseSprite = value; }
        }

        public string BaseString
        {
            get { return _baseString; }
            set { _baseString = value; }
        }

        public Texture2D GoldSprite
        {
            get { return _goldSprite; }
            set { _goldSprite = value; }
        }

        public string GoldString
        {
            get { return _goldString; }
            set { _goldString = value; }
        }

        public Vector2 Box1Position
        {
            get { return _box1Position; }
            set { _box1Position = value; }
        }

        public Vector2 Box2Position
        {
            get { return _box2Position; }
            set { _box2Position = value; }
        }

        public Vector2 Box3Position
        {
            get { return _box3Position; }
            set { _box3Position = value; }
        }

        public Texture2D KillSprite
        {
            get { return _killSprite; }
            set { _killSprite = value; }
        }

        public string KillString
        {
            get { return _killString; }
            set { _killString = value; }
        }

        public Texture2D LiveSprite
        {
            get { return _liveSprite; }
            set { _liveSprite = value; }
        }

        public string LiveString
        {
            get { return _liveString; }
            set { _liveString = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Dictionary<string, int> PlayersKillInfo
        {
            get { return _playersKillInfo; }
            set { _playersKillInfo = value; }
        }


        public List<String> TowersInfo
        {
            get { return _towersInfo; }
            set { _towersInfo = value; }
        }

        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }
        #endregion

        #region Methods

        public Interface()
        {

        }

        public Interface(String baseStr, String playerStr, Vector2 position, Vector2 box1, Vector2 box2, Vector2 box3)
        {
            _towersBoutons = new List<Bouton>();
            _towersInfo = new List<string>();
            _baseString = baseStr;
            _infoPlayer = playerStr;
            _position = position;
            _box1Position = box1;
            _box2Position = box2;
            _box3Position = box3;
        }

        public void Update()
        {

        }

        public int FindBoutonIndex(List<Bouton> boutons, String name)
        {
            for (int i = 0; i < boutons.Count(); i++)
            {
                if (boutons[i].Name == name)
                    return i;
            }
            return -1;
        }

        public void RemoveSelectedTowers()
        {
            foreach (Bouton b in TowersBoutons)
            {
                b.Selected = false;
            }
        }

        #endregion
    }
}
