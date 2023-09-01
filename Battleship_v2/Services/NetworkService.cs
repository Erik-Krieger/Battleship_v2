using Battleship_v2.Networking;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace Battleship_v2.Services
{
    public class NetworkService
    {
        /// <summary>
        /// 
        /// </summary>
        public const int PORT = 1776;

        /// <summary>
        /// 
        /// </summary>
        public NetworkPeer NetworkPeer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NetworkStream Stream { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        private object m_Lock = new object();

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static NetworkService Instance { get; set; } = new NetworkService();

        /// <summary>
        /// 
        /// </summary>
        private NetworkService() { }

        /// <summary>
        /// 
        /// </summary>
        public void OpenServer()
        {
            NetworkPeer = new WebSocketServer();

            ((WebSocketServer)NetworkPeer).Connect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theHostname"></param>
        public async void JoinServer(string theHostname)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                NetworkPeer = new WebSocketClient();
            }));

            NetworkPeer.Connect(theHostname);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            // If there is no network peer, just abort here.
            if (NetworkPeer is null)
            {
                return;
            }

            // Shutdown the Network Thread.
            NetworkPeer.Stop();
        }
    }
}
