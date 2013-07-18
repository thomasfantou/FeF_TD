using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FeF_TD
{
    public class ClientPlayerRoom
    {
        public String HostName { get; set; }
        public String GameName { get; set; }
        public int PlayerCount { get; set; }
        public List<String> PlayersName { get; set; }

        static ClientPlayerRoom instance = null;
        static readonly object padlock = new object();
        
        public delegate void LaunchGameHandler();
        public event LaunchGameHandler LaunchGame;

        private ClientPlayerRoom()
        {
            HostName = "";
            GameName = "";
            PlayerCount = 0;
            PlayersName = new List<string>();

        }

        public static ClientPlayerRoom GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ClientPlayerRoom();
                }
                return instance;
            }

        }

        public void LaunchGameEvent()
        {
            LaunchGame();
        }

        public void UpdateClientPlayerRoom(NetIncomingMessage im)
        {
            GameName = im.ReadString();
            HostName = im.ReadString();
            PlayerCount = im.ReadInt32();
            String playerName;
            PlayersName.Clear();
            while ((playerName = im.ReadString()) != "PlayerListEnd")
            {
                PlayersName.Add(playerName);
            }

        }

    }
}
