using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

/*
 *  Author Kighten
 */


namespace FeF_TD
{
    public class World
    {

        #region variables

        private Ground[,] _map;
        private string _name;
        public enum Ground
        {
            outside,
            inside,
            tborder,
            rborder,
            bborder,
            lborder,
            ltcorner,
            rtcorner,
            lbcorner,
            rbcorner,
            ltinnercorner,
            rtinnercorner,
            rbinnercorner,
            lbinnercorner,
            begining,
            ending
        }

        #endregion

        public static int[,] map;
        public static int width;
        public static int lenght;
        private Vector2 vector32 = new Vector2(-32, -32);


        #region properties

        public Ground[,] Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion



        #region methodes

        public World()
        {
            width = 16;
            lenght = 9;
            map = new int[width, lenght];
            _map = new Ground[width, lenght];
        }

        public void LoadMap(int player)
        {
            switch (player)
            {
                case 2:
                    StreamReader reader = new StreamReader("Content\\" + Config.DATAS + "Map2players.txt");


                    String line;
                    int nLine = 0;

                    line = reader.ReadLine();

                    while (line != null)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            map[i,nLine] = int.Parse(line[i].ToString());
                        }
                        line = reader.ReadLine();
                        nLine++;
                    }

                    

                    reader.Close();
                       

                    break;
            }

            //on met l'enum dans map
            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if(map[j,i] == 1)
                    {
                        //si on a un chemin sur la premiere ligne de gauche ou du haut, c'est le point d'entré
                        if (i == 0 || j == 0)
                            _map[j,i] = Ground.begining;
                        //si le chemin est sur la derniere ligne du bas ou a doite, c'est le point de sortie
                        else if (i == (lenght-1) || j == (width - 1))
                            _map[j,i] = Ground.ending;
                        else
                            _map[j,i] = Ground.inside;
                    }
                    else
                    {
                        try
                        {
                            if (map[j, i - 1] == 1)
                            {
                                if (map[j - 1, i] == 1)
                                    _map[j, i] = Ground.rbinnercorner;
                                else if (map[j + 1, i] == 1)
                                    _map[j, i] = Ground.lbinnercorner;
                                else
                                    _map[j, i] = Ground.bborder;
                            }
                            else if (map[j, i + 1] == 1)
                            {
                                if (map[j - 1, i] == 1)
                                    _map[j, i] = Ground.rtinnercorner;
                                else if (map[j + 1, i] == 1)
                                    _map[j, i] = Ground.ltinnercorner;
                                else
                                    _map[j, i] = Ground.tborder;
                            }

                            

                            else if (map[j - 1, i] == 1)
                                _map[j, i] = Ground.rborder;

                            else if (map[j + 1, i] == 1)
                                _map[j, i] = Ground.lborder;

                            else if (map[j - 1, i - 1] == 1)
                                _map[j, i] = Ground.rbcorner;
                            else if (map[j + 1, i - 1] == 1)
                                _map[j, i] = Ground.lbcorner;
                            else if (map[j - 1, i + 1] == 1)
                                _map[j, i] = Ground.rtcorner;
                            else if (map[j + 1, i + 1] == 1)
                                _map[j, i] = Ground.ltcorner;
                            else
                                _map[j, i] = Ground.outside;
                        }
                        catch (Exception e) //Si on est sur les premiere/derniere ligne et colonne
                        {
                            _map[j, i] = Ground.outside;
                            if(j > 0)
                                if (map[j - 1, i] == 1)
                                    _map[j, i] = Ground.rborder;
                            if(j < 15)
                                if (map[j + 1, i] == 1)
                                    _map[j, i] = Ground.lborder;
                            if (i > 0)
                                if (map[j, i - 1] == 1)
                                    _map[j, i] = Ground.tborder;
                            if (i < 8)
                                if (map[j, i + 1] == 1)
                                    _map[j, i] = Ground.bborder;
                        }
                    }
                }
          

            }
        }

        public static Vector2 GetCurrentBlock(Vector2 mousePosition)
        {
            Vector2 currentBlock;

            currentBlock.X = ((int)mousePosition.X / (int)Config.APP_CELLS_SIZE) * Config.APP_CELLS_SIZE;
            currentBlock.Y = ((int)mousePosition.Y / (int)Config.APP_CELLS_SIZE) * Config.APP_CELLS_SIZE;

            return currentBlock;
        }

        public Vector2 GetCurrentTowerPosition(Vector2 mousePosition)
        {
            Vector2 currentBlock;

            currentBlock = GetCurrentBlock(mousePosition + vector32);
            currentBlock.X += 32;
            currentBlock.Y += 32;

            return currentBlock;
        }

        public bool IsAvailablePosition(Vector2 mousePosition, List<Tower> towers)
        {
            try
            {
                Vector2 currentBlock = GetCurrentTowerPosition(mousePosition);
                Vector2 indexes;
                indexes.X = currentBlock.X / Config.APP_CELLS_SIZE;
                indexes.Y = currentBlock.Y / Config.APP_CELLS_SIZE;
                bool alreadyTower = false;

                foreach (Tower t in towers)
                {
                    if (new Rectangle((int)t.Position.X, (int)t.Position.Y, Config.APP_CELLS_SIZE, Config.APP_CELLS_SIZE).Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)))
                    {
                        alreadyTower = true;
                    }
                }

                if (map[(int)indexes.X, (int)indexes.Y] == 0 &&
                    map[(int)indexes.X + 1, (int)indexes.Y] == 0 &&
                    map[(int)indexes.X, (int)indexes.Y + 1] == 0 &&
                    map[(int)indexes.X + 1, (int)indexes.Y + 1] == 0 &&
                    !alreadyTower)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        #endregion

    }
}
