using System;
using System.Threading;

namespace FeF_TD
{
#if WINDOWS || XBOX
    static class Program
    {
        static GameStateManagementGame game;
        public static bool isRunning;

        static void Main(string[] args)
        {
            RunGame();
        }

        public static void RunGame()
        {
            using (game = new GameStateManagementGame())
            {
                isRunning = true;
                game.Run();
                isRunning = false;
            }
        }

        public static void ExitGame()
        {
            game.Exit();
        }


    }
#endif
}

