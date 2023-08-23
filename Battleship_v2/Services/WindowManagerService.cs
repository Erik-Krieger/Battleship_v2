using Battleship_v2.ViewModels;
using System.Threading;

namespace Battleship_v2.Services
{
    public class WindowManagerService
    {
        public static WindowManagerService Instance { get; private set; } = new WindowManagerService();
        private WindowManagerService() { }

        public NavigationViewModel NavigationViewModel { get; private set; }

        public void RegisterNavigationViewModel(NavigationViewModel theViewModel)
        {
            NavigationViewModel = theViewModel;
        }
    }
}
