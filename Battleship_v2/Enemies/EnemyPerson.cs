using Battleship_v2.Networking;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using System;
using System.Windows;

namespace Battleship_v2.Enemies
{
    public class EnemyPerson : Enemy
    {
        private bool eventEnabled = false;

        public EnemyPerson() { }

        public void InjectNetworkPeer(NetworkPeer theNetworkPeer)
        {
            theNetworkPeer.OnMessage += MakeMoveOnMessage;
        }

        public override Position NextMove()
        {
            var np = NetworkService.Instance.NetworkPeer;

            lock (np.MessageQueue)
            {
                if (np.GetMessage(out string aMessage))
                {
                    return parsePositionString(aMessage);
                }

                // Enable the event handler.
                eventEnabled = true;

                return null;
            }
        }

        /// <summary>
        /// This Method is called, when a message is received from the websocket.
        /// </summary>
        public void MakeMoveOnMessage()
        {
            // If the Event handler hasn't been enabled, don't do anything
            // This is needed to prevent the opponent to make moves, when it's our turn.
            if (!eventEnabled)
            {
                return;
            }

            var np = NetworkService.Instance.NetworkPeer;

            // Try getting the message from the message Queue.
            // If there is no message exit.
            if (!np.GetMessage(out string aMessage))
            {
                return;
            }

            // Parse the String into a Position.
            var aPosition = parsePositionString(aMessage);

            // Disable the Event handler.
            eventEnabled = false;

            // Play the move, this construct is used to execute the operation on the main Thread.
            // This needs to be done, so that the Converter in the UI is triggered.
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                GameManagerService.Instance.PlayNextMove(aPosition);
            }));
        }

        /// <summary>
        /// Returns a Position instance of the parsed string.
        /// </summary>
        /// <param name="thePositionString">The String to parse.</param>
        /// <returns>The Position the String represents</returns>
        private static Position parsePositionString(string thePositionString)
        {
            if (string.IsNullOrEmpty(thePositionString))
            {
                return new Position();
            }

            var aStringArray = thePositionString.Split(',');

            if (!int.TryParse(aStringArray[0], out int anXPos))
            {
                return new Position();
            }

            if (!int.TryParse(aStringArray[1], out int anYPos))
            {
                return new Position();
            }

            return new Position(anXPos, anYPos);
        }
    }
}
