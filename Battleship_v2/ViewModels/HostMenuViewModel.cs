using Battleship_v2.Enemies;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class HostMenuViewModel : BaseViewModel
    {
        public ICommand CmdBegin
        {
            get => m_CmdBegin ?? new CommandHandler(() => WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new GameViewModel(), () => NetworkService.Instance.NetworkPeer.PeerConnected);
        }
        private ICommand m_CmdBegin;

        public ICommand CmdBackToMenu
        {
            get => m_CmdBackToMenu ?? new CommandHandler(() => WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new MultiplayerSetupViewModel());
        }
        private ICommand m_CmdBackToMenu;

        public HostMenuViewModel()
        {
            NetworkService.Instance.OpenServer();
            GameManagerService.Instance.SelectDifficulty(Difficulty.Person);

            ((EnemyPerson)GameManagerService.Instance.Opponent).InjectNetworkPeer(NetworkService.Instance.NetworkPeer);
        }
    }
}
