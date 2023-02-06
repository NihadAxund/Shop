using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Network
{
    static class Connection_Key
    {
        public const int PORT = 27001;
        public static IPAddress IPADDRESS = IPAddress.Loopback;
        public static void New_IP(string ip)
        {
            IPADDRESS = IPAddress.Parse(ip);
        }
    }
}
