using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FeF_TD
{
    public static class ServerGame
    {
        public static void SendPlayerUpdated(NetServer server, NetIncomingMessage im)
        {
            try
            {
                NetOutgoingMessage msg = server.CreateMessage();
                msg.Write("SendPlayerUpdated");
                msg.Write(im.ReadString());
                msg.Write(im.ReadInt32());
                msg.Write(im.ReadInt32());
                msg.Write(im.ReadInt32());
                int towerCount = im.ReadInt32();
                msg.Write(towerCount);
                for (int i = 0; i < towerCount; i++)
                    msg.Write(im.ReadString());
                Server.SendToAll(msg);
            }
            catch (Exception)
            {
                
                throw;
            }

            
        }

        public static void SendNextWaveCalled(NetServer server)
        {
            try
            {
                NetOutgoingMessage msg = server.CreateMessage();
                msg.Write("NextWaveCalled");
                Server.SendToAll(msg);
            }
            catch (Exception)
            {

            }
        }

        public static void SendStunMob(NetServer server, NetIncomingMessage im)
        {
            try
            {
                NetOutgoingMessage msg = server.CreateMessage();
                msg.Write("StunMob");
                msg.Write(im.ReadInt32());
                Server.SendToAll(msg);
            }
            catch (Exception e)
            {

            }
        }

        public static void SendFearMob(NetServer server, NetIncomingMessage im)
        {
            try
            {
                NetOutgoingMessage msg = server.CreateMessage();
                msg.Write("FearMob");
                msg.Write(im.ReadInt32());
                Server.SendToAll(msg);
            }
            catch (Exception e)
            {

            }
        }
    }
}
