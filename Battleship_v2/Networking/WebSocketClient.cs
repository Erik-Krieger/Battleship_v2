using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship_v2.Networking
{
    public class WebSocketClient : NetworkPeer
    {
        /// <summary>
        /// The WebSocket-Client used for the connection.
        /// </summary>
        private ClientWebSocket webSocket { get; set; } = new ClientWebSocket();

        /// <summary>
        /// The default constructor
        /// </summary>
        public WebSocketClient() { }

        /// <summary>
        /// This will run the 
        /// </summary>
        private async void receiveMessage()
        {
            // Create a read buffer with a size of 4kb.
            byte[] aByteArray = new byte[4096];
            ArraySegment<byte> aBuffer = new ArraySegment<byte>(aByteArray);

            // Run the loop forever.
            while (true)
            {
                // Wait for message to be received.
                var aResponse = await webSocket.ReceiveAsync(aBuffer, default);
                
                // Check if the Message is not null
                if (!(aResponse is null))
                {
                    // Convert the Message to a String.
                    string aMessage = aBuffer.ToString();
                    // Add the String to the Message Queue.
                    addMessageToQueue(aMessage);
                }
            }
        }

        /// <summary>
        /// Used to send messages to the connected WebSocketServer.
        /// </summary>
        /// <param name="theMessage">The message to send</param>
        private async void sendMessageAsync(string theMessage)
        {
            var aByteArray = Encoding.UTF8.GetBytes(theMessage);
            await webSocket.SendAsync(new System.ArraySegment<byte>(aByteArray), WebSocketMessageType.Text, false, default);
        }

        public override void SendMessage(string theMessage)
        {
            sendMessageAsync(theMessage);
        }
        
        /// <summary>
        /// This is the Method used to start the WebSocketClient and it's thread.
        /// </summary>
        /// <param name="theHostname">The Hostname of the Server Machine, this can also be an IP-Address</param>
        public override void Connect(string theHostname)
        {
            var aUri = new Uri($"ws://{theHostname}");

            webSocket.ConnectAsync(aUri, default);

            m_NetworkThread = new Thread(new ThreadStart(receiveMessage));
            m_NetworkThread.Start();
        }
    }
}
