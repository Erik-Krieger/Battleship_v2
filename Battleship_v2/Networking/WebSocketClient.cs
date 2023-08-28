using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship_v2.Networking
{
    public class WebSocketClient : NetworkPeer
    {
        private ClientWebSocket webSocket { get; set; } = new ClientWebSocket();

        public WebSocketClient() { }

        private async void receiveMessage()
        {
            byte[] aByteArray = new byte[4096];
            ArraySegment<byte> aBuffer = new ArraySegment<byte>(aByteArray);

            var aCancelToken = new CancellationToken();

            while (true)
            {
                var aResponse = await webSocket.ReceiveAsync(aBuffer, aCancelToken);
                if (aResponse != null)
                {
                    string aMessage = aBuffer.ToString();
                    addMessageToQueue(aMessage);
                }
            }
        }

        private async void sendMessageAsync(string theMessage)
        {
            var aByteArray = Encoding.UTF8.GetBytes(theMessage);
            await webSocket.SendAsync(new System.ArraySegment<byte>(aByteArray), WebSocketMessageType.Text, false, new System.Threading.CancellationToken());
        }

        public override void SendMessage(string theMessage)
        {
            sendMessageAsync(theMessage);
        }
    }
}
