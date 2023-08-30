using Battleship_v2.Enemies;
using Battleship_v2.Networking;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace Battleship_v2.Services
{
    public class NetworkService
    {
        public const int PORT = 1776;

        public NetworkPeer NetworkPeer { get; set; }

        public NetworkStream Stream { get; private set; } = null;

        private object m_Lock = new object();

        public TcpListener Listener
        {
            get
            {
                lock (m_Lock)
                {
                    if (m_Listener == null)
                    {
                        m_Listener = new TcpListener(IPAddress.Loopback, PORT);
                        m_Listener.Start();
                        Debug.WriteLine($"Server has started on {IPAddress.Loopback}:{PORT}.\nWaiting for a connection...");
                    }
                    return m_Listener;
                }
            }
        }
        private TcpListener m_Listener = null;

        public static NetworkService Instance { get; set; } = new NetworkService();

        private NetworkService() { }

        /// <summary>
        /// 
        /// </summary>
        public void OpenServer()
        {
            NetworkPeer = new WebSocketServer();
            NetworkPeer.Connect("");
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseServer() { }

        public void JoinServer(string theHostname)
        {
            NetworkPeer = new WebSocketClient();

            NetworkPeer.Connect(theHostname);
        }

        public void Close()
        {
            if (NetworkPeer is null)
            {
                return;
            }

            NetworkPeer.Stop();
        }
    }
}
