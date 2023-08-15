using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.ViewModels
{
    public class MultiplayerSetupViewModel
    {
        private NavigationViewModel m_NavigationViewModel;

        public MultiplayerSetupViewModel(NavigationViewModel theNavigationViewModel)
        {
            m_NavigationViewModel = theNavigationViewModel;
        }
    }
}
