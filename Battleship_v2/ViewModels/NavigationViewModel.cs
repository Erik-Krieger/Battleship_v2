using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class NavigationViewModel : PropertyChangeHandler
    {
        private object m_SelectedViewModel;

        public object SelectedViewModel
        {
            get => m_SelectedViewModel;
            set => SetProperty(ref m_SelectedViewModel, value);
        }

        public NavigationViewModel() { }
    }
}
