using Battleship_v2.Services;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
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
                        if (aPartsArray.Length == 4)
                        {
                            if (int.TryParse(aPartsArray[1], out int aTurnIndex))
                            {
                                GameManagerService.Instance.SetFirstTurnFromInt(aTurnIndex);

                                // Extracting the Ship List String representations
                                var essr = aPartsArray[2];
                                var yssr = aPartsArray[3];

                                // Convert the String representations to Ship Lists.
                                var enemyShipsUshortList = NetworkService.ConvertStringRepToUshortList(essr);
                                var yourShipsUshortList = NetworkService.ConvertStringRepToUshortList(yssr);

                                // 
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    // Start the game.
                                    ((JoinMenuViewModel)jm).BeginGame(yourShipsUshortList, enemyShipsUshortList);
                                    // Tell the UI to recheck it's can execute states.
                                    CommandManager.InvalidateRequerySuggested();
                                }));
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

        public static string ConvertUshortToStringRep(ushort theShort)
        {
            return $"{(char)(theShort>>8)}{(char)(theShort & 0xFF)}";
        }

        public static ushort ConvertStringRepToUshort(string theStringRep)
        {
            if (string.IsNullOrEmpty(theStringRep))
            {
                throw new ArgumentException("The string cannot be null or empty", nameof(theStringRep));
            }

            if (theStringRep.Length != 2)
            {
                throw new ArgumentException($"The stirng has to have a length of 2. Your input: {theStringRep} with a length of {theStringRep.Length} is invalid", nameof(theStringRep));
            }

            int aBitField = (theStringRep[0] << 8) + theStringRep[1];
            return (ushort)aBitField;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static byte[] EncodeString(string theString)
        {
            var aUtf8EncodedByteArray = Encoding.UTF8.GetBytes(theString);
            var aBase64String = Convert.ToBase64String(aUtf8EncodedByteArray);
            return Encoding.UTF8.GetBytes(aBase64String);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theBytes"></param>
        /// <returns></returns>
        public static string DecodeString(byte[] theBytes)
        {
            var aBase64String = Encoding.UTF8.GetString(theBytes);
            int anIdx = aBase64String.IndexOf('\0');

            if (anIdx != -1)
            {
                aBase64String = aBase64String.Substring(0, anIdx);
            }

            var aUtf8EncodedByteArray = Convert.FromBase64String(aBase64String);
            return Encoding.UTF8.GetString(aUtf8EncodedByteArray);
        }
    }
}
