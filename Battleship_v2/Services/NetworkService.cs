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
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseServer() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theHostname"></param>
        public async void JoinServer(string theHostname)
        {
            Uri uri = new Uri($"ws://{theHostname}");
            ClientWebSocket ws = new ClientWebSocket();
            await ws.ConnectAsync(uri, default);

            var bytes = new byte[1024];
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(bytes), default);
            string res = Encoding.UTF8.GetString(bytes, 0, result.Count);

            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client Closed", default);
        }
    }
}
