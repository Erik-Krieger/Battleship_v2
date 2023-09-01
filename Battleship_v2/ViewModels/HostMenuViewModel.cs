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

        public ICommand CmdBackToMenu
        {
            get => m_CmdBackToMenu ?? new CommandHandler(() => backToMenu());
        }
        private ICommand m_CmdBackToMenu;

        public HostMenuViewModel()
        {
            NetworkService.Instance.OpenServer();
            GameManagerService.Instance.SelectDifficulty(Difficulty.Person);

            ((EnemyPerson)GameManagerService.Instance.Opponent).InjectNetworkPeer(NetworkService.Instance.NetworkPeer);
        }

        private void startGame()
        {
            var aTurn = GameManagerService.Instance.SetFirstTurnRandom();
            NetworkService.Instance.NetworkPeer.SendMessage($"start-game,{(int)aTurn}");
            WindowManagerService.ChangeView(new GameViewModel());
        }

        private void backToMenu()
        {
            WindowManagerService.ChangeView(new MultiplayerSetupViewModel());
            NetworkService.Instance.Close();
        }
    }
}
