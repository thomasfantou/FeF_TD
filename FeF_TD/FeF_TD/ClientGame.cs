using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;

namespace FeF_TD
{
    public class ClientGame
    {
        static ClientGame instance = null;
        static readonly object padlock = new object();


        public delegate void StartGameHandler();
        public event StartGameHandler StartGame;

        public List<Player> players;

        private int oldPlayerGold;
        private int oldPlayerLive;
        private int oldPlayerKill;

        private string currentPlayerName;


        private ClientGame()
        {
            List<String> names = ClientPlayerRoom.GetInstance().PlayersName;
            players = new List<Player>();
            foreach (String name in names)
            {
                players.Add(new Player(name));
            }
        }

        public static ClientGame GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ClientGame();
                }
                return instance;
            }

        }

        public void InitDone()
        {
            NetOutgoingMessage msg = Client.GetClient.CreateMessage();
            msg.Write("InitDone");
            Client.GetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void StartGameEvent()
        {
            StartGame();
        }

        public Player GetMyPlayer(String playerName)
        {
            return players.Find(o => o.Name == playerName);
        }

        public void OutUpdatePlayer(String playerName)
        {
            currentPlayerName = playerName;

            Thread thread = new Thread(OutUpdatePlayerThread);
            thread.Start();
        }

        private void OutUpdatePlayerThread()
        {
            Player player = players.Find(o => o.Name == currentPlayerName);
            //si il y a eu un changement sur le player, on update
            //if (oldPlayerGold != player.Gold
            //    || oldPlayerKill != player.Kill
            //    || oldPlayerLive != player.Live)
            //{
                NetOutgoingMessage msg = Client.GetClient.CreateMessage();
                msg.Write("UpdatePlayer");
                msg.Write(player.Name);
                msg.Write(player.Kill);
                msg.Write(player.Gold);
                msg.Write(player.Live);

                msg.Write(player.Towers.Count);
                foreach (Tower tower in player.Towers)
                {
                    msg.Write(tower.GetInfos());
                }


                Client.GetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
                oldPlayerGold = player.Gold;
                oldPlayerKill = player.Kill;
                oldPlayerLive = player.Live;
            //}
        }

        public void OutNextWaveCalled()
        {
            try
            {
                NetOutgoingMessage msg = Client.GetClient.CreateMessage();
                msg.Write("NextWaveCalled");
                Client.GetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            }
            catch (Exception e)
            {

            }
        }

        public void OutStunMob(int idMob)
        {
            try
            {
                NetOutgoingMessage msg = Client.GetClient.CreateMessage();
                msg.Write("StunMob");
                msg.Write(idMob);
                Client.GetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            }
            catch (Exception e)
            {

            }
        }

        public void OutFearMob(int idMob)
        {
            try
            {
                NetOutgoingMessage msg = Client.GetClient.CreateMessage();
                msg.Write("FearMob");
                msg.Write(idMob);
                Client.GetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            }
            catch (Exception e)
            {

            }
        }

        public void InUpdatePlayerList(NetIncomingMessage im)
        {
            try
            {
                String name = im.ReadString();
                if (currentPlayerName != name)
                {

                    players.Find(o => o.Name == name).Kill = im.ReadInt32();
                    players.Find(o => o.Name == name).Gold = im.ReadInt32();
                    players.Find(o => o.Name == name).Live = im.ReadInt32();

                    //players.Find(o => o.Name == name).Towers.Clear();

                    int towerCount = im.ReadInt32();
                    for (int i = 0; i < towerCount; i++)
                    {
                        Tower tower = Tower.SetInfos(im.ReadString());
                        if (players.Find(o => o.Name == name).Towers.Find(o => o.Id == tower.Id) != null)
                        {
                            players.Find(o => o.Name == name).Towers.Find(o => o.Id == tower.Id).ToNewVersion(tower);
                        }
                        else
                        {
                            players.Find(o => o.Name == name).Towers.Add(tower);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                
                throw;
            }

        }

        public void InNextWaveCalled(NetIncomingMessage im)
        {
            try
            {
                Wave.GetInstance().Timer = 100;
                Wave.GetInstance().Bouton.Enabled = false;

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public void InStunMob(NetIncomingMessage im)
        {
            try
            {
                int idMob = im.ReadInt32();
                Wave.GetInstance().Mobs.Find(o => o.Id == idMob).Stun = true;
                Wave.GetInstance().Mobs.Find(o => o.Id == idMob).StunTimer = 1000;


            }
            catch (Exception e)
            {

                throw;
            }
        }

        public void InFearMob(NetIncomingMessage im)
        {
            try
            {
                int idMob = im.ReadInt32();
                Wave.GetInstance().Mobs.Find(o => o.Id == idMob).Fear();


            }
            catch (Exception e)
            {

                throw;
            }
        }

        public List<Player> GetOtherPlayersThan(String playerName)
        {
            List<Player> tempList = new List<Player>();

            foreach (Player player in players)
            {
                if (player.Name != playerName)
                    tempList.Add(player);
            }
            return tempList;
        }


    }
}
