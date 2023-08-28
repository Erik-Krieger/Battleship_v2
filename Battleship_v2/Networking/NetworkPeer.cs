using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship_v2.Networking
{
    public abstract class NetworkPeer
    {
        public const int PORT = 1337;

        public List<string> MessageQueue { get; set; } = new List<string>();

        public NetworkPeer() { }

        public event Action OnMessage;

        public bool GetMessage(out string theMessage)
        {
            theMessage = null;

            lock (MessageQueue)
            {

                if (MessageQueue == null || MessageQueue.Count == 0)
                {
                    return false;
                }

                theMessage = MessageQueue.First();
                MessageQueue.RemoveAt(0);
                return true;
            }
        }

        protected private void addMessageToQueue(string theMessage)
        {
            if (string.IsNullOrEmpty(theMessage))
            {
                return;
            }

            lock (MessageQueue)
            {
                MessageQueue.Add(theMessage);
            }

            OnMessage?.Invoke();
        }

        public abstract void SendMessage(string theMessage);
    }
}
