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
    public class MultiplayerSetupViewModel
    {
        public ICommand CmdHost
        {
            get => m_CmdHost ?? new CommandHandler(() => WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new HostMenuViewModel());
        }
        private ICommand m_CmdHost;

        public ICommand CmdJoin
        {
            get => m_CmdJoin ?? new CommandHandler(() => WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new JoinMenuViewModel());
        }
        private ICommand m_CmdJoin;

        public ICommand CmdBackToMenu
        {
            get => m_CmdBackToMenu ?? new CommandHandler(() => WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new MainMenuViewModel());
        }
        private ICommand m_CmdBackToMenu;

        public MultiplayerSetupViewModel() {}
    }
}
