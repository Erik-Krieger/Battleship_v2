using System.Windows.Input;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class GameOverViewModel
    {
        public object GameStatusPanel { get; set; }

        public ICommand CmdPlayAgain { get => m_CmdPlayAgain ?? new CommandHandler( () => { } ); }
        private ICommand m_CmdPlayAgain;

        public ICommand CmdGoToMenu { get => m_CmdGoToMenu ?? new CommandHandler( () => { } ); }
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
    }
}
