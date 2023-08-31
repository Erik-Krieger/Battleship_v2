using Battleship_v2.Enemies;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using System;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class JoinMenuViewModel : BaseViewModel
    {
        public string Hostname { get; set; } = string.Empty;

        public ICommand CmdJoin
        {
            get => m_CmdJoin ?? new CommandHandler(() => connect(), () => Hostname != "");
        }
        private ICommand m_CmdJoin;

        public ICommand CmdBackToMenu
        {
            get => m_CmdBackToMenu ?? new CommandHandler(() => WindowManagerService.ChangeView(new MainMenuViewModel()));
        }
        private ICommand m_CmdBackToMenu;

        public JoinMenuViewModel() { }

        private void connect()
        {
            NetworkService.Instance.JoinServer(Hostname);
            GameManagerService.Instance.SelectDifficulty(Difficulty.Person);

            //WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new GameViewModel();
        }

        public void BeginGame()
        {
            WindowManagerService.ChangeView(new GameViewModel());
        }
    }
}
