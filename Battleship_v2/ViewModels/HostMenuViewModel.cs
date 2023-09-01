using Battleship_v2.Enemies;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class HostMenuViewModel : BaseViewModel
    {
        public ICommand CmdBegin
        {
            get => m_CmdBegin ?? new CommandHandler(() => startGame(), () => NetworkService.Instance.NetworkPeer.PeerConnected);
        }
        private ICommand m_CmdBegin;

        /// <summary>
        /// The default constructor for the HostMenuViewModel
        /// </summary>
        public HostMenuViewModel()
        {
            // Start the WebSocketServer
            NetworkService.Instance.OpenServer();

            // Set the difficulty to person, to tell the game that we're going against another human
            GameManagerService.Instance.SelectDifficulty(Difficulty.Person);

            // Set the NetworkPeer to the WebSocketServer
            ((EnemyPerson)GameManagerService.Instance.Opponent).InjectNetworkPeer(NetworkService.Instance.NetworkPeer);
        }

        private void startGame()
        {
            // let chance decide who begins.
            var aTurn = GameManagerService.Instance.SetFirstTurnRandom();
            // Invert aTurn, because we need to send the opposite value of what we're storing to the client.
            aTurn = aTurn == PlayerType.You ? PlayerType.Enemy : PlayerType.You;

            // Send the start-game message to the connected client and include an integer indicating, who makes the fist turn.
            NetworkService.Instance.NetworkPeer.SendMessage($"start-game,{(int)aTurn}");

            // We have to enable the event, in case our opponent makes the first turn, to activate the event listener.
            // If this is not done Network messages will just be ignored.
            ((EnemyPerson)GameManagerService.Instance.Opponent).EventEnabled = !GameManagerService.Instance.YourTurn;

            // Change the current View to the main game view.
            WindowManagerService.ChangeView(new GameViewModel());
        }
    }
}
