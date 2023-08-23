using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Battleship_v2.Services
{
    public class NetworkService
    {
        public const int PORT = 1776;

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
            TcpListener server = new TcpListener(IPAddress.Loopback, PORT);

            server.Start();

            Debug.WriteLine($"Server has started on 127.0.0.1:{PORT}.\nWaiting for a connection...");

            TcpClient client = server.AcceptTcpClient();

            Debug.WriteLine("A client connected");

            Stream = client.GetStream();

            while ( client.Available < 3 )
            {
                // wait for enough bytes to be available
                Thread.Sleep(50);
            }

            byte[] bytes = new byte[client.Available];

            Stream.Read(bytes, 0, bytes.Length);

            // translate bytes of request to string
            string data = Encoding.UTF8.GetString(bytes);

            if (Regex.IsMatch(data, "^GET"))
            {
                const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

                byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                    + "Connection: Upgrade" + eol
                    + "Upgrade: websocket" + eol
                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                        System.Security.Cryptography.SHA1.Create().ComputeHash(
                            Encoding.UTF8.GetBytes(
                            new System.Text.RegularExpressions.Regex("Sec-WebSocket-Key:(.*)").Match(data).Groups[1].Value.Trim()
                                + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                            )
                        )
                    ) + eol
                    + eol);
                
                Stream.Write(response, 0, response.Length);
            }
        }

        public async void RecieveLoop()
        {
            bool keepGoing = true;

            do
            {
                var b = new byte[1024];
                var i = await Stream.ReadAsync(b, 0, 1024);
                var s = b.ToString();
            }
            while (keepGoing);
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
