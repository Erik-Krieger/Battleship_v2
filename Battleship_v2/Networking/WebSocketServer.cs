using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Battleship_v2.Networking
{
    public class WebSocketServer : NetworkPeer
    {
        private TcpListener listener { get; set; } = new TcpListener(IPAddress.Loopback, PORT);

        private TcpClient m_TcpClient;

        private NetworkStream m_Stream;

        public WebSocketServer() { }

        public void Start()
        {
            listener.Start();
            Debug.WriteLine($"Server has started on {IPAddress.Loopback}:{PORT}, Waiting for a connection…");

            m_TcpClient = listener.AcceptTcpClient();
            Debug.WriteLine("A client connected.");

            NetworkStream m_Stream = m_TcpClient.GetStream();

            // enter to an infinite cycle to be able to handle every change in stream
            while (true)
            {
                // Check if there is Data in the stream.
                // If not sleep for 25ms to conserve energy.
                while (!m_Stream.DataAvailable)
                {
                    Thread.Sleep(25);
                }

                // Once the is something is something in the stream
                // Keep checking until it has 3 bytes.
                while (m_TcpClient.Available < 3) ; // match against "get"

                byte[] aByteArray = new byte[m_TcpClient.Available];
                m_Stream.Read(aByteArray, 0, aByteArray.Length);
                string s = Encoding.UTF8.GetString(aByteArray);

                if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
                {
                    Debug.WriteLine("=====Handshaking from client=====\n{0}", s);

                    // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
                    // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
                    // 3. Compute SHA-1 and Base64 hash of the new value
                    // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
                    string aSecureWebSocketKey = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                    aSecureWebSocketKey += "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                    byte[] aSecureWebSocketKeyHash = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(aSecureWebSocketKey));
                    string aSecureWebSocketKeyHashAsBase64 = Convert.ToBase64String(aSecureWebSocketKeyHash);

                    // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                    byte[] response = Encoding.UTF8.GetBytes(
                        "HTTP/1.1 101 Switching Protocols\r\n" +
                        "Connection: Upgrade\r\n" +
                        "Upgrade: websocket\r\n" +
                        "Sec-WebSocket-Accept: " + aSecureWebSocketKeyHashAsBase64 + "\r\n\r\n");

                    m_Stream.Write(response, 0, response.Length);

                    // Set the PeerConnected flag to true
                    PeerConnected = true;
                    // Reload the CommandManager, to activate the Play Button immediately.
                    CommandManager.InvalidateRequerySuggested();
                }
                else
                {
                    bool fin = (aByteArray[0] & 0b10000000) != 0,
                        mask = (aByteArray[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"
                    int opcode = aByteArray[0] & 0b00001111, // expecting 1 - text message
                        anOffset = 2;
                    ulong aMessageLength = (ulong)aByteArray[1] & 0b01111111;

                    if (aMessageLength == 126)
                    {
                        // bytes are reversed because websocket will print them in Big-Endian, whereas
                        // BitConverter will want them arranged in little-endian on windows
                        aMessageLength = BitConverter.ToUInt16(new byte[] { aByteArray[3], aByteArray[2] }, 0);
                        anOffset = 4;
                    }
                    else if (aMessageLength == 127)
                    {
                        // To test the below code, we need to manually buffer larger messages — since the NIC's autobuffering
                        // may be too latency-friendly for this code to run (that is, we may have only some of the bytes in this
                        // websocket frame available through client.Available).
                        aMessageLength = BitConverter.ToUInt64(new byte[] { aByteArray[9], aByteArray[8], aByteArray[7], aByteArray[6], aByteArray[5], aByteArray[4], aByteArray[3], aByteArray[2] }, 0);
                        anOffset = 10;
                    }

                    if (aMessageLength == 0)
                    {
                        Debug.WriteLine("msglen == 0");
                    }
                    else if (mask)
                    {
                        byte[] decoded = new byte[aMessageLength];
                        byte[] aBitmaskArray = new byte[4] { aByteArray[anOffset], aByteArray[anOffset + 1], aByteArray[anOffset + 2], aByteArray[anOffset + 3] };
                        anOffset += 4;

                        for (ulong i = 0; i < aMessageLength; ++i)
                            decoded[i] = (byte)(aByteArray[anOffset + (int)i] ^ aBitmaskArray[i % 4]);

                        string text = Encoding.UTF8.GetString(decoded);
                        //Debug.WriteLine($"{text}");
                        addMessageToQueue(text);
                    }
                    else
                    {
                        Debug.WriteLine("mask bit not set");
                    }

                    Debug.WriteLine("");
                }
            }
        }

        public override void SendMessage(string theMessage)
        {
            if (m_TcpClient.GetStream() == null || string.IsNullOrEmpty(theMessage))
            {
                return;
            }

            byte[] aByteArray = Encoding.UTF8.GetBytes(theMessage);

            byte[] aHeader;
            if ( aByteArray.Length < 126)
            {
                aHeader = new byte[]
                {
                    129,
                    (byte)aByteArray.Length,
                };
            }
            else if ( aByteArray.Length <= 65536)
            {
                aHeader = new byte[]
                {
                    129,
                    126,
                    (byte)(aByteArray.Length >> 8),
                    (byte)(aByteArray.Length),
                };
            }
            else
            {
                aHeader = new byte[]
                {
                    129,
                    127,
                    (byte)(aByteArray.Length >> 56),
                    (byte)(aByteArray.Length >> 48),
                    (byte)(aByteArray.Length >> 40),
                    (byte)(aByteArray.Length >> 32),
                    (byte)(aByteArray.Length >> 24),
                    (byte)(aByteArray.Length >> 16),
                    (byte)(aByteArray.Length >> 8),
                    (byte)(aByteArray.Length),
                };
            }

            byte[] aFinalMessage = new byte[aByteArray.Length + aHeader.Length];
            aHeader.CopyTo(aFinalMessage, 0);
            aByteArray.CopyTo(aFinalMessage, aHeader.Length);
            
            m_TcpClient.GetStream().Write(aFinalMessage, 0, aFinalMessage.Length);
        }

        public override void Connect(string theHostname = "")
        {
            m_NetworkThread = new Thread(new ThreadStart(this.Start));
            m_NetworkThread.Start();
        }
    }
}