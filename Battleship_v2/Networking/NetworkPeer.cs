using Battleship_v2.Services;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace Battleship_v2.Networking
{
    public abstract class NetworkPeer : PropertyChangeHandler
    {
        protected private Thread m_NetworkThread {  get; set; }

        protected private CancellationTokenSource m_CancelToken = new CancellationTokenSource();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theMessage"></param>
        protected private void addMessageToQueue(string theMessage)
        {
            // Cancel processing if the message is null or empty.
            if (string.IsNullOrEmpty(theMessage))
            {
                return;
            }

            // This is a hacky workaround
            if (theMessage.StartsWith("start-game"))
            {
                if (NetworkService.Instance.NetworkPeer is WebSocketClient)
                {
                    var jm = WindowManagerService.Instance.NavigationViewModel.SelectedViewModel;

                    if (jm is JoinMenuViewModel)
                    {
                        string[] aPartsArray = theMessage.Split(',');
                        if (aPartsArray.Length > 1)
                        {
                            if (int.TryParse(aPartsArray[1], out int aTurnIndex))
                            {
                                GameManagerService.Instance.SetFirstTurnFromInt(aTurnIndex);
                                ((JoinMenuViewModel)jm).BeginGame();
                                CommandManager.InvalidateRequerySuggested();
                            }
                        }
                        return;
                    }
                }
            }

            lock (MessageQueue)
            {
                MessageQueue.Add(theMessage);
            }

            Debug.WriteLine(theMessage + "\n");

            OnMessage?.Invoke();
        }

        public abstract void SendMessage(string theMessage);

        public abstract void Connect(string theHostname);

        /// <summary>
        /// Terminates the Websocket Thread.
        /// </summary>
        public virtual void Stop()
        {
            // Cancel the WebSocketConnections
            m_CancelToken.Cancel();

            // Try terminating the NetworkThread.
            try
            {
                m_NetworkThread.Abort();
            }
            catch (ThreadAbortException) { }
        }
    }
}
