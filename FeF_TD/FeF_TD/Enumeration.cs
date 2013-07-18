using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeF_TD
{
    public static class Enumeration
    {
        public enum BuildingState
        {
            None,
            Aoe,
            Attaque,
            Freeze,
            Poison,
            Speed
        }

        public enum Screen
        {
            MainMenu,
            GameCreation,
            GameList,
            Server,
            PlayerRoom,
            Game
        }
    }
}
