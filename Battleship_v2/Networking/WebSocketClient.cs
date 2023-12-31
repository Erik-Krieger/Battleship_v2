﻿using Battleship_v2.Services;
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
            byte[] aByteArray = new byte[1024];
            ArraySegment<byte> aBuffer = new ArraySegment<byte>(aByteArray);

            // Run the loop forever.
            while (true)
            {
                // Wait for message to be received.
                /*try
                {*/
                var aResponse = await webSocket.ReceiveAsync(aBuffer, m_CancelToken.Token);
                /*}
                catch ()
                {

                }*/
                
                // Check if the Message is not null
                if (!(aResponse is null))
                {
                    // Convert the Message to a String.
                    string aMessage = DecodeString(aByteArray);
                    // Add the String to the Message Queue.
                    addMessageToQueue(aMessage);
                }

                Array.Clear(aByteArray, 0, aByteArray.Length);
            }
        }

        /// <summary>
        /// Used to send messages to the connected WebSocketServer.
        /// </summary>
        /// <param name="theMessage">The message to send</param>
        private async void sendMessageAsync(string theMessage)
        {
            var aByteArray = EncodeString(theMessage);
            await webSocket.SendAsync(new System.ArraySegment<byte>(aByteArray), WebSocketMessageType.Text, false, default);
        }

        public override void SendMessage(string theMessage)
        {
            sendMessageAsync(theMessage);
        }
        
        private async void connect(string theHostname)
        {
            var aUri = new Uri($"ws://{theHostname}:{NetworkService.PORT}");

            await webSocket.ConnectAsync(aUri, m_CancelToken.Token);

            Debug.WriteLine("Connected");

            m_NetworkThread = new Thread(new ThreadStart(receiveMessage));
            m_NetworkThread.Start();

            Debug.WriteLine("Started seperate thread.");
        }

        /// <summary>
        /// This is the Method used to start the WebSocketClient and it's thread.
        /// </summary>
        /// <param name="theHostname">The Hostname of the Server Machine, this can also be an IP-Address</param>
        public override void Connect(string theHostname)
        {
            connect(theHostname);
        }
    }
}
