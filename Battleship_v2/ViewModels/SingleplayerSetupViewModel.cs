using Battleship_v2.Services;
using Battleship_v2.Utility;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class SingleplayerSetupViewModel : BaseViewModel
    {
        public SingleplayerSetupViewModel() { }

        public ICommand CmdBegin
        {
            get
            {
                GameManagerService.Instance.SelectDifficulty(0);
                return m_CmdBegin ?? new CommandHandler(() => WindowManagerService.ChangeView(new GameViewModel()));
            }
        }
        private ICommand m_CmdBegin;

        public int DifficultyIndex { get; set; } = 0;
    }
}
