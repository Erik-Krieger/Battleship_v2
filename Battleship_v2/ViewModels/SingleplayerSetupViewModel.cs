using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Battleship_v2.Enemies;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class SingleplayerSetupViewModel
    {
        public SingleplayerSetupViewModel() {}

        public ICommand CmdBegin
        {
            get
            {
                GameManagerService.Instance.SelectDifficulty(0);
                return m_CmdBegin ?? new CommandHandler( () =>
                {
                    WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new GameViewModel();
                } );
            }
        }
        private ICommand m_CmdBegin;

        public int DifficultyIndex { get; set; } = 0;
    }
}
