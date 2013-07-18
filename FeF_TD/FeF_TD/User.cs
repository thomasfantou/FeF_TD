using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FeF_TD
{
    public class User
    {
        public System.Net.IPEndPoint EndPoint { get; set; }
        public String Name { get; set; }
        public String Gamename { get; set; }
        public bool InitDone { get; set; }

        public User()
        {
            InitDone = false;
        }
    }
}
