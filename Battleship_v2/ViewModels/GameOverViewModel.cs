using System.Windows.Input;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public sealed class GameOverViewModel : BaseViewModel
    {
        public object GameStatusPanel { get; set; }

        public ICommand CmdPlayAgain { get => m_CmdPlayAgain ?? new CommandHandler( () => playAgain() ); }
        private ICommand m_CmdPlayAgain;

        public ICommand CmdGoToMenu { get => m_CmdGoToMenu ?? new CommandHandler(() => WindowManagerService.ChangeView(new MainMenuViewModel())); }
        private ICommand m_CmdGoToMenu;

        public GameOverViewModel(bool isWinner)
        {
            if ( isWinner )
            {
                GameStatusPanel = new WinnerViewModel();
            }
            else
            {
                GameStatusPanel = new LoserViewModel();
            }
        }

        private void playAgain()
        {
            // This will change
            WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new MainMenuViewModel();
        }
    }
}
