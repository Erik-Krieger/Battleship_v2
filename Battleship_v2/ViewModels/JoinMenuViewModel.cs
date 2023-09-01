using Battleship_v2.Enemies;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using System;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class JoinMenuViewModel : BaseViewModel
    {
        public string Hostname { get; set; } = "127.0.0.1";

        public ICommand CmdJoin
        {
            get => m_CmdJoin ?? new CommandHandler(() => connect(), () => Hostname != "");
        }
        private ICommand m_CmdJoin;

        public JoinMenuViewModel() { }

        private void connect()
        {
            NetworkService.Instance.JoinServer(Hostname);
            GameManagerService.Instance.SelectDifficulty(Difficulty.Person);
        }

        public void BeginGame()
        {
            //
            ((EnemyPerson)GameManagerService.Instance.Opponent).InjectNetworkPeer(NetworkService.Instance.NetworkPeer);
            //
            ((EnemyPerson)GameManagerService.Instance.Opponent).EventEnabled = !GameManagerService.Instance.YourTurn;
            // Change the View to display the main Game Screen.
            WindowManagerService.ChangeView(new GameViewModel());
        }
    }
}
