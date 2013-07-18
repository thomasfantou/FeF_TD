using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using System.Threading;

namespace FeF_TD
{
    public static class Server
    {
        private static NetServer server;
        private static Thread listenerThread;
        private static List<User> clients;
        private static int playerCount;
        private static Thread playerRoomThread;
        private static DateTime date;
        private static bool isStarted;
        private static String hostName;
        private static String gameName;
        private static int maxPlayer;

        private static bool gameStarted = false;

        public static int PlayerCount
        {
            get { return Server.playerCount; }
            set { Server.playerCount = value; }
        }


        public static DateTime Date
        {
            get { return Server.date; }
            set { Server.date = value; }
        }


        public static bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }


        public static String HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public static String GameName
        {
            get { return gameName; }
            set { gameName = value; }
        }

        public static int MaxPlayer
        {
            get { return maxPlayer; }
            set { maxPlayer = value; }
        }
        
        public static void Update()
        {
            if (isStarted)
            {
                if (!gameStarted)
                {
                    if (clients != null && clients.Count != 0)
                    {
                        bool allInitDone = true;
                        bool breaked = false;
                        foreach (User c in clients)
                        {
                            if (c.InitDone == false)
                            {
                                allInitDone = false;
                                breaked = true;
                                break;
                            }
                        }
                        if (!breaked)
                        {
                            if (allInitDone)
                            {
                                StartGame();
                                gameStarted = true;
                            }
                        }
                    }
                }
            }
        }

        public static void Start()
        {
            try
            {
                if (!isStarted)
                {
                    NetPeerConfiguration config = new NetPeerConfiguration(Config.APP_IDENTIFIER);
                    config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                    config.Port = Config.APP_PORT;
                    //config.LocalAddress = System.Net.IPAddress.Parse("10.255.253.126");

                    // create and start server
                    server = new NetServer(config);
                    server.Start();
                    listenerThread = new Thread(ServerListener);
                    listenerThread.Start();
                    Server.isStarted = true;
                    
                    playerCount = 1;
                    clients = new List<User>();
                }
            }
            catch (Exception e)
            {

            }
        }

        public static void Stop()
        {
            try
            {
                if (isStarted)
                {
                    server.Shutdown("Application exiting");
                    isStarted = false;
                    listenerThread.Abort();
                }
            }
            catch (Exception e)
            {

            }
        }

        private static void ServerListener()
        {
            while (Program.isRunning)
            {
                NetIncomingMessage im;
                while ((im = server.ReadMessage()) != null)
                {
                    // handle incoming message
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage infos = server.CreateMessage();
                            infos.Write(GameName);
                            infos.Write(HostName);
                            infos.Write(playerCount + "/" + maxPlayer);
                            infos.Write(date.Hour + ":" + date.Minute);

                            server.SendDiscoveryResponse(infos, im.SenderEndpoint);
                            
                            break;
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            break;
                        case NetIncomingMessageType.StatusChanged: //Un client se connect

                            AddToPlayersList(im.SenderEndpoint);
                            break;
                        case NetIncomingMessageType.Data:
                            DataReader(im);
                            
                            break;
                        default:
                            break;
                    }
                }
                Thread.Sleep(10);
            }
        }

        private static void DataReader(NetIncomingMessage im)
        {
            switch (im.ReadString())
            {
                case "Identified myself":
                    AddToPlayersList(im.ReadString(), im.ReadString(), im.SenderEndpoint);
                    break;
                case "InitDone":
                    foreach (User client in clients)
                    {
                        if (client.EndPoint.Address.Equals(im.SenderEndpoint.Address) && client.EndPoint.Port.Equals(im.SenderEndpoint.Port))
                        {
                            client.InitDone = true;
                        }
                    }
                    break;
                case "UpdatePlayer":
                    ServerGame.SendPlayerUpdated(server, im);
                    break;
                case "NextWaveCalled":
                    ServerGame.SendNextWaveCalled(server);
                    break;
                case "StunMob":
                    ServerGame.SendStunMob(server, im);
                    break;
                case "FearMob":
                    ServerGame.SendFearMob(server, im);
                    break;
            }
        }

        public static List<User> GetPlayers()
        {
            return clients;
            
        }

        private static void AddToPlayersList(System.Net.IPEndPoint ep)
        {
            bool alreadyExist = false;
            foreach (User client in clients)
            {
                if (client.EndPoint == ep)
                {
                    alreadyExist = true;
                }
            }
            if (!alreadyExist)
            {
                User c = new User();
                c.EndPoint = ep;
                clients.Add(c);
            }
        }

        private static void AddToPlayersList(String gameName, String playerName, System.Net.IPEndPoint ep)
        {
            foreach (User client in clients)
            {
                if (client.EndPoint.Address.Equals(ep.Address) && client.EndPoint.Port.Equals(ep.Port))
                {
                    client.Name = playerName;
                    client.Gamename = gameName;
                }

            }
        }

        public static void StartPlayerRoomServer()
        {
            playerRoomThread = new Thread(PlayerRoomServer);
            playerRoomThread.Start();
        }

        public static void LaunchGame()
        {
            StopPlayerRoomServer();
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("LaunchGame");
            SendToAll(msg);
        }

        public static void StopPlayerRoomServer()
        {
            playerRoomThread.Abort();
        }

        private static void PlayerRoomServer()
        {
            while (Program.isRunning)
            {
                NetOutgoingMessage msg = server.CreateMessage();
                msg.Write("PlayerRoom");
                msg.Write(hostName);
                msg.Write(gameName);
                msg.Write(maxPlayer);

                foreach (User user in Server.GetPlayers())
                {
                    msg.Write(user.Name);
                }
                msg.Write("PlayerListEnd");

                SendToAll(msg);

                Thread.Sleep(1000);
            }
        }

        public static void SendToAll(NetOutgoingMessage msg)
        {
            foreach (NetConnection nc in server.Connections)
            {
                server.SendMessage(msg, nc, NetDeliveryMethod.ReliableOrdered);
            }
        }

        private static void StartGame()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("AllInitDone");

            SendToAll(msg);
        }
    }
}
