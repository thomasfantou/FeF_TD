using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;

namespace FeF_TD
{
    public static class Client
    {
        private static NetClient client;
        private static Thread listenerThread;
        private static List<User> hosts;
        private static String hostClientName;
        private static String hostGameName;
        private static System.Net.IPEndPoint hostEndPoint;
        private static bool lockHosts;

        private static bool isStarted;

        public static bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }

        public static NetClient GetClient
        {
            get { return Client.client; }
            set { Client.client = value; }
        }

        public static void Update()
        {
            if (isStarted)
            {
                //NetIncomingMessage msg;
                //msg = client.ReadMessage();
                //if (msg != null)
                //{
                //    switch (msg.MessageType)
                //    {
                //        case NetIncomingMessageType.DebugMessage:
                //        case NetIncomingMessageType.ErrorMessage:
                //        case NetIncomingMessageType.WarningMessage:
                //        case NetIncomingMessageType.VerboseDebugMessage:
                //            string text = msg.ReadString();
                //            break;
                //        case NetIncomingMessageType.StatusChanged:
                //            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                //            string reason = msg.ReadString();

                //            break;
                //        case NetIncomingMessageType.Data:
                //            string chat = msg.ReadString();
                //            break;
                //        default:

                //            break;
                //    }
                //}
            }

        }

        public static void Start()
        {
            try
            {
                if (!isStarted)
                {
                    NetPeerConfiguration config = new NetPeerConfiguration(Config.APP_IDENTIFIER);
                    config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

                    client = new NetClient(config);
                    client.Start();



                    listenerThread = new Thread(ClientListener);
                    listenerThread.Start();

                    isStarted = true;

                    hosts = new List<User>();
                    lockHosts = false;
                }
            }
            catch (Exception e)
            {

            }
        }

        public static void DiscoverNetwork()
        {
            client.DiscoverLocalPeers(Config.APP_PORT);
        }

        public static void Stop()
        {
            if (isStarted)
            {
                isStarted = false;
            }
        }

        private static void ClientListener()
        {
            while (Program.isRunning)
            {
                NetIncomingMessage im;
                while ((im = client.ReadMessage()) != null)
                {
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            String gameName = im.ReadString();
                            String hostName = im.ReadString();
                            String players = im.ReadString();
                            String date = im.ReadString();
                            AddToHostList(im.SenderEndpoint, hostName, gameName);


                            SGamesList.AddRow(gameName, hostName, players, date);
                            break;
                        case NetIncomingMessageType.Data:
                            DataReader(im);

                            break;
                    }
                }
            }
        }

        private static void DataReader(NetIncomingMessage im)
        {
            switch (im.ReadString())
            {
                case "PlayerRoom":
                    ClientPlayerRoom.GetInstance().UpdateClientPlayerRoom(im);
                    break;
                case "LaunchGame":
                    ClientPlayerRoom.GetInstance().LaunchGameEvent();
                    break;
                case "AllInitDone":
                    ClientGame.GetInstance().StartGameEvent();
                    break;
                case "SendPlayerUpdated":
                    ClientGame.GetInstance().InUpdatePlayerList(im);
                    break;
                case "NextWaveCalled":
                    ClientGame.GetInstance().InNextWaveCalled(im);
                    break;
                case "StunMob":
                    ClientGame.GetInstance().InStunMob(im);
                    break;
            }
        }

        private static void AddToHostList(System.Net.IPEndPoint sep, String hostname, String gamename)
        {
            bool alreadyInList = false;
            foreach (User host in hosts)
            {
                if (host.EndPoint == sep)
                    alreadyInList = true;
            }
            if (!alreadyInList)
            {
                User host = new User();
                host.EndPoint = sep;
                host.Name = hostname;
                host.Gamename = gamename;
                while (lockHosts)
                {

                }
                hosts.Add(host);
            }
        }

        public static void ConnectTo(String gameName, String hostName, System.Net.IPEndPoint ep = null, String playerName = null)
        {
            bool connectSucceed = false;
            if (ep != null)
            {
                client.Connect(ep);
                connectSucceed = true;
            }
            else
            {
                foreach (User host in hosts)
                {
                    if (host.Gamename == gameName && host.Name == hostName)
                    {
                        client.Connect(host.EndPoint);
                        connectSucceed = true;
                    }
                }
            }
            if (connectSucceed)
            {
                tempGameName = gameName;
                if (playerName != null)
                    tempPlayerName = playerName;
                else
                    tempPlayerName = hostName;
                Thread msgThread = new Thread(SendIDMessage);
                msgThread.Start();
            }
        }

        private static String tempGameName;
        private static String tempPlayerName;

        private static void SendIDMessage()
        {
            Thread.Sleep(500);
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write("Identified myself");
            msg.Write(tempGameName);
            msg.Write(tempPlayerName);

            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public static void ConnectAsHost(String playerName, String gameName)
        {
            hostClientName = playerName;
            hostGameName = gameName;
            Client.Start();
            Client.DiscoverNetwork();
            Thread myCheckoutTread = new Thread(CheckoutHost);
            myCheckoutTread.Start();
        }

        private static void CheckoutHost()
        {
            bool c = true;
            while (c)
            {
                if (hosts != null)
                {
                    lockHosts = true;
                    foreach (User host in hosts)
                    {
                        if (host.Gamename == hostGameName && host.Name == hostClientName)
                        {
                            hostEndPoint = host.EndPoint;
                            Client.ConnectTo(hostGameName, hostClientName, hostEndPoint);
                            c = false;
                            break;
                        }
                    }
                    lockHosts = false;
                    Thread.Sleep(500);

                }
            }
        }

        
    }
}
