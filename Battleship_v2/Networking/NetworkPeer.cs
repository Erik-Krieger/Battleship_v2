using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Battleship_v2.Networking
{
    public abstract class NetworkPeer : PropertyChangeHandler
    {
        public const int PORT = 1337;

        protected private Thread m_NetworkThread {  get; set; }

        public bool PeerConnected
        {
            get => m_PeerConnected;
            set
            {
                m_PeerConnected = value;
                NotifyPropertyChanged(nameof(PeerConnected));
            }
        }
        private bool m_PeerConnected = false;

        public List<string> MessageQueue { get; set; } = new List<string>();

        public NetworkPeer() { }

        public delegate void Notify(); // delegate

        public event Notify OnMessage;

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

            Debug.WriteLine(theMessage);

            OnMessage?.Invoke();
        }

        public abstract void SendMessage(string theMessage);

        public abstract void Connect(string theHostname);

        /// <summary>
        /// Terminates the Websocket Thread.
        /// </summary>
        public void Stop()
        {
            try
            {
                m_NetworkThread.Abort();
            }
            catch (ThreadAbortException) { }
        }
    }
}
