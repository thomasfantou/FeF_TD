using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeF_TD
{
    public static class Config
    {
        public const String IMG_GAME = @"Content\Sprites\Game\";
        public const String IMG_MOBS = @"Content\Sprites\Mobs\";
        public const String IMG_SYS = @"Content\Sprites\System\";
        public const String IMG_TOWER = @"Content\Sprites\Towers\";
        public const String IMG_STRING = @"Content\Sprites\System\Strings\";
        public const String FONT = @"Content\Fonts\";
        public const String DATAS = @"Content\Datas\";
        public const String MUSICS = @"Content\Musics\";

        //public static String gameName;
        //public static String playerName;
        //public static int maximumPlayer;

        public const String APP_IDENTIFIER = "FEFTDID";
        public const int APP_PORT = 14242;

        public const String GAME_LIST_HOST = "Host";
        public const String GAME_LIST_GAME = "Game name";
        public const String GAME_LIST_PLAYERS = "Players";
        public const String GAME_LIST_TIME = "Creation date";


        public const String GAME_GUI_BASE = "Upgrade your base \nto increase\nbuilding\npossibilities";
        public const String GAME_GUI_PLAYER = "Player Informations";
        public const String GAME_GUI_TOWER_AOE = "This turrent is AOE";
        public const String GAME_SCREEN_WAVE_MSG = "Time before next wave : ";
        public const String Game_SCREEN_WAVE_MSG2 = "Mobs left : ";

        public const float WAVE_TIMER = 20000; //ms
        public const float WAVE_POP_TIMER = 1000; //ms
        public const int WAVE_MOB_COUNT = 40;
        public const int WAVES_COUNT = 40;
        public const float MOB_TIMER_ANIM = 800;
        public const int APP_CELLS_SIZE = 64;

        public const int GAME_PLAYER_INITIAL_LIVES = 30;
        public const int GAME_PLAYER_INITIAL_GOLD = 25000;

    }
}
