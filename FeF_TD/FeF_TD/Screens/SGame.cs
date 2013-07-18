using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Datas;
using System.Threading;

namespace FeF_TD
{
    class SGame : GameScreen
    {
        private World world;
        private Texture2D map;
        private Texture2D loadingString;

        private Interface gui;
        private TowerGUI towerGui;
        private CursorSelection cursSelection;
        private List<FloatingMessage> fms;
        private Wave wave;


        public static DataTower[] aoeTowers;
        public static DataTower[] attaqueTowers;
        public static DataTower[] freezeTowers;
        public static DataTower[] poisonTowers;
        public static DataTower[] speedTowers;
        public static DataMob[] mobs;

        private bool showStatsPlayers;

        private Enumeration.BuildingState buildingState;
        private Texture2D texturenull;
        private Texture2D textureRange;
        private Texture2D healthBar;
        //private Player player;
        private String myName;
        private float C2STimer = 0;
        private List<Player> otherPlayers;
        private bool youLose;
        private Texture2D textureYouLose;

        public SGame(GraphicsDeviceManager gdm, SpriteBatch sb, GraphicsDevice gd)
            : base(gdm, sb, gd)
        {
            this.screen = Enumeration.Screen.Game;
            buildingState = Enumeration.BuildingState.None;
            fms = new List<FloatingMessage>();
            cgame = ClientGame.GetInstance();
        }

        public override void LoadContent(ContentManager content)
        {
            LineBatch.Init(graphicsDevice);
            loadingString = content.Load<Texture2D>(Config.IMG_STRING + "Loading");
            cgame.StartGame += new ClientGame.StartGameHandler(cgame_StartGame);
            world = new World();
            world.LoadMap(2);

            map = content.Load<Texture2D>(Config.IMG_GAME + "map");


            InitPlayer();
            InitInterface(content);
            InitTowers(content);
            InitMobs(content);
            InitSelector(content);
            InitWaveFirst(content); // on initialise le lvl1


            InitOverSprite(content);

            base.LoadContent(content);
        }

        void cgame_StartGame()
        {
            initDone = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        private void InitPlayer()
        {
            //player = new Player(GlobalAppVariables.PlayerName);
            myName = GlobalAppVariables.PlayerName;


            otherPlayers = cgame.GetOtherPlayersThan(myName);
        }

        private void InitSelector(ContentManager content)
        {

            cursSelection = new CursorSelection();
            cursSelection.Sprite = content.Load<Texture2D>(Config.IMG_GAME + "selector");
        }

        private void InitInterface(ContentManager content)
        {
            gui = new Interface(Config.GAME_GUI_BASE, Config.GAME_GUI_PLAYER,
                new Vector2(0, (9 * Config.APP_CELLS_SIZE)), new Vector2(2, (9 * Config.APP_CELLS_SIZE) + 2), new Vector2(350, (9 * Config.APP_CELLS_SIZE) + 2), new Vector2(690, (9 * Config.APP_CELLS_SIZE) + 2));
            gui.BackgroundSprite = content.Load<Texture2D>(Config.IMG_SYS + "interface");
            gui.BaseSprite = content.Load<Texture2D>(Config.IMG_SYS + "base");
            gui.BasePosition = new Vector2(9, 7);
            gui.GoldSprite = content.Load<Texture2D>(Config.IMG_SYS + "gold");
            gui.GoldPosition = new Vector2(20, 50);
            gui.LiveSprite = content.Load<Texture2D>(Config.IMG_SYS + "life");
            gui.LivePosition = new Vector2(20, 100);
            gui.BaseLvlUp = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "lvlup"), new Vector2(240, 90),
                                    content.Load<Texture2D>(Config.IMG_SYS + "lvlupover"), Vector2.Zero);
            gui.KillSprite = content.Load<Texture2D>(Config.IMG_SYS + "mort");
            gui.KillPosition = new Vector2(150, 50);
            gui.Font = content.Load<SpriteFont>(Config.FONT + "gui");
            gui.Font2 = content.Load<SpriteFont>(Config.FONT + "InputFont");
            gui.Font3 = content.Load<SpriteFont>(Config.FONT + "font3");

            List<Bouton> boutons = new List<Bouton>();
            boutons.Add(new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "Icones-Towers\\" + "Aoe"), new Vector2(0, 5), "aoe"));
            boutons.Add(new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "Icones-Towers\\" + "Attaque"), new Vector2(65, 5), "attaque"));
            boutons.Add(new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "Icones-Towers\\" + "Freeze"), new Vector2(130, 5), "freeze"));
            boutons.Add(new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "Icones-Towers\\" + "Poison"), new Vector2(195, 5), "poison"));
            boutons.Add(new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "Icones-Towers\\" + "Speed"), new Vector2(260, 5), "speed"));
            gui.TowersBoutons = boutons;

            gui.TowersInfo.Add(Config.GAME_GUI_TOWER_AOE);
            gui.TowersInfo.Add(Config.GAME_GUI_TOWER_AOE);
            gui.TowersInfo.Add(Config.GAME_GUI_TOWER_AOE);
            gui.TowersInfo.Add(Config.GAME_GUI_TOWER_AOE);
            gui.TowersInfo.Add(Config.GAME_GUI_TOWER_AOE);

            gui.TowersInfoPosition = new Vector2(10, 90);

            gui.BaseStringPosition = new Vector2(145, 10);
            gui.InfoPlayerPosition = new Vector2(30, 5);

            youLose = false;
            textureYouLose = content.Load<Texture2D>(Config.IMG_SYS + "Youlose");

        }

        private void InitTowers(ContentManager content)
        {
            texturenull = content.Load<Texture2D>(Config.IMG_SYS + "6464");

            aoeTowers = new DataTower[6];
            attaqueTowers = new DataTower[6];
            freezeTowers = new DataTower[6];
            poisonTowers = new DataTower[6];
            speedTowers = new DataTower[6];

            Sprites.towerAoe = new Texture2D[6];
            Sprites.towerAttaque = new Texture2D[6];
            Sprites.towerFreeze = new Texture2D[6];
            Sprites.towerPoison = new Texture2D[6];
            Sprites.towerSpeed = new Texture2D[6];

            for (int i = 0; i < 6; i++)
            {
                aoeTowers[i] = new DataTower();
                attaqueTowers[i] = new DataTower();
                freezeTowers[i] = new DataTower();
                poisonTowers[i] = new DataTower();
                speedTowers[i] = new DataTower();
            }

            //Charger les fichiers xml
            for (int i = 0; i < 6; i++)
            {
                aoeTowers[i] = content.Load<DataTower>(Config.DATAS + "Tower-aoe" + (i + 1));
                attaqueTowers[i] = content.Load<DataTower>(Config.DATAS + "Tower-attaque" + (i + 1));
                freezeTowers[i] = content.Load<DataTower>(Config.DATAS + "Tower-freeze" + (i + 1));
                poisonTowers[i] = content.Load<DataTower>(Config.DATAS + "Tower-poison" + (i + 1));
                speedTowers[i] = content.Load<DataTower>(Config.DATAS + "Tower-speed" + (i + 1));



                Sprites.towerAoe[i] = content.Load<Texture2D>(Config.IMG_TOWER + aoeTowers[i].spritePath);
                Sprites.towerAttaque[i] = content.Load<Texture2D>(Config.IMG_TOWER + attaqueTowers[i].spritePath);
                Sprites.towerFreeze[i] = content.Load<Texture2D>(Config.IMG_TOWER + freezeTowers[i].spritePath);
                Sprites.towerPoison[i] = content.Load<Texture2D>(Config.IMG_TOWER + poisonTowers[i].spritePath);
                Sprites.towerSpeed[i] = content.Load<Texture2D>(Config.IMG_TOWER + speedTowers[i].spritePath);


            }

            Base.GetInstance().DataBase = content.Load<DataBase>(Config.DATAS + "Base-Lvl");
            Base.GetInstance().Init();

            Sprites.towerAoeBullet = content.Load<Texture2D>(Config.IMG_TOWER + "Aoe//AoeBullet");
            Sprites.towerAttaqueBullet = content.Load<Texture2D>(Config.IMG_TOWER + "Attaque//AttaqueBullet");
            Sprites.towerFreezeBullet = content.Load<Texture2D>(Config.IMG_TOWER + "Freeze//FreezeBullet");
            Sprites.towerPoisonBullet = content.Load<Texture2D>(Config.IMG_TOWER + "Poison//PoisonBullet");
            Sprites.towerSpeedBullet = content.Load<Texture2D>(Config.IMG_TOWER + "Speed//SpeedBullet");



            towerGui = new TowerGUI();
            towerGui.TowerLvlUp = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "lvlup"), gui.Box3Position + new Vector2(285, 30),
                                    content.Load<Texture2D>(Config.IMG_SYS + "lvlupover"), Vector2.Zero);

        }

        private void InitMobs(ContentManager content)
        {
            mobs = new DataMob[Config.WAVES_COUNT];
            Sprites.mob = new Texture2D[Config.WAVES_COUNT];
            healthBar = content.Load<Texture2D>(Config.IMG_GAME + "healthbar");

            //Charger les fichiers xml
            for (int i = 0; i < Config.WAVES_COUNT; i++)
            {
                mobs[i] = content.Load<DataMob>(Config.DATAS + "Mob-wave" + (i + 1));

                Sprites.mob[i] = content.Load<Texture2D>(Config.IMG_MOBS + mobs[i].spritePath);
            }
        }

        private void InitWaveFirst(ContentManager content)
        {
            InitWave(1);

            wave.Bouton = new Bouton(content.Load<Texture2D>(Config.IMG_SYS + "boutonwave"), new Vector2(950, 2),
                content.Load<Texture2D>(Config.IMG_SYS + "boutonwaveover"), Vector2.Zero);

        }

        private void InitWave(int level)
        {
            wave = Wave.GetInstance();
            wave.Set(mobs[level - 1], Sprites.mob[level - 1]);

        }

        private void InitOverSprite(ContentManager content)
        {
            textureRange = content.Load<Texture2D>(Config.IMG_GAME + "range");
        }

        public override void Update(GameTime gameTime)
        {
            if (initDone)
            {
                //Thread thread1 = new Thread(CheckMouseClick);
                //Thread thread2 = new Thread(CheckMouseRightClick);
                //thread1.Start();
                //thread2.Start();

                if (!youLose)
                {
                    CheckMouseClick();
                    CheckMouseRightClick();
                }

                KeyboardState keyboardState = Keyboard.GetState();

                showStatsPlayers = false;

                if (keyboardState.IsKeyDown(Keys.Tab))
                    showStatsPlayers = true;

                for (int i = 0; i < fms.Count(); i++)
                {
                    if (!fms[i].Update()) //Si le time to leave est finit, on supprime le message de la liste
                    {
                        fms.RemoveAt(i);
                    }
                }

                bool waveFinish = wave.Update(gameTime, cgame.GetMyPlayer(myName));

                if (waveFinish)
                {
                    this.InitWave(wave.CurrentWave + 1); //on initialise la vague suivante
                    wave.Bouton.Enabled = true;
                }

                if (towerGui.Enabled)
                    towerGui.Update();

                UpdateTower(gameTime);

                previousKeyboardState = keyboardState;

                base.Update(gameTime);

                if (C2STimer <= 0)
                {
                    cgame.OutUpdatePlayer(myName);
                    C2STimer = 150;
                }
                C2STimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }


        protected override void CheckMouseClick()
        {
            base.CheckMouseClick();

            if (leftMouseClicked)
            {
                bool stopCheckingMouse = false;

                if (IsCursorInGUI())
                {
                    gui.RemoveSelectedTowers();
                    buildingState = Enumeration.BuildingState.None;
                }
                if (!towerGui.Enabled)
                {
                    foreach (Bouton b in gui.TowersBoutons)
                    {
                        if (b.IsMouseOver(mousePosition))
                        {
                            switch (b.Name)
                            {
                                case "aoe":
                                    buildingState = Enumeration.BuildingState.Aoe;
                                    break;
                                case "attaque":
                                    buildingState = Enumeration.BuildingState.Attaque;
                                    break;
                                case "freeze":
                                    buildingState = Enumeration.BuildingState.Freeze;
                                    break;
                                case "poison":
                                    buildingState = Enumeration.BuildingState.Poison;
                                    break;
                                case "speed":
                                    buildingState = Enumeration.BuildingState.Speed;
                                    break;
                            }
                        }
                    }
                }


                if (!IsCursorInGUI() && buildingState != Enumeration.BuildingState.None)
                {
                    Vector2 position = world.GetCurrentTowerPosition(mousePosition);
                    bool isAvailable = world.IsAvailablePosition(mousePosition, cgame.GetMyPlayer(myName).Towers);

                    if (isAvailable)
                        BuildTower(position);

                    stopCheckingMouse = true;
                }

                if (towerGui.Enabled && towerGui.TowerLvlUp.IsMouseOver(mousePosition))
                {
                    int lvlTowerCalled = cgame.GetMyPlayer(myName).FocusedTower.Level;
                    if (lvlTowerCalled < cgame.GetMyPlayer(myName).BaseLevel)
                    {
                        if (cgame.GetMyPlayer(myName).Buy(towerGui.PriceNext))
                        {
                            cgame.GetMyPlayer(myName).FocusedTower.LevelUp();
                            FloatingMessage fm = new FloatingMessage("- " + towerGui.PriceNext + " g", cgame.GetMyPlayer(myName).FocusedTower.Position, Color.Yellow, new Vector2(0, -1), 1f);
                            FloatingMessage fm2 = new FloatingMessage("Level Up!", cgame.GetMyPlayer(myName).FocusedTower.Position + new Vector2(0, 32), Color.Gold, new Vector2(-1, 1), 0.3f);
                            fms.Add(fm);
                            fms.Add(fm2);
                        }
                        else
                        {
                            FloatingMessage fm = new FloatingMessage("Not enough gold !", cgame.GetMyPlayer(myName).FocusedTower.Position, Color.Red, new Vector2(0, 1), 0.5f);
                            fms.Add(fm);
                        }
                    }
                    else
                    {
                        FloatingMessage fm = new FloatingMessage("Increase your base !", cgame.GetMyPlayer(myName).FocusedTower.Position, Color.Red, new Vector2(0, 1), 0.5f);
                        fms.Add(fm);
                    }

                    stopCheckingMouse = true;
                }

                if (gui.BaseLvlUp.IsMouseOver(mousePosition))
                {
                    if (cgame.GetMyPlayer(myName).BaseLevel < 6)
                    {
                        if (cgame.GetMyPlayer(myName).BaseUpgrade())
                        {
                            FloatingMessage fm = new FloatingMessage("- " + towerGui.PriceNext + " g", gui.BasePosition + new Vector2(80, 50), Color.Yellow, new Vector2(0, -1), 1f);
                            FloatingMessage fm2 = new FloatingMessage("Level Up!", gui.BasePosition + new Vector2(80, 82), Color.Gold, new Vector2(-1, 1), 0.3f);
                            fms.Add(fm);
                            fms.Add(fm2);
                        }
                        else
                        {
                            FloatingMessage fm = new FloatingMessage("Not enough gold !", gui.BasePosition + new Vector2(80, 50), Color.Red, new Vector2(0, 1), 0.5f);
                            fms.Add(fm);
                        }
                    }
                }

                //a chaque clic gauche on enleve la selection
                towerGui.Disable();
                cursSelection.Selected = false;


                if (!stopCheckingMouse)
                {
                    foreach (Tower tower in cgame.GetMyPlayer(myName).Towers)
                    {
                        if (new Rectangle((int)tower.Position.X, (int)tower.Position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE)
                            .Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)))
                        {
                            cgame.GetMyPlayer(myName).FocusedTower = tower;
                            towerGui.Set(tower);
                            towerGui.Enable();
                            cursSelection.Set(tower.Position);
                            cursSelection.Selected = true;

                            stopCheckingMouse = true;
                            break;
                        }

                    }
                }

                //if (!stopCheckingMouse)
                //{
                //    foreach (Mob mob in wave.Mobs)
                //    {
                //        if (mob.Alive && mob.Enabled)
                //        {
                //            if (Intersections.intersectPixelMouse(mousePosition, mob))
                //            {
                //                break;
                //            }
                //        }
                //    }
                //}

                if (wave.Bouton.Enabled && wave.Bouton.IsMouseOver(mousePosition))
                {
                    cgame.OutNextWaveCalled();
                }

                leftMouseClicked = false;
            }

            if (cgame.GetMyPlayer(myName) != null)
            {
                bool gameContinue = cgame.GetMyPlayer(myName).CheckPlayer();

                if (!gameContinue)
                {
                    //GameStateManagementGame.CallScreen(Enumeration.Screen.Game, Enumeration.Screen.MainMenu);
                    youLose = true;
                }
            }
        }

        protected override void CheckMouseRightClick()
        {
            base.CheckMouseRightClick();

            if (rightMouseClicked)
            {
                if (IsCursorInGUI())
                {
                }
                else
                {
                    gui.RemoveSelectedTowers();
                    buildingState = Enumeration.BuildingState.None;

                    if (towerGui.Enabled) // si on a selectionné une tour, on va changer la target
                    {
                        foreach (Mob mob in wave.Mobs)
                        {
                            if (mob.Alive && mob.Enabled)
                            {
                                if (Intersections.intersectPixelMouse(mousePosition, mob))
                                {
                                    towerGui.Tower.ManualTargetedMob = mob;
                                    break;
                                }
                            }
                        }

                    }
                }
            }

            rightMouseClicked = false;
        }

        private bool IsCursorInGUI()
        {
            Rectangle rectGUI = new Rectangle((int)gui.Position.X, (int)gui.Position.Y, gui.BackgroundSprite.Width, gui.BackgroundSprite.Height);
            if (rectGUI.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)))
                return true;
            else
                return false;
        }

        private void BuildTower(Vector2 position)
        {
            Tower tower = new Tower();
            switch (buildingState)
            {
                case Enumeration.BuildingState.Aoe:
                    tower.Set(aoeTowers[0], buildingState);
                    break;
                case Enumeration.BuildingState.Attaque:
                    tower.Set(attaqueTowers[0], buildingState);
                    break;
                case Enumeration.BuildingState.Freeze:
                    tower.Set(freezeTowers[0], buildingState);
                    break;
                case Enumeration.BuildingState.Poison:
                    tower.Set(poisonTowers[0], buildingState);
                    break;
                case Enumeration.BuildingState.Speed:
                    tower.Set(speedTowers[0], buildingState);
                    break;
            }

            tower.Position = position;
            if (cgame.GetMyPlayer(myName).Buy(tower))
            {
                FloatingMessage fm = new FloatingMessage("- " + tower.Price + " g", mousePosition, Color.Yellow, new Vector2(0, -1), 1f);
                fms.Add(fm);
            }
            else
            {
                FloatingMessage fm = new FloatingMessage("Not enough gold !", mousePosition, Color.Red, new Vector2(0, 1), 0.5f);
                fms.Add(fm);
            }
            buildingState = Enumeration.BuildingState.None;
            gui.RemoveSelectedTowers();
        }

        public void UpdateTower(GameTime gameTime)
        {
            try
            {
                foreach (Tower tower in cgame.GetMyPlayer(myName).Towers)
                {
                    tower.FindTarget(wave.Mobs);
                    tower.Update(gameTime);
                    tower.ManageCollision(cgame.GetMyPlayer(myName));
                }
                foreach (Player player in otherPlayers)
                {
                    foreach (Tower tower in player.Towers)
                    {
                        tower.FindTarget(wave.Mobs);
                        tower.Update(gameTime);
                        tower.ManageCollisionOtherPlayer(player);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }


        public override void Draw(GameTime gameTime)
        {
            if (initDone)
            {
                graphicsDevice.Clear(Color.LightBlue);

                spriteBatch.Begin();

                #region Draw Map
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        int posx = 0;
                        int posy = 0;
                        switch (world.Map[j, i])
                        {
                            case World.Ground.begining:
                                posx = 2;
                                posy = 2;
                                break;
                            case World.Ground.ending:
                                posx = 2;
                                posy = 2;
                                break;
                            case World.Ground.inside:
                                posx = 1;
                                posy = 1;
                                break;
                            case World.Ground.outside:
                                posx = 5;
                                posy = 4;
                                break;
                            case World.Ground.ltcorner:
                                posx = 0;
                                posy = 0;
                                break;
                            case World.Ground.rtcorner:
                                posx = 4;
                                posy = 0;
                                break;
                            case World.Ground.lbcorner:
                                posx = 0;
                                posy = 4;
                                break;
                            case World.Ground.rbcorner:
                                posx = 4;
                                posy = 4;
                                break;
                            case World.Ground.ltinnercorner:
                                posx = 5;
                                posy = 3;
                                break;
                            case World.Ground.rtinnercorner:
                                posx = 5;
                                posy = 0;
                                break;
                            case World.Ground.lbinnercorner:
                                posx = 5;
                                posy = 2;
                                break;
                            case World.Ground.rbinnercorner:
                                posx = 5;
                                posy = 1;
                                break;
                            case World.Ground.tborder:
                                posx = 1;
                                posy = 0;
                                break;
                            case World.Ground.rborder:
                                posx = 4;
                                posy = 1;
                                break;
                            case World.Ground.bborder:
                                posx = 1;
                                posy = 4;
                                break;
                            case World.Ground.lborder:
                                posx = 0;
                                posy = 1;
                                break;
                        }
                        spriteBatch.Draw(map, new Vector2(j * Config.APP_CELLS_SIZE, i * Config.APP_CELLS_SIZE), new Rectangle(64 * posx, 64 * posy, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), Color.White);
                    }
                }
                #endregion

                #region Draw Buildings Temp

                if (buildingState != Enumeration.BuildingState.None)
                {
                    Vector2 position = world.GetCurrentTowerPosition(mousePosition);
                    bool isAvailable = world.IsAvailablePosition(mousePosition, cgame.GetMyPlayer(myName).Towers);
                    int index;


                    switch (buildingState)
                    {
                        case Enumeration.BuildingState.Aoe:
                            index = gui.FindBoutonIndex(gui.TowersBoutons, "aoe");
                            gui.RemoveSelectedTowers();
                            gui.TowersBoutons[index].Selected = true;
                            if (!IsCursorInGUI())
                            {
                                if (isAvailable)
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(50, 150, 200));
                                else
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(255, 50, 50, 70));
                                spriteBatch.Draw(Sprites.towerAoe[0], position, Color.White);
                            }
                            break;
                        case Enumeration.BuildingState.Attaque:
                            index = gui.FindBoutonIndex(gui.TowersBoutons, "attaque");
                            gui.RemoveSelectedTowers();
                            gui.TowersBoutons[index].Selected = true;
                            if (!IsCursorInGUI())
                            {
                                if (isAvailable)
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(50, 150, 200));
                                else
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(255, 50, 50, 70));
                                spriteBatch.Draw(Sprites.towerAttaque[0], position, Color.White);
                            }
                            break;
                        case Enumeration.BuildingState.Freeze:
                            index = gui.FindBoutonIndex(gui.TowersBoutons, "freeze");
                            gui.RemoveSelectedTowers();
                            gui.TowersBoutons[index].Selected = true;
                            if (!IsCursorInGUI())
                            {
                                if (isAvailable)
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(50, 150, 200));
                                else
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(255, 50, 50, 70));
                                spriteBatch.Draw(Sprites.towerFreeze[0], position, Color.White);
                            }
                            break;
                        case Enumeration.BuildingState.Poison:
                            index = gui.FindBoutonIndex(gui.TowersBoutons, "poison");
                            gui.RemoveSelectedTowers();
                            gui.TowersBoutons[index].Selected = true;
                            if (!IsCursorInGUI())
                            {
                                if (isAvailable)
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(50, 150, 200));
                                else
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(255, 50, 50, 70));
                                spriteBatch.Draw(Sprites.towerPoison[0], position, Color.White);
                            }
                            break;
                        case Enumeration.BuildingState.Speed:
                            index = gui.FindBoutonIndex(gui.TowersBoutons, "speed");
                            gui.RemoveSelectedTowers();
                            gui.TowersBoutons[index].Selected = true;
                            if (!IsCursorInGUI())
                            {
                                if (isAvailable)
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(50, 150, 200));
                                else
                                    spriteBatch.Draw(texturenull, new Rectangle((int)position.X, (int)position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE), new Color(255, 50, 50, 70));
                                spriteBatch.Draw(Sprites.towerSpeed[0], position, Color.White);
                            }
                            break;
                    }

                }

                #endregion

                #region Draw Mobs

                foreach (Mob mob in wave.Mobs)
                {
                    if (mob.Enabled && mob.Alive)
                    {
                        float angle = 0;
                        switch (mob.Direction1)
                        {
                            case Mob.Direction.Up:
                                angle = -MathHelper.PiOver2;
                                break;
                            case Mob.Direction.Right:
                                angle = 0;
                                break;
                            case Mob.Direction.Down:
                                angle = MathHelper.PiOver2;
                                break;
                            case Mob.Direction.Left:
                                angle = MathHelper.Pi;
                                break;
                        }
                        if (!mob.IsHit)
                            spriteBatch.Draw(mob.Sprite, mob.Position + new Vector2(32, 32), mob.SpriteRect, Color.White, angle, new Vector2(32, 32), 1f, SpriteEffects.None, 0);
                        else
                            spriteBatch.Draw(mob.Sprite, mob.Position + new Vector2(32, 32), mob.SpriteRect, Color.Red, angle, new Vector2(32, 32), 1f, SpriteEffects.None, 0);

                        spriteBatch.Draw(healthBar, new Vector2(mob.Position.X + (mob.Sprite.Width / 2 / 3) - (healthBar.Width / 2), mob.Position.Y), Color.White);
                        if ((float)mob.Health / (float)mob.BaseHealth >= 0.75)
                        {
                            spriteBatch.Draw(healthBar, new Vector2(mob.Position.X + (mob.Sprite.Width / 2 / 3) - (healthBar.Width / 2) + 1, mob.Position.Y + 1),
                                new Rectangle(60, 10, (int)(healthBar.Width * ((float)mob.Health / (float)mob.BaseHealth)), healthBar.Height - 2),
                                Color.Green);
                        }
                        else if ((float)mob.Health / (float)mob.BaseHealth >= 0.35)
                        {
                            spriteBatch.Draw(healthBar, new Vector2(mob.Position.X + (mob.Sprite.Width / 2 / 3) - (healthBar.Width / 2) + 1, mob.Position.Y + 1),
                                new Rectangle(60, 10, (int)(healthBar.Width * ((float)mob.Health / (float)mob.BaseHealth)), healthBar.Height - 2),
                                Color.Yellow);
                        }
                        else
                        {
                            spriteBatch.Draw(healthBar, new Vector2(mob.Position.X + (mob.Sprite.Width / 2 / 3) - (healthBar.Width / 2) + 1, mob.Position.Y + 1),
                                new Rectangle(60, 10, (int)(healthBar.Width * ((float)mob.Health / (float)mob.BaseHealth)), healthBar.Height - 2),
                                Color.Red);
                        }
                    }
                }

                #endregion

                #region Draw In Game Stuff

                if (cursSelection.Selected)
                {

                    Vector2 middleTextureTower = new Vector2(cursSelection.Position.X + (cursSelection.Sprite.Width / 2), cursSelection.Position.Y + (cursSelection.Sprite.Height / 2));
                    spriteBatch.Draw(textureRange,
                        new Vector2(middleTextureTower.X - (towerGui.Range), middleTextureTower.Y - (towerGui.Range)),
                        new Rectangle(0, 0, (int)textureRange.Width, (int)textureRange.Height),
                        Color.White,
                        0,
                        Vector2.Zero,
                        (towerGui.Range / 100.0f) * 2,
                        SpriteEffects.None,
                        0);


                    spriteBatch.Draw(cursSelection.Sprite, cursSelection.Position, Color.White);

                    if (towerGui.Tower.ManualTargetedMob != null)
                    {
                        LineBatch.DrawLine(spriteBatch, new Color(230, 0, 0, 100), towerGui.Tower.Position + new Vector2(32, 32), towerGui.Tower.ManualTargetedMob.Position + new Vector2(32, 32));
                    }
                    else if (towerGui.Tower.TargetedMob != null)
                    {
                        LineBatch.DrawLine(spriteBatch, new Color(230, 0, 0, 100), towerGui.Tower.Position + new Vector2(32, 32), towerGui.Tower.TargetedMob.Position + new Vector2(32, 32));
                    }
                }

                


                spriteBatch.DrawString(gui.Font2, wave.ToString(), new Vector2(650, 5), Color.PeachPuff);
                if (wave.Bouton.Enabled)
                {
                    if (!wave.Bouton.IsMouseOver(mousePosition))
                        spriteBatch.Draw(wave.Bouton.Texture, wave.Bouton.Position, Color.White);
                    else
                        spriteBatch.Draw(wave.Bouton.TextureOver, wave.Bouton.PositionOver, Color.White);
                }

                if (showStatsPlayers)
                {
                    int baseTopPos = 540;
                    int baseLeftPos = 650;
                    for (int i = 0; i < cgame.players.Count; i++)
                    {
                        spriteBatch.DrawString(gui.Font3, cgame.players.ElementAt(i).Name, new Vector2(baseLeftPos, baseTopPos - i * 50), Color.White);
                        spriteBatch.DrawString(gui.Font3, cgame.players.ElementAt(i).Live.ToString(), new Vector2(baseLeftPos + 150, baseTopPos - i * 30), Color.Red);
                        spriteBatch.DrawString(gui.Font3, cgame.players.ElementAt(i).Kill.ToString(), new Vector2(baseLeftPos + 220, baseTopPos - i * 30), Color.Gray);
                        spriteBatch.DrawString(gui.Font3, cgame.players.ElementAt(i).Gold.ToString(), new Vector2(baseLeftPos + 290, baseTopPos - i * 30), Color.Yellow);
                    }
                    spriteBatch.DrawString(gui.Font3, "Name", new Vector2(baseLeftPos, baseTopPos - cgame.players.Count * 50), Color.White);
                    spriteBatch.DrawString(gui.Font3, "Lives", new Vector2(baseLeftPos + 150, baseTopPos - cgame.players.Count * 50), Color.Red);
                    spriteBatch.DrawString(gui.Font3, "Kills", new Vector2(baseLeftPos + 220, baseTopPos - cgame.players.Count * 50), Color.Gray);
                    spriteBatch.DrawString(gui.Font3, "Gold", new Vector2(baseLeftPos + 290, baseTopPos - cgame.players.Count * 50), Color.Yellow);
                }

                #endregion

                #region Draw GUI

                spriteBatch.Draw(gui.BackgroundSprite, gui.Position, Color.White);
                spriteBatch.Draw(gui.BaseSprite, gui.BasePosition, Color.White);
                if (!gui.BaseLvlUp.IsMouseOver(mousePosition))
                    spriteBatch.Draw(gui.BaseLvlUp.Texture, gui.BaseLvlUp.Position, Color.White);
                else
                    spriteBatch.Draw(gui.BaseLvlUp.TextureOver, gui.BaseLvlUp.PositionOver, Color.White);
                spriteBatch.Draw(gui.GoldSprite, gui.GoldPosition, Color.White);
                spriteBatch.DrawString(gui.Font2, cgame.GetMyPlayer(myName).Gold.ToString(), gui.GoldPosition + new Vector2(50, 0), Color.Yellow);

                spriteBatch.Draw(gui.LiveSprite, gui.LivePosition, Color.White);
                spriteBatch.DrawString(gui.Font2, cgame.GetMyPlayer(myName).Live.ToString(), gui.LivePosition + new Vector2(50, 0), Color.Red);

                spriteBatch.Draw(gui.KillSprite, gui.KillPosition, Color.White);
                spriteBatch.DrawString(gui.Font2, cgame.GetMyPlayer(myName).Kill.ToString(), gui.KillPosition + new Vector2(50, 0), Color.Black);

                spriteBatch.DrawString(gui.Font, gui.BaseString, gui.BaseStringPosition, Color.White);
                spriteBatch.DrawString(gui.Font2, gui.InfoPlayer, gui.InfoPlayerPosition, Color.White);

                if (!towerGui.Enabled) //sinon on affiche les info de la tour
                {

                    foreach (Bouton b in gui.TowersBoutons)
                    {
                        if (!b.Selected)
                            spriteBatch.Draw(b.Texture, b.Position, Color.White);
                        else
                            spriteBatch.Draw(b.Texture, b.Position, Color.Violet);
                    }

                    switch (buildingState)
                    {
                        case Enumeration.BuildingState.Aoe:
                            spriteBatch.DrawString(gui.Font3, aoeTowers[0].ToString(), new Vector2(685, (9 * Config.APP_CELLS_SIZE) + 75), Color.White);
                            break;
                        case Enumeration.BuildingState.Attaque:
                            spriteBatch.DrawString(gui.Font3, attaqueTowers[0].ToString(), new Vector2(685, (9 * Config.APP_CELLS_SIZE) + 75), Color.Red);
                            break;
                        case Enumeration.BuildingState.Freeze:
                            spriteBatch.DrawString(gui.Font3, freezeTowers[0].ToString(), new Vector2(685, (9 * Config.APP_CELLS_SIZE) + 75), Color.Blue);
                            break;
                        case Enumeration.BuildingState.Poison:
                            spriteBatch.DrawString(gui.Font3, poisonTowers[0].ToString(), new Vector2(685, (9 * Config.APP_CELLS_SIZE) + 75), Color.Green);
                            break;
                        case Enumeration.BuildingState.Speed:
                            spriteBatch.DrawString(gui.Font3, speedTowers[0].ToString(), new Vector2(685, (9 * Config.APP_CELLS_SIZE) + 75), Color.Yellow);
                            break;
                    }

                }
                else
                {
                    spriteBatch.DrawString(gui.Font3, towerGui.Name, gui.Box3Position + new Vector2(260, 5), Color.Salmon);

                    spriteBatch.DrawString(gui.Font3, "Level :", gui.Box3Position + new Vector2(5, 30), Color.Tomato);
                    spriteBatch.DrawString(gui.Font3, towerGui.Level.ToString(),
                        gui.Box3Position + new Vector2(140, 30), Color.White);
                    spriteBatch.DrawString(gui.Font3, "Damage :", gui.Box3Position + new Vector2(5, 50), Color.Tomato);
                    spriteBatch.DrawString(gui.Font3, towerGui.Damage.ToString(),
                        gui.Box3Position + new Vector2(140, 50), Color.White);
                    spriteBatch.DrawString(gui.Font3, "Attack Speed :", gui.Box3Position + new Vector2(5, 70), Color.Tomato);
                    spriteBatch.DrawString(gui.Font3, towerGui.AttaqueSpeed.ToString(),
                        gui.Box3Position + new Vector2(140, 70), Color.White);

                    spriteBatch.DrawString(gui.Font3, towerGui.Comment, gui.Box3Position + new Vector2(10, 100), Color.Wheat);

                    if (cgame.GetMyPlayer(myName).FocusedTower.Level < 6)
                    {

                        if (!towerGui.TowerLvlUp.IsMouseOver(mousePosition))
                        {
                            spriteBatch.Draw(towerGui.TowerLvlUp.Texture, towerGui.TowerLvlUp.Position, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(towerGui.TowerLvlUp.TextureOver, towerGui.TowerLvlUp.PositionOver, Color.White);
                            spriteBatch.DrawString(gui.Font3, "Level Up", gui.Box3Position + new Vector2(260, 90), Color.Yellow);

                            spriteBatch.DrawString(gui.Font3, "-> " + (towerGui.Level + 1).ToString(), gui.Box3Position + new Vector2(180, 30), Color.Yellow);
                            spriteBatch.DrawString(gui.Font3, "-> " + towerGui.DamageNext.ToString(), gui.Box3Position + new Vector2(180, 50), Color.Yellow);
                            spriteBatch.DrawString(gui.Font3, "-> " + towerGui.AttaqueSpeedNext.ToString(), gui.Box3Position + new Vector2(180, 70), Color.Yellow);
                        }

                        spriteBatch.DrawString(gui.Font3, towerGui.PriceNext + " g", gui.Box3Position + new Vector2(260, 70), Color.Yellow);

                    }
                }

                //spriteBatch.DrawString(gui.Font3, "toto", new Vector2(70, 70), new Color(255, 255, 0, 0), 0, Vector2.Zero, 2, SpriteEffects.None, 0);

                spriteBatch.DrawString(gui.Font3, cgame.GetMyPlayer(myName).BaseLevel.ToString(), gui.BasePosition + new Vector2(50, 50), Color.DarkOrange, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

                if (gui.BaseLvlUp.IsMouseOver(mousePosition))
                {
                    spriteBatch.DrawString(gui.Font3, Base.GetInstance().BasePrice[cgame.GetMyPlayer(myName).BaseLevel - 1].ToString() + "g", gui.BasePosition + new Vector2(140,100), Color.Yellow);

                }

                #endregion


                #region Draw Towers
                try
                {
                    foreach (Tower tower in cgame.GetMyPlayer(myName).Towers)
                    {
                        //spriteBatch.Draw(tower.Sprite, tower.Position + new Vector2(32, 32), tower.Rect, Color.White, tower.Rotate, new Vector2(32, 32), 1, SpriteEffects.None, 0);

                        //foreach (Missile missile in tower.Missiles)
                        //{
                        //    spriteBatch.Draw(missile.Sprite, missile.Position, Color.White);
                        //}

                        Texture2D towerSprite = new Texture2D(this.graphicsDevice, 1, 1);
                        switch (tower.TowerState)
                        {
                            case Enumeration.BuildingState.Aoe:
                                towerSprite = Sprites.towerAoe[tower.Level - 1];
                                break;
                            case Enumeration.BuildingState.Attaque:
                                towerSprite = Sprites.towerAttaque[tower.Level - 1];
                                break;
                            case Enumeration.BuildingState.Freeze:
                                towerSprite = Sprites.towerFreeze[tower.Level - 1];
                                break;
                            case Enumeration.BuildingState.Poison:
                                towerSprite = Sprites.towerPoison[tower.Level - 1];
                                break;
                            case Enumeration.BuildingState.Speed:
                                towerSprite = Sprites.towerSpeed[tower.Level - 1];
                                break;
                        }
                        spriteBatch.Draw(towerSprite, tower.Position + new Vector2(32, 32), tower.Rect, Color.White, tower.Rotate, new Vector2(32, 32), 1, SpriteEffects.None, 0);
                        foreach (Missile missile in tower.Missiles)
                        {
                            spriteBatch.Draw(missile.Sprite, missile.Position, Color.White);
                        }
                    }
                }
                catch (Exception e)
                {

                }

                try
                {
                    foreach (Player player in otherPlayers)
                    {
                        foreach (Tower tower in player.Towers)
                        {
                            //spriteBatch.Draw(tower.Sprite, tower.Position + new Vector2(32, 32), tower.Rect, Color.White, tower.Rotate, new Vector2(32, 32), 1, SpriteEffects.None, 0);

                            //foreach (Missile missile in tower.Missiles)
                            //{
                            //    spriteBatch.Draw(missile.Sprite, missile.Position, Color.White);
                            //}

                            Texture2D towerSprite = new Texture2D(this.graphicsDevice, 1, 1);
                            switch (tower.TowerState)
                            {
                                case Enumeration.BuildingState.Aoe:
                                    towerSprite = Sprites.towerAoe[tower.Level - 1];
                                    break;
                                case Enumeration.BuildingState.Attaque:
                                    towerSprite = Sprites.towerAttaque[tower.Level - 1];
                                    break;
                                case Enumeration.BuildingState.Freeze:
                                    towerSprite = Sprites.towerFreeze[tower.Level - 1];
                                    break;
                                case Enumeration.BuildingState.Poison:
                                    towerSprite = Sprites.towerPoison[tower.Level - 1];
                                    break;
                                case Enumeration.BuildingState.Speed:
                                    towerSprite = Sprites.towerSpeed[tower.Level - 1];
                                    break;
                            }
                            spriteBatch.Draw(towerSprite, tower.Position + new Vector2(32, 32), tower.Rect, new Color(255, 255, 255, 150), tower.Rotate, new Vector2(32, 32), 1, SpriteEffects.None, 0);
                            foreach (Missile missile in tower.Missiles)
                            {
                                spriteBatch.Draw(missile.Sprite, missile.Position, Color.White);
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                    throw;
                }


                #endregion

                #region Draw OverAll

                foreach (FloatingMessage fm in fms)
                {
                    spriteBatch.DrawString(gui.Font3, fm.Text, fm.Position, fm.Color, 0, Vector2.Zero, fm.Size, SpriteEffects.None, 0);
                }

                if (youLose)
                {
                    spriteBatch.Draw(textureYouLose, new Vector2(300, 300), Color.White);
                }

                #endregion

                spriteBatch.End();


                base.Draw(gameTime);
            }
            else //draw loading screen
            {
                if (loadingString != null)
                {
                    graphicsDevice.Clear(Color.SlateGray);
                    spriteBatch.Begin();
                    spriteBatch.Draw(loadingString, new Vector2(300, 300), Color.White);
                    spriteBatch.End();
                }
            }
        }
    }
}
