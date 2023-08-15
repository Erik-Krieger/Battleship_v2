using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class SingleplayerSetupViewModel
    {
        private NavigationViewModel m_NavigationViewModel;

        public SingleplayerSetupViewModel(NavigationViewModel theNavigationViewModel)
        {
            m_NavigationViewModel = theNavigationViewModel;
        }

        public ICommand CmdBegin { get => m_CmdBegin ?? new CommandHandler( () => { m_NavigationViewModel.SelectedViewModel = new GameViewModel(m_NavigationViewModel); } ); }
        private ICommand m_CmdBegin;

        public int DifficultyIndex { get; set; } = 1;
    }
}
