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

        public static void ChangeView(BaseViewModel theViewModel)
        {
            if (theViewModel is null)
            {
                return;
            }

            // We do this to make calls to ChangeView more concise.
            WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = theViewModel;
        }

        public static void OpenMainMenu()
        {
            // Make Sure the NetworkThread is terminated.
            NetworkService.Instance.Close();
            // Change the view
            ChangeView(new MainMenuViewModel());
        }

        public static void OpenMultiplayerMenu()
        {
            // Make sure the NetworkThread is terminated.
            NetworkService.Instance.Close();
            // Change the View
            ChangeView(new MultiplayerSetupViewModel());
        }
    }
}
