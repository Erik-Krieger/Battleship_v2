using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Services
{
    public class NetworkService
    {
        public static NetworkService Instance { get; set; } = new NetworkService();

        private WebSocket Server { get; set; }

        private NetworkService() { }

        /*public bool OpenServer(int thePort = 22)
        {
            Server = new WebSocket();
            return true;
        }*/

        public void CloseServer() { }

        public bool JoinServer( string theHostname )
        {
            return true;
        }
    }
}
